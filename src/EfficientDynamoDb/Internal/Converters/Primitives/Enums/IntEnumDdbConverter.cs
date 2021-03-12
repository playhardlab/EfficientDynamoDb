using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;


namespace EfficientDynamoDb.Internal.Converters.Primitives.Enums
{
    internal sealed class IntEnumDdbConverter<TEnum> : DdbConverter<TEnum>, IDictionaryKeyConverter<TEnum>, ISetValueConverter<TEnum> where TEnum : struct, Enum
    {
        public IntEnumDdbConverter() : base(true)
        {
        }

        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToInt();

            return Unsafe.As<int, TEnum>(ref value);
        }
        
        public override TEnum Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out int value, out _))
                throw new DdbException($"Couldn't parse int enum ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return Unsafe.As<int, TEnum>(ref value);
        }
        
        public override AttributeValue Write(ref TEnum value) => new AttributeValue(new NumberAttributeValue(Unsafe.As<TEnum, int>(ref value).ToString()));

        public override void Write(in DdbWriter writer, ref TEnum value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref TEnum value) => writer.JsonWriter.WritePropertyName(Unsafe.As<TEnum, int>(ref value));

        public void WriteStringValue(in DdbWriter writer, ref TEnum value) => writer.JsonWriter.WriteStringValue(Unsafe.As<TEnum, int>(ref value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref TEnum value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, int>(ref value));
            writer.WriteEndObject();
        }
    }
}