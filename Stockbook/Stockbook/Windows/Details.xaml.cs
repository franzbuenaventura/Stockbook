// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Details.xaml.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The Details Class that contains the backend for Details Window
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Windows
{
    using System.Linq;

    using Class;

    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for Details
    /// </summary>
    public partial class Details : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Details"/> class.
        /// </summary>
        /// <param name="order">
        /// The transaction order that will be displayed in the window
        /// </param>
        public Details(TransactionOrder order)
        {
            this.InitializeComponent();

            var title = "Details - Ref. No: " + order.RefNo + " - " + order.TransactionType;

            this.Head.Title = title;

            foreach (var trans in order.Transactions.OrderBy(q => q.Product.Name))
            {
                this.dataGrid.Items.Add(trans);
            }

            foreach (var dc in dataGrid.Columns)
            {
                if (dc.Header.ToString() == "Case" || dc.Header.ToString() == "Pack" || dc.Header.ToString() == "Piece" || dc.Header.ToString() == "Product Name")
                {
                    dataGrid.Columns.FirstOrDefault(q => q.Header == dc.Header).IsReadOnly = true;
                }
            }

            this.TitleLabel.Content = title;
        }
    }
}
