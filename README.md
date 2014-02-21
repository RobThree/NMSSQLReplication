NMSSQLReplication
=================

NMSSQLReplication provides a number of extensionmethods on the `SqlConnection` object (**not** `IDbConnection`, since the replication information and methods are SQL-Server specific!) that allow you to easily retrieve information about the replication allowing you to build, for example, a replication status monitor. This information is retrieved by executing a number of stored procedures described in [Programmatically Monitor Replication](http://technet.microsoft.com/en-us/library/ms147874.aspx). All you need to do is open a `SqlConnection` (make sure you have the required permissions!) and call the methods provided by NMSSQLReplication

## Current state of this project

Currently the project is geared towards *retrieving* (e.g. reading) (status)information; specifically `Transactional Publication` replication (since I don't have any other replications going on to test on at the moment). Making changes to the replication, like modifying monitor threshold metrics for a publication, is something I'd like to implement in the future. Also `Merge` replication will be supported as soon as I have something to test against.

The following stored procedures are implemented, including all of their parameters and mapping to result objects:

* [sp_replmonitorhelppublisher](http://technet.microsoft.com/en-us/library/ms174423.aspx)
* [sp_replmonitorhelppublication](http://technet.microsoft.com/en-us/library/ms186304.aspx)
* [sp_replmonitorhelpsubscription](http://technet.microsoft.com/en-us/library/ms188073.aspx)
* [sp_replmonitorsubscriptionpendingcmds](http://technet.microsoft.com/en-us/library/ms189452.aspx)
* [sp_replmonitorhelppublicationthresholds](http://technet.microsoft.com/en-us/library/ms189442.aspx)

The code is tested against SQL Server 2012; other versions have not (yet) been tested. Feedback and/or help of any kind is most welcome!

## Usage

```c#
using (var connection = new SqlConnection("Data Source=192.168.0.1;Initial Catalog=distribution;Integrated Security=SSPI;"))
{
    connection.Open();

    //List publishers
    var publishers = connection.ListPublishers();
}
````

This returns a `Publisher` object:
```
Publisher
   Name : SQLSRVR0018,
   DistributionDb : distribution,
   Status : 3 - Idle,
   Warning : 0 - None,
   PublicationCount : 1,
   ReturnStamp : 2014022118003038,
   ReturnStampDateTime : 2014-02-21T18:00:30.38
````

Similarly you can call the `ListPublications()`, `ListSubscriptions()`, `ListPendingTransactionalCommands()` and `ListPublicationThresholds()` methods which, in turn, will return a `Publication`, `Subscription`, `PendingTransactionalCommands` and `PublicationThreshold` object respectively.
