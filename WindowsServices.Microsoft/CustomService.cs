using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices.Microsoft
{
    internal class CustomService : ServiceBase
    {
        private Timer _timer;

        public void Start(string[] args)
        {
            OnStart(args);
        }
        protected override void OnStart(string[] args)
        {
            WriteMessage("Starting..");
            _timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        private void Work(object? state)
        {
            WriteMessage("Working...");
        }

        public void Stop()
        {
            OnStop();
        }
        protected override void OnStop()
        {
            WriteMessage("Stopping..");
        }

        private readonly string? _filename = "c:\\CustomService\\MicrosoftService.txt";
        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            File.AppendAllText(_filename, message + "\n");
        }
    }
}
