using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using Stockbook.Model;

namespace Stockbook.Windows
{
    using Stockbook.Class;

    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : MetroWindow
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
