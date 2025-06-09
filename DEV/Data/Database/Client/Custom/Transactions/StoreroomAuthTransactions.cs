using Common.Enumerations;

using Database;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database
{
    
    public class StoreroomAuth_RetrieveChunkSearch : StoreroomAuth_TransactionBaseClass
    {

        public StoreroomAuth_RetrieveChunkSearch()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_StoreroomAuth> StoreroomAuthList { get; set; }

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
            List<b_StoreroomAuth> tmpArray = null;

            // Explicitly set tenant id from dbkey
            StoreroomAuth.ClientId = this.dbKey.Client.ClientId;
            StoreroomAuth.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            StoreroomAuthList = new List<b_StoreroomAuth>(tmpArray);

        }
    }
    
    public class StoreroomAuth_RetrieveAllStoreroomBySiteId : StoreroomAuth_TransactionBaseClass
    {

        public List<b_StoreroomAuth> StoreroomList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_StoreroomAuth> tmpList = null;
            StoreroomAuth.RetrieveAllStoreroomBySiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.StoreroomAuth, ref tmpList);
            StoreroomList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class StoreroomAuth_RetrieveByStoreroomAuthId_V2 : StoreroomAuth_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (StoreroomAuth.StoreroomAuthId == 0)
            {
                string message = "StoreroomAuth has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            StoreroomAuth.RetrieveByStoreroomAuthId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class StoreroomAuth_ValidateSiteId : StoreroomAuth_TransactionBaseClass
    {
        public StoreroomAuth_ValidateSiteId()
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

                StoreroomAuth.ValidateSiteId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
}
