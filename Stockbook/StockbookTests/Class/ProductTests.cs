// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductTests.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   Unit Test (NUnit) for Product Class in StockBook project. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StockbookTests.Class
{
    using NUnit.Framework;

    using Stockbook.Class;
    using Stockbook.Model;

    /// <summary>
    /// Unit Test (NUnit) for Product Class in StockBook project. 
    /// </summary>
    [TestFixture]
    public class ProductTests
    {
        /// <summary>
        /// Testing CreateProduct() for creating a product and asserting by getting the product and check if equal
        /// </summary>
        [Test]
        public void CreateProductTest()
        {
            var expectedResult = new Product { Name = "CreateProductTest" };
            Product.CreateProduct(expectedResult);
            var actualResult = Product.GetProduct(expectedResult.Id);
            actualResult.CaseBalance = 0;
            UnitTestingHelper.AssertPublicPropertiesEqual(expectedResult, actualResult);
            Product.DeleteProduct(expectedResult.Id);
        }

        /// <summary>
        /// Testing GetProduct() for getting a product and asserting by checking if it exist 
        /// </summary>
        [Test]
        public void GetProductTest()
        {
            var expectedResult = new Product { Name = "GetProductTest" };
            Product.CreateProduct(expectedResult);
            var actualResult = Product.GetProduct(expectedResult.Id);
            actualResult.CaseBalance = 0;
            UnitTestingHelper.AssertPublicPropertiesEqual(expectedResult, actualResult);

            if (Product.GetProduct("DoesNotExist") != null)
            {
                Assert.Fail("Product should not exist and must return null");
            }

            if (Product.GetProduct(expectedResult.Id + " ") != null)
            {
                Assert.Fail("Product must return null since the id is modified");
            }

            Product.DeleteProduct(expectedResult.Id);
        }

        /// <summary>
        /// Testing DeleteProduct() for deleting a product and asserting if it still exist
        /// </summary>
        [Test]
        public void DeleteProductTest()
        {
            var expectedResult = new Product { Name = "DeleteProductTest" };
            Product.CreateProduct(expectedResult);
            Product.DeleteProduct(expectedResult.Id);
            if (Product.GetProduct(expectedResult.Id) != null)
            {
                Assert.Fail("Product should not exist and must return null");
            }

            Assert.Pass();
        }

        /// <summary>
        /// Testing EditProduct for editing the product and asserting if changes are saved
        /// </summary>
        [Test]
        public void EditProductTest()
        {
            var actualResult = new Product { Name = "EditProductTest" };
            Product.CreateProduct(actualResult);
            actualResult.CaseBalance = 12;
            Product.EditProduct(actualResult);
            var expectedResult = 12;
            Assert.AreEqual(expectedResult, actualResult.CaseBalance);

            var temp = actualResult.Id;
            actualResult.Id += "fail";
            if (Product.EditProduct(actualResult))
            {
                Assert.Fail();
            }

            actualResult.Id = temp;
            Product.DeleteProduct(actualResult.Id);
        }

        /// <summary>
        /// Testing GetAllProducts for getting all the products and asserting if all products are got
        /// </summary>
        [Test]
        public void GetAllProductsTest()
        {
            var actualResult = new Product { Name = "GetAllProductsTest" };
            Product.CreateProduct(actualResult);
            actualResult = new Product { Name = "GetAllProductsTest" };
            Product.CreateProduct(actualResult);
            actualResult = new Product { Name = "GetAllProductsTest" };
            Product.CreateProduct(actualResult);
            actualResult = new Product { Name = "GetAllProductsTest" };
            Product.CreateProduct(actualResult);
            actualResult = new Product { Name = "GetAllProductsTest" };
            Product.CreateProduct(actualResult);

            var prods = Product.GetAllProducts();
            var expectedResult = 5;
            Assert.AreEqual(expectedResult, prods.Count);

            foreach (var temp in prods)
            {
                Product.DeleteProduct(temp.Id);
            }
        }
    }
}