using Client.Models.PhysicalInventory;
using DataContracts;
using System.Collections.Generic;
using System.Text;

namespace Client.BusinessWrapper.PhysicalInventory
{
    public class PhysicalInventoryWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;      
        public PhysicalInventoryWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public GridPhysicalInventoryList AddInventoryInGrid(string clientlookupid, decimal QuantityCounted, int Count,ref List<string> returnErrorList, long StoreroomId = 0, string StoreroomName = "")
        {
            GridPhysicalInventoryList returnobj = new GridPhysicalInventoryList();
            PartHistory newPhysicalInventory = new PartHistory()
            {
                PhysicalInventoryId = Count,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartClientLookupId = clientlookupid,
                PartQtyCounted = QuantityCounted,
                PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                IsPhysicalInventory = true
            };
            newPhysicalInventory.ValidatePhysicalInventoryRecordCount(userData.DatabaseKey);

            if (newPhysicalInventory.ErrorMessages.Count == 0)
            {
                Part objPart = new Part()                                           // Retrieve the part number and the storeroom information 
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.Personnel.SiteId,
                    ClientLookupId = clientlookupid,
                    StoreroomId = StoreroomId,
                };
                objPart.RetrieveByClientLookUpIdNUPCCode(this.userData.DatabaseKey);
                if (objPart.PartId > 0)
                {
                    newPhysicalInventory.PartId = objPart.PartId;
                    newPhysicalInventory.PartUPCCode = objPart.UPCCode;
                    newPhysicalInventory.Description = objPart.Description;
                   
                  
                    if(userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        objPart.StoreroomId = StoreroomId;
                        objPart.RetriveByPartIdForMultiStoreroom_V2(userData.DatabaseKey);
                        if (objPart != null && objPart.ErrorMessages == null)
                        {
                            newPhysicalInventory.PartStoreroomQtyOnHand = objPart.QtyOnHand;
                            newPhysicalInventory.PartStorerommLocation1_1 = objPart.Location1_1;
                            newPhysicalInventory.PartStorerommLocation1_2 = objPart.Location1_2;
                            newPhysicalInventory.PartStorerommLocation1_3 = objPart.Location1_3;
                            newPhysicalInventory.PartStorerommLocation1_4 = objPart.Location1_4;
                            newPhysicalInventory.PartStoreroomUpdateIndex = objPart.UpdateIndex;
                            GridPhysicalInventoryList obj = getGridPhysicalInventoryList(clientlookupid, QuantityCounted, StoreroomId, StoreroomName, newPhysicalInventory);
                            return obj;
                        }
                     }
                    else
                    {
                       PartStoreroom objPartStoreroom = new PartStoreroom()
                      {
                          ClientId = objPart.ClientId,
                          PartId = objPart.PartId
                      };
                      List<PartStoreroom> PartStorerommList = PartStoreroom.RetrieveByPartId(this.userData.DatabaseKey, objPartStoreroom);
                        if (PartStorerommList.Count > 0)
                        {
                            newPhysicalInventory.PartStoreroomQtyOnHand = PartStorerommList[0].QtyOnHand;
                            newPhysicalInventory.PartStorerommLocation1_1 = PartStorerommList[0].Location1_1;
                            newPhysicalInventory.PartStorerommLocation1_2 = PartStorerommList[0].Location1_2;
                            newPhysicalInventory.PartStorerommLocation1_3 = PartStorerommList[0].Location1_3;
                            newPhysicalInventory.PartStorerommLocation1_4 = PartStorerommList[0].Location1_4;
                            newPhysicalInventory.PartStoreroomUpdateIndex = PartStorerommList[0].UpdateIndex;
                            GridPhysicalInventoryList obj = getGridPhysicalInventoryList(clientlookupid, QuantityCounted, StoreroomId, StoreroomName, newPhysicalInventory);
                            return obj;
                        }
                     }

                   
                }
                else
                {
                    returnErrorList = objPart.ErrorMessages;
                }
            }
            else
            {
                returnErrorList = newPhysicalInventory.ErrorMessages;             
            }
            return returnobj;
        }

        #region V2-687 get GridPhysicalInventoryList
        private static GridPhysicalInventoryList getGridPhysicalInventoryList(string clientlookupid, decimal QuantityCounted, long StoreroomId, string StoreroomName, PartHistory newPhysicalInventory)
        {
            return new GridPhysicalInventoryList()
            {
                PartId = newPhysicalInventory.PartId,
                PartClientLookupId = clientlookupid,
                Description = newPhysicalInventory.Description,
                PartUPCCode = newPhysicalInventory.PartUPCCode,
                QtyOnHand = newPhysicalInventory.PartStoreroomQtyOnHand,
                QuantityCount = QuantityCounted,
                PartStoreroomUpdateIndex = newPhysicalInventory.PartStoreroomUpdateIndex,
                Section = newPhysicalInventory.PartStorerommLocation1_1,
                Row = newPhysicalInventory.PartStorerommLocation1_2,
                Shelf = newPhysicalInventory.PartStorerommLocation1_3,
                Bin = newPhysicalInventory.PartStorerommLocation1_4,
                StoreroomId = StoreroomId,
                StoreroomName = StoreroomName

            };
        }
        #endregion
        public PartHistory SaveReceipt(List<GridPhysicalInventoryList> list)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            foreach (var item in list)
            {
                tmpList.Add(new PartHistory
                {
                    PartId = item?.PartId ?? 0,
                    PartClientLookupId = item?.PartClientLookupId ?? string.Empty,
                    Description = item?.Description ?? string.Empty,
                    PartUPCCode = item?.PartUPCCode ?? string.Empty,
                    PartStoreroomQtyOnHand=item?.QtyOnHand??0,
                    PartQtyCounted = item?.QuantityCount ?? 0,
                    PartStoreroomUpdateIndex=item?.PartStoreroomUpdateIndex??0,
                    PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                    SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                    StoreroomId = item?.StoreroomId ?? 0,
                    MultiStoreroom = this.userData.DatabaseKey.Client.UseMultiStoreroom
                });
            }
            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            if(this.userData.DatabaseKey.Client.UseMultiStoreroom) //V2-687 for multistoreroom
            {
                parthistory.PhysicalInventory_V2(userData.DatabaseKey);
            }
            else
            {
                parthistory.PhysicalInventory(userData.DatabaseKey);
            }
            return parthistory;
        }
    }
}