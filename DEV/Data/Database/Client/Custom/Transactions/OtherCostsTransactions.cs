using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;

namespace Database.Client.Custom.Transactions
{
    public class OtherCostsTransactions : OtherCosts_TransactionBaseClass
    {
       

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
           
        }
    }

    public class OtherCosts_RetrieveByObjectId :OtherCosts_TransactionBaseClass
    {
        public List<b_OtherCosts> OtherCostList { get; set; }

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
            List<b_OtherCosts> tmpArray = null;


            OtherCosts.RetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            OtherCostList = new List<b_OtherCosts>();
            foreach (b_OtherCosts tmpObj in tmpArray)
            {
                OtherCostList.Add(tmpObj);
            }
        }
    }

    public class OtherCosts_SummeryRetrieveByObjectId : OtherCosts_TransactionBaseClass
    {
        public List<b_OtherCosts> OtherCostsList { get; set; }
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
            List<b_OtherCosts> tmpArray = null;


            OtherCosts.SummeryRetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            OtherCostsList = new List<b_OtherCosts>();
            foreach (b_OtherCosts tmpObj in tmpArray)
            {
                OtherCostsList.Add(tmpObj);
            }
        }

    }


    public class OtherCosts_RetrieveByTypeAndObjectId : OtherCosts_TransactionBaseClass
    {
        public List<b_OtherCosts> OtherCostList { get; set; }

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
            List<b_OtherCosts> tmpArray = null;


            OtherCosts.RetrieveByTypeAndObjectId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            OtherCostList = new List<b_OtherCosts>();
            foreach (b_OtherCosts tmpObj in tmpArray)
            {
                OtherCostList.Add(tmpObj);
            }
        }
    }



    public class OtherCosts_RetrieveByObjectIdandOtherCostId : OtherCosts_TransactionBaseClass
    {
        public List<b_OtherCosts> OtherCostList { get; set; }

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
            List<b_OtherCosts> tmpArray = null;


            OtherCosts.RetrieveByObjectIdandOtherCostId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            OtherCostList = new List<b_OtherCosts>();
            foreach (b_OtherCosts tmpObj in tmpArray)
            {
                OtherCostList.Add(tmpObj);
            }
        }
    }


}
