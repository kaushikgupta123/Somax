using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class AlertFollow
    {
        public List<AlertFollow> RetrieveAll(DatabaseKey dbKey)
        {
            AlertFollow_RetrieveAll trans = new AlertFollow_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
               
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AlertFollowList);
        }


        public static List<AlertFollow> UpdateFromDatabaseObjectList(List<b_AlertFollow> dbObjs)
        {
            List<AlertFollow> result = new List<AlertFollow>();

            foreach (b_AlertFollow dbObj in dbObjs)
            {
                AlertFollow tmp = new AlertFollow();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void AlertFollow_RetrieveByObjectForUser(DatabaseKey dbKey)
        {
            AlertFollow_RetrieveByObjectForUser_Transactions trans = new AlertFollow_RetrieveByObjectForUser_Transactions
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.AlertFollow = new b_AlertFollow();
            trans.AlertFollow.AlertFollowId = -1;
            trans.AlertFollow.ClientId = this.ClientId;
            trans.AlertFollow.UserInfoId = this.UserInfoId;
            trans.AlertFollow.ObjectId = this.ObjectId;
            trans.AlertFollow.ObjectType = this.ObjectType;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AlertFollow);
        }
    }
}
