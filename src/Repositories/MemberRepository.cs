using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    /// <summary>
    /// Repository for managing Member entities in the database.
    /// </summary>
    public class MemberRepository : BaseRepository, IRepository<Member>
    {
        /// <summary>
        /// Retrieves all members from the database.
        /// </summary>
        /// <returns>A collection of <see cref="Member"/> entities.</returns>
        public IEnumerable<Member> GetAll()
        {
            var members = new List<Member>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Members";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(MapMember(reader));
                    }
                }
            }
            return members;
        }

        /// <summary>
        /// Retrieves a member by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the member.</param>
        /// <returns>The <see cref="Member"/> entity if found; otherwise, null.</returns>
        public Member GetById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Members WHERE MemberID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapMember(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a new member to the database.
        /// </summary>
        /// <param name="entity">The member to add.</param>
        public void Add(Member entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Members (FirstName, LastName, Email, RegisteredDate, IsActive) VALUES (@FirstName, @LastName, @Email, @RegisteredDate, @IsActive)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);
                    cmd.Parameters.AddWithValue("@RegisteredDate", entity.RegisteredDate);
                    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates an existing member in the database.
        /// </summary>
        /// <param name="entity">The member with updated information.</param>
        public void Update(Member entity)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE Members SET FirstName=@FirstName, LastName=@LastName, Email=@Email, RegisteredDate=@RegisteredDate, IsActive=@IsActive WHERE MemberID=@MemberID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", entity.LastName);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);
                    cmd.Parameters.AddWithValue("@RegisteredDate", entity.RegisteredDate);
                    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cmd.Parameters.AddWithValue("@MemberID", entity.MemberID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes a member from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the member to delete.</param>
        public void Delete(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Members WHERE MemberID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Maps a database reader row to a Member object.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>A populated <see cref="Member"/> object.</returns>
        private Member MapMember(MySqlDataReader reader)
        {
            return new Member
            {
                MemberID = Convert.ToInt32(reader["MemberID"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                RegisteredDate = Convert.ToDateTime(reader["RegisteredDate"]),
                IsActive = Convert.ToBoolean(reader["IsActive"])
            };
        }
    }
}
