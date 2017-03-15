using System;

namespace Stockbook.Model
{
    public class StockCardTransaction
    {
        public DateTime Date { get; set; }
        public string RefNo { get; set; }
        public string Transaction { get; set; }
        public decimal Case { get; set; }
        public decimal Pack { get; set; }
        public decimal Piece { get; set; }
        public decimal CaseBalance { get; set; }
        public decimal PackBalance { get; set; }
        public decimal PieceBalance { get; set; }
        public string Particular { get; set; }
    }
}