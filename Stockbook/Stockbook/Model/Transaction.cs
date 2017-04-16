// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Transaction.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Transaction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Stockbook.Model
{
    using Class;

    public class Transaction
    {
        public Product Product { get; set; }
        public decimal CaseTransact { get; set; }
        public decimal PackTransact { get; set; }   
        public decimal PieceTransact { get; set; } 
    }
}