// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockbookWindows.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The StockbookWindow Class that contains all the helper methods for the Windows of the project
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Class
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
     
    using Microsoft.Win32;

    using Model;

    using Newtonsoft.Json;

    using Windows;

    /// <summary>
    /// The class for Stock book 
    /// </summary>
    public class StockbookWindows
    {
        /// <summary>
        /// The absolute path.
        /// </summary>
        private static readonly string SettingsAbsolutePath = Environment.CurrentDirectory + @"\Settings\";

        /// <summary>
        /// The file name.
        /// </summary>
        protected static readonly string ConfigFullPath = SettingsAbsolutePath + @"config.json";

        /// <summary>
        /// The refresh main window.
        /// </summary>
        public static void RefreshMainWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Title == "Main")
                {
                    ((MainMetro)window).InitializeProductsView();
                }
            }
        }

        /// <summary>
        /// The database backup.
        /// </summary>
        /// <param name="saveDialog">
        /// The save Dialog.
        /// </param>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool DatabaseBackup(bool saveDialog = false, string location = "")
        {
            var backupName = "Backup - " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture)
                           + ".stockbook";
            var databaseBackup = new DatabaseBackupModel
                                                     {
                                                         Date = DateTime.Now,
                                                         Products = Product.GetAllProducts(),
                                                         TransactionOrders = TransactionOrder.GetAllTransactions()
                                                     };
            if (saveDialog)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = backupName,
                    Filter = "Stock Book Database Backup | *.stockbook",
                    InitialDirectory = "Desktop"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(databaseBackup));
                }
            }
            else if (!string.IsNullOrWhiteSpace(location))
            {
                Directory.CreateDirectory(location);
                File.WriteAllText(location + backupName, JsonConvert.SerializeObject(databaseBackup));
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The auto backup.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool AutoBackup()
        {
            var config = OpenConfig();
   
            var isBackup = false;
            var tempTicks = DateTime.Now.Ticks - config.LastBackup.Ticks;

            RetainPolicyBalance(config.RetainHistoryCount, config.AutoBackupLocation);

            switch (config.TimeIntervalAutoBackup)
            {
                case "Weekly":
                    if (tempTicks >= TimeSpan.TicksPerDay * 7)
                    {
                        isBackup = true;
                    }
                    break;
                case "Daily":
                    if (tempTicks >= TimeSpan.TicksPerDay)
                    {
                        isBackup = true;
                    }
                    break;
                case "Hourly":
                    if (tempTicks >= TimeSpan.TicksPerHour)
                    {
                        isBackup = true;
                    }
                    break;
            }

            if (isBackup && config.IsAutoBackupOn)
            {
                if (config.IsRetainHistoryOn)
                {
                    RetainPolicyBalance(config.RetainHistoryCount, config.AutoBackupLocation);
                }
                DatabaseBackup(location: config.AutoBackupLocation);
                config.LastBackup = DateTime.Now;
                SaveConfig(config);
            }

            return true;
        }

        /// <summary>
        /// The retain policy balance.
        /// </summary>
        /// <param name="retainHistoryCount">
        /// The retain History Count.
        /// </param>
        /// <param name="autoBackupLocation">
        /// The auto Backup Location.
        /// </param>
        private static void RetainPolicyBalance(int retainHistoryCount, string autoBackupLocation)
        {
            var backupFiles = Directory.GetFiles(autoBackupLocation, "*.stockbook").ToList();
            while (backupFiles.Count >= retainHistoryCount)
            {
                if (File.Exists(backupFiles[0]))
                {
                    try
                    {
                        File.Delete(backupFiles[0]);
                    }
                    catch (IOException e)
                    {
                        Debug.WriteLine(e);
                        return;
                    }
                }
                backupFiles = Directory.GetFiles(autoBackupLocation, "*.stockbook").ToList();
            }
        }

        /// <summary>
        /// The open config.
        /// </summary>
        /// <returns>
        /// The <see cref="Config"/>.
        /// </returns>
        public static Config OpenConfig()
        {
            StockbookWindows.InitializeConfig();

            var fileName = StockbookWindows.ConfigFullPath;
            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        return JsonConvert.DeserializeObject<Config>(s);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// The save config.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="isInitialize">
        /// The is Initialize.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool SaveConfig(Config config, bool isInitialize = false)
        {
            try
            {
                if (File.Exists(ConfigFullPath))
                {
                    if (isInitialize)
                    {
                        return false;
                    }

                    File.Delete(ConfigFullPath);
                }

                using (var sw = File.CreateText(ConfigFullPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(config));
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return false;
        }

        /// <summary>
        /// The initialize config.
        /// </summary>
        public static void InitializeConfig()
        {
                Directory.CreateDirectory(SettingsAbsolutePath);
                var config = new Config
                                 {
                                     LastModified = DateTime.Now,
                                     AutoBackupLocation = SettingsAbsolutePath,
                                     IsAutoBackupOn = true,
                                     IsRetainHistoryOn = true,
                                     RetainHistoryCount = 100,
                                     TimeIntervalAutoBackup = "Daily",
                                     CompanyName = "Change Name of the Company in Settings",
                                     Currency = "USD - $"
                                    };
                 SaveConfig(config, true);
        }

        /// <summary>
        /// The restore backup.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool RestoreBackup()
        {
            var openFileDialog = new OpenFileDialog
                                     { 
                                         Filter = "Stock Book Database Backup | *.stockbook",
                                         InitialDirectory = "Desktop"
                                     };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                { 
                         var readAllText = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                         var databaseBackup = JsonConvert.DeserializeObject<DatabaseBackupModel>(readAllText);
                        foreach (var prod in databaseBackup.Products)
                        {
                            Product.CreateProduct(prod);
                        }

                        foreach (var transaction in databaseBackup.TransactionOrders)
                        {
                            TransactionOrder.CreateTransaction(transaction);
                        }

                         return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return false;
        }
    }
}
