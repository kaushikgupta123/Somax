using Common.Constants;
using DataContracts;
using System;
using System.Linq;
using System.Collections.Generic;
using Client.Models.Parts;
using Client.Models.InventoryCheckout;

namespace Client.BusinessWrapper.InventoryCheckout
{
    public class InventoryCheckoutWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;
        public InventoryCheckoutWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<DataContracts.Personnel> FillIssuTo()
        {
            DataContracts.Personnel personnel = new DataContracts.Personnel()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<DataContracts.Personnel> PersonnelList = personnel.RetrieveForLookupList(userData.DatabaseKey);
            return PersonnelList;
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
        internal PartModel populatePartDetails(long PartId, long StoreroomId = 0)
        {
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId,
                StoreroomId = StoreroomId,
            };
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                obj.RetriveByPartIdForMultiStoreroom_V2(userData.DatabaseKey);
            }
            else
            {
                obj.RetriveByPartId(userData.DatabaseKey);
            }
           
            objPart = initializeControls(obj);

            return objPart;
        }

        internal long GetStoreroomId(long _partId)
        {
            List<PartStoreroom> psList = new List<PartStoreroom>();
            PartStoreroom ps = new PartStoreroom();
            ps.ClientId = userData.DatabaseKey.Client.ClientId;
            ps.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ps.PartId = _partId;
            psList = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, ps);
            dynamic PartStoreroomId = psList.First().PartStoreroomId;
            return PartStoreroomId;
        }

        internal PartHistoryModel ValidateSelectedItems(string issueToClientLookupId, long PartStoreroomId, string _chargeType, string chargeToClientLookupId, long _chargeToId, decimal _TransactionQuantity, string _partClientLookupId, long _partId, string _description, string _upcCode,string _comments)
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
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                TransactionType = PartHistoryTranTypes.PartIssue,
                IsPartIssue = true,
                ErrorMessagerow = null,
                PartUPCCode = _upcCode
            };

            objPartHistory.ValidateAdd(userData.DatabaseKey);
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

        internal List<PartHistory> ConfirmData(List<InventoryCheckoutModel> dataList)
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
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = s.UPCCode,
                    PerformedById = s.PersonnelId ?? 0,
                    RequestorId = s.PersonnelId ?? 0,
                    IsPerformAdjustment = true,
                    Comments=s.Comments, //V2-624
                    MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom, //V2-687
                    StoreroomId = s.StoreroomId ?? 0  //V2-687
                })
                .ToList();
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateByForeignKeysnew(userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }

        internal List<PartHistory> PartReturnConfirmData(List<InventoryCheckoutModel> dataList)
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
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    TransactionType = PartHistoryTranTypes.PartIssue,
                    IsPartIssue = true,
                    ErrorMessagerow = null,
                    PartUPCCode = s.UPCCode,
                    PerformedById = s.PersonnelId ?? 0,
                    RequestorId = s.PersonnelId ?? 0,
                    IsPerformAdjustment = true,
                    Comments = s.Comments, //V2-624
                    MultiStoreroom = userData.DatabaseKey.Client.UseMultiStoreroom, //V2-687
                    StoreroomId = s.StoreroomId ?? 0  //V2-687
                })
                .ToList();
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateReturnPartByForeignKeysnew(userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }
    }
}