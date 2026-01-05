using System;

namespace LibraryManager.Models
{
    /// <summary>
    /// Represents an author of a book.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets the unique identifier for the author.
        /// </summary>
        public int AuthorID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the author.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the author.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the author.
        /// </summary>
        public DateTime? BirthDate { get; set; }
        
        /// <summary>
        /// Gets the full name of the author (First Name + Last Name).
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
