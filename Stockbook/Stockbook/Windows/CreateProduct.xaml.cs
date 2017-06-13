// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateProduct.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The CreateProduct Class that contains the backend for CreateProduct Window
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Windows
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using MahApps.Metro.Controls;

    using Stockbook.Class;

    /// <summary>
    /// Interaction logic for CreateProduct
    /// </summary>
    public partial class CreateProduct : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProduct"/> class and initialize Location filter
        /// </summary>
        public CreateProduct()
        {
            this.InitializeComponent();

            var listProducts = Product.GetAllProducts();
            foreach (var item in listProducts.Select(q => q.Location).Distinct())
            {
                this.LocationInput.Items.Add(item);
            }
        }

        /// <summary>
        /// The submit create click will add product and store it in database
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void SubmitCreateClick(object sender, RoutedEventArgs e)
        {
            decimal Case, pack, piece, casePacks, packPieces;
            if (decimal.TryParse(this.CaseInput.Text.Trim(), out Case) && decimal.TryParse(this.CaseToPack.Text.Trim(), out casePacks) 
                && decimal.TryParse(this.PackToPiece.Text.Trim(), out packPieces) && decimal.TryParse(this.PackInput.Text.Trim(), out pack) 
                && decimal.TryParse(this.PieceInput.Text.Trim(), out piece) && !string.IsNullOrWhiteSpace(this.LocationInput.Text) 
                && !string.IsNullOrWhiteSpace(this.PrincipalInput.Text) && !string.IsNullOrWhiteSpace(this.CategoryInput.Text) 
                && !string.IsNullOrWhiteSpace(this.NameInput.Text) && !string.IsNullOrWhiteSpace(this.CodeInput.Text))
                {
                var prod = new Product
                {
                    Id = string.Empty,
                    Location = this.LocationInput.Text.Trim(),
                    Category = this.CategoryInput.Text.Trim(),
                    Principal = this.PrincipalInput.Text.Trim(),
                    Name = this.NameInput.Text.Trim(),
                    ProdCode = this.CodeInput.Text.Trim(),
                    CaseValue = Case,
                    PackValue = pack,
                    PieceValue = piece,
                    CaseBalance = 0,
                    PackBalance = 0,
                    PieceBalance = 0,
                    PackToPieces = packPieces,
                    CaseToPacks = casePacks
                };
                Product.CreateProduct(prod);
                StockbookWindows.RefreshMainWindow();
                this.Close();
            }
            else
            {
                var errorMessage = string.Empty;
                if (!decimal.TryParse(this.CaseInput.Text.Trim(), out Case))
                {
                    errorMessage = "Case Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(this.PackInput.Text.Trim(), out pack))
                {
                    errorMessage = "Pack Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(this.PieceInput.Text.Trim(), out piece))
                {
                    errorMessage = "Piece Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(this.PackToPiece.Text.Trim(), out piece))
                {
                    errorMessage = "Pack To Piece Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(this.CaseToPack.Text.Trim(), out piece))
                {
                    errorMessage = "Case To Pack Input has invalid characters or no value was given";
                }
                else if (string.IsNullOrWhiteSpace(this.LocationInput.Text))
                {
                    errorMessage = "No value was given in Location Input";
                }
                else if (string.IsNullOrWhiteSpace(this.PrincipalInput.Text))
                {
                    errorMessage = "No value was given in Principal Input";
                }
                else if (string.IsNullOrWhiteSpace(this.CategoryInput.Text))
                {
                    errorMessage = "No value was given in Category Input";
                }
                else if (string.IsNullOrWhiteSpace(this.NameInput.Text))
                {
                    errorMessage = "No value was given in Name Input";
                }
                else if (string.IsNullOrWhiteSpace(this.CodeInput.Text))
                {
                    errorMessage = "No value was given in Code Input";
                }

                MessageBox.Show(
                    "Error: " + errorMessage,
                    "Error in Creating Transaction",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// The location input drop down closed event will filter out the products by principal.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void LocationInputDropDownClosed(object sender, EventArgs e)
        { 
            if (((ComboBox)sender).SelectedItem != null)
            { 
                    var location = ((ComboBox)sender).SelectedItem.ToString();
                    var listProducts = Product.GetAllProducts();
                this.PrincipalInput.ItemsSource = null;
                this.PrincipalInput.Items.Clear();
                this.CategoryInput.ItemsSource = null;
                this.CategoryInput.Items.Clear();
                this.PrincipalInput.ItemsSource = listProducts.Where(q => q.Location == location).Select(q => q.Principal).Distinct();
             }
        }

        /// <summary>
        /// The principal input drop down closed event will filter out the products by principal.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void PrincipalInputDropDownClosed(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                var principal = ((ComboBox)sender).SelectedItem.ToString();
                var listProducts = Product.GetAllProducts();
                this.CategoryInput.ItemsSource = null;
                this.CategoryInput.Items.Clear();
                this.CategoryInput.ItemsSource = listProducts.Where(q => q.Principal == principal).Select(q => q.Category).Distinct();
            }
        }
    }
}
