
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Database.Business;

namespace Database
{
    public class VendorCatalogItem_RetrieveByPartMasterId_V2 : VendorCatalogItem_TransactionBaseClass
    {
        public List<b_VendorCatalogItem> VendorCatalogItemList { get; set; }

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
            List<b_VendorCatalogItem> tmpArray = null;

            VendorCatalogItem.RetrieveByPartMasterId_V2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            VendorCatalogItemList = new List<b_VendorCatalogItem>();
            foreach (b_VendorCatalogItem tmpObj in tmpArray)
            {
                VendorCatalogItemList.Add(tmpObj);
            }
        }
    }
}
