using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Repository for managing Loan entities and related transactions in the database.
    /// </summary>
    public class LoanRepository : BaseRepository, IRepository<Loan>
    {
        /// <summary>
        /// Retrieves all loans from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Loan"/> entities.</returns>
        public IEnumerable<Loan> GetAll()
        {
            var loans = new List<Loan>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Loans";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loans.Add(MapLoan(reader));
                    }
                }
            }
            return loans;
        }

        /// <summary>
        /// Retrieves a loan by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the loan.</param>
        /// <returns>The <see cref="Loan"/> entity if found; otherwise, null.</returns>
        public Loan GetById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Loans WHERE LoanID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapLoan(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a new loan using a transaction to ensure book availability is updated.
        /// </summary>
        /// <param name="entity">The loan to add.</param>
        public void Add(Loan entity)
        {
            CreateLoan(entity);
        }

        /// <summary>
        /// Creates a new loan transaction.
        /// Checks book availability, inserts the loan record, and updates the book status to unavailable.
        /// </summary>
        /// <param name="loan">The loan details.</param>
        /// <exception cref="Exception">Thrown if the book is not available.</exception>
        public void CreateLoan(Loan loan)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Check availability
                        string checkQuery = "SELECT Available FROM Books WHERE BookID = @BookID";
                        bool isAvailable = false;
                        using (var checkCmd = new MySqlCommand(checkQuery, conn, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@BookID", loan.BookID);
                            object result = checkCmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                isAvailable = Convert.ToBoolean(result);
                            }
                        }

                        if (!isAvailable)
                        {
                            throw new Exception("Book is not available for loan.");
                        }

                        // 2. Insert Loan
                        string insertQuery = "INSERT INTO Loans (BookID, MemberID, LoanDate, ReturnDate, Returned) VALUES (@BookID, @MemberID, @LoanDate, @ReturnDate, @Returned)";
                        using (var insertCmd = new MySqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@BookID", loan.BookID);
                            insertCmd.Parameters.AddWithValue("@MemberID", loan.MemberID);
                            insertCmd.Parameters.AddWithValue("@LoanDate", loan.LoanDate);
                            insertCmd.Parameters.AddWithValue("@ReturnDate", loan.ReturnDate);
                            insertCmd.Parameters.AddWithValue("@Returned", loan.Returned);
                            insertCmd.ExecuteNonQuery();
                        }

                        // 3. Update Book availability
                        string updateQuery = "UPDATE Books SET Available = FALSE WHERE BookID = @BookID";
                        using (var updateCmd = new MySqlCommand(updateQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@BookID", loan.BookID);
                            updateCmd.ExecuteNonQuery();
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

        /// <summary>
        /// Updates an existing loan.
        /// </summary>
        /// <param name="entity">The loan with updated information.</param>
        public void Update(Loan entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE Loans SET BookID=@BookID, MemberID=@MemberID, LoanDate=@LoanDate, ReturnDate=@ReturnDate, Returned=@Returned WHERE LoanID=@LoanID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", entity.BookID);
                    cmd.Parameters.AddWithValue("@MemberID", entity.MemberID);
                    cmd.Parameters.AddWithValue("@LoanDate", entity.LoanDate);
                    cmd.Parameters.AddWithValue("@ReturnDate", entity.ReturnDate);
                    cmd.Parameters.AddWithValue("@Returned", entity.Returned);
                    cmd.Parameters.AddWithValue("@LoanID", entity.LoanID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Returns a book from a loan transaction.
        /// Updates the loan record and sets the book status to available.
        /// </summary>
        /// <param name="loanId">The identifier of the loan to return.</param>
        public void ReturnBook(int loanId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get BookID
                        int bookId = 0;
                        string getBookQuery = "SELECT BookID FROM Loans WHERE LoanID = @LoanID";
                        using (var cmd = new MySqlCommand(getBookQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@LoanID", loanId);
                            object result = cmd.ExecuteScalar();
                            if (result != null) bookId = Convert.ToInt32(result);
                        }

                        // Update Loan
                        string updateLoanQuery = "UPDATE Loans SET Returned = TRUE, ReturnDate = @ReturnDate WHERE LoanID = @LoanID";
                        using (var cmd = new MySqlCommand(updateLoanQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@LoanID", loanId);
                            cmd.ExecuteNonQuery();
                        }

                        // Update Book
                        string updateBookQuery = "UPDATE Books SET Available = TRUE WHERE BookID = @BookID";
                        using (var cmd = new MySqlCommand(updateBookQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookID", bookId);
                            cmd.ExecuteNonQuery();
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

        /// <summary>
        /// Deletes a loan from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the loan to delete.</param>
        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Loans WHERE LoanID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Maps a database reader row to a Loan object.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>A populated <see cref="Loan"/> object.</returns>
        private Loan MapLoan(MySqlDataReader reader)
        {
            return new Loan
            {
                LoanID = Convert.ToInt32(reader["LoanID"]),
                BookID = Convert.ToInt32(reader["BookID"]),
                MemberID = Convert.ToInt32(reader["MemberID"]),
                LoanDate = Convert.ToDateTime(reader["LoanDate"]),
                ReturnDate = reader["ReturnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReturnDate"]),
                Returned = Convert.ToBoolean(reader["Returned"])
            };
        }
    }
}
