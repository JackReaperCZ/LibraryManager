using LibraryManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LibraryManager.Repositories
{
    public class MemberRepository : BaseRepository, IRepository<Member>
    {
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
