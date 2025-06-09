using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class AppGroupApprovers_RetrieveByApprovalGroupIdV2 : AppGroupApprovers_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AppGroupApprovers.AppGroupApproversId > 0)
            {
                string message = "AppGroupApproversId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_AppGroupApprovers tmpList = null;
            AppGroupApprovers.RetrieveAppGroupApproversChildGridV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class AppGroupApprovers_RetrieveChunkSearchFromDetails : AppGroupApprovers_TransactionBaseClass
    {
        public List<b_AppGroupApprovers> appGroupApproversList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AppGroupApprovers.AppGroupApproversId > 0)
            {
                string message = "AppGroupApproversId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_AppGroupApprovers> tmpList = null;
            AppGroupApprovers.RetrieveChunkSearchFromDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            appGroupApproversList = new List<b_AppGroupApprovers>();
            foreach (var item in tmpList)
            {
                appGroupApproversList.Add(item);
            }
        }
    }

    public class AppGroupApprovers_RetrieveById_V2 : AppGroupApprovers_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (AppGroupApprovers.AppGroupApproversId == 0)
            {
                string message = "AppGroupApprovers has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            AppGroupApprovers.RetrieveByIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class AppGroupApprovers_ValidateApproverId : AppGroupApprovers_TransactionBaseClass
    {
        public AppGroupApprovers_ValidateApproverId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                AppGroupApprovers.ValidateApproverId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    #region V2-726
    public class AppGroupApprovers_RetrieveApproversForApproval : AppGroupApprovers_TransactionBaseClass
    {

        public List<b_AppGroupApprovers> ApproverList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_AppGroupApprovers> tmpList = null;
            AppGroupApprovers.RetrieveApproversForApproval(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.AppGroupApprovers, ref tmpList);
            ApproverList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region V2-720
    public class AppGroupApprovers_ValidateUpperLevelExists_V2 : AppGroupApprovers_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AppGroupApprovers.ValidateUpperLevelExists(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    #endregion

    #region V2-730
    public class AppGroupApprovers_RetrieveApproversForMultiLevelApproval : AppGroupApprovers_TransactionBaseClass
    {

        public List<b_AppGroupApprovers> ApproverList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_AppGroupApprovers> tmpList = null;
            AppGroupApprovers.RetrieveApproversForMultilevelApproval(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.AppGroupApprovers, ref tmpList);
            ApproverList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion
}
