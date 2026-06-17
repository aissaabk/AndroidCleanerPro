using AndroidCleanerPro.Models;
using System.Diagnostics;
using System.IO;

namespace AndroidCleanerPro.Services
{
    public class AdbService
    {

        private readonly string adbPath =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Tools",
                "adb.exe"
            );



        // =========================================
        // Check adb.exe
        // =========================================

        private bool CheckAdb()
        {

            if (!File.Exists(adbPath))
            {
                throw new FileNotFoundException(
                    "adb.exe not found:\n" + adbPath
                );
            }


            return true;
        }




        // =========================================
        // Execute ADB command
        // =========================================

        public string Run(string arguments)
        {

            CheckAdb();


            try
            {

                using Process process = new Process();


                process.StartInfo = new ProcessStartInfo
                {
                    FileName = adbPath,

                    Arguments = arguments,

                    RedirectStandardOutput = true,

                    RedirectStandardError = true,

                    UseShellExecute = false,

                    CreateNoWindow = true
                };



                process.Start();



                string output =
                    process.StandardOutput.ReadToEnd();



                string error =
                    process.StandardError.ReadToEnd();



                process.WaitForExit(10000);



                if (!string.IsNullOrWhiteSpace(error))
                {
                    return error;
                }



                return output.Trim();

            }
            catch (Exception ex)
            {

                return
                    "ADB ERROR:\n" +
                    ex.Message;

            }

        }





        // =========================================
        // Execute command on selected device
        // =========================================

        public string RunOnDevice(
            string serial,
            string command)
        {

            if (string.IsNullOrWhiteSpace(serial))
            {
                return "Device serial empty";
            }



            return Run(
                $"-s {serial} {command}"
            );

        }





        // =========================================
        // Get connected devices
        // =========================================

        public List<AndroidDevice> GetDevices()
        {

            var devices =
                new List<AndroidDevice>();


            string result =
                Run("devices");



            if (result.StartsWith("ADB ERROR"))
            {
                return devices;
            }



            foreach (
                string line
                in result.Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries))
            {


                string item =
                    line.Trim();



                // ignore header

                if (item.StartsWith("List"))
                    continue;



                if (!item.Contains("\t"))
                    continue;



                string[] data =
                    item.Split('\t');



                if (data.Length >= 2)
                {

                    devices.Add(
                        new AndroidDevice
                        {
                            Serial = data[0],

                            Status = data[1]
                        }
                    );

                }

            }



            return devices;

        }





        // =========================================
        // Get foreground application
        // =========================================

        public string GetCurrentApp(
            string serial)
        {

            return RunOnDevice(
                serial,
                "shell dumpsys window | grep mCurrentFocus"
            );

        }





        // =========================================
        // Get Android model
        // =========================================

        public string GetModel(
            string serial)
        {

            return RunOnDevice(
                serial,
                "shell getprop ro.product.model"
            );

        }





        // =========================================
        // Install APK
        // =========================================

        public string InstallApk(
            string serial,
            string apkPath)
        {

            return RunOnDevice(
                serial,
                $"install \"{apkPath}\""
            );

        }





        // =========================================
        // Uninstall package
        // =========================================

        public string UninstallPackage(
            string serial,
            string packageName)
        {

            return RunOnDevice(
                serial,
                $"uninstall {packageName}"
            );

        }




        // =========================================
        // Disable package
        // =========================================

        public string DisablePackage(
            string serial,
            string packageName)
        {

            return RunOnDevice(
                serial,
                $"shell pm disable-user --user 0 {packageName}"
            );

        }

        public List<string> GetPackages()
        {
            var result = Run("shell pm list packages -3");

            return result.Split('\n')
                .Where(l => l.Contains("package:"))
                .Select(l => l.Replace("package:", "").Trim())
                .ToList();
        }


    }
}