using AndroidCleanerPro.Services;
using System.Collections.Generic;
using System.Linq;

namespace AndroidCleanerPro.Services
{
    public class AdLogService
    {
        public List<string> GetAdLogs(AdbService adb, string serial)
        {
            var result = adb.RunOnDevice(serial, "logcat -d");

            var lines = result.Split('\n');

            var ads = new List<string>();

            foreach (var line in lines)
            {
                if (line.Contains("AdRequest") ||
                    line.Contains("AdView") ||
                    line.Contains("loadAd") ||
                    line.Contains("Ads") ||
                    line.Contains("OpenAd") ||
                    line.Contains("Interstitial") ||
                    line.Contains("Rewarded"))
                {
                    ads.Add(line);
                }
            }

            return ads;
        }
    }
}