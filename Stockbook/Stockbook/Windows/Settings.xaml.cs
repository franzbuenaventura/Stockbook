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
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using Stockbook.Class;

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
            // TODO: Products JSON File
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
            // TODO: Transaction JSON File
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
        }

   
    }
}
