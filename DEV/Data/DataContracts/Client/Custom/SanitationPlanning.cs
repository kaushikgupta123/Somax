using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class SanitationPlanning
    {

        public static List<SanitationPlanning> UpdateFromDatabaseObjectList(List<b_SanitationPlanning> dbObjs)
        {
            List<SanitationPlanning> result = new List<SanitationPlanning>();

            foreach (b_SanitationPlanning dbObj in dbObjs)
            {
                SanitationPlanning tmp = new SanitationPlanning();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }


        public List<SanitationPlanning> RetrieveAll(DatabaseKey dbKey)
        {
            SanitationPlanning_RetrieveAll trans = new SanitationPlanning_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationPlanningList);
        }

        public List<SanitationPlanning> SanitationPlanningRetrieveByMasterId(DatabaseKey dbKey, long ClientId,
            long SanitationMasterId, string category)
        {
            SanitationPlanningTransactions_RetrieveByMasterId trans = new SanitationPlanningTransactions_RetrieveByMasterId 
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                ClientId = ClientId,
                SanitationMasterId = SanitationMasterId,
                Category = category
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationPlanning = this.ToDatabaseObject();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationPlanningList);
        }
        public List<SanitationPlanning> SanitationPlanningRetrieveBy_SanitationJobID(DatabaseKey dbKey)
        {
            SanitationPlanningTransactions_RetrieveBySanitationJobID trans = new SanitationPlanningTransactions_RetrieveBySanitationJobID
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SanitationPlanning = this.ToDatabaseObjectForSanitationJob();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SanitationPlanningList);
        }
        public b_SanitationPlanning ToDatabaseObjectForSanitationJob()
        {
            b_SanitationPlanning dbObj = new b_SanitationPlanning();
            dbObj.ClientId = this.ClientId;
            dbObj.SanitationJobId = this.SanitationJobId;
            dbObj.Category = this.Category;

            return dbObj;
        }
        public void Create_SanitationPlanning(DatabaseKey dbKey)
        {
            SanitationPlanning_Create trans = new SanitationPlanning_Create();
            trans.SanitationPlanning = this.ToDatabaseObject_ForCreateAndUpdate();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SanitationPlanning);
        }
        public b_SanitationPlanning ToDatabaseObject_ForCreateAndUpdate()
        {
            b_SanitationPlanning dbObj = new b_SanitationPlanning();
            dbObj.ClientId = this.ClientId;
            dbObj.SanitationPlanningId = this.SanitationPlanningId;
            dbObj.SanitationMasterId = this.SanitationMasterId;
            dbObj.SanitationJobId = this.SanitationJobId;
            dbObj.Category = this.Category;
            dbObj.CategoryValue = this.CategoryValue;
            dbObj.CategoryId = this.CategoryId;
            dbObj.Description = this.Description;
            dbObj.Dilution = this.Dilution;
            dbObj.Instructions = this.Instructions;
            dbObj.Quantity = this.Quantity;
            dbObj.UnitCost = this.UnitCost;
            return dbObj;
        }
        public void Update_SanitationPlanning(DatabaseKey dbKey)
        {
            SanitationPlanning_Update trans = new SanitationPlanning_Update();
            trans.SanitationPlanning = this.ToDatabaseObject_ForCreateAndUpdate();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SanitationPlanning);
        }
        public void UpdateFromDatabaseObjectDevExpressPrintSanitationToolExtended(b_SanitationPlanning dbObj)
        {
            this.SanitationJobId = dbObj.SanitationJobId;
            this.SanitationPlanningId = dbObj.SanitationPlanningId;
            this.CategoryValue = dbObj.CategoryValue;
            this.Description = dbObj.Description;
            this.Instructions = dbObj.Instructions;
            this.Quantity = dbObj.Quantity;
        }
        public void UpdateFromDatabaseObjectDevExpressPrintSanitationChemicalExtended(b_SanitationPlanning dbObj)
        {
            this.SanitationJobId = dbObj.SanitationJobId;
            this.SanitationPlanningId = dbObj.SanitationPlanningId;
            this.CategoryValue = dbObj.CategoryValue;
            this.Description = dbObj.Description;
            this.Instructions = dbObj.Instructions;
            this.Quantity = dbObj.Quantity;
            this.Dilution = dbObj.Dilution;
        }
    }
}
