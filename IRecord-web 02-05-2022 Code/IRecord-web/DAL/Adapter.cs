 
using System; 
using System.Data; 
using System.Data.SqlClient;
using System.Configuration;

namespace IrecordDAL
{
    public sealed class Adapter
    {


        public static string ConnectionDB()
        {
            return ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ToString();
        }



        public static System.Data.SqlClient.SqlConnection Connection
        {
            get { return new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ToString()); }
        }

        #region "ExecuteScalar function"

        public static object ExecuteScalar(string SQL)
        {
            return ExecuteScalar(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static object ExecuteScalar(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static object ExecuteScalar(string SQL, CommandType SQLType)
        {
            return ExecuteScalar(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static object ExecuteScalar(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static object ExecuteScalar(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecuteScalar(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static object ExecuteScalar(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static object ExecuteScalar(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecuteScalar(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static object ExecuteScalar(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlCommand sqlcmd = new SqlCommand();
            object scalar = null;
            ConnectionState state = default(ConnectionState);
            SqlParameter p = null;
            state = DBConnection.State;
            try
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Open();
                }
                var _with1 = sqlcmd;
                _with1.CommandText = SQL;
                _with1.CommandType = SQLType;
                _with1.Connection = DBConnection;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with1.Parameters.Add(p);
                    }
                }
                scalar = _with1.ExecuteScalar();
                return scalar;
            }
            catch (Exception ex)
            {
                scalar = 0;
                return scalar;
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Close();
                }
            }

        }

        public static object ExecuteScalar(SqlTransaction trans, string SQL)
        {
            return ExecuteScalar(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static object ExecuteScalar(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(trans, SQL, CommandType.Text, commandParameters);
        }

        public static object ExecuteScalar(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecuteScalar(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static object ExecuteScalar(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlCommand sqlcmd = new SqlCommand();
            object scalar = null;
            SqlParameter p = null;

            try
            {
                var _with2 = sqlcmd;
                _with2.CommandText = SQL;
                _with2.CommandType = SQLType;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with2.Parameters.Add(p);
                    }
                }
                _with2.Connection = trans.Connection;
                _with2.Transaction = trans;
                scalar = _with2.ExecuteScalar();

                return scalar;
            }
            catch (Exception ex)
            {
                scalar = 0;
                return scalar;
            }

        }
        #endregion

        #region "ExecuteExists Function"

        public static bool ExecuteExist(string SQL)
        {
            return ExecuteExist(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static bool ExecuteExist(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteExist(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static bool ExecuteExist(string SQL, CommandType SQLType)
        {
            return ExecuteExist(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static bool ExecuteExist(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecuteExist(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static bool ExecuteExist(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecuteExist(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static bool ExecuteExist(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteExist(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static bool ExecuteExist(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecuteExist(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static bool ExecuteExist(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataReader rdr = null;
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            bool exist = false;
            ConnectionState state = default(ConnectionState);
            state = DBConnection.State;
            try
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Open();
                }
                var _with3 = sqlcmd;
                _with3.CommandText = SQL;
                _with3.CommandType = SQLType;
                _with3.Connection = DBConnection;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with3.Parameters.Add(p);
                    }
                }
                rdr = _with3.ExecuteReader();
                exist = rdr.Read();

                rdr.Close();
                return exist;
            }
            catch (Exception ex)
            {
                exist = false;
                return exist;
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Close();
                }
            }

        }

        public static bool ExecuteExist(SqlTransaction trans, string SQL)
        {
            return ExecuteExist(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static bool ExecuteExist(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteExist(trans, SQL, CommandType.Text, commandParameters);
        }

        public static bool ExecuteExist(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecuteExist(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static bool ExecuteExist(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataReader rdr = null;
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            bool exist = false;
            try
            {
                var _with4 = sqlcmd;
                _with4.CommandText = SQL;
                _with4.CommandType = SQLType;
                _with4.Connection = trans.Connection;
                _with4.Transaction = trans;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with4.Parameters.Add(p);
                    }
                }
                rdr = _with4.ExecuteReader();
                exist = rdr.Read();
                rdr.Close();
                return exist;
            }
            catch (Exception ex)
            {
                exist = false;
                return exist;
            }

        }
        #endregion

        #region "ExecuteReader Function"
        public static SqlDataReader ExecuteReader(string SQL)
        {
            return ExecuteReader(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static SqlDataReader ExecuteReader(string SQL, CommandType SQLType)
        {
            return ExecuteReader(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static SqlDataReader ExecuteReader(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecuteReader(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static SqlDataReader ExecuteReader(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecuteReader(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataReader rdr = null;
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            ConnectionState state = default(ConnectionState);
            state = DBConnection.State;
            try
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Open();
                }
                var _with5 = sqlcmd;
                _with5.CommandText = SQL;
                _with5.CommandType = SQLType;
                _with5.Connection = DBConnection;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with5.Parameters.Add(p);
                    }
                }
                rdr = _with5.ExecuteReader();
                return rdr;
            }
            catch (Exception ex)
            {
                rdr = null;
                return rdr;
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Close();
                }
            }
        }

        public static SqlDataReader ExecuteReader(SqlTransaction trans, string SQL)
        {
            return ExecuteReader(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(trans, SQL, CommandType.Text, commandParameters);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecuteReader(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataReader rdr = null;
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            try
            {
                var _with6 = sqlcmd;
                _with6.CommandText = SQL;
                _with6.CommandType = SQLType;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with6.Parameters.Add(p);
                    }
                }
                _with6.Connection = trans.Connection;
                _with6.Transaction = trans;
                rdr = _with6.ExecuteReader();
                return rdr;
            }
            catch (Exception ex)
            {
                return rdr;
            }
        }
        #endregion

        #region "ExecuteNonQuery Function"
        public static int ExecutenNonQuery(string SQL)
        {

            return ExecutenNonQuery(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecutenNonQuery(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static int ExecutenNonQuery(string SQL, CommandType SQLType)
        {
            return ExecutenNonQuery(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecutenNonQuery(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static int ExecutenNonQuery(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecutenNonQuery(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecutenNonQuery(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static int ExecutenNonQuery(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecutenNonQuery(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlCommand sqlcmd = new SqlCommand();
            ConnectionState state = default(ConnectionState);
            int intResult = 0;
            SqlParameter p = null;
            state = DBConnection.State;
            try
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Open();
                }
                var _with7 = sqlcmd;
                _with7.CommandText = SQL;
                _with7.CommandType = SQLType;
                _with7.Connection = DBConnection;
                _with7.CommandTimeout = 0;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with7.Parameters.Add(p);
                    }
                }
                intResult = _with7.ExecuteNonQuery();
                return intResult;

            }
            catch (Exception ex)
            {
                return intResult;
            }
            finally
            {
                if (state == ConnectionState.Closed)
                {
                    DBConnection.Close();
                }
            }
        }

        public static int ExecutenNonQuery(SqlTransaction trans, string SQL)
        {
            return ExecutenNonQuery(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecutenNonQuery(trans, SQL, CommandType.Text, commandParameters);
        }

        public static int ExecutenNonQuery(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecutenNonQuery(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static int ExecutenNonQuery(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlCommand sqlcmd = new SqlCommand();
            int intResult = 0;
            SqlParameter p = null;
            try
            {
                var _with8 = sqlcmd;
                _with8.CommandText = SQL;
                _with8.CommandType = SQLType;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with8.Parameters.Add(p);
                    }
                }
                _with8.Connection = trans.Connection;
                _with8.Transaction = trans;
                intResult = _with8.ExecuteNonQuery();
                return intResult;
            }
            catch (Exception ex)
            {
                return intResult;
            }
        }

        #endregion

        #region "ExecuteDataset Function"
        public static DataSet ExecuteDataset(string SQL)
        {
            return ExecuteDataset(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static DataSet ExecuteDataset(string SQL, CommandType SQLType)
        {
            return ExecuteDataset(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static DataSet ExecuteDataset(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecuteDataset(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static DataSet ExecuteDataset(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecuteDataset(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataSet dtSet = new DataSet();
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            try
            {
                var _with9 = sqlcmd;
                _with9.CommandText = SQL;
                _with9.CommandType = SQLType;
                _with9.CommandTimeout = 0;
                _with9.Connection = DBConnection;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with9.Parameters.Add(p);
                    }
                }
                dbAdapter.SelectCommand = sqlcmd;
                dbAdapter.Fill(dtSet);
            }
            catch (Exception ex)
            {
                dtSet = null;
            }
            return dtSet;
        }

        public static DataSet ExecuteDataset(SqlTransaction trans, string SQL)
        {
            return ExecuteDataset(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataset(trans, SQL, CommandType.Text, commandParameters);
        }

        public static DataSet ExecuteDataset(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecuteDataset(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataSet ExecuteDataset(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataSet dtSet = new DataSet();
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            try
            {
                var _with10 = sqlcmd;
                _with10.CommandText = SQL;
                _with10.CommandType = SQLType;
                _with10.CommandTimeout = 0;
                _with10.Connection = trans.Connection;
                _with10.Transaction = trans;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with10.Parameters.Add(p);
                    }
                }
                dbAdapter.SelectCommand = sqlcmd;
                dbAdapter.Fill(dtSet);

            }
            catch (Exception ex)
            {
                dtSet = null;
            }

            return dtSet;

        }

        #endregion

        #region "ExecuteDataTable Function"
        public static DataTable ExecuteDataTable(string SQL)
        {
            return ExecuteDataTable(Adapter.Connection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(Adapter.Connection, SQL, CommandType.Text, commandParameters);
        }

        public static DataTable ExecuteDataTable(string SQL, CommandType SQLType)
        {
            return ExecuteDataTable(Adapter.Connection, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(Adapter.Connection, SQL, SQLType, commandParameters);
        }

        public static DataTable ExecuteDataTable(System.Data.SqlClient.SqlConnection DBConnection, string SQL)
        {
            return ExecuteDataTable(DBConnection, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(System.Data.SqlClient.SqlConnection DBConnection, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(DBConnection, SQL, CommandType.Text, commandParameters);
        }

        public static DataTable ExecuteDataTable(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType)
        {
            return ExecuteDataTable(DBConnection, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(System.Data.SqlClient.SqlConnection DBConnection, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            try
            {
                var _with11 = sqlcmd;
                _with11.CommandText = SQL;
                _with11.CommandType = SQLType;
                _with11.CommandTimeout = 0;
                _with11.Connection = DBConnection;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with11.Parameters.Add(p);
                    }
                }
                dbAdapter.SelectCommand = sqlcmd;
                dbAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public static DataTable ExecuteDataTable(SqlTransaction trans, string SQL)
        {
            return ExecuteDataTable(trans, SQL, CommandType.Text, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(SqlTransaction trans, string SQL, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(trans, SQL, CommandType.Text, commandParameters);
        }

        public static DataTable ExecuteDataTable(SqlTransaction trans, string SQL, CommandType SQLType)
        {
            return ExecuteDataTable(trans, SQL, SQLType, (SqlParameter[])null);
        }

        public static DataTable ExecuteDataTable(SqlTransaction trans, string SQL, CommandType SQLType, params SqlParameter[] commandParameters)
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlCommand sqlcmd = new SqlCommand();
            SqlParameter p = null;
            try
            {
                var _with12 = sqlcmd;
                _with12.CommandText = SQL;
                _with12.CommandType = SQLType;
                _with12.CommandTimeout = 0;
                _with12.Connection = trans.Connection;
                _with12.Transaction = trans;
                if ((commandParameters != null))
                {
                    foreach (SqlParameter p_loopVariable in commandParameters)
                    {
                        p = p_loopVariable;
                        _with12.Parameters.Add(p);
                    }
                }
                dbAdapter.SelectCommand = sqlcmd;
                dbAdapter.Fill(dt);

            }
            catch (Exception ex)
            {
                dt = null;
            }

            return dt;

        }

        #endregion



        #region Parameter

        public static void AddParameter1(SqlParameterCollection parameters, string name, object value)
        {
            parameters.Add(new SqlParameter(name, value ?? DBNull.Value));
        }
        public static void AddParameter(SqlParameterCollection parameters, string name, object value, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(name, value ?? DBNull.Value);
            parameter.Direction = direction;
            parameters.Add(parameter);
        }
        public static void AddParameter(SqlParameterCollection parameters, string name, SqlDbType type, object value)
        {
            AddParameter(parameters, name, type, 0, value ?? DBNull.Value, ParameterDirection.Input);
        }
        private static void AddParameter(SqlParameterCollection parameters, string name, SqlDbType type, object value, ParameterDirection direction)
        {
            AddParameter(parameters, name, type, 0, value ?? DBNull.Value, direction);
        }
        public static void AddParameter(SqlParameterCollection parameters, string name, SqlDbType type, int size, object value)
        {
            AddParameter(parameters, name, type, size, value ?? DBNull.Value, ParameterDirection.Input);
        }
        public static void AddParameter(SqlParameterCollection parameters, string name, SqlDbType type, int size, object value, ParameterDirection direction)
        {
            SqlParameter parameter;
            if (size < 1)
                parameter = new SqlParameter(name, type);
            else
                parameter = new SqlParameter(name, type, size);
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = direction;
            parameters.Add(parameter);
        }


        #endregion




        #region "ParameterCollectionNew"


        public static void AddParam(SqlParameterCollection objPcollection, string ParaName, object ParaValue)
        {
            objPcollection.Add(new SqlParameter(ParaName, ParaValue));
        }
        public static SqlParameter[] param(SqlParameterCollection objPcollection)
        {
            int count = 0;
            SqlParameter[] sqlParam = new SqlParameter[objPcollection.Count];
            foreach (SqlParameter p_loopVariable in objPcollection)
            {
                sqlParam[count] = new SqlParameter(p_loopVariable.ParameterName, p_loopVariable.Value);
                count++;
            }
            return sqlParam;
        }


        //c#
        // DataTable dt = new DataTable();
        //SqlParameterCollection pcol = new SqlCommand().Parameters;
        //Adapter.AddParam(pcol, "@Clnc_Id", 4);
        //Adapter.AddParam(pcol, "@Clnc_Name", "EAST");        
        //Adapter.AddParam(pcol, "@Clnc_Code", "EAST");        
        //Adapter.AddParam(pcol, "@BranchStTime", Convert.ToDateTime("2007-04-01 09:00:00.000"));     
        //dt = Adapter.ExecuteDataTable("test_branchList", CommandType.StoredProcedure, Adapter.param(pcol));   


        //vb.net
        // Dim dt As New DataTable()
        //Dim pcol As SqlParameterCollection = New SqlCommand().Parameters
        //Adapter.AddParam(pcol, "@Clnc_Id", 4)
        //Adapter.AddParam(pcol, "@Clnc_Name", "EAST")
        //Adapter.AddParam(pcol, "@Clnc_Code", "EAST")
        //Adapter.AddParam(pcol, "@BranchStTime", Convert.ToDateTime("2007-04-01 09:00:00.000"))
        //dt = Adapter.ExecuteDataTable("test_branchList", CommandType.StoredProcedure, Adapter.param(pcol))

        #endregion

    }

}
