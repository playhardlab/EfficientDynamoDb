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

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class ULongDdbConverter : NumberDdbConverter<ulong>, IDictionaryKeyConverter<ulong>, ISetValueConverter<ulong>
    {
        public override ulong Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();

        public override void Write(Utf8JsonWriter writer, string attributeName, ref ulong value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref ulong value) => WriteInlined(writer, ref value);

        public void WritePropertyName(Utf8JsonWriter writer, ref ulong value) => writer.WritePropertyName(value);
        
        public void WriteStringValue(Utf8JsonWriter writer, ref ulong value) => writer.WriteStringValue(value);

        public override ulong Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out ulong value, out _))
                throw new DdbException($"Couldn't parse ulong ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref ulong value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}