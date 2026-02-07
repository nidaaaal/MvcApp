using Microsoft.Data.SqlClient;
using System.Data;

namespace MvcApp.Repository
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }   

       protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected async Task<SqlDataReader> ExecuteSp(string spName, SqlParameter[] sqlParameters)
        {
            SqlConnection conn = GetConnection();

            try
            {
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    cmd.Parameters.AddRange(sqlParameters);
                }

                await conn.OpenAsync();

                return await cmd.ExecuteReaderAsync();
            }
            catch
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                throw;

            }

        }


    }
}
