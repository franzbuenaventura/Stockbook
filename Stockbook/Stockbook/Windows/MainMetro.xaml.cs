// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainMetro.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
// <summary>
//   The MainMetro Class that contains the backend for MainMetro Window
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Class;

    using Model;

    /// <summary>
    /// The main metro.
    /// </summary>
    public partial class MainMetro
    {
        // Private Instances

        /// <summary>
        /// The default value for location filter in products list view.
        /// </summary>
        private string locationFilterProduct = "All Location";

        /// <summary>
        /// The default value for principal filter in products list view.
        /// </summary>
        private string principalFilterProduct = "All Principal";

        /// <summary>
        /// The default value for category filter in products list view.
        /// </summary>
        private string categoryFilterProduct = "All Category";

        /// <summary>
        /// The default value for location filter in transactions list view.
        /// </summary>
        private string particularFilterTrans = "All Particular";

        /// <summary>
        /// The default value for principal filter in transactions list view.
        /// </summary>
        private string principalFilterTrans = "All Principal";

        /// <summary>
        /// The default value for category filter in transactions list view.
        /// </summary>
        private string salesmanFilterTrans = "All Salesman";

        /// <summary>
        /// The default value for transaction filter in transactions list view.
        /// </summary>
        private string transactionFilterTrans = "All Transactions";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMetro"/> class and initialize Product and Transaction views
        /// </summary>
        public MainMetro()
        {
            this.InitializeComponent();
            this.InitializeProductsView();
            this.InitializeTransView();
            StockbookWindows.AutoBackup();


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(this.DispatcherTimerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// The dispatcher timer tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            StockbookWindows.AutoBackup();
        }

        #region Inventory 

        /// <summary>
        /// Initialize products view by calling InitializeProductsFilter and making selected columns a read only
        /// </summary>
        public void InitializeProductsView()
        {
            this.InitializeProductsFilter(
                this.locationFilterProduct,
                this.principalFilterProduct,
                this.categoryFilterProduct);

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
            try
            {
                foreach (var dc in this.DataGrid.Columns)
                {
                    if (dc.Header.ToString() == "Case Bal" || dc.Header.ToString() == "Pack Bal" || dc.Header.ToString() == "Piece Bal"
                        || dc.Header.ToString() == "Case To Packs" || dc.Header.ToString() == "Packs To Pieces")
                    {
                        var firstOrDefault = this.DataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header);
                        if (firstOrDefault != null)
                        {
                            firstOrDefault.IsReadOnly = true;
                        }
                    }
                    if (dc.Header.ToString() == "Case Val" || dc.Header.ToString() == "Pack Val" || dc.Header.ToString() == "Piece Val")
                    {
                        ((DataGridTextColumn)dc).Binding.StringFormat = currency;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///  The initialize products filter determines the products that will be printed in View depending on the current filter parameters
        /// </summary>
        /// <param name="newValueLocation">
        /// The parameter for a new location value or default empty if user did not put any
        /// </param>
        /// <param name="newValuePrincipal">
        /// The parameter for a new principal value or default empty if user did not put any
        /// </param>
        /// <param name="newValueCategory">
        /// The parameter for a new category value or default empty if user did not put any
        /// </param>
        private void InitializeProductsFilter(string newValueLocation = "", string newValuePrincipal = "", string newValueCategory = "")
        {
            var listProducts = Product.GetAllProducts();
            this.LocationFilter.ItemsSource = null;
            this.LocationFilter.Items.Clear();
            this.LocationFilter.Items.Add("All Location");
            this.PrincipalFilter.ItemsSource = null;
            this.PrincipalFilter.Items.Clear();
            this.PrincipalFilter.Items.Add("All Principal");
            this.CategoryFilter.ItemsSource = null;
            this.CategoryFilter.Items.Clear();
            this.CategoryFilter.Items.Add("All Category");
            this.DataGrid.CommitEdit();

            foreach (var prod in listProducts.Select(q => q.Location).Distinct())
            {
                this.LocationFilter.Items.Add(prod);
            }

            if (this.locationFilterProduct != newValueLocation)
            {
                this.principalFilterProduct = "All Principal";
                newValuePrincipal = this.principalFilterProduct;
                this.categoryFilterProduct = "All Category";
                newValueCategory = this.categoryFilterProduct;
            }

            if (this.principalFilterProduct != newValuePrincipal)
            {
                this.categoryFilterProduct = "All Category";
                newValueCategory = this.categoryFilterProduct;
            }

            if (newValueLocation != "All Location")
            {
                listProducts = listProducts.Where(q => q.Location == newValueLocation).ToList();
            }

            foreach (var prod in listProducts.Select(q => q.Principal).Distinct())
            {
                this.PrincipalFilter.Items.Add(prod);
            }

            if (newValuePrincipal != "All Principal")
            {
                listProducts = listProducts.Where(q => q.Principal == newValuePrincipal).ToList();
            }

            foreach (var prod in listProducts.Select(q => q.Category).Distinct())
            {
                this.CategoryFilter.Items.Add(prod);
            }

            if (newValueCategory != "All Category")
            {
                listProducts = listProducts.Where(q => q.Category == newValueCategory).ToList();
            }

            this.DataGrid.ItemsSource = listProducts.OrderBy(q => q.Location).ToList();
            this.LocationFilter.SelectedIndex = this.LocationFilter.Items.IndexOf(newValueLocation);
            this.PrincipalFilter.SelectedIndex = this.PrincipalFilter.Items.IndexOf(newValuePrincipal);
            this.CategoryFilter.SelectedIndex = this.CategoryFilter.Items.IndexOf(newValueCategory);
        }

        /// <summary>
        /// The add product event button from Products View will create a new window for CreateProduct
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the cell, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the routed event arguments if applicable
        /// </param>
        private void AddProductClick(object sender, RoutedEventArgs e)
        { 
            CreateProduct create = new CreateProduct();
            create.Show();
        }

        /// <summary>
        /// The data grid cell edit ending for product view columns that can be edited via cell editor
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the cell, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The event argument which contains the product that has been updated or will be updated
        /// </param>
        private void DataGridCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        { 
            var prod = e.EditingElement.DataContext as Product;
            var newValue = ((TextBox)e.EditingElement).Text.Replace("$",string.Empty).Replace("¥", string.Empty).Replace("₱", string.Empty).Replace(",", string.Empty).Trim();
            decimal temp = decimal.TryParse(newValue, out temp) ? temp : 0;
            var oldValue = string.Empty;
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
                case "Case Val":
                    oldValue = prod.CaseValue.ToString(CultureInfo.CurrentCulture);
                    prod.CaseValue = temp;
                    break;
                case "Pack Val":
                    oldValue = prod.PackValue.ToString(CultureInfo.CurrentCulture);
                    prod.PackValue = temp;
                    break;
                case "Piece Val":
                    oldValue = prod.PieceValue.ToString(CultureInfo.CurrentCulture);
                    prod.PieceValue = temp;
                    break;  
            }

            if (oldValue != newValue)
            {
                if (
                    MessageBox.Show(
                        "Are you want to Edit - " + prod.Name + " : " + " from " + oldValue + " to " + newValue + " ?",
                        "Confirm",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Product.EditProduct(prod);
                    this.DataGrid.CommitEdit();
                    this.InitializeProductsFilter(this.locationFilterProduct, this.principalFilterProduct, this.categoryFilterProduct);
                }
            }
        }

        /// <summary>
        /// The delete click event if the user desire to delete a row, it will show a message box before deleting the value permanently
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the cell, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the routed event arguments if applicable
        /// </param>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var prod = ((FrameworkElement)sender).DataContext as Product;
            if (MessageBox.Show(
                    "Are you want to Delete - " + prod.Name + "?",
                    "Confirm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Product.DeleteProduct(prod.Id);
            }

            this.InitializeProductsFilter(this.locationFilterProduct, this.principalFilterProduct, this.categoryFilterProduct);
        }

        /// <summary>
        /// The location filter drop down closed event, will change the locationFilterProduct to the a new value
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void LocationFilterDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeProductsFilter(newValue.ToString(), this.principalFilterProduct, this.categoryFilterProduct);
                this.locationFilterProduct = newValue.ToString();   
            }
        }

        /// <summary>
        /// The principal filter drop down closed event, will change the locationFilterProduct to the a new value
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void PrincipalFilterDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeProductsFilter(this.locationFilterProduct, newValue.ToString(), this.categoryFilterProduct);
                this.principalFilterProduct = newValue.ToString();
            }
        }

        /// <summary>
        /// The category filter drop down closed, will change the locationFilterProduct to the a new value
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void CategoryFilterDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeProductsFilter(this.locationFilterProduct, this.principalFilterProduct, newValue.ToString());
                this.categoryFilterProduct = newValue.ToString();
            }
        }

        /// <summary>
        /// The export to excel click, from Products View will create a new window for ExportExcel
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void ExportToExcelClick(object sender, RoutedEventArgs e)
        {
            ExportExcel export = new ExportExcel("Product");
            export.Show();
        }

        #endregion

        #region Transaction

        /// <summary>
        /// Initialize transaction view by calling InitializeTransFilter and making selected columns a read only
        /// </summary>
        public void InitializeTransView()
        {
            this.TransactionFilterTrans.Items.Add("All Transactions");
            this.TransactionFilterTrans.Items.Add("Sales");
            this.TransactionFilterTrans.Items.Add("Purchased");
            this.InitializeTransFilter("All Transactions", this.particularFilterTrans, this.principalFilterTrans, this.salesmanFilterTrans);

            foreach (var dc in this.TransactionDataGrid.Columns)
            {
                if (dc.Header.ToString() == "Type" || dc.Header.ToString() == "Ref No." || dc.Header.ToString() == "Particular" || dc.Header.ToString() == "Salesman" ||
                    dc.Header.ToString() == "Principal List" || dc.Header.ToString() == "Prod Count" || dc.Header.ToString() == "Input Date")
                {
                    var firstOrDefault = this.TransactionDataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header);
                    if (firstOrDefault != null)
                    {
                        firstOrDefault.IsReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// The initialize transaction filter determines the products that will be printed in View depending on the current filter parameters
        /// </summary>
        /// <param name="newValueTransaction">
        /// The parameter for a new transaction value or default empty if user did not put any
        /// </param>
        /// <param name="newValueParticular">
        /// The parameter for a new particular value or default empty if user did not put any
        /// </param>
        /// <param name="newValuePrincipal">
        /// The parameter for a new principal value or default empty if user did not put any
        /// </param>
        /// <param name="newValueSalesman">
        /// The parameter for a new salesman value or default empty if user did not put any
        /// </param>
        public void InitializeTransFilter(
            string newValueTransaction = "",
            string newValueParticular = "",
            string newValuePrincipal = "",
            string newValueSalesman = "")
        {
            var listTrans = TransactionOrder.GetAllTransactions();

            this.ParticularFilterTrans.ItemsSource = null;
            this.ParticularFilterTrans.Items.Clear();
            this.ParticularFilterTrans.Items.Add("All Particular");
            this.PrincipalFilterTrans.ItemsSource = null;
            this.PrincipalFilterTrans.Items.Clear();
            this.PrincipalFilterTrans.Items.Add("All Principal");
            this.SalesmanFilterTrans.ItemsSource = null;
            this.SalesmanFilterTrans.Items.Clear();
            this.SalesmanFilterTrans.Items.Add("All Salesman");

            if (newValueTransaction != "All Transactions")
            {
                listTrans = listTrans.Where(q => q.TransactionType == newValueTransaction).ToList();
            }
             
            foreach (var prod in listTrans.Select(q => q.Particular).Distinct())
            {
                this.ParticularFilterTrans.Items.Add(prod);
            }

            if (newValueParticular != "All Particular")
            {
                listTrans = listTrans.Where(q => q.Particular == newValueParticular).ToList();
            }

            foreach (var prod in listTrans.SelectMany(q => q.Transactions).Select(s => s.Product.Principal).Distinct())
            {
                this.PrincipalFilterTrans.Items.Add(prod);
            }

            if (newValuePrincipal != "All Principal")
            {
                listTrans =
                    listTrans.Where(q => q.Transactions.Exists(s => s.Product.Principal == newValuePrincipal)).ToList();
            }

            foreach (var prod in listTrans.Select(q => q.SalesmanName).Distinct())
            {
                this.SalesmanFilterTrans.Items.Add(prod);
            }

            if (newValueSalesman != "All Salesman")
            {
                listTrans =
                    listTrans.Where(q => q.SalesmanName == newValueSalesman).ToList();
            }

            if (!string.IsNullOrWhiteSpace(this.startDate.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction >= this.startDate.SelectedDate).ToList();
            }

            if (!string.IsNullOrWhiteSpace(this.endDate.Text))
            {
                listTrans = listTrans.Where(q => q.DateTransaction <= this.endDate.SelectedDate).ToList();
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
                                   PrincipalList =
                                       string.Join(
                                           ",",
                                           tran.Transactions.Select(q => q.Product)
                                               .ToList()
                                               .Select(q => q.Principal)
                                               .Distinct()
                                               .ToList()),
                                   RefNo = tran.RefNo,
                                   ItemCount = tran.Transactions.Count,
                                   SalesmanName = tran.SalesmanName,
                                   Transactions = tran.Transactions
                               };
                transView.Add(temp);
            }

            this.TransactionDataGrid.ItemsSource = transView;
            this.transactionFilterTrans = newValueTransaction;
            this.particularFilterTrans = newValueParticular;
            this.principalFilterTrans = newValuePrincipal;
            this.salesmanFilterTrans = newValueSalesman;
            this.TransactionFilterTrans.SelectedIndex = this.TransactionFilterTrans.Items.IndexOf(newValueTransaction);
            this.ParticularFilterTrans.SelectedIndex = this.ParticularFilterTrans.Items.IndexOf(newValueParticular);
            this.PrincipalFilterTrans.SelectedIndex = this.PrincipalFilterTrans.Items.IndexOf(newValuePrincipal);
            this.SalesmanFilterTrans.SelectedIndex = this.SalesmanFilterTrans.Items.IndexOf(newValueSalesman);
        }

        /// <summary>
        /// The add sales click, from Transaction View will create a new window for CreateSalesPurchased
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void AddSalesClick(object sender, RoutedEventArgs e)
        {  
            CreateSalesPurchased sales = new CreateSalesPurchased("Sales");
            sales.Show();
        }

        /// <summary>
        /// The add purchased click, from Transaction View will create a new window for CreateSalesPurchased
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void AddPurchasedClick(object sender, RoutedEventArgs e)
        {  
            CreateSalesPurchased purchased = new CreateSalesPurchased("Purchased");
            purchased.Show();
        }

        /// <summary>
        /// The export transaction click, from Transaction View will create a window to export the invoice of a transaction 
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void ExportTransClick(object sender, RoutedEventArgs e)
        {
            var transView = ((FrameworkElement)sender).DataContext as TransactionView;
            var trans = TransactionOrder.GetTransaction(transView.Id);
            ExcelExport.ExcelInvoice.ExportTransactionInvoice(trans);
        }

        /// <summary>
        /// The delete transaction click, from Transaction View will delete the row of transaction with a message box before it goes through
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void DeleteTransClick(object sender, RoutedEventArgs e)
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
                TransactionOrder.DeleteTransaction(transView.Id);

            }

            this.InitializeTransView();
            this.InitializeProductsFilter(this.locationFilterProduct, this.principalFilterProduct, this.categoryFilterProduct);
        }

        /// <summary>
        /// The export group click, from Transaction View will create a new window for ExportExcel
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the button, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void ExportGroupClick(object sender, RoutedEventArgs e)
        {
            ExportExcel export = new ExportExcel("Transaction");
            export.Show();
        }

        #endregion

        /// <summary>
        /// The transaction filter transaction drop down closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void TransactionFilterTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransFilter(newValue.ToString(), this.particularFilterTrans, this.principalFilterTrans, this.salesmanFilterTrans);
            }
        }

        /// <summary>
        /// The particular filter trans drop down closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void ParticularFilterTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransFilter(this.transactionFilterTrans, newValue.ToString(), this.principalFilterTrans, this.salesmanFilterTrans);
            }
        }

        /// <summary>
        /// The principal filter trans drop down closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void PrincipalFilterTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransFilter(this.transactionFilterTrans, this.particularFilterTrans, newValue.ToString(), this.salesmanFilterTrans);
            }
        }

        /// <summary>
        /// The category filter trans drop down closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void SalesmanFilterTransDropDownClosed(object sender, EventArgs e)
        {
            var newValue = ((ComboBox)sender).SelectedItem;
            if (newValue != null)
            {
                this.InitializeTransFilter(this.transactionFilterTrans, this.particularFilterTrans, this.principalFilterTrans, newValue.ToString());
            }
        }

        /// <summary>
        /// The start date calendar closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void StartDateCalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = ((DatePicker)sender).Text;
            if (newValue != null)
            {
                this.InitializeTransFilter(this.transactionFilterTrans, this.particularFilterTrans, this.principalFilterProduct, this.salesmanFilterTrans);
            }
        }

        /// <summary>
        /// The end date calendar closed, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void EndDateCalendarClosed(object sender, RoutedEventArgs e)
        {
            var newValue = ((DatePicker)sender).Text;
            if (newValue != null)
            {
                this.InitializeTransFilter(this.transactionFilterTrans, this.particularFilterTrans, this.principalFilterProduct, this.salesmanFilterTrans);
            }
        }

        /// <summary>
        /// The details transaction click, from Transaction View will check if a new value is selected and will initialize again
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void DetailsTransClick(object sender, RoutedEventArgs e)
        {
            var transView = ((FrameworkElement)sender).DataContext as TransactionView;
            Details details = new Details(TransactionOrder.GetTransaction(transView.Id));
            details.Show();
        }

        /// <summary>
        /// The Settings click, from Transaction View will create a new window for Settings
        /// </summary>
        /// <param name="sender">
        /// The sender is the parent object of the combo box, which is the Data Grid 
        /// </param>
        /// <param name="e">
        /// The e is the event arguments if applicable
        /// </param>
        private void SettingClick(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        } 
    }
}
