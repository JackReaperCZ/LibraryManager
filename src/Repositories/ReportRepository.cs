using MySql.Data.MySqlClient;
using System.Data;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Repository for generating aggregated reports from the database.
    /// </summary>
    public class ReportRepository : BaseRepository
    {
        /// <summary>
        /// Retrieves a report of members and their loan counts.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing Member names and LoanCount.</returns>
        public DataTable GetMemberLoanCounts()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT m.FirstName, m.LastName, COUNT(l.LoanID) as LoanCount 
                    FROM Members m
                    LEFT JOIN Loans l ON m.MemberID = l.MemberID
                    GROUP BY m.MemberID, m.FirstName, m.LastName
                    ORDER BY LoanCount DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Retrieves a report of popular books based on loan count.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing Book titles, Genres, and LoanCount.</returns>
        public DataTable GetPopularBooks()
        {
             using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT b.Title, b.Genre, COUNT(l.LoanID) as LoanCount
                    FROM Books b
                    LEFT JOIN Loans l ON b.BookID = l.BookID
                    GROUP BY b.BookID, b.Title, b.Genre
                    ORDER BY LoanCount DESC";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Retrieves statistics about books and loans per genre.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing Genre, BookCount, and TotalLoans.</returns>
        public DataTable GetGenreStats()
        {
             using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT b.Genre, COUNT(b.BookID) as BookCount, COUNT(l.LoanID) as TotalLoans
                    FROM Books b
                    LEFT JOIN Loans l ON b.BookID = l.BookID
                    GROUP BY b.Genre";
                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
