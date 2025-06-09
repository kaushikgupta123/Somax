using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public abstract class DataBusinessBase
    {
        #region Abstract Properties
        public long ClientId { get; set; }
        #endregion

        #region Abstract Functions
        public abstract void InsertIntoDatabase(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName);
        public abstract void UpdateInDatabase(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName);
        public abstract void DeleteFromDatabase(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName);
        public abstract void RetrieveByPKFromDatabase(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName);
        #endregion

        public static List<string> RetrieveDistinctEntriesFromTable(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, 
            string callerUserName, long clientId, string table, string column)
        {
            SqlCommand command = null;
            List<string> results = new List<string>();

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                results = Database.StoredProcedure.sql_Custom_GetDistinctColumnEntriesFromTable.CallStoredProcedure(command, callerUserInfoId, callerUserName, clientId, table, column);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
            return results;
        }

        public static long RetrieveIdByClientLookupId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId,
            string callerUserName, long clientId, string table, string lookupId)
        {
            SqlCommand command = null;
            long results = 0;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                results = Database.StoredProcedure.usp_All_RetrieveIdByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, clientId, table, lookupId);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
            return results;
        }
    }
}
