using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Stockbook.Class;
using Stockbook.Model;

namespace Stockbook.Windows
{
    /// <summary>
    /// Interaction logic for CreateSalesPurchased.xaml
    /// </summary>
    public partial class CreateSalesPurchased : MetroWindow
    {
        private List<Transaction> transList = new List<Transaction>();
        private readonly string _transType;
        private string _principalFilter = string.Empty;
        private string _categoryFilter = string.Empty;
        private string _locationFilter = string.Empty;
        private string _nameFilter = string.Empty;
        public CreateSalesPurchased(string transType)
        {
            InitializeComponent();
            _transType = transType;
            Title.Text = "Add " + transType;
            InitializeTransactionFilter();

            foreach (var dc in dataGrid.Columns)
            {
                if (dc.Header.ToString() == "Case" || dc.Header.ToString() == "Pack" || dc.Header.ToString() == "Piece" || dc.Header.ToString() == "Product")
                {
                    dataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }
        }

        //private void InitializeSalesPurchased()
        //{
        //    var listProducts = _db.GetAllProducts();
        //    foreach (var item in listProducts.Select(q => q.Principal).Distinct())
        //    {
        //        PrincipalInput.Items.Add(item);
        //    }
        //    foreach (var item in listProducts.Select(q => q.Category).Distinct())
        //    {
        //        CategoryInput.Items.Add(item);
        //    }
        //    foreach (var item in listProducts.Select(q => q.Name).Distinct())
        //    {
        //        NameInput.Items.Add(item);
        //    }
        //    foreach (var item in listProducts.Select(q => q.Location).Distinct())
        //    {
        //        LocationInput.Items.Add(item);
        //    }

        //}
        private void InitializeDataGrid()
        {
            dataGrid.Items.Clear();
            foreach (var trans in transList)
            {
                dataGrid.Items.Add(trans);
            }
        }
        private void SubmitCreate_Click(object sender, RoutedEventArgs e)
        {
            var prod =
                Product.GetAllProducts()
                    .FirstOrDefault(
                        q =>
                            q.Name == NameInput.Text && q.Principal == PrincipalInput.Text &&
                            q.Category == CategoryInput.Text);
            decimal Case = 1, pack =1, piece =1;
            if (decimal.TryParse(CaseInput.Text.Trim(), out Case) && decimal.TryParse(PackInput.Text.Trim(), out pack) && decimal.TryParse(PieceInput.Text.Trim(), out piece) && prod != null && !transList.Exists(q=>q.Product.Id == prod.Id))
            {
                var trans = new Transaction
                {
                    Product = prod,
                    CaseTransact = Case,
                    PackTransact = pack,
                    PieceTransact = piece
                };
                transList.Add(trans);
                CaseInput.Text = "0";
                PackInput.Text = "0";
                PieceInput.Text = "0";
                NameInput.Text = string.Empty;
                InitializeDataGrid();
            }
            else
            {
                string errorMessage = string.Empty;
                if (!decimal.TryParse(CaseInput.Text.Trim(), out Case))
                {
                    errorMessage = "Case Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(PackInput.Text.Trim(), out pack))
                {
                    errorMessage = "Pack Input has invalid characters or no value was given";
                }
                else if (!decimal.TryParse(PieceInput.Text.Trim(), out piece))
                {
                    errorMessage = "Piece Input has invalid characters or no value was given";
                }
                else if (prod == null)
                {
                    errorMessage = "No product was chosen";
                }
                else if (transList.Exists(q => q.Product.Id == prod.Id))
                {
                    errorMessage = "The product was added already";
                }
                //else if (string.IsNullOrWhiteSpace(RefIdInput.Text))
                //{
                //    errorMessage = "No value was given in Ref No. Input";
                //}
                //else if (string.IsNullOrWhiteSpace(ParticularInput.Text))
                //{
                //    errorMessage = "No value was given in Particual Input";
                //}
                MessageBox.Show("Error: " + errorMessage, "Error in Creating Transaction", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
          
        }
        private void InitializeTransactionFilter(string newValueLocation = "", string newValuePrincipal = "", string newValueCategory = "",  string newValueName="")
        {
            var listProducts = Product.GetAllProducts();
            LocationInput.Items.Clear();
            PrincipalInput.Items.Clear();
            CategoryInput.Items.Clear();
            NameInput.Items.Clear();
            foreach (var item in listProducts.Select(q => q.Location).Distinct())
            {
                LocationInput.Items.Add(item);
            }
            if (!String.IsNullOrWhiteSpace(newValueLocation))
            {
                listProducts = listProducts.Where(q => q.Location == newValueLocation).ToList();
                LocationInput.SelectedIndex = LocationInput.Items.IndexOf(newValueLocation);
            } 
            if (!String.IsNullOrWhiteSpace(newValueLocation))
            { 
                foreach (var item in listProducts.Select(q => q.Principal).Distinct())
                {
                    PrincipalInput.Items.Add(item);
                }
                PrincipalInput.SelectedIndex = PrincipalInput.Items.IndexOf(newValuePrincipal);
            }
            if (!String.IsNullOrWhiteSpace(newValuePrincipal))
            {
                listProducts = listProducts.Where(q => q.Principal == newValuePrincipal).ToList();
                foreach (var item in listProducts.Select(q => q.Category).Distinct())
                {
                    CategoryInput.Items.Add(item);
                }
                CategoryInput.SelectedIndex = CategoryInput.Items.IndexOf(newValueCategory);
            }
            if (!String.IsNullOrWhiteSpace(newValueCategory))
            {
                listProducts = listProducts.Where(q => q.Category == newValueCategory).ToList();
                foreach (var item in listProducts.Select(q => q.Name).Distinct())
                {
                    NameInput.Items.Add(item);
                }
                NameInput.SelectedIndex = NameInput.Items.IndexOf(newValueName);
            }
            _locationFilter = newValueLocation;
            _principalFilter = newValuePrincipal;
            _categoryFilter = newValueCategory;
            _nameFilter = newValueName;
        }

        private void LocationInput_DropDownClosed(object sender, EventArgs e)
        {
            var value = (sender as ComboBox).SelectedItem;
            if (value != null)
            {
            InitializeTransactionFilter(value.ToString(), _principalFilter,_categoryFilter,_nameFilter);
            }
        }

        private void PrincipalInput_DropDownClosed(object sender, EventArgs e)
        {
            var value = (sender as ComboBox).SelectedItem;
            if (value != null)
            { InitializeTransactionFilter(_locationFilter, value.ToString(), _categoryFilter, _nameFilter); }
        }

        private void CategoryInput_DropDownClosed(object sender, EventArgs e)
        {
            var value = (sender as ComboBox).SelectedItem;
            if (value != null)
            {
                InitializeTransactionFilter(_locationFilter, _principalFilter, value.ToString(), _nameFilter);
            }
        }

        private void NameInput_DropDownClosed(object sender, EventArgs e)
        {
            var value = (sender as ComboBox).SelectedItem;
            if (value != null)
            {
                InitializeTransactionFilter(_locationFilter, _principalFilter, _categoryFilter, value.ToString());
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            decimal discount, terms;
            string discountTemp = DiscountInput.Text.Replace("%",string.Empty);
            if (!string.IsNullOrWhiteSpace(RefIdInput.Text) && !string.IsNullOrWhiteSpace(ParticularInput.Text) && !string.IsNullOrWhiteSpace(SalesmanInput.Text) && !string.IsNullOrWhiteSpace(AddressInput.Text) && decimal.TryParse(discountTemp.Trim(), out discount) && decimal.TryParse(TermsInput.Text.Trim(), out terms))
            {
                var order = new TransactionOrder
                {
                    TransactionType = _transType,
                    DateTransaction = DateTime.Now,
                    RefNo = RefIdInput.Text,
                    Particular = ParticularInput.Text,
                    Transactions = transList,
                    DiscountPercentage = discount,
                    ParticularAddress = ParticularInput.Text,
                    SalesmanName = SalesmanInput.Text,
                    Terms = terms
                };
                TransactionOrder.CreateTransaction(order);
                foreach (var trans in transList)
                {
                    var prod = trans.Product;
                    prod = Product.BalanceCasePackPiece(trans,prod,_transType);
                    Product.EditProduct(prod);
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "Main")
                    {
                        (window as MainMetro).InitializeTrans();
                        (window as MainMetro).InitializeProducts();
                    }
                }
                Close();
            }
            else
            {
            string errorMessage = string.Empty; 
             if (string.IsNullOrWhiteSpace(RefIdInput.Text))
            {
                errorMessage = "No value was given in Ref No. Input";
            }
            else if (string.IsNullOrWhiteSpace(ParticularInput.Text))
            {
                errorMessage = "No value was given in Particual Input";
            }
            else if (string.IsNullOrWhiteSpace(SalesmanInput.Text))
            {
                errorMessage = "No value was given in Salesman Input";
            }
            else if (string.IsNullOrWhiteSpace(AddressInput.Text))
            {
                errorMessage = "No value was given in Address Input";
            }
                else if (!decimal.TryParse(discountTemp.Trim(), out discount))
            {
                errorMessage = "Discount Input has invalid characters or no value was given";
            }
            else if (!decimal.TryParse(TermsInput.Text.Trim(), out terms))
            {
                errorMessage = "Terms Input has invalid characters or no value was given";
            }
                MessageBox.Show("Error: " + errorMessage, "Error in Creating Transaction Order", MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }

        private void DeleteTrans_Click(object sender, RoutedEventArgs e)
        {
            var trans = ((FrameworkElement)sender).DataContext as Transaction;
            transList.Remove(trans);
            InitializeDataGrid();
        }

        private void DiscountInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var discountPercent = (sender as TextBox).Text;
            if (!discountPercent.Contains("%"))
            {
                (sender as TextBox).Text = discountPercent + "%";
            }
        }
    }
}
