NMSSQLReplication
=================

NMSSQLReplication provides a number of extensionmethods on the `SqlConnection` object (**not** `IDbConnection`, since the replication information and methods are SQL-Server specific!) that allow you to easily retrieve information about replication allowing you to build, for example, a [replication status monitor](https://github.com/RobThree/NMSSQLReplication/tree/master/MSSQLReplicationMonitorService). This information is retrieved by executing a number of stored procedures described in [Programmatically Monitor Replication](http://technet.microsoft.com/en-us/library/ms147874.aspx). All you need to do is open a `SqlConnection` (make sure you have the required permissions!) and call the methods provided by NMSSQLReplication

## Current state of this project

Currently the project is geared towards *retrieving* (e.g. reading) (status)information; specifically `Transactional Publication` replication (since I don't have any other replications going on to test on at the moment). Making changes to the replication, like modifying monitor threshold metrics for a publication, is something I'd like to implement in the future. Also `Merge` replication will be supported as soon as I have something to test against.

The following stored procedures are implemented, including all of their parameters and mapping to result objects:

* [sp_replmonitorhelppublisher](http://technet.microsoft.com/en-us/library/ms174423.aspx)
* [sp_replmonitorhelppublication](http://technet.microsoft.com/en-us/library/ms186304.aspx)
* [sp_replmonitorhelpsubscription](http://technet.microsoft.com/en-us/library/ms188073.aspx)
* [sp_replmonitorsubscriptionpendingcmds](http://technet.microsoft.com/en-us/library/ms189452.aspx)
* [sp_replmonitorhelppublicationthresholds](http://technet.microsoft.com/en-us/library/ms189442.aspx)

The code is tested against SQL Server 2012; other versions have not (yet) been tested.

The following stored procedures haven't (yet!) been implemented:

1. To monitor merge changes waiting to be uploaded or downloaded
  * [sp_showpendingchanges](http://technet.microsoft.com/en-us/library/ms186795.aspx)
2. To monitor Merge Agent sessions
  * [sp_replmonitorhelpmergesession](http://technet.microsoft.com/en-us/library/ms187726.aspx)
  * [sp_replmonitorhelpmergesessiondetail](http://technet.microsoft.com/en-us/library/ms186970.aspx)
3. To modify the monitor threshold metrics for a publication
  * [sp_replmonitorchangepublicationthreshold](http://technet.microsoft.com/en-us/library/ms176085.aspx)

## Usage

```c#
using (var connection = new SqlConnection("Data Source=192.168.0.1;Initial Catalog=distribution;Integrated Security=SSPI;"))
{
    connection.Open();

    //List publishers
    var publishers = connection.ListPublishers();
}
````

This returns an `IEnumerable` of type `Publisher`:
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

Similarly you can call the `ListPublications()`, `ListSubscriptions()`, `ListPendingTransactionalCommands()` and `ListPublicationThresholds()` methods which, in turn, will return `IEnumerables` of types `Publication`, `Subscription`, `PendingTransactionalCommands` and `PublicationThreshold` object respectively.

A more complete example, excersizing all (currently implemented) methods, should explain more:

```c#
using NMSSQLReplication;
using System.Configuration;
using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["mssql4"].ConnectionString))
        {
            connection.Open();

            //List all publishers
            var publishers = connection.ListPublishers();
            foreach (var p in publishers)
            {
                //List all publications for publisher p
                var publications = connection.ListPublications(p.Name);
                foreach (var l in publications)
                {
                    //List publication thresholds for publication l
                    var publicationthresholds = connection.ListPublicationThresholds(p.Name, l.PublisherDb, l.Name);

                    //List all subscriptions for publication l
                    var subscriptions = connection.ListSubscriptions(p.Name, filterpublicationtype: l.PublicationType);
                    foreach (var s in subscriptions)
                    {
                        //List all pending transactional commands for subscription s
                        var pendingtransactions = connection.ListPendingTransactionalCommands(p.Name, l.PublisherDb, l.Name, s.Subscriber, s.SubscriberDb, (SubscriptionType)s.Subtype);
                    }
                }
            }
        }
    }
}
````

## Finally...

Feedback and/or help of any kind is most welcome!
