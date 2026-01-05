using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Repository for managing Author entities in the database.
    /// </summary>
    public class AuthorRepository : BaseRepository, IRepository<Author>
    {
        /// <summary>
        /// Retrieves all authors from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Author"/> entities.</returns>
        public IEnumerable<Author> GetAll()
        {
            var authors = new List<Author>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Authors";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        authors.Add(MapAuthor(reader));
                    }
                }
            }
            return authors;
        }

        /// <summary>
        /// Retrieves an author by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>The <see cref="Author"/> entity if found; otherwise, null.</returns>
        public Author GetById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Authors WHERE AuthorID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapAuthor(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a new author to the database.
        /// </summary>
        /// <param name="entity">The author to add.</param>
        public void Add(Author entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Authors (FirstName, LastName, BirthDate) VALUES (@FirstName, @LastName, @BirthDate)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an existing author in the database.
        /// </summary>
        /// <param name="entity">The author with updated information.</param>
        public void Update(Author entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE Authors SET FirstName=@FirstName, LastName=@LastName, BirthDate=@BirthDate WHERE AuthorID=@AuthorID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                    cmd.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
                    cmd.Parameters.AddWithValue("@AuthorID", entity.AuthorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes an author from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the author to delete.</param>
        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Authors WHERE AuthorID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Maps a database reader row to an Author object.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>A populated <see cref="Author"/> object.</returns>
        private Author MapAuthor(MySqlDataReader reader)
        {
            return new Author
            {
                AuthorID = Convert.ToInt32(reader["AuthorID"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                BirthDate = reader["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BirthDate"])
            };
        }
    }
}
