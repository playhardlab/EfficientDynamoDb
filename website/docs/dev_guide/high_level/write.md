---
id: write
title: Writing Data
slug: ../dev-guide/high-level/write
---

DynamoDB provides two primary operations for writing data:

* `PutItem` - Creates a new item, or replaces an old item with a new item.
* `UpdateItem` - Adds or updates specific attributes of an item.
* `DeleteItem` - Deletes an item from the database.

DynamoDB also supports a `BatchWriteItem` operation for executing up to 25 `PutItem` operations in a single request.
It's covered in [batch operations guide](batch.md).

## PutItem

Creates a new item or replaces an old item with a new item.
If an item that has the same primary key as the new item already exists in the specified table, the new item completely replaces the existing item.

```csharp
await ddbContext.PutItemAsync(new UserEntity("John", "Doe"));
```

You can return the item's attribute values in the same operation by setting `ReturnValues` in fluent API.
It might be helpful if you want to know the item's state before or after the save.
`ReturnValues` enum contains many options, but `PutItem` operation only supports `None`, `AllOld`, and `AllNew`.

```csharp
var item = await ddbContext.PutItem()
    .WithItem(new UserEntity("John", "Doe"))
    .WithReturnValues(ReturnValues.AllOld)
    .ToEntityAsync();
```

## UpdateItem

Edits an existing item's attributes or adds a new item to the table if it does not already exist.
You can put, delete, or add attribute values.

`UpdateItem` is a rather complicated operation, so you can only use a fluent API to access it.
The primary key and at least one update operation must be specified in every request.

EfficientDynamoDb provides an easy way to build update expressions.
All you need to do is pass `Expression` referring to the property you want to update to `.On(...)` method of fluent API.
And then follow it with the action you want to perform, e.g. `Assign(...)`.

```csharp
await ddbContext.UpdateItem<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .On(x => x.LastName).Assign("Doe")
    .ExecuteAsync();
```

Please, refer to [UpdateExpression developer guide](update-expression.md) for more details about `UpdateExpression` builder usage and tricks.

You can return the item's attribute values in the same operation by setting `ReturnValues` in fluent API.
It might be helpful if you want to know the item's state before or after the update.

```csharp
var oldValues = await ddbContext.UpdateItem<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .WithReturnValues(ReturnValues.UpdatedOld)
    .ToEntityAsync();
```

## DeleteItem

Deletes a single item in a table by primary key.
You can perform a conditional delete operation that deletes the item if it exists, or if it has an expected attribute value.

To delete an item you just need to pass primary key to the `DeleteItemAsync<T>` method.

```csharp
// If there is only a partition key
await ddbContext.DeleteItemAsync<UserEntity>("partitionKey");

// If your primary key consists of partition and sort key
await ddbContext.DeleteItemAsync<UserEntity>("partitionKey", "sortKey")
```

Unless you specify conditions, the `DeleteItem` is an idempotent operation.
Running it multiple times on the same item or attribute does not result in an error response.

You can use fluent API to add more configurations to delete request.
It might be useful when you want to know if your request deleted an item or it wasn't present in the table at all.

```csharp
// This call will return null if item hasn't been present in the table
var deletedItem = await ddbContext.DeleteItem<MixedEntity>()
    .WithPrimaryKey("partitionKey")
    .WithReturnValues(ReturnValues.AllOld)
    .ToItemAsync();
```

## Conditions

Write operations in DynamoDb support conditions.
EfficientDynamoDb provides the same fluent API for specifying write condition for all three operations.

If condition is not met, operation will throw the `ConditionalCheckFailedException`.

In the following examples we'll use this condition:

```csharp
var condition = Condition<UserEntity>.On(x => x.FirstName).EqualsTo("John");
```

PutItem:

```csharp
await ddbContext.PutItem()
    .WithItem(new UserEntity("John", "Doe"))
    .WithCondition(condition)
    .ExecuteAsync()
```

Update:

```csharp
await ddbContext.UpdateItem<UserEntity>()
    .WithPrimaryKey("partitionKey", "sortKey")
    .On(x => x.FirstName).Assign("John")
    .WithCondition(condition)
    .ExecuteAsync();
```

DeleteItem:

```csharp
await ddbContext.DeleteItem<MixedEntity>()
    .WithPrimaryKey("partitionKey")
    .WithCondition(condition)
    .ExecuteAsync();
```

## Compatibility API

EfficientDynamoDb provides two extension methods, `SaveAsync(...)` and `DeleteAsync(...)`, for making it easier to transition from official AWS .NET SDK.

**It's highly recommended to use native EfficientDynamoDb API for all new features and migrate old code from these compatibility methods as soon as possible.
Their usage may lead to redundant RCU and WCU consumption due to suboptimal execution flow.**

### SaveAsync

`SaveAsync` looks similar to the native `PutItem` calls but in fact it executes `UpdateItem` operations.
It leads to several important differences:

1. It doesn't replace an item completely so if you remove a property from your entity class it won't be deleted from item in DynamoDB.
1. `PutItem`-like behavior affects only top-level properties. E.g., if you delete or add a property to nested object, it will be completely replaced in DynamoDB.

`SaveAsync` uses the [DynamoDBVersion](attributes.md#DynamoDBVersion) attribute for enabling optimistic concurrency.

Example:

```csharp
await ddbContext.SaveAsync(new UserEntity("John", "Doe"));
```

### DeleteAsync

Deletes an item passed as a parameter.

Similar to `SaveAsync` it uses the [DynamoDBVersion](attributes.md#DynamoDBVersion) attribute for enabling optimistic concurrency.
An item will be deleted only if it's version matches the version of parameter object.

```csharp
await ddbContext.DeleteAsync(new UserEntity("John", "Doe"));
```

## Useful links

* API reference
  * [PutItem](../../api_reference/put-item.md)
  * [UpdateItem](../../api_reference/update-item.md)
  * [DeleteItem](../../api_reference/delete-item.md)
* [Condition Builder guide](conditions.md)