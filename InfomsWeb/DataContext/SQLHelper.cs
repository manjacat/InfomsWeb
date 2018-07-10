using InfomsWeb.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfomsWeb.DataContext
{
    public class SQLHelper
    {

        //Push and pull Data. All Connection Strings Come HERE Please Create Connection Here
        protected string connectionString;

        //contstructor
        public SQLHelper()
        {
        }

        protected DataSet QuerySet(string sqlquery)
        {
            return QuerySet(sqlquery, null, CommandType.Text);
        }

        protected DataSet QuerySet(string sqlquery, SqlParameter[] parameters)
        {
            return QuerySet(sqlquery, parameters, CommandType.Text);
        }

        protected DataSet QuerySet(string sqlquery, SqlParameter[] parameters, CommandType cmdType)
        {
            DataSet ds = new DataSet("result");
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sqlquery, myConnection);
                if (parameters != null)
                    myCommand.Parameters.AddRange(parameters);
                myCommand.CommandType = cmdType;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = myCommand;
                myCommand = null;
                da.Fill(ds);
                myConnection.Close();
            }
            return ds;
        }

        protected DataTable QueryTable(string sqlquery)
        {
            return QueryTable(sqlquery, null);
        }

        protected DataTable QueryTable(string sqlquery, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet("result");
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Close();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(sqlquery, myConnection);
                    if (parameters != null)
                        myCommand.Parameters.AddRange(parameters);
                    myCommand.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = myCommand;
                    myCommand = null;
                    da.Fill(ds);
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    //Log Error
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    // throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }

            }
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        protected DataSet QueryTable(string StoredProcName, SqlParameter[] parameters, CommandType sqlCommandType)
        {
            DataSet ds = new DataSet("result");

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                try
                {
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(StoredProcName, myConnection);
                    myCommand.Parameters.AddRange(parameters);
                    myCommand.CommandType = sqlCommandType;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = myCommand;
                    myCommand = null;
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        ds.Tables[0].TableName = "main result";
                }
                catch (Exception ex)
                {
                    //Log Error
                    string sqlquery = "Stored Proc: " + StoredProcName;
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }
            }

            return ds;

        }

        protected int ExecNonQuery(string StoredProcName, SqlParameter[] parameters, CommandType sqlCommandType)
        {
            int rowsAffected = 0;
            DataSet ds = new DataSet("result");
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Close();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(StoredProcName, myConnection);
                    myCommand.Parameters.AddRange(parameters);
                    myCommand.CommandType = sqlCommandType;
                    rowsAffected = myCommand.ExecuteNonQuery();
                    myCommand = null;
                }
                catch (Exception ex)
                {
                    //Log Error
                    string sqlquery = "Stored Proc: " + StoredProcName;
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }
                return rowsAffected;

            }
        }

        protected int ExecNonQuery(string SQLscript, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet("result");
            int rowsAffected = 0;
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Close();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(SQLscript, myConnection);
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }
                    myCommand.CommandType = CommandType.Text;
                    rowsAffected = myCommand.ExecuteNonQuery();
                    myCommand = null;
                }
                catch (Exception ex)
                {
                    //Log Error
                    string sqlquery = SQLscript;
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }
                return rowsAffected;
            }
        }

        protected int ExecNonQueryDelete(string SQLScript)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            return ExecNonQuery(SQLScript, parameters.ToArray());
        }

        protected int ExecNonQuery(string SQLScript)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            return ExecNonQuery(SQLScript, parameters.ToArray());
        }

        protected int ExecNonQuery(string sqlscript, string[] parameters)
        {
            int rowsAffected = 0;
            for (int i = 0; i < parameters.Count(); i++)
            {
                sqlscript = sqlscript.Replace(":param" + i, parameters[i].ToString());
            }
            DataSet ds = new DataSet("result");
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Close();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(sqlscript, myConnection);
                    myCommand.CommandType = CommandType.Text;
                    rowsAffected = myCommand.ExecuteNonQuery();
                    myCommand = null;
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    //Log Error
                    string sqlquery = sqlscript;
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }
                return rowsAffected;
            }

        }

        protected object ExecScalar(string SQLscript, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet("result");
            object firstData = new object();
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                try
                {
                    if (myConnection.State == ConnectionState.Open)
                        myConnection.Close();
                    myConnection.Open();
                    SqlCommand myCommand = new SqlCommand(SQLscript, myConnection);
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }
                    myCommand.CommandType = CommandType.Text;
                    firstData = myCommand.ExecuteScalar();
                    myCommand = null;
                }
                catch (Exception ex)
                {
                    //Log Error
                    string sqlquery = SQLscript;
                    Logging log = new Logging();
                    log.Error(ex, sqlquery);
                    throw ex;
                }
                finally
                {
                    //Ensure connection is always closed even if error
                    myConnection.Close();
                }
                return firstData;
            }
        }

        public DataTable RunQuery(string sqlScript)
        {
            DataTable dt = new DataTable();
            dt = QueryTable(sqlScript);
            return dt;
        }
    }
}