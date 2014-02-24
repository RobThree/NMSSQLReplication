using NMSSQLReplication;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;

namespace MSSQLReplicationMonitorService
{
    public class ReplicationMonitorController : ApiController
    {
        // GET /monitor/listpublishers
        [HttpGet]
        public IEnumerable<Publisher> ListPublishers(string connectionstringname, string publisher = null)
        {
            using (var c = GetOpenConnection(connectionstringname))
                return c.ListPublishers(publisher);
        }


        // GET /monitor/listpublications
        [HttpGet]
        public IEnumerable<Publication> ListPublications(string connectionstringname, string publisher = null, string publisherdb = null, string publication = null, PublicationType publicationtype = PublicationType.Any)
        {
            using (var c = GetOpenConnection(connectionstringname))
                return c.ListPublications(publisher, publisherdb, publication, publicationtype);
        }

        // GET /monitor/listsubscriptions
        [HttpGet]
        public IEnumerable<Subscription> ListSubscriptions(string connectionstringname, string publisher = null, string publisherdb = null, string publication = null, PublicationType publicationtype = PublicationType.Any, Mode mode = Mode.All, int topnum = 0, bool excludeanonymous = false)
        {
            using (var c = GetOpenConnection(connectionstringname))
                return c.ListSubscriptions(publisher, publisherdb, publication, publicationtype, mode, topnum, excludeanonymous);
        }

        // GET /monitor/listpendingtransactionalcommands
        [HttpGet]
        public IEnumerable<PendingTransactionalCommands> ListPendingTransactionalCommands(string connectionstringname, string publisher = null, string publisherdb = null, string publication = null, string subscriber = null, string subscriberdb = null, SubscriptionType subscriptiontype = SubscriptionType.Pull)
        {
            using (var c = GetOpenConnection(connectionstringname))
                return c.ListPendingTransactionalCommands(publisher, publisherdb, publication, subscriber, subscriberdb, subscriptiontype);
        }

        // GET /monitor/listpublicationthresholds
        [HttpGet]
        public IEnumerable<PublicationThreshold> ListPublicationThresholds(string connectionstringname, string publisher = null, string publisherdb = null, string publication = null, PublicationType? publicationtype = null, string thresholsmetricname = null)
        {
            using (var c = GetOpenConnection(connectionstringname))
                return c.ListPublicationThresholds(publisher, publisherdb, publication, publicationtype, thresholsmetricname);
        }

        private SqlConnection GetOpenConnection(string connectionstringname)
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionstringname].ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
