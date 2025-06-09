using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
namespace DataContracts
{
    public partial class AlertLocal
    {
        public List<AlertLocal> RetrieveAll(DatabaseKey dbKey)
        {
            AlertLocal_RetrieveAll trans = new AlertLocal_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AlertLocalList);
        }


        public static List<AlertLocal> UpdateFromDatabaseObjectList(List<b_AlertLocal> dbObjs)
        {
            List<AlertLocal> result = new List<AlertLocal>();

            foreach (b_AlertLocal dbObj in dbObjs)
            {
                AlertLocal tmp = new AlertLocal();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
    }
}
