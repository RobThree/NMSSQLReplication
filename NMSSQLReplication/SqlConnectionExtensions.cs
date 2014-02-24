using Dapper;
using Dapper.ColumnMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace NMSSQLReplication
{
    /// <summary>
    /// Provides a set of static methods for <see cref="SqlConnection"/> objects abstracting Replication management.
    /// </summary>
    public static class SqlConnectionExtensions
    {
        static SqlConnectionExtensions()
        {
            //Initialize (column)mappings for all resultobjects
            //TODO: this is kinda hairy; we need a better way to get the classes that need to be mapped. Maybe make
            //      them share a common ("empty") baseclass or put them in a separate (sub)namespace?
            ColumnTypeMapper.RegisterForTypes(
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsClass && t.IsPublic && !IsStatic(t) && !typeof(Attribute).Equals(t.BaseType))
                    .ToArray()
                );
        }

        private static bool IsStatic(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// This returns monitoring information for all Publishers using this Distributor.
        /// </summary>
        /// <param name="connection">The connection to run the command on.</param>
        /// <param name="filterpublisher">To limit the result set to a single Publisher, specify filterpublisher.</param>
        /// <returns>This returns monitoring information for all Publishers using this Distributor.</returns>
        /// <remarks>See <a href="http://technet.microsoft.com/en-us/library/ms174423.aspx">MSDN</a></remarks>
        public static IEnumerable<Publisher> ListPublishers(this SqlConnection connection, 
            string filterpublisher = null)
        {
            return connection.Query<Publisher>("sp_replmonitorhelppublisher",
                new
                {
                    publisher = new DbString { Value = filterpublisher, IsAnsi = true, Length = 128 }
                },
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// This returns monitoring information for all publications using this Distributor.
        /// </summary>
        /// <param name="connection">The connection to run the command on.</param>
        /// <param name="filterpublisher">To limit the result set to a single Publisher, specify filterpublisher.</param>
        /// <param name="filterpublisherdb">To limit the result set to a single Publisher Database, specify filterpublisherdb.</param>
        /// <param name="filterpublication">To limit the result set to a single Publication, specify filterpublisher.</param>
        /// <param name="filterpublicationtype">To limit the result set to a single Publication Type, specify filterpublicationtype.</param>
        /// <returns>This returns monitoring information for all publications using this Distributor.</returns>
        /// <remarks>See <a href="http://technet.microsoft.com/en-us/library/ms186304.aspx">MSDN</a></remarks>
        public static IEnumerable<Publication> ListPublications(this SqlConnection connection, 
            string filterpublisher = null, string filterpublisherdb = null, string filterpublication = null, 
            PublicationType filterpublicationtype = PublicationType.Any)
        {
            return connection.Query<Publication>("sp_replmonitorhelppublication",
                new
                {
                    publisher = new DbString { Value = filterpublisher, IsAnsi = true, Length = 128 },
                    publisher_db = new DbString { Value = filterpublisherdb, IsAnsi = true, Length = 128 },
                    publication = new DbString { Value = filterpublication, IsAnsi = true, Length = 128 },
                    publication_type = filterpublicationtype == PublicationType.Any ? null : (int?)filterpublicationtype
                },
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// This returns monitoring information for all subscriptions using this Distributor.
        /// </summary>
        /// <param name="connection">The connection to run the command on.</param>
        /// <param name="filterpublisher">To limit the result set to a single Publisher, specify filterpublisher.</param>
        /// <param name="filterpublisherdb">To limit the result set to a single Publisher Database, specify filterpublisherdb.</param>
        /// <param name="filterpublication">To limit the result set to a single Publication, specify filterpublisher.</param>
        /// <param name="filterpublicationtype">To limit the result set to a single Publication Type, specify filterpublicationtype.</param>
        /// <param name="filtermode">To limit the result set to a specific subset, specify filterpublicationtype.</param>
        /// <param name="topnum">Restricts the result set to only the specified number of subscriptions at the top of the returned data.</param>
        /// <param name="excludeanonymous">Is if anonymous pull subscriptions are excluded from the result set.</param>
        /// <returns>This returns monitoring information for all subscriptions using this Distributor.</returns>
        /// <remarks>See <a href="http://technet.microsoft.com/en-us/library/ms188073.aspx">MSDN</a></remarks>
        public static IEnumerable<Subscription> ListSubscriptions(this SqlConnection connection, 
            string filterpublisher = null, string filterpublisherdb = null, string filterpublication = null, 
            PublicationType filterpublicationtype = PublicationType.Any, Mode filtermode = Mode.All, 
            int topnum = 0, bool excludeanonymous = false)
        {
            return connection.Query<Subscription>("sp_replmonitorhelpsubscription",
                new
                {
                    publisher = new DbString { Value = filterpublisher, IsAnsi = true, Length = 128 },
                    publisher_db = new DbString { Value = filterpublisherdb, IsAnsi = true, Length = 128 },
                    publication = new DbString { Value = filterpublication, IsAnsi = true, Length = 128 },
                    publication_type = filterpublicationtype == PublicationType.Any ? null : (int?)filterpublicationtype,
                    mode = filtermode,
                    topnum = topnum <= 0 ? null : (int?)topnum,
                    exclude_anonymous = excludeanonymous
                },
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// This returns monitoring information for all commands pending for all subscriptions using this Distributor.
        /// </summary>
        /// <param name="connection">The connection to run the command on.</param>
        /// <param name="filterpublisher">To limit the result set to a single Publisher, specify filterpublisher.</param>
        /// <param name="filterpublisherdb">To limit the result set to a single Publisher Database, specify filterpublisherdb.</param>
        /// <param name="filterpublication">To limit the result set to a single Publication, specify filterpublisher.</param>
        /// <param name="filtersubscriber">To limit the result set to a single Subscriber, specify filtersubscriber.</param>
        /// <param name="filtersubscriberdb">The type of subscription to filter.</param>
        /// <param name="filtersubscriptiontype"></param>
        /// <returns>This returns monitoring information for all commands pending for all subscriptions using this Distributor.</returns>
        /// <remarks>See <a href="http://technet.microsoft.com/en-us/library/ms189452.aspx">MSDN</a></remarks>
        public static IEnumerable<PendingTransactionalCommands> ListPendingTransactionalCommands(this SqlConnection connection, 
            string filterpublisher = null, string filterpublisherdb = null, string filterpublication = null, 
            string filtersubscriber = null, string filtersubscriberdb = null, 
            SubscriptionType filtersubscriptiontype = SubscriptionType.Push)
        {
            return connection.Query<PendingTransactionalCommands>("sp_replmonitorsubscriptionpendingcmds",
                new
                {
                    publisher = new DbString { Value = filterpublisher, IsAnsi = true, Length = 128 },
                    publisher_db = new DbString { Value = filterpublisherdb, IsAnsi = true, Length = 128 },
                    publication = new DbString { Value = filterpublication, IsAnsi = true, Length = 128 },
                    subscriber = new DbString { Value = filtersubscriber, IsAnsi = true, Length = 128 },
                    subscriber_db = new DbString { Value = filtersubscriberdb, IsAnsi = true, Length = 128 },
                    subscription_type = filtersubscriptiontype
                },
                commandType: CommandType.StoredProcedure
            );
        }

        /// <summary>
        /// This returns the monitoring thresholds set for all publications using this Distributor. 
        /// </summary>
        /// <param name="connection">The connection to run the command on.</param>
        /// <param name="filterpublisher">To limit the result set to a single Publisher, specify filterpublisher.</param>
        /// <param name="filterpublisherdb">To limit the result set to a single Publisher Database, specify filterpublisherdb.</param>
        /// <param name="filterpublication">To limit the result set to a single Publication, specify filterpublisher.</param>
        /// <param name="filterpublicationtype">To limit the result set to a single Publication Type, specify filterpublicationtype.</param>
        /// <param name="filterthresholdmetricname">To limit the result set to a single Threshold Metric, specify filterthresholdmetricname.</param>
        /// <returns>This returns the monitoring thresholds set for all publications using this Distributor.</returns>
        /// <remarks>See <a href="http://technet.microsoft.com/en-us/library/ms189442.aspx">MSDN</a></remarks>
        public static IEnumerable<PublicationThreshold> ListPublicationThresholds(this SqlConnection connection, 
            string filterpublisher = null, string filterpublisherdb = null, string filterpublication = null, 
            PublicationType? filterpublicationtype = PublicationType.Any, string filterthresholdmetricname = null)
        {
            return connection.Query<PublicationThreshold>("sp_replmonitorhelppublicationthresholds",
                new
                {
                    publisher = new DbString { Value = filterpublisher, IsAnsi = true, Length = 128 },
                    publisher_db = new DbString { Value = filterpublisherdb, IsAnsi = true, Length = 128 },
                    publication = new DbString { Value = filterpublication, IsAnsi = true, Length = 128 },
                    publication_type = filterpublicationtype == PublicationType.Any ? null : (int?)filterpublicationtype,
                    thresholdmetricname = new DbString { Value = filterthresholdmetricname, IsAnsi = true, Length = 128 }
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}