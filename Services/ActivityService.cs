using System.Text.RegularExpressions;

namespace AndroidCleanerPro.Services
{
    public class ActivityService
    {
        private readonly AdbService _adb = new();

        public (string package, string activity) GetCurrentApp()
        {
            string output = _adb.Run("shell dumpsys window");

            var focusLine = output.Split('\n')
                .FirstOrDefault(l => l.Contains("mCurrentFocus"));

            if (focusLine == null)
                return ("Unknown", "Unknown");

            // استخراج package/activity
            var match = Regex.Match(focusLine, @"\s(\S+)/(\S+)}");

            if (match.Success)
            {
                return (match.Groups[1].Value, match.Groups[2].Value);
            }

            return ("Unknown", "Unknown");
        }
    }
}