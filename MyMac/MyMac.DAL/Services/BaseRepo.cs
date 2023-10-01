
using System.Data;
using System.Data.SqlClient;
using Dapper;



namespace MyMac.DAL.Services
{
   

    public class BaseRepo
    {
        public static string ConnectionString { get; set; }


        public object Query<T>(object gET_ALL_STATUS, object p, CommandType text)
        {
            throw new NotImplementedException();
        }

        protected async Task<IEnumerable<T>> Query<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }
                try
                {
                    return await conn.QueryAsync<T>(sql, param, commandType: commandType);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }

                return await conn.QueryAsync<T>(sql, param, commandType: commandType);
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }

                return await conn.QueryFirstOrDefaultAsync<T>(sql, param, commandType: commandType);
            }
        }




        protected IDataReader GetReader<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }

                return conn.ExecuteReader(sql, param, commandType: commandType);
            }
        }

        protected DataTable GetReader(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                DataTable table = new DataTable();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }

                table.Load(conn.ExecuteReader(sql, param, commandType: commandType));
                return table;
            }
        }


        protected async Task<DataSet> GetMultipleResult(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            DataSet ds = new DataSet();
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                    SqlCommand sqlComm = new SqlCommand(sql, conn);


                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }
                else if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Connecting || conn.State == ConnectionState.Executing || conn.State == ConnectionState.Fetching)
                {
                    conn.Close();
                    conn.Open();
                }


            }
            return ds;
        }

        
    }
}
