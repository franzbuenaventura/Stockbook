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
}
