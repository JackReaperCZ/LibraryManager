using System;

namespace LibraryManager.Models
{
    public enum Genre
    {
        Fiction,
        NonFiction, // Mapped to 'Non-Fiction'
        Science,
        History,
        Other
    }

    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; }
        public Genre Genre { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
