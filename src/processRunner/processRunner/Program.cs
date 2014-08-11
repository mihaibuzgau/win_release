using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace processRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                var svc = new ProcessRunner();
                svc.Start();
                Console.ReadKey();
                svc.Stop();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new ProcessRunner() 
			    };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
