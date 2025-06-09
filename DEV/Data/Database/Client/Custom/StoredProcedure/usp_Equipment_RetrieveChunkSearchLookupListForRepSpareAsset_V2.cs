using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_Equipment_RetrieveChunkSearchLookupListForRepSpareAsset_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveChunkSearchLookupListForRepSpareAsset_V2";
        public usp_Equipment_RetrieveChunkSearchLookupListForRepSpareAsset_V2()
        {

        }

        public static List<b_Equipment> CallStoredProcedure(
    SqlCommand command,
    long callerUserInfoId,
    string callerUserName,
    b_Equipment obj
)
        {
            List<b_Equipment> records = new List<b_Equipment>();
            b_Equipment b_Equipment = new b_Equipment();
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Make", obj.Make, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Model", obj.Model, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 63);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Bit, "IsAssigned", obj.IsAssigned);

            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderColumn", obj.OrderbyColumn, 256);
            command.SetStringInputParameter(SqlDbType.VarChar, "OrderDirection", obj.OrderBy, 256);


            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    b_Equipment = b_Equipment.ProcessRowForChunkSearchLookupListForRepSpareAsset(reader);
                    records.Add(b_Equipment);
                }
            }
            catch (Exception ex)
            { }
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
