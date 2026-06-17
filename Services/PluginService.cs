using System.Collections.Generic;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class PluginService
    {
        public List<Plugin> LoadPlugins()
        {
            return new List<Plugin>
            {
                new Plugin
                {
                    Name = "Fastboot Reboot",
                    Command = "adb reboot bootloader",
                    Description = "Enter fastboot mode"
                },

                new Plugin
                {
                    Name = "Restart Device",
                    Command = "adb reboot",
                    Description = "Reboot Android device"
                },

                new Plugin
                {
                    Name = "Kill Ads",
                    Command = "pm disable-user --user 0 ads",
                    Description = "Disable ads globally"
                }
            };
        }
    }
}