using System;
using System.IO;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class BackupService
    {
        public void BackupApps(List<AndroidApp> apps)
        {
            string folder = "Backups";
            Directory.CreateDirectory(folder);

            string file = Path.Combine(folder,
                $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            using StreamWriter sw = new(file);

            foreach (var app in apps)
            {
                sw.WriteLine($"{app.PackageName}|{app.Risk}|{app.RiskLevel}");
            }
        }
    }
}