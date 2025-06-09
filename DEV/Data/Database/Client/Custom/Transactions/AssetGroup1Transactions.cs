using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;

namespace Database.Transactions
{

    public class RetrieveAssetGroup1ByClientIdSiteId : AssetGroup1_TransactionBaseClass
    {

        public List<b_AssetGroup1> AssetGroup1List { get; set; }

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
            List<b_AssetGroup1> tmpArray = null;
            AssetGroup1.AssetGroup1RetrieveByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup1List = new List<b_AssetGroup1>();
            foreach (b_AssetGroup1 tmpObj in tmpArray)
            {
                AssetGroup1List.Add(tmpObj);
            }
        }

    }

    public class Retrieve_AssetGroup1ByAssetGroup1Id : AssetGroup1_TransactionBaseClass
    {

        public List<b_AssetGroup1> AssetGroup1List { get; set; }

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
            List<b_AssetGroup1> tmpArray = null;
            AssetGroup1.AssetGroup1RetrieveByAssetGroup1Id(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup1List = new List<b_AssetGroup1>();
            foreach (b_AssetGroup1 tmpObj in tmpArray)
            {
                AssetGroup1List.Add(tmpObj);
            }
        }

    }

    public class Retrieve_AssetGroup1ByInActiveFlag_V2 : AssetGroup1_TransactionBaseClass
    {

        public List<b_AssetGroup1> AssetGroup1List { get; set; }

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
            List<b_AssetGroup1> tmpArray = null;
            AssetGroup1.AssetGroup1RetrieveByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup1List = new List<b_AssetGroup1>();
            foreach (b_AssetGroup1 tmpObj in tmpArray)
            {
                AssetGroup1List.Add(tmpObj);
            }
        }

    }

    public class RetrieveAll_AssetGroup1ByInActiveFlag_V2 : AssetGroup1_TransactionBaseClass
    {

        public List<b_AssetGroup1> AssetGroup1List { get; set; }

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
            List<b_AssetGroup1> tmpArray = null;
            AssetGroup1.AssetGroup1RetrieveAllByInActiveFlag_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            AssetGroup1List = new List<b_AssetGroup1>();
            foreach (b_AssetGroup1 tmpObj in tmpArray)
            {
                AssetGroup1List.Add(tmpObj);
            }
        }

    }

    public class AssetGroup1_ValidateOldClientLookupIdV2 : AssetGroup1_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup1.AssetGroup1ValidateOldClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class AssetGroup1_ValidateNewClientLookupIdV2 : AssetGroup1_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            AssetGroup1.AssetGroup1_ValidateNewClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

}
