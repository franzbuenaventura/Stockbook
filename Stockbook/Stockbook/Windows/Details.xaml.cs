using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Stockbook.Model;

namespace Stockbook.Windows
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        public Details(TransactionOrder order)
        {
            InitializeComponent();
            Head.Title = "Details - Ref. No: " + order.RefNo + " - " + order.TransactionType;
            foreach (var trans in order.Transactions.OrderBy(q=>q.Product.Name))
            {
                dataGrid.Items.Add(trans);
            }
            foreach (var dc in dataGrid.Columns)
            {
                if (dc.Header.ToString() == "Case" || dc.Header.ToString() == "Pack" || dc.Header.ToString() == "Piece" || dc.Header.ToString() == "Product Name")
                {
                    dataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }
        }
    }
}
