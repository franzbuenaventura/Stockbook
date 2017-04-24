// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransactionOrderTests.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
// <summary>
//   Unit Test (NUnit) for Transaction Order Class in StockBook project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StockbookTests.Class
{
    using System;

    using NUnit.Framework;

    using Stockbook.Class;

    /// <summary>
    /// The transaction order tests.
    /// </summary>
    [TestFixture]
    public class TransactionOrderTests
    {
        /// <summary>
        /// Testing CreateTransaction() for creating a Transaction Order and asserting by getting the product and check if equal
        /// </summary>
        [Test]
        public void CreateTransactionTest()
        {
            var actualResult = new TransactionOrder
                                   {
                                       TransactionType = "Sales",
                                       DateTransaction = DateTime.Now,
                                       RefNo = "TestRef"
                                   };
            actualResult = TransactionOrder.CreateTransaction(actualResult);
            var expectedResult = TransactionOrder.GetTransaction(actualResult.Id);

            UnitTestingHelper.AssertPublicPropertiesEqual(actualResult, expectedResult);
            TransactionOrder.DeleteTransaction(actualResult.Id);
        }

        /// <summary>
        /// Testing GetTransaction() for getting a transaction and asserting by checking if it exist 
        /// </summary>
        [Test]
        public void GetTransactionTest()
        {
            var actualResult = new TransactionOrder
            {
                TransactionType = "Sales",
                DateTransaction = DateTime.Now,
                RefNo = "TestRef"
            };

            actualResult = TransactionOrder.CreateTransaction(actualResult);

            var expectedResult = TransactionOrder.GetTransaction(actualResult.Id);
            UnitTestingHelper.AssertPublicPropertiesEqual(actualResult, expectedResult);

            if (TransactionOrder.GetTransaction("DoesNotExist") != null)
            {
                Assert.Fail("Transaction Order should not exist and must return null");
            }

            if (TransactionOrder.GetTransaction(actualResult.Id + " ") != null)
            {
                Assert.Fail("Transaction Order must return null since the id is modified");
            }

            TransactionOrder.DeleteTransaction(actualResult.Id);
        }

        /// <summary>
        /// The delete transaction test.
        /// </summary>
        [Test]
        public void DeleteTransactionTest()
        {

            var actualResult = new TransactionOrder
            {
                TransactionType = "Sales",
                DateTransaction = DateTime.Now,
                RefNo = "TestRef"
            };
            actualResult = TransactionOrder.CreateTransaction(actualResult);
        
            TransactionOrder.DeleteTransaction(actualResult.Id);

            var expectedResult = TransactionOrder.GetTransaction(actualResult.Id);

            Assert.IsNull(expectedResult);

        }

        /// <summary>
        /// The get all transactions test.
        /// </summary>
        [Test]
        public void GetAllTransactionsTest()
        {
                for (var i = 0; i < 5; i++)
                {
                    var actualResult = new TransactionOrder
                    {
                        TransactionType = "Sales",
                        DateTransaction = DateTime.Now,
                        RefNo = "TestRef"
                    };
                    TransactionOrder.CreateTransaction(actualResult);
                }

                var trans = TransactionOrder.GetAllTransactions();
                var expectedResult = 5;
                Assert.AreEqual(expectedResult, trans.Count);

                foreach (var temp in trans)
                {
                    TransactionOrder.DeleteTransaction(temp.Id);
                }
        }

        /// <summary>
        /// The edit transaction test.
        /// </summary>
        [Test]
        public void EditTransactionTest()
        {
            var actualResult = new TransactionOrder
            {
                TransactionType = "Sales",
                DateTransaction = DateTime.Now,
                RefNo = "TestRef"
            };
            actualResult = TransactionOrder.CreateTransaction(actualResult);
            actualResult.RefNo = "Change Ref No";
            TransactionOrder.EditTransaction(actualResult);
            var expectedResult = TransactionOrder.GetTransaction(actualResult.Id);
            Assert.AreEqual(actualResult.RefNo, expectedResult.RefNo);

            TransactionOrder.DeleteTransaction(actualResult.Id);
        }


        /// <summary>
        /// The delete all transactions test.
        /// </summary>
        [Test]
        public void DeleteAllTransactionTest()
        {
            for (var i = 0; i < 5; i++)
            {
                var actualResult = new TransactionOrder
                {
                    TransactionType = "Sales",
                    DateTransaction = DateTime.Now,
                    RefNo = "TestRef"
                };
                TransactionOrder.CreateTransaction(actualResult);
            }

            var trans = TransactionOrder.GetAllTransactions();
            var expectedResult = TransactionOrder.DeleteAllTransactions();
            Assert.AreEqual(expectedResult, trans.Count);
        }
    }
}