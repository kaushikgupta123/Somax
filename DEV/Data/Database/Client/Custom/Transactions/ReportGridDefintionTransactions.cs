using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Database
{
    public class ReportGridDefintion_RetrieveByReportListingId : ReportGridDefintion_TransactionBaseClass
    {       
        public List<b_ReportGridDefintion> ReportGridDefintionList { get; set; }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Preprocess()
        {
           // throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_ReportGridDefintion> tmpArray = null;
            ReportGridDefintion.RetrieveByReportListingId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ReportGridDefintionList = tmpArray;
        }
    }


    public class ReportGridDefintion_RetrieveAllByReportListingId_V2 : ReportGridDefintion_TransactionBaseClass
    {
        public List<b_ReportGridDefintion> ReportGridDefintionList { get; set; }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_ReportGridDefintion> tmpArray = null;
            ReportGridDefintion.RetrieveAllByReportListingId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ReportGridDefintionList = tmpArray;
        }
    }
}
