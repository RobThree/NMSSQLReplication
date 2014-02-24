using MSSQLReplicationMonitorService.Framework;
using System.Configuration;
using System.ServiceProcess;
using System.Web.Http;
using System.Web.Http.SelfHost;


namespace MSSQLReplicationMonitorService
{
    /// <summary>
    /// The actual implementation of the windows service goes here...
    /// </summary>
    [WindowsService("MSSQLReplicationMonitorService",
        DisplayName = "MSSQLReplicationMonitorService",
        Description = "Exposes MSSQL Replication information API over HTTP",
        EventLogSource = "MSSQLReplicationMonitorService",
        StartMode = ServiceStartMode.Automatic)]
    public class ServiceImplementation : IWindowsService
    {
        private HttpSelfHostServer _server;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        /// <summary>
        /// This method is called when the service gets a request to start.
        /// </summary>
        /// <param name="args">Any command line arguments</param>
        public void OnStart(string[] args)
        {
            var config = new HttpSelfHostConfiguration(ConfigurationManager.AppSettings["baseurl"]);

            config.Routes.MapHttpRoute(
                name: "ReplicationMonitor",
                routeTemplate: "{action}/{connectionstringname}",
                defaults: new { controller = "ReplicationMonitor" }
            );

            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
        }

        /// <summary>
        /// This method is called when the service gets a request to stop.
        /// </summary>
        public void OnStop()
        {
            _server.Dispose();
        }

        /// <summary>
        /// This method is called when a service gets a request to pause, 
        /// but not stop completely.
        /// </summary>
        public void OnPause()
        {
        }

        /// <summary>
        /// This method is called when a service gets a request to resume 
        /// after a pause is issued.
        /// </summary>
        public void OnContinue()
        {
        }

        /// <summary>
        /// This method is called when the machine the service is running on
        /// is being shutdown.
        /// </summary>
        public void OnShutdown()
        {
        }

        /// <summary>
        /// This method is called when a custom command is issued to the service.
        /// </summary>
        /// <param name="command">The command identifier to execute.</param >
        public void OnCustomCommand(int command)
        {
        }
    }
}
