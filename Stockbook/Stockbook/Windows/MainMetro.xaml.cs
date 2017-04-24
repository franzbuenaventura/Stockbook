// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainMetro.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   The main metro.
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
    using Products;

    /// <summary>
    /// The main metro.
    /// </summary>
    public partial class MainMetro
    {
        public MainMetro()
        {
            InitializeComponent();
            InitializeProducts();
            InitializeTrans();
        }
        #region Inventory 
        private string _locationFilter = "All Location";
        private string _principalFilter = "All Principal";
        private string _categoryFilter = "All Category";
        public void InitializeProducts()
        {
            InitializeProductsFilter(_locationFilter,_principalFilter, _categoryFilter);
            foreach (var dc in DataGrid.Columns)
            {
                if (dc.Header.ToString() == "Case Balance" || dc.Header.ToString() == "Pack Balance" || dc.Header.ToString() == "Piece Balance" || dc.Header.ToString() == "Case To Packs" || dc.Header.ToString() == "Packs To Pieces")
                {
                    DataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }
            foreach (var dc in DataGrid1.Columns)
            {
                if (dc.Header.ToString() == "Type" || dc.Header.ToString() == "Ref No." || dc.Header.ToString() == "Particular" || dc.Header.ToString() == "Principal List" || dc.Header.ToString() == "Prod Count" || dc.Header.ToString() == "Add Date")
                {
                    DataGrid1.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }

        }
        private void InitializeProductsFilter(string newValueLocation = "",string newValuePrincipal = "", string newValueCategory = "")
        {
            var listProducts = Product.GetAllProducts();
            LocationFilter.ItemsSource = null;
            LocationFilter.Items.Clear();
            LocationFilter.Items.Add("All Location");
            PrincipalFilter.ItemsSource = null;
            PrincipalFilter.Items.Clear();
            PrincipalFilter.Items.Add("All Principal");
            CategoryFilter.ItemsSource = null;
            CategoryFilter.Items.Clear();
            CategoryFilter.Items.Add("All Category");
            DataGrid.CommitEdit();
            foreach (var prod in listProducts.Select(q => q.Location).Distinct())
            {
                LocationFilter.Items.Add(prod);
            }
            if (_locationFilter != newValueLocation)
            {
                _principalFilter = "All Principal";
                newValuePrincipal = _principalFilter;
                _categoryFilter = "All Category";
                newValueCategory = _categoryFilter;
            }
            if (_principalFilter != newValuePrincipal)
            {
                _categoryFilter = "All Category";
                newValueCategory = _categoryFilter;
            }

            if (newValueLocation != "All Location")
            {
                listProducts = listProducts.Where(q => q.Location == newValueLocation).ToList();
            }
            if (newValuePrincipal != "All Principal")
            {
                listProducts = listProducts.Where(q => q.Principal == newValuePrincipal).ToList();
            } 
            if (newValueCategory != "All Category")
            {
                listProducts = listProducts.Where(q => q.Category== newValueCategory).ToList();
            }
            foreach (var prod in listProducts.Select(q => q.Principal).Distinct())
            {
                PrincipalFilter.Items.Add(prod);
            }
            foreach (var prod in listProducts.Select(q => q.Category).Distinct())
            {
                CategoryFilter.Items.Add(prod);
            }
            DataGrid.ItemsSource = listProducts.OrderBy(q=>q.Location).ToList();
            LocationFilter.SelectedIndex = LocationFilter.Items.IndexOf(newValueLocation);
            PrincipalFilter.SelectedIndex = PrincipalFilter.Items.IndexOf(newValuePrincipal);
            CategoryFilter.SelectedIndex = CategoryFilter.Items.IndexOf(newValueCategory);
        }

        private void Add_Product_Click(object sender, RoutedEventArgs e)
        { 
            CreateProduct create = new CreateProduct();
            create.Show();
        }
          
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        { 
            Product prod = e.EditingElement.DataContext as Product;
            var newValue = (e.EditingElement as TextBox).Text;
            decimal temp = decimal.TryParse(newValue, out temp) ? temp : 0;
            var oldValue = "";
            switch (e.Column.Header.ToString())
            {
                case "Principal":
                    oldValue = prod.Principal;
                    prod.Principal = newValue;
                    break;
                case "Category":
                    oldValue = prod.Category;
                    prod.Category = newValue;
                    break;
                case "Location":
                    oldValue = prod.Location;
                    prod.Location = newValue;
                    break;
                case "Name":
                    oldValue = prod.Name;
                    prod.Name = newValue;
                    break;
                case "Prod. Code":
                    oldValue = prod.ProdCode;
                    prod.ProdCode = newValue;
                    break;
                case "Case Value":
                    oldValue = prod.CaseValue + "";
                    prod.CaseValue = temp;
                    break;
                case "Pack Value":
                    oldValue = prod.PackValue + "";
                    prod.PackValue = temp;
                    break;
                case "Piece Value":
                    oldValue = prod.PieceValue + "";
                    prod.PieceValue = temp;
                    break;
                case "Case Quantity":
                    oldValue = prod.CaseBalance + "";
                    prod.CaseBalance = temp;
                    break;
                case "Pack Quantity":
                    oldValue = prod.PackBalance + "";
                    prod.PackBalance = temp;
                    break;
                case "Piece Quantity":
                    oldValue = prod.PieceBalance + "";
                    prod.PieceBalance = temp;
                    break;
                //case "Case To Packs":
                //    oldValue = prod.CaseToPacks + "";
                //    prod.CaseToPacks = temp;
                //    break;
                //case "Pack To Pieces":
                //    oldValue = prod.PackToPieces + "";
                //    prod.PackToPieces = temp;
                //    break;
            }
            if (oldValue != newValue)
            {
                if (MessageBox.Show("Are you want to Edit - " + prod.Name  +": "+ newValue+ " from "+oldValue+ " ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //prod = BalanceCaseToPackToPieces(e.Column.Header.ToString(), prod, oldValue);
                    Product.EditProduct(prod);
                    DataGrid.CommitEdit();
                    InitializeProducts();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var prod = ((FrameworkElement)sender).DataContext as Product; 
            if (MessageBox.Show("Are you want to Delete - " + prod.Name + "?" , "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Product.DeleteProduct(prod.Id);
            }
            InitializeProducts();
        }

        private void LocationFilter_DropDownClosed(object sender, EventArgs e)
        {

            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeProductsFilter(newValue.ToString(), _principalFilter, _categoryFilter);
                _locationFilter = newValue.ToString();   
            }
        }
        private void PrincipalFilter_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeProductsFilter(_locationFilter, newValue.ToString(), _categoryFilter);
                _principalFilter = newValue.ToString();
            }
        }
        private void CategoryFilter_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeProductsFilter(_locationFilter, _principalFilter, newValue.ToString());
                _categoryFilter = newValue.ToString();
            }
        }
        private void Export_to_Excel_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel export = new ExportExcel("Product");
            export.Show();
        }
        #endregion

        #region Transaction

        private string _locationFilterTrans = "All Location";
        private string _principalFilterTrans = "All Principal";
        private string _categoryFilterTrans = "All Category";
        private string _transactionFilterTrans = "All Transactions";

        public void InitializeTrans()
        {
            TransactionFilterTrans.Items.Add("All Transactions");
            TransactionFilterTrans.Items.Add("Sales");
            TransactionFilterTrans.Items.Add("Purchased");
            InitializeTransFilter("All Transactions", _locationFilterTrans,_principalFilterTrans, _categoryFilterTrans);
        }
        public void InitializeTransFilter(string newValueTransaction ="", string newValueLocation = "", string newValuePrincipal = "", string newValueCategory = "")
        {
            var listTrans = Class.TransactionOrder.GetAllTransactions();
            //List<Transaction> listProductsInTrans =
            //    listTrans.SelectMany(q => q.Transactions).ToList();
            //var listProd = listProductsInTrans.GroupBy(q => q.Product.Name).Select(s=>s.First()).Select(q=>q.Product).ToList();
            LocationFilterTrans.ItemsSource = null;
            LocationFilterTrans.Items.Clear();
            LocationFilterTrans.Items.Add("All Location");
            PrincipalFilterTrans.ItemsSource = null;
            PrincipalFilterTrans.Items.Clear();
            PrincipalFilterTrans.Items.Add("All Principal");
            CategoryFilterTrans.ItemsSource = null;
            CategoryFilterTrans.Items.Clear();
            CategoryFilterTrans.Items.Add("All Category");

            if (newValueTransaction != "All Transactions")
            {
                listTrans = listTrans.Where(q => q.TransactionType == newValueTransaction).ToList();
            }
            if (_locationFilterTrans != newValueLocation)
            {
                _principalFilterTrans = "All Principal";
                newValuePrincipal = _principalFilterTrans;
                _categoryFilterTrans = "All Category";
                newValueCategory = _categoryFilterTrans;
            }
            if (_principalFilterTrans != newValuePrincipal)
            {
                _categoryFilterTrans = "All Category";
                newValueCategory = _categoryFilterTrans;
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Location).Distinct())
            {
                LocationFilterTrans.Items.Add(prod);
            }
            if (newValueLocation != "All Location")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s=>s.Product.Location == newValueLocation)).ToList();
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Principal).Distinct())
            {
                PrincipalFilterTrans.Items.Add(prod);
            }
            if (newValuePrincipal != "All Principal")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Principal == newValuePrincipal)).ToList();
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Category).Distinct())
            {
                CategoryFilterTrans.Items.Add(prod);
            }
            if (newValueCategory != "All Category")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Category == newValueCategory)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(startDate.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction >= startDate.SelectedDate).ToList();
            }
            if (!string.IsNullOrWhiteSpace(endDate.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction <= endDate.SelectedDate).ToList();
            }

            var transView = new List<TransactionView>(); 
            foreach (var tran in listTrans)
            {
                var temp = new TransactionView
                {
                    Id = tran.Id,
                    TransactionType = tran.TransactionType,
                    DateTransaction = tran.DateTransaction,
                    Particular = tran.Particular,
                    PrincipalList = string.Join(",", tran.Transactions.Select(q => q.Product).ToList().Select(q => q.Principal).Distinct().ToList()),
                    RefNo = tran.RefNo,
                    ItemCount = tran.Transactions.Count,
                    SalesmanName = tran.SalesmanName,
                    Transactions = tran.Transactions
                };
                transView.Add(temp);
            }
             DataGrid1.ItemsSource = transView;
            _transactionFilterTrans = newValueTransaction;
            _locationFilterTrans = newValueLocation;
            _principalFilterTrans = newValuePrincipal;
            _categoryFilterTrans = newValueCategory; 
            TransactionFilterTrans.SelectedIndex = TransactionFilterTrans.Items.IndexOf(newValueTransaction);
            LocationFilterTrans.SelectedIndex = LocationFilterTrans.Items.IndexOf(newValueLocation);
            PrincipalFilterTrans.SelectedIndex = PrincipalFilterTrans.Items.IndexOf(newValuePrincipal);
            CategoryFilterTrans.SelectedIndex = CategoryFilterTrans.Items.IndexOf(newValueCategory);
        }

        private void AddSales_Click(object sender, RoutedEventArgs e)
        {  
            CreateSalesPurchased sales = new CreateSalesPurchased("Sales");
            sales.Show();
        }

        private void AddPurchased_Click(object sender, RoutedEventArgs e)
        {  
            CreateSalesPurchased purchased = new CreateSalesPurchased("Purchased");
            purchased.Show();
        }

        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
           
        }

        private void ExportTrans_Click(object sender, RoutedEventArgs e)
        {
            var transView = ((FrameworkElement)sender).DataContext as TransactionView;
            var trans = Class.TransactionOrder.GetTransaction(transView.Id);
            ExcelExport.ExcelInvoice.ExportTransactionInvoice(trans);
        }
        private void DeleteTrans_Click(object sender, RoutedEventArgs e)
        {
            var transView = ((FrameworkElement)sender).DataContext as TransactionView;
            if (MessageBox.Show("Are you want to Delete - " + transView.RefNo + "?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                    foreach (var trans in transView.Transactions)
                {
                        var prod = Product.GetProduct(trans.Product.Id);
                        if (transView.TransactionType == "Purchased")
                        {
                        Product.BalanceCasePackPiece(trans, prod, "Sales");
                        }
                        else
                    {
                        Product.BalanceCasePackPiece(trans, prod, "Purchased");
                    }
                    Product.EditProduct(prod);
                }
                Class.TransactionOrder.DeleteTransaction(transView.Id);
            }
            InitializeTrans();
            InitializeProducts();

        }
        private void ExportGroup_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel export = new ExportExcel("Transaction");
            export.Show();
        }
        #endregion

        private void TransactionFilterTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransFilter(newValue.ToString(), _locationFilterTrans, _principalFilterTrans, _categoryFilterTrans);
            }
        }

        private void LocationFilterTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransFilter(_transactionFilterTrans, newValue.ToString(), _principalFilterTrans, _categoryFilterTrans);
            }
        }

        private void PrincipalFilterTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransFilter(_transactionFilterTrans, _locationFilterTrans, newValue.ToString(), _categoryFilterTrans);
            }
        }
        private void CategoryFilterTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransFilter(_transactionFilterTrans, _locationFilterTrans, _principalFilterTrans, newValue.ToString());
            }
        }
        private void startDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = (sender as DatePicker).Text;
            if (newValue != null)
            {
                InitializeTransFilter(_transactionFilterTrans, _locationFilterTrans, _principalFilter, _categoryFilterTrans);
            }
        }

        private void endDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = (sender as DatePicker).Text;
            if (newValue != null)
            {
                InitializeTransFilter(_transactionFilterTrans, _locationFilterTrans, _principalFilter, _categoryFilterTrans);
            }
        }

        private void DetailsTrans_Click(object sender, RoutedEventArgs e)
        {
            var transView = ((FrameworkElement)sender).DataContext as TransactionView;
            Details details = new Details(TransactionOrder.GetTransaction(transView.Id));
            details.Show();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
    }
}
