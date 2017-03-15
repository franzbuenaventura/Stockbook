namespace Stockbook.Model
{
    public class Transaction
    {
        public Product Product { get; set; }
        public decimal CaseTransact { get; set; }
        public decimal PackTransact { get; set; }   
        public decimal PieceTransact { get; set; } 
    }
}