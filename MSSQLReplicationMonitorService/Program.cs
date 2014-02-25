using MSSQLReplicationMonitorService.Framework;
using System;
using System.Linq;
using System.ServiceProcess;

namespace MSSQLReplicationMonitorService
{
    static class Program
    {
        // The main entry point for the windows service application.
        static void Main(string[] args)
        {
            try
            {
                // if install was a command line flag, then run the installer at runtime.
                if (args.Contains("-install", StringComparer.InvariantCultureIgnoreCase))
                {
                    WindowsServiceInstaller.RuntimeInstall<ServiceImplementation>();
                }

                    // if uninstall was a command line flag, run uninstaller at runtime.
                else if (args.Contains("-uninstall", StringComparer.InvariantCultureIgnoreCase))
                {
                    WindowsServiceInstaller.RuntimeUnInstall<ServiceImplementation>();
                }

                    // otherwise, fire up the service as either console or windows service based on UserInteractive property.
                else
                {
                    var implementation = new ServiceImplementation();

                    // if started from console, file explorer, etc, run as console app.
                    if (Environment.UserInteractive)
                    {
                        ConsoleHarness.Run(args, implementation);
                    }

                        // otherwise run as a windows service
                    else
                    {
                        ServiceBase.Run(new WindowsServiceHarness(implementation));
                    }
                }
            }

            catch (Exception ex)
            {
                ConsoleHarness.WriteToConsole(ConsoleColor.Red, "An exception occurred in Main(): {0}", ex);
            }
        }
    }
}
