// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProductHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Class
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using Stockbook.Model;

    public class ProductHelper
    {
        private static string ProductFolder()
        {
            var temp = Environment.CurrentDirectory + @"\ProductsDb\";
            Directory.CreateDirectory(temp);
            return temp;
        }

        public static void CreateProduct(Product prod)
        {
            string tempName = prod.Id + " - " + prod.Name.Replace(".", "").Replace("/", " ");
            string fileName = ProductFolder() + tempName + @".json";
            prod.Id = tempName;
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

                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        public int GenerateProductId()
        {
            int id = 1;
            string fileName = ProductFolder() + "IdCounter" + @".json";
            try
            {
                // Check if file already exists. If yes 
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            id = int.Parse(s);
                        }
                    }
                }
                //New File 
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(id + 1);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            return id;
        }
        public Product GetProduct(string fileName)
        {
            var sI = new Product();
            fileName = ProductFolder() + fileName + @".json";
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    return JsonConvert.DeserializeObject<Product>(s);
                }
            }

            return sI;
        }
        public void DeleteProduct(string fileName)
        {
            var temp = ProductFolder() + fileName + @".json";
            if (File.Exists(temp))
            {
                try
                {
                    File.Delete(temp);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        public List<Product> GetAllProducts()
        {
            var productList = new List<Product>();
            string[] listFileLoc = Directory.GetFiles(ProductFolder());
            foreach (var fileLoc in listFileLoc)
            {
                using (StreamReader sr = File.OpenText(fileLoc))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (!fileLoc.Contains("IdCounter"))
                        {
                            productList.Add(JsonConvert.DeserializeObject<Product>(s));
                        }
                    }
                }
            }
            return productList;
        }
        public void EditProduct(Product product)
        {
            DeleteProduct(product.Id);
            string fileName = ProductFolder() + product.Id + @".json";
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
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
