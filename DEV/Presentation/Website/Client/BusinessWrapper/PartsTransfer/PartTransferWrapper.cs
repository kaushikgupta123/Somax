using Client.Models.PartTransfer;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Common.Constants;
using Client.Common;
using Common.Extensions;
using Common.Enumerations;
using Data.DataContracts;

namespace Client.BusinessWrapper.PartsTransfer
{
    public class PartTransferWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;
        public PartTransferWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        internal List<KeyValuePair<string, string>> populateListDetails()
        {
            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "PartTransfer", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "-- Select All --"));
            }
            return customList;
        }
        public List<PartTransferModel> PartTransferList(int SearchTextDropID)
        {
            DataContracts.PartTransfer parttransfer = new DataContracts.PartTransfer();
            parttransfer.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            parttransfer.CustomQueryDisplayId = SearchTextDropID;
            List<PartTransfer> PartTransferSearchList = parttransfer.RetrieveAllForSearchNew(this.userData.DatabaseKey, userData.Site.TimeZone);

            List<PartTransferModel> eventList = InitializeModel(PartTransferSearchList);
            return eventList;
        }
        List<PartTransferModel> InitializeModel(List<DataContracts.PartTransfer> list)
        {
            List<PartTransferModel> eventList = new List<PartTransferModel>();
            PartTransferModel model;
            foreach (var item in list)
            {
                model = new PartTransferModel();
                model.ClientId = item.ClientId;
                model.PartTransferId = item.PartTransferId;
                model.RequestSite_Name = item.RequestSite_Name;
                model.RequestPart_ClientLookupId = item.RequestPart_ClientLookupId;
                model.Quantity = item.Quantity;
                model.RequestPart_Description = item.RequestPart_Description;
                model.IssuePart_Description = item.IssuePart_Description;
                model.Status = item.Status;
                model.Description = item.Description;
                model.IssueSite_Name = item.IssueSite_Name;
                model.IssuePartId = item.IssuePartId;
                model.IssuePart_ClientLookupId = item.IssuePart_ClientLookupId;
                model.Reason = item.Reason;
                model.LastEvent = item.LastEvent;
                model.LastEventDate = item.LastEventDate;

                //------
                model.IssueSiteId = item.IssueSiteId;
                model.RequestSiteId = item.RequestSiteId;
                model.RequestPartId = item.RequestPartId;
                model.Creator_PersonnelId = item.Creator_PersonnelId;
                model.RequiredDate = item.RequiredDate;
                model.ShippingAccountId = item.ShippingAccountId;
                model.QuantityIssued = item.QuantityIssued;
                model.QuantityReceived = item.QuantityReceived;
                model.LastEventBy_PersonnelId = item.LastEventBy_PersonnelId;
                model.CreatedBy = item.CreatedBy;
                model.UpdateIndex = item.UpdateIndex;
                eventList.Add(model);
            }
            return eventList;
        }

        public PartTransferModel GetPartTransferDetail(long PartTransferId)
        {
            PartTransferModel partTransferModel = new PartTransferModel();

            DataContracts.PartTransfer parttransfer = new DataContracts.PartTransfer();
            parttransfer.PartTransferId = PartTransferId;
            parttransfer.ClientId = userData.DatabaseKey.Client.ClientId;
            parttransfer.SiteId = userData.DatabaseKey.Personnel.SiteId;
            parttransfer.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            DataContracts.PartStoreroom partStore = new DataContracts.PartStoreroom()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartId = parttransfer.RequestPartId
            };
            List<PartStoreroom> listps = new List<PartStoreroom>();
            listps = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, partStore);
            partStore = listps.FirstOrDefault();

            DataContracts.PartStoreroom partStoreissue = new DataContracts.PartStoreroom()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartId = parttransfer.IssuePartId
            };
            List<PartStoreroom> listpsissue = new List<PartStoreroom>();
            listpsissue = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, partStoreissue);
            partStoreissue = listpsissue.FirstOrDefault();

            //hdnUpdateIndex.Value = parttransfer.UpdateIndex.ToString();
            partTransferModel.UpdateIndex = parttransfer.UpdateIndex;
            //hdnClientId.Value = parttransfer.ClientId.ToString();
            partTransferModel.ClientId = parttransfer.ClientId;
            //hdnObjectId.Value = parttransfer.PartTransferId.ToString();
            //hdnSiteId.Value = parttransfer.SiteId.ToString();


            partTransferModel.PartTransferId = parttransfer.PartTransferId;

            partTransferModel.RequestPart_ClientLookupId = parttransfer.RequestPart_ClientLookupId;

            partTransferModel.RequestSite_Name = parttransfer.RequestSite_Name;

            //uicLocation.Configure(uiConfig.Request_Location, localization.Request_Location, FormatLocation(ps));
            partTransferModel.Request_Location = FormatLocation(partStore);
            partTransferModel.RequestPart_Description = parttransfer.RequestPart_Description;


            partTransferModel.Reason = parttransfer.Reason;
            partTransferModel.Quantity = parttransfer.Quantity;
            partTransferModel.QuantityInTransit = parttransfer.QuantityIssued - parttransfer.QuantityReceived;
            partTransferModel.QuantityIssued = parttransfer.QuantityIssued;
            partTransferModel.QuantityReceived = parttransfer.QuantityReceived;
            partTransferModel.CreatedBy = parttransfer.CreateBy_PersonnelName;
            if (parttransfer.CreateDate != null && parttransfer.CreateDate == default(DateTime))
            {
                partTransferModel.CreateDate = null;
            }
            else
            {
                partTransferModel.CreateDate = parttransfer.CreateDate;
            }
            partTransferModel.CreateDate = parttransfer.CreateDate;

            partTransferModel.IssueSiteId = parttransfer.IssueSiteId;
            partTransferModel.IssueSite_Name = parttransfer.IssueSite_Name;
            partTransferModel.IssuePartId = parttransfer.IssuePartId;
            partTransferModel.IssuePart_ClientLookupId = parttransfer.IssuePart_ClientLookupId;
            partTransferModel.IssuePart_QtyOnHand = parttransfer.IssuePart_QtyOnHand;
            partTransferModel.Issue_Location = FormatLocation(partStoreissue);
            partTransferModel.IssuePart_Description = parttransfer.IssuePart_Description == null ? "" : parttransfer.IssuePart_Description;

            partTransferModel.Status = parttransfer.Status;
            partTransferModel.LastEvent = parttransfer.LastEvent;
            partTransferModel.LastEventBy_PersonnelId = parttransfer.LastEventBy_PersonnelId;
            partTransferModel.LastEventBy_PersonnelName = parttransfer.LastEventBy_PersonnelName;
            if (parttransfer.LastEventDate != null && parttransfer.LastEventDate == default(DateTime))
            {
                partTransferModel.LastEventDate = null;
            }
            else
            {
                partTransferModel.LastEventDate = parttransfer.LastEventDate;
            }
            partTransferModel.RequestSiteId = parttransfer.RequestSiteId;
            partTransferModel.ShippingAccountId = parttransfer.ShippingAccountId;
            if (parttransfer.RequiredDate != null && parttransfer.RequiredDate == DateTime.MinValue)
            {
                partTransferModel.RequiredDate = null;//parttransfer.RequiredDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                partTransferModel.RequiredDate = parttransfer.RequiredDate;
            }
            // partTransferModel.RequiredDate = parttransfer.RequiredDate;
            return partTransferModel;
        }
        private string FormatLocation(DataContracts.PartStoreroom ps)
        {
            string location = string.Empty;
            List<string> locs = new List<string>();
            if (!string.IsNullOrEmpty(ps.Location1_5))
            {
                locs.Add(ps.Location1_5.ToString() + "-");
            }
            if (!string.IsNullOrEmpty(ps.Location1_1))
            {
                locs.Add(ps.Location1_1.ToString() + "-");
            }
            if (!string.IsNullOrEmpty(ps.Location1_2))
            {
                locs.Add(ps.Location1_2.ToString() + "-");
            }
            if (!string.IsNullOrEmpty(ps.Location1_3))
            {
                locs.Add(ps.Location1_3.ToString() + "-");
            }
            if (!string.IsNullOrEmpty(ps.Location1_4))
            {
                locs.Add(ps.Location1_4.ToString());
            }
            location = string.Join("", locs);
            if (!string.IsNullOrEmpty(location))
            {
                location = location.LastIndexOf('-') - (location.Length - 1) == 0 ? location.Remove(location.Length - 1) : location;
            }
            return location;

        }

        #region Part Transfer Save
        public List<string> SavePT(PartTransferModel ptm)
        {
            DataContracts.PartTransfer p = new DataContracts.PartTransfer();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.PartTransferId = ptm.PartTransferId;// ObjectId;
            p.Retrieve(userData.DatabaseKey);
            p.Reason = ptm.Reason ?? "";
            p.Quantity = ptm.Quantity ?? 0;
            p.RequiredDate = ptm.RequiredDate;
            p.ShippingAccountId = ptm.ShippingAccountId;
            p.Update(userData.DatabaseKey);
            return p.ErrorMessages;
        }
        #endregion Part Transfer save

        #region Part Issue
        public List<string> SavePTIssue(PartTransferIssueModel ptm)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptm.PartTransferId ?? 0
                //SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);
            pt.Quantity = ptm.IssueQuantity ?? 1;
            pt.Comments = ptm.Comment;
            pt.LastEvent = PartTransferStatus.Issue;
            pt.Status = PartTransferStatus.InTransit;
            pt.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.TransactionType = PartHistoryTranTypes.TransferIssue;
            pt.QuantityOutstanding = pt.Quantity - pt.QuantityIssued;
            if (ptm.IssueQuantity > pt.QuantityOutstanding)
            {
                pt.ErrorMessages = new List<string>();
                var msg = "Total issued cannot be more than the quantity requested " + ptm.IssueQuantity;//UserData.ClientLocalization.Global.StoredProcValidation.ValidationError.Where(a => a.Code == 1803).Select(s => s.Message).FirstOrDefault();                
                pt.ErrorMessages.Add(msg);
                return pt.ErrorMessages;
            }
            DataContracts.PartStoreroom partStore = new DataContracts.PartStoreroom()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                //SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartId = pt.IssuePartId //--from tfs
            };
            List<PartStoreroom> listps = new List<PartStoreroom>();
            listps = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, partStore);
            partStore = listps.FirstOrDefault();
            pt.IssuePart_QtyOnHand = ptm.IssueQuantity ?? 1;// Convert.ToDecimal(uicQuantity.Text);
            if (ptm.IssueQuantity > partStore.QtyOnHand)
            {
                pt.ErrorMessages = new List<string>();
                var msg = "You cannot issue {" + partStore.QtyOnHand + "} more than is currently on - hand {" + partStore.QtyOnHand + "}";
                pt.ErrorMessages.Add(msg);
                return pt.ErrorMessages;
            }
            pt.PartTransferIssue(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (pt.ErrorMessages == null || pt.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferIssue, pt.PartTransferId, pt.PartTransferEventLogId);


            }
            return pt.ErrorMessages;
        }
        #endregion part Issue

        #region part receive
        public List<string> SavePartReceive(PartTransferReceiveModel ptm)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptm.PartTransferId ?? 0
                // SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);
            //RetrievePageControls(pt);
            // V2-??? - RKL - 2023-Jan-27
            // Data from the PartTransferReceiveModel - needs to be copied to the PartTransfer 
            // We were not copying the Receive Quantity
            pt.TxnQuantity = ptm.ReceiveQuantity ?? 0;
            pt.Comments = ptm.Comment;// uicComments.Text;
            pt.LastEvent = PartTransferStatus.Receipt;
            pt.LoggedUser_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.TransactionType = PartHistoryTranTypes.TransferReceipt;//Data.Common.Constants.PartHistoryTranTypes.TransferReceipt;
           // If the previously Received quantity + qty received this time are >= Quantity Requested
           // Set the status to complete 
            if ((pt.QuantityReceived + pt.TxnQuantity) >= pt.Quantity)
            {
                pt.Status = PartTransferStatus.Complete;
            }
            // If the previously Received quantity + qty received this time are < Quantity Requested
            // Set the status to waiting 
            else if ((pt.QuantityReceived + pt.TxnQuantity) < pt.Quantity)
            {
                pt.Status = PartTransferStatus.Waiting;
            }
           // pt.Quantity = ptm.ReceiveQuantity ?? 1;//string.IsNullOrEmpty(uicQuantity.Text) ? 1 : Convert.ToDecimal(uicQuantity.Text);
           
            //pt.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            //if (pt.QuantityReceived >= Convert.ToDecimal(ptm.ReceiveQuantity))
            //{
            //    pt.Status = PartTransferStatus.Complete;

            //}
            //else if (pt.QuantityReceived < Convert.ToDecimal(ptm.ReceiveQuantity))
            //{
            //    pt.Status = PartTransferStatus.Waiting;
            //    pt.LastEvent = PartTransferStatus.Receipt;
            //}


            // 
            // Error Checking 
            // 
            // 1 - If the Received Quantity is null or zero - then error 
            if (pt.TxnQuantity == 0)
            {
                pt.ErrorMessages = new List<string>();
                pt.ErrorMessages.Add("Quantity cannot be blank or zero");
                return pt.ErrorMessages;
            }
            // 2 - If the Received Quantity is greater than intransit quantity - then error 
            decimal QtyInTransit;
            QtyInTransit = pt.QuantityIssued - pt.QuantityReceived;
            //pt.TxnQuantity = ptm.ReceiveQuantity ?? 0;
            if (pt.TxnQuantity > QtyInTransit)
            {
                var msg = "Quantity received cannot be more than quantity in transit {" + QtyInTransit + "}";//UserData.ClientLocalization.Global.StoredProcValidation.ValidationError.Where(a => a.Code == 1805).Select(s => s.Message).FirstOrDefault();
                pt.ErrorMessages = new List<string>();
                pt.ErrorMessages.Add(msg);
                return pt.ErrorMessages;

            }
            // Perform the Receipt
            pt.PartTransferReceive(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (pt.ErrorMessages == null || pt.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferReceipt, pt.PartTransferId, pt.PartTransferEventLogId);

            }
            return pt.ErrorMessages;
        }
        #endregion part receive

        #region part send
        public List<string> SavePartSend(PartTransferModel ptm)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptm.PartTransferId
                // SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);
            //RetrievePageControls(pt);
            pt.PartTransferId = ptm.PartTransferId;
            pt.ClientId = userData.DatabaseKey.Client.ClientId;
            pt.LastEvent = PartTransferStatus.Sent;
            pt.Status = PartTransferStatus.Waiting;
            pt.LastEventDate = DateTime.UtcNow;
            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.TransactionDate = DateTime.UtcNow;
            pt.Comments = "";
            pt.QuantityInTransit = pt.QuantityIssued - pt.QuantityReceived;

            pt.PartTransferSend(this.userData.DatabaseKey, userData.Site.TimeZone);
            if (pt.ErrorMessages == null || pt.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferSend, pt.PartTransferId, pt.PartTransferEventLogId);
            }

            return pt.ErrorMessages;
        }
        #endregion part send
        #region EventLog
        public List<PartTransferEventLogModel> GetEventLogs(long PartTransferId)
        {
            PartTransferEventLogModel partTransferEventLogModel;
            List<PartTransferEventLogModel> partTransferEventLogModelList = new List<PartTransferEventLogModel>();
            PartTransferEventLog ptlog = new PartTransferEventLog();
            List<PartTransferEventLog> data = new List<PartTransferEventLog>();
            ptlog.ClientId = userData.DatabaseKey.Client.ClientId;
            ptlog.PartTransferId = PartTransferId;
            data = ptlog.RetriveByPartTransferId(userData.DatabaseKey);
            foreach (var d in data)
            {
                partTransferEventLogModel = new PartTransferEventLogModel();
                partTransferEventLogModel.PartTransferEventLogId = d.PartTransferEventLogId;
                partTransferEventLogModel.Event = d.Event;
                partTransferEventLogModel.CreatedBy = d.Personnel;
                partTransferEventLogModel.Created = d.TransactionDate;
                partTransferEventLogModel.Comments = d.Comments;
                partTransferEventLogModelList.Add(partTransferEventLogModel);

            }
            return partTransferEventLogModelList;
        }

        public PrintPartTransferModel EventLogPrint(long EventLogID)
        {
            DataTable dtShipment = new DataTable();
            INTDataLayer.BAL.ReportBAL PTShipment = new INTDataLayer.BAL.ReportBAL();
            dtShipment = PTShipment.RetrieveShipment(userData.DatabaseKey.Client.ClientId, Convert.ToInt64(EventLogID), userData.DatabaseKey.ClientConnectionString);
            PrintPartTransferModel PrintModel = new PrintPartTransferModel();
            PrintModel = ConvertToList<PrintPartTransferModel>(dtShipment);
            return PrintModel;
        }
        public static T ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList().FirstOrDefault();
        }

        #endregion


        #region Part Deny
        public List<string> SavePTDeny(PartTransferDenyModel ptd)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptd.PartTransferId ?? 0,// ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);
            pt.LastEvent = PartTransferStatus.Denied;
            pt.Status = PartTransferStatus.Denied;    // RKL 2023-04-05
            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.EventCode = ptd.DenyReasonId;
            pt.Comments = ptd.Comment;
            pt.CancelOrDenyState = "Deny";
            pt.PartTransferCancelDeny(this.userData.DatabaseKey, userData.Site.TimeZone);

            try
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferDenied, pt.PartTransferId, pt.PartTransferEventLogId);
            }
            catch(Exception ex)
            {

            }

            return pt.ErrorMessages;

        }
        #endregion part Issue

        #region Part ForceComplete
        public List<string> SaveForceComplete(PartTransferForceCompleteModel ptd)
        {
            PartTransfer pt = new PartTransfer()
            {
                //CallerUserInfoId = _dbKey.User.UserInfoId,
                //CallerUserName = _dbKey.UserName
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptd.PartTransferId ?? 0,// ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);
            if (pt.QuantityIssued != pt.QuantityReceived)
            {
                pt.Status = PartTransferStatus.ForceCompPend;
                pt.LastEvent =PartTransferEvents.ForceCompPend;
            }
            else
            {
                pt.Status = PartTransferStatus.Complete;
                pt.LastEvent = PartTransferEvents.ForceComplete;
            }

            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.EventCode = ptd.ForceCompleteReasonId;
            pt.Comments = ptd.Comment;
            pt.ConfirmForceComplete = false;
            pt.PartTransferForceComplete(this.userData.DatabaseKey, userData.Site.TimeZone);

            try
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferForceCompPend, pt.PartTransferId, pt.PartTransferEventLogId);
            }
            catch (Exception ex)
            {

            }
            return pt.ErrorMessages;

        }
        #endregion Part ForceComplete


        #region Cancel
        public List<string> PTCancel(PartTransferCancelModel ptm)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptm.PartTransferId ?? 0,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);

            if (pt.QuantityIssued == 0)
            {
                pt.ErrorMessages = new List<string>();
                pt.ErrorMessages.Add("Issued Quantity cannot be 0");
                return pt.ErrorMessages;
            }
            pt.LastEvent = PartTransferStatus.Canceled;
            pt.Status = PartTransferStatus.Canceled;      // RKL - 2023-04-05
            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.EventCode = ptm.CancelReason;//popCancelReason.Value;
            pt.Comments = ptm.CancelComment;//popCancelComments.Text;
            pt.CancelOrDenyState = "Cancel";
            pt.PartTransferCancelDeny(this.userData.DatabaseKey, userData.Site.TimeZone);
            try
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                objAlert.CreatePartTransferAlert(AlertTypeEnum.PartTransferCanceled, pt.PartTransferId, pt.PartTransferEventLogId);
            }
            catch (Exception ex)
            {

            }

           
            return pt.ErrorMessages;
        }
        #endregion Cancel

        #region ConfirmForceComplete
        public List<string> ConfirmFC(PartTransferModel ptm)
        {
            DataContracts.PartTransfer pt = new DataContracts.PartTransfer()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartTransferId = ptm.PartTransferId,//ObjectId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            pt.Retrieve(userData.DatabaseKey);

            pt.LastEvent = PartTransferStatus.ForceComplete;
            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.Quantity = pt.QuantityReceived - pt.QuantityIssued;
            pt.TransactionType = PartHistoryTranTypes.TransferAdjust;
            pt.EventCode = "";// popForceCompleteReason.Value;
            pt.Comments = "";// popForceCompleteComments.Text;
            pt.Status = PartTransferStatus.Complete;
            pt.ConfirmForceComplete = true;
            pt.PartTransferForceComplete(this.userData.DatabaseKey, userData.Site.TimeZone);
            return pt.ErrorMessages;
        }
        #endregion ConfirmForceComplete
    }

}