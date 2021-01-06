using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Enums
{
    internal sealed class LongEnumDdbConverter<TEnum> : DdbConverter<TEnum>, IDictionaryKeyConverter<TEnum>, ISetValueConverter<TEnum> where TEnum : struct, Enum
    {
        public LongEnumDdbConverter() : base(true)
        {
        }

        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToLong();

            return Unsafe.As<long, TEnum>(ref value);
        }
        
        public override TEnum Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out long value, out _))
                throw new DdbException($"Couldn't parse long enum ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return Unsafe.As<long, TEnum>(ref value);
        }
        
        public override AttributeValue Write(ref TEnum value) => new NumberAttributeValue(Unsafe.As<TEnum, long>(ref value).ToString());
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref TEnum value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref TEnum value) => WriteInlined(writer, ref value);

        public void WritePropertyName(Utf8JsonWriter writer, ref TEnum value) => writer.WritePropertyName(Unsafe.As<TEnum, long>(ref value));
        
        public void WriteStringValue(Utf8JsonWriter writer, ref TEnum value) => writer.WriteStringValue(Unsafe.As<TEnum, long>(ref value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref TEnum value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, long>(ref value));
            writer.WriteEndObject();
        }
    }
}