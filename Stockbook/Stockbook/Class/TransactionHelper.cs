using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Stockbook.Model;

namespace Stockbook.Class
{
    public class TransactionHelper
    {

        private string TransactionFolder()
        {
            var temp = Environment.CurrentDirectory + @"\TransactionsDb\";
            Directory.CreateDirectory(temp);
            return temp;
        }
        public void CreateTransaction(TransactionOrder trans)
        {
            string tempName = trans.Id + " - " + trans.TransactionType.Replace(".", "").Replace("/", " ") + " - " + trans.DateTransaction.ToShortDateString().Replace(".", "").Replace("/", " ");
            string fileName = TransactionFolder() + tempName + @".json";
            trans.Id = tempName;
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(trans));
                }

                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
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
        public int GenerateTransactionId()
        {
            int id = 1;
            string fileName = TransactionFolder() + "IdCounter" + @".json";
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
        public TransactionOrder GetTransaction(string fileName)
        {
            var sI = new TransactionOrder();
            fileName = TransactionFolder() + fileName + @".json";
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    return JsonConvert.DeserializeObject<TransactionOrder>(s);
                }
            }

            return sI;
        }
        public void DeleteTransaction(string fileName)
        {
            var temp = TransactionFolder() + fileName + @".json";
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
        public List<TransactionOrder> GetAllTransactions()
        {
            var transactionList = new List<TransactionOrder>();
            string[] listFileLoc = Directory.GetFiles(TransactionFolder());
            foreach (var fileLoc in listFileLoc)
            {
                using (StreamReader sr = File.OpenText(fileLoc))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (!fileLoc.Contains("IdCounter"))
                        {
                            transactionList.Add(JsonConvert.DeserializeObject<TransactionOrder>(s));
                        }
                    }
                }
            }
            return transactionList;
        }
        public void EditTransaction(TransactionOrder transaction)
        {
            DeleteTransaction(transaction.Id);
            string fileName = TransactionFolder() + transaction.Id + @".json";
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(transaction));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}