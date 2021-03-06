# The Atomic, Batch and Bulk Operations

RepoDb supports the different set of operations by default. With these operations, you can maximize the different ways of implementation to make your application more performant and efficient.

## Atomic Operations

This operation refers to a single minute execution to accomplish the job. In most cases, if your dataset is small, then an atomic execution is much faster and optimal.

To be specific, if you have created a list of Person and wish to save it into the database.

```csharp
var people = CreatePeople(30);
```

Then you are iterating it like below, embedded with transaction.

```csharp
using (var connection = new SqlConnection("Server=.;Database=TestDB;Integrated Security=SSPI;"))
{
	using (var transaction = connection.EnsureOpen().BeginTransaction())
	{
		people.ForEach(p => connection.Insert(p, transaction: transaction));
		transaction.Commit();
	}
}
```

The operations of like [Insert](https://repodb.net/operation/insert), [Update](https://repodb.net/operation/update), [Delete](https://repodb.net/operation/delete) and [Merge](https://repodb.net/operation/merge) are all atomic.

## Batch Operations

This operation refers to a single execution of multiple command texts. Imagine executing the 10 INSERT statements in one-go. It allows you to control the number of rows to be processed against the database.

By using this operation, you are able to optimize the execution in response to the following situations.

- Network Latency (On-Premise, Cloud)
- Number of Columns
- Kind of Data (Blob, Plain Text, etc)
- Many More...

To be specific, if you have created a list of Person like below and wish to save it into the database.

```csharp
var people = CreatePeople(1000);
```

And if you know that you can maximize the performance by sending 100 rows per batch. Then you do it like below.

```csharp
using (var connection = new SqlConnection("Server=.;Database=TestDB;Integrated Security=SSPI;"))
{
	var rowsInserted = connection.InsertAll(people, batchSize: 100);
}
```

By default, the execution is wrapped within the Transaction to make it more ACID. The operations of like [InsertAll](https://repodb.net/operation/insertall), [UpdateAll](https://repodb.net/operation/updateall) and [MergeAll](https://repodb.net/operation/mergeall) are all packed-executions.

## Bulk Operations

This operation refers to a kind of execution that process all the data at once. This operation is the most optimal operation to be used when processing few thousand of rows (or even millions).

The drawback to this is that, it skips all the necessary checks of the underlying RDBMS data provider like logging, auditing and constraints.

To be specific, if you have created a list of Person like below and wish to save it to the database.

```csharp
var people = CreatePeople(100000);
```

Then you can the mentioned datasets in a bulk it like below.

```csharp
using (var connection = new SqlConnection("Server=.;Database=TestDB;Integrated Security=SSPI;"))
{
	var rowsInserted = connection.BulkInsert(people);
}
```

The operations of like [BulkInsert](https://repodb.net/operation/bulkinsert), [BulkUpdate](https://repodb.net/operation/bulkupdate), [BulkDelete](https://repodb.net/operation/bulkdelete) and [BulkMerge](https://repodb.net/operation/bulkmerge) are all bulk-operations.

## Repository Implementation

To make sure the repository can handle the smallest-to-the-biggest datasets, the mentioned methods above must properly be used.

We highly recommend to you to have your own standards of when to do the Batch operation. The only requirement is to have your magic number as a standard. In our case, we used the range of 30-1000, any number below this should be dealt by Atomic operation and any number above this should be dealt by Bulk operation.

See the sample code below for SaveAll.

```csharp
using (var connection = new SqlConnection("Server=.;Database=TestDB;Integrated Security=SSPI;"))
{
        var count = people.Count();
	using (var transaction = connection.EnsureOpen().BeginTransaction())
	{
                if (count <= 30)
                {
		        people.ForEach(p => connection.Insert(p, transaction: transaction));
		}
                else if (count > 30 && count <= 1000)
                {
		        connection.InsertAll(people, transaction: transaction);
		}
                else
                {
		        connection.BulkInsert(people, transaction: transaction);
		}
                transaction.Commit();
	}
}
```

