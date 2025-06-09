using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ServiceOrderLineItem : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string Description { get; set; }
        public string Labor { get; set; }
        public string Materials { get; set; }
        public string Other { get; set; }
        public decimal Total { get; set; }
        public bool DeleteAllowed { get; set; }
        public bool LabourExists { get; set; }
        public bool PartExists { get; set; }
        public bool OthersExists { get; set; }
        public Int64 EquipmentId { get; set; }
        public string FIDescription { get; set; }
        public string Status { get; set; }
        #endregion

        public static List<ServiceOrderLineItem> ServiceOrderLineItemRetrieveByServiceOrderId(DatabaseKey dbKey, ServiceOrderLineItem serviceorderlineitem)
        {
            List<ServiceOrderLineItem> ServiceOrderLineItemList = new List<ServiceOrderLineItem>();

            ServiceOrderLineItem_RetrieveByServiceOrderId trans = new ServiceOrderLineItem_RetrieveByServiceOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };

            trans.ServiceOrderLineItem = serviceorderlineitem.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ServiceOrderLineItem.UpdateFromDatabaseObjectList(trans.ServiceOrderLineItemList);

        }
        public static List<ServiceOrderLineItem> UpdateFromDatabaseObjectList(List<b_ServiceOrderLineItem> dbObjs)
        {
            List<ServiceOrderLineItem> result = new List<ServiceOrderLineItem>();

            foreach (b_ServiceOrderLineItem dbObj in dbObjs)
            {
                ServiceOrderLineItem tmp = new ServiceOrderLineItem();
                tmp.UpdateFromDataBaseObjectExtendedPR(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDataBaseObjectExtendedPR(b_ServiceOrderLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Description = dbObj.Description;
            this.Labor = dbObj.Labor;
            this.Materials = dbObj.Materials;
            this.Other = dbObj.Others;
            this.Total = dbObj.Total;
            this.DeleteAllowed = dbObj.DeleteAllowed;
            this.LabourExists = dbObj.LabourExists;
            this.PartExists = dbObj.PartExists;
            this.OthersExists = dbObj.OthersExists;
            this.EquipmentId = dbObj.EquipmentId;
            this.FIDescription = dbObj.FIDescription;
            this.SchedServiceId = dbObj.SchedServiceId;
            this.VMRSSystem = dbObj.VMRSSystem;
            this.VMRSAssembly = dbObj.VMRSAssembly;
            this.Status = dbObj.Status;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
        public ServiceOrderLineItem ValidateForDeleteLineItem(DatabaseKey dbKey, ServiceOrderLineItem serviceorderlineitem)
        {
            //List<ServiceOrderLineItem> ServiceOrderLineItemList = new List<ServiceOrderLineItem>();
            ServiceOrderLineItem_ValidateForDelete trans = new ServiceOrderLineItem_ValidateForDelete()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ServiceOrderLineItem = serviceorderlineitem.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            ServiceOrderLineItem objData = new ServiceOrderLineItem();
            objData.UpdateFromDataBaseObjectExtendedPR(trans.ServiceOrderLineItem);
            return objData;
        }

        public void ServiceOrderLineItemDeleteCustom(DatabaseKey dbKey)
        {
            ServiceOrderLineItem_DeleteCustom trans = new ServiceOrderLineItem_DeleteCustom();
            trans.ServiceOrderLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
    }
}
