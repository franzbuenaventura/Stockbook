// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductTests.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
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
            var actualResult = new Product { Name = "CreateProductTest" };
            Product.CreateProduct(actualResult);
            var expectedResult = Product.GetProduct(actualResult.Id);
            UnitTestingHelper.AssertPublicPropertiesEqual(actualResult, expectedResult);
            Product.DeleteProduct(actualResult.Id);
        }

        /// <summary>
        /// Testing GetProduct() for getting a product and asserting by checking if it exist 
        /// </summary>
        [Test]
        public void GetProductTest()
        {
            var actualResult = new Product { Name = "GetProductTest" };
            Product.CreateProduct(actualResult);
            var expectedResult = Product.GetProduct(actualResult.Id);
            UnitTestingHelper.AssertPublicPropertiesEqual(actualResult, expectedResult);
            
            if (Product.GetProduct("DoesNotExist") != null)
            {
                Assert.Fail("Product should not exist and must return null");
            }

            if (Product.GetProduct(actualResult.Id + " ") != null)
            {
                Assert.Fail("Product must return null since the id is modified");
            }

            Product.DeleteProduct(actualResult.Id);
        }

        /// <summary>
        /// Testing DeleteProduct() for deleting a product and asserting if it still exist
        /// </summary>
        [Test]
        public void DeleteProductTest()
        {
            var actualResult = new Product { Name = "DeleteProductTest" };
            Product.CreateProduct(actualResult);
            Product.DeleteProduct(actualResult.Id);
            if (Product.GetProduct(actualResult.Id) != null)
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
            for (var i = 0; i < 5; i++)
            {
                var actualResult = new Product { Name = "GetAllProductsTest" };
                Product.CreateProduct(actualResult);
            }

            var prods = Product.GetAllProducts();
            var expectedResult = 5;
            Assert.AreEqual(expectedResult, prods.Count);

            foreach (var temp in prods)
            {
                Product.DeleteProduct(temp.Id);
            }
        }

        /// <summary>
        /// The balance case pack piece test on a Sales/Purchased transaction.
        /// </summary>
        [Test]
        public void BalanceCasePackPieceTest()
        {
            var testProduct = new Product
                                  {
                                      Name = "BalanceCasePackPieceTest",
                                      CaseBalance = 10,
                                      PackBalance = 10,
                                      PieceBalance = 10,
                                      CaseToPacks = 5,
                                      PackToPieces = 5
                                  };
            testProduct = Product.CreateProduct(testProduct);

            var testTransaction = new Transaction { CaseTransact = 1, PackTransact = 20, PieceTransact = 20 };

            var actualResult = Product.BalanceCasePackPiece(testTransaction, testProduct, "Purchased");
            var expectedResult = new Product { CaseBalance = 18, PackBalance = 1, PieceBalance = 0 };

            Assert.AreEqual(actualResult.CaseBalance, expectedResult.CaseBalance);
            Assert.AreEqual(actualResult.PackBalance, expectedResult.PackBalance);
            Assert.AreEqual(actualResult.PieceBalance, expectedResult.PieceBalance);


            actualResult = Product.BalanceCasePackPiece(testTransaction, testProduct, "Sales");
            expectedResult = new Product { CaseBalance = 12, PackBalance = 2, PieceBalance = 0 };

            Assert.AreEqual(actualResult.CaseBalance, expectedResult.CaseBalance);
            Assert.AreEqual(actualResult.PackBalance, expectedResult.PackBalance);
            Assert.AreEqual(actualResult.PieceBalance, expectedResult.PieceBalance);

            Product.DeleteProduct(testProduct.Id);
        }

        /// <summary>
        /// Testing delete all products in database
        /// </summary>
        [Test]
        public void DeleteAllProductsTest()
        {
            for (var i = 0; i < 5; i++)
            {
                var actualResult = new Product { Name = "GetAllProductsTest" };
                Product.CreateProduct(actualResult);
            }

            var prods = Product.GetAllProducts();
            var expectedResult = Product.DeleteAllProducts();
            Assert.AreEqual(expectedResult, prods.Count);
        }
    }
}