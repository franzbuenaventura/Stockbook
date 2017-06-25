// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportExcel.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
// <summary>
//   The ExportExcel Class that contains the backend for ExportExcel Window
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
    /// Interaction logic for ExportExcel 
    /// </summary>
    public partial class ExportExcel
    {
        /// <summary>
        /// The location of the product window.
        /// </summary>
        private string locationFilter = "All Location";

        /// <summary>
        /// The principal of the product window.
        /// </summary>
        private string principalFilter = "All Principal";

        /// <summary>
        /// The category of the product window.
        /// </summary>
        private string categoryFilter = "All Category";

        /// <summary>
        /// The product list that will be exported
        /// </summary>
        private List<Product> prodList = null;

        /// <summary>
        /// The location filter of the transaction window.
        /// </summary>
        private string locationTransFilter = "All Location";

        /// <summary>
        /// The principal filter of the transaction window.
        /// </summary>
        private string principalTransFilter = "All Principal";

        /// <summary>
        /// The category filter of the transaction window.
        /// </summary>
        private string categoryTransFilter = "All Category";

        /// <summary>
        /// The type filter of the transaction window.
        /// </summary>
        private string typeTransFilter = "All Transactions";

        /// <summary>
        /// The particular filter of the transaction window.
        /// </summary>
        private string particularTransFilter = "All Particular";

        /// <summary>
        /// The salesman filter of the transaction window.
        /// </summary>
        private string salesmanTransFilter = "All Salesman";

        /// <summary>
        /// The name filter of the transaction window.
        /// </summary>
        private string nameTransFilter = "All Product";

        /// <summary>
        /// The list transactions that will be exported.
        /// </summary>
        private List<TransactionOrder> listTransactions = new List<TransactionOrder>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportExcel"/> class and initialize the product and transactions
        /// </summary>
        /// <param name="tab">
        /// The tab is either Product tab or Transaction tab that will be shown
        /// </param>
        public ExportExcel(string tab)
        {
            this.InitializeComponent();
            this.InitializeProductsView();
            this.InitializeTransView();
            if (tab == "Product")
            {
                this.tabControl.SelectedIndex = 0;
                this.tabControl.SelectedItem = this.productTab;
            }
           else if (tab == "Transaction")
           {
               this.tabControl.SelectedIndex = 1;
               this.tabControl.SelectedItem = this.transactionTab;
            }
        }


        /// <summary>
        /// Initialize products view by calling InitializeProductsFilter and making selected columns a read only
        /// </summary>
        public void InitializeProductsView()
        {
            var config = StockbookWindows.OpenConfig();
            var currency = string.Empty;
            switch (config.Currency)
            {
                case "PHP - ₱":
                    currency = "₱#,###,##0.00";
                    break;
                case "USD - $":
                    currency = "$#,###,##0.00";
                    break;
                case "YEN - ¥":
                    currency = "¥#,###,##0.00";
                    break;
            }


            this.InitializeProducts(this.locationFilter, this.principalFilter, this.categoryFilter);
            foreach (var dc in this.DataGrid.Columns)
            {
                dc.IsReadOnly = true;

                if (dc.Header.ToString() == "Case Val" || dc.Header.ToString() == "Pack Val" || dc.Header.ToString() == "Piece Val")
                {
                    ((DataGridTextColumn)dc).Binding.StringFormat = currency;
                }
            }
        }

        #region Products

        /// <summary>
        /// The method initialize the products and filter it with the input of the user
        /// </summary>
        /// <param name="newValueLocation">
        /// The new value location filter if user changed it. 
        /// </param>
        /// <param name="newValuePrincipal">
        /// The new value principal filter if user changed it.
        /// </param>
        /// <param name="newValueCategory">
        /// The new value category filter if user changed it.
        /// </param>
        private void InitializeProducts(string newValueLocation, string newValuePrincipal, string newValueCategory)
        {
            var listProducts = Product.GetAllProducts();
            this.PrincipalInput.Items.Clear();
            this.PrincipalInput.Items.Add("All Principal");
            this.CategoryInput.Items.Clear();
            this.CategoryInput.Items.Add("All Category");
            this.LocationInput.Items.Clear();
            this.LocationInput.Items.Add("All Location");

            foreach (var item in listProducts.Select(q => q.Location).Distinct())
            {
                this.LocationInput.Items.Add(item);
            }

            if (newValueLocation == "All Location")
            {
                foreach (var item in listProducts.Select(q => q.Principal).Distinct())
                {
                    this.PrincipalInput.Items.Add(item);
                }
            }
            else
            {
                foreach (
                    var item in
                    listProducts.Where(q => q.Location == newValueLocation).Select(q => q.Principal).Distinct())
                {
                    this.PrincipalInput.Items.Add(item);
                }
            }

            // Filter Category
            var categoryList = Product.GetAllProducts();
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
                this.CategoryInput.Items.Add(item);
            }

            // Check if new Location or Principal
            if (this.locationFilter != newValueLocation)
            {
                this.principalFilter = "All Principal";
                newValuePrincipal = this.principalFilter;
                this.categoryFilter = "All Category";
                newValueCategory = this.categoryFilter;
            }
            else if (this.principalFilter != newValuePrincipal)
            {
                this.categoryFilter = "All Category";
                newValueCategory = this.categoryFilter;
            }

            if (newValueCategory != "All Category")
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

            this.DataGrid.ItemsSource = listProducts;
            this.prodList = listProducts;
            this.LocationInput.SelectedIndex = this.LocationInput.Items.IndexOf(newValueLocation);
            this.PrincipalInput.SelectedIndex = this.PrincipalInput.Items.IndexOf(newValuePrincipal);
            this.CategoryInput.SelectedIndex = this.CategoryInput.Items.IndexOf(newValueCategory);
        }

        /// <summary>
        /// The location input drop down closed event when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void LocationInputDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem.ToString();
            if (newValue != this.locationFilter)
            {
                this.InitializeProducts(newValue, this.principalFilter, this.categoryFilter);
                this.locationFilter = newValue;
            }
        }

        /// <summary>
        /// The principal input drop down closed event when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void PrincipalInputDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem.ToString();
            if (newValue != this.principalFilter)
            {
                this.InitializeProducts(this.locationFilter, newValue, this.categoryFilter);
                this.principalFilter = newValue;
            }
        }

        /// <summary>
        /// The category input drop down closed event when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void CategoryInputDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem.ToString();
            if (newValue != this.categoryFilter)
            {
                this.InitializeProducts(this.locationFilter, this.principalFilter, newValue);
                this.categoryFilter = newValue;
            }
        }

        /// <summary>
        /// The export products click event when user wants to export the product list to excel
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void ExportProductsClick(object sender, RoutedEventArgs e)
        {
            ExcelExport.ExcelInvoice.ExportProducts(this.prodList, this.LocationInput.Text, this.PrincipalInput.Text, this.CategoryInput.Text);
        }

        #endregion

        #region Transactions

        /// <summary>
        /// Initialize transaction view by calling InitializeTransFilter and making selected columns a read only
        /// </summary>
        public void InitializeTransView()
        {
            this.InitializeTransactions(
                this.locationTransFilter,
                this.principalTransFilter,
                this.categoryTransFilter,
                this.nameTransFilter,
                this.typeTransFilter,
                this.particularTransFilter,
                this.salesmanTransFilter);
            foreach (var dc in this.TransactionDataGrid.Columns)
            {
                dc.IsReadOnly = true;
            }
        }


        /// <summary>
        /// The method initialize the transactions and filter it with the input of the user
        /// </summary>
        /// <param name="newValueLocation">
        /// The new value location filter if user changed it. 
        /// </param>
        /// <param name="newValuePrincipal">
        /// The new value principal filter if user changed it. 
        /// </param>
        /// <param name="newValueCategory">
        /// The new value category filter if user changed it. 
        /// </param>
        /// <param name="newValueName">
        /// The new value name filter if user changed it. 
        /// </param>
        /// <param name="newValueType">
        /// The new value type filter if user changed it. 
        /// </param>
        /// <param name="newValueParticular">
        /// The new value particular filter if user changed it. 
        /// </param>
        /// <param name="newValueSalesman">
        /// The new value salesman filter if user changed it. 
        /// </param>
        private void InitializeTransactions(string newValueLocation, string newValuePrincipal, string newValueCategory, string newValueName, string newValueType, string newValueParticular, string newValueSalesman)
        {
            var listTrans = TransactionOrder.GetAllTransactions();
            this.PrincipalInputTrans.Items.Clear();
            this.PrincipalInputTrans.Items.Add("All Principal");
            this.CategoryInputTrans.Items.Clear();
            this.CategoryInputTrans.Items.Add("All Category");
            this.LocationInputTrans.Items.Clear();
            this.LocationInputTrans.Items.Add("All Location");
            this.TypeInputTrans.Items.Clear();
            this.TypeInputTrans.Items.Add("All Transactions");
            this.TypeInputTrans.Items.Add("Sales");
            this.TypeInputTrans.Items.Add("Purchased");
            this.ParticularInputTrans.Items.Clear();
            this.ParticularInputTrans.Items.Add("All Particular");
            this.SalesmanInputTrans.Items.Clear();
            this.SalesmanInputTrans.Items.Add("All Salesman");
            this.NameInputTrans.Items.Clear();
            this.NameInputTrans.Items.Add("All Product");

            // Remove This
            foreach (var prod in listTrans.Select(q => q.TransactionType).Distinct())
            {
                this.TypeInputTrans.Items.Add(prod);
            }

            if (newValueType != "All Transactions")
            {
                listTrans = listTrans.Where(q => q.TransactionType == newValueType).ToList();
            }

            foreach (var prod in listTrans.Select(q => q.Particular).Distinct())
            {
                this.ParticularInputTrans.Items.Add(prod);
            }

            if (newValueParticular != "All Particular")
            {
                listTrans = listTrans.Where(q => q.Particular == newValueParticular).ToList();
            }

            foreach (var prod in listTrans.Select(q => q.SalesmanName).Distinct())
            {
                this.SalesmanInputTrans.Items.Add(prod);
            }

            if (newValueSalesman != "All Salesman")
            {
                listTrans = listTrans.Where(q => q.SalesmanName == newValueSalesman).ToList();
            }
 
            if (this.locationTransFilter != newValueLocation)
            {
                this.principalTransFilter = "All Principal";
                newValuePrincipal = this.principalTransFilter;
                this.categoryTransFilter = "All Category";
                newValueCategory = this.categoryTransFilter;
                this.nameTransFilter = "All Product";
                newValueName = this.nameTransFilter;
            }

            if (this.principalTransFilter != newValuePrincipal)
            {
                this.categoryTransFilter = "All Category";
                newValueCategory = this.categoryTransFilter;
                this.nameTransFilter = "All Product";
                newValueName = this.nameTransFilter;
            }

            if (this.categoryTransFilter != newValueCategory)
            {
                this.nameTransFilter = "All Product";
                newValueName = this.nameTransFilter;
            }

            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Location).Distinct())
            {
                this.LocationInputTrans.Items.Add(prod);
            }

            if (newValueLocation != "All Location")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Location == newValueLocation)).ToList();
            }

            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Principal).Distinct())
            {
                this.PrincipalInputTrans.Items.Add(prod);
            }

            if (newValuePrincipal != "All Principal")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Principal == newValuePrincipal)).ToList();
            }

            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Category).Distinct())
            {
                this.CategoryInputTrans.Items.Add(prod);
            }

            if (newValueCategory != "All Category")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Category == newValueCategory)).ToList();
            }

            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Name).Distinct())
            {
                this.NameInputTrans.Items.Add(prod);
            }

            if (newValueName != "All Product")
            {
                listTrans = listTrans.Where(q => q.Transactions.Exists(s => s.Product.Name == newValueName)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(this.DateFrom.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction.Date >= this.DateFrom.SelectedDate).ToList();
            }

            if (!string.IsNullOrWhiteSpace(this.DateTo.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction.Date <= this.DateTo.SelectedDate).ToList();
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

            this.typeTransFilter = newValueType;
            this.locationTransFilter = newValueLocation;
            this.principalTransFilter = newValuePrincipal;
            this.categoryTransFilter = newValueCategory;
            this.particularTransFilter = newValueParticular;
            this.salesmanTransFilter = newValueSalesman;
            this.nameTransFilter = newValueName;
            this.TypeInputTrans.SelectedIndex = this.TypeInputTrans.Items.IndexOf(newValueType);
            this.LocationInputTrans.SelectedIndex = this.LocationInputTrans.Items.IndexOf(newValueLocation);
            this.PrincipalInputTrans.SelectedIndex = this.PrincipalInputTrans.Items.IndexOf(newValuePrincipal);
            this.CategoryInputTrans.SelectedIndex = this.CategoryInputTrans.Items.IndexOf(newValueCategory);
            this.ParticularInputTrans.SelectedIndex = this.ParticularInputTrans.Items.IndexOf(newValueParticular);
            this.SalesmanInputTrans.SelectedIndex = this.SalesmanInputTrans.Items.IndexOf(newValueSalesman);
            this.NameInputTrans.SelectedIndex = this.NameInputTrans.Items.IndexOf(newValueName);
            this.listTransactions = listTrans;
            this.TransactionDataGrid.ItemsSource = transView;
        }

        /// <summary>
        /// The location input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void LocationInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(newValue.ToString(), this.principalTransFilter,  this.categoryTransFilter, this.nameTransFilter, this.typeTransFilter, this.particularTransFilter, this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The principal input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void PrincipalInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, newValue.ToString(), this.categoryTransFilter, this.nameTransFilter, this.typeTransFilter, this.particularTransFilter, this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The category input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void CategoryInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, this.principalTransFilter, newValue.ToString(), this.nameTransFilter, this.typeTransFilter, this.particularTransFilter, this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The name input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void NameInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, this.principalTransFilter, this.categoryTransFilter, newValue.ToString(), this.typeTransFilter, this.particularTransFilter, this.salesmanTransFilter);
            }

        }

        /// <summary>
        /// The type input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void TypeInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, this.principalTransFilter, this.categoryTransFilter, this.nameTransFilter, newValue.ToString(), this.particularTransFilter, this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The particular input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void ParticularInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, this.principalTransFilter, this.categoryTransFilter, this.nameTransFilter, this.typeTransFilter, newValue.ToString(), this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The salesman input trans drop down closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void SalesmanInputTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransactions(this.locationTransFilter, this.principalTransFilter, this.categoryTransFilter, this.nameTransFilter, this.typeTransFilter,  this.particularTransFilter, newValue.ToString());
            }
        }

        /// <summary>
        /// The date from calendar closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the date
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void DateFromCalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = ((DatePicker)sender).Text;
            if (newValue != null)
            {
                this.InitializeTransactions(
                    this.locationTransFilter,
                    this.principalTransFilter,
                    this.categoryTransFilter,
                    this.nameTransFilter,
                    this.typeTransFilter,
                    this.particularTransFilter,
                    this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The date to calendar closed when user change the value.
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the date
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void DateToCalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = ((DatePicker)sender).Text;
            if (newValue != null)
            {
                this.InitializeTransactions(
                    this.locationTransFilter,
                    this.principalTransFilter,
                    this.categoryTransFilter,
                    this.nameTransFilter,
                    this.typeTransFilter,
                    this.particularTransFilter,
                    this.salesmanTransFilter);
            }
        }

        /// <summary>
        /// The export excel trans click event when user wants to export the product list to excel
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void ExportExcelTransClick(object sender, RoutedEventArgs e)
        {
            if (this.DateFrom.SelectedDate == null)
            {
                this.DateFrom.SelectedDate = new DateTime(2000, 1, 1);
            }

            if (this.DateTo.SelectedDate == null)
            {
                this.DateTo.SelectedDate = DateTime.Now;
            }
             
            ExcelExport.ExcelInvoice.ExportTransactions(this.listTransactions, this.DateFrom.SelectedDate.Value, this.DateTo.SelectedDate.Value);
        }

        #endregion
    }
}
