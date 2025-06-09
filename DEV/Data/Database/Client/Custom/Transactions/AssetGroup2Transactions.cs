using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;

namespace Database.Transactions
{

    public class RetrieveAssetGroup2ByClientIdSiteId : AssetGroup2_TransactionBaseClass
    {

        public List<b_AssetGroup2> AssetGroup2List { get; set; }

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
            List<b_AssetGroup2> tmpArray = null;
            AssetGroup2.AssetGroup2RetrieveByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup2List = new List<b_AssetGroup2>();
            foreach (b_AssetGroup2 tmpObj in tmpArray)
            {
                AssetGroup2List.Add(tmpObj);
            }
        }

    }


    public class Retrieve_AssetGroup2ByAssetGroup2Id : AssetGroup2_TransactionBaseClass
    {

        public List<b_AssetGroup2> AssetGroup2List { get; set; }

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
            List<b_AssetGroup2> tmpArray = null;
            AssetGroup2.AssetGroup2RetrieveByAssetGroup2Id(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup2List = new List<b_AssetGroup2>();
            foreach (b_AssetGroup2 tmpObj in tmpArray)
            {
                AssetGroup2List.Add(tmpObj);
            }
        }

    }


    public class Retrieve_AssetGroup2ByInActiveFlag_V2 : AssetGroup2_TransactionBaseClass
    {

        public List<b_AssetGroup2> AssetGroup2List { get; set; }

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
            List<b_AssetGroup2> tmpArray = null;
            AssetGroup2.AssetGroup2RetrieveByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup2List = new List<b_AssetGroup2>();
            foreach (b_AssetGroup2 tmpObj in tmpArray)
            {
                AssetGroup2List.Add(tmpObj);
            }
        }

    }

    public class RetrieveAll_AssetGroup2ByInActiveFlag_V2 : AssetGroup2_TransactionBaseClass
    {

        public List<b_AssetGroup2> AssetGroup2List { get; set; }

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
            List<b_AssetGroup2> tmpArray = null;
            AssetGroup2.AssetGroup2RetrieveAllByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup2List = new List<b_AssetGroup2>();
            foreach (b_AssetGroup2 tmpObj in tmpArray)
            {
                AssetGroup2List.Add(tmpObj);
            }
        }

    }


    public class AssetGroup2_ValidateNewClientLookupIdV2 : AssetGroup2_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup2.AssetGroup2_ValidateNewClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class AssetGroup2_ValidateOldClientLookupIdV2 : AssetGroup2_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup2.AssetGroup2ValidateOldClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
}
