// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for Settings.xaml
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
            StockbookWindows.DatabaseBackup();
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
        }
    }
}
