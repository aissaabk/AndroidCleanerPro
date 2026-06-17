namespace AndroidCleanerPro.Models
{
    public class AndroidDevice
    {
        public string Serial { get; set; }
        public string Status { get; set; }

        public string Display =>
            $"{Serial} ({Status})";
    }
}