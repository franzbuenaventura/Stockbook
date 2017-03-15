using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    public class StockCard
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ProdCode { get; set; }
        public decimal CaseBalance { get; set; }
        public decimal PackBalance{ get; set; }
        public decimal PieceBalance{ get; set; }
        public string Location { get; set; }
        public  List<StockCardTransaction> ListTransactions { get; set; }
    }
}