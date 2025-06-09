/*
 ******************************************************************************
 * Added By INDUSNET TECHNOLOGIES
 ****************************************************************************** 
 *This class is being used as a data layer to access DB objects and files
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Common.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace INTDataLayer.DAL
{
    /// <summary>
    /// <rem>Enumeration defining direction of paramater passed in SQL Query or Store Procedure.</rem>
    /// </summary>
    public enum QyParameterDirection : int
    {
        Input = 1,
        Output = 2,
        Return = 3
    }

    public class ProcedureExecute : IDisposable
    {
        private string strCommandText = string.Empty;
        private bool blnSP = true;

        // Sql Parameter collection object
        private ArrayList oParameters = new ArrayList();

        private bool blnLocalConn = true;

        #region Constrator
        /// <summary>
        /// <remarks>Constructor create object to execute a store procedure</remarks>
        /// </summary>
        /// <param name="StoredProcName"></param>
        public ProcedureExecute(string StoredProcName)
            : this(StoredProcName, false)
        {

        }
        /// <summary>
        /// <remarks>Constructor create object to execute a SQL Query String</remarks>
        /// </summary>
        /// <param name="SqlString"></param>
        /// <param name="IsTextQuery"></param>
        public ProcedureExecute(string SqlString, bool IsTextQuery)
        {
            blnSP = !IsTextQuery;
            strCommandText = SqlString;
        }

        #endregion Constrator

        #region DataTable
        /// <summary>
        /// Return a datatable object after fetching data from database
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            DataTable dt = null;
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd);

            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();

            da.Fill(ds);
            if ((null != ds) && (ds.Tables.Count > 0))
            {
                dt = ds.Tables[0];
            }

            if (this.blnLocalConn)
            {
                this.oConn.Close();
            }
            oCmd.Dispose();

            return dt;
        }

        /// <summary>
        /// <remarks>Return a datatable object after fetching data from database.</remarks>
        /// <remarks>ConnectionStr :- Connection string of SQL Server database.</remarks>
        /// </summary>
        /// <param name="ConnectionStr"></param>
        /// <returns></returns>
        public DataTable GetTable(string ConnectionStr)
        {
                DataTable dt = null;
                SqlCommand oCmd = new SqlCommand();
                this.InitQuery(oCmd, ConnectionStr);

                SqlDataAdapter da = new SqlDataAdapter(oCmd);
                DataSet ds = new DataSet();

                da.Fill(ds);
                if ((null != ds) && (ds.Tables.Count > 0))
                {
                    dt = ds.Tables[0];
                }

                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                oCmd.Dispose();

            return dt;
        }

        #endregion DataTable

        #region DataReader
        /// <summary>
        /// <remarks>Return a SqlDataReader object after fetching data from database</remarks>
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetReader()
        {
            SqlDataReader dr = null;
            try
            {
                string ConnectionStr = String.Empty;
                SqlCommand oCmd = new SqlCommand();
                this.InitQuery(oCmd, ConnectionStr);
                dr = oCmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception v1)
            {
                return dr;
            }
        }

        public SqlDataReader GetReader(string ConnectionStr)
        {
            SqlDataReader dr = null;
            try
            {

                SqlCommand oCmd = new SqlCommand();
                this.InitQuery(oCmd, ConnectionStr);
                dr = oCmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception v1)
            {
                return dr;

            }
        }
        #endregion DataReader

        #region DataSet

        // REturn a Datatable
        /// <summary>
        /// <remarks>Return a dataset object after fetching data from database</remarks>
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            string ConnectionStr = String.Empty;
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd, ConnectionStr);

            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                oCmd.Dispose();
            }

            return ds;
        }

        /// <summary>
        /// <remarks>Return a dataset object after fetching data from database</remarks>
        /// <remarks>ConnectionStr :- Connection string of SQL Server database.</remarks>
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string ConnectionString)
        {
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd, ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                oCmd.Dispose();
            }

            return ds;
        }



        #endregion DataSet

        #region NonQuery
        /// <summary>
        /// <remarks>Execute a Non Query for Inserting/Deleting/updating records in database and return execution status</remarks>
        /// </summary>
        /// <param name="ConnectionStr"></param>
        /// <returns></returns>
        public int RunActionQuery(string ConnectionStr)
        {
            int intRowsAffected = -1;

            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd, ConnectionStr);

            try
            {
                intRowsAffected = oCmd.ExecuteNonQuery();
                oCmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                oCmd.Dispose();
            }

            return intRowsAffected;
        }


        
        #endregion NonQuery

        #region Scalar
        /// <summary>
        /// Executing a query by Executescaler() command and return scaler value i.e. First column of first row.
        /// </summary>
        /// <returns></returns>
        public object GetScalar()
        {
            object oRetVal = null;

            string ConnectionStr = String.Empty;
            SqlCommand oCmd = new SqlCommand();
            this.InitQuery(oCmd, ConnectionStr);

            try
            {
                oRetVal = oCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.blnLocalConn)
                {
                    this.oConn.Close();
                }
                oCmd.Dispose();
            }

            return oRetVal;
        }

        #endregion Scalar

        #region Initializes a Query

        /// <summary>
        /// <remarks>Initializing a Query, Opening a database connection and setting parameter to SqlCommand</remarks>
        /// </summary>
        /// <param name="oCmd"></param>
        private void InitQuery(SqlCommand oCmd)
        {
            blnLocalConn = (this.oConn == null);
            if (blnLocalConn)
            {
                string conn = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
                Open(conn);
                blnLocalConn = true;
            }
            oCmd.Connection = oConn;

            oCmd.CommandText = this.strCommandText;
            oCmd.CommandType = (this.blnSP ? CommandType.StoredProcedure : CommandType.Text);

            oCmd.CommandTimeout = (24 * 60 * 60);	// 1 Day

            foreach (object oItem in this.oParameters)
            {
                oCmd.Parameters.Add((SqlParameter)oItem);
            }
        }

        /// <summary>
        /// <remarks>Initializing a Query, Opening a database connection and setting parameter to SqlCommand</remarks>
        /// <remarks>ConnectionStr :- Connection string of SQL Server database.</remarks>
        /// </summary>
        /// <param name="oCmd"></param>
        /// <param name="ConnectionStr"></param>
        private void InitQuery(SqlCommand oCmd, string ConnectionStr)
        {
            blnLocalConn = (this.oConn == null);
            if (blnLocalConn)
            {
                string conn = ConnectionStr;
                Open(conn);
                blnLocalConn = true;
            }
            oCmd.Connection = oConn;

            oCmd.CommandText = this.strCommandText;
            oCmd.CommandType = (this.blnSP ? CommandType.StoredProcedure : CommandType.Text);

            oCmd.CommandTimeout = (24 * 60 * 60);	// 1 Day

            foreach (object oItem in this.oParameters)
            {
                oCmd.Parameters.Add((SqlParameter)oItem);
            }
        }

        #endregion Initializes a Query

        #region Parameter handling

        #region Type: Table Value Parameter

        /// <summary>
        /// <remarks>Add a table value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddTVP(string Name, DataTable Value)
        {
            AddTVP(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a table value parameter with direction of parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddTVP(string Name, DataTable Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Structured);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Table Value Parameter

        #region Type: Integer

        /// <summary>
        ///<remarks> Add a Integer value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddIntegerPara(string Name, int Value)
        {
            AddIntegerPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Integer value parameter with direction of parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddIntegerPara(string Name, int? Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }

        /// <summary>
        ///<remarks> Add a Null value parameter in Sql Command for an integer type parameter </remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddIntegerNullPara(string Name)
        {
            AddIntegerNullPara(Name, QyParameterDirection.Input);
        }

        /// <summary>
        ///<remarks> Add a Null value parameter in Sql Command for an integer type parameter with parameter direction.</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Direction"></param>
        public void AddIntegerNullPara(string Name, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = DBNull.Value;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Integer

        #region Type: BigInt

        /// <summary>
        /// <remarks> Add a Long/BigInt/Int64 value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddBigIntegerPara(string Name, long Value)
        {
            AddBigIntegerPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Long/BigInt/Int64 value parameter with direction of parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddBigIntegerPara(string Name, long Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.BigInt);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }

        /// <summary>
        /// <remarks>Add a Null value parameter in Sql Command for an BigInt type parameter</remarks>
        /// </summary>
        /// <param name="Name"></param>
        public void AddBigIntegerNullPara(string Name)
        {
            AddIntegerNullPara(Name, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Null value parameter in Sql Command for an BigInt type parameter with parameter direction</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Direction"></param>
        public void AddBigIntegerNullPara(string Name, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Int);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = DBNull.Value;
            this.oParameters.Add(oPara);
        }

        #endregion Type: BigInt

        #region Type: Char

        /// <summary>
        /// <remarks>Add a char value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        public void AddCharPara(string Name, int Size, char Value)
        {
            AddCharPara(Name, Size, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a char value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddCharPara(string Name, int Size, char Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value.Equals(null))
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Char, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Char

        #region Type: NChar

        /// <summary>
        /// <remarks>Add a Nchar value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        public void AddNCharPara(string Name, int Size, char Value)
        {
            AddNCharPara(Name, Size, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Nchar value parameter in with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddNCharPara(string Name, int Size, char Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value.Equals(null))
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: NChar

        #region Type: Varchar

        /// <summary>
        /// <remarks> Add a varchar value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        public void AddVarcharPara(string Name, int Size, string Value)
        {
            AddVarcharPara(Name, Size, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a varchar value parameter in Sql Command with parameter direction</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddVarcharPara(string Name, int Size, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }

            SqlParameter oPara = new SqlParameter(Name, SqlDbType.VarChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Varchar

        #region Type: NVarchar

        /// <summary>
        /// <remarks> Add a Nvarchar value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        public void AddNVarcharPara(string Name, int Size, string Value)
        {
            AddNVarcharPara(Name, Size, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Nvarchar value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Size"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddNVarcharPara(string Name, int Size, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NVarChar, Size);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: NVarchar

        #region Type: Boolean

        /// <summary>
        /// <remarks>Add a Boolean value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddBooleanPara(string Name, bool Value)
        {
            AddBooleanPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Boolean value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddBooleanPara(string Name, bool Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Bit);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Boolean

        #region Type: DateTime

        /// <summary>
        /// <remarks>Add a DateTime value parameter in Sql command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddDateTimePara(string Name, DateTime Value)
        {
            AddDateTimePara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks> Add a DateTime value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddDateTimePara(string Name, DateTime Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.DateTime);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }


        /// <summary>
        /// <remarks>Add a DateTime value parameter in Sql command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddDateTimePara(string Name, DateTime? Value)
        {
            if (Value != null)
            {
                AddDateTimePara(Name, Value.Value, QyParameterDirection.Input);
            }
            else
            {
                AddDateTimePara(Name, null, QyParameterDirection.Input);
            }

        }

        /// <summary>
        /// <remarks> Add a DateTime value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddDateTimePara(string Name, DateTime? Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.DateTime);
            oPara.Direction = GetParaType(Direction);
            if (Value != null)
            {
                oPara.Value = null;
            }
            else
            {
                oPara.Value = Value;
            }
            this.oParameters.Add(oPara);
        }

        #endregion Type: DateTime

        #region Type: Text

        /// <summary>
        /// <remarks>Add a Text value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddTextPara(string Name, string Value)
        {
            AddTextPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Text value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddTextPara(string Name, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Text);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Text

        #region Type: NText

        /// <summary>
        /// <remarks>Add a NText Parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddNTextPara(string Name, string Value)
        {
            AddNTextPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a NText Parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddNTextPara(string Name, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.NText);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: NText

        #region Type: Decimal

        /// <summary>
        /// <remarks>Add a Demimal value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Scale"></param>
        /// <param name="Precision"></param>
        /// <param name="Value"></param>
        public void AddDecimalPara(string Name, byte Scale, byte Precision, decimal Value)
        {
            AddDecimalPara(Name, Scale, Precision, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Decimal value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Scale"></param>
        /// <param name="Precision"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddDecimalPara(string Name, byte Scale, byte Precision, decimal Value, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Decimal);
            oPara.Scale = Scale;
            oPara.Precision = Precision;
            oPara.Direction = GetParaType(Direction);
            oPara.Value = Value;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Decimal

        #region Type: Image

        /// <summary>
        /// <remarks>Add a Image(byte Array) parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddImagePara(string Name, byte[] Value)
        {
            AddImagePara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a Image(byte array) parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddImagePara(string Name, byte[] Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Image);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: Image

        #region Type: XML

        /// <summary>
        /// <remarks>Add a XML(string) Parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddXMLPara(string Name, string Value)
        {
            AddXMLPara(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a XML(string) parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddXMLPara(string Name, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
                oValue = DBNull.Value;

            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Xml);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }

        #endregion Type: XML

        #region Type: Varbaniry Value Parameter

        /// <summary>
        /// <remarks>Add a structured parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void AddVarbinary(string Name, string Value)
        {
            AddVarbinary(Name, Value, QyParameterDirection.Input);
        }

        /// <summary>
        /// <remarks>Add a structured parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Direction"></param>
        public void AddVarbinary(string Name, string Value, QyParameterDirection Direction)
        {
            object oValue = (object)Value;
            if (Value == null)
            {
                oValue = DBNull.Value;
            }
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Structured);
            oPara.Direction = GetParaType(Direction);
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        public void AddVarbinaryData(string Name, byte[] Value)
        {
            byte[] oValue = (byte[])Value;           
            SqlParameter oPara = new SqlParameter(Name, SqlDbType.Binary);           
            oPara.Value = oValue;
            this.oParameters.Add(oPara);
        }
        #endregion Type: Varbaniry Value Parameter

        #region Adds a NULL value Parameter
        /// <summary>
        /// <remarks>Add a Null Value parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        public void AddNullValuePara(string Name)
        {
            SqlParameter oPara = new SqlParameter(Name, DBNull.Value);
            oPara.Direction = ParameterDirection.Input;
            this.oParameters.Add(oPara);
        }

        /// <summary>
        /// <remarks>Add a Null Value parameter with parameter direction in Sql Command</remarks>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Direction"></param>
        public void AddNullValuePara(string Name, QyParameterDirection Direction)
        {
            SqlParameter oPara = new SqlParameter(Name, DBNull.Value);
            oPara.Direction = GetParaType(Direction);
            this.oParameters.Add(oPara);
        }

        #endregion Adds a NULL value Parameter

        #region Adds the Return Parameter
        /// <summary>
        /// <remarks>Add a Return Parameter which return an integer.</remarks>
        /// </summary>
        public void AddReturnPara()
        {
            this.AddIntegerPara("ReturnIntPara", 0, QyParameterDirection.Return);
        }

        #endregion Adds the Return Parameter

        #region Returns the value of the passed parameter
        /// <summary>
        /// <remarks>Return the value of passed parameter in Sql Command</remarks>
        /// </summary>
        /// <param name="ParaName"></param>
        /// <returns></returns>
        public object GetParaValue(string ParaName)
        {
            object oValue = null;
            SqlParameter oPara = null;

            ParaName = ParaName.Trim().ToLower();
            foreach (object oItem in this.oParameters)
            {
                oPara = (SqlParameter)oItem;
                if (oPara.ParameterName.ToLower() == ParaName)
                {
                    oValue = oPara.Value;
                    break;
                }
            }

            return oValue;
        }

        #endregion Returns the value of the passed parameter

        #region Returns the value of the Return Parameter

        /// <summary>
        /// <remarks>Return the object of Return Parameter</remarks>
        /// </summary>
        /// <returns></returns>
        public object GetReturnParaValue()
        {
            return this.GetParaValue("ReturnIntPara");
        }

        #endregion Returns the value of the Return Parameter

        #region Clears the parameters

        /// <summary>
        /// <remarks>Clear the parameter collection object</remarks>
        /// </summary>
        public void ClearParameters()
        {
            this.oParameters.Clear();
        }

        #endregion Clears the parameters

        #region Converts enum to parameter direction

        /// <summary>
        /// <remarks>Set the direction of parameter</remarks>
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        private ParameterDirection GetParaType(QyParameterDirection Direction)
        {
            switch (Direction)
            {
                case QyParameterDirection.Output:
                    return ParameterDirection.InputOutput;
                case QyParameterDirection.Return:
                    return ParameterDirection.ReturnValue;
                default:
                    return ParameterDirection.Input;
            }
        }

        #endregion Converts enum to parameter direction

        #endregion Parameter handling

        #region Dispose

        /// <summary>
        /// <remarks>Disposing connection object and clear Parameter collection object</remarks>
        /// </summary>
        public void Dispose()
        {
            this.oConn.Dispose();
            this.oParameters.Clear();
        }

        #endregion Dispose

        #region Opens a connection
        /// <summary>
        /// <remarks>Create a Sql Connection and establish connection</remarks>
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public bool Open(string ConnectionString)
        {
            blnIsOpen = false;
            oConn = new SqlConnection(ConnectionString);
            oConn.Open();
            blnIsOpen = true;
            return blnIsOpen;
        }

        #endregion Opens a connection

        #region Connection

        private SqlConnection oConn = null;

        /// <summary>
        /// <remarks>Set Sql Connection</remarks>
        /// </summary>
        public SqlConnection Connection
        {
            set
            {
                oConn = value;
            }
        }

        #endregion Connection

        #region IsOpen

        private bool blnIsOpen = false;

        /// <summary>
        /// <remarks>Get boolean value indicating connection is open or not</remarks>
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return blnIsOpen;
            }
        }

        #endregion IsOpen
        #region RKL-Mail Report Timeout
        private void InitQueryReport(SqlCommand oCmd, string ConnectionStr ,ref int ReportTimeOut)
        {
           ReportTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["ReportTimeOut"].ToString());
            try
            {
                blnLocalConn = (this.oConn == null);
                if (blnLocalConn)
                {
                    string conn = ConnectionStr;
                    Open(conn);
                    blnLocalConn = true;
                }
                oCmd.Connection = oConn;

                oCmd.CommandText = this.strCommandText;
                oCmd.CommandType = (this.blnSP ? CommandType.StoredProcedure : CommandType.Text);

                oCmd.CommandTimeout = ReportTimeOut * 60; //*(24 * 60 * 60)*/  // 1 Day

                foreach (object oItem in this.oParameters)
                {
                    oCmd.Parameters.Add((SqlParameter)oItem);
                }

            }
            catch (SqlException ex)
            {
                string msg = ex.Message;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public DataTable GetTableReport(string ConnectionStr, ref string timeoutError)
        {
            int ReportTimeOut = 0;
            DataTable dt = null;
            SqlCommand oCmd = new SqlCommand();
            this.InitQueryReport(oCmd, ConnectionStr,ref ReportTimeOut);
            TimeSpan timeout = TimeSpan.FromMinutes(ReportTimeOut);
            string errormsg = string.Empty;
            var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Create a task to execute the command
                var task = Task.Run(() =>
                {
                    //oCmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(oCmd);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    if ((null != ds) && (ds.Tables.Count > 0))
                    {
                        dt = ds.Tables[0];
                    }

                    if (this.blnLocalConn)
                    {
                        this.oConn.Close();
                    }

                    oCmd.Dispose();
                }, cancellationTokenSource.Token);

                // Wait for the task to complete or timeout
                if (!task.Wait(timeout))
                {
                    // Cancel the task if it exceeds the timeout
                    cancellationTokenSource.Cancel();
                    throw new OperationCanceledException("The operation was canceled due to timeout.");
                }
            }
           
            catch (AggregateException ex)
            {
                // Unwrap AggregateException to get the inner exceptions
                foreach (var innerEx in ex.InnerExceptions)
                {
                    if (innerEx is SqlException sqlEx)
                    {
                        throw sqlEx;
                    }
                    if (innerEx is OperationCanceledException cancelEx)
                    {
                        throw cancelEx;
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex is InvalidOperationException exinvalid)
                {
                    throw exinvalid;
                }
                else 
                {

                    throw ex;
                }             

            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
            timeoutError = errormsg;
            return dt;
        }
        #endregion
    }
}
