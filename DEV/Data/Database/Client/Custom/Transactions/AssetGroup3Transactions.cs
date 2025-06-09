using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;

namespace Database.Transactions
{

    public class RetrieveAssetGroup3ByClientIdSiteId : AssetGroup3_TransactionBaseClass
    {

        public List<b_AssetGroup3> AssetGroup3List { get; set; }

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
            List<b_AssetGroup3> tmpArray = null;
            AssetGroup3.AssetGroup3RetrieveByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup3List = new List<b_AssetGroup3>();
            foreach (b_AssetGroup3 tmpObj in tmpArray)
            {
                AssetGroup3List.Add(tmpObj);
            }
        }

    }


    public class Retrieve_AssetGroup3ByAssetGroup3Id : AssetGroup3_TransactionBaseClass
    {

        public List<b_AssetGroup3> AssetGroup3List { get; set; }

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
            List<b_AssetGroup3> tmpArray = null;
            AssetGroup3.AssetGroup3RetrieveByAssetGroup3Id(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup3List = new List<b_AssetGroup3>();
            foreach (b_AssetGroup3 tmpObj in tmpArray)
            {
                AssetGroup3List.Add(tmpObj);
            }
        }

    }


    public class Retrieve_AssetGroup3ByInActiveFlag_V2 : AssetGroup3_TransactionBaseClass
    {

        public List<b_AssetGroup3> AssetGroup3List { get; set; }

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
            List<b_AssetGroup3> tmpArray = null;
            AssetGroup3.AssetGroup3RetrieveByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup3List = new List<b_AssetGroup3>();
            foreach (b_AssetGroup3 tmpObj in tmpArray)
            {
                AssetGroup3List.Add(tmpObj);
            }
        }

    }

    public class RetrieveAll_AssetGroup3ByInActiveFlag_V2 : AssetGroup3_TransactionBaseClass
    {

        public List<b_AssetGroup3> AssetGroup3List { get; set; }

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
            List<b_AssetGroup3> tmpArray = null;
            AssetGroup3.AssetGroup3RetrieveAllByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup3List = new List<b_AssetGroup3>();
            foreach (b_AssetGroup3 tmpObj in tmpArray)
            {
                AssetGroup3List.Add(tmpObj);
            }
        }

    }

    public class AssetGroup3_ValidateNewClientLookupIdV2 : AssetGroup3_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup3.AssetGroup3_ValidateNewClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class AssetGroup3_ValidateOldClientLookupIdV2 : AssetGroup3_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup3.AssetGroup3ValidateOldClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
}
