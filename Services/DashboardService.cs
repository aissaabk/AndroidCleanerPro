using System.Linq;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class DashboardService
    {
        public DashboardStats BuildStats(System.Collections.Generic.List<AndroidApp> apps)
        {
            return new DashboardStats
            {
                TotalApps = apps.Count,
                HighRiskApps = apps.Count(a => a.Risk >= 70),
                MediumRiskApps = apps.Count(a => a.Risk >= 30 && a.Risk < 70),
                LowRiskApps = apps.Count(a => a.Risk < 30)
            };
        }
    }
}