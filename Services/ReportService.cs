using System.Text;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class ReportService
    {
        public string GenerateReport(List<AndroidApp> apps)
        {
            StringBuilder sb = new();

            sb.AppendLine("=== Android Cleaner Report ===");
            sb.AppendLine($"Total Apps: {apps.Count}");
            sb.AppendLine("");

            int high = 0, medium = 0, low = 0;

            foreach (var app in apps)
            {
                sb.AppendLine($"{app.PackageName} | Risk: {app.RiskLevel}");

                if (app.Risk >= 50) high++;
                else if (app.Risk >= 20) medium++;
                else low++;
            }

            sb.AppendLine("");
            sb.AppendLine($"HIGH: {high}");
            sb.AppendLine($"MEDIUM: {medium}");
            sb.AppendLine($"LOW: {low}");

            return sb.ToString();
        }
    }
}