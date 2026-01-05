using System;

namespace LibraryManager.Models
{
    /// <summary>
    /// Represents a library member.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Gets or sets the unique identifier for the member.
        /// </summary>
        public int MemberID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the member.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the member.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the member.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the date when the member registered.
        /// </summary>
        public DateTime RegisteredDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the member is currently active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the full name of the member (First Name + Last Name).
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
