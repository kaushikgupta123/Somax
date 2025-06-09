using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    public partial class Instructions: DataContractBase
    {

        #region Property
        public DateTime CreateDate { get; set; }
        #endregion
        public List<Instructions> RetrieveInstructionsByObjectId_V2(DatabaseKey dbKey)
        {
            List<Instructions> auditList = new List<Instructions>();

            Instructions_RetrieveInstructionsByObjectIdFromDatabase_V2 trans = new Instructions_RetrieveInstructionsByObjectIdFromDatabase_V2();
            trans.Instructions = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            auditList = UpdateFromDatabaseObjectList(trans.InstructionsList);

            return auditList;
        }
        public static List<Instructions> UpdateFromDatabaseObjectList(List<b_Instructions> dbObj)
        {
            List<Instructions> result = new List<Instructions>();
            foreach (b_Instructions audit in dbObj)
            {
                Instructions tmp = new Instructions();
                tmp.UpdateFromDatabaseExtendedObject(audit);
                result.Add(tmp);
            }

            return result;
        }
        public void UpdateFromDatabaseExtendedObject(b_Instructions dbObj)
        {
            UpdateFromDatabaseObject(dbObj);

            this.CreateDate = dbObj.CreateDate;
        }
        public void UpdateFromDatabaseObjectInstructionsPrintExtended(b_Instructions dbObj)
        {
          //  UpdateFromDatabaseObject(dbObj);
            this.ObjectId = dbObj.ObjectId;            
            this.Contents = dbObj.Contents;
        }
    }
}
