using AndroidCleanerPro.Models;
using AndroidCleanerPro.Services;
using AndroidCleanerPro.ViewModels;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace AndroidCleanerPro
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel vm = new();

        private readonly LiveMonitorService monitor = new();
        private readonly AdKillerService killer = new();
        private readonly BackupService backup = new();
        private readonly ExportService export = new();
        private readonly AdbService adb = new();
        private readonly ThemeService theme = new();
        private readonly AdLogService adLog = new();
        private readonly AppScannerService appScanner = new();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = vm;

            // 🔥 IMPORTANT: bind ComboBox to ViewModel
            DevicesCombo.ItemsSource = vm.Devices;

            monitor.OnAppChanged += (app) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Title = "Running App : " + app;
                });
            };
        }

        // =====================================================
        // Detect Devices (ADB ONLY SOURCE)
        // =====================================================
        private void DetectDevice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.Devices.Clear();

                var list = adb.GetDevices();

                if (list == null || list.Count == 0)
                {
                    MessageBox.Show("No Android device found");
                    return;
                }

                foreach (var device in list)
                {
                    vm.Devices.Add(device);
                }

                // 🔥 auto select first device
                DevicesCombo.SelectedIndex = vm.Devices.Count > 0 ? 0 : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ADB ERROR: " + ex.Message);
            }
        }

        // =====================================================
        // Get selected device
        // =====================================================
        private string? SelectedDevice()
        {
            if (DevicesCombo.SelectedItem is not AndroidDevice device)
            {
                MessageBox.Show("Please select a device from the list");
                return null;
            }

            return device.Serial;
        }

        // =====================================================
        // Device Information
        // =====================================================
        private void DetectDeviceS_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            string brand = adb.RunOnDevice(serial, "shell getprop ro.product.brand");
            string model = adb.RunOnDevice(serial, "shell getprop ro.product.model");
            string android = adb.RunOnDevice(serial, "shell getprop ro.build.version.release");

            DeviceText.Text =
                $"Brand : {brand}\n" +
                $"Model : {model}\n" +
                $"Android : {android}\n" +
                $"Serial : {serial}";
        }

        // =====================================================
        // Load Apps
        // =====================================================
        private void LoadApps_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            vm.LoadApps(adb, serial);

            AppsGrid.ItemsSource = vm.Apps;
        }

        // =====================================================
        // Current App
        // =====================================================
        private void CurrentApp_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            var app = adb.RunOnDevice(serial,
                "shell dumpsys window | grep mCurrentFocus");

            CurrentAppText.Text = app;
        }

        // =====================================================
        // Scan Ads Logs
        // =====================================================
        private void ScanAds_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            var logs = adLog.GetAdLogs(adb, serial);

            AppsGrid.ItemsSource = logs;

            vm.Stats.AdsDetected = logs.Count;
        }
        //########################################
        // uninstall apps
        //########################################
        private void Uninstall_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            if (AppsGrid.SelectedItem is not AppInfo app)
            {
                MessageBox.Show("Select an app first");
                return;
            }

            var confirm = MessageBox.Show(
                $"Uninstall {app.PackageName} ?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = adb.UninstallPackage(serial, app.PackageName);

            MessageBox.Show(result);

            vm.LoadApps(adb, serial);
            AppsGrid.ItemsSource = vm.Apps;
        }
        // =====================================================
        // Backup Ads
        // =====================================================
        private void BackupAds_Click(object sender, RoutedEventArgs e)
        {
            var ads = killer.GetAdApps();

            backup.BackupApps(ads);

            MessageBox.Show("Backup completed");
        }

        // =====================================================
        // Disable Ads
        // =====================================================
        private void DisableAds_Click(object sender, RoutedEventArgs e)
        {
            var ads = killer.GetAdApps();

            backup.BackupApps(ads);

            killer.DisableAdApps(ads);

            MessageBox.Show("Ads disabled");
        }

        // =====================================================
        // Export TXT
        // =====================================================
        private void ExportTxt_Click(object sender, RoutedEventArgs e)
        {
            export.ExportToTxt(vm.Stats, "report.txt");
        }

        // =====================================================
        // Export JSON
        // =====================================================
        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            export.ExportToJson(vm.Stats, "report.json");
        }

        // =====================================================
        // Monitor
        // =====================================================
        private void StartMonitor_Click(object sender, RoutedEventArgs e)
        {
            monitor.Start();
            MessageBox.Show("Monitoring started");
        }

        // =====================================================
        // Reboot
        // =====================================================
        private void Reboot_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            adb.RunOnDevice(serial, "reboot");
        }

        // =====================================================
        // Fastboot
        // =====================================================
        private void Fastboot_Click(object sender, RoutedEventArgs e)
        {
            var serial = SelectedDevice();
            if (serial == null) return;

            adb.RunOnDevice(serial, "reboot bootloader");
        }

        // =====================================================
        // Theme
        // =====================================================
        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            theme.SetDark();
        }

        private void LightMode_Click(object sender, RoutedEventArgs e)
        {
            theme.SetLight();
        }

        private void OpenFacebook_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.facebook.com/profile.php?id=61590706739243",
                UseShellExecute = true
            });
        }

        private void OpenYouTube_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.youtube.com/@YourChannelName",
                UseShellExecute = true
            });
        }

        private void github_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.github.com/aissaabk/AndroidCleanerPro",
                UseShellExecute = true
            });
        }
        private void dzlearn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://dzlearn.vercel.app",
                UseShellExecute = true
            });
        }
    }
}