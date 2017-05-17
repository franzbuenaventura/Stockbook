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

    }
}