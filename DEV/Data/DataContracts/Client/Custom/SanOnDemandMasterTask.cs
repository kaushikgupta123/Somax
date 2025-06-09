using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class SanOnDemandMasterTask
    {
        #region Task All Work
        public List<SanOnDemandMasterTask> SanOnDemandMasterTask_RetrieveAllBy_SanOnDemandMasterId(DatabaseKey dbKey)
        {
            SanitationJobTask_RetrieveBy_SanOnDemandMasterId trans = new SanitationJobTask_RetrieveBy_SanOnDemandMasterId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanOnDemandMasterTask = ToDatabaseObject();
            trans.Execute();
            return UpdateFromDatabaseObjectListForSanOnDemandMaster(trans.SanOnDemandMasterTaskList);

        }

        public static List<SanOnDemandMasterTask> UpdateFromDatabaseObjectListForSanOnDemandMaster(List<b_SanOnDemandMasterTask> dbObjs)
        {
            List<SanOnDemandMasterTask> result = new List<SanOnDemandMasterTask>();

            foreach (b_SanOnDemandMasterTask dbObj in dbObjs)
            {
                SanOnDemandMasterTask tmp = new SanOnDemandMasterTask();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }




        #endregion

    }
}
