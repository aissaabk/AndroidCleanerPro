using System.IO;
using System.Text.Json;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class ExportService
    {
        public void ExportToTxt(DashboardStats stats, string path)
        {
            File.WriteAllText(path,
                $"Total Apps: {stats.TotalApps}\n" +
                $"High Risk: {stats.HighRiskApps}\n" +
                $"Medium Risk: {stats.MediumRiskApps}\n" +
                $"Low Risk: {stats.LowRiskApps}");
        }

        public void ExportToJson(DashboardStats stats, string path)
        {
            File.WriteAllText(path,
                JsonSerializer.Serialize(stats, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }
    }
}