using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class StoreroomTransfer : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public Int64 PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public string StoreroomName { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public DataTable StoreroomTransferList { get; set; }
        #endregion
        string ValidateFor = string.Empty;
        public List<StoreroomTransfer> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            StoreroomTransfer_RetrieveChunkSearch trans = new StoreroomTransfer_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.StoreroomTransfer = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoreroomTransfer> StoreroomTransferlist = new List<StoreroomTransfer>();

            foreach (b_StoreroomTransfer StoreroomTransfer in trans.StoreroomTransferList)
            {
                StoreroomTransfer tmpStoreroomTransfer = new StoreroomTransfer();
                tmpStoreroomTransfer.UpdateFromDatabaseObjectForRetriveAllForSearch(StoreroomTransfer);
                StoreroomTransferlist.Add(tmpStoreroomTransfer);
            }
            return StoreroomTransferlist;
        }

        public b_StoreroomTransfer ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_StoreroomTransfer dbObj = new b_StoreroomTransfer();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.IssueStoreroomId = this.IssueStoreroomId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_StoreroomTransfer dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.StoreroomTransferId = dbObj.StoreroomTransferId;
            this.PartId = dbObj.PartId;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.PartDescription = dbObj.PartDescription;
            this.Status = dbObj.Status;
            this.QuantityIssued = dbObj.QuantityIssued;
            this.QuantityReceived = dbObj.QuantityReceived;
            this.StoreroomName = dbObj.StoreroomName;
            this.TotalCount = dbObj.TotalCount;
        }
        #region Validate Transfer Process
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errorslist = new List<StoredProcValidationError>();
            DataTable lulist = new DataTable("lulist");
            //lulist.Columns.Add("StoreroomTransferId", typeof(Int64));
            //lulist.Columns.Add("Quantity", typeof(decimal));
            //// Add a row for each column to be validated
            //lulist.Rows.Add(StoreroomTransferList);
            lulist = StoreroomTransferList;
            if (ValidateFor == "ValidateForIncomingTransfer")
            {
                StoreroomTransfer_ValidateForReceiptProcess trans = new StoreroomTransfer_ValidateForReceiptProcess()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    lulist = lulist
                };
                trans.StoreroomTransfer = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errorslist = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            if (ValidateFor == "ValidateForOutgoingTransfer")
            {
                StoreroomTransfer_ValidateForIssueProcess trans = new StoreroomTransfer_ValidateForIssueProcess()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    lulist = lulist
                };
                trans.StoreroomTransfer = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errorslist = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            return errorslist;
        }

        #endregion

        #region Outgoing Transfer
        public List<StoreroomTransfer> RetrieveOutgoingTransferChunkSearch(DatabaseKey dbKey)
        {
            StoreroomTransfer_RetrieveOutgoingTransferChunkSearch trans = new StoreroomTransfer_RetrieveOutgoingTransferChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.StoreroomTransfer = this.ToDateBaseObjectForRetrieveOutgoingTransferChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoreroomTransfer> StoreroomTransferlist = new List<StoreroomTransfer>();

            foreach (b_StoreroomTransfer StoreroomTransfer in trans.StoreroomTransferList)
            {
                StoreroomTransfer tmpStoreroomTransfer = new StoreroomTransfer();
                tmpStoreroomTransfer.UpdateFromDatabaseObjectForRetriveAllForOutgoingTransferSearch(StoreroomTransfer);
                StoreroomTransferlist.Add(tmpStoreroomTransfer);
            }
            return StoreroomTransferlist;
        }

        public b_StoreroomTransfer ToDateBaseObjectForRetrieveOutgoingTransferChunkSearch()
        {
            b_StoreroomTransfer dbObj = new b_StoreroomTransfer();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.IssueStoreroomId = this.IssueStoreroomId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForOutgoingTransferSearch(b_StoreroomTransfer dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.StoreroomTransferId = dbObj.StoreroomTransferId;
            this.PartId = dbObj.PartId;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.PartDescription = dbObj.PartDescription;
            this.Status = dbObj.Status;
            this.QuantityIssued = dbObj.QuantityIssued;
            this.RequestQuantity = dbObj.RequestQuantity;
            this.StoreroomName = dbObj.StoreroomName;
            this.TotalCount = dbObj.TotalCount;
        }

        public void ValidateOutgoingTransfer(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForOutgoingTransfer";
            Validate<StoreroomTransfer>(dbKey);
        }
        #endregion

        #region Incoming Transfer
        public void ValidateIncomingTransfer(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForIncomingTransfer";
            Validate<StoreroomTransfer>(dbKey);
        }
        #endregion

        #region Create  AutoGenerate  V2-1059
        public b_StoreroomTransfer StoreroomTransferAutoGeneration_V2(DatabaseKey dbKey)
        {
            DataTable lulist = new DataTable("lulist");
            lulist = StoreroomTransferList;
            StoreroomTransfer_AutoGeneration_V2 trans = new StoreroomTransfer_AutoGeneration_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                lulist = lulist
            };
            trans.StoreroomTransfer = this.ToDatabaseObject();
            trans.StoreroomTransfer = this.ToDatabaseObjectStoreroomAutoGeneration();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
          
            return trans.StoreroomTransfer;
        }
        public b_StoreroomTransfer ToDatabaseObjectStoreroomAutoGeneration()
        {
            b_StoreroomTransfer dbObj = this.ToDatabaseObject();
            return dbObj;
        }
        #endregion
    }
}
