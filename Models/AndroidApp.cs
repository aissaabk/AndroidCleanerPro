namespace AndroidCleanerPro.Models
{
    public class AndroidApp
    {
        public bool IsSelected { get; set; }

        public string PackageName { get; set; }

        public int Risk { get; set; }

        public string RiskLevel =>
            Risk >= 80 ? "CRITICAL" :
            Risk >= 50 ? "HIGH" :
            Risk >= 20 ? "MEDIUM" : "LOW";

        public bool IsAdApp => Risk >= 50;
    }
}