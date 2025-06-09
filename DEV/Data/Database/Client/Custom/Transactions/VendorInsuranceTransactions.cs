using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class VendorInsurance_RetrieveChunkSearchByVendorId: VendorInsurance_TransactionBaseClass
    {
        public List<b_VendorInsurance> VendorInsuranceList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (VendorInsurance.VendorId == 0)
            {
                string message = "VendorInsurance has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_VendorInsurance> tmpList = null;
            VendorInsurance.RetrieveChunkSearchByVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            VendorInsuranceList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
