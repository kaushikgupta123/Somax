using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class WOPlanLineItem : DataContractBase
    {
        #region Property
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Status { get; set; }
        public DateTime? CompleteDate { get; set; }

        #endregion

        #region Work Order Plan Chunk Search
        public List<WOPlanLineItem> RetrieveWOPlanLineItem_ByWorkOrderPlanId(DatabaseKey dbKey)
        {
            WOPlanLineItem_RetrieveByWorkOrderPlanId trans = new WOPlanLineItem_RetrieveByWorkOrderPlanId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.WOPlanLineItem = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<WOPlanLineItem> WorkOrderPlanlist = new List<WOPlanLineItem>();

            foreach (b_WOPlanLineItem WOP in trans.WOPlanLineItemList)
            {
                WOPlanLineItem tmpWOP = new WOPlanLineItem();
                tmpWOP.UpdateFromDatabaseObjectForRetriveAllForSearch(WOP);
                WorkOrderPlanlist.Add(tmpWOP);
            }
            return WorkOrderPlanlist;
        }
        //public b_WOPlanLineItem ToDateBaseObjectForRetrieveChunkSearch()
        //{
        //    b_WOPlanLineItem dbObj = this.ToDatabaseObject();
        //    dbObj.WorkOrderPlanId = this.WorkOrderPlanId;

        //    return dbObj;
        //}
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_WOPlanLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.Description = dbObj.Description;
            this.RequiredDate = dbObj.RequiredDate;
            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.Status = dbObj.Status;
            this.CompleteDate = dbObj.CompleteDate;
        }

        #endregion
    }
}
