using System;
using System.Collections.Generic;

namespace Stockbook.Model
{
    using Stockbook.Class;

    public class Transaction
    {
        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets the case balance of transaction.
        /// </summary>
        public decimal CaseTransact { get; set; }

        /// <summary>
        /// Gets or sets the pack balance of transaction.
        /// </summary>
        public decimal PackTransact { get; set; }

        /// <summary>
        /// Gets or sets the piece balance of transaction.
        /// </summary>
        public decimal PieceTransact { get; set; }
    }
}