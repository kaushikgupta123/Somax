using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class AlertDefine
    {


        public List<AlertDefine> RetrieveAll(DatabaseKey dbKey)
        {
            AlertDefine_RetrieveAll trans = new AlertDefine_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AlertDefineList);
        }


        public static List<AlertDefine> UpdateFromDatabaseObjectList(List<b_AlertDefine> dbObjs)
        {
            List<AlertDefine> result = new List<AlertDefine>();

            foreach (b_AlertDefine dbObj in dbObjs)
            {
                AlertDefine tmp = new AlertDefine();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
    }
}
