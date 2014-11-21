# ![Logo](https://raw.github.com/RobThree/NMSSQLReplication/master/icon_64.png) NMSSQLReplication

NMSSQLReplication provides a number of extensionmethods on the `SqlConnection` object (**not** `IDbConnection`, since the replication information and methods are SQL-Server specific!) that allow you to easily retrieve information about replication allowing you to build, for example, a [replication status monitor](https://github.com/RobThree/NMSSQLReplication/tree/master/MSSQLReplicationMonitorService). This information is retrieved by executing a number of stored procedures described in [Programmatically Monitor Replication](http://technet.microsoft.com/en-us/library/ms147874.aspx). All you need to do is open a `SqlConnection` (make sure you have the required permissions!) and call the methods provided by NMSSQLReplication. NMSSQLReplication is available as a [NuGet package](https://www.nuget.org/packages/NMSSQLReplication).

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

Another example; this time we're "nesting" all objects into their parents by using inherited classes, [AutoMapper](https://github.com/AutoMapper/AutoMapper) and some LINQ magic. We finally serialize everything using [Json.NET](http://james.newtonking.com/json).

```c#
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NMSSQLReplication;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        //Setup autmapper mapping
        Mapper.CreateMap<Publisher, ExtendedPublisher>()
            .ForMember(m => m.Publications, opt => opt.Ignore());
        Mapper.CreateMap<Publication, ExtendedPublication>()
            .ForMember(m => m.Thresholds, opt => opt.Ignore())
            .ForMember(m => m.Subscriptions, opt => opt.Ignore());
        Mapper.CreateMap<Subscription, ExtendedSubscription>()
            .ForMember(m => m.PendingTransactionalCommands, opt => opt.Ignore());
        Mapper.AssertConfigurationIsValid();

        //Initialize and open connection
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myserver"].ConnectionString))
        {
            connection.Open();

            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            });

            //Use some nested linq magic to retrieve all objects and then use Json.NET to serialize the entire graph into result
            var result = JsonConvert.SerializeObject(
                connection.ListPublishers().Select(p => new ExtendedPublisher(p)
                {
                    Publications = connection.ListPublications(p.Name).Select(l => new ExtendedPublication(l)
                    {
                        Thresholds = connection.ListPublicationThresholds(p.Name, l.PublisherDb, l.Name).ToArray(),
                        Subscriptions = connection.ListSubscriptions(p.Name, filterpublicationtype: l.PublicationType).Select(s => new ExtendedSubscription(s)
                        {
                            PendingTransactionalCommands = connection.ListPendingTransactionalCommands(p.Name, l.PublisherDb, l.Name, s.Subscriber, s.SubscriberDb, (SubscriptionType)s.Subtype).ToArray()
                        }).ToArray()
                    }).ToArray()
                }).ToArray()
            );
        }
    }

    public class ExtendedPublisher : Publisher
    {
        public ExtendedPublisher(Publisher p) { Mapper.Map<Publisher, ExtendedPublisher>(p, this); }

        public Publication[] Publications { get; set; }
    }

    public class ExtendedPublication : Publication
    {
        public ExtendedPublication(Publication p) { Mapper.Map<Publication, ExtendedPublication>(p, this); }

        public PublicationThreshold[] Thresholds { get; set; }
        public Subscription[] Subscriptions { get; set; }
    }

    public class ExtendedSubscription : Subscription
    {
        public ExtendedSubscription(Subscription s) { Mapper.Map<Subscription, ExtendedSubscription>(s, this); }

        public PendingTransactionalCommands[] PendingTransactionalCommands { get; set; }
    }
}
````

## Finally...

Feedback and/or help of any kind is most welcome!

[![Build status](https://ci.appveyor.com/api/projects/status/eoebdei9t4uqsxjt)](https://ci.appveyor.com/project/RobIII/nmssqlreplication) <a href="https://www.nuget.org/packages/NMSSQLReplication/"><img src="http://img.shields.io/nuget/v/NMSSQLReplication.svg?style=flat-square" alt="NuGet version" height="18"></a> <a href="https://www.nuget.org/packages/NMSSQLReplication/"><img src="http://img.shields.io/nuget/dt/NMSSQLReplication.svg?style=flat-square" alt="NuGet version" height="18"></a>
