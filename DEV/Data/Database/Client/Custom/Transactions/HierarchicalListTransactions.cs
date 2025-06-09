using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class HierarchicalList_RetrieveActiveListByName : HierarchicalList_TransactionBaseClass
    {
        public List<b_HierarchicalList> HierarchicalLists { get; set; }

        public override void PerformWorkItem()
        {
            b_HierarchicalList[] tmpArray = null;
            HierarchicalList.RetrieveActiveListFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            HierarchicalLists = new List<b_HierarchicalList>(tmpArray);
        }
    }
}
