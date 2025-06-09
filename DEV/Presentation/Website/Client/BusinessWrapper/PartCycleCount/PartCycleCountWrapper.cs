using Client.BusinessWrapper.Common;
using Client.Models.PartCycleCount;

using DataContracts;
using Client.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.PartCycleCount
{
    public class PartCycleCountWrapper: CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public PartCycleCountWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<PartCycleCountSearchModel> GetPartCycleCountGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string Area = "",  string Row = "", string Shelf = "", string Bin = "", List<string> StockType = null, bool Critical = false, bool Consignment = false, DateTime? GenerateThrough = null, string PartClientLookupId = "", string PartDescription = "", string Section = "" , long StoreroomId = 0)
        {
            PartCycleCountSearchModel _GeneratePartCountSearchModel;
            List<PartCycleCountSearchModel> GeneratePartCountSearchModelList = new List<PartCycleCountSearchModel>();
            Part part = new Part();
            part.ClientId = this.userData.DatabaseKey.Client.ClientId;
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.ClientLookupId = PartClientLookupId;
            part.Description = PartDescription ?? "";
            part.Section = Section ?? "";
            part.PlaceArea = Area ?? "";
            part.Row = Row ?? "";
            part.Shelf = Shelf ?? "";
            part.Bin = Bin ?? "";
            part.StockType = string.Join(",", StockType ?? new List<string>());
            part.Critical = Critical;
            part.Consignment = Consignment;
            part.GenerateThrough = GenerateThrough;
            part.StoreroomId = StoreroomId;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                part.PartCycleCountChunkSearchForMultiStoreroom(this.userData.DatabaseKey);
            }
            else
            {
                part.PartCycleCountChunkSearch(this.userData.DatabaseKey);
            }
            var listofPart = part.listOfPart;
            foreach (var item in listofPart)
            {
                _GeneratePartCountSearchModel = new PartCycleCountSearchModel();
                _GeneratePartCountSearchModel.PartId = item.PartId;
                _GeneratePartCountSearchModel.ClientLookupId = item.ClientLookupId;
                _GeneratePartCountSearchModel.PartDescription = item.Description;
               _GeneratePartCountSearchModel.QtyOnHand = item.QtyOnHand;
                _GeneratePartCountSearchModel.Section = item.Location1_1;
               _GeneratePartCountSearchModel.Row = item.Location1_2;
               _GeneratePartCountSearchModel.Shelf = item.Location1_3;
                _GeneratePartCountSearchModel.Bin = item.Location1_4;
                _GeneratePartCountSearchModel.Area = item.Location1_5;
                _GeneratePartCountSearchModel.Variance = 0 - item.QtyOnHand; 
                _GeneratePartCountSearchModel.QuantityCount = 0;
                _GeneratePartCountSearchModel.TotalCount = item.TotalCount;
                GeneratePartCountSearchModelList.Add(_GeneratePartCountSearchModel);
            }
            return GeneratePartCountSearchModelList;
        }

        private void CreateEventLog(Int64 WOId, string Status)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Adjust On Hand Quantity
        public PartHistory SaveHandsOnQty(PartGridPhysicalInvList model)
        {
            PartStoreroom objPartStoreroom = new PartStoreroom()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = model.PartId.Value
            };
            List<PartStoreroom> PartStorerommList = PartStoreroom.RetrieveByPartId(this.userData.DatabaseKey, objPartStoreroom);
            decimal QtyOnHand = 0;
            int UpdateIndex = 0;
            if (PartStorerommList != null && PartStorerommList.Count > 0)
            {
                QtyOnHand = PartStorerommList[0].QtyOnHand;
                UpdateIndex = PartStorerommList[0].UpdateIndex;
            }

            List<PartHistory> lstPartHistory = new List<PartHistory>();
            PartHistory tmpModel = new PartHistory();
            tmpModel = new PartHistory
            {
                PartId = model?.PartId ?? 0,
                PartClientLookupId = model?.PartClientLookupId ?? string.Empty,
                Description = model?.Description ?? string.Empty,
                PartUPCCode = model?.PartUPCCode ?? string.Empty,
                PartStoreroomQtyOnHand = QtyOnHand,
                PartQtyCounted = model?.QuantityCount ?? 0,
                PartStoreroomUpdateIndex = UpdateIndex,
                PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            lstPartHistory.Add(tmpModel);
            PartHistory parthistory = new PartHistory() { PartHistoryList = lstPartHistory };
            parthistory.UpdatepartCount(userData.DatabaseKey);
            return parthistory;
        }

        public PartHistory SaveHandsOnQtyList(List<PartCycleCountSearchModel> list,long StoreroomId)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            foreach (var item in list)
            {
                tmpList.Add(new PartHistory
                {
                    PartId = item?.PartId ?? 0,
                    PartClientLookupId = item?.ClientLookupId ?? string.Empty,
                    Description = item?.PartDescription ?? string.Empty,
                    PartStoreroomQtyOnHand = item?.QuantityCount ?? 0,
                    PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                    SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                    StoreroomId = StoreroomId,
                    MultiStoreroom = this.userData.DatabaseKey.Client.UseMultiStoreroom
                });
            }
            PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
            if(this.userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                parthistory.UpdatepartCountForMultiStoreroom(userData.DatabaseKey);
            }
            else
            {
                parthistory.UpdatepartCount(userData.DatabaseKey);
            }
           
            return parthistory;



        }
        #endregion
    }
}