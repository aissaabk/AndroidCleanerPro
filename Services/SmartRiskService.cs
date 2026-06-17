using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class SmartRiskService
    {
        public int CalculateRisk(string package)
        {
            int score = 0;

            // Ad SDKs قوية
            if (package.Contains("byte")) score += 50;
            if (package.Contains("applovin")) score += 45;
            if (package.Contains("unity")) score += 40;

            // سلوك مشبوه
            if (package.Contains("ads")) score += 20;
            if (package.Contains("tracker")) score += 15;
            if (package.Contains("analytics")) score += 10;

            // تطبيقات معروفة آمنة (تقليل المخاطر)
            if (package.Contains("google")) score -= 30;
            if (package.Contains("system")) score -= 40;

            return Math.Max(0, score);
        }
    }
}