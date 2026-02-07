using Microsoft.Data.SqlClient;
using MvcApp.Repository.Interfaces;
using System.Diagnostics;
using System.Net;
using MvcApp.Models.Entities;
using MvcApp.Models.ViewModels;
using MvcApp.Models.Common;

namespace MvcApp.Repository
{
    public class UserRepository : BaseRepository,IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<int> RegisterUser(RegisterViewModel dto,string hashedpass,int age,DateTime dob)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@username",dto.UserName),
                new SqlParameter("@hashed_password",hashedpass),
                new SqlParameter("@first_name",dto.FirstName),
                new SqlParameter("@last_name",dto.LastName),
                new SqlParameter("@display_name",string.IsNullOrEmpty(dto.DisplayName) ? (object)DBNull.Value : dto.DisplayName),
                new SqlParameter("@date_of_birth",dob),
                new SqlParameter("@age",age),
                new SqlParameter("@gender",dto.Gender),
                new SqlParameter("@address",dto.Address),
                new SqlParameter("@city",dto.City),
                new SqlParameter("@state",dto.State),
                new SqlParameter("@zipcode",dto.ZipCode),
                new SqlParameter("@phone",dto.Phone),
                new SqlParameter("@mobile", string.IsNullOrEmpty(dto.Mobile) ? (object)DBNull.Value : dto.Phone)
            };

            SqlDataReader reader = await ExecuteSp("auth.register_user", parameters);

            if(await reader.ReadAsync())
            {
                int ResultCode = Convert.ToInt32(reader["ResultCode"]);
                string Message = Convert.ToString(reader["Message"]) ?? "";

                return ResultCode;
            }

            return 0;

        }

        public async Task<Credential?> GetUserByUsername(string username)
        {
            string sql = "SELECT id,username,hashed_password FROM auth.credentials WHERE username = @username;";
            await using var conn = GetConnection();

            await using SqlCommand cmd = new SqlCommand(sql, conn);

            Credential credential = new Credential();

            cmd.Parameters.AddWithValue("@username", username);

            await conn.OpenAsync();



            await using var read = await cmd.ExecuteReaderAsync();

            if (!await read.ReadAsync()) return null;
         
                credential.Id = Convert.ToInt32(read["id"]);
                credential.UserName = Convert.ToString(read["username"]) ?? "";
                credential.HashedPassword = Convert.ToString(read["hashed_password"]) ?? "";


            return credential;

        }

        public async Task SaveLogin(int id)
        {
           await using var conn = GetConnection();

            DateTime now = DateTime.Now;

            string sql = "UPDATE auth.credentials SET login_at = @now WHERE id = @id;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                cmd.Parameters.AddWithValue("@now", now);
                cmd.Parameters.AddWithValue("@id", id);

                await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<Users?> GetUserProfile(int id)
        {
            await using var conn = GetConnection();

            string sql = "SELECT * FROM app.users WHERE id = @id;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();

                cmd.Parameters.AddWithValue("@id", id);

                var read = await cmd.ExecuteReaderAsync();

                if (!await read.ReadAsync()) return null;

                return new Users
                {
                    FirstName = Convert.ToString(read["first_name"]),
                    LastName = Convert.ToString(read["last_name"]),
                    DisplayName = read["display_name"] == DBNull.Value ? null : Convert.ToString(read["display_name"]),
                    DateOfBirth = Convert.ToDateTime(read["date_of_birth"]),
                    Gender = Convert.ToBoolean(read["gender"]),
                    Age = Convert.ToInt32(read["age"]),
                    Address = Convert.ToString(read["address"]),
                    City = read["city"] == DBNull.Value ? null : Convert.ToString(read["city"]),
                    State = read["state"] == DBNull.Value ? null : Convert.ToString(read["state"]),
                    ZipCode = Convert.ToInt32(read["zipcode"]),
                    Phone = Convert.ToString(read["phone"]),
                    Mobile = read["mobile"] == DBNull.Value ? null : Convert.ToString(read["mobile"])

                };

            }

        }
        public async Task<DbResponse> UpdateUserProfile(int id,UpdateViewModel updateProfile,int age)
        {
            var paramiters = new SqlParameter[]
            {
                new SqlParameter("@id",id),
                new SqlParameter("@first_name",updateProfile.FirstName),
                new SqlParameter("@last_name",updateProfile.LastName),
                new SqlParameter("@display_name",string.IsNullOrEmpty(updateProfile.DisplayName) ? (object)DBNull.Value : updateProfile.DisplayName),
                new SqlParameter("@date_of_birth",updateProfile.DateOfBirth),
                new SqlParameter("@age",age),
                new SqlParameter("@gender",updateProfile.Gender),
                new SqlParameter("@address",updateProfile.Address),
                new SqlParameter("@city",string.IsNullOrEmpty(updateProfile.City) ? (object)DBNull.Value : updateProfile.City),
                new SqlParameter("@state",string.IsNullOrEmpty(updateProfile.State) ? (object)DBNull.Value : updateProfile.State),
                new SqlParameter("@zipcode",updateProfile.ZipCode),
                new SqlParameter("@phone",updateProfile.Phone),
                new SqlParameter("@mobile",string.IsNullOrEmpty(updateProfile.Mobile) ? (object)DBNull.Value : updateProfile.Mobile)
            };

            var read = await ExecuteSp("app.profile_update",paramiters);

            await read.ReadAsync();

            return new DbResponse
            {
                ResultCode = Convert.ToInt32(read["ResultCode"]),
                Message = Convert.ToString(read["Message"]) ?? ""
            };
               


        }
    }
}
