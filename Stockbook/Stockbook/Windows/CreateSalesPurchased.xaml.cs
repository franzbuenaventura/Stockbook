// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateSalesPurchased.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The CreateSalesPurchased Class that contains the backend for CreateSalesPurchased Window
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Class;
    using Model;

    /// <summary>
    /// Interaction logic for CreateSalesPurchased
    /// </summary>
    public partial class CreateSalesPurchased
    {
        /// <summary>
        /// The transaction list that are inputted by user
        /// </summary>
        private readonly List<Transaction> transList = new List<Transaction>();

        /// <summary>
        /// The transaction type of the window
        /// </summary>
        private readonly string transType;

        /// <summary>
        /// The principal filter of the window
        /// </summary>
        private string principalFilter = "All Principal";

        /// <summary>
        /// The category filter of the window
        /// </summary>
        private string categoryFilter = "All Category";

        /// <summary>
        /// The location filter of the window
        /// </summary>
        private string locationFilter = "All Location";

        /// <summary>
        /// The name filter of the window
        /// </summary>
        private string nameFilter = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSalesPurchased"/> class and initialize transaction filters
        /// </summary>
        /// <param name="transType">
        /// The type of transaction (Sales or Purchased)
        /// </param>
        public CreateSalesPurchased(string transType)
        {
            this.InitializeComponent();
            this.transType = transType;
            this.Title.Text = "Add " + transType;
            this.InitializeTransactionFilter(this.locationFilter, this.principalFilter, this.categoryFilter);

            foreach (var dc in this.dataGrid.Columns)
            {
                if (dc.Header.ToString() == "Case" || dc.Header.ToString() == "Pack" || dc.Header.ToString() == "Piece" || dc.Header.ToString() == "Product")
                {
                    this.dataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }
        }

        /// <summary>
        /// The method will initialize the data grid by the current transaction list
        /// </summary>
        private void InitializeDataGrid()
        {
            this.dataGrid.Items.Clear();
            foreach (var trans in this.transList)
            {
                this.dataGrid.Items.Add(trans);
            }
        }

        /// <summary>
        /// The submit create click event will add products for the transaction
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void SubmitCreateClick(object sender, RoutedEventArgs e)
        {
            var prod =
                Product.GetAllProducts()
                    .FirstOrDefault(
                        q =>
                            q.Name == this.NameInput.Text);
            decimal Case = 1, pack = 1, piece = 1;
            if (decimal.TryParse(this.CaseInput.Text.Trim(), out Case) && decimal.TryParse(this.PackInput.Text.Trim(), out pack)
                && decimal.TryParse(this.PieceInput.Text.Trim(), out piece) && prod != null
                && !this.transList.Exists(q => q.Product.Id == prod.Id))
            {
                var trans = new Transaction
                {
                    Product = prod,
                    CaseTransact = Case,
                    PackTransact = pack,
                    PieceTransact = piece
                };
                this.transList.Add(trans);
                this.CaseInput.Text = "0";
                this.PackInput.Text = "0";
                this.PieceInput.Text = "0";
                this.NameInput.Text = string.Empty;
                this.InitializeDataGrid();
            }
            else
            {
                string errorMessage = string.Empty;
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
                else if (prod == null)
                {
                    errorMessage = "No product was chosen";
                }
                else if (this.transList.Exists(q => q.Product.Id == prod.Id))
                {
                    errorMessage = "The product was added already";
                }
                 
                MessageBox.Show(
                    "Error: " + errorMessage,
                    "Error in Creating Transaction",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// The method will initialize the transaction filter on the current window
        /// </summary>
        /// <param name="newValueLocation">
        /// The new value location if user inputted
        /// </param>
        /// <param name="newValuePrincipal">
        /// The new value principal if user inputted
        /// </param>
        /// <param name="newValueCategory">
        /// The new value category if user inputted
        /// </param>
        /// <param name="newValueName">
        /// The new value name if user inputted
        /// </param>
        private void InitializeTransactionFilter(
            string newValueLocation = "",
            string newValuePrincipal = "",
            string newValueCategory = "",
            string newValueName = "")
        {
            var listProducts = Product.GetAllProducts(); 

            this.LocationInput.ItemsSource = null;
            this.LocationInput.Items.Clear();
            this.LocationInput.Items.Add("All Location");
            this.PrincipalInput.ItemsSource = null;
            this.PrincipalInput.Items.Clear();
            this.PrincipalInput.Items.Add("All Principal");
            this.CategoryInput.ItemsSource = null;
            this.CategoryInput.Items.Clear();
            this.CategoryInput.Items.Add("All Category");
            this.NameInput.ItemsSource = null;
            this.NameInput.Items.Clear();


            foreach (var prod in listProducts.Select(q => q.Location).Distinct())
            {
                this.LocationInput.Items.Add(prod);
            }

            if (this.locationFilter != newValueLocation)
            {
                this.principalFilter = "All Principal";
                newValuePrincipal = this.principalFilter;
                this.categoryFilter = "All Category";
                newValueCategory = this.categoryFilter;
            }

            if (this.principalFilter != newValuePrincipal)
            {
                this.categoryFilter = "All Category";
                newValueCategory = this.categoryFilter;
            }

            if (newValueLocation != "All Location")
            {
                listProducts = listProducts.Where(q => q.Location == newValueLocation).ToList();
            }


            foreach (var prod in listProducts.Select(q => q.Principal).Distinct())
            {
                this.PrincipalInput.Items.Add(prod);
            }

            if (newValuePrincipal != "All Principal")
            {
                listProducts = listProducts.Where(q => q.Principal == newValuePrincipal).ToList();
            }

            foreach (var prod in listProducts.Select(q => q.Category).Distinct())
            {
                this.CategoryInput.Items.Add(prod);
            }

            if (newValueCategory != "All Category")
            {
                listProducts = listProducts.Where(q => q.Category == newValueCategory).ToList();
            }


            this.NameInput.ItemsSource = listProducts.Select(q => q.Name).ToList();
            this.LocationInput.SelectedIndex = this.LocationInput.Items.IndexOf(newValueLocation);
            this.PrincipalInput.SelectedIndex = this.PrincipalInput.Items.IndexOf(newValuePrincipal);
            this.CategoryInput.SelectedIndex = this.CategoryInput.Items.IndexOf(newValueCategory);
            this.NameInput.SelectedIndex = this.NameInput.Items.IndexOf(newValueName); 
        }

        /// <summary>
        /// The location input drop down closed event will filter out the products by location
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void LocationInputDropDownClosed(object sender, EventArgs e)
        {
            var value = ((ComboBox)sender).SelectedItem;
            if (value != null)
            {
                this.InitializeTransactionFilter(value.ToString(), this.principalFilter, this.categoryFilter, this.nameFilter);
                this.locationFilter = value.ToString();

            }
        }

        /// <summary>
        /// The principal input drop down closed event will filter out the products by principal
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void PrincipalInputDropDownClosed(object sender, EventArgs e)
        {
            var value = ((ComboBox)sender).SelectedItem;
            if (value != null)
            {
                this.InitializeTransactionFilter(
                    this.locationFilter,
                    value.ToString(),
                    this.categoryFilter,
                    this.nameFilter);
                this.principalFilter = value.ToString();

            }
        }

        /// <summary>
        /// The category input drop down closed will filter out the products by category
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void CategoryInputDropDownClosed(object sender, EventArgs e)
        {
            var value = ((ComboBox)sender).SelectedItem;
            if (value != null)
            {
                this.InitializeTransactionFilter(this.locationFilter, this.principalFilter, value.ToString(), this.nameFilter);
                this.categoryFilter = value.ToString();
            }
        }

        /// <summary>
        /// The name input drop down closed will filter out the products by name
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void NameInputDropDownClosed(object sender, EventArgs e)
        {
            var value = ((ComboBox)sender).SelectedItem;
            if (value != null)
            {
                this.InitializeTransactionFilter(this.locationFilter, this.principalFilter, this.categoryFilter, value.ToString());
                this.nameFilter = value.ToString();
            }
        }

        /// <summary>
        /// The submit create click event will finalize the transaction and store it in database
        /// </summary>       
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            decimal discount, terms;
            string discountTemp = this.DiscountInput.Text.Replace("%", string.Empty);
            if (!string.IsNullOrWhiteSpace(this.RefIdInput.Text) && !string.IsNullOrWhiteSpace(this.ParticularInput.Text) && !string.IsNullOrWhiteSpace(this.SalesmanInput.Text) && !string.IsNullOrWhiteSpace(this.AddressInput.Text) && decimal.TryParse(discountTemp.Trim(), out discount) && decimal.TryParse(this.TermsInput.Text.Trim(), out terms))
            {
                var order = new TransactionOrder
                {
                    TransactionType = this.transType,
                    DateTransaction = DateTime.Now,
                    RefNo = this.RefIdInput.Text,
                    Particular = this.ParticularInput.Text,
                    Transactions = this.transList,
                    DiscountPercentage = discount,
                    ParticularAddress = this.ParticularInput.Text,
                    SalesmanName = this.SalesmanInput.Text,
                    Terms = terms
                };
                TransactionOrder.CreateTransaction(order);
                foreach (var trans in this.transList)
                {
                    var prod = trans.Product;
                    prod = Product.BalanceCasePackPiece(trans, prod, this.transType);
                    Product.EditProduct(prod);
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "Main")
                    {
                        ((MainMetro)window).InitializeTransView();
                        ((MainMetro)window).InitializeProductsView();
                    }
                }

                this.Close();
            }
            else
            {
            string errorMessage = string.Empty; 
             if (string.IsNullOrWhiteSpace(this.RefIdInput.Text))
            {
                errorMessage = "No value was given in Ref No. Input";
            }
            else if (string.IsNullOrWhiteSpace(this.ParticularInput.Text))
            {
                errorMessage = "No value was given in Particular Input";
            }
            else if (string.IsNullOrWhiteSpace(this.SalesmanInput.Text))
            {
                errorMessage = "No value was given in Salesman Input";
            }
            else if (string.IsNullOrWhiteSpace(this.AddressInput.Text))
            {
                errorMessage = "No value was given in Address Input";
            }
                else if (!decimal.TryParse(discountTemp.Trim(), out discount))
            {
                errorMessage = "Discount Input has invalid characters or no value was given";
            }
            else if (!decimal.TryParse(this.TermsInput.Text.Trim(), out terms))
            {
                errorMessage = "Terms Input has invalid characters or no value was given";
            }

                MessageBox.Show(
                    "Error: " + errorMessage,
                    "Error in Creating Transaction Order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// The delete transaction click would delete a product on transaction
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void DeleteTransClick(object sender, RoutedEventArgs e)
        {
            var trans = ((FrameworkElement)sender).DataContext as Transaction;
            this.transList.Remove(trans);
            this.InitializeDataGrid();
        }

        /// <summary>
        /// The discount input text changed just put a % sign at the end if there is no %
        /// </summary>  
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void DiscountInputTextChanged(object sender, TextChangedEventArgs e)
        {
            var discountPercent = ((TextBox)sender).Text;
            if (!discountPercent.Contains("%"))
            {
                ((TextBox)sender).Text = discountPercent + "%";
            }
        }
    }
}
