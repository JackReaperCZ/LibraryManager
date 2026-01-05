using System;

namespace LibraryManager.Models
{
    /// <summary>
    /// Represents the genre of a book.
    /// </summary>
    public enum Genre
    {
        /// <summary>
        /// Fiction genre.
        /// </summary>
        Fiction,
        /// <summary>
        /// Non-fiction genre. Mapped to 'Non-Fiction' in database.
        /// </summary>
        NonFiction,
        /// <summary>
        /// Science genre.
        /// </summary>
        Science,
        /// <summary>
        /// History genre.
        /// </summary>
        History,
        /// <summary>
        /// Other genres.
        /// </summary>
        Other
    }

    /// <summary>
    /// Represents a book in the library.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the price of the book.
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book is currently available for loan.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets the genre of the book.
        /// </summary>
        public Genre Genre { get; set; }

        /// <summary>
        /// Gets or sets the publication date of the book.
        /// </summary>
        public DateTime PublishedDate { get; set; }
    }
}
