using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    internal sealed class TransactWriteItemsHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode _node;

        public TransactWriteItemsHighLevelHttpContent(DynamoDbContext context, BuilderNode node) : base("DynamoDB_20120810.TransactWriteItems")
        {
            _context = context;
            _node = node;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            
            DdbExpressionVisitor? visitor = null;
            var writeState = 0;
            var itemsProcessed = false;
            foreach (var node in _node)
            {
                if (node.Type != BuilderNodeType.BatchItems)
                {
                    node.WriteValue(in ddbWriter, ref writeState);
                    continue;
                }

                if (itemsProcessed)
                    continue;

                writer.WritePropertyName("TransactItems");
            
                writer.WriteStartArray();
                
                var itemsNode = (BatchItemsNode<ITransactWriteItemBuilder>) node;
                using var itemsEnumerator = itemsNode.Value.GetEnumerator();

                while (WriteItems(in ddbWriter, ref visitor, itemsEnumerator))
                {
                    if (ddbWriter.ShouldFlush)
                        await ddbWriter.FlushAsync().ConfigureAwait(false);
                }
                
                writer.WriteEndArray();

                itemsProcessed = true;
            }

            writer.WriteEndObject();
        }
        
        private bool WriteItems(in DdbWriter ddbWriter, ref DdbExpressionVisitor? visitor, IEnumerator<ITransactWriteItemBuilder> enumerator)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                while(enumerator.MoveNext())
                {
                    switch (enumerator.Current!.NodeType)
                    {
                        case BuilderNodeType.TransactConditionCheckNode:
                            WriteConditionCheck(in ddbWriter, ref builder, ref visitor, enumerator.Current!);
                            break;
                        case BuilderNodeType.TransactDeleteItemNode:
                            WriteDeleteItem(in ddbWriter, ref builder, ref visitor, enumerator.Current!);
                            break;
                        case BuilderNodeType.TransactPutItemNode:
                            WritePutItem(in ddbWriter, ref builder, ref visitor, enumerator.Current!);
                            break;
                        case BuilderNodeType.TransactUpdateItemNode:
                            WriteUpdateItem(in ddbWriter, ref builder, ref visitor, enumerator.Current!);
                            break;
                    }

                    if (ddbWriter.ShouldFlush)
                        return true;
                }
            }
            finally
            {
                builder.Dispose();
            }

            return false;
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, ITransactWriteItemBuilder item)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Update");
            
            ddbWriter.JsonWriter.WriteStartObject();
            
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
            ddbWriter.WriteUpdateItem(_context.Config.Metadata, ref builder, visitor, classInfo, item.GetNode());

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteDeleteItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, ITransactWriteItemBuilder item)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Delete");
            
            ddbWriter.JsonWriter.WriteStartObject();
            
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            var writeState = 0;
            foreach (var node in item.GetNode())
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase)node).Write(in ddbWriter, classInfo, ref writeState);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }
        
        private void WritePutItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, ITransactWriteItemBuilder item)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Put");
            
            ddbWriter.JsonWriter.WriteStartObject();

            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);
            
            var writeState = 0;
            foreach (var node in item.GetNode())
            {
                switch (node.Type)
                {
                    case BuilderNodeType.Item:
                        if (writeState.IsBitSet(NodeBits.Item))
                            break;
                        
                        var itemNode = ((ItemNode) node);

                        ddbWriter.JsonWriter.WritePropertyName("Item");
                        ddbWriter.WriteEntity(itemNode.EntityClassInfo, itemNode.Value);

                        writeState = writeState.SetBit(NodeBits.Item);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteConditionCheck(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, ITransactWriteItemBuilder item)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("ConditionCheck");
            
            ddbWriter.JsonWriter.WriteStartObject();

            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, classInfo.TableName!);

            var writeState = 0;
            foreach (var node in item.GetNode())
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase)node).Write(in ddbWriter, classInfo, ref writeState);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }
    }
}