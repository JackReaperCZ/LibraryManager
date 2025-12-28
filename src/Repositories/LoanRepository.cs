using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    public class LoanRepository : BaseRepository, IRepository<Loan>
    {
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

        public void Add(Loan entity)
        {
            // Simple add without transaction logic, but requirements say "Při vytvoření výpůjčky...".
            // So we should probably use CreateLoan instead.
            // But for interface compliance:
            CreateLoan(entity);
        }

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

        public void ReturnLoan(int loanId)
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
                         using (var getCmd = new MySqlCommand(getBookQuery, conn, transaction))
                        {
                            getCmd.Parameters.AddWithValue("@LoanID", loanId);
                            object result = getCmd.ExecuteScalar();
                            if (result != null)
                            {
                                bookId = Convert.ToInt32(result);
                            }
                            else
                            {
                                throw new Exception("Loan not found.");
                            }
                        }

                        // Update Loan
                        string updateLoanQuery = "UPDATE Loans SET Returned = TRUE, ReturnDate = @ReturnDate WHERE LoanID = @LoanID";
                        using (var updateCmd = new MySqlCommand(updateLoanQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@LoanID", loanId);
                            updateCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Update Book
                        string updateBookQuery = "UPDATE Books SET Available = TRUE WHERE BookID = @BookID";
                        using (var updateCmd = new MySqlCommand(updateBookQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@BookID", bookId);
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
