using System.Collections.Generic;
using Database.Business;
using System;

namespace Database
{   
    public class UserReportGridDefintion_RetrieveByReportId_V2 : UserReportGridDefintion_TransactionBaseClass
    {

            public override void PerformLocalValidation()
            {
                base.UseTransaction = false;    // moved from PerformWorkItem
                base.PerformLocalValidation();
                //if (UserReportGridDefintion.UserReportGridDefintionId == 0)
                //{
                //    string message = "UserReportGridDefintion has an invalid ID.";
                //    throw new Exception(message);
                //}
            }

            public override void PerformWorkItem()
            {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            UserReportGridDefintion.RetrieveByReportIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            }
        }

    public class UserReportGridDefintion_RetrieveByReportId : UserReportGridDefintion_TransactionBaseClass
    {
        public List<b_UserReportGridDefintion> UserReportGridDefintionList { get; set; }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_UserReportGridDefintion> tmpArray = null;
            UserReportGridDefintion.RetrieveByReportId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            UserReportGridDefintionList = tmpArray;
        }
    }

    public class UserReportGridDefintion_UpdateByReportId : UserReportGridDefintion_TransactionBaseClass   
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserReportGridDefintion.ReportId == 0)
            {
                string message = "UserReportGridDefintion has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserReportGridDefintion.UpdateByReportId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserReportGridDefintion.ReportId > 0);
        }
    }


    public class UserReportGridDefintion_RetrieveAllByReportId_V2 : UserReportGridDefintion_TransactionBaseClass
    {
        public List<b_UserReportGridDefintion> UserReportGridDefintionList { get; set; }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_UserReportGridDefintion> tmpArray = null;
            UserReportGridDefintion.RetrieveAllByReportId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            UserReportGridDefintionList = tmpArray;
        }
    }

    public class UserReportGridDefintion_DeleteByReportId : UserReportGridDefintion_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserReportGridDefintion.ReportId == 0)
            {
                string message = "UserReportGridDefintion has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserReportGridDefintion.DeleteByReportId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserReportGridDefintion.ReportId > 0);
        }
    }
    public class UserReportGridDefintion_UpdateFilterByReportId : UserReportGridDefintion_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserReportGridDefintion.ReportId == 0)
            {
                string message = "UserReportGridDefintion has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserReportGridDefintion.UpdateFilterByReportId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserReportGridDefintion.ReportId > 0);
        }
    }    
}
