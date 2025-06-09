using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ReportGridDefintion : DataContractBase
    {
        public List<ReportGridDefintion> RetrieveByReportListingId(DatabaseKey dbKey)
        {
            ReportGridDefintion_RetrieveByReportListingId trans = new ReportGridDefintion_RetrieveByReportListingId();
            trans.ReportGridDefintion = this.ToDatabaseObject();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseList(trans.ReportGridDefintionList);
        }
        public List<ReportGridDefintion> UpdateFromDatabaseList(List<b_ReportGridDefintion> dbObjs)
        {
            List<ReportGridDefintion> result = new List<ReportGridDefintion>();

            foreach (b_ReportGridDefintion dbObj in dbObjs)
            {
                ReportGridDefintion tmp = new ReportGridDefintion();
                tmp.UpdateFromDatabaseObject(dbObj);
               // tmp.ReportEventLogId = dbObj.ReportEventLogId;
                result.Add(tmp);
            }
            return result;
        }


        public List<ReportGridDefintion> RetrieveAllByReportListingId_V2(DatabaseKey dbKey)
        {
            ReportGridDefintion_RetrieveAllByReportListingId_V2 trans = new ReportGridDefintion_RetrieveAllByReportListingId_V2();
            trans.ReportGridDefintion = this.ToDatabaseObject();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseList(trans.ReportGridDefintionList);
        }
    }
}
