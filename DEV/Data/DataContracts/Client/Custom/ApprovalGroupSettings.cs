using System;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    public partial class ApprovalGroupSettings : DataContractBase
    {
        public List<ApprovalGroupSettings> RetrieveApprovalGroupSettings_V2(DatabaseKey dbKey)
        {
            ApprovalGroupSettings_Retrieve_V2 trans = new ApprovalGroupSettings_Retrieve_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ApprovalGroupSettings = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ApprovalGroupSettings> approvalGroupSettingslist = new List<ApprovalGroupSettings>();
            foreach (b_ApprovalGroupSettings approvalGroupSettings in trans.ApprovalGroupSettingsList)
            {
                ApprovalGroupSettings tmp = new ApprovalGroupSettings();
                tmp.UpdateFromDatabaseObject(approvalGroupSettings);
                approvalGroupSettingslist.Add(tmp);
            }

            return approvalGroupSettingslist;
        }
        #region V2-730
        public b_ApprovalGroupSettings ToDatabaseObjectForLogin()
        {
            b_ApprovalGroupSettings dbObj = new b_ApprovalGroupSettings();
            dbObj.WorkRequests = this.WorkRequests;
            dbObj.PurchaseRequests = this.PurchaseRequests;
            dbObj.MaterialRequests = this.MaterialRequests;
            dbObj.SanitationRequests = this.SanitationRequests;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForLogin(b_ApprovalGroupSettings dbObj)
        {
            this.WorkRequests = dbObj.WorkRequests;
            this.PurchaseRequests = dbObj.PurchaseRequests;
            this.MaterialRequests = dbObj.MaterialRequests;
            this.SanitationRequests = dbObj.SanitationRequests;
        }
        #endregion
    }

}
