namespace AndroidCleanerPro.Services
{
    public class LicenseService
    {
        private const string VALID_KEY = "DZ-2026-PRO-KEY";

        public bool Validate(string key)
        {
            return key == VALID_KEY;
        }
    }
}