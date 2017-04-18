using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    using Stockbook.Class;

    public class TransactionView
    {
        public string Id { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime DateTransaction { get; set; }
        public string Particular { get; set; }
        public string SalesmanName { get; set; }
        public string RefNo { get; set; }
        public string TransactionType { get; set; }
        public string PrincipalList { get; set; }
        public decimal ItemCount { get; set; }
    }
}