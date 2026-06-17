namespace AndroidCleanerPro.Models
{
    public class AppInfo
    {
        public string PackageName { get; set; } = "";
        public string AppName { get; set; } = "";

        public DateTime InstallDate { get; set; }

        public bool IsSelected { get; set; }

        public string InstallDateText =>
            InstallDate == DateTime.MinValue
                ? "Unknown"
                : InstallDate.ToString("yyyy-MM-dd HH:mm");

        public long InstallTicks => InstallDate.Ticks;

        // 🔥 ADD THIS (fix crash)
        public string RiskLevel { get; set; } = "LOW";

    }
}