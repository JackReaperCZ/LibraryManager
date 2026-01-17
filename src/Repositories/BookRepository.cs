using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    public class BookRepository : BaseRepository, IRepository<Book>
    {
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
                    entity.BookID = (int)cmd.LastInsertedId;
                }
            }
        }

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

        public int? GetAuthorIdForBook(int bookId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT AuthorID FROM BookAuthors WHERE BookID = @BookID LIMIT 1";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    var result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        public void SetBookAuthor(int bookId, int? authorId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string deleteQuery = "DELETE FROM BookAuthors WHERE BookID = @BookID";
                        using (var deleteCmd = new MySqlCommand(deleteQuery, conn, transaction))
                        {
                            deleteCmd.Parameters.AddWithValue("@BookID", bookId);
                            deleteCmd.ExecuteNonQuery();
                        }

                        if (authorId.HasValue)
                        {
                            string insertQuery = "INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID)";
                            using (var insertCmd = new MySqlCommand(insertQuery, conn, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@BookID", bookId);
                                insertCmd.Parameters.AddWithValue("@AuthorID", authorId.Value);
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

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

        private string MapGenreToString(Genre genre)
        {
            return genre == Genre.NonFiction ? "Non-Fiction" : genre.ToString();
        }

        private Genre MapStringToGenre(string genreStr)
        {
            if (genreStr == "Non-Fiction") return Genre.NonFiction;
            if (Enum.TryParse(genreStr, out Genre result)) return result;
            return Genre.Other;
        }
    }
}
