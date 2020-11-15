using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Reader
{
    public static class DdbJsonReader
    {
        private const int DefaultBufferSize = 16 * 1024;
        private const int DefaultAttributesBufferSize = 32;
        
        public static async ValueTask<List<Document>?> ReadAsync(Stream utf8Json)
        {
            var readerState = new JsonReaderState();

            DdbReadStack readStack = default;

            try
            {
                var buffer = ArrayPool<byte>.Shared.Rent(DefaultBufferSize);

                try
                {
                    var bytesInBuffer = 0;

                    while (true)
                    {
                        var isFinalBlock = false;

                        while (true)
                        {
                            var bytesRead = await utf8Json.ReadAsync(new Memory<byte>(buffer, bytesInBuffer, buffer.Length - bytesInBuffer)).ConfigureAwait(false);
                            if (bytesRead == 0)
                            {
                                isFinalBlock = true;
                                break;
                            }

                            bytesInBuffer += bytesRead;

                            if (bytesInBuffer == buffer.Length)
                                break;
                        }

                        ReadCore(ref readerState, isFinalBlock, new ReadOnlySpan<byte>(buffer, 0, bytesInBuffer), ref readStack);

                        var bytesConsumed = (int) readStack.BytesConsumed;
                        bytesInBuffer -= bytesConsumed;

                        if (isFinalBlock)
                            break;

                        // Check if we need to shift or expand the buffer because there wasn't enough data to complete deserialization.
                        if ((uint) bytesInBuffer > ((uint) buffer.Length / 2))
                        {
                            // We have less than half the buffer available, double the buffer size.
                            byte[] dest = ArrayPool<byte>.Shared.Rent((buffer.Length < (int.MaxValue / 2)) ? buffer.Length * 2 : int.MaxValue);

                            // Copy the unprocessed data to the new buffer while shifting the processed bytes.
                            Buffer.BlockCopy(buffer, bytesConsumed, dest, 0, bytesInBuffer);

                            ArrayPool<byte>.Shared.Return(buffer);
                            buffer = dest;
                        }
                        else if (bytesInBuffer != 0)
                        {
                            // Shift the processed bytes to the beginning of buffer to make more room.
                            Buffer.BlockCopy(buffer, bytesConsumed, buffer, 0, bytesInBuffer);
                        }
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            finally
            {
                readStack.Dispose();
            }

            return readStack.Current.Document!["Items"].AsDocumentList();
        }
        
        private static void ReadCore(ref JsonReaderState readerState, bool isFinalBlock, ReadOnlySpan<byte> buffer, ref DdbReadStack readStack)
        {
            var reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);
            readStack.BytesConsumed = 0;
            ReadCore(ref reader, ref readStack);

            readerState = reader.CurrentState;
        }

        private static void ReadCore(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                    {
                        HandleStringValue(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.PropertyName:
                    {
                        HandlePropertyName(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.StartObject:
                    {
                        if (!state.IsLastFrame || state.Current.IsProcessingValue())
                        {
                            // Parse inner object start
                            HandleNestedStartObject(ref state);
                        }
                        else
                        {
                            // Parse root object start
                            HandleRootStartObject(ref state);
                        }
                        break;
                    }
                    case JsonTokenType.EndObject:
                    {
                        HandleEndObject(ref state);
                        break;
                    }
                    case JsonTokenType.StartArray:
                    {
                        HandleStartArray(ref state);
                        break;
                    }
                    case JsonTokenType.EndArray:
                    {
                        HandleEndArray(ref state);
                        break;
                    }
                    case JsonTokenType.Number:
                    {
                        HandleNumberValue(ref reader, ref state);
                        break;
                    }
                }
            }
            
            state.BytesConsumed += reader.BytesConsumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStringValue(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            if (state.Current.KeyName != null)
                return;
            
            ref var prevState = ref state.GetPrevious();

            // Postpone Document creation and rent buffer instead so we can create a document object only when we know exact number of attributes
            // TODO: Consider adding optional hints struct as input for deserialization to allow users specify more accurate buffer size that completely eliminates the need to resize the buffer during parsing
            if (prevState.DocumentBuffer.RentedBuffer == null)
                prevState.DocumentBuffer = new ReusableBuffer<KeyValuePair<string, AttributeValue>>(DefaultAttributesBufferSize);

            switch (state.Current.AttributeType)
            {
                case AttributeType.String:
                    prevState.DocumentBuffer.Add(new KeyValuePair<string, AttributeValue>(prevState.KeyName!, new AttributeValue(new StringAttributeValue(reader.GetString()))));
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNumberValue(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            if (state.IsLastFrame && state.Current.KeyName == "Count")
                state.Current.ItemsLength = reader.GetInt32();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandlePropertyName(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            if (state.ContainsDdbAttributeType())
            {
                state.Current.AttributeType = GetDdbAttributeType(ref reader);
            }
            else
            {
                state.Current.KeyName = reader.GetString();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleRootStartObject(ref DdbReadStack state)
        {
            state.Current.Document = new Document();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNestedStartObject(ref DdbReadStack state)
        {
            state.Push(1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndObject(ref DdbReadStack state)
        {
            if (!state.IsLastFrame)
            {
                var document = state.Current.CreateDocumentFromBuffer();
                
                state.Pop(1);

                if(state.Current.Items != null && document != null)
                    state.Current.Items?.Add(document);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(ref DdbReadStack state)
        {
            var list = new List<Document>(state.Current.ItemsLength);
            state.Current.Document!.Add(state.Current.KeyName!, new AttributeValue(new ListAttributeValue(list)));
            
            state.Push(0);
            state.Current.Items = list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndArray(ref DdbReadStack state)
        {
            state.Pop(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AttributeType GetDdbAttributeType(ref Utf8JsonReader reader)
        {
            // TODO: Consider adding unescaping support
            var propertyName = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            var key = propertyName.Length > 1 ? MemoryMarshal.Read<short>(propertyName) : propertyName[0];

            return AttributeTypesMap.Get(key);
        }
    }
}