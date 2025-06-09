using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
        public class MemorizeSearch_RetrieveForSearch : MemorizeSearch_TransactionBaseClass
        {

            public List<b_MemorizeSearch> MemorizeSearchList { get; set; }
            
            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
               
            }
            public override void PerformWorkItem()
            {
                List<b_MemorizeSearch> tmpList = null;

            MemorizeSearch.RetrieveForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            MemorizeSearchList = tmpList;
            }

            public override void Postprocess()
            {
                base.Postprocess();
            }
        }


    public class RetrieveafterCreateAndDelete : MemorizeSearch_TransactionBaseClass
    {

        public List<b_MemorizeSearch> MemorizeSearchList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_MemorizeSearch> tmpList = null;

            MemorizeSearch.RetrieveafterCreateAndDelete(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            MemorizeSearchList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}

