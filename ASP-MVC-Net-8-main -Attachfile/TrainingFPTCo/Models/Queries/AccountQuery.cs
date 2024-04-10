using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using TrainingFPTCo.Models;

namespace TrainingFPTCo.Models.Queries
{
    public class AccountQuery
    {
        public int InsertAccount(
           int roleId,
           string username,
           string password,
           string extraCode,
           string email,
           string phone,
           string address,
           string fullName,
           string firstName,
           string lastName,
           DateTime? birthday,
           string gender,
           string education,
           string programingLanguage,
           int? toeicScore,
           string skills,
           string ipCleant
       )
        {
            int idAccount = 0;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "INSERT INTO [Users] ([RoleId], [Username], [Password], [ExtraCode], [Email], [Phone], [Address], [FullName], [FirstName], [LastName], [Birthday], [Gender], [Education], [ProgramingLanguage], [ToeicScore], [Skills], [IPCleant], [CreatedAt]) VALUES (@RoleId, @Username, @Password, @ExtraCode, @Email, @Phone, @Address, @FullName, @FirstName, @LastName, @Birthday, @Gender, @Education, @ProgramingLanguage, @ToeicScore, @Skills, @IPCleant, @CreatedAt); SELECT SCOPE_IDENTITY()";

                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@ExtraCode", extraCode);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@birthday", birthday.HasValue ? (object)birthday : DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Education", education);
                cmd.Parameters.AddWithValue("@ProgramingLanguage", programingLanguage);
                cmd.Parameters.AddWithValue("@toeicScore", toeicScore.HasValue ? (object)toeicScore : DBNull.Value);
                cmd.Parameters.AddWithValue("@Skills", skills);
                cmd.Parameters.AddWithValue("@IPCleant", ipCleant);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                idAccount = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return idAccount;
        }


        public AccountDetail GetAccountById(int id)
        {
            AccountDetail accountDetail = new AccountDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Users] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accountDetail.Id = Convert.ToInt32(reader["Id"]);
                        accountDetail.RoleId = Convert.ToInt32(reader["RoleId"]);
                        // Lấy thông tin chi tiết của Role từ bảng Roles (cần thay đổi tên cột nếu cần)
                        // Ví dụ: accountDetail.Role = GetRoleById(accountDetail.RoleId);
                        accountDetail.Username = reader["Username"].ToString();
                        accountDetail.ExtraCode = reader["ExtraCode"].ToString();
                        accountDetail.Password = reader["Password"].ToString();
                        accountDetail.Email = reader["Email"].ToString();
                        accountDetail.Phone = reader["Phone"].ToString();
                        accountDetail.Address = reader["Address"].ToString();
                        accountDetail.FullName = reader["FullName"].ToString();
                        accountDetail.FirstName = reader["FirstName"].ToString();
                        accountDetail.LastName = reader["LastName"].ToString();
                        accountDetail.Birthday = reader["Birthday"] != DBNull.Value ? Convert.ToDateTime(reader["Birthday"]) : (DateTime?)null;
                        accountDetail.Gender = reader["Gender"].ToString();
                        accountDetail.Education = reader["Education"].ToString();
                        accountDetail.ProgramingLanguage = reader["ProgramingLanguage"].ToString();
                        accountDetail.ToeicScore = reader["ToeicScore"] != DBNull.Value ? Convert.ToInt32(reader["ToeicScore"]) : (int?)null;
                        accountDetail.Skills = reader["Skills"].ToString();
                        accountDetail.IPCleant = reader["IPCleant"].ToString();
                        accountDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        accountDetail.UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : (DateTime?)null;
                        accountDetail.DeletedAt = reader["DeletedAt"] != DBNull.Value ? Convert.ToDateTime(reader["DeletedAt"]) : (DateTime?)null;
                    }
                    connection.Close();
                }
            }
            return accountDetail;
        }


        public Role GetRoleById(int roleId)
        {
            Role role = null;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Roles] WHERE [Id] = @roleId";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@roleId", roleId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        role = new Role
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Status = reader["Status"].ToString(),
                            // Thêm các thông tin khác về vai trò nếu cần
                        };
                    }
                }
                connection.Close();
            }
            return role;
        }




        public List<AccountDetail> GetAllAccounts(string? searchString, string? filterStatus)
        {
            List<AccountDetail> accounts = new List<AccountDetail>();

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                if (!string.IsNullOrEmpty(filterStatus))
                {
                    sqlQuery = @"SELECT U.*, R.[Status] AS [RoleStatus] 
                         FROM [Users] U 
                         INNER JOIN [Roles] R ON U.RoleId = R.Id 
                         WHERE (U.[Username] LIKE @searchString OR U.[FullName] LIKE @searchString) 
                         AND U.[DeletedAt] IS NULL AND R.[Status] = @status";
                }
                else
                {
                    sqlQuery = @"SELECT U.*, R.[Status] AS [RoleStatus] 
                         FROM [Users] U 
                         INNER JOIN [Roles] R ON U.RoleId = R.Id 
                         WHERE (U.[Username] LIKE @searchString OR U.[FullName] LIKE @searchString) 
                         AND U.[DeletedAt] IS NULL";
                }

                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@searchString", $"%{searchString}%");
                if (!string.IsNullOrEmpty(filterStatus))
                {
                    command.Parameters.AddWithValue("@status", filterStatus);
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AccountDetail account = new AccountDetail();
                        account.Role = GetRoleById(Convert.ToInt32(reader["RoleId"]));
                        account.Id = Convert.ToInt32(reader["Id"]);
                        account.RoleId = Convert.ToInt32(reader["RoleId"]);
                        account.Status = reader["RoleStatus"].ToString();
                        account.Username = reader["Username"].ToString();
                        account.ExtraCode = reader["ExtraCode"].ToString();
                        account.Password = reader["Password"].ToString();
                        account.Email = reader["Email"].ToString();
                        account.Phone = reader["Phone"].ToString();
                        account.Address = reader["Address"].ToString();
                        account.FullName = reader["FullName"].ToString();
                        account.FirstName = reader["FirstName"].ToString();
                        account.LastName = reader["LastName"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("Birthday")))
                        {
                            account.Birthday = Convert.ToDateTime(reader["Birthday"]);
                        }
                        account.Gender = reader["Gender"].ToString();
                        account.Education = reader["Education"].ToString();
                        account.ProgramingLanguage = reader["ProgramingLanguage"].ToString();
                        if (!reader.IsDBNull(reader.GetOrdinal("ToeicScore")))
                        {
                            account.ToeicScore = Convert.ToInt32(reader["ToeicScore"]);
                        }
                        account.Skills = reader["Skills"].ToString();
                        account.IPCleant = reader["IPCleant"].ToString();
                        account.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["CreatedAt"]);
                        account.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]);
                        account.DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["DeletedAt"]);
                        accounts.Add(account);
                    }
                    connection.Close();
                }
            }
            return accounts;
        }


        public List<AccountDetail> GetFilteredAccountsForTrainingStaff(string searchString, string filterStatus)
        {
            List<AccountDetail> filteredAccounts = new List<AccountDetail>();

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT U.*, R.[Status] AS [RoleStatus] FROM [Users] U " +
                                  "INNER JOIN [Roles] R ON U.RoleId = R.Id " +
                                  "WHERE (U.[Username] LIKE @searchString OR U.[FullName] LIKE @searchString) " +
                                  "AND U.[DeletedAt] IS NULL " +
                                  "AND (R.[Name] IN ('trainingstaff', 'trainer', 'trainee'))";

                if (!string.IsNullOrEmpty(filterStatus))
                {
                    sqlQuery += " AND R.[Status] = @status";
                }

                connection.Open();
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                if (!string.IsNullOrEmpty(filterStatus))
                {
                    command.Parameters.AddWithValue("@status", filterStatus);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapping dữ liệu từ SqlDataReader sang AccountDetail tương tự như trong phương thức GetAllAccounts
                        AccountDetail account = new AccountDetail();
                        // Mapping các thuộc tính của tài khoản vào đây
                        filteredAccounts.Add(account);
                    }
                }
            }

            return filteredAccounts;
        }
        public bool UpdateAccountById(
            int roleId,
            string username,
            string password,
            string extraCode,
            string email,
            string phone,
            string address,
            string fullName,
            string firstName,
            string lastName,
            DateTime? birthday,
            string gender,
            string education,
            string programingLanguage,
            int? toeicScore,
            string skills,
            string ipCleant,
            int id
)
        {
            bool checkUpdate = false;
            // Kiểm tra các trường bắt buộc không được rỗng
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(extraCode) || string.IsNullOrWhiteSpace(email))
            {
                return checkUpdate; // Trả về false nếu bất kỳ trường bắt buộc nào bị trống
            }
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Users] SET [RoleId] = @roleId, [Username] = @username, [Password] = @password, [ExtraCode] = @extraCode, [Email] = @email, [Phone] = @phone, [Address] = @address, [FullName] = @fullName, [FirstName] = @firstName, [LastName] = @lastName, [Birthday] = @birthday, [Gender] = @gender, [Education] = @education, [ProgramingLanguage] = @programingLanguage, [ToeicScore] = @toeicScore, [Skills] = @skills, [IPCleant] = @ipCleant, [UpdatedAt] = @updatedAt WHERE [Id] = @id"; 
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.Parameters.AddWithValue("@username", username ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@password", password ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@extraCode", extraCode ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@email", email ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@phone", phone ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@address", address ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@fullName", fullName ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@firstName", firstName ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@lastName", lastName ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@birthday", birthday.HasValue ? (object)birthday : DBNull.Value);
                cmd.Parameters.AddWithValue("@gender", gender ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@education", education ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@programingLanguage", programingLanguage ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@toeicScore", toeicScore.HasValue ? toeicScore : DBNull.Value);
                cmd.Parameters.AddWithValue("@skills", skills ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@ipCleant", ipCleant ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }



        public bool DeleteAccountById(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "DELETE FROM [Users] WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    statusDelete = true;
                }
                connection.Close();
            }
            // Trả về true nếu xóa thành công, ngược lại trả về false
            return statusDelete;
        }


    }
}
