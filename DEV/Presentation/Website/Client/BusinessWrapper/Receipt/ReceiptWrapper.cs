using Client.Models.InventoryReceipt;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Client.BusinessWrapper.Receipt
{
    public class ReceiptWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public string newClientlookupId { get; set; }
        public ReceiptWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public GridPartReceiptList AddPartInGrid(string ClientLookupId, decimal UnitCost, decimal ReceiptQuantity,int count, long StoreroomId = 0, string StoreroomName = "")
        {
            GridPartReceiptList returnObj = new GridPartReceiptList();
          
            PartHistory newInventoryReceipt = new PartHistory()
            {
                InventoryReceiptId = count,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                TransactionQuantity = ReceiptQuantity,
                Cost = UnitCost,
                IsInventoryReceipt = true

            };
            newInventoryReceipt.ValidateInventoryReceiptAdd(this.userData.DatabaseKey);
            if (newInventoryReceipt.ErrorMessages.Count == 0)
            {
                Part objPart = new Part()
                {
                    ClientLookupId = ClientLookupId,
                    SiteId = this.userData.DatabaseKey.Personnel.SiteId
                };
                objPart.RetrieveByClientLookUpIdNUPCCode(userData.DatabaseKey);
                returnObj.PartId = objPart.PartId;
                returnObj.PartClientLookupId = objPart.ClientLookupId;
                returnObj.PartUPCCode = objPart.UPCCode;
                returnObj.Description = objPart.Description;
                returnObj.PerformedById = this.userData.DatabaseKey.Personnel.PersonnelId;
                returnObj.PartAverageCost = objPart.AverageCost;
                returnObj.ReceiptQuantity = ReceiptQuantity;
                returnObj.StoreroomId = StoreroomId;
                returnObj.StoreroomName = StoreroomName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                newInventoryReceipt.ErrorMessages.ForEach(err => sb.Append(err + "<br />"));
                returnObj.errorListInString = sb.ToString();
            }
            return returnObj;
        }
      
        public List<ErrorModel> SaveReceipt(List<GridPartReceiptList> list)
        {
            PartHistory obj = new PartHistory();
            List<PartHistory> tmpList = new List<PartHistory>();
            List<ErrorModel> errorList = new List<ErrorModel>();
            foreach (var item in list)
            {
                tmpList.Add(new PartHistory
                {
                    PartId = item?.PartId ?? 0,
                    PartClientLookupId = item?.PartClientLookupId ?? string.Empty,
                    PartUPCCode = item?.PartUPCCode ?? string.Empty,
                    Description = item?.Description ?? string.Empty,
                    Cost = item?.UnitCost ?? 0,
                    PartAverageCost = item?.PartAverageCost ?? 0,
                    TransactionQuantity = item?.ReceiptQuantity ?? 0,
                    PerformedById = this.userData.DatabaseKey.Client.ClientId,
                    SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                    StoreroomId= item?.StoreroomId ?? 0,
                    MultiStoreroom = this.userData.DatabaseKey.Client.UseMultiStoreroom
                });
            }
            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            if(userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                parthistory.InventoryReceipt_V2(userData.DatabaseKey);
            }
            else
            {
                parthistory.InventoryReceipt(userData.DatabaseKey);
            }
            if (parthistory.PartHistoryList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (PartHistory objPartHistory in parthistory.PartHistoryList)
                {
                    objPartHistory.ErrorMessages.ForEach(err => sb.Append(err + Environment.NewLine));
                    objPartHistory.ErrorMessagerow = sb.ToString();
                    errorList.Add(new ErrorModel { Errormsg = objPartHistory.ErrorMessagerow, PartId = objPartHistory.PartId });
                    sb.Clear();
                }
                errorList.Distinct().ToList();
                if (errorList != null && errorList.Count > 0)
                {
                    List<successListId> removeItems = new List<successListId>();
                    foreach (var item in list)
                    {
                        bool isExist = false;
                        foreach (var item2 in errorList)
                        {
                            if (item.PartId == item2.PartId)
                            {
                                isExist = true;
                            }
                        }
                        if (!isExist)
                        {
                            removeItems.Add(new successListId { SuccpartId = item.PartId });
                        }
                    }
                    removeItems.Distinct().ToList();
                    errorList[0].SuccListId = removeItems;
                }
            }
            return errorList;
        }
    }
}