using Database;
using Database.Business;

using System.Collections.Generic;

namespace DataContracts
{
    public partial class RepairableSpareLog : DataContractBase
    {
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string PersonnelName { get; set; }
        public string ParentClientLookupId { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssignedClientLookupId { get; set; }
        public string TransactionDateS { get; set; }
        public List<RepairableSpareLog> RetrieveByEquipmentId(DatabaseKey dbKey)
        {
            RepairableSpareLog_RetrieveByEquipmentId trans = new RepairableSpareLog_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.RepairableSpareLog = this.ToDatabaseObjectByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<RepairableSpareLog> objRepairableSpareLogList = new List<RepairableSpareLog>();
            foreach (b_RepairableSpareLog obj in trans.RepairableSpareLogList)
            {
                RepairableSpareLog tmpRepairableSpareLog = new RepairableSpareLog();

                tmpRepairableSpareLog.UpdateFromDatabaseObjectByEquipmentId(obj);
                objRepairableSpareLogList.Add(tmpRepairableSpareLog);
            }
            return objRepairableSpareLogList;

        }

        public b_RepairableSpareLog ToDatabaseObjectByEquipmentId()
        {
            b_RepairableSpareLog dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.TransactionDateS = this.TransactionDateS;
            dbObj.PersonnelName = this.PersonnelName;
            dbObj.ParentClientLookupId = this.ParentClientLookupId;
            dbObj.AssetGroup1Name = this.AssetGroup1Name;
            dbObj.AssetGroup2Name = this.AssetGroup2Name;
            dbObj.AssetGroup2Name = this.AssetGroup3Name;
            dbObj.AssignedClientLookupId = this.AssignedClientLookupId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectByEquipmentId(b_RepairableSpareLog dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.RepairableSpareLogId = dbObj.RepairableSpareLogId;
            this.SiteId = dbObj.SiteId;
            this.EquipmentId = dbObj.EquipmentId;
            this.TransactionDate = dbObj.TransactionDate;
            this.Status = dbObj.Status;
            this.PersonnelId = dbObj.PersonnelId;
            this.Location = dbObj.Location;
            this.ParentId = dbObj.ParentId;
            this.AssetGroup1 = dbObj.AssetGroup1;
            this.AssetGroup2 = dbObj.AssetGroup2;
            this.AssetGroup3 = dbObj.AssetGroup3;
            this.Assigned = dbObj.Assigned;
            this.PersonnelName = dbObj.PersonnelName;
            this.ParentClientLookupId = dbObj.ParentClientLookupId;
            this.AssetGroup1Name = dbObj.AssetGroup1Name;
            this.AssetGroup2Name = dbObj.AssetGroup2Name;
            this.AssetGroup3Name = dbObj.AssetGroup3Name;
            this.AssignedClientLookupId = dbObj.AssignedClientLookupId;
            // Turn on auditing
            AuditEnabled = true;
        }



    }
}
