using System;
using System.Collections.Generic;
using System.Linq;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Models
{
    public class DashboardStats
    {
        // عدد كل التطبيقات المثبتة
        public int TotalApps { get; set; }

        // تطبيقات ذات خطورة عالية
        public int HighRiskApps { get; set; }

        // تطبيقات ذات خطورة متوسطة
        public int MediumRiskApps { get; set; }

        // تطبيقات منخفضة الخطورة
        public int LowRiskApps { get; set; }

        // عدد الإعلانات المكتشفة
        public int AdsDetected { get; set; }

        // عدد التطبيقات التي تم تعطيلها
        public int DisabledApps { get; set; }

        // عدد التطبيقات المفحوصة
        public int ScannedApps { get; set; }

        // 🔥 NEW: التطبيقات التي تم تثبيتها مؤخراً (آخر 7 أيام)
        public int RecentApps { get; set; }

        // 🔥 NEW: التطبيقات القديمة (أكثر من سنة)
        public int OldApps { get; set; }

        // نسبة التطبيقات عالية الخطورة
        public double RiskPercentage
        {
            get
            {
                if (TotalApps == 0)
                    return 0;

                return (double)HighRiskApps / TotalApps * 100;
            }
        }

        // نسبة التطبيقات الآمنة
        public double SafePercentage
        {
            get
            {
                if (TotalApps == 0)
                    return 0;

                return (double)LowRiskApps / TotalApps * 100;
            }
        }

        // إعادة تصفير الإحصائيات
        public void Reset()
        {
            TotalApps = 0;
            HighRiskApps = 0;
            MediumRiskApps = 0;
            LowRiskApps = 0;

            AdsDetected = 0;
            DisabledApps = 0;
            ScannedApps = 0;

            RecentApps = 0;
            OldApps = 0;
        }

        // تحديث الإحصائيات من قائمة التطبيقات (AppInfo + AndroidApp)
        public void Calculate(IEnumerable<dynamic> apps)
        {
            Reset();

            if (apps == null)
                return;

            ScannedApps = apps.Count();

            foreach (var app in apps)
            {
                TotalApps++;

                // =========================
                // 🔥 Risk calculation (OLD LOGIC preserved)
                // =========================
                string risk = app.RiskLevel?.ToString()?.ToLower();

                switch (risk)
                {
                    case "high":
                    case "high risk":
                        HighRiskApps++;
                        break;

                    case "medium":
                    case "medium risk":
                        MediumRiskApps++;
                        break;

                    default:
                        LowRiskApps++;
                        break;
                }

                // =========================
                // 🔥 NEW: Install date logic (AppInfo support)
                // =========================
                try
                {
                    DateTime installDate = app.InstallDate;

                    if (installDate != DateTime.MinValue)
                    {
                        if (installDate >= DateTime.Now.AddDays(-7))
                        {
                            RecentApps++;
                        }

                        if (installDate <= DateTime.Now.AddYears(-1))
                        {
                            OldApps++;
                        }
                    }
                }
                catch
                {
                    // ignore if model doesn't have InstallDate
                }
            }
        }
    }
}