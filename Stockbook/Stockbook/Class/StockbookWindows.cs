namespace Stockbook.Class
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;

    using Windows;

    using Microsoft.Win32;

    using Model;

    using Newtonsoft.Json;

    /// <summary>
    /// The class for Stock book 
    /// </summary>
    public class StockbookWindows
    {
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
        public static void DatabaseBackup()
        {
            var databaseBackup = new DatabaseBackupModel
                                                     {
                                                         Date = DateTime.Now,
                                                         Products = Product.GetAllProducts(),
                                                         TransactionOrders = TransactionOrder.GetAllTransactions()
                                                     };
            var saveFileDialog = new SaveFileDialog
                                                {
                                                    FileName = "Backup - " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture) + ".stockbook",
                                                    Filter = "Stock Book Database Backup | *.stockbook",
                                                    InitialDirectory = "Desktop"
                                                };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(databaseBackup));
            }
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
