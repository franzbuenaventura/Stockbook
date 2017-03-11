using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    public class Product
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Principal { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string ProdCode { get; set; }
        public decimal CaseValue { get; set; }
        public decimal PackValue { get; set; }
        public decimal PieceValue { get; set; }
        public decimal CaseBalance { get; set; }
        public decimal PackBalance { get; set; }
        public decimal PieceBalance { get; set; }
        public decimal CaseToPacks { get; set; }
        public decimal PackToPieces { get; set; }
    }

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

    public class Transaction
    {
        public Product Product { get; set; }
        public decimal CaseTransact { get; set; }
        public decimal PackTransact { get; set; }   
        public decimal PieceTransact { get; set; } 
    }

    public class TransactionView
    {
        public string Id { get; set; }
        public DateTime DateTransaction { get; set; }
        public string Particular { get; set; }
        public string SalesmanName { get; set; }
        public string RefNo { get; set; }
        public string TransactionType { get; set; }
        public string PrincipalList { get; set; }
        public decimal ItemCount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

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
