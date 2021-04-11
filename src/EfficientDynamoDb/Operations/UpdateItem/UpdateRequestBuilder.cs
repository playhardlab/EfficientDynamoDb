using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.UpdateItem
{
    internal sealed class UpdateEntityRequestBuilder<TEntity> : IUpdateEntityRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IUpdateEntityRequestBuilder<TEntity>>.Node => _node;

        IUpdateEntityRequestBuilder<TEntity> ITableBuilder<IUpdateEntityRequestBuilder<TEntity>>.Create(BuilderNode newNode) 
            => new UpdateEntityRequestBuilder<TEntity>(_context, newNode);

        public UpdateEntityRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }
        
        private UpdateEntityRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => ToItemAsync(cancellationToken);

        public async Task<TEntity?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemAsync<TEntity>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UpdateItemEntityResponse<TEntity>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemResponseAsync<TEntity>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }
        
        public IAttributeUpdate<IUpdateEntityRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<IUpdateEntityRequestBuilder<TEntity>, TEntity, TProperty>(this, expression);

        public IUpdateEntityRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IUpdateEntityRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IUpdateEntityRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IUpdateEntityRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IUpdateEntityRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new ConditionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IUpdateEntityRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        public IUpdateDocumentRequestBuilder<TEntity> AsDocument() => new UpdateDocumentRequestBuilder<TEntity>(_context, _node);

        IUpdateEntityRequestBuilder<TEntity> IUpdateItemBuilder<IUpdateEntityRequestBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new UpdateEntityRequestBuilder<TEntity>(_context, new UpdateAttributeNode(update, nodeType, _node));
    }
    
     internal sealed class UpdateDocumentRequestBuilder<TEntity> : IUpdateDocumentRequestBuilder<TEntity> where TEntity : class
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode? _node;

        BuilderNode? ITableBuilder<IUpdateDocumentRequestBuilder<TEntity>>.Node => _node;

        IUpdateDocumentRequestBuilder<TEntity> ITableBuilder<IUpdateDocumentRequestBuilder<TEntity>>.Create(BuilderNode newNode)
            => new UpdateDocumentRequestBuilder<TEntity>(_context, newNode);

        public UpdateDocumentRequestBuilder(DynamoDbContext context)
        {
            _context = context;
        }
        
        internal UpdateDocumentRequestBuilder(DynamoDbContext context, BuilderNode? node)
        {
            _context = context;
            _node = node;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default) => ToItemAsync(cancellationToken);
        
        public async Task<Document?> ToItemAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemAsync<Document>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public async Task<UpdateItemEntityResponse<Document>> ToResponseAsync(CancellationToken cancellationToken = default)
        {
            var classInfo = _context.Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            return await _context.UpdateItemResponseAsync<Document>(classInfo, _node, cancellationToken).ConfigureAwait(false);
        }

        public IAttributeUpdate<IUpdateDocumentRequestBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<IUpdateDocumentRequestBuilder<TEntity>, TEntity, TProperty>(this, expression);

        public IUpdateDocumentRequestBuilder<TEntity> WithReturnValues(ReturnValues returnValues) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new ReturnValuesNode(returnValues, _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithReturnConsumedCapacity(ReturnConsumedCapacity returnConsumedCapacity) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new ReturnConsumedCapacityNode(returnConsumedCapacity, _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithReturnCollectionMetrics(ReturnItemCollectionMetrics returnItemCollectionMetrics) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new ReturnItemCollectionMetricsNode(returnItemCollectionMetrics, _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithCondition(FilterBase condition) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new ConditionNode(condition, _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> filterSetup) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new ConditionNode(filterSetup(Condition.ForEntity<TEntity>()), _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public IUpdateDocumentRequestBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new PartitionKeyNode<TPk>(pk, _node));

        IUpdateDocumentRequestBuilder<TEntity> IUpdateItemBuilder<IUpdateDocumentRequestBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new UpdateDocumentRequestBuilder<TEntity>(_context, new UpdateAttributeNode(update, nodeType, _node));
    }
}