using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;
using Common.Structures;

namespace Database
{
  
    public class VendorRequest_ChunkSearchV2 : VendorRequest_TransactionBaseClass
    {
        public List<b_VendorRequest> RetVendorRequestList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_VendorRequest> tmpList = null;
            VendorRequest.VendorRequestRetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetVendorRequestList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
