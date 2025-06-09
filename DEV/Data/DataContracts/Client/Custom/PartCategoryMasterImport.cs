using Database;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class PartCategoryMasterImport
    {
        public int error_message_count { get; set; }
        #region process 

        public void Create_PartCategoryMasterProcessInterface(DatabaseKey dbKey)
        {
            PartCategoryMasterImport_ProcessImport trans = new PartCategoryMasterImport_ProcessImport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartCategoryMasterImport = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PartCategoryMasterImport);
            this.error_message_count = trans.PartCategoryMasterImport.error_message_count;
    }
        #endregion

        public List<PartCategoryMasterImport> PartCategoryMasterImportRetrieveAll(DatabaseKey dbKey)
        {
            PartCategoryMasterImport_RetrieveAll trans = new PartCategoryMasterImport_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PartCategoryMasterImportList);
        }
        public static List<PartCategoryMasterImport> UpdateFromDatabaseObjectList(List<b_PartCategoryMasterImport> dbObjs)
        {
            List<PartCategoryMasterImport> result = new List<PartCategoryMasterImport>();

            foreach (b_PartCategoryMasterImport dbObj in dbObjs)
            {
                PartCategoryMasterImport tmp = new PartCategoryMasterImport();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
       
        public void RetrieveClientLookupID(DatabaseKey dbKey)
        {
            PartCategoryMasterImport_RetrieveByClientLookUpID trans = new PartCategoryMasterImport_RetrieveByClientLookUpID();
            //trans._ClientLookupID = this.ClientLookupId;
            trans.PartCategoryMasterImport = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartCategoryMasterImport);
        }
    }
}
