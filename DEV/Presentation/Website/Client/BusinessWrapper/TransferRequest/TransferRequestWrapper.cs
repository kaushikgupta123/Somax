using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.PurchaseRequest;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using Notification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Client.Models.PartLookup;
using Client.Models.PunchoutModel;
using System.Net;
using System.IO;
using Client.Models.PunchoutExport;
using Client.BusinessWrapper.Configuration.SiteSetUp;
using System.Reflection;
using Client.Localization;
using Client.Models.PurchaseRequest.UIConfiguration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Data;
using Client.Models;
using Database.Business;
using Client.Models.AutoTRGeneration;

namespace Client.BusinessWrapper
{
    public class TransferRequestWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;

        public TransferRequestWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        //#region V2-1059 TPR AutoGenerate

        public List<AutoTRGenerationSearchModel> GetAutoTRGenerateChunkList(int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", string StoreroomIDList = "")
        {
            PartStoreroom partStoreroom = new PartStoreroom();
            AutoTRGenerationSearchModel TRModel;
            List<AutoTRGenerationSearchModel> AutoTRGenerationSearchList = new List<AutoTRGenerationSearchModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            partStoreroom.ClientId = userData.DatabaseKey.Client.ClientId;
            partStoreroom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partStoreroom.orderbyColumn = orderbycol;
            partStoreroom.orderBy = orderDir;
            partStoreroom.offset1 = Convert.ToString(skip);
            partStoreroom.nextrow = Convert.ToString(length);
            partStoreroom.StoreroomIDList = StoreroomIDList;
            partStoreroom.PersonnelId= userData.DatabaseKey.Personnel.PersonnelId;


            List<PartStoreroom> partStoreroomList = partStoreroom.RetrieveChunkSearchAutoTRGeneration(this.userData.DatabaseKey);
            if (partStoreroomList != null)
            {
                foreach (var dbObj in partStoreroomList)
                {
                    TRModel = new AutoTRGenerationSearchModel();
                    TRModel.RowId = dbObj.RowId;
                    TRModel.PartIdClientLookupId = dbObj.PartIdClientLookupId;
                    TRModel.RequestStr = dbObj.RequestStr;
                    TRModel.IssueStr = dbObj.IssueStr;
                    TRModel.PartDescription = dbObj.PartDescription;
                    TRModel.TransferQuantity = dbObj.TransferQuantity;
                    TRModel.Max = dbObj.Max;
                    TRModel.Min = dbObj.Min;
                    TRModel.OnHand = dbObj.OnHand;

                    TRModel.RequestPTStoreroomId = dbObj.RequestPTStoreroomId;
                    TRModel.RequestStoreroomId = dbObj.RequestStoreroomId;
                    TRModel.RequestPartId = dbObj.RequestPartId;
                    TRModel.IssuePTStoreroomId = dbObj.IssuePTStoreroomId;
                    TRModel.IssueStoreroomId = dbObj.IssueStoreroomId;
                    TRModel.IssuePartId = dbObj.IssuePartId;
                    TRModel.Creator_PersonnelId = dbObj.Creator_PersonnelId;

                    TRModel.TotalCount = dbObj.TotalCount;
                    AutoTRGenerationSearchList.Add(TRModel);
                }
            }
            return AutoTRGenerationSearchList;
        }


        //#endregion

        #region Create TR AutoGenerate  V2-1059
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
                dataTable.Columns.Add(prop.Name, type);

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
        internal StoreroomTransfer STransAutoGenerate_V2(List<PartTranProcessTableModel> partTranProcessTableLists)
        {

            DataTable dataTable = ToDataTable<PartTranProcessTableModel>(partTranProcessTableLists);// 
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.ClientId = this.userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.SiteId = this.userData.DatabaseKey.Personnel.SiteId;

            storeroomTransfer.StoreroomTransferList = ToDataTable<PartTranProcessTableModel>(partTranProcessTableLists); storeroomTransfer.StoreroomTransferAutoGeneration_V2(this.userData.DatabaseKey);

            return storeroomTransfer;
        }
        #endregion
    }
}