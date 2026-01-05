using System;

namespace LibraryManager.Models
{
    /// <summary>
    /// Represents a book loan transaction.
    /// </summary>
    public class Loan
    {
        /// <summary>
        /// Gets or sets the unique identifier for the loan.
        /// </summary>
        public int LoanID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the borrowed book.
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the member who borrowed the book.
        /// </summary>
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the date when the book was borrowed.
        /// </summary>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the book was returned. Null if not yet returned.
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book has been returned.
        /// </summary>
        public bool Returned { get; set; }
    }
}
