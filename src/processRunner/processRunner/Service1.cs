using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Reflection;

namespace processRunner
{
    public partial class ProcessRunner : ServiceBase
    {
        public ProcessRunner()
        {
            InitializeComponent();
        }

        internal void Start()
        {

            string curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = curDir;


            var startInfo = new ProcessStartInfo("run.cmd");
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            var process = new Process();
            process.StartInfo = startInfo;

            
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            process.Exited += new EventHandler(process_Exited);

            this.runningProcess = process;
            this.runningProcess.Start();
        }

        protected Process runningProcess;

        protected override void OnStart(string[] args)
        {
            this.Start();            
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            File.AppendAllText("run.out", e.Data);
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            File.AppendAllText("run.err", e.Data);
        }

        void process_Exited(object sender, EventArgs e)
        {
            Environment.Exit(this.runningProcess.ExitCode);
        }

        protected override void OnStop()
        {
            if (this.runningProcess != null && !this.runningProcess.HasExited)
            {
                try
                {
                    this.runningProcess.Kill();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
