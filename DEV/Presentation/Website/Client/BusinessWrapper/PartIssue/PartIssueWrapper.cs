using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Client.Models.PartIssue;
using Client.Models.Parts;
using Client.Models.Common;


namespace Client.BusinessWrapper.PartIssue
{
    public class PartIssueWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public PartIssueWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        internal List<DataContracts.Personnel> FillIssuTo()
        {
            DataContracts.Personnel personnel = new DataContracts.Personnel()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.DatabaseKey.User.DefaultSiteId
            };
            List<DataContracts.Personnel> PersonnelList = personnel.RetrieveForLookupList(_userData.DatabaseKey);
            return PersonnelList;
        }
        internal List<PartHistory> PartReturnConfirmData(List<PartIssueReturnModel> dataList)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (dataList != null)
            {
                tmpList = dataList.Select(s => new PartHistory
                {
                    IssueToClientLookupId = s.IssueToClentLookupId,
                    IssuedTo = Convert.ToString(s.PersonnelId),
                    PartStoreroomId = s.PartStoreroomId ?? 0,
                    TransactionDate = System.DateTime.UtcNow,
                    ChargeType_Primary = s.ChargeType,
                    ChargeToClientLookupId = s.ChargeToClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(s.ChargeToId),
                    TransactionQuantity = s.Quantity ?? 0,
                    PartClientLookupId = s.PartClientLookupId,
                    PartId = s.PartId ?? 0,
                    Description = s.PartDescription,
                    SiteId = _userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = s.UPCCode,
                    PerformedById = s.PersonnelId ?? 0,
                    RequestorId = s.PersonnelId ?? 0,
                    IsPerformAdjustment = true,
                    Comments = s.Comments,
                    MultiStoreroom = _userData.DatabaseKey.Client.UseMultiStoreroom,
                    StoreroomId = s.StoreroomId ?? 0
                })
                .ToList();
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateReturnPartByForeignKeysnew(_userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }

        internal List<PartHistory> PartIssueConfirmData(List<PartIssueModel> dataList)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (dataList != null)
            {
                tmpList = dataList.Select(s => new PartHistory
                {
                    IssueToClientLookupId = s.IssueToClentLookupId,
                    IssuedTo = Convert.ToString(s.PersonnelId),
                    PartStoreroomId = s.PartStoreroomId ?? 0,
                    TransactionDate = System.DateTime.UtcNow,
                    ChargeType_Primary = s.ChargeType,
                    ChargeToClientLookupId = s.ChargeToClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(s.ChargeToId),
                    TransactionQuantity = s.Quantity ?? 0,
                    PartClientLookupId = s.PartClientLookupId,
                    PartId = s.PartId ?? 0,
                    Description = s.PartDescription,
                    SiteId = _userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = s.UPCCode,
                    PerformedById = s.PersonnelId ?? 0,
                    RequestorId = s.PersonnelId ?? 0,
                    IsPerformAdjustment = true,
                    Comments = s.Comments,
                    MultiStoreroom = _userData.DatabaseKey.Client.UseMultiStoreroom,
                    StoreroomId = s.StoreroomId ?? 0
                })
                .ToList();
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateReturnPartByForeignKeysnew(_userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }
        internal PartModel populatePartDetails(long PartId, long StoreroomId = 0)
        {
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId,
                StoreroomId = StoreroomId,
            };
            if (_userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                obj.RetriveByPartIdForMultiStoreroom_V2(_userData.DatabaseKey);
            }
            else
            {
                obj.RetriveByPartId(_userData.DatabaseKey);
            }

            objPart = initializeControls(obj);

            return objPart;
        }
        public PartModel initializeControls(Part obj)
        {
            PartModel objPart = new PartModel();

            objPart.ClientLookupId = obj.ClientLookupId;
            objPart.PartId = obj.PartId;
            objPart.AccountId = obj?.AccountId ?? 0;
            objPart.AccountClientLookupId = obj?.Account_ClientLookupId ?? string.Empty;
            objPart.AppliedCost = obj?.AppliedCost ?? 0;
            objPart.AverageCost = obj?.AverageCost ?? 0;
            objPart.Description = obj?.Description ?? string.Empty;
            objPart.InactiveFlag = obj?.InactiveFlag ?? false;
            objPart.CriticalFlag = obj?.Critical ?? false;
            objPart.IssueUnit = obj?.IssueUnit ?? string.Empty;
            objPart.Manufacturer = obj?.Manufacturer ?? string.Empty;
            objPart.ManufacturerID = obj?.ManufacturerId ?? string.Empty;
            objPart.StockType = obj?.StockType ?? string.Empty;
            objPart.UPCCode = obj?.UPCCode ?? string.Empty;
            objPart.CountFrequency = obj?.CountFrequency ?? 0;
            objPart.LastCounted = obj?.LastCounted ?? DateTime.MinValue;
            objPart.Section = obj?.Location1_1 ?? string.Empty;
            objPart.PlaceArea = obj?.Location1_5 ?? string.Empty;
            objPart.Row = obj?.Location1_2 ?? string.Empty;
            objPart.Shelf = obj?.Location1_3 ?? string.Empty;
            objPart.Bin = obj?.Location1_4 ?? string.Empty;
            objPart.Maximum = obj?.QtyMaximum ?? 0;
            objPart.OnHandQuantity = obj?.QtyOnHand ?? 0;
            objPart.Minimum = obj?.QtyReorderLevel ?? 0;
            objPart.OnOrderQuantity = obj?.QtyOnOrder ?? 0;
            objPart.OnRequestQuantity = obj?.QtyOnRequest ?? 0;
            objPart.AutoPurchaseFlag = obj?.AutoPurch ?? false;
            objPart.PartStoreroomId = obj.PartStoreroomId;
            return objPart;
        }


        internal PartHistoryModel ValidateSelectedItems(string issueToClientLookupId, long PartStoreroomId, string _chargeType, string chargeToClientLookupId, long _chargeToId, decimal _TransactionQuantity, string _partClientLookupId, long _partId, string _description, string _upcCode, string _comments)
        {
            PartHistory objPartHistory = new PartHistory()
            {
                PartIssueId = 0,
                IssueToClientLookupId = issueToClientLookupId,
                PartStoreroomId = PartStoreroomId,
                TransactionDate = System.DateTime.UtcNow,
                ChargeType_Primary = _chargeType,
                ChargeToClientLookupId = chargeToClientLookupId,
                ChargeToId_Primary = _chargeToId,
                TransactionQuantity = _TransactionQuantity,
                PartClientLookupId = _partClientLookupId,
                PartId = _partId,
                Description = _description,
                SiteId = _userData.DatabaseKey.Personnel.SiteId,
                TransactionType = PartHistoryTranTypes.PartIssue,
                IsPartIssue = true,
                ErrorMessagerow = null,
                PartUPCCode = _upcCode
            };

            objPartHistory.ValidateAdd(_userData.DatabaseKey);
            PartHistoryModel objPartHistoryModel = new PartHistoryModel();
            objPartHistoryModel.PartIssueId = objPartHistory.PartIssueId;
            objPartHistoryModel.IssueToClientLookupId = objPartHistory.IssueToClientLookupId;
            objPartHistoryModel.PartStoreroomId = objPartHistory.PartStoreroomId;
            objPartHistoryModel.TransactionDate = objPartHistory.TransactionDate;
            objPartHistoryModel.ChargeType_Primary = objPartHistory.ChargeType_Primary;
            objPartHistoryModel.ChargeToClientLookupId = objPartHistory.ChargeToClientLookupId;
            objPartHistoryModel.ChargeToId_Primary = objPartHistory.ChargeToId_Primary;
            objPartHistoryModel.TransactionQuantity = objPartHistory.TransactionQuantity;
            objPartHistoryModel.PartClientLookupId = objPartHistory.PartClientLookupId;
            objPartHistoryModel.PartId = objPartHistory.PartId;
            objPartHistoryModel.Description = objPartHistory.Description;
            objPartHistoryModel.SiteId = objPartHistory.SiteId;
            objPartHistoryModel.TransactionType = objPartHistory.TransactionType;
            objPartHistoryModel.IsPartIssue = objPartHistory.IsPartIssue;
            objPartHistoryModel.ErrorMessagerow = objPartHistory.ErrorMessagerow;
            objPartHistoryModel.PartUPCCode = objPartHistory.PartUPCCode;
            objPartHistoryModel.ErrorMessages = objPartHistory.ErrorMessages;
            return objPartHistoryModel;
        }

        internal List<PartHistory> ConfirmData(List<PartIssueModel> dataList)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (dataList != null)
            {
                tmpList = dataList.Select(s => new PartHistory
                {
                    IssueToClientLookupId = s.IssueToClentLookupId,
                    IssuedTo = Convert.ToString(s.PersonnelId),
                    PartStoreroomId = s.PartStoreroomId ?? 0,
                    TransactionDate = System.DateTime.UtcNow,
                    ChargeType_Primary = s.ChargeType,
                    ChargeToClientLookupId = s.ChargeToClientLookupId,
                    ChargeToId_Primary = Convert.ToInt64(s.ChargeToId),
                    TransactionQuantity = s.Quantity ?? 0,
                    PartClientLookupId = s.PartClientLookupId,
                    PartId = s.PartId ?? 0,
                    Description = s.PartDescription,
                    SiteId = _userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = s.UPCCode,
                    PerformedById = s.PersonnelId ?? 0,
                    RequestorId = s.PersonnelId ?? 0,
                    IsPerformAdjustment = true,
                    Comments = s.Comments, //V2-624
                    MultiStoreroom = _userData.DatabaseKey.Client.UseMultiStoreroom,
                    StoreroomId = s.StoreroomId ?? 0
                })
                .ToList();
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateByForeignKeysnew(_userData.DatabaseKey);
            }
            return PartHistoryListTemp;
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
            workOrder.SiteId = _userData.DatabaseKey.User.DefaultSiteId;
            workOrder.ClientId = this._userData.DatabaseKey.Client.ClientId;

            workorderlist = workOrder.GetAllWorkOrderLookupListForPartIssueV2(_userData.DatabaseKey);

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

    }

}