using Common.Enumerations;

using Database;
using Database.Business;
using Database.StoredProcedure;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class ApprovalGroupSettings_Retrieve_V2 : ApprovalGroupSettings_TransactionBaseClass
    {
        public List<b_ApprovalGroupSettings> ApprovalGroupSettingsList { get; set; }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            b_ApprovalGroupSettings[] tmpArray = null;
            b_ApprovalGroupSettings ObjApproval = new b_ApprovalGroupSettings();
            ObjApproval.ClientId = dbKey.Client.ClientId;
            ObjApproval.SiteId = ApprovalGroupSettings.SiteId;
            ObjApproval.RetreiveApprovalGroupSettingsList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ApprovalGroupSettingsList = new List<b_ApprovalGroupSettings>(tmpArray);
        }
    }
}
