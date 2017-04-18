// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transaction.cs" company="Franz Buenaventura">
//   Author: Franz Justin Buenaventura
//   Website: www.franzbuenaventura.com 
//   License: GNU Affero General Public License v3.0
// </copyright>
//  
// <summary>
//   The Transaction Class that contains all the details about a transaction (Sales or Purchased) in StockBook project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Class
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Documents;

    using Newtonsoft.Json;

    using Stockbook.Model;

    /// <summary>
    /// The transaction helper.
    /// </summary>
    public class TransactionOrder
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public List<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the date transaction.
        /// </summary>
        public DateTime DateTransaction { get; set; }

        /// <summary>
        /// Gets or sets the particular.
        /// </summary>
        public string Particular { get; set; }

        /// <summary>
        /// Gets or sets the ref no.
        /// </summary>
        public string RefNo { get; set; }

        /// <summary>
        /// Gets or sets the salesman name.
        /// </summary>
        public string SalesmanName { get; set; }

        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the particular address.
        /// </summary>
        public string ParticularAddress { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        public decimal Terms { get; set; }

        /// <summary>
        /// Create a transaction order and store it in a JSON file on Transaction Database Folder
        /// </summary>
        /// <param name="transactionOrder">
        /// This is the transaction order that will be created and stored in the database
        /// </param>
        public static void CreateTransaction(TransactionOrder transactionOrder)
        {
            var id = GenerateTransactionId() + " - " + transactionOrder.TransactionType.Replace(".", string.Empty).Replace("/", " ") + " - " + transactionOrder.DateTransaction.ToShortDateString().Replace(".", string.Empty).Replace("/", " ");
            var path = TransactionFolder() + id + @".json";
            transactionOrder.Id = id;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(transactionOrder));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Getting the transaction by the transaction order Id
        /// </summary>
        /// <param name="transactionId">
        /// The transaction order Id of the transaction order that would be received 
        /// </param>
        /// <returns>
        /// Returns the <see cref="TransactionOrder"/> with the same transaction Id.
        /// </returns>
        public static TransactionOrder GetTransaction(string transactionId)
        {
            var temp = TransactionFolder() + transactionId + @".json";
            using (StreamReader sr = File.OpenText(temp))
            {
                string transaction;
                while ((transaction = sr.ReadLine()) != null)
                {
                    return JsonConvert.DeserializeObject<TransactionOrder>(transaction);
                }
            }

            return null;
        }

        /// <summary>
        /// Deleting a transaction by the transaction order Id in the database
        /// </summary>
        /// <param name="transactionId">
        /// The transaction Id of the transaction order that would be deleted 
        /// </param>
        /// <returns>
        /// It will return <see cref="bool"/>, true if its successful and false otherwise.
        /// </returns>
        public static bool DeleteTransaction(string transactionId)
        {
            var temp = TransactionFolder() + transactionId + @".json";
            if (File.Exists(temp))
            {
                try
                {
                    File.Delete(temp);
                    return true;
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieve all the transaction orders in the transaction Database
        /// </summary>
        /// <returns>
        /// The List of transaction orders.
        /// </returns>
        public static List<TransactionOrder> GetAllTransactions()
        {
            var transactionList = new List<TransactionOrder>();
            string[] listFileLoc = Directory.GetFiles(TransactionFolder());
            foreach (var fileLoc in listFileLoc)
            {
                using (StreamReader sr = File.OpenText(fileLoc))
                {
                    string value;
                    while ((value = sr.ReadLine()) != null)
                    {
                        if (!fileLoc.Contains("IdCounter"))
                        {
                            transactionList.Add(JsonConvert.DeserializeObject<TransactionOrder>(value));
                        }
                    }
                }
            }
            return transactionList;
        }

        /// <summary>
        /// Editing the transaction order by the transaction order id in the database
        /// </summary>
        /// <param name="transactionOrder">
        /// The modified transaction that would be replaced, can't change transaction order id 
        /// </param>
        /// <returns>
        /// It will return <see cref="bool"/>, true if its successful and false otherwise.
        /// </returns>
        public bool EditTransaction(TransactionOrder transactionOrder)
        {
            DeleteTransaction(transactionOrder.Id);
            string fileName = TransactionFolder() + transactionOrder.Id + @".json";
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(transactionOrder));
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
        /// Returns the absolute path of transaction order folder and if it does not exist it will create a directory
        /// </summary>
        /// <returns>
        /// The Absolute Path for the transactions order database directory 
        /// </returns>
        private static string TransactionFolder()
        {
            var temp = Environment.CurrentDirectory + @"\TransactionsDb\";
            Directory.CreateDirectory(temp);
            return temp;
        }

        /// <summary>
        /// Generate the transaction id (An index starting from 1) and create a file IdCounter.JSON if does not exist 
        /// </summary>
        /// <returns>
        /// The generated transaction Id which is an increment of the previous id
        /// </returns>
        private static int GenerateTransactionId()
        {
            var id = 1;
            var fileName = TransactionFolder() + "IdCounter" + @".json";
            try
            {
                // Check if file already exists. If yes 
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            id = int.Parse(line);
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