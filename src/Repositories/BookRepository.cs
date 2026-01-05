using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Repository for managing Book entities in the database.
    /// </summary>
    public class BookRepository : BaseRepository, IRepository<Book>
    {
        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Book"/> entities.</returns>
        public IEnumerable<Book> GetAll()
        {
            var books = new List<Book>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Books";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(MapBook(reader));
                    }
                }
            }
            return books;
        }

        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <returns>The <see cref="Book"/> entity if found; otherwise, null.</returns>
        public Book GetById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Books WHERE BookID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapBook(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="entity">The book to add.</param>
        public void Add(Book entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Books (Title, Price, Available, Genre, PublishedDate) VALUES (@Title, @Price, @Available, @Genre, @PublishedDate)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", entity.Title);
                    cmd.Parameters.AddWithValue("@Price", entity.Price);
                    cmd.Parameters.AddWithValue("@Available", entity.Available);
                    cmd.Parameters.AddWithValue("@Genre", MapGenreToString(entity.Genre));
                    cmd.Parameters.AddWithValue("@PublishedDate", entity.PublishedDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="entity">The book with updated information.</param>
        public void Update(Book entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE Books SET Title=@Title, Price=@Price, Available=@Available, Genre=@Genre, PublishedDate=@PublishedDate WHERE BookID=@BookID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", entity.Title);
                    cmd.Parameters.AddWithValue("@Price", entity.Price);
                    cmd.Parameters.AddWithValue("@Available", entity.Available);
                    cmd.Parameters.AddWithValue("@Genre", MapGenreToString(entity.Genre));
                    cmd.Parameters.AddWithValue("@PublishedDate", entity.PublishedDate);
                    cmd.Parameters.AddWithValue("@BookID", entity.BookID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a book from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete.</param>
        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Books WHERE BookID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Maps a database reader row to a Book object.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>A populated <see cref="Book"/> object.</returns>
        private Book MapBook(MySqlDataReader reader)
        {
            return new Book
            {
                BookID = Convert.ToInt32(reader["BookID"]),
                Title = reader["Title"].ToString(),
                Price = Convert.ToSingle(reader["Price"]),
                Available = Convert.ToBoolean(reader["Available"]),
                Genre = MapStringToGenre(reader["Genre"].ToString()),
                PublishedDate = Convert.ToDateTime(reader["PublishedDate"])
            };
        }

        /// <summary>
        /// Maps a Genre enum value to its string representation for the database.
        /// </summary>
        /// <param name="genre">The genre enum.</param>
        /// <returns>The string representation of the genre.</returns>
        private string MapGenreToString(Genre genre)
        {
            return genre == Genre.NonFiction ? "Non-Fiction" : genre.ToString();
        }

        /// <summary>
        /// Maps a string representation of a genre to its enum value.
        /// </summary>
        /// <param name="genreStr">The string representation of the genre.</param>
        /// <returns>The <see cref="Genre"/> enum value.</returns>
        private Genre MapStringToGenre(string genreStr)
        {
            if (genreStr == "Non-Fiction") return Genre.NonFiction;
            if (Enum.TryParse(genreStr, out Genre result)) return result;
            return Genre.Other;
        }
    }
}
