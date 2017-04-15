using System;
using Stockbook.Model;

namespace Stockbook.Class
{
    public class EtcHelper
    {

        public Product BalanceCasePackPiece(Transaction trans, Product prod, string type = "Sales")
        {
            if (prod.PackToPieces > 0 && prod.CaseToPacks > 0)
            {
                if (type == "Sales")
                {
                    var tempTransaction = (trans.CaseTransact * prod.CaseToPacks + trans.PackTransact) * prod.PackToPieces + trans.PieceTransact;
                    var tempTotalBalance = (prod.CaseBalance * prod.CaseToPacks + prod.PackBalance) * prod.PackToPieces + prod.PieceBalance;
                    var finalBalance = tempTotalBalance - tempTransaction;
                    prod.PieceBalance = finalBalance % prod.PackToPieces;
                    prod.PackBalance = Math.Truncate(finalBalance / prod.PackToPieces);
                    prod.CaseBalance = Math.Truncate(prod.PackBalance / prod.CaseToPacks);
                    prod.PackBalance = prod.PackBalance % prod.CaseToPacks;

                }
                else if (type == "Purchased")
                {
                    var tempTransaction = (trans.CaseTransact * prod.CaseToPacks + trans.PackTransact) * prod.PackToPieces + trans.PieceTransact;
                    var tempTotalBalance = (prod.CaseBalance * prod.CaseToPacks + prod.PackBalance) * prod.PackToPieces + prod.PieceBalance;
                    var finalBalance = tempTotalBalance + tempTransaction;
                    prod.PieceBalance = finalBalance % prod.PackToPieces;
                    prod.PackBalance = Math.Truncate(finalBalance / prod.PackToPieces);
                    prod.CaseBalance = Math.Truncate(prod.PackBalance / prod.CaseToPacks);
                    prod.PackBalance = prod.PackBalance % prod.CaseToPacks;
                }
            }
            else
            {
                if (type == "Sales")
                {
                    prod.CaseBalance -= trans.CaseTransact;
                    prod.PackBalance -= trans.PackTransact;
                    prod.PieceBalance -= trans.PieceTransact;
                }
                if (type == "Purchased")
                {
                    prod.CaseBalance += trans.CaseTransact;
                    prod.PackBalance += trans.PackTransact;
                    prod.PieceBalance += trans.PieceTransact;
                }
            }
            return prod;
        }

    }
}