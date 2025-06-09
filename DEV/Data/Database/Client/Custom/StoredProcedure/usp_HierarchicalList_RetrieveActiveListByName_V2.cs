using Database.Business;
using Database.SqlClient;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_HierarchicalList_RetrieveActiveListByName_V2 stored procedure.
    /// </summary>
    public class usp_HierarchicalList_RetrieveActiveListByName_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_HierarchicalList_RetrieveActiveListByName_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_HierarchicalList_RetrieveActiveListByName_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_HierarchicalList_RetrieveActiveListByName_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that identifies the clientid of user</param>
        /// <param name="siteId">long that identifies the siteid of user</param>
        /// <param name="ListName">string that identifies based on which data needs to be retrieved</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static ArrayList CallStoredProcedure(
            SqlCommand command,            
            long callerUserInfoId,
            string callerUserName,            
            b_HierarchicalList obj
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_LookupList record = null;
            SqlParameter RETURN_CODE_parameter = null;
            //SqlParameter        clientId_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

            // Setup clientId parameter.
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);

            command.SetStringInputParameter(SqlDbType.NVarChar, "ListName", obj.ListName, 30);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    records.Add((b_HierarchicalList)b_HierarchicalList.ProcessRowForList(reader));

                }
            }
            finally
            {
                if (null != reader)
                {
                    if (false == reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

            // Return the result
            return records;
        }
    }
}
