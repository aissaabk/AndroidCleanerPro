using System.Windows;

namespace AndroidCleanerPro.Services
{
    public class ThemeService
    {
        public void SetDark()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
        }

        public void SetLight()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
        }
    }
}