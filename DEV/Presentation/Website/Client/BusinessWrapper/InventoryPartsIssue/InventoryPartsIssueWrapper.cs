using Client.BusinessWrapper.Common;
using Client.Models.Common;
using Client.Models.InventoryPartsIssue;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;

namespace Client.BusinessWrapper.InventoryPartsIssue
{
    public class InventoryPartsIssueWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
       
        public InventoryPartsIssueWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<DataContracts.Personnel> getPersonnelIssuTo()
        {
            DataContracts.Personnel personnel = new DataContracts.Personnel()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<DataContracts.Personnel> PersonnelList = personnel.RetrieveForLookupList(userData.DatabaseKey);
            return PersonnelList;
        }
        #region WorkOrder Lookuplist chunk
        public List<WorkOrderLookUpModel> GetWorkOrderLookupListGridData(string OrderColumn, string OrderDirection, int pageNumber, int ResultsPerPage, string clientLookupId, string Description, string ChargeTo, string WorkAssigned, string Requestor, string Status)
        {
            WorkOrderLookUpModel newWorkOrderLookupModel;
            List<WorkOrderLookUpModel> newWorkOrderLookupSearchModelList = new List<WorkOrderLookUpModel>();
            List<WorkOrder> workorderlist = new List<WorkOrder>();

            WorkOrder workOrder = new WorkOrder();
            workOrder.ClientLookupId = clientLookupId;
            workOrder.Description = Description;
            workOrder.ChargeTo_Name = ChargeTo;
            workOrder.WorkAssigned_Name = WorkAssigned;
            workOrder.Requestor_Name = Requestor;
            workOrder.Status = Status;
            workOrder.OrderbyColumn = OrderColumn;
            workOrder.OrderBy = OrderDirection;
            workOrder.OffSetVal = pageNumber;
            workOrder.NextRow = ResultsPerPage;
            workOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = this.userData.DatabaseKey.Client.ClientId;

            workorderlist = workOrder.GetAllWorkOrderLookupListForPartIssueV2(userData.DatabaseKey);

            foreach (var workorder in workorderlist)
            {
                newWorkOrderLookupModel = new WorkOrderLookUpModel();
                newWorkOrderLookupModel.ClientLookupId = workorder.ClientLookupId;
                newWorkOrderLookupModel.Description = string.IsNullOrEmpty(workorder.Description) ? "" : workorder.Description;
                newWorkOrderLookupModel.ChargeTo = string.IsNullOrEmpty(workorder.ChargeTo_Name) ? "" : workorder.ChargeTo_Name;
                newWorkOrderLookupModel.WorkAssigned = string.IsNullOrEmpty(workorder.WorkAssigned_Name) ? "" : workorder.WorkAssigned_Name;
                newWorkOrderLookupModel.Requestor = string.IsNullOrEmpty(workorder.Requestor_Name) ? "" : workorder.Requestor_Name;
                newWorkOrderLookupModel.Status = string.IsNullOrEmpty(workorder.Status) ? "" : workorder.Status;
                newWorkOrderLookupModel.TotalCount = workorder.TotalCount;
                newWorkOrderLookupModel.WorkOrderId = workorder.WorkOrderId;
                newWorkOrderLookupSearchModelList.Add(newWorkOrderLookupModel);

            }
            return newWorkOrderLookupSearchModelList;
        }
        #endregion

        #region Save Parts Issue Records
        internal Tuple<PartHistory, string> SavePartIssueData(PartsIssue partsIssue)
        {
            string errorMsg = string.Empty;
            PartHistory tmpPartHistory = new PartHistory();
           
            if (partsIssue != null)
            {
                tmpPartHistory.IssueToClientLookupId = partsIssue.IssueToClentLookupId;
                tmpPartHistory.IssuedTo = Convert.ToString(partsIssue.selectedPersonnelId ?? 0);
                tmpPartHistory.PartStoreroomId = partsIssue.PartStoreroomId ?? 0;
                tmpPartHistory.TransactionDate = System.DateTime.UtcNow;
                tmpPartHistory.ChargeType_Primary = partsIssue.ChargeType;
                tmpPartHistory.ChargeToClientLookupId = partsIssue.ChargeToClientLookupId;
                tmpPartHistory.ChargeToId_Primary = Convert.ToInt64(partsIssue.ChargeToId);
                tmpPartHistory.TransactionQuantity = partsIssue.Quantity ?? 0;
                tmpPartHistory.PartClientLookupId = partsIssue.PartClientLookupId;
                tmpPartHistory.PartId = partsIssue.PartId ?? 0;
                tmpPartHistory.Description = partsIssue.PartDescription;
                tmpPartHistory.SiteId = userData.DatabaseKey.Personnel.SiteId;
                tmpPartHistory.TransactionType = PartHistoryTranTypes.PartIssue;
                tmpPartHistory.IsPartIssue = true;
                tmpPartHistory.ErrorMessagerow = null;
                tmpPartHistory.PerformedById = partsIssue.selectedPersonnelId ?? 0;
                tmpPartHistory.RequestorId = partsIssue.selectedPersonnelId ?? 0;
                tmpPartHistory.IsPerformAdjustment = false;
                tmpPartHistory.Comments = partsIssue.Comments;
                tmpPartHistory.MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom;
                tmpPartHistory.StoreroomId = partsIssue.StoreroomId ?? 0;
                tmpPartHistory.CreateByForeignKeysForIssuePartStockOut(userData.DatabaseKey);
                if (tmpPartHistory != null && tmpPartHistory.ErrorsFromList.Count == 0)
                {
                    if (tmpPartHistory.ErrorMessagerow == null)
                    {
                        errorMsg = string.Empty;
                    }
                    else
                    {
                        errorMsg = tmpPartHistory.ErrorMessagerow.ToString();
                    }
                }
                else
                {
                    errorMsg = string.Join(",", tmpPartHistory.ErrorsFromList).ToString();
                }

            }


            return Tuple.Create(tmpPartHistory, errorMsg);
        }
        #endregion

    }
}
