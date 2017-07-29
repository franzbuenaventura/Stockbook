// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
// <summary>
//   The Settings Class that contains the backend for Settings Window
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Windows
{
    using System;
    using System.Windows;

    using Class;

    /// <summary>
    /// Interaction logic for Settings
    /// </summary>
    public partial class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            this.InitializeComponent();
            this.InitializeSettings();
        }

        /// <summary>
        /// The initialize settings.
        /// </summary>
        public void InitializeSettings()
        { 
            var config = StockbookWindows.OpenConfig();
            this.LastBackup.Text = config.LastBackup.ToLongDateString();
            this.LocationTextBox.Text = config.AutoBackupLocation;
            this.AutoBackupCheckBox.IsChecked = config.IsAutoBackupOn;
            this.RetainBackupCheckBox.IsChecked = config.IsRetainHistoryOn;
            this.RetainCountTextBox.Text = config.RetainHistoryCount.ToString();
            switch (config.TimeIntervalAutoBackup)
            {
                case "Weekly":
                    this.WeeklyBackupRadioButton.IsChecked = true;
                    break;
                case "Daily":
                    this.DailyBackupRadioButton.IsChecked = true;
                    break;
                case "Hourly":
                    this.HourlyBackupRadioButton.IsChecked = true;
                    break;
            }

            this.ProductCountTextBlock.Text = Product.GetAllProducts().Count.ToString();
            this.TransactionCountTextBlock.Text = TransactionOrder.GetAllTransactions().Count.ToString();

            this.CompanyNameTextBox.Text = config.CompanyName;

            this.CurrencyComboBox.Items.Clear();
            this.CurrencyComboBox.Items.Add("PHP - ₱");
            this.CurrencyComboBox.Items.Add("USD - $");
            this.CurrencyComboBox.Items.Add("YEN - ¥");
            this.CurrencyComboBox.SelectedIndex = this.CurrencyComboBox.Items.IndexOf(config.Currency);
        }

        /// <summary>
        /// The import products click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ImportProductsClick(object sender, RoutedEventArgs e)
        {
            var prod = new Product
                           {
                               Name = "ProductA", Category = "CategoryA", Location = "LocationA", ProdCode = "ProdCodeA", Principal = "PrincipalA",
                               CaseBalance = 15, CaseValue = 100, CaseToPacks = 10,
                               PackBalance = 5, PackValue = 10, PackToPieces = 5,
                               PieceBalance = 2, PieceValue = 2,
                           };
            Product.CreateProduct(prod);
            prod = new Product
                           {
                               Name = "ProductB", Category = "CategoryA", Location = "LocationA", ProdCode = "ProdCodeB", Principal = "PrincipalA",
                               CaseBalance = 15, CaseValue = 200, CaseToPacks = 10,
                               PackBalance = 5, PackValue = 20, PackToPieces = 5,
                               PieceBalance = 2, PieceValue = 4,
                           };
            Product.CreateProduct(prod);
            prod = new Product
                           {
                               Name = "ProductC", Category = "CategoryA", Location = "LocationA", ProdCode = "ProdCodeC", Principal = "PrincipalA",
                               CaseBalance = 30, CaseValue = 200, CaseToPacks = 10,
                               PackBalance = 8, PackValue = 20, PackToPieces = 5,
                               PieceBalance = 4, PieceValue = 4,
                           };
            Product.CreateProduct(prod);
            prod = new Product
                           {
                               Name = "ProductD", Category = "CategoryB", Location = "LocationA", ProdCode = "ProdCodeD", Principal = "PrincipalB",
                               CaseBalance = 30, CaseValue = 200, CaseToPacks = 10,
                               PackBalance = 8, PackValue = 20, PackToPieces = 5,
                               PieceBalance = 4, PieceValue = 4,
                           };
            Product.CreateProduct(prod);
            prod = new Product
                {
                    Name = "ProductE", Category = "CategoryB", Location = "LocationA", ProdCode = "ProdCodeE", Principal = "PrincipalB",
                    CaseBalance = 15, CaseValue = 100, CaseToPacks = 10,
                    PackBalance = 5, PackValue = 10, PackToPieces = 5,
                    PieceBalance = 2, PieceValue = 2,
                };
            Product.CreateProduct(prod);
            prod = new Product
                {
                    Name = "ProductF", Category = "CategoryC", Location = "LocationB", ProdCode = "ProdCodeF", Principal = "PrincipalA",
                    CaseBalance = 30, CaseValue = 200, CaseToPacks = 10,
                    PackBalance = 8, PackValue = 20, PackToPieces = 5,
                    PieceBalance = 4, PieceValue = 4,
                };
            Product.CreateProduct(prod);

        }

        /// <summary>
        /// The import transactions click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ImportTransactionsClick(object sender, RoutedEventArgs e)
        {
            var transactionOrder = new TransactionOrder
                                       {
                                           DateTransaction = DateTime.Now,
                                           DiscountPercentage = 0,
                                           Particular = "",
                                           ParticularAddress = "",
                                           RefNo = "21"
                                           
                                       };
        }

        /// <summary>
        /// The delete transactions click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeleteTransactionsClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all transaction order in database?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var deleteCount = TransactionOrder.DeleteAllTransactions();
                MessageBox.Show(
                    "Successfully Deleted Transaction Orders: " + deleteCount,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            StockbookWindows.RefreshMainWindow();
            InitializeSettings();
        }

        /// <summary>
        /// The delete products click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeleteProductsClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all products in database?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var deleteCount = Product.DeleteAllProducts();
                MessageBox.Show(
                    "Successfully Deleted Products: " + deleteCount,
                    string.Empty,
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
            }
            StockbookWindows.RefreshMainWindow();
            InitializeSettings();
        }


        /// <summary>
        /// The backup database_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BackupDatabaseClick(object sender, RoutedEventArgs e)
        {
            StockbookWindows.DatabaseBackup(true); 
        }

        /// <summary>
        /// The restore database_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RestoreDatabaseClick(object sender, RoutedEventArgs e)
        {
            StockbookWindows.RestoreBackup();
            StockbookWindows.RefreshMainWindow();
            InitializeSettings();
        }

        /// <summary>
        /// The auto backup_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoBackupClick(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// The location text box got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LocationTextBoxGotFocus(object sender, RoutedEventArgs e)
        { 
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                this.LocationTextBox.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// The update settings button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void UpdateSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            var config = StockbookWindows.OpenConfig();
            var isChecked = this.AutoBackupCheckBox.IsChecked;
            if (isChecked != null)
            {
                config.IsAutoBackupOn = isChecked.Value;
            }

            if (!string.IsNullOrWhiteSpace(this.LocationTextBox.Text))
            {
                config.AutoBackupLocation = this.LocationTextBox.Text;
            }

            int retainCount;
            if (!string.IsNullOrWhiteSpace(this.RetainCountTextBox.Text) && int.TryParse(this.RetainCountTextBox.Text, out retainCount))
            {
                config.RetainHistoryCount = retainCount;
            }

            isChecked = this.RetainBackupCheckBox.IsChecked;
            if (isChecked != null)
            {
                config.IsRetainHistoryOn = isChecked.Value;
            }

            if (this.WeeklyBackupRadioButton.IsChecked.Value)
            {
                config.TimeIntervalAutoBackup = "Weekly";
            }
            else if (this.DailyBackupRadioButton.IsChecked.Value)
            {
                config.TimeIntervalAutoBackup = "Daily";
            }
            else if (this.HourlyBackupRadioButton.IsChecked.Value)
            {
                config.TimeIntervalAutoBackup = "Hourly";
            }

            if (!string.IsNullOrWhiteSpace(this.CompanyNameTextBox.Text))
            {
                config.CompanyName = this.CompanyNameTextBox.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.CurrencyComboBox.SelectedValue.ToString()))
            {
                config.Currency = this.CurrencyComboBox.SelectedValue.ToString();
            }

            StockbookWindows.SaveConfig(config);
            this.Close();
        }
    }
}
