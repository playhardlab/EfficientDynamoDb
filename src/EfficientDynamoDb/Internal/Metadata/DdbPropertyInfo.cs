using System;
using System.Reflection;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Metadata
{
    internal abstract class DdbPropertyInfo
    {
        public string AttributeName { get; }
        public DynamoDbAttributeType AttributeType { get; }

        public abstract DdbClassInfo RuntimeClassInfo { get; }
        
        public abstract DdbConverter ConverterBase { get; }
        
        public PropertyInfo PropertyInfo { get; }

        public abstract bool IsNull(object obj);
        
        public abstract bool ShouldWrite(object obj);

        public abstract void SetValue(object obj, in AttributeValue attributeValue);

        public abstract void SetDocumentValue(object obj, Document document);

        public abstract void Write(object obj, in DdbWriter ddbWriter);

        public abstract void WriteValue(object obj, in DdbWriter ddbWriter);

        protected DdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbAttributeType attributeType)
        {
            PropertyInfo = propertyInfo;
            AttributeName = attributeName;
            AttributeType = attributeType;
        }

        public abstract bool TryReadAndSetMember(object obj, ref DdbReader reader);
    }

    internal sealed class DdbPropertyInfo<T> : DdbPropertyInfo
    {
        public override DdbClassInfo RuntimeClassInfo { get; }
        
        public DdbConverter<T> Converter { get; }
        
        public Func<object, T> Get { get; }
        
        public Action<object, T> Set { get; }

        public override DdbConverter ConverterBase => Converter;

        public override bool IsNull(object obj) => Get(obj) is null;

        public override bool ShouldWrite(object obj)
        {
            var value = Get(obj);
            return value != null && Converter.ShouldWrite(ref value);
        }

        public DdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbAttributeType attributeType, DdbConverter<T> converter, DynamoDbContextMetadata metadata) : base(propertyInfo, attributeName, attributeType)
        {
            Converter = converter;

            Get = EmitMemberAccessor.CreatePropertyGetter<T>(propertyInfo);
            Set = EmitMemberAccessor.CreatePropertySetter<T>(propertyInfo);

            RuntimeClassInfo = metadata.GetOrAddClassInfo(propertyInfo.PropertyType, converter.GetType());
        }

        public override void SetValue(object obj, in AttributeValue attributeValue)
        {
            var value = Converter.Read(in attributeValue);

            Set!(obj, value);
        }

        public override void SetDocumentValue(object obj, Document document)
        {
            var value = Get(obj);
            if (value is null || !Converter.ShouldWrite(ref value))
                return;
            
            document.Add(AttributeName, Converter.Write(ref value));
        }

        public override void Write(object obj, in DdbWriter ddbWriter)
        {
            var value = Get(obj);
            if (value is null || !Converter.ShouldWrite(ref value))
                return;
            
            ddbWriter.JsonWriter.WritePropertyName(AttributeName);
            Converter.Write(in ddbWriter, ref value);
        }

        public override void WriteValue(object obj, in DdbWriter ddbWriter)
        {
            var value = Get(obj);
            Converter.Write(in ddbWriter, ref value);
        }

        public override bool TryReadAndSetMember(object obj, ref DdbReader reader)
        {
            if (Converter.UseDirectRead)
            {
                Set(obj, Converter.Read(ref reader));
                return true;
            }
            
            if (!Converter.TryRead(ref reader, out var value))
                return false;

            Set(obj, value);
            return true;
        }
    }
}