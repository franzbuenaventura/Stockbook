using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Stockbook.Class;
using Stockbook.Model;
using Stockbook.Windows;

namespace Stockbook.Products
{
    /// <summary>
    /// Interaction logic for CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : MetroWindow
    {
        public CreateProduct()
        {
            InitializeComponent();
            InitializeCreate();
        }

        private void InitializeCreate()
        {
            var listProducts = Product.GetAllProducts();
           foreach (var item in listProducts.Select(q=>q.Location).Distinct())
            {
                LocationInput.Items.Add(item);
            } 
        }

        private void SubmitCreate_Click(object sender, RoutedEventArgs e)
        {
            decimal Case, pack, piece,casePacks,packPieces;
            if (decimal.TryParse(CaseInput.Text.Trim(), out Case) && decimal.TryParse(CaseToPack.Text.Trim(), out casePacks) && decimal.TryParse(PackToPiece.Text.Trim(), out packPieces) && decimal.TryParse(PackInput.Text.Trim(), out pack) && decimal.TryParse(PieceInput.Text.Trim(), out piece) && !string.IsNullOrWhiteSpace(LocationInput.Text) && !string.IsNullOrWhiteSpace(PrincipalInput.Text) && !string.IsNullOrWhiteSpace(CategoryInput.Text) && !string.IsNullOrWhiteSpace(NameInput.Text) && !string.IsNullOrWhiteSpace(CodeInput.Text))
            {
                var prod = new Product
                {
                    Id =  "",
                    Location = LocationInput.Text.Trim(),
                    Category = CategoryInput.Text.Trim(),
                    Principal = PrincipalInput.Text.Trim(),
                    Name = NameInput.Text.Trim(),
                    ProdCode = CodeInput.Text.Trim(),
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
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Title == "Main")
                {
                    (window as MainMetro).InitializeProducts();
                }
                }
                Close();
            }
            else
            {
                
            string errorMessage = "";
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
            else if (!decimal.TryParse(PackToPiece.Text.Trim(), out piece))
            {
                errorMessage = "Pack To Piece Input has invalid characters or no value was given";
            }
            else if (!decimal.TryParse(CaseToPack.Text.Trim(), out piece))
            {
                errorMessage = "Case To Pack Input has invalid characters or no value was given";
            }
            else if (string.IsNullOrWhiteSpace(LocationInput.Text))
            {
                errorMessage = "No value was given in Location Input";
            }
            else if (string.IsNullOrWhiteSpace(PrincipalInput.Text))
            {
                errorMessage = "No value was given in Principal Input";
            }
            else if (string.IsNullOrWhiteSpace(CategoryInput.Text))
            {
                errorMessage = "No value was given in Category Input";
            }
            else if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                errorMessage = "No value was given in Name Input";
            }
            else if (string.IsNullOrWhiteSpace(CodeInput.Text))
            {
                errorMessage = "No value was given in Code Input";
            } 
            MessageBox.Show("Error: " + errorMessage, "Error in Creating Transaction", MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
        }

        private void LocationInput_DropDownClosed(object sender, System.EventArgs e)
        { 
            if ((sender as ComboBox).SelectedItem != null)
            { 
                    var location = (sender as ComboBox).SelectedItem.ToString();
                    var listProducts = Product.GetAllProducts();
                    PrincipalInput.ItemsSource = null;
                    PrincipalInput.Items.Clear();
                    CategoryInput.ItemsSource = null;
                    CategoryInput.Items.Clear();
                PrincipalInput.ItemsSource = listProducts.Where(q => q.Location == location).Select(q => q.Principal).Distinct();
             }
        }

        private void PrincipalInput_DropDownClosed(object sender, System.EventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                var principal = (sender as ComboBox).SelectedItem.ToString();
                var listProducts = Product.GetAllProducts();
                CategoryInput.ItemsSource = null;
                CategoryInput.Items.Clear();
                CategoryInput.ItemsSource = listProducts.Where(q => q.Principal == principal).Select(q => q.Category).Distinct();
            }
        }

        private void CategoryInput_DropDownClosed(object sender, System.EventArgs e)
        {

        }
         
    }
}
