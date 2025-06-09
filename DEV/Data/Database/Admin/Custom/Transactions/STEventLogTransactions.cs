using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;
namespace Database.Transactions
{
    public class STEventLog_CreateInAdminSite : AbstractTransactionManager
    {
        public STEventLog_CreateInAdminSite()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_STEventLog STEventLog { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (STEventLog.STEventLogId > 0)
            {
                string message = "STEventLog has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            STEventLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
}
