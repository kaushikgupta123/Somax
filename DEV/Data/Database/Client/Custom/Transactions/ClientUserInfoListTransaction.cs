using Common.Enumerations;

using Database;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database
{
    public class ClientUserInfoList_RetrieveChunkSearchLookupList : ClientUserInfoList_TransactionBaseClass
    {
            public List<b_ClientUserInfoList> clientUserInfoList { get; set; }

            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
            }

            public override void PerformWorkItem()
            {
                List<b_ClientUserInfoList> tmpList = null;
                ClientUserInfoList.RetrieveChunkSearchClientUserInfoList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
                clientUserInfoList = tmpList;
            }

            public override void Postprocess()
            {
                base.Postprocess();
            }

        }

    
}
