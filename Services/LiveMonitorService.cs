using System;
using System.Timers;

namespace AndroidCleanerPro.Services
{
    public class LiveMonitorService
    {
        private readonly ActivityService _activity = new();
        private System.Timers.Timer timer;

        public event Action<string> OnAppChanged;

        private string lastApp = "";

        public void Start()
        {
            timer = new System.Timers.Timer(2000); // كل 2 ثانية
            timer.Elapsed += CheckApp;
            timer.Start();
        }

        private void CheckApp(object sender, ElapsedEventArgs e)
        {
            var result = _activity.GetCurrentApp();

            if (result.package != lastApp)
            {
                lastApp = result.package;
                OnAppChanged?.Invoke(result.package);
            }
        }

        public void Stop()
        {
            timer?.Stop();
        }
    }
}