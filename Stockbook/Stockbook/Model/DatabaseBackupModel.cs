using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    using Stockbook.Class;

    public class DatabaseBackupModel
    {
        public DateTime Date { get; set; }
        public List<TransactionOrder> TransactionOrders { get; set; }
        public List<Product> Products { get; set; }
        public int ProductIdCounter { get; set; }
        public int TransactionIdCounter { get; set; }
    }
}