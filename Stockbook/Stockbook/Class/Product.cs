// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The Product Class that contains all the details about a product in StockBook project. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Class
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using Model;

    using Newtonsoft.Json;

    /// <summary>
    ///  The Product Class that contains all the details about a product in StockBook project. 
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the principal.
        /// </summary>
        public string Principal { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        public string ProdCode { get; set; }

        /// <summary>
        /// Gets or sets the case value.
        /// </summary>
        public decimal CaseValue { get; set; }

        /// <summary>
        /// Gets or sets the pack value.
        /// </summary>
        public decimal PackValue { get; set; }

        /// <summary>
        /// Gets or sets the piece value.
        /// </summary>
        public decimal PieceValue { get; set; }

        /// <summary>
        /// Gets or sets the case balance.
        /// </summary>
        public decimal CaseBalance { get; set; }

        /// <summary>
        /// Gets or sets the pack balance.
        /// </summary>
        public decimal PackBalance { get; set; }

        /// <summary>
        /// Gets or sets the piece balance.
        /// </summary>
        public decimal PieceBalance { get; set; }

        /// <summary>
        /// Gets or sets the case to packs.
        /// </summary>
        public decimal CaseToPacks { get; set; }

        /// <summary>
        /// Gets or sets the pack to pieces.
        /// </summary>
        public decimal PackToPieces { get; set; }

        /// <summary>
        /// Create a product and store it in a JSON file on Product Database Folder
        /// </summary>
        /// <param name="prod">
        /// This is the product that will be created and stored in the database
        /// </param>
        /// <returns>
        /// The <see cref="Product"/> that was just been created.
        /// </returns>
        public static Product CreateProduct(Product prod)
        {
            // The final id of the product would be the Generated Product ID and Product name (Example: 1 - Koolaid)
            var tempName = GenerateProductId() + " - " + prod.Name.Replace(".", string.Empty).Replace("/", " ");
            prod.Id = tempName;

            var fileName = ProductFolder() + tempName + @".json";
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(prod));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return prod;
        }

        /// <summary>
        /// Getting the Product by the Product Id
        /// </summary>
        /// <param name="productId">
        /// The product Id of the Product that would be received 
        /// </param>
        /// <returns>
        /// Returns the <see cref="Product"/> with the same Product Id.
        /// </returns>
        public static Product GetProduct(string productId)
        {
            var product = new Product();
            productId = ProductFolder() + productId + @".json";
            try
            {
                using (StreamReader sr = File.OpenText(productId))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        return JsonConvert.DeserializeObject<Product>(s);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e);
                return null;
            } 

            return product;
        }


        /// <summary>
        /// Deleting the Product by the Product Id in the database
        /// </summary>
        /// <param name="productId">
        /// The product Id of the Product that would be deleted 
        /// </param>
        /// <returns>
        /// It will return <see cref="bool"/>, true if its successful and false otherwise.
        /// </returns>
        public static bool DeleteProduct(string productId)
        {
            var temp = ProductFolder() + productId + @".json";
            if (File.Exists(temp))
            {
                try
                {
                    File.Delete(temp);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            return false;
        }


        /// <summary>
        /// Deletes all the product stored in the database
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> number of deleted items.
        /// </returns>
        public static int DeleteAllProducts()
        {
            var prods = GetAllProducts();
            var count = 0;
            foreach (var prod in prods)
            {
                DeleteProduct(prod.Id);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Retrieve all the products in the Product Database
        /// </summary>
        /// <returns>
        /// The List of Products.
        /// </returns>
        public static List<Product> GetAllProducts()
        {
            var productList = new List<Product>();
            var listFileLoc = Directory.GetFiles(ProductFolder());
            foreach (var fileLoc in listFileLoc)
            {
                using (StreamReader sr = File.OpenText(fileLoc))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Get all file names in the Product Db except for ID Counter file
                        if (!fileLoc.Contains("IdCounter"))
                        {
                            productList.Add(JsonConvert.DeserializeObject<Product>(line));
                        }
                    }
                }
            }

            return productList;
        }

        /// <summary>
        /// Editing the Product by the Product Id in the database
        /// </summary>
        /// <param name="product">
        /// The modified product that would be replaced, can't change Product Id 
        /// </param>
        /// <returns>
        /// It will return <see cref="bool"/>, true if its successful and false otherwise.
        /// </returns>
        public static bool EditProduct(Product product)
        {
            if (!DeleteProduct(product.Id))
            {
                return false;
            }

            var fileName = ProductFolder() + product.Id + @".json";
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(product));
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return false;
        }

        /// <summary>
        /// The method updates the case/pack/piece of a stock after a transaction is made and returns the resulting product
        /// </summary>
        /// <param name="transaction">
        /// The transaction that will modify the product stock balance
        /// </param>
        /// <param name="product">
        /// The product that will be modified by transaction
        /// </param>
        /// <param name="transactionType">
        /// The type of transaction to either add or subtract the balance of stock
        /// </param>
        /// <returns>
        /// The <see cref="Product"/> that has been modified by transaction and got a balance case/pack/piece.
        /// </returns>
        public static Product BalanceCasePackPiece(Transaction transaction, Product product, string transactionType = "Sales")
        {
            if (product.PackToPieces > 0 && product.CaseToPacks > 0)
            {
                var tempTransaction = (transaction.CaseTransact * product.CaseToPacks + transaction.PackTransact) * product.PackToPieces + transaction.PieceTransact;
                var tempTotalBalance = (product.CaseBalance * product.CaseToPacks + product.PackBalance) * product.PackToPieces + product.PieceBalance;
                decimal finalBalance = 0;

                switch (transactionType)
                {
                    case "Sales":
                        {
                             finalBalance = tempTotalBalance - tempTransaction;
                        }

                        break;
                    case "Purchased":
                        {
                             finalBalance = tempTotalBalance + tempTransaction;
                        }

                        break;
                }

                product.PieceBalance = finalBalance % product.PackToPieces;
                product.PackBalance = Math.Truncate(finalBalance / product.PackToPieces);
                product.CaseBalance = Math.Truncate(product.PackBalance / product.CaseToPacks);
                product.PackBalance = product.PackBalance % product.CaseToPacks;

            }
            else
            {
                switch (transactionType)
                {
                    case "Sales":
                        product.CaseBalance -= transaction.CaseTransact;
                        product.PackBalance -= transaction.PackTransact;
                        product.PieceBalance -= transaction.PieceTransact;
                        break;
                    case "Purchased":
                        product.CaseBalance += transaction.CaseTransact;
                        product.PackBalance += transaction.PackTransact;
                        product.PieceBalance += transaction.PieceTransact;
                        break;
                }
            }

            return product;
        }


        /// <summary>
        /// Returns the absolute path of product folder and if it does not exist it will create a directory
        /// </summary>
        /// <returns>
        /// The Absolute Path for the products database directory 
        /// </returns>
        private static string ProductFolder()
        {
            var absolutePath = Environment.CurrentDirectory + @"\ProductsDb\";
            Directory.CreateDirectory(absolutePath);
            return absolutePath;
        }

        /// <summary>
        /// Generate the product id (An index starting from 1) and create a file IdCounter.JSON if does not exist 
        /// </summary>
        /// <returns>
        /// The generated product Id which is an increment of the previous id
        /// </returns>
        private static int GenerateProductId()
        {
            var id = 1;
            var fileName = ProductFolder() + "IdCounter" + @".json";
            try
            {
                // Check if file already exists. If yes then get the id and add increment of one.
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string textId;
                        while ((textId = sr.ReadLine()) != null)
                        {
                            id = int.Parse(textId);
                        }
                    }
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(id + 1);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return id;
        }
    }
}
