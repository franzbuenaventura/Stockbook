using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Stockbook.Class;
using Stockbook.Model;

namespace Stockbook.Products
{
    /// <summary>
    /// Interaction logic for ExportExcel.xaml
    /// </summary>
    public partial class ExportExcel : Window
    {
        private string _locationFilter = "All Location";
        private string _principalFilter = "All Principal";
        private string _categoryFilter = "All Category";
        private List<Product> prodList = null;
        public ExportExcel(string tab)
        {
            InitializeComponent(); 
            InitializeProducts(_locationFilter, _principalFilter,_categoryFilter);
            InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans,_nameFilterTrans, _typeFilterTrans,
                _particularFilterTrans, _salesmanFilterTrans);
            if (tab == "Product")
            {
                tabControl.SelectedIndex = 0;
                tabControl.SelectedItem = productTab;
            }
           else if (tab == "Transaction")
           {
                tabControl.SelectedIndex = 1;
                tabControl.SelectedItem = transactionTab;
            }
        }

        #region Products
        private void InitializeProducts(string newValueLocation, string newValuePrincipal, string newValueCategory)
        {
            var listProducts = DbClass.ProductHelper.GetAllProducts();
            PrincipalInput.Items.Clear();
            PrincipalInput.Items.Add("All Principal");
            CategoryInput.Items.Clear();
            CategoryInput.Items.Add("All Category");
            LocationInput.Items.Clear();
            LocationInput.Items.Add("All Location"); 
            //Filter Items
            foreach (var item in listProducts.Select(q => q.Location).Distinct())
            {
                LocationInput.Items.Add(item);
            } 
            if (newValueLocation == "All Location")
            {
                foreach (var item in listProducts.Select(q => q.Principal).Distinct())
                {
                    PrincipalInput.Items.Add(item);
                }
            }
            else
            { 
                foreach (var item in listProducts.Where(q => q.Location == newValueLocation).Select(q => q.Principal).Distinct())
                {
                    PrincipalInput.Items.Add(item);
                }
            } 
            //Filter Category
            var categoryList = DbClass.ProductHelper.GetAllProducts(); 
             if (newValuePrincipal != "All Principal")
            {
                categoryList = categoryList.Where(q => q.Principal == newValuePrincipal).ToList();
            }
             if (newValueLocation != "All Location")
            {
                categoryList = categoryList.Where(q => q.Location == newValueLocation).ToList();
            }
            foreach (var item in categoryList.Select(q => q.Category).Distinct())
            {
                CategoryInput.Items.Add(item);
            }
            //Check if new Location or Principal
            if (_locationFilter != newValueLocation)
            {
                _principalFilter = "All Principal";
                newValuePrincipal = _principalFilter;
                _categoryFilter = "All Category";
                newValueCategory = _categoryFilter;
            }
            else if (_principalFilter != newValuePrincipal)
            {
                _categoryFilter = "All Category";
                newValueCategory = _categoryFilter;
            } 
            if (newValueCategory != "All Category" )
            {
                listProducts = listProducts.Where(q => q.Category == newValueCategory).ToList();
            }
            if (newValuePrincipal != "All Principal")
            {
                listProducts = listProducts.Where(q => q.Principal == newValuePrincipal).ToList();
            }
            if (newValueLocation != "All Location")
            {
                listProducts = listProducts.Where(q => q.Location == newValueLocation).ToList();
            }
            DataGrid.ItemsSource = listProducts;
            prodList = listProducts;
            LocationInput.SelectedIndex = LocationInput.Items.IndexOf(newValueLocation);
            PrincipalInput.SelectedIndex = PrincipalInput.Items.IndexOf(newValuePrincipal);
            CategoryInput.SelectedIndex = CategoryInput.Items.IndexOf(newValueCategory);
        }
        private void LocationInput_DropDownClosed(object sender, System.EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem.ToString();
            InitializeProducts(newValue, _principalFilter, _categoryFilter);
            _locationFilter = newValue;
        }
        private void PrincipalInput_DropDownClosed(object sender, System.EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem.ToString();
            InitializeProducts(_locationFilter, newValue, _categoryFilter);
            _principalFilter = newValue;

        }
        private void CategoryInput_DropDownClosed(object sender, System.EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem.ToString();
            InitializeProducts(_locationFilter, _principalFilter, newValue);
            _categoryFilter = newValue;
        }
        private void ExportProducts_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper.ExcelInvoice.ExportProducts(prodList, LocationInput.Text, PrincipalInput.Text,CategoryInput.Text);
        }
        #endregion

        #region Transactions
        private string _locationFilterTrans = "All Location";
        private string _principalFilterTrans = "All Principal";
        private string _categoryFilterTrans = "All Category";
        private string _typeFilterTrans = "All Transactions";
        private string _particularFilterTrans = "All Particular";
        private string _salesmanFilterTrans = "All Salesman";
        private string _nameFilterTrans = "All Product";
        private List<TransactionOrder> listTransactions = new List<TransactionOrder>();

        private void InitializeTransactions(string newValLocation, string newValPrincipal, string newValCategory, string newValName, string newValType, string newValParticular, string newValSalesman)
        {
            var listTrans = DbClass.TransactionHelper.GetAllTransactions();
            PrincipalInputTrans.Items.Clear();
            PrincipalInputTrans.Items.Add("All Principal");
            CategoryInputTrans.Items.Clear();
            CategoryInputTrans.Items.Add("All Category");
            LocationInputTrans.Items.Clear();
            LocationInputTrans.Items.Add("All Location");
            TypeInputTrans.Items.Clear();
            TypeInputTrans.Items.Add("All Transactions");
            TypeInputTrans.Items.Add("Sales");
            TypeInputTrans.Items.Add("Purchased");
            ParticularInputTrans.Items.Clear();
            ParticularInputTrans.Items.Add("All Particular");
            SalesmanInputTrans.Items.Clear();
            SalesmanInputTrans.Items.Add("All Salesman");
            NameInputTrans.Items.Clear();
            NameInputTrans.Items.Add("All Product");

            //Remove This
            foreach (var prod in listTrans.Select(q => q.TransactionType).Distinct())
            {
                TypeInputTrans.Items.Add(prod);
            }
            if (newValType != "All Transactions")
            {
                listTrans = listTrans.Where(q => q.TransactionType == newValType).ToList();
            }
            foreach (var prod in listTrans.Select(q => q.Particular).Distinct())
            {
                ParticularInputTrans.Items.Add(prod);
            }
            if (newValParticular != "All Particular")
            {
                listTrans = listTrans.Where(q => q.Particular == newValParticular).ToList();
            }
            foreach (var prod in listTrans.Select(q => q.SalesmanName).Distinct())
            {
                SalesmanInputTrans.Items.Add(prod);
            }
            if (newValSalesman != "All Salesman")
            {
                listTrans = listTrans.Where(q => q.SalesmanName == newValSalesman).ToList();
            } 
            if (_locationFilterTrans != newValLocation)
            {
                _principalFilterTrans = "All Principal";
                newValPrincipal = _principalFilterTrans;
                _categoryFilterTrans = "All Category";
                newValCategory = _categoryFilterTrans;
                _nameFilterTrans = "All Product";
                newValName = _nameFilterTrans;
            }
            if (_principalFilterTrans != newValPrincipal)
            {
                _categoryFilterTrans = "All Category";
                newValCategory = _categoryFilterTrans;
                _nameFilterTrans = "All Product";
                newValName = _nameFilterTrans;
            }
            if (_categoryFilterTrans != newValCategory)
            {
                _nameFilterTrans = "All Product";
                newValName = _nameFilterTrans;
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Location).Distinct())
            {
                LocationInputTrans.Items.Add(prod);
            }
            if (newValLocation != "All Location")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Location == newValLocation)).ToList();
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Principal).Distinct())
            {
                PrincipalInputTrans.Items.Add(prod);
            }
            if (newValPrincipal != "All Principal")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Principal == newValPrincipal)).ToList();
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Category).Distinct())
            {
                CategoryInputTrans.Items.Add(prod);
            }
            if (newValCategory != "All Category")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Category == newValCategory)).ToList();
            }
            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Name).Distinct())
            {
                NameInputTrans.Items.Add(prod);
            }
            if (newValName != "All Product")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Name == newValName)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(DateFrom.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction.Date >= DateFrom.SelectedDate).ToList();
            }
            if (!string.IsNullOrWhiteSpace(DateTo.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction.Date <= DateTo.SelectedDate).ToList();
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
            _typeFilterTrans = newValType;
            _locationFilterTrans = newValLocation;
            _principalFilterTrans = newValPrincipal;
            _categoryFilterTrans = newValCategory;
            _particularFilterTrans = newValParticular;
            _salesmanFilterTrans = newValSalesman;
            _nameFilterTrans = newValName;
            TypeInputTrans.SelectedIndex = TypeInputTrans.Items.IndexOf(newValType);
            LocationInputTrans.SelectedIndex = LocationInputTrans.Items.IndexOf(newValLocation);
            PrincipalInputTrans.SelectedIndex = PrincipalInputTrans.Items.IndexOf(newValPrincipal);
            CategoryInputTrans.SelectedIndex = CategoryInputTrans.Items.IndexOf(newValCategory);
            ParticularInputTrans.SelectedIndex = ParticularInputTrans.Items.IndexOf(newValParticular);
            SalesmanInputTrans.SelectedIndex = SalesmanInputTrans.Items.IndexOf(newValSalesman);
            NameInputTrans.SelectedIndex = NameInputTrans.Items.IndexOf(newValName);
            listTransactions = listTrans;
            DataGrid1.ItemsSource = transView;
        }
        private void LocationInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(newValue.ToString(), _principalFilterTrans,  _categoryFilterTrans,_nameFilterTrans, _typeFilterTrans,_particularFilterTrans,_salesmanFilterTrans);
            }
        }
        private void PrincipalInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, newValue.ToString(), _categoryFilterTrans, _nameFilterTrans, _typeFilterTrans, _particularFilterTrans, _salesmanFilterTrans);
            }
        }
        private void CategoryInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, newValue.ToString(), _nameFilterTrans, _typeFilterTrans, _particularFilterTrans, _salesmanFilterTrans);
            }
        }
        private void NameInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans,newValue.ToString(), _typeFilterTrans, _particularFilterTrans, _salesmanFilterTrans);
            }

        }
        private void TypeInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans, _nameFilterTrans, newValue.ToString(), _particularFilterTrans, _salesmanFilterTrans);
            }
        }
        private void ParticularInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans, _nameFilterTrans, _typeFilterTrans, newValue.ToString(), _salesmanFilterTrans);
            }
        }
        private void SalesmanInputTrans_DropDownClosed(object sender, EventArgs e)
        {
            var newValue = (sender as ComboBox).SelectedItem;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans, _nameFilterTrans, _typeFilterTrans,  _particularFilterTrans,newValue.ToString());
            }
        }
        private void DateFrom_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = (sender as DatePicker).Text;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans, _nameFilterTrans, _typeFilterTrans,
    _particularFilterTrans, _salesmanFilterTrans);
            }
        }
        private void DateTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = (sender as DatePicker).Text;
            if (newValue != null)
            {
                InitializeTransactions(_locationFilterTrans, _principalFilterTrans, _categoryFilterTrans, _nameFilterTrans, _typeFilterTrans,
    _particularFilterTrans, _salesmanFilterTrans);
            }
        }
        private void ExportExcelTrans_Click(object sender, RoutedEventArgs e)
        {
            if (DateFrom.SelectedDate == null)
            {
                DateFrom.SelectedDate = new DateTime(2000, 1, 1);
            }
            if (DateTo.SelectedDate == null)
            {
                DateTo.SelectedDate = DateTime.Now;
            }
            ExcelHelper.ExcelInvoice.ExportTransactions(listTransactions, DateFrom.SelectedDate.Value, DateTo.SelectedDate.Value);
        }
        #endregion
         
    }
}
