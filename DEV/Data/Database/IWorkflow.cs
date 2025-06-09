using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Database.Business
{
    public interface IWorkflow
    {
          void UpdateWorkflowObjectInDatabase(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName);
    }
}
