using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.Transactions;
using Database;

namespace DataContracts
{
    public partial class STEventLog
    {
        public void CreateInAdmintSite(DatabaseKey dbKey)
        {
            STEventLog_CreateInAdminSite trans = new STEventLog_CreateInAdminSite();
            trans.STEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.STEventLog);
        }
    }
}
