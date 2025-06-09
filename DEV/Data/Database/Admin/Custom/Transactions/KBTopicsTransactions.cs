using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    #region Search
    public class KBTopics_RetrieveChunkSearchV2 : KBTopics_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (KBTopics.KBTopicsId > 0)
            {
                string message = "KBTopics has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_KBTopics tmpList = null;
            KBTopics.RetrieveChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region Details
    public class KBTopics_RetrieveByForeignKeys_V2 : KBTopics_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (KBTopics.KBTopicsId == 0)
            {
                string message = "KBTopics has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            KBTopics.RetrieveByForeignKeysFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion

    public class KbTopics_RetrieveTags : KBTopics_TransactionBaseClass
    {
        public b_KBTopics objKbtopics { get; set; }
        public List<List<b_KBTopics>> KbTopicsPersonnelList { get; set; }
        public List<b_KBTopics> KbTopicsList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (KBTopics.KBTopicsId == 0)
            //{
            //    string message = "Service Order has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            List<List<b_KBTopics>> tmpArray = null;
            KBTopics.RetrievePersonnelInitial(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            KbTopicsPersonnelList = new List<List<b_KBTopics>>();
            KbTopicsList = new List<b_KBTopics>();
            foreach (List<b_KBTopics> tmpObj in tmpArray)
            {
                foreach (b_KBTopics tmpObj2 in tmpObj)
                {
                    KbTopicsList.Add(tmpObj2);
                }
                KbTopicsPersonnelList.Add(tmpObj);
            }

        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
