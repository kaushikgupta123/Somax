using Client.BusinessWrapper.Common;
using Client.Models.StoreroomTransfer;

using Common.Constants;
using Common.Enumerations;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.BusinessWrapper
{
    public class StoreroomTransferWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();

        public StoreroomTransferWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<StoreroomTransferModel> GetStoreroomTransferChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "",long? StoreroomId=null,string SearchText="")
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            StoreroomTransferModel stModel;
            List<StoreroomTransferModel> StoreroomTransferModelList = new List<StoreroomTransferModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.CustomQueryDisplayId = CustomQueryDisplayId;
            storeroomTransfer.orderbyColumn = orderbycol;
            storeroomTransfer.orderBy = orderDir;
            storeroomTransfer.offset1 = Convert.ToString(skip);
            storeroomTransfer.nextrow = Convert.ToString(length);

            storeroomTransfer.IssueStoreroomId = StoreroomId.HasValue ? StoreroomId.Value : 0;
            storeroomTransfer.SearchText=SearchText;

            List<StoreroomTransfer> storeroomTransferList = storeroomTransfer.RetrieveChunkSearch(this.userData.DatabaseKey);
            if (storeroomTransferList != null)
            {
                foreach (var item in storeroomTransferList)
                {
                    stModel = new StoreroomTransferModel();
                    stModel.StoreroomTransferId = item.StoreroomTransferId;
                    stModel.PartClientLookupId = item.PartClientLookupId;
                    stModel.PartDescription = item.PartDescription;
                    stModel.Status = item.Status;
                    stModel.QuantityIssued = Math.Round(item.QuantityIssued, 2);
                    stModel.QuantityReceived = Math.Round(item.QuantityReceived, 2);
                    stModel.StoreroomName = item.StoreroomName;
                    stModel.TotalCount = item.TotalCount;
                    StoreroomTransferModelList.Add(stModel);
                }
            }
            return StoreroomTransferModelList;
        }
        public List<StoreroomTransferModel> GetStoreroomTransferOutgoingTransferChunkList(int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", long? StoreroomId = null, string SearchText = "")
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            StoreroomTransferModel stModel;
            List<StoreroomTransferModel> StoreroomTransferModelList = new List<StoreroomTransferModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.orderbyColumn = orderbycol;
            storeroomTransfer.orderBy = orderDir;
            storeroomTransfer.offset1 = Convert.ToString(skip);
            storeroomTransfer.nextrow = Convert.ToString(length);

            storeroomTransfer.IssueStoreroomId = StoreroomId.HasValue ? StoreroomId.Value : 0;
            storeroomTransfer.SearchText = SearchText;

            List<StoreroomTransfer> storeroomTransferList = storeroomTransfer.RetrieveOutgoingTransferChunkSearch(this.userData.DatabaseKey);
            if (storeroomTransferList != null)
            {
                foreach (var item in storeroomTransferList)
                {
                    stModel = new StoreroomTransferModel();
                    stModel.StoreroomTransferId = item.StoreroomTransferId;
                    stModel.PartClientLookupId = item.PartClientLookupId;
                    stModel.PartDescription = item.PartDescription;
                    stModel.Status = item.Status;
                    stModel.QuantityIssued = Math.Round(item.QuantityIssued, 2);
                    stModel.QuantityReceived = Math.Round(item.QuantityReceived, 2);
                    stModel.RequestQuantity = Math.Round(item.RequestQuantity, 2);
                    stModel.StoreroomName = item.StoreroomName;
                    stModel.TotalCount = item.TotalCount;
                    StoreroomTransferModelList.Add(stModel);
                }
            }
            return StoreroomTransferModelList;
        }
        #endregion

        #region Outgoing Transfer
        public List<string> IssueProcess(List<StoreroomTransferProcessIssuesModel> config)
        {
            StoreroomTransfer objST = new StoreroomTransfer();
            objST.StoreroomTransferList = ToDataTable<StoreroomTransferProcessIssuesModel>(config);
            objST.ClientId = userData.DatabaseKey.Client.ClientId;
            objST.SiteId = userData.DatabaseKey.Personnel.SiteId;
            objST.ValidateOutgoingTransfer(userData.DatabaseKey);
            if (objST.ErrorMessages == null || objST.ErrorMessages.Count == 0)
            {
                PartHistory objPartHistory = new PartHistory();
                objPartHistory.ClientId = userData.DatabaseKey.Client.ClientId;
                objPartHistory.SiteId = userData.DatabaseKey.Personnel.SiteId;
                objPartHistory.PerformedById = userData.DatabaseKey.Personnel.PersonnelId;
                objPartHistory.StoreroomTransferList = ToDataTable<StoreroomTransferProcessIssuesModel>(config);
                objPartHistory.ProcessIssues(this.userData.DatabaseKey);
                return objPartHistory.ErrorMessages;
            }
            else
            {
                return objST.ErrorMessages;
            }
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                if(prop.Name== "IssueQty" || prop.Name== "ReceiveQty")
                {
                    dataTable.Columns.Add("Quantity", type);
                }
                else
                {
                    dataTable.Columns.Add(prop.Name, type);
                }
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion

        #region Incoming Transfer
        public List<string> ReceiptProcess(List<StoreroomTransferProcessReceiptsModel> config)
        {
            StoreroomTransfer objST = new StoreroomTransfer();
            objST.StoreroomTransferList = ToDataTable<StoreroomTransferProcessReceiptsModel>(config);
            objST.ClientId = userData.DatabaseKey.Client.ClientId;
            objST.SiteId = userData.DatabaseKey.Personnel.SiteId;
            objST.ValidateIncomingTransfer(userData.DatabaseKey);
            if (objST.ErrorMessages == null || objST.ErrorMessages.Count == 0)
            {
                PartHistory objPartHistory = new PartHistory();
                objPartHistory.ClientId = userData.DatabaseKey.Client.ClientId;
                objPartHistory.SiteId = userData.DatabaseKey.Personnel.SiteId;
                objPartHistory.PerformedById = userData.DatabaseKey.Personnel.PersonnelId;
                objPartHistory.StoreroomTransferList = ToDataTable<StoreroomTransferProcessReceiptsModel>(config);
                objPartHistory.ProcessReceipt(this.userData.DatabaseKey);
                return objPartHistory.ErrorMessages;
            }
            else
            {
                return objST.ErrorMessages;
            }
        }
        #endregion
        #region Action Function
        public Tuple<StoreroomTransfer, string>  StoreroomTransferRetrievebyStoreroomTransferId(long StoreroomTransferId)
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.StoreroomTransferId = StoreroomTransferId;
            storeroomTransfer.Retrieve(_dbKey);

            Storeroom storeroom = new Storeroom();
            storeroom.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroom.StoreroomId = storeroomTransfer.RequestStoreroomId;
            storeroom.Retrieve(_dbKey);
            return Tuple.Create(storeroomTransfer, storeroom.Name);
        }
        public StoreroomTransfer savePartTransferRequest(AddTransferRequest addPartTransfer)
        {
            var PartStoreroomIdAndStoreroomId = addPartTransfer.IssuePartStoreroomIdAndStoreroomId.Split('#');
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            PartStoreroom ps = new PartStoreroom();
            ps.ClientId = userData.DatabaseKey.Client.ClientId;
            ps.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ps.StoreroomId = addPartTransfer.RequestStoreroomId;
            ps.PartId= addPartTransfer.PartId;
            var partstoreroomsRecords=ps.RetrieveByStoreroomIdAndPartId(_dbKey);

            storeroomTransfer.SiteId = userData.DatabaseKey.Personnel.SiteId;
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.RequestPartId = addPartTransfer.PartId;
            storeroomTransfer.RequestPTStoreroomID = partstoreroomsRecords[0].PartStoreroomId;
            storeroomTransfer.RequestStoreroomId = addPartTransfer.RequestStoreroomId;
            storeroomTransfer.RequestQuantity = addPartTransfer.RequestQuantity ?? 0;
            storeroomTransfer.Reason = addPartTransfer.Reason;
            storeroomTransfer.IssuePartId = addPartTransfer.PartId;
            storeroomTransfer.CreatedBy = userData.DatabaseKey.UserName;
            storeroomTransfer.IssuePTStoreroomID = Convert.ToInt64(PartStoreroomIdAndStoreroomId[0]);
            storeroomTransfer.IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            storeroomTransfer.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            storeroomTransfer.Status = PartTransferStatusConstants.Open;

            storeroomTransfer.Create(userData.DatabaseKey);

            return storeroomTransfer;
        }
        public StoreroomTransfer UpdateTransferRequest(AddTransferRequest addPartTransfer)
        {
            var PartStoreroomIdAndStoreroomId = addPartTransfer.IssuePartStoreroomIdAndStoreroomId.Split('#');
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.StoreroomTransferId = addPartTransfer.StoreroomTransferId;
            storeroomTransfer.Retrieve(_dbKey);
            //Validating Request Part ID Existing  
            //bool RequestPartidValid = false;
            //if (storeroomTransfer.RequestPartId != addPartTransfer.PartId)  //Same Part ID with same storeroom no need to check validation
            //{
            //    storeroomTransfer.RequestPartId = addPartTransfer.PartId;
            //    storeroomTransfer.RequestStoreroomId = addPartTransfer.RequestStoreroomId;
            //    storeroomTransfer.ValidateRequestPartId(this.userData.DatabaseKey);
            //    if (storeroomTransfer.ErrorMessages == null || storeroomTransfer.ErrorMessages.Count == 0)
            //    {
            //        //RequestPartidValid = true;
            //    }
            //    else
            //    {
            //        return storeroomTransfer;
            //    }
            //}
            //else
            //{
                storeroomTransfer.RequestPartId = addPartTransfer.PartId;
                storeroomTransfer.RequestStoreroomId = addPartTransfer.RequestStoreroomId;
            //}

            //Validating Issue Part ID Existing 
            //bool issuePartidValid = false;
            //if (storeroomTransfer.IssueStoreroomId != Convert.ToInt64(PartStoreroomIdAndStoreroomId[1])) 
            //{
            //    storeroomTransfer.IssuePartId = addPartTransfer.PartId;
            //    storeroomTransfer.IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            //    storeroomTransfer.ValidateIssuePartId(this.userData.DatabaseKey);
                
            //    if (storeroomTransfer.ErrorMessages == null || storeroomTransfer.ErrorMessages.Count == 0)
            //    {
            //        //issuePartidValid = true;
            //    }
            //    else
            //    {
            //        return storeroomTransfer;
            //    }
            //}
            //else
            //{
                storeroomTransfer.IssuePartId = addPartTransfer.PartId;
                storeroomTransfer.IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            //}
            storeroomTransfer.RequestPTStoreroomID = addPartTransfer.RequestPartStoreroomId;
            storeroomTransfer.RequestQuantity = addPartTransfer.RequestQuantity ?? 0;
            storeroomTransfer.Reason = addPartTransfer.Reason ?? string.Empty;

            storeroomTransfer.CreatedBy = userData.DatabaseKey.UserName;
            storeroomTransfer.IssuePTStoreroomID = Convert.ToInt64(PartStoreroomIdAndStoreroomId[0]);
            storeroomTransfer.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            storeroomTransfer.Status = PartTransferStatusConstants.Open;
            storeroomTransfer.Update(userData.DatabaseKey);
          
            return storeroomTransfer;
        }
        public StoreroomTransfer DeleteTransferRequest(long StoreroomTransferId)
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.StoreroomTransferId = StoreroomTransferId;
            storeroomTransfer.Delete(userData.DatabaseKey);
            return storeroomTransfer;
        }
        public StoreroomTransfer DenyTransferRequest(long StoreroomTransferId)
        {
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            storeroomTransfer.StoreroomTransferId = StoreroomTransferId;
            storeroomTransfer.Retrieve(_dbKey);
            if (storeroomTransfer.ErrorMessages == null)
            {
                storeroomTransfer.Status = PartTransferStatusConstants.Denied;
                storeroomTransfer.Update(userData.DatabaseKey);
            }
            return storeroomTransfer;
        }
        public PartHistory ForceComplete(long StoreroomTransferId)
        {
            PartHistory parthistory = new PartHistory
            {
                SiteId = userData.Site.SiteId,
                PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                StoreroomTransferId = StoreroomTransferId
            };
            parthistory.StoreroomTransferForceComplete(userData.DatabaseKey);
            return parthistory;
        }
        #endregion
    }
}