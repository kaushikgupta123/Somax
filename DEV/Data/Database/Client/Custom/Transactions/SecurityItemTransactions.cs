
/*
 *  Added By Indusnet Technologies
 */ 

using System;
using System.Collections.Generic;
using Database.Business;

namespace Database.Transactions
{
    public class SecurityItem_CreateTemplate : SecurityItem_TransactionBaseClass
    {
        public int NoOfTemplateCreated { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityItem.SecurityItemId > 0)
            {
                string message = "SecurityItem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
          NoOfTemplateCreated= SecurityItem.CreateTemplate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class SecurityItem_RetrieveAllByClientAndSecurityProfile : SecurityItem_TransactionBaseClass
    {
        public List<b_SecurityItem> SecurityItemList { get; set; }  
     
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SecurityItem> tmpList = new List<b_SecurityItem>();            
            SecurityItem.RetrieveAllByClientAndSecurityProfileId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SecurityItemList = tmpList;
        }
    }

    public class CustomSecurityItem_RetrieveAll_V2 : SecurityItem_TransactionBaseClass
    {
        public List<b_SecurityItem> SecurityItemList { get; set; }


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SecurityItem> tmpList = new List<b_SecurityItem>();
            SecurityItem.CustomRetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SecurityItemList = tmpList;
        }
    }
}
