using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    public class TransactionOrder
    {
        public string Id { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime DateTransaction { get; set; }
        public string Particular { get; set; }
        public string RefNo { get; set; }
        public string SalesmanName { get; set; }
        public string TransactionType { get; set; }
        public string ParticularAddress { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Terms { get; set; }
    }
}