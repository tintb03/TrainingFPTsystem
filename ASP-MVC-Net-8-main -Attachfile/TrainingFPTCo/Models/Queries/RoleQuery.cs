using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace TrainingFPTCo.Models.Queries
{
    public class RoleQuery
    {
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Roles]";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role role = new Role
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Status = reader["Status"].ToString()
                            // Thêm các thông tin khác về vai trò nếu cần
                        };
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }


        public List<Role> GetTrainingStaffRoles()
        {
            List<Role> trainingStaffRoles = new List<Role>();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Roles] WHERE Name IN ('Trainer', 'Trainee')";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Role role = new Role
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Status = reader["Status"].ToString()
                            // Thêm các thông tin khác về vai trò nếu cần
                        };
                        trainingStaffRoles.Add(role);
                    }
                }
            }
            return trainingStaffRoles;
        }
    }
}
