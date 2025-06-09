using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using Client.Common;
using Client.Models.IoTEvent;
using Database.Business;

namespace Client.BusinessWrapper.IoTEvent
{
    public class IoTEventWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public IoTEventWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<IoTEventModel> GetIotEventInfoGridData(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", long iotEventId = 0, string iotEventSource = "", string iotEventType = "", string ioTStatus = "", string iotDisposition = "", string iotWorkOrderId = "", string iotFaultCode = "",  string assetClentLookupId = "", DateTime? CreateDate = null, string iotDeviceID = "", string txtSearchval = "")
        {
            List<IoTEventModel> ioTEventModelList = new List<IoTEventModel>();
            IoTEventModel ioTEventModel;
            List<DataContracts.IoTEvent> IoTEventList = new List<DataContracts.IoTEvent>();
            DataContracts.IoTEvent ioTEvent = new DataContracts.IoTEvent();
            ioTEvent.CustomQueryDisplayId = CustomQueryDisplayId;
            ioTEvent.ClientId = userData.DatabaseKey.Client.ClientId;
            ioTEvent.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            ioTEvent.OrderbyColumn = orderbycol;
            ioTEvent.OrderBy = orderDir;
            ioTEvent.OffSetVal = skip;
            ioTEvent.NextRow = length;
            ioTEvent.IoTEventId = iotEventId;
            ioTEvent.SourceType = (iotEventSource != null) ? iotEventSource : ""; 
            ioTEvent.EventType = (iotEventType != null) ? iotEventType : "";
            ioTEvent.Status = (ioTStatus != null) ? ioTStatus : "";
            ioTEvent.Disposition = (iotDisposition != null) ? iotDisposition : "";
            ioTEvent.WOClientLookupId = (iotWorkOrderId != null) ? iotWorkOrderId : "";
            ioTEvent.FaultCode = (iotFaultCode != null) ? iotFaultCode : "";
            ioTEvent.CreateDate = CreateDate;
            ioTEvent.AssetId = (assetClentLookupId != null) ? assetClentLookupId : "";
            ioTEvent.IoTDeviceClientLookupId = (iotDeviceID != null) ? iotDeviceID : "";
            ioTEvent.SearchText = txtSearchval;
            IoTEventList = ioTEvent.IoTEventRetrieveChunkSearch(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in IoTEventList)
            {
                ioTEventModel = new IoTEventModel();
                ioTEventModel.ClientId = item.ClientId;
                ioTEventModel.SiteId = item.SiteId;

                ioTEventModel.IoTEventId = item.IoTEventId;
                ioTEventModel.SourceType = item.SourceType;
                ioTEventModel.EventType = item.EventType;
                ioTEventModel.AssetID = item.AssetId;
                ioTEventModel.AssetName = item.AssetName;
                ioTEventModel.Status = item.Status;
                ioTEventModel.Disposition = item.Disposition;
                ioTEventModel.WOClientLookupId = item.WOClientLookupId;
                ioTEventModel.FaultCode = item.FaultCode;
                if (item.CreateDate != null && item.CreateDate != default(DateTime))
                {
                    ioTEventModel.CreateDate = item.CreateDate;
                }
                else
                {
                    ioTEventModel.CreateDate = null;
                }
                ioTEventModel.IoTDeviceClientLookupId = item.IoTDeviceClientLookupId;
                 if (item.ProcessDate != null && item.ProcessDate != default(DateTime))
                {
                    ioTEventModel.ProcessDate = item.ProcessDate;
                }
                else
                {
                    ioTEventModel.ProcessDate = null;
                }
                ioTEventModel.ProcessBy_Personnel = item.ProcessBy_Personnel;
                ioTEventModel.Comments = item.Comments;
                ioTEventModel.TotalCount = item.TotalCount;
                ioTEventModelList.Add(ioTEventModel);
            }

            return ioTEventModelList;
        }

        public IoTEventModel IotEventRetriveById(long IoTEventId)
        {
            var LanguageList = UtilityFunction.LocalizationTypes();
            IoTEventModel model = new IoTEventModel();
            DataContracts.IoTEvent ei = new DataContracts.IoTEvent()
            {
                IoTEventId = IoTEventId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            ei.RetrieveByPKForeignkey(userData.DatabaseKey); // change to retrieve by PK
            model.ClientId = ei.ClientId;
            model.IoTEventId = ei.IoTEventId;
            model.SourceType = ei.SourceType;
            model.EventType = ei.EventType;
            model.EquipClientLookupId = ei.EquipClientLookupId;
            model.AssetName = ei.AssetName;
            model.Status = ei.Status;
            model.Disposition = ei.Disposition;
            model.DismissReason = ei.DismissReason;
            model.WOClientLookupId = ei.WOClientLookupId;
            model.FaultCode = ei.FaultCode;
            if (ei.CreateDate != null && ei.CreateDate == default(DateTime))
            {
                model.CreateDate = null;
            }
            else
            {
                model.CreateDate = ei.CreateDate;
            }
            model.IoTDeviceClientLookupId = ei.IoTDeviceClientLookupId;
            if (ei.ProcessDate != null && ei.ProcessDate == default(DateTime))
            {
                model.ProcessDate = null;
            }
            else
            {
                model.ProcessDate = ei.ProcessDate;
            }
            model.ProcessBy_Personnel = ei.ProcessBy_Personnel;
            model.Comments = ei.Comments;
            return model;
        }


        public List<string> AcknowledgeEvent(IoTEventAcknowledgeModel acknowledgeModel)
        {
            DataContracts.IoTEvent ei = new DataContracts.IoTEvent();
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.IoTEventId = acknowledgeModel.IoTEventId;
            ei.Retrieve(userData.DatabaseKey);
            ei.Disposition = "Acknowledge";
            ei.FaultCode = acknowledgeModel.FaultCode;
            ei.Comments =!string.IsNullOrEmpty(acknowledgeModel.Comments) ? acknowledgeModel.Comments : "";
            ei.Status = EventStatusConstants.Processed;
            ei.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Update(userData.DatabaseKey);
            return ei.ErrorMessages;
        }

        public List<string> DismissEvent(IoTEventDismissModel dismissModel)
        {
            DataContracts.IoTEvent ei = new DataContracts.IoTEvent();
            ei.ClientId = userData.DatabaseKey.Client.ClientId;
            ei.IoTEventId = dismissModel.IoTEventId;
            ei.Retrieve(userData.DatabaseKey);
            ei.Disposition = "Dismiss";
            ei.DismissReason = dismissModel.DismissReason;
            ei.Comments = !string.IsNullOrEmpty(dismissModel.Comments) ? dismissModel.Comments : "";
            ei.Status = EventStatusConstants.Processed;
            ei.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            ei.ProcessDate = DateTime.UtcNow;
            ei.Update(userData.DatabaseKey);
            return ei.ErrorMessages;
        }

    }

}
