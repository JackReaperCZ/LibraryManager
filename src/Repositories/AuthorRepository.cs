using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    public class AuthorRepository : BaseRepository, IRepository<Author>
    {
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
