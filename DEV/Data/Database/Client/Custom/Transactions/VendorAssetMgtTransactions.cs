using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class VendorAssetMgt_RetrieveChunkSearchByVendorId : VendorAssetMgt_TransactionBaseClass
    {
        public List<b_VendorAssetMgt> VendorAssetMgtList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorAssetMgt.VendorId == 0)
            {
                string message = "VendorAssetMgt has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_VendorAssetMgt> tmpList = null;
            VendorAssetMgt.RetrieveVendorAssetMgtChunkSearchByVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            VendorAssetMgtList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    } 
    public class VendorAssetMgt_RetrieveHeaderByVendorId : VendorAssetMgt_TransactionBaseClass
    {
        public List<b_VendorAssetMgt> VendorAssetMgtList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorAssetMgt.VendorId == 0)
            {
                string message = "VendorAssetMgt has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_VendorAssetMgt> tmpList = null;
            VendorAssetMgt.RetrieveVendorAssetMgtHeaderByVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            VendorAssetMgtList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
