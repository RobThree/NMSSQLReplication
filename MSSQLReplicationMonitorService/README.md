MSSQL ReplicationMonitor Service
=================

The MSSQL ReplicationMonitor Service is a lightweight Windows Service to provide MSSQL Replication information over HTTP. It uses [WebAPI](http://www.asp.net/web-api) to provide the HTTP component and is built as a [Debuggable Self-installable Windows Service](http://geekswithblogs.net/BlackRabbitCoder/archive/2011/03/01/c-toolbox-debug-able-self-installable-windows-service-template-redux.aspx) for easy deployment and debugging.

## Installation

First things first. You (probably, **unless** the application / service is ran as a user with administrative privileges) need to add an URL reservation before you can run the application (or service) ny executing the following command as Administrator:

`netsh http add urlacl url=http://+:8090/replmonitor user=DOMAIN\user`

or

`netsh http add urlacl url=http://+:8090/replmonitor user=MACHINENAME\user`

Where `8090` is the port to be used. Make sure to specify the correct `DOMAIN\user` or `MACHINENAME\user`!

Next, edit the config file `MSSQLReplicationMonitorService.exe.config` to specify the `baseurl` and add one or more connectionstrings:

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
      <connectionStrings>
        <add name="serverA" connectionString="Data Source=192.168.0.1;Initial Catalog=distribution;Integrated Security=SSPI;"/>
        <add name="serverB" connectionString="Data Source=10.10.0.123;Initial Catalog=distribution;Integrated Security=SSPI;"/>
      </connectionStrings>
      <appSettings>
        <add key="baseurl" value="http://localhost:8090/replmonitor"/>
      </appSettings>
      <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
      </startup>
    </configuration>
```

Ofcourse you can change the desired port (in this example `8090`) and root (here `/replmonitor`). Make sure this matches the urlacl you added earlier. You can add one or more connectionstrings; you will specify the connectionstrings' `name` in the urls to indicate which server/database you want to use. Make sure the user you're using to run the application / service as has the required permissions on the target databases / servers to execute the stored procedures described in [Programmatically Monitor Replication](http://technet.microsoft.com/en-us/library/ms147874.aspx).

Now you can run the executable on the commandline as `MSSQLReplicationMonitorService.exe`. This will run the application until you enter `Q` to quit. To install the application as a service use `MSSQLReplicationMonitorService.exe -install` and, conversely, you can run `MSSQLReplicationMonitorService.exe -uninstall` to remove the service. Don't forget to change the user and startup mode in `services.msc` to the desired user and `automatic` respectively and to start the service.

## Usage

When the application or service is running you can open a browser and navigate to the `base-url` + `/replmonitor/listpublications/<-connectionstringname->` to test the service. Below are some examples:

1. `http://localhost:8090/replmonitor/listpublications/serverA` Will list the publications for connectionstring `serverA`. An optional `publisher` can be specified to filter a specific publisher (for example `.../listpublications/serverA?publisher=foo`).
2. `http://localhost:8090/replmonitor/listpublishers/serverA` Will list the publishers for connectionstring `serverA`. Optional querystring parameters are `publisher`, `publisherdb`, `publication` and [`publicationtype`](https://github.com/RobThree/NMSSQLReplication/blob/master/NMSSQLReplication/Enums/PublicationType.cs).
3. `http://localhost:8090/replmonitor/listsubscriptions/mssql4?publisher=serverA&publicationtype=transactional` Will list the subscriptions for connectionstring `serverA`. The [`publicationtype`](https://github.com/RobThree/NMSSQLReplication/blob/master/NMSSQLReplication/Enums/PublicationType.cs) parameter is required. Optional querystring parameters are `publisher`, `publisherdb` and `publication`.
4. `http://localhost:8090/replmonitor/listpendingtransactionalcommands/serverA?publisher=serverA&publisherdb=mydb&publication=mydb&subscriber=SERVERX\SQLEXPRESS&subscriberdb=mydb&subscriptiontype=push` Will list information about pending transactional commands. Required parameters are `publisher`, `publisherdb`, `publication`, `subscriber`, `subscriberdb` and [`subscriptiontype`](https://github.com/RobThree/NMSSQLReplication/blob/master/NMSSQLReplication/Enums/SubscriptionType.cs).
5. `http://localhost:8090/replmonitor/listpublicationthresholds/serverA?publisher=serverA&publisherdb=mydb&publication=mydb` Will list publication thresholds. Required parameters are `publisher`, `publisherdb` and `publication`. Optional parameters are [`publicationtype`](https://github.com/RobThree/NMSSQLReplication/blob/master/NMSSQLReplication/Enums/PublicationType.cs) and `thresholdmetricsname`.

The service will return XML or JSON depending on the `Accept` header sent. For example, Firefox uses `text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8` by default so the results will be returned in XML format. When you change the accept-header to `text/html,application/json,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8` a JSON response will be returned.
