/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PopupAddWorkRequest
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Sep-04 SOM-1037 Roger Lawton       Many changes - Cleaned up code (removed commented code)
* 2017-Feb-08 SOM-1228 Roger Lawton       Set the email of the altertargets in the alerttarget list
* 2017-Feb-10 SOM-1199 Roger Lawton       Added PurchaseRequestReturned 
* 2017-Feb-11 SOM-801  Roger Lawton       Added PurchaseRequestDenied 
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;
using INTDataLayer.BAL;
using Notification;
using Microsoft.AspNet.SignalR;
using Presentation.Common;
using INTDataLayer.EL;

namespace DataContracts
{

    public class ProcessAlert
    {
        public ProcessAlert(UserData userdata)
        {
            this.userdata = userdata;
            this.TargetSet = false;
        }
        #region properties
        protected UserData userdata;
        protected AlertSetup setup;
        protected List<AlertTarget> AlertTargetList;
        protected List<AlertTarget> UserInfoTargetList;
        protected bool TargetSet;
        #endregion
        #region Constant
        string SenderId = string.Empty;
        string FCM_SenderId = string.Empty;

        string AndroidRegKey = string.Empty;
        string FCMAndroidRegKey = string.Empty;
        string AndroidRegKey2 = string.Empty;

        #endregion
        // Create Alert for Single Object 
        // May remove this method - the data
        public void CreateAlert<T>(Object obj, AlertTypeEnum alertType) where T : class
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Determine the target for the Alert
                SetTargetList(obj, alertType);
                if (this.AlertTargetList.Count > 0)
                {
                    // Create the alert
                    Alerts alert = AlertCreate(obj, alertType);
                    if (alert.AlertsId > 0)
                    {
                        // alert is created
                        // send to the targets
                        AlertSendToTarget(alert);
                    }
                }
            }
        }
        // Create Alert for List of Object IDs

        public void CreatePartTransferAlert(AlertTypeEnum alertType, long pt_id, long pt_ev_id)
        {
            //
            // Retrieve the PartTransfer 
            PartTransfer pt = new PartTransfer()
            {
                ClientId = userdata.DatabaseKey.Client.ClientId,
                PartTransferId = pt_id
            };
            pt.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
            // Retrieve the PartTransferEventLog 
            PartTransferEventLog ev = new PartTransferEventLog()
            {
                ClientId = pt.ClientId,
                PartTransferEventLogId = pt_ev_id
            };
            ev.RetrieveForAlert(userdata.DatabaseKey, userdata.Site.TimeZone);
            // Retrieved the transfer - now get setup information 
            // If the type is Send then the setup information is for the Issue Site 
            long target_site = 0;
            if (alertType == AlertTypeEnum.PartTransferSend
                || alertType == AlertTypeEnum.PartTransferCanceled
                || alertType == AlertTypeEnum.PartTransferForceCompPend)
            {
                target_site = pt.IssueSiteId;
            }
            // It the type is issue then the setup information is for the request site 
            if (alertType == AlertTypeEnum.PartTransferIssue
                || alertType == AlertTypeEnum.PartTransferReceipt
                || alertType == AlertTypeEnum.PartTransferDenied)
            {
                target_site = pt.RequestSiteId;
            }
            setup = new AlertSetup()
            {
                ClientId = userdata.DatabaseKey.Client.ClientId,
                SiteId = target_site,
                Alert_Name = alertType.ToString()
            };
            setup.RetrieveForNotification(userdata.DatabaseKey);
            if (setup.Alert_Active)
            {
                // Determine the target for the Alert
                SetTargetList(pt, alertType);
                if (this.AlertTargetList.Count > 0)
                {

                    // Create the alert
                    Alerts alert = AlertCreate(pt, ev, alertType);
                    if (alert.AlertsId > 0)
                    {
                        // alert is created
                        // send to the targets
                        AlertSendToTarget(alert);
                    }
                }
            }
        }
        // Create Alert for List of Object IDs
        public void CreateAlert<T>(AlertTypeEnum alertType, List<long> obj_ids) where T : class
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            Type type = typeof(T);
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Process the following for each item in the object id list
                WorkOrder wo;
                PurchaseOrder po;
                PurchaseRequest pr;
                Object obj = null;
                PartMasterRequest pmr;
                ServiceOrder So;
                BBUKPI bBUKPI;
                IoTEvent ioTEvent;
                foreach (long objectid in obj_ids)
                {
                    // Get the object
                    if (type == typeof(WorkOrder))
                    {
                        wo = new WorkOrder()
                        {
                            ClientId = setup.ClientId,
                            WorkOrderId = objectid
                        };
                        wo.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = wo;
                    }
                    if (type == typeof(PurchaseOrder))
                    {
                        po = new PurchaseOrder()
                        {
                            ClientId = setup.ClientId,
                            PurchaseOrderId = objectid
                        };
                        po.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = po;
                    }
                    if (type == typeof(PurchaseRequest))
                    {
                        pr = new PurchaseRequest()
                        {
                            ClientId = setup.ClientId,
                            PurchaseRequestId = objectid
                        };
                        pr.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = pr;
                    }
                    //SOM-1515
                    if (type == typeof(PartMasterRequest))
                    {
                        pmr = new PartMasterRequest()
                        {
                            ClientId = setup.ClientId,
                            PartMasterRequestId = objectid
                        };
                        pmr.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = pmr;
                    }
                    if (type == typeof(ServiceOrder))
                    {
                        So = new ServiceOrder()
                        {
                            ClientId = setup.ClientId,
                            SiteId=setup.SiteId,
                            ServiceOrderId = objectid
                        };
                        So= So.RetrieveByServiceOrderId(userdata.DatabaseKey);
                        obj = So;
                    }
                    if (type == typeof(BBUKPI))
                    {
                        bBUKPI = new BBUKPI()
                        {
                            ClientId = setup.ClientId,
                            SiteId = setup.SiteId,
                            BBUKPIId = objectid
                        };
                        bBUKPI.RetriveSiteDetailsByBBUKPIId(userdata.DatabaseKey);
                        obj = bBUKPI;
                    }
                    if (type == typeof(IoTEvent))
                    {
                        ioTEvent = new IoTEvent()
                        {
                            ClientId = setup.ClientId,
                            SiteId = setup.SiteId,
                            IoTEventId = objectid
                        };
                        ioTEvent.RetrieveByPKForeignkey(userdata.DatabaseKey);
                        obj = ioTEvent;
                    }
                    // Determine the target for the Alert
                    SetTargetList(obj, alertType);
                    if (this.AlertTargetList.Count > 0)
                    {
                        // Create the alert
                        Alerts alert = AlertCreate(obj, alertType);
                        if (alert.AlertsId > 0)
                        {
                            // alert is created
                            // send to the targets
                            AlertSendToTarget(alert);
                        }
                    }
                }
            }
        }
        // Create Alert for Purchase Order Receipt
        // Sending type in case we want to reuse for another type later
        public void CreateAlert(AlertTypeEnum alertType, long param1, decimal param2)
        {
            switch (alertType)
            {
                case AlertTypeEnum.PurchaseOrderReceipt:

                    PurchaseOrderLineItem pol = new PurchaseOrderLineItem()
                    {
                        ClientId = userdata.DatabaseKey.Client.ClientId,
                        PurchaseOrderLineItemId = param1
                    };
                    pol.RetrieveForAlert_V2(userdata.DatabaseKey);
                    // Check if a purchase request exists and if it was not auto-generated
                    if (pol.PurchaseRequestId > 0 && !pol.PurchaseRequest_AutoGenerated)
                    {
                        setup = RetrieveAlertInfo(alertType);
                        pol.QuantityReceived = param2;
                        // Determine the target for the Alert
                        SetTargetList(pol, alertType);
                        if (this.AlertTargetList.Count > 0)
                        {
                            // Create the alert
                            Alerts alert = AlertCreate(pol, alertType);
                            if (alert.AlertsId > 0)
                            {
                                // alert is created
                                // send to the targets
                                AlertSendToTarget(alert);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        // Create Alert for Sensor Reading Work Order Created 
        // Sending type to use for both SensorReadingAlert and SensorReadingAlertWO
        public void CreateAlertforSensorReading(AlertTypeEnum alertType, long SensorReadingId, long workorderid)
        {
            switch (alertType)
            {
                case AlertTypeEnum.SensorReadingAlertWO:

                    // Retrieve the sensor reading based on the passed id
                    setup = RetrieveAlertInfo(alertType);
                    if (setup.IsActive)
                    {
                        SensorReading sr = new SensorReading()
                        {
                            ClientId = setup.ClientId,
                            SensorReadingId = SensorReadingId
                        };
                        sr.Retrieve(userdata.DatabaseKey);
                        if (sr.SensorReadingId > 0)
                        {
                            // Retrieve the Equipment Sensor Cross-Reference Data
                            Equipment_Sensor_Xref EQ_SX = new Equipment_Sensor_Xref()
                            {
                                ClientId = userdata.DatabaseKey.Client.ClientId,
                                SensorId = sr.SensorID
                            };
                            EQ_SX.RetriveBySensorId(userdata.DatabaseKey);
                            if (EQ_SX.Equipment_Sensor_XrefId > 0)
                            {
                                WorkOrder wo = new WorkOrder
                                {
                                    ClientId = userdata.DatabaseKey.Client.ClientId,
                                    SiteId = userdata.DatabaseKey.User.DefaultSiteId,
                                    WorkOrderId = workorderid
                                };
                                wo.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                                if (wo.WorkOrderId > 0)
                                {
                                    // Determine the target for the Alert
                                    SetTargetList(sr, alertType);             // Currently a target list - will change to get person assigned to wo
                                    if (this.AlertTargetList.Count > 0)
                                    {
                                        Alerts alert = new Alerts();
                                        alert.ClientId = setup.ClientId;
                                        alert.AlertDefineId = setup.AlertDefineId;
                                        alert.From = userdata.DatabaseKey.UserName;
                                        alert.AlertType = setup.Alert_Type;
                                        alert.IsCleared = false;
                                        alert.ObjectName = "SensorReading";
                                        alert.ObjectId = sr.SensorReadingId;
                                        // Headline - 'Work Order {0} Generated From Sensor Reading'
                                        alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                                        alert.Summary = string.Empty;
                                        // Details - Work Order: {0}~Description: {1}~Equipment ID: {2}~Equipment Name: {3}~Sensor Name: {4}~Sensor Value: {5}
                                        string details = setup.Alert_Details;
                                        details = details.Replace("~", "\r\n");
                                        details = string.Format(details, wo.ClientLookupId
                                                                       , wo.Description
                                                                       , wo.ChargeToClientLookupId
                                                                       , wo.ChargeTo_Name
                                                                       , EQ_SX.SensorName
                                                                       , sr.PlotValues);
                                        alert.Details = details;
                                        alert.Create(userdata.DatabaseKey);
                                        if (alert.AlertsId > 0)
                                        {
                                            // alert is created
                                            // send to the targets
                                            AlertSendToTarget(alert);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        // V2-285 - RKL 2020-Mar-05
        // This is only user for items with a user list
        // The user list is the target list 
        // Change to send only one alert with multiple alert users
        public void CreateAlert<T>(AlertTypeEnum alertType, List<long> obj_ids, Object notes, List<Tuple<long, string, string>> UserList) where T : class //V2-276
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            Type type = typeof(T);
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Process the following for each item in the object id list
                WorkOrder wo;
                PurchaseOrder po;
                PurchaseRequest pr;
                Equipment equ;
                Part part;
                Object obj = null;
                PartMasterRequest pmr;
                ServiceOrder serviceOrder;
                WorkOrderPlan workorderPlan;
                Project project;
                foreach (long objectid in obj_ids)
                {
                    // Get the object
                    if (type == typeof(WorkOrder))
                    {
                        wo = new WorkOrder()
                        {
                            ClientId = setup.ClientId,
                            WorkOrderId = objectid
                        };
                        wo.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = wo;

                    }
                    if (type == typeof(PurchaseOrder))
                    {
                        po = new PurchaseOrder()
                        {
                            ClientId = setup.ClientId,
                            PurchaseOrderId = objectid
                        };
                        po.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = po;
                    }
                    if (type == typeof(PurchaseRequest))
                    {
                        pr = new PurchaseRequest()
                        {
                            ClientId = setup.ClientId,
                            PurchaseRequestId = objectid
                        };
                        pr.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = pr;
                    }
                    if (type == typeof(Equipment))
                    {
                        equ = new Equipment()
                        {
                            ClientId = setup.ClientId,
                            EquipmentId = objectid
                        };
                        equ.RetrieveforMentionAlert(userdata.DatabaseKey);
                        obj = equ;
                    }
                    if (type == typeof(Part))
                    {
                        part = new Part()
                        {
                            ClientId = setup.ClientId,
                            PartId = objectid
                        };
                        part.PartRetrieveforMentionAlert(userdata.DatabaseKey);
                        obj = part;
                    }
                    if (type == typeof(PartMasterRequest))
                    {
                        pmr = new PartMasterRequest()
                        {
                            ClientId = setup.ClientId,
                            PartMasterRequestId = objectid
                        };
                        pmr.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = pmr;
                    }
                    if (type == typeof(ServiceOrder))
                    {
                        serviceOrder = new ServiceOrder()
                        {
                            ClientId = setup.ClientId,
                            ServiceOrderId = objectid
                        };
                        serviceOrder.Retrieve(userdata.DatabaseKey);
                        obj = serviceOrder;
                    }
                    if (type == typeof(WorkOrderPlan))
                    {
                        workorderPlan = new WorkOrderPlan()
                        {
                            ClientId = setup.ClientId,
                            WorkOrderPlanId = objectid
                        };
                        workorderPlan.Retrieve(userdata.DatabaseKey);
                        obj = workorderPlan;
                    }
                  
                    if (type == typeof(Project))
                    {
                        project = new Project()
                        {
                            ClientId = setup.ClientId,
                            ProjectId = objectid
                        };
                        project.Retrieve(userdata.DatabaseKey);
                        obj = project;
                    }
                    //V2-592 for WorkOrderPlan
                    //V2-594 for Project
                    // V2-285 - RKL - 2020-Mar-05
                    // We have retrieved the "object" 
                    // Now set the target list
                    // The target list is set from the passed User List
                    SetTargetList(obj, alertType, UserList);
                    if (this.AlertTargetList.Count > 0)
                    {
                        // We only need one alert with multiple alert users
                        // The parameters are, object (Equipment, Work Order,etc.), Notes (the notes), alerttype, "From"
                        Alerts alert = AlertCreate(obj, (Notes)notes, alertType, userdata.DatabaseKey.UserName);
                        if (alert != null)
                        {
                            AlertSendToTarget(alert);
                        }
                        /*
                        // Create the alert
                        Alerts alert=null;
                        Alerts alertUser = null;                        
                        List<Alerts> alertAll = new List<Alerts>();
                        if (alertType == AlertTypeEnum.WorkOrderCommentMention  || alertType == AlertTypeEnum.AssetCommentMention || alertType == AlertTypeEnum.PartCommentMention)
                        {                          
                            foreach (var  entry in UserList)
                            {
                                alertUser = AlertCreate(obj, (Notes)notes, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                       else if (alertType == AlertTypeEnum.WorkOrderAssign)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        else
                        {
                            alert = AlertCreate(obj, alertType);
                        }

                        if (alert!=null &&   alert.AlertsId > 0)
                        {
                            // alert is created
                            // send to the targets
                            AlertSendToTarget(alert);
                        }

                        if(alertAll !=null && alertAll.Count>0)
                        {   foreach (var item in alertAll)
                        {
                            if (item.AlertsId > 0)
                            {
                                // alert is created
                                // send to the targets
                                //AlertSendToTarget(item);
                                AlertSendToCustomTarget(item, UserList.Where(x=>x.Item2==item.From).FirstOrDefault());
                            }

                        }}
                        */
                    }
                }
            }
        }
        public void CreateAlert<T>(AlertTypeEnum alertType, List<long> obj_ids, List<Tuple<long, string, string>> UserList) where T : class //V2-293
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            Type type = typeof(T);
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Process the following for each item in the object id list
                WorkOrder wo;
                ServiceOrder SO;
                MaterialRequest mr;
                PurchaseRequest pr;
                BBUKPI bBUKPI;
                Object obj = null;
                foreach (long objectid in obj_ids)
                {
                    // Get the object
                    if (type == typeof(WorkOrder))
                    {
                        wo = new WorkOrder()
                        {
                            ClientId = setup.ClientId,
                            WorkOrderId = objectid
                        };
                        wo.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = wo;

                    }

                    if (type == typeof(ServiceOrder))
                    {
                        SO = new ServiceOrder()
                        {
                            ClientId = setup.ClientId,
                            ServiceOrderId = objectid
                        };
                        SO.Retrieve(userdata.DatabaseKey);
                        obj = SO;
                    }

                    //V2-726
                    if (type == typeof(MaterialRequest))
                    {
                        mr = new MaterialRequest()
                        {
                            ClientId = setup.ClientId,
                            MaterialRequestId = objectid
                        };
                        mr.RetriveByMaterialRequestId(userdata.DatabaseKey);
                        obj = mr;

                    }
                    if (type == typeof(PurchaseRequest))
                    {
                        pr = new PurchaseRequest()
                        {
                            ClientId = setup.ClientId,
                            PurchaseRequestId = objectid
                        };
                        pr.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = pr;
                    }
                    #region V2-823
                    if (type == typeof(BBUKPI))
                    {
                        bBUKPI = new BBUKPI()
                        {
                            ClientId = setup.ClientId,
                            SiteId=userdata.Site.SiteId,
                            BBUKPIId = objectid
                        };
                        bBUKPI.RetriveEnterpriseDetailsByBBUKPIId(userdata.DatabaseKey, userdata.Site.TimeZone);
                        obj = bBUKPI;
                    }
                    #endregion
                    // Determine the target for the Alert
                    if (alertType == AlertTypeEnum.WorkOrderApprovalNeeded)
                    {
                        SetTargetList(obj, alertType, UserList);
                    }
                    else if(alertType == AlertTypeEnum.MaterialRequestApprovalNeeded)
                    {
                        SetTargetList(obj, alertType,type);
                    }
                    else if (alertType == AlertTypeEnum.PurchaseRequestApprovalNeeded)
                    {
                        SetTargetList(obj, alertType, type);
                    }
                    else if (alertType == AlertTypeEnum.KPIReOpened)//V2-823
                    {
                        SetTargetList(obj, alertType, UserList);
                    }
                    else
                    { 
                        SetTargetList(obj, alertType);
                    }
                    if (this.AlertTargetList.Count > 0)
                    {
                        // Create the alert                       
                        Alerts alertUser = null;
                        List<Alerts> alertAll = new List<Alerts>();
                        if (alertType == AlertTypeEnum.WorkOrderAssign)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        //Service Order
                        if (alertType == AlertTypeEnum.ServiceOrderAssign)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        //WorkOrderApprovalNeeded
                        if (alertType == AlertTypeEnum.WorkOrderApprovalNeeded)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        //V2-726 MaterialRequestApprovalNeeded
                        if (alertType == AlertTypeEnum.MaterialRequestApprovalNeeded)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2,type);
                                alertAll.Add(alertUser);
                            }
                        }
                        if (alertType == AlertTypeEnum.WRApprovalRouting)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        if (alertType == AlertTypeEnum.PurchaseRequestApprovalNeeded)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        #region V2-823
                        if (alertType == AlertTypeEnum.KPIReOpened)
                        {
                            foreach (var entry in UserList)
                            {
                                alertUser = AlertCreate(obj, alertType, entry.Item2);
                                alertAll.Add(alertUser);
                            }
                        }
                        #endregion
                        if (alertAll != null && alertAll.Count > 0)
                        {
                            foreach (var item in alertAll)
                            {
                                if (item.AlertsId > 0)
                                {
                                    // alert is created
                                    // send to the targets
                                    //AlertSendToTarget(item);
                                    AlertSendToCustomTarget(item, UserList.Where(x => x.Item2 == item.From).FirstOrDefault());
                                }

                            }
                        }


                    }
                }
            }
        }


        /// <summary>
        /// SetTargetList - Public
        /// Used to set the target list from the calling routine
        /// Required for Alert Types:
        ///   AlertTypeEnum.PurchaseRequestApprovalNeeded
        /// </summary>
        /// <param name="targets"></param>



        public void SetTargetList(List<long> targets)
        {
            this.TargetSet = true;
            this.AlertTargetList = new List<AlertTarget>();
            foreach (long peid in targets)
            {
                AlertTarget target = new AlertTarget();
                target.UserInfoId = peid;
                AlertTargetList.Add(target);
                GetUserInfoList("personnel", peid);
            }
            SetEmailList();
        }
        // SOM-1228
        private void SetEmailList()
        {
            Personnel personnel = new Personnel()
            {
                ClientId = userdata.DatabaseKey.Client.ClientId,
                SiteId = userdata.Site.SiteId,
                AlertTargetList = this.AlertTargetList
            };
            personnel.FillAlertTargetEmail(userdata.DatabaseKey);
        }

        // SOM-1037
        // Protected Methods
        // Method to retrieve the information for a particular alert
        protected AlertSetup RetrieveAlertInfo(AlertTypeEnum alerttype)
        {
            // Retrieve 
            AlertSetup setup = new AlertSetup()
            {
                ClientId = userdata.DatabaseKey.Client.ClientId,
                SiteId = userdata.DatabaseKey.User.DefaultSiteId,
                Alert_Name = alerttype.ToString()
            };
            setup.RetrieveForNotification(userdata.DatabaseKey);
            return setup;
        }

        private void GetUserInfoList(string usertype, Int64 Pid)
        {
            LoginInfo l = new LoginInfo();
            Personnel p = new Personnel();
            AlertTarget t = new AlertTarget();
            switch (usertype.ToLower())
            {
                case "personnel":
                    l = new LoginInfo();
                    p = new Personnel();
                    t = new AlertTarget();
                    p.ClientId = userdata.DatabaseKey.Client.ClientId;
                    p.PersonnelId = Pid;
                    p.PersonnelIds = new List<long>();
                    p.PersonnelIds.Add(Pid);
                    var perList = p.RetrieveByPKs(userdata.DatabaseKey);
                    l.ClientId = userdata.DatabaseKey.Client.ClientId;
                    l.UserInfoId = perList.FirstOrDefault()?.UserInfoId ?? default(long);
                    l.RetrieveByUserInfoId(userdata.DatabaseKey);
                    t.UserInfoId = p.UserInfoId;
                    t.CallerUserName = l.UserName;
                    t.UserName = l.UserName;
                    UserInfoTargetList = new List<AlertTarget>();
                    UserInfoTargetList.Add(t);
                    break;
                case "user":
                    l = new LoginInfo();
                    p = new Personnel();
                    t = new AlertTarget();
                    l.ClientId = userdata.DatabaseKey.Client.ClientId;
                    l.UserInfoId = p.UserInfoId;
                    l.RetrieveByUserInfoId(userdata.DatabaseKey);
                    t.UserInfoId = Pid;
                    t.CallerUserName = l.UserName;
                    t.UserName = l.UserName;
                    UserInfoTargetList = new List<AlertTarget>();
                    UserInfoTargetList.Add(t);

                    break;
            }
        }
        /// <summary>
        /// V2-285 - RKL - 2020-Mar-05 
        /// Set the target list for types with a user list
        protected void SetTargetList(object obj, AlertTypeEnum alertType, List<Tuple<long, string, string>> UserList)
        {
            if (!this.TargetSet)
            {
                this.AlertTargetList = new List<AlertTarget>();
                if (UserList.Count > 0)
                {
                    foreach (Tuple<long, string, string> item in UserList)
                    {
                        AlertTarget target = new AlertTarget();
                        target.UserInfoId = item.Item1;
                        target.CallerUserName = item.Item2;
                        target.UserName = item.Item2;
                        target.email_address = item.Item3;
                        AlertTargetList.Add(target);
                    }
                    this.UserInfoTargetList = this.AlertTargetList;
                }
            }
        }
        // Method to retrieve the target list for a particular alert
        protected void SetTargetList(Object obj, AlertTypeEnum alertType)
        {
            if (!this.TargetSet)
            {
                this.AlertTargetList = new List<AlertTarget>();
                if (setup.Alert_TargetList)
                {
                    // Retrieve the target list
                    AlertTarget alerttarget = new AlertTarget()
                    {
                        ClientId = setup.ClientId,
                        AlertSetupId = setup.AlertSetupId
                    };
                    AlertTargetList = alerttarget.RetreiveTargetList(userdata.DatabaseKey);
                    foreach (AlertTarget at in AlertTargetList)
                    {
                        GetUserInfoList("personnel", at.UserInfoId);
                    }


                }
                else
                {
                    // Based on the alert - create a target list - could only be one 
                    switch (alertType)
                    {
                        case AlertTypeEnum.WorkRequestApproved:
                        case AlertTypeEnum.WorkRequestDenied:
                        case AlertTypeEnum.WorkOrderComplete:
                            WorkOrder workorder = (WorkOrder)obj;
                            // Creator's PersonnelId 
                            // SOM-1134 - Added the Sourcetype check
                            if (workorder.Creator_PersonnelId > 0 && workorder.SourceType != WorkOrderSourceTypes.PreventiveMaint)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = workorder.Creator_PersonnelId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("personnel", target.UserInfoId);
                            }
                            break;
                        case AlertTypeEnum.WorkOrderCommentMention:
                        case AlertTypeEnum.WorkOrderAssign:                       
                            WorkOrder wo = (WorkOrder)obj;
                            if (wo.Creator_PersonnelId > 0)
                            {
                                AlertTarget t = new AlertTarget();
                                t.UserInfoId = wo.Creator_PersonnelId;
                                AlertTargetList.Add(t);
                                GetUserInfoList("personnel", t.UserInfoId);
                            }

                            break;
                        //case AlertTypeEnum.WorkOrderApprovalNeeded:
                        //    WorkOrder woApprovalNeeded = (WorkOrder)obj;
                        //    //if (wo.Creator_PersonnelId > 0)
                        //    //{
                        //        AlertTarget tApprovalNeeded = new AlertTarget();
                        //    //t.UserInfoId = woApprovalNeeded.Creator_PersonnelId;
                        //    tApprovalNeeded.UserInfoId = woApprovalNeeded.ApproveBy_PersonnelId;
                        //    AlertTargetList.Add(tApprovalNeeded);
                        //        GetUserInfoList("personnel", tApprovalNeeded.UserInfoId);
                        //   // }

                        //    break;
                        case AlertTypeEnum.ServiceOrderComplete:
                        case AlertTypeEnum.ServiceOrderAssign:
                            ServiceOrder SO = (ServiceOrder)obj;
                            if (SO.ServiceOrderId > 0)
                            {
                                AlertTarget t = new AlertTarget();
                                t.UserInfoId = userdata.DatabaseKey.Personnel.PersonnelId;
                                AlertTargetList.Add(t);
                                GetUserInfoList("personnel", t.UserInfoId);
                            }

                            break;

                        case AlertTypeEnum.PurchaseRequestApproved:
                        case AlertTypeEnum.PurchaseRequestConverted:
                        case AlertTypeEnum.PurchaseRequestReturned:             // SOM-1199
                        case AlertTypeEnum.PurchaseRequestDenied:               // SOM-801
                            PurchaseRequest pr = (PurchaseRequest)obj;
                            // Creator's PersonnelId 
                            if (pr.CreatedBy_PersonnelId > 0)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = pr.CreatedBy_PersonnelId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("personnel", target.UserInfoId);
                            }
                            break;
                        case AlertTypeEnum.PurchaseOrderReceipt:
                            PurchaseOrderLineItem pol = (PurchaseOrderLineItem)obj;
                            // Creator's PersonnelId 
                            if (pol.PurchaseRequest_Creator_PersonnelId > 0)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = pol.PurchaseRequest_Creator_PersonnelId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("personnel", target.UserInfoId);
                            }
                            break;
                        //SOM-1351
                        // Will be adding an alert to the person assigned if they exist. 
                        // SensorReadingAlert is currently using a target list
                        case AlertTypeEnum.SensorReadingAlert:
                            SensorReading sensorlist = (SensorReading)obj;
                            if (sensorlist.SensorReadingId > 0)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = userdata.DatabaseKey.User.UserInfoId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("user", target.UserInfoId);
                            }
                            break;
                        case AlertTypeEnum.POEmailToVendor:
                        case AlertTypeEnum.POImportedReviewRequired:
                            PurchaseOrder purchaseOrder = (PurchaseOrder)obj;
                            if (purchaseOrder.PurchaseOrderId > 0)
                            {
                                AlertTarget target;
                                if (purchaseOrder.Creator_PersonnelId > 0)
                                {
                                    target = new AlertTarget();
                                    target.UserInfoId = purchaseOrder.Creator_PersonnelId;
                                    AlertTargetList.Add(target);
                                    GetUserInfoList("personnel", target.UserInfoId);
                                }
                                // Have to check if buyer and creator are the same person. 
                                if (purchaseOrder.Buyer_PersonnelId > 0 && (purchaseOrder.Buyer_PersonnelId != purchaseOrder.Creator_PersonnelId))
                                {
                                    target = new AlertTarget();
                                    target.UserInfoId = purchaseOrder.Buyer_PersonnelId;
                                    AlertTargetList.Add(target);
                                    GetUserInfoList("personnel", target.UserInfoId);
                                }
                            }
                            break;
                        case AlertTypeEnum.PartMasterRequestProcessed:
                        case AlertTypeEnum.PartMasterRequestAdditionProcessed:
                        case AlertTypeEnum.PartMasterRequestReplaceProcessed:
                        case AlertTypeEnum.PartMasterRequestInactivationProcessed:
                        case AlertTypeEnum.PartMasterRequestSXReplaceProcessed:
                        case AlertTypeEnum.PartMasterRequestSiteApproved:
                        case AlertTypeEnum.PartMasterRequestApproved:
                        case AlertTypeEnum.PartMasterRequestDenied:
                        case AlertTypeEnum.PartMasterRequestReturned:
                        case AlertTypeEnum.PartMasterRequestECO_ReplaceProcessed:
                        case AlertTypeEnum.PartMasterRequestECO_SX_ReplaceProcessed:
                            PartMasterRequest pmr = (PartMasterRequest)obj;
                            if (pmr.PartMasterRequestId > 0)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = pmr.CreatedBy_PersonnelId;
                                AlertTargetList.Add(target);
                            }
                            break;
                        case AlertTypeEnum.PartTransferIssue:
                        case AlertTypeEnum.PartTransferReceipt:
                        case AlertTypeEnum.PartTransferDenied:
                            PartTransfer pt = (PartTransfer)obj;
                            if (pt.Creator_PersonnelId > 0)
                            {
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = pt.Creator_PersonnelId;
                                AlertTargetList.Add(target);
                            }
                            break;
                        // V2-285 - RKL - 2020-Mar-05 
                        // The notification and email for an AssetCommentMention should be to the person selected 
                        // (the user selected with the @)
                        // NOT the Equipment Creator
                        // To Implement - I created a new SetTargetList Method with a different set of parameters.
                        // Creator's PersonnelId - NO - PER RKL
                        /*
                        case AlertTypeEnum.AssetCommentMention:
                            Equipment equipment = (Equipment)obj;
                            if (!string.IsNullOrEmpty(equipment.CreateBy))
                            {
                                LoginInfo l = new LoginInfo();
                                l.ClientId = userdata.DatabaseKey.Client.ClientId;
                                
                                l.UserName = equipment.CreateBy;
                                l.RetrieveByUserName(userdata.DatabaseKey);
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = l.UserInfoId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("personnel", target.UserInfoId);
                            }

                            break;
                        */
                        case AlertTypeEnum.PartCommentMention:
                            Part prt = (Part)obj;
                            // Creator's PersonnelId 

                            if (!string.IsNullOrEmpty(prt.CreateBy))
                            {
                                LoginInfo l = new LoginInfo();
                                l.ClientId = userdata.DatabaseKey.Client.ClientId;

                                l.UserName = prt.CreateBy;
                                l.RetrieveByUserName(userdata.DatabaseKey);
                                AlertTarget target = new AlertTarget();
                                target.UserInfoId = l.UserInfoId;
                                AlertTargetList.Add(target);
                                GetUserInfoList("personnel", target.UserInfoId);
                            }

                            break;
                        case AlertTypeEnum.WRApprovalRouting:
                            WorkOrder wr = (WorkOrder)obj;
                            if (wr.Creator_PersonnelId > 0)
                            {
                                AlertTarget t = new AlertTarget();
                                t.UserInfoId = wr.Creator_PersonnelId;
                                AlertTargetList.Add(t);
                                GetUserInfoList("personnel", t.UserInfoId);
                            }

                            break;
                            //V2-1077
                        case AlertTypeEnum.WorkOrderPlanner:
                            WorkOrder WorkOrderdetails = (WorkOrder)obj;
                            if (WorkOrderdetails.Planner_PersonnelId > 0)
                            {
                                AlertTarget t = new AlertTarget();
                                t.UserInfoId = WorkOrderdetails.Planner_PersonnelId;
                                AlertTargetList.Add(t);
                                GetUserInfoList("personnel", t.UserInfoId);
                            }

                            break;

                        default:
                            break;
                    }
                    // SOM-1228 - Set the email of the altertargets in the alerttarget list
                    SetEmailList();
                }
            }
            return;
        }

        protected Alerts AlertCreate(Object obj, AlertTypeEnum alert_type)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;
            #region V2-1077
            // WorkOrder  Planner
            if (alert_type == AlertTypeEnum.WorkOrderPlanner)
            {
                WorkOrder wo = (WorkOrder)obj;
                // Headline - You have been assigned on Work Order {0}
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details - You have been assigned to work order:{0}~Description:{1}~Charge To:{2}~{3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.Description.Length > 100 ? wo.Description.Substring(0, 100) : wo.Description
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               );
                alert.Details = details;

            }
            #endregion
            // Work Request Approval Needed
            if (alert_type == AlertTypeEnum.WorkRequestApprovalNeeded)
            {
                WorkOrder wo = (WorkOrder)obj;
                // Headline - Work Request {0} had been added 
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details - Work Request: {0}\nRequestor: {1}\nCharge Type: {2}\nCharge To:  {3}\nCharge To Name: {4}\nDescription: {5}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description);
                alert.Details = details;

            }

            // Work Request Approved
            if (alert_type == AlertTypeEnum.WorkRequestApproved)
            {
                WorkOrder wo = (WorkOrder)obj;
                // Headline - Work Request {0} had been approved
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details -         Work Request: {0}~Requestor: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}~Approved by: {6}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description
                                               , wo.ApproveBy_PersonnelClientLookupId);
                alert.Details = details;

            }

            // Work Request Denied
            if (alert_type == AlertTypeEnum.WorkRequestDenied)
            {
                WorkOrder wo = (WorkOrder)obj;
                // Headline - Work Request {0} has been denied
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details - Work Request: {0}~Requestor: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}~Denied By: {6}~Deny Reason: {7}~Deny Comments{8}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description
                                               , wo.DeniedBy_PersonnelId_Name
                                               , wo.DeniedReason
                                               , wo.DeniedComment);
                alert.Details = details;
            }

            // Work Order Complete
            if (alert_type == AlertTypeEnum.WorkOrderComplete)
            {
                WorkOrder wo = (WorkOrder)obj;
                // Headline - Work Order {0} complete
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Work Order: {0}~Requester: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}~Complete By:{6}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description
                                               , wo.CompleteBy_PersonnelName);
                alert.Details = details;
            }



            // WorkOrderCommentMention
            if (alert_type == AlertTypeEnum.WorkOrderCommentMention)
            {               
                WorkOrder wo = (WorkOrder)obj;
                // Headline - Work Request {0} had been approved
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details -         Work Request: {0}~Requestor: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}~Approved by: {6}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description
                                               , wo.ApproveBy_PersonnelClientLookupId);
                alert.Details = details;

            }
            // Purchase Request Approval Needed
            if (alert_type == AlertTypeEnum.PurchaseRequestApprovalNeeded)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} needs approval
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Requester: {1}~Reason: {2}~Comments: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Creator_PersonnelName
                                               , pr.Reason
                                               , pr.Process_Comments);
                alert.Details = details;

            }
            // SOM-1199
            // Purchase Request Returned to Requester/Creator
            if (alert_type == AlertTypeEnum.PurchaseRequestReturned)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} needs approval
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Reason: {1}~Returned By: {2}~Returned Date/Time: {3}~Comments: {4}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Reason
                                               , pr.Processed_PersonnelName
                                               , pr.Processed_Date.ConvertFromUTCToUser(userdata.Site.TimeZone).ToString("g")
                                               , pr.Return_Comments);
                alert.Details = details;

            }


            // Purchase Request Approved
            if (alert_type == AlertTypeEnum.PurchaseRequestApproved)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} has been approved
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Reason: {1}~Approved By: {2}~Approved Date/Time: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime appv_date = pr.Approved_Date ?? DateTime.UtcNow;
                DateTime appv_loc = appv_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Reason
                                               , pr.Approved_PersonnelName
                                               , appv_loc.ToString("g"));
                alert.Details = details;

            }
            // SOM-801
            // Purchase Request Denied
            if (alert_type == AlertTypeEnum.PurchaseRequestDenied)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} has been denied
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Reason: {1}~Denied By: {2}~Denied Date/Time: {3}~Deny Comments: {4}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime appv_date = pr.Approved_Date ?? DateTime.UtcNow;
                DateTime appv_loc = appv_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Reason
                                               , pr.Processed_PersonnelName
                                               , pr.Processed_Date.ConvertFromUTCToUser(userdata.Site.TimeZone).ToString("g")
                                               , pr.Process_Comments);
                alert.Details = details;

            }

            // Purchase Request Converted
            if (alert_type == AlertTypeEnum.PurchaseRequestConverted)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} Converted to Purchase Order {1}
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId, pr.PurchaseOrderClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Reason: {1}~Converted to Purchase Order {2} ~Converted By: {3}~Converted Date/Time: {4}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime proc_date = pr.ProcessedDate;
                DateTime proc_local = proc_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Reason
                                               , pr.PurchaseOrderClientLookupId
                                               , pr.Processed_PersonnelName
                                               , proc_local.ToString("g"));
                alert.Details = details;

            }
            // Purchase Order Receipt
            if (alert_type == AlertTypeEnum.PurchaseOrderReceipt)
            {
                PurchaseOrderLineItem pol = (PurchaseOrderLineItem)obj;
                // Headline - PO Receipt–Order: {0} – Request: {1}'
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseOrderLineItem";
                alert.ObjectId = pol.PurchaseOrderLineItemId;
                alert.Headline = string.Format(setup.Alert_Headline, pol.PurchaseOrder_ClientLookupId, pol.PurchaseRequest_ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Order: {0}~Request: {1}~Line Item: {2}~Description: {3}~Part Number: {4}~Quantity Received: {5}~Charge Type: {6}~Charge To: {7}~Charge To Name: {8}~Vendor ID: {9}~Vendor Name: {10}'  //V2-598
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                //DateTime proc_date = pol.;
                //DateTime proc_local = proc_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pol.PurchaseOrder_ClientLookupId
                                               , pol.PurchaseRequest_ClientLookupId
                                               , pol.LineNumber.ToString()
                                               , pol.Description
                                               , pol.PartClientLookupId
                                               , pol.QuantityReceived.ToString("N2")
                                               , pol.ChargeType
                                               , pol.ChargeToClientLookupId
                                               , pol.ChargeTo_Name
                                               , pol.Vendor_ClientLookupId
                                               , pol.Vendor_Name);
                alert.Details = details;

            }
            //SOM-1515
            if (alert_type == AlertTypeEnum.PartMasterRequestSiteApprovalNeeded)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                //'Part Master Request {0} needs site approval'-- Headline
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - 'Part Master Request {0} needs site approval~Created By: {1}~Created On: {2}~Manufacturer: {3}~Manufacturer ID {4}: ~Justification: {5}') --Details
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime c_date = pmr.CreatedDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Create Date = Converted by Retrieval method
                //DateTime c_loc = c_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pmr.PartMasterRequestId
                                               , pmr.Requester
                                               , c_date.ToString("g")
                                               , pmr.Manufacturer
                                               , pmr.ManufacturerId
                                               , pmr.Justification);
                alert.Details = details;

            }
            if (alert_type == AlertTypeEnum.PartMasterRequestApprovalNeeded)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                //'Part Master Req {0}-{1} enterprise approval'-- Headline
                string site_name = pmr.Site_Name.Truncate(16);
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString(), site_name);
                alert.Summary = string.Empty;
                // Details - 'Part Master Request {0} needs enterprise approval~Site Name: {1}~Created By: {2}~Created On: {3}~Manufacturer: {4}~Manufacturer ID {5}: ~Justification: {6}') --Details
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime c_date = pmr.CreatedDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Create Date = Converted by Retrieval method
                //DateTime c_loc = c_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pmr.PartMasterRequestId
                                               , pmr.Site_Name
                                               , pmr.Requester
                                               , c_date.ToString("g")
                                               , pmr.Manufacturer
                                               , pmr.ManufacturerId
                                               , pmr.Justification);
                alert.Details = details;

            }
            if (alert_type == AlertTypeEnum.PartMasterRequestDenied)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                // Headline - Part Master Request {0} Denied
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - Part Master Request {0} has been denied~Denied By: {1}~Denied On: {2}~Denied Reason: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Need to retrieve the denied log entry 
                ReviewLog RLog = new ReviewLog()
                {
                    ClientId = userdata.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    Function = ReviewLogConstants.Denied,
                    ObjectId = pmr.PartMasterRequestId
                };

                List<ReviewLog> rlog = RLog.Retrieve_LogDetailsByPMRId(this.userdata.DatabaseKey,
                                                                                      userdata.Site.TimeZone).OrderByDescending(x => x.ReviewDate).ToList();
                if (rlog != null && rlog.Count > 0)
                {
                    DateTime appv_date = pmr.ApproveDeny_Date ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                    // Approve/deny date converted by retrieve method
                    //DateTime appv_loc = appv_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                    details = string.Format(details, pmr.PartMasterRequestId
                                                   , pmr.ApproveDenyBy
                                                   , appv_date.ToString("g")
                                                   , rlog[0].Comments);
                    alert.Details = details;
                }


            }
            if (alert_type == AlertTypeEnum.PartMasterRequestReturned)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                // Headline -'Part Master Request {0} Returned'  
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - 'Part Master Request {0} has been returned~Returned By: {1}~Returned On: {2}~Returned Reason: {3}
                string details = setup.Alert_Details;
                ReviewLog RLog = new ReviewLog()
                {
                    ClientId = userdata.DatabaseKey.Client.ClientId,
                    TableName = "PartMasterRequest",
                    Function = ReviewLogConstants.Returned,
                    ObjectId = pmr.PartMasterRequestId
                };

                List<ReviewLog> rlog = RLog.Retrieve_LogDetailsByPMRId(this.userdata.DatabaseKey,
                                                                       userdata.Site.TimeZone).OrderByDescending(x => x.ReviewDate).ToList();

                if (rlog != null && rlog.Count > 0)
                {
                    DateTime r_date = rlog[0].ReviewDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                    // Review date convertd by retrieve method 
                    //DateTime r_loc = r_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                    details = details.Replace("~", "\r\n");
                    details = string.Format(details, pmr.PartMasterRequestId,
                                                     pmr.LastReviewedBy,
                                                     r_date.ToString("g"),
                                                     rlog[0].Comments);
                    alert.Details = details;
                }

            }
            if (alert_type == AlertTypeEnum.PartMasterRequestApproved)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                // Headline -'Part Master Request {0} Enterprise Approved'
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - 'Part Master Request {0} has been Enterprise Approved~Approved By: {1}~Approved On: {2}'
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime appv_date = pmr.ApproveDeny2_Date ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Approval Date is converted to user by the retrieve function
                //DateTime appv_loc = appv_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pmr.PartMasterRequestId
                                               , pmr.ApproveDenyBy2
                                               , appv_date.ToString("g"));
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestSiteApproved)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                // Headline -'Part Master Request {0} Site Approved'
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - 'Part Master Request {0} has been Site Approved~Approved By: {1}~Approved On: {2}'
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime appv_date = pmr.ApproveDeny_Date ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Approval Date is converted to user by the retrieve function
                //DateTime appv_loc = appv_date.ConvertFromUTCToUser(userdata.Site.TimeZone);
                details = string.Format(details, pmr.PartMasterRequestId
                                               , pmr.ApproveDenyBy
                                               , appv_date.ToString("g"));
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.POEmailToVendor)
            {
                PurchaseOrder po = (PurchaseOrder)obj;
                alert.ObjectName = "PurchaseOrder";
                alert.ObjectId = po.PurchaseOrderId;
                // Purchase Order {0} sent to Vendor
                alert.Headline = string.Format(setup.Alert_Headline, po.ClientLookupId);
                alert.Summary = string.Empty;
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // 'Purchase Order {0} sent to Vendor - {1}
                details = string.Format(details, po.ClientLookupId, po.VendorName);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.POImportedReviewRequired)
            {
                PurchaseOrder po = (PurchaseOrder)obj;
                alert.ObjectName = "PurchaseOrder";
                alert.ObjectId = po.PurchaseOrderId;
                // 'Purchase Order {0} Imported – Please Review
                alert.Headline = string.Format(setup.Alert_Headline, po.ClientLookupId.ToString());
                alert.Summary = string.Empty;
                // Purchase Order {0} imported for Vendor {1}~Please Review and Process
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, po.ClientLookupId, po.VendorName);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part Management Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                // Details - Part Management Request {0} was Processed On {1} ~ New Part Master {2} added
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CompleteDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId, CompleteDate.ToString("g"), pmr.PartMaster_ClientLookupId);
                alert.Details = details;
            }
            // SOM-1576
            if (alert_type == AlertTypeEnum.PartMasterRequestAdditionProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part Addition Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part Addition Request {0} was processed on {1}~New Part {2} added to {3}~and is available for usage'
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CreatedDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CreatedDate.ToString("g")
                                               , pmr.Part_ClientLookupId
                                               , pmr.Site_Name);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestReplaceProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part Replace Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part Replace Request {0} was processed on {1}~Part {2} changed to link with part master {3}~in {4}~The Part ID has been changed to {5}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CreatedDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CreatedDate.ToString("g")
                                               , pmr.Part_PrevClientLookupId
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Site_Name
                                               , pmr.Part_ClientLookupId);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestInactivationProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part Inactivation Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part Inactivation Request {0} was processed on {1}~Part {2} in {3} has been inactivated
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CreatedDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CreatedDate.ToString("g")
                                               , pmr.Part_ClientLookupId
                                               , pmr.Site_Name);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestSXReplaceProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part SX Replace Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part SX Replace Request {0} was processed on {1}~Part {2} changed to link with part master {3}~in {4} and is available for usage
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CompleteDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CompleteDate.ToString("g")
                                               , pmr.Part_ClientLookupId
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Site_Name);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestECO_ReplaceProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part ECO Replace Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part ECO Replace Request {0} was processed on {1}~Part Master {2} Added~Part {3} changed to link with part master {4}~in {5} and is available for usage
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CompleteDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CompleteDate.ToString("g")
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Part_ClientLookupId
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Site_Name);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartMasterRequestECO_ReplaceProcessed)
            {
                PartMasterRequest pmr = (PartMasterRequest)obj;
                alert.ObjectName = "PartMasterRequest";
                alert.ObjectId = pmr.PartMasterRequestId;
                // Part ECO SX Replace Request {0} has been processed
                alert.Headline = string.Format(setup.Alert_Headline, pmr.PartMasterRequestId.ToString());
                alert.Summary = string.Empty;
                //Part ECO SX Replace Request {0} was processed on {1}~Part Master {2} Added~Part {3} changed to link with part master {4}~in {5} and is available for usage
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                DateTime CompleteDate = pmr.CompleteDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, pmr.PartMasterRequestId
                                               , CompleteDate.ToString("g")
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Part_ClientLookupId
                                               , pmr.PartMaster_ClientLookupId
                                               , pmr.Site_Name);
                alert.Details = details;
            }

            // Service Order Complete
            if (alert_type == AlertTypeEnum.ServiceOrderComplete)
            {
                ServiceOrder so = (ServiceOrder)obj;
                // Headline - Work Order {0} complete
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "ServiceOrder";
                alert.ObjectId = so.ServiceOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, so.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, so.ClientLookupId
                                               , so.EquipmentClientLookupId
                                               , so.Description.Length > 500 ? so.Description.Substring(0, 500) : so.Description
                                               , so.CompletedByPersonnels);
                alert.Details = details;
            }

            //V2-726
            // Work Request Sent For Approval
            if (alert_type == AlertTypeEnum.WRApprovalRouting)
            {
                WorkOrder wo = (WorkOrder)obj;
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                alert.Summary = string.Empty;
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description);
                alert.Details = details;

            }
            if (alert_type == AlertTypeEnum.KPISubmitted)
            {
                BBUKPI bBUKPI = (BBUKPI)obj;
                alert.ObjectName = "BBUKPI";
                alert.ObjectId = bBUKPI.BBUKPIId;
                alert.Headline = string.Format(setup.Alert_Headline, bBUKPI.Year, bBUKPI.Week, bBUKPI.SiteName);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, bBUKPI.SiteName
                                               , bBUKPI.SubmitBy_Name);

                alert.Details = details;
            }
            // Meter Reading triggered an event
            if (alert_type == AlertTypeEnum.APMMeterEvent)
            {
                IoTEvent ie = (IoTEvent)obj;
                // Headline - APM Meter Event for Asset {0}
                //{0} - IoTEvent.EquipmentClientLookupId
                alert.ObjectName = "IoTEvent";
                alert.ObjectId = ie.IoTEventId;
                alert.Headline = string.Format(setup.Alert_Headline, ie.EquipClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Event: {1}~Asset: {0}~Meter Interval:  {2}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, ie.EquipClientLookupId
                                               , ie.IoTEventId
                                               , ie.IoTDevice_MeterInterval);
                alert.Details = details;

            }
            // Meter Reading triggered a warning event
            if (alert_type == AlertTypeEnum.APMWarningEvent)
            {
                IoTEvent ie = (IoTEvent)obj;
                // Headline - APM Warning Event for Asset {0}
                //{0} - IoTEvent.EquipmentClientLookupId
                alert.ObjectName = "IoTEvent";
                alert.ObjectId = ie.IoTEventId;
                alert.Headline = string.Format(setup.Alert_Headline, ie.EquipClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Event: {1}~Asset: {0}~Reading: {2}~Trigger High: {3}~Trigger Low: {4}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, ie.EquipClientLookupId,
                                                 ie.IoTEventId
                                               , ie.IoTReading_Reading.ToString() + " " + ie.IoTDevice_SensorUnit
                                               , ie.IoTDevice_TriggerHigh
                                               , ie.IoTDevice_TriggerLow);
                alert.Details = details;

            }
            // Meter Reading triggered a critical event
            if (alert_type == AlertTypeEnum.APMCriticalEvent)
            {
                IoTEvent ie = (IoTEvent)obj;
                // Headline - APM Critical Event for Asset {0}
                //{0} - IoTEvent.EquipmentClientLookupId
                alert.ObjectName = "IoTEvent";
                alert.ObjectId = ie.IoTEventId;
                alert.Headline = string.Format(setup.Alert_Headline, ie.EquipClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Event: {1}~Asset: {0}~Reading: {2}~Trigger High: {3}~Trigger Low: {4}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, ie.EquipClientLookupId
                                               , ie.IoTEventId
                                               , ie.IoTReading_Reading.ToString() + " " + ie.IoTDevice_SensorUnit
                                               , ie.IoTDevice_TriggerHighCrit
                                               , ie.IoTDevice_TriggerLowCrit);
                alert.Details = details;

            }
            alert.Create(userdata.DatabaseKey);
            return alert;
        }

        protected Alerts AlertCreate(Object obj, Notes note, AlertTypeEnum alert_type)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;

            // WorkOrderCommentMention
            if (alert_type == AlertTypeEnum.WorkOrderCommentMention)
            {
                string FirstName = userdata.DatabaseKey.User.FirstName == null ? "" : userdata.DatabaseKey.User.FirstName;
                string LastName = userdata.DatabaseKey.User.LastName == null ? "" : userdata.DatabaseKey.User.LastName;
                string SentName = FirstName + " " + LastName;
                DateTime today = DateTime.Today; // As DateTime
                string SendDate = today.ToString("MM/dd/yyyy");
                string SiteName = userdata.Site.Name;
                WorkOrder wo = (WorkOrder)obj;
                Notes Nt = (Notes)note;
                // Headline - Work Request {0} had been approved
                //{0} - Workorder.clientlookupid
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                // Summary - not used 
                //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                alert.Summary = string.Empty;
                // Details -         Work Request: {0}~Requestor: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}~Approved by: {6}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                                ,SentName
                                               , SendDate
                                               , SiteName
                                               );
                alert.Details = details;

            }

            alert.Create(userdata.DatabaseKey);
            return alert;
        }
        protected Alerts AlertCreate(Object obj, Notes note, AlertTypeEnum alert_type, string UserName)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = UserName; //userdata.DatabaseKey.UserName;
            //string FirstName = userdata.DatabaseKey.User.FirstName;
            //string LastName = userdata.DatabaseKey.User.LastName;
            //DateTime today = DateTime.Today; // As DateTime
            //string s_today = today.ToString("MM/dd/yyyy"); // As String
            //string SiteName = userdata.Site.Name;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;

            // ServiceOrderCommentMention
            if (alert_type == AlertTypeEnum.ServiceOrderCommentMention)
            {
                ServiceOrder serviceOrder = (ServiceOrder)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = serviceOrder.ServiceOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, serviceOrder.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, serviceOrder.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                               );
                alert.Details = details;

            }

            // WorkOrderCommentMention
            if (alert_type == AlertTypeEnum.WorkOrderCommentMention)
            {
                string FirstName = userdata.DatabaseKey.User.FirstName == null ? "" : userdata.DatabaseKey.User.FirstName;
                string LastName = userdata.DatabaseKey.User.LastName == null ? "" : userdata.DatabaseKey.User.LastName;
                string SentName = FirstName + " " + LastName;
                DateTime today = DateTime.Today; // As DateTime
                string SendDate = today.ToString("MM/dd/yyyy");
                string SiteName = userdata.Site.Name;
                WorkOrder wo = (WorkOrder)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                                ,SentName
                                               , SendDate
                                               , SiteName
                                               );
                alert.Details = details;

            }
            // AssetCommentMentionNotificationMaintenancetab
            if (alert_type == AlertTypeEnum.AssetCommentMention)
            {
                string FirstName = userdata.DatabaseKey.User.FirstName==null?"": userdata.DatabaseKey.User.FirstName;
                string LastName = userdata.DatabaseKey.User.LastName == null ? "" : userdata.DatabaseKey.User.LastName;
                string SentName = FirstName+ " "+ LastName;               
                DateTime today = DateTime.Today; // As DateTime
                string SendDate = today.ToString("MM/dd/yyyy"); 
                string SiteName = userdata.Site.Name;
                Equipment eq = (Equipment)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "Equipment";
                alert.ObjectId = eq.EquipmentId;
                alert.Headline = string.Format(setup.Alert_Headline, eq.ClientLookupId);



                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, eq.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                               , SentName
                                               , SendDate
                                               , SiteName
                                               );
                alert.Details = details;

            }
            // PartCommentMention
            if (alert_type == AlertTypeEnum.PartCommentMention)
            {
                string FirstName = userdata.DatabaseKey.User.FirstName == null ? "" : userdata.DatabaseKey.User.FirstName;
                string LastName = userdata.DatabaseKey.User.LastName == null ? "" : userdata.DatabaseKey.User.LastName;
                string SentName = FirstName + " " + LastName;
                DateTime today = DateTime.Today; // As DateTime
                string SendDate = today.ToString("MM/dd/yyyy");
                string SiteName = userdata.Site.Name;
                Part part = (Part)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "Part";
                alert.ObjectId = part.PartId;
                alert.Headline = string.Format(setup.Alert_Headline, part.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, part.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                                ,SentName
                                               , SendDate
                                               , SiteName
                                               );
                alert.Details = details;

            }
            // WOPCommentMention
            if (alert_type == AlertTypeEnum.WOPlanCommentMention)
            {
                WorkOrderPlan wop = (WorkOrderPlan)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "WorkOrderPlan";
                alert.ObjectId = wop.WorkOrderPlanId;
                alert.Headline = string.Format(setup.Alert_Headline, wop.WorkOrderPlanId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wop.WorkOrderPlanId
                                               , Nt.OwnerName
                                               , Nt.Content
                                               );
                alert.Details = details;

            }
            // ProjectCommentMention
            if (alert_type == AlertTypeEnum.ProjectCommentMention)
            {
                string FirstName = userdata.DatabaseKey.User.FirstName == null ? "" : userdata.DatabaseKey.User.FirstName;
                string LastName = userdata.DatabaseKey.User.LastName == null ? "" : userdata.DatabaseKey.User.LastName;
                string SentName = FirstName + " " + LastName;
                DateTime today = DateTime.Today; // As DateTime
                string SendDate = today.ToString("MM/dd/yyyy");
                string SiteName = userdata.Site.Name;
                Project project = (Project)obj;
                Notes Nt = (Notes)note;
                alert.ObjectName = "Project";
                alert.ObjectId = project.ProjectId;
                alert.Headline = string.Format(setup.Alert_Headline, project.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, project.ClientLookupId
                                               , Nt.OwnerName
                                               , Nt.Content
                                                ,SentName
                                               , SendDate
                                               , SiteName
                                               );
                alert.Details = details;

            }
            alert.Create(userdata.DatabaseKey);
            return alert;
        }

        protected Alerts AlertCreate(Object obj, AlertTypeEnum alert_type, string UserName)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = UserName; //userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;

            // WorkOrderAssign
            if (alert_type == AlertTypeEnum.WorkOrderAssign)
            {
               string AssetGroup1Name = String.IsNullOrEmpty(this.userdata.Site.AssetGroup1Name) ? "Asset Group 1" : this.userdata.Site.AssetGroup1Name;
               string AssetGroup2Name = String.IsNullOrEmpty(this.userdata.Site.AssetGroup2Name) ? "Asset Group 2" : this.userdata.Site.AssetGroup2Name;
                WorkOrder wo = (WorkOrder)obj;
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                //details = string.Format(details, wo.ClientLookupId);             
                details = string.Format(details, wo.ClientLookupId
                                              , wo.Description
                                              , wo.ChargeToClientLookupId, AssetGroup1Name
                                              , wo.AssetGroup1Description, AssetGroup2Name, wo.AssetGroup2Description);
                alert.Details = details;              

            }

            //Service Order
            if (alert_type == AlertTypeEnum.ServiceOrderAssign)
            {
                ServiceOrder So = (ServiceOrder)obj;
                alert.ObjectName = "ServiceOrder";
                alert.ObjectId = So.ServiceOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, So.ClientLookupId);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, So.ClientLookupId);
                alert.Details = details;

            }
            // WorkOrderApprovalNeeded and WRApprovalRouting
            if (alert_type == AlertTypeEnum.WorkOrderApprovalNeeded || alert_type == AlertTypeEnum.WRApprovalRouting /*V2-726*/)
            {
                WorkOrder wo = (WorkOrder)obj;
                alert.ObjectName = "WorkOrder";
                alert.ObjectId = wo.WorkOrderId;
                alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                string details = setup.Alert_Details;
                // Details - Work Order: {0}~Requestor: {1}~Charge Type: {2}~Charge To:  {3}~Charge To Name: {4}~Description: {5}
                details = details.Replace("~", "\r\n");
                details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.ChargeType
                                               , wo.ChargeToClientLookupId
                                               , wo.ChargeTo_Name
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description);
            
                alert.Details = details;

            }

            // PurchaseRequestApprovalNeeded
            if (alert_type == AlertTypeEnum.PurchaseRequestApprovalNeeded)
            {
                PurchaseRequest pr = (PurchaseRequest)obj;
                // Headline - Purchase Request {0} needs approval
                //{0} - PurchaseRequest.ClientLookupId
                alert.ObjectName = "PurchaseRequest";
                alert.ObjectId = pr.PurchaseRequestId;
                alert.Headline = string.Format(setup.Alert_Headline, pr.ClientLookupId);
                // Summary - not used 
                alert.Summary = string.Empty;
                // Details - Purchase Request: {0}~Requester: {1}~Reason: {2}~Comments: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, pr.ClientLookupId
                                               , pr.Creator_PersonnelName
                                               , pr.Reason
                                               , pr.Process_Comments);
                alert.Details = details;

            }

            #region V2- 823 BBUKPI
            //BBUKPI
            if (alert_type == AlertTypeEnum.KPIReOpened)
            {
                BBUKPI So = (BBUKPI)obj;
                alert.ObjectName = "BBUKPI";
                alert.ObjectId = So.BBUKPIId;
                alert.Headline = setup.Alert_Headline;
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, So.Year
                                               , So.Week);
                alert.Details = details;
            }
            #endregion

            alert.Create(userdata.DatabaseKey);
            return alert;
        }

        protected Alerts AlertCreate(PartTransfer ptxfr, PartTransferEventLog ev, AlertTypeEnum alert_type)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;
            if (alert_type == AlertTypeEnum.PartTransferSend)
            {
                // This should probably have 
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Part Transfer Request {0} Sent
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Part Transfer Request {0} created and sent to {1}.~Created By: {2}~From: {3}~Created On: {4}~{5} Part ID: {6}~{7} Part Description: {8}~{9} Part ID: {10}~{11} Description: {12} ~Date Required: {13}~Reason: {14}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Create date is converted by the data contract to the site of the logged in user 
                // In this case this will be the Request Site
                // This notification is going to the Issue Site
                // We need to convert back to utc then convert to issue site time zone
                // HOWEVER - we will do this later
                // For now we will leave as converted to the logged in user's time zone
                // DateTime CreateDate = ptxfr.CreateDate.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Rquired date is nullable - make a non-nullable data  entered by the user - no need to convert
                DateTime Required = ptxfr.RequiredDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ptxfr.IssueSite_Name
                                               , ptxfr.CreateBy_PersonnelName
                                               , ptxfr.RequestSite_Name
                                               , ptxfr.CreateDate.ToString("g")
                                               , ptxfr.RequestSite_Name
                                               , ptxfr.RequestPart_ClientLookupId
                                               , ptxfr.RequestSite_Name
                                               , ptxfr.RequestPart_Description
                                               , ptxfr.IssueSite_Name
                                               , ptxfr.IssuePart_ClientLookupId
                                               , ptxfr.IssueSite_Name
                                               , ptxfr.IssuePart_Description
                                               , Required.ToString("d")
                                               , ptxfr.Reason);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartTransferIssue)
            {
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Parts Issued to Part Transfer Request {0} 
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Parts Issued to Part Transfer Request {0}~Issue Quantity: {1}~Issued By: {2}~Issue Date: {3}~Part ID: {4}~Part Description: {5}~Comments: {6}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Issue date is converted to the logged in user's timezone 
                // If it is null then use current date/time
                // The parttransfer.retrieveforalert should retrieve this 
                DateTime IssueDate = ev.TransactionDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ev.Quantity
                                               , ev.NameFull
                                               , IssueDate.ToString("g")
                                               , ptxfr.RequestPart_ClientLookupId
                                               , ptxfr.RequestPart_Description
                                               , ev.Comments);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartTransferReceipt)
            {
                // This should probably have 
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Parts Received for Part Transfer Request {0} 
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Parts Received for a Part Transfer {0}~Receipt Quantity: {1}~Received By: {2}~Receipt Date: {3}~Part ID: {4}~Part Description: {5}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Create date needs to be changed based on the target site's time zond
                // The parttransfer.retrieveforalert should retrieve this 
                DateTime ReceiptDate = ev.TransactionDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ev.Quantity
                                               , ev.NameFull
                                               , ReceiptDate.ToString("g")
                                               , ptxfr.RequestPart_ClientLookupId
                                               , ptxfr.RequestPart_Description);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartTransferDenied)
            {
                // This should probably have 
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Part transfer request {0} has been denied
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Part Transfer {0} Denied~Denied By: {1}~Denied Date: {2}~Reason: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Create date needs to be changed based on the target site's time zond
                // The parttransfer.retrieveforalert should retrieve this 
                DateTime DeniedDate = ev.TransactionDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ev.NameFull
                                               , DeniedDate.ToString("g")
                                               , ev.Comments);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartTransferForceCompPend)
            {
                // This should probably have 
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Part transfer request {0} is pending force complete
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Part Transfer {0} is pending a force complete~Force Complete Initiated By: {1}~Initiated Date: {2}~Comments {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Create date needs to be changed based on the target site's time zond
                // The parttransfer.retrieveforalert should retrieve this 
                DateTime FCDate = ev.TransactionDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ev.NameFull
                                               , FCDate.ToString("g")
                                               , ev.Comments);
                alert.Details = details;
            }
            if (alert_type == AlertTypeEnum.PartTransferCanceled)
            {
                // This should probably have 
                alert.ObjectName = "PartTransfer";
                alert.ObjectId = ptxfr.PartTransferId;
                // Part transfer request {0} has been canceled
                alert.Headline = string.Format(setup.Alert_Headline, ptxfr.PartTransferId.ToString());
                alert.Summary = string.Empty;
                //Part Transfer {0} Canceled~Canceled By: {1}~Canceled Date: {3}
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                // Create date needs to be changed based on the target site's time zond
                // The parttransfer.retrieveforalert should retrieve this 
                DateTime CanDate = ev.TransactionDate ?? DateTime.UtcNow.ConvertFromUTCToUser(userdata.Site.TimeZone);
                // Created Date is converted to user by the retrieve function
                details = string.Format(details, ptxfr.PartTransferId
                                               , ev.FullName
                                               , CanDate.ToString("g")
                                               , ev.Comments);
                alert.Details = details;
            }
            //
            // Create the alert
            // 
            alert.Create(userdata.DatabaseKey);
            return alert;

        }
        protected void AlertSendToTarget(Alerts alert)
        {
            try
            {

                // Created the alert - now send it to everyone in the target list
                foreach (AlertTarget at in AlertTargetList)
                {
                    AlertUser alertuser = new AlertUser();
                    alertuser.ClientId = userdata.DatabaseKey.Client.ClientId;
                    alertuser.UserId = at.UserInfoId;
                    alertuser.AlertsId = alert.AlertsId;
                    alertuser.ActiveDate = DateTime.UtcNow;
                    alertuser.Permission = "";
                    alertuser.IsRead = false;
                    alertuser.IsDeleted = false;
                    alertuser.Create(userdata.DatabaseKey);
                    SendNotification(at.UserInfoId, userdata, alert.Headline);

                    // SOM-652 - Email 
                    // Email if setup 
                    if (setup.EmailSend && !string.IsNullOrEmpty(at.email_address))
                    {
                        // SOM-1199 
                        // Email formatting.  Need to change the \r\n with Char(10)+Char(13)
                        string details = alert.Details.Replace("\r\n", "<br/>");
                        //EmailModule emailModule = new EmailModule();
                        Presentation.Common.EmailModule emailModule = new Presentation.Common.EmailModule();
                        emailModule.ToEmailAddress = at.email_address;
                        //emailModule.MailSubject = alert.Headline;
                        emailModule.Subject = alert.Headline;
                        emailModule.MailBody = details;
                        bool IsSent = emailModule.SendEmail();
                    }
                }
                if (UserInfoTargetList != null && UserInfoTargetList.Count > 0)
                {
                    foreach (AlertTarget at in UserInfoTargetList)
                    {
                        string user = string.Empty;
                        user = string.IsNullOrEmpty(at.CallerUserName) ? at.UserName : at.CallerUserName;
                        //NotificationHub.Hubs.NotificationHub objNotifHub = new NotificationHub.Hubs.NotificationHub();
                        //objNotifHub.SendNotification(user, alert.Headline);
                        var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub.Hubs.NotificationHub>();
                        context.Clients.Group(user).broadcaastNotif(alert.Headline);
                    }
                }
                return;
            }
            catch (Exception ex)
            { throw ex; }
        }


        protected void AlertSendToCustomTarget(Alerts alert, Tuple<long, string, string> UserInfo)
        {
            try
            {
                // Created the alert - now send it to everyone in the target list

                AlertUser alertuser = new AlertUser();
                alertuser.ClientId = userdata.DatabaseKey.Client.ClientId;
                alertuser.UserId = UserInfo.Item1;//at.UserInfoId;
                alertuser.AlertsId = alert.AlertsId;
                alertuser.ActiveDate = DateTime.UtcNow;
                alertuser.Permission = "";
                alertuser.IsRead = false;
                alertuser.IsDeleted = false;
                alertuser.Create(userdata.DatabaseKey);
                // SendNotification(at.UserInfoId, userdata, alert.Headline);
                SendNotification(UserInfo.Item1, userdata, alert.Headline);
                // SOM-652 - Email 
                // Email if setup 
                // if (setup.EmailSend && !string.IsNullOrEmpty(at.email_address))
                if (setup.EmailSend && !string.IsNullOrEmpty(UserInfo.Item3))
                {
                    // SOM-1199 
                    // Email formatting.  Need to change the \r\n with Char(10)+Char(13)
                    string details = alert.Details.Replace("\r\n", "<br/>");

                    //EmailModule emailModule = new EmailModule();
                    Presentation.Common.EmailModule emailModule = new Presentation.Common.EmailModule();
                    //  emailModule.ToEmailAddress = at.email_address;
                    emailModule.ToEmailAddress = UserInfo.Item3;
                    //emailModule.MailSubject = alert.Headline;
                    emailModule.Subject = alert.Headline;
                    emailModule.MailBody = details;
                    bool IsSent = emailModule.SendEmail();
                }

                string user = string.Empty;
                //  user = string.IsNullOrEmpty(at.CallerUserName) ? at.UserName : at.CallerUserName;
                user = string.IsNullOrEmpty(UserInfo.Item2) ? "" : UserInfo.Item2;
                //NotificationHub.Hubs.NotificationHub objNotifHub = new NotificationHub.Hubs.NotificationHub();
                //objNotifHub.SendNotification(user, alert.Headline);
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub.Hubs.NotificationHub>();
                if(!string.IsNullOrEmpty(user))
                {
                    context.Clients.Group(user).broadcaastNotif(alert.Headline);
                }
                

                return;
            }
            catch (Exception ex)
            { throw ex; }
        }


        #region old code 2
        public void CreateAlert<T>(UserData userdata, Object obj, AlertTypeEnum alertType) where T : class
        {
            DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder();
            DataContracts.Equipment equipment = new DataContracts.Equipment();
            DataContracts.PurchaseRequest purchaseRequest = new DataContracts.PurchaseRequest();

            Type type = typeof(T);
            if (type == typeof(DataContracts.WorkOrder))
            {
                workOrder = obj as DataContracts.WorkOrder;
            }
            if (type == typeof(DataContracts.Equipment))
            {
                equipment = obj as DataContracts.Equipment;
            }
            if (type == typeof(DataContracts.PurchaseRequest))
            {
                purchaseRequest = obj as DataContracts.PurchaseRequest;
            }
            //DataContracts.WorkOrder workorder = new DataContracts.WorkOrder();
            //ProcessAlert objAlert = new ProcessAlert(this.UserData);
            //objAlert.CreateAlert<DataContracts.WorkOrder>(this.UserData, workorder);

            AlertDefine alertDefine = new AlertDefine();
            AlertLocal alertlocal = new AlertLocal();
            AlertSetup alertSetup = new AlertSetup();
            AlertTarget alertTarget = new AlertTarget();

            Alerts alerts = new Alerts();
            AlertUser alertuser = new AlertUser();



            List<AlertDefine> alertDefineList = new List<AlertDefine>();
            List<AlertLocal> alertLocalList = new List<AlertLocal>();
            List<AlertSetup> alertSetUpList = new List<AlertSetup>();
            List<AlertTarget> AlertTargetList = new List<AlertTarget>();
            List<AlertUser> alertuserList = new List<AlertUser>();

            alertDefineList = alertDefine.RetrieveAll(userdata.DatabaseKey);
            if (alertDefineList.Count > 0)
            {
                string AlertType = alertType.ToString();
                //alertDefine = alertDefineList.SingleOrDefault(x => x.Name.ToLower() == "WorkRequestAdd".ToLower());
                alertDefine = alertDefineList.FirstOrDefault(x => x.Name.ToLower() == AlertType.ToLower());
                if (alertDefine != null)                                //SOM - 857   if (alertDefine.AlertDefineId > 0)
                {
                    alertSetUpList = alertSetup.RetrieveAll(userdata.DatabaseKey);
                    alertSetUpList = alertSetUpList.Where(x => x.AlertDefineId == alertDefine.AlertDefineId && x.IsActive == true).ToList();
                    if (alertSetUpList.Count > 0)
                    {
                        //Retrieve corresponding alertLocals and alertTargetList
                        alertLocalList = alertlocal.RetrieveAll(userdata.DatabaseKey);
                        alertLocalList = alertLocalList.Where(x => x.AlertDefineId == alertDefine.AlertDefineId).ToList();

                        AlertTargetList = alertTarget.RetrieveAll(userdata.DatabaseKey);
                        AlertTargetList = (from t in AlertTargetList
                                           join s in alertSetUpList
                                               on t.AlertSetupId equals s.AlertSetupId
                                           select t).ToList();


                        alertlocal = alertLocalList.FirstOrDefault(x => x.Localization.ToLower() == (userdata.Site.LocalizationLanguage + "-" + userdata.Site.LocalizationCulture).ToLower());
                        AlertLocal al = alertLocalList.FirstOrDefault();
                        if (alertlocal.AlertLocalId > 0)
                        {

                            if (type == typeof(DataContracts.WorkOrder))
                            {
                                AlertCreateForWorkOrderObject(alertType, userdata, workOrder, alerts, alertDefine, alertlocal);
                            }
                            else if (type == typeof(DataContracts.PurchaseRequest))
                            {
                                AlertCreateForPurchaseRequestObject(userdata, purchaseRequest, alerts, alertDefine, alertlocal);
                            }



                            foreach (AlertTarget at in AlertTargetList)
                            {
                                alertuser.ClientId = userdata.DatabaseKey.Client.ClientId;//workOrder.ClientId;
                                alertuser.AlertUserId = 0;
                                alertuser.UserId = at.UserInfoId;
                                alertuser.AlertsId = alerts.AlertsId;
                                alertuser.ActiveDate = DateTime.UtcNow;
                                alertuser.Permission = "";
                                alertuser.IsRead = false;
                                alertuser.IsDeleted = false;
                                alertuser.Create(userdata.DatabaseKey);
                                SendNotification(at.UserInfoId, userdata, alerts.Headline);
                            }
                        }
                    }
                }
            }
        }

        protected void AlertCreateForWorkOrderObject(AlertTypeEnum AlertType, UserData userdata, DataContracts.WorkOrder workOrder, DataContracts.Alerts alerts,
                DataContracts.AlertDefine alertDefine, DataContracts.AlertLocal alertlocal)
        {

            workOrder.ClientId = userdata.DatabaseKey.Client.ClientId;
            workOrder.WorkOrderId = workOrder.WorkOrderId;
            workOrder.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);

            //Create the alert
            alerts.ClientId = workOrder.ClientId;
            alerts.AlertDefineId = alertDefine.AlertDefineId;
            // Have different formats based on the alert 
            string details;
            switch (AlertType)
            {
                case AlertTypeEnum.WorkRequestApprovalNeeded:
                    // Headline - Work Request {0} had been added 
                    //{0} - Workorder.clientlookupid
                    alerts.Headline = string.Format(alertlocal.Headline, workOrder.ClientLookupId);
                    // Summary - not used 
                    //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                    alerts.Summary = string.Empty;
                    // Details - Work Request: {0}\nRequestor: {1}\nCharge Type: {2}\nCharge To:  {3}\nCharge To Name: {4}\nDescription: {5}
                    details = alertlocal.Details;
                    details = details.Replace("~", "<br>");
                    details = string.Format(details, workOrder.ClientLookupId
                                                    , workOrder.Createby
                                                    , workOrder.ChargeType
                                                    , workOrder.ChargeToClientLookupId
                                                    , workOrder.ChargeTo_Name
                                                    , workOrder.Description.Length > 500 ? workOrder.Description.Substring(0, 500) : workOrder.Description
                                                                        );
                    alerts.Details = details;
                    break;
                default:
                    alerts.Headline = string.Format(alertlocal.Headline, workOrder.ClientLookupId);
                    //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
                    alerts.Summary = string.Empty;
                    details = alertlocal.Details;
                    details = details.Replace("~", "<br>");
                    details = string.Format(alertlocal.Details, workOrder.ClientLookupId
                                                                      , workOrder.Createby
                                                                      , workOrder.ChargeType
                                                                      , workOrder.ChargeToClientLookupId
                                                                      , workOrder.ChargeTo_Name
                                                                      , workOrder.Description.Length > 500 ? workOrder.Description.Substring(0, 500) : workOrder.Description
                                                                        );
                    alerts.Details = details;
                    break;
            }
            alerts.From = userdata.DatabaseKey.UserName;
            alerts.AlertType = alertDefine.Type;
            alerts.ObjectId = workOrder.WorkOrderId;
            alerts.ObjectName = "WorkOrder";
            alerts.IsCleared = false;
            alerts.Create(userdata.DatabaseKey);
        }

        protected void AlertCreateForServiceOrderObject(AlertTypeEnum AlertType, UserData userdata, DataContracts.ServiceOrder serviceOrder, DataContracts.Alerts alerts,
                DataContracts.AlertDefine alertDefine, DataContracts.AlertLocal alertlocal)
        {

            serviceOrder.ClientId = userdata.DatabaseKey.Client.ClientId;
            serviceOrder.ServiceOrderId = serviceOrder.ServiceOrderId;
            serviceOrder.Retrieve(userdata.DatabaseKey);

            //Create the alert
            alerts.ClientId = serviceOrder.ClientId;
            alerts.AlertDefineId = alertDefine.AlertDefineId; 
            string details;

            alerts.Headline = string.Format(alertlocal.Headline, serviceOrder.ClientLookupId);
            //alerts.Summary = string.Format(alertlocal.Summary, workOrder.Description.Length > 120 ? workOrder.Description.Substring(0, 120) : workOrder.Description);
            alerts.Summary = string.Empty;
            details = alertlocal.Details;
            details = details.Replace("~", "<br>");
            details = string.Format(alertlocal.Details, serviceOrder.ClientLookupId
                                                              , serviceOrder.CreateBy
                                                              , ""//serviceOrder.ChargeType
                                                              , ""//serviceOrder.ChargeToClientLookupId
                                                              , ""//serviceOrder.ChargeTo_Name
                                                              , serviceOrder.Description.Length > 500 ? serviceOrder.Description.Substring(0, 500) : serviceOrder.Description
                                                                );
            alerts.Details = details;

            alerts.From = userdata.DatabaseKey.UserName;
            alerts.AlertType = alertDefine.Type;
            alerts.ObjectId = serviceOrder.ServiceOrderId;
            alerts.ObjectName = "ServiceOrder";
            alerts.IsCleared = false;
            alerts.Create(userdata.DatabaseKey);
        }

        public void CreateAlert<T>(UserData userdata, Object obj, AlertTypeEnum alertType, long ScheduledAssignedUserId, List<object> lstOfId) where T : class
        {
            DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder();
            DataContracts.Equipment equipment = new DataContracts.Equipment();
            DataContracts.PurchaseRequest purchaseRequest = new DataContracts.PurchaseRequest();
            DataContracts.ServiceOrder serviceOrder = new DataContracts.ServiceOrder();
            Type type = typeof(T);
            if (type == typeof(DataContracts.WorkOrder))
            {
                workOrder = obj as DataContracts.WorkOrder;
            }
            if (type == typeof(DataContracts.Equipment))
            {
                equipment = obj as DataContracts.Equipment;
            }
            if (type == typeof(DataContracts.PurchaseRequest))
            {
                purchaseRequest = obj as DataContracts.PurchaseRequest;
            }
            if (type == typeof(DataContracts.ServiceOrder))
            {
                serviceOrder = obj as DataContracts.ServiceOrder;
            }
            //DataContracts.WorkOrder workorder = new DataContracts.WorkOrder();
            //ProcessAlert objAlert = new ProcessAlert(this.UserData);
            //objAlert.CreateAlert<DataContracts.WorkOrder>(this.UserData, workorder);

            AlertDefine alertDefine = new AlertDefine();
            AlertLocal alertlocal = new AlertLocal();
            AlertSetup alertSetup = new AlertSetup();
            AlertTarget alertTarget = new AlertTarget();

            Alerts alerts = new Alerts();
            AlertUser alertuser = new AlertUser();



            List<AlertDefine> alertDefineList = new List<AlertDefine>();
            List<AlertLocal> alertLocalList = new List<AlertLocal>();
            List<AlertSetup> alertSetUpList = new List<AlertSetup>();
            List<AlertTarget> AlertTargetList = new List<AlertTarget>();
            List<AlertUser> alertuserList = new List<AlertUser>();

            alertDefineList = alertDefine.RetrieveAll(userdata.DatabaseKey);
            if (alertDefineList.Count > 0)
            {
                string AlertType = alertType.ToString();
                //alertDefine = alertDefineList.SingleOrDefault(x => x.Name.ToLower() == "WorkRequestAdd".ToLower());
                alertDefine = alertDefineList.FirstOrDefault(x => x.Name.ToLower() == AlertType.ToLower());
                if (alertDefine != null)                               //  SOMAX-857  if (alertDefine.AlertDefineId != 0)
                {
                    alertSetUpList = alertSetup.RetrieveAll(userdata.DatabaseKey);
                    alertSetUpList = alertSetUpList.Where(x => x.AlertDefineId == alertDefine.AlertDefineId && x.IsActive == true).ToList();
                    if (alertSetUpList.Count > 0)
                    {
                        //Retrieve corresponding alertLocals and alertTargetList
                        alertLocalList = alertlocal.RetrieveAll(userdata.DatabaseKey);
                        alertLocalList = alertLocalList.Where(x => x.AlertDefineId == alertDefine.AlertDefineId).ToList();

                        //AlertTargetList = alertTarget.RetrieveAll(userdata.DatabaseKey);
                        //AlertTargetList = (from t in AlertTargetList
                        //         join s in alertSetUpList
                        //            on t.AlertSetupId equals s.AlertSetupId
                        //        select t).ToList();


                        alertlocal = alertLocalList.FirstOrDefault(x => x.Localization.ToLower() == (userdata.Site.LocalizationLanguage + "-" + userdata.Site.LocalizationCulture).ToLower());
                        AlertLocal al = alertLocalList.FirstOrDefault();

                        List<long> tg = new List<long>();
                        tg.Add(ScheduledAssignedUserId);

                        setup = RetrieveAlertInfo(alertType);

                        if (setup.Alert_Active)
                        {
                            // Determine the target for the Alert
                            SetTargetList(tg);
                            if (alertlocal.AlertLocalId > 0)
                            {
                                foreach (object objlstOfId in lstOfId)
                                {
                                    //workOrder.WorkOrderId = Convert.ToInt32(objLstWorkOrderId);
                                    //AlertCreateForWorkOrderObject(userdata, workOrder, alerts, alertDefine, alertlocal);

                                    if (this.AlertTargetList.Count > 0)
                                    {
                                        if (type == typeof(DataContracts.WorkOrder))
                                        {
                                            workOrder.WorkOrderId = Convert.ToInt32(objlstOfId);
                                            AlertCreateForWorkOrderObject(alertType, userdata, workOrder, alerts, alertDefine, alertlocal);

                                        }
                                        else if (type == typeof(DataContracts.PurchaseRequest))
                                        {
                                            purchaseRequest.PurchaseRequestId = Convert.ToInt32(objlstOfId);
                                            AlertCreateForPurchaseRequestObject(userdata, purchaseRequest, alerts, alertDefine, alertlocal);
                                        }
                                        else if (type == typeof(DataContracts.ServiceOrder))
                                        {
                                            serviceOrder.ServiceOrderId = Convert.ToInt32(objlstOfId);
                                            AlertCreateForServiceOrderObject(alertType, userdata, serviceOrder, alerts, alertDefine, alertlocal);
                                        }
                                        if (alerts.AlertsId > 0)
                                        {
                                            // alert is created
                                            // send to the targets
                                            AlertSendToTarget(alerts);
                                        }
                                    }


                                    //alertuser.ClientId = userdata.DatabaseKey.Client.ClientId;
                                    //alertuser.UserId = ScheduledAssignedUserId;
                                    //alertuser.AlertsId = alerts.AlertsId;
                                    //alertuser.ActiveDate = DateTime.UtcNow;
                                    //alertuser.Permission = "";
                                    //alertuser.IsRead = false;
                                    //alertuser.IsDeleted = false;
                                    //alertuser.Create(userdata.DatabaseKey);
                                    //SendNotification(ScheduledAssignedUserId, userdata, alerts.Headline);

                                    alerts.AlertsId = 0;
                                    alertuser.AlertUserId = 0;
                                }
                            }


                        }
                    }
                }
            }

        }


        protected void AlertCreateForPurchaseRequestObject(UserData userdata, DataContracts.PurchaseRequest purchaseRequest, DataContracts.Alerts alerts,
           DataContracts.AlertDefine alertDefine, DataContracts.AlertLocal alertlocal)
        {

            purchaseRequest.ClientId = userdata.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = purchaseRequest.PurchaseRequestId;
            purchaseRequest.RetrieveByPKForeignKeys(userdata.DatabaseKey, userdata.Site.TimeZone);

            //Create the alert
            alerts.ClientId = purchaseRequest.ClientId;
            alerts.AlertDefineId = alertDefine.AlertDefineId;
            alerts.Headline = string.Format(alertlocal.Headline, purchaseRequest.ClientLookupId);
            alerts.Summary = string.Format(alertlocal.Summary, purchaseRequest.Reason);
            alerts.Details = string.Format(alertlocal.Details
                                                                , purchaseRequest.ClientLookupId
                                                                , purchaseRequest.Creator_PersonnelName
                                                                , "" //ChargeType
                                                                , "" //ChargeToClientLookupId
                                                                , "" //ChargeTo_Name
                                                                , purchaseRequest.Reason //purchaseRequest.Description.Length > 500 ? purchaseRequest.Description.Substring(0, 500) : purchaseRequest.Description
                                                                );
            alerts.From = userdata.DatabaseKey.UserName;
            alerts.AlertType = alertDefine.Type;
            alerts.ObjectId = purchaseRequest.PurchaseRequestId;
            alerts.ObjectName = "PurchaseRequest";
            alerts.IsCleared = false;

            alerts.Create(userdata.DatabaseKey);



        }

        #endregion old code 2

        protected void SendNotification(long UserId, UserData userdata, string Notification_alert)
        {
            // The UserId is the PersonnelId found in the target list
            DataTable dt = new DataTable();
            Mobile objMobile = new Mobile();
            ApplicationCommon objAppCommon = new ApplicationCommon();
            dt = objMobile.GetLoginRegDetails(UserId, userdata.DatabaseKey.ClientConnectionString);


            SenderId = ConfigurationManager.AppSettings[WebConfigConstants.SENDERID];
            FCM_SenderId = ConfigurationManager.AppSettings[WebConfigConstants.fcm_SendorId];

            AndroidRegKey = ConfigurationManager.AppSettings[WebConfigConstants.ANDROIDREGKEY];
            AndroidRegKey2 = ConfigurationManager.AppSettings[WebConfigConstants.ANDROIDREGKEY2];
            FCMAndroidRegKey = ConfigurationManager.AppSettings[WebConfigConstants.fcm_ServerKey];


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        string DeviceType = dt.Rows[i]["DeviceType"].ToString();
                        string RegistrationKey = dt.Rows[i]["RegistrationKey"].ToString();
                        int badgeCount = Convert.ToInt32(dt.Rows[i]["Badge"].ToString());
                        string iBadge = badgeCount.ToString();

                        if (DeviceType == "1")
                        {
                            StringBuilder sbA = new StringBuilder();
                            sbA.Append("{");
                            sbA.Append("\"Message\":\"" + Notification_alert + "\"");
                            sbA.Append("}");
                            try
                            {
                                objAppCommon.SendAndriodNotification(sbA.ToString(), RegistrationKey, AndroidRegKey, SenderId);
                            }
                            catch(Exception ex)
                            {
                                try
                                {
                                    objAppCommon.SendAndriodNotification(sbA.ToString(), RegistrationKey, AndroidRegKey2, SenderId);
                                }
                                catch (Exception exc)
                                {
                                    try
                                    {
                                        objAppCommon.SendAndriodNotificationforFCM(sbA.ToString(), RegistrationKey, FCMAndroidRegKey, FCM_SenderId);
                                    }
                                    catch (Exception exe)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            badgeCount = badgeCount + 1;
                            iBadge = badgeCount.ToString();
                            StringBuilder sbI = new StringBuilder();
                            string DeviceId = RegistrationKey;
                            string PushMessage = Notification_alert;
                            sbI.Append("{");
                            sbI.Append("\"aps\":{");
                            sbI.Append("\"alert\":");
                            sbI.Append("\"" + PushMessage + "\",");
                            sbI.Append("\"badge\":");
                            sbI.Append("" + iBadge + ",");
                            sbI.Append("\"sound\":");
                            sbI.Append("\"new.caf\"");
                            sbI.Append("}");
                            sbI.Append("}");
                            bool stat = objAppCommon.SendIOSNotification(sbI.ToString(), RegistrationKey, "0", "2");
                            bool stat2 = objAppCommon.SendIOSNotification2(sbI.ToString(), RegistrationKey, "0", "2");
                            bool stat3 = objAppCommon.SendIOSNotification3(sbI.ToString(), RegistrationKey, "0", "2");
                            bool stat4 = objAppCommon.SendIOSNotification4(sbI.ToString(), RegistrationKey, "0", "2");

                            if (stat == true)
                            {
                                objMobile.UpdateBadge(RegistrationKey, userdata.DatabaseKey.ClientConnectionString);
                            }
                            else if (stat2 == true)
                            {
                                objMobile.UpdateBadge(RegistrationKey, userdata.DatabaseKey.ClientConnectionString);
                            }
                            else if (stat3 == true)
                            {
                                objMobile.UpdateBadge(RegistrationKey, userdata.DatabaseKey.ClientConnectionString);
                            }
                            else if (stat4 == true)
                            {
                                objMobile.UpdateBadge(RegistrationKey, userdata.DatabaseKey.ClientConnectionString);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
        #region  V2-332

        public void CreateAlert<T>(AlertTypeEnum alertType, long obj_id, string UserName, string Email, string headerBgURL, string somaxLogoURL, string spnloginurl, string footerURL, string Password) where T : class //V2-332
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            Type type = typeof(T);
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Process the following for each item in the object id list
                UserDetails UD;
                Object obj = null;
                // Get the object
                if (type == typeof(UserDetails))
                {
                    UD = new UserDetails()
                    {
                        ClientId = setup.ClientId,
                        SiteId = setup.SiteId,
                        UserInfoId = obj_id
                    };
                    UD.RetrieveUserDetailsByUserInfoID(userdata.DatabaseKey);
                    obj = UD;

                }

                // Determine the target for the Alert
                SetTargetList(alertType, obj_id, UserName, Email);
                if (this.AlertTargetList.Count > 0)
                {
                    // Create the alert                       
                    Alerts alertUser = null;
                    List<Alerts> alertAll = new List<Alerts>();
                    if (alertType == AlertTypeEnum.NewUserAdded)
                    {
                        alertUser = AlertCreateUserInfo(obj, alertType, headerBgURL, somaxLogoURL, spnloginurl, footerURL, Password);
                        alertAll.Add(alertUser);

                         UD = (UserDetails)obj;
                        obj_id = UD.PersonnelId;
                        // alert.ObjectId = UD.UserInfoId;
                        // obj_id = obj.per;//personnelid
                    }
                    if (alertAll != null && alertAll.Count > 0)
                    {
                        foreach (var item in alertAll)
                        {
                            if (item.AlertsId > 0)
                            {
                                // alert is created
                                // send to the targets
                                AlertSendToTarget(item, obj_id, UserName, Email);
                            }

                        }
                    }


                }
            }
        }
        protected void SetTargetList(AlertTypeEnum alertType, long UserInfoId, string UserName, string Email)
        {
            if (!this.TargetSet)
            {
                this.AlertTargetList = new List<AlertTarget>();

                AlertTarget target = new AlertTarget();
                target.UserInfoId = UserInfoId;
                target.CallerUserName = UserName;
                target.UserName = UserName;
                target.email_address = Email;
                AlertTargetList.Add(target);
                this.UserInfoTargetList = this.AlertTargetList;

            }
        }
        protected void AlertSendToTarget(Alerts alert, long UserId, string UserName, string Email)
        {
            try
            {
                // Created the alert - now send it to everyone in the target list

                AlertUser alertuser = new AlertUser();
                alertuser.ClientId = userdata.DatabaseKey.Client.ClientId;
                alertuser.UserId = UserId;//at.UserInfoId;
                alertuser.AlertsId = alert.AlertsId;
                alertuser.ActiveDate = DateTime.UtcNow;
                alertuser.Permission = "";
                alertuser.IsRead = false;
                alertuser.IsDeleted = false;
                alertuser.Create(userdata.DatabaseKey);
                // SendNotification(at.UserInfoId, userdata, alert.Headline);
                SendNotification(UserId, userdata, alert.Headline);
                // SOM-652 - Email 
                // Email if setup 
                // if (setup.EmailSend && !string.IsNullOrEmpty(at.email_address))
                if (setup.EmailSend && !string.IsNullOrEmpty(Email))
                {
                    // SOM-1199 
                    // Email formatting.  Need to change the \r\n with Char(10)+Char(13)
                    string details = alert.Details.Replace("\r\n", "<br/>");
                    //EmailModule emailModule = new EmailModule();
                    Presentation.Common.EmailModule emailModule = new Presentation.Common.EmailModule();
                    //  emailModule.ToEmailAddress = at.email_address;
                    emailModule.ToEmailAddress = Email;
                    //emailModule.MailSubject = alert.Headline;
                    emailModule.Subject = alert.Headline;
                    emailModule.MailBody = details;
                    bool IsSent = emailModule.SendEmail();
                }

                string user = string.Empty;
                //  user = string.IsNullOrEmpty(at.CallerUserName) ? at.UserName : at.CallerUserName;
                user = string.IsNullOrEmpty(UserName) ? "" : UserName;
                //NotificationHub.Hubs.NotificationHub objNotifHub = new NotificationHub.Hubs.NotificationHub();
                //objNotifHub.SendNotification(user, alert.Headline);
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub.Hubs.NotificationHub>();
                context.Clients.Group(user).broadcaastNotif(alert.Headline);

                return;
            }
            catch (Exception ex)
            { throw ex; }
        }
        protected Alerts AlertCreateUserInfo(Object obj, AlertTypeEnum alert_type, string headerBgURL, string somaxLogoURL, string spnloginurl, string footerURL, string Password)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;
            // NewUserAdded
            if (alert_type == AlertTypeEnum.NewUserAdded)
            {
                UserDetails UD = (UserDetails)obj;
                alert.ObjectName = "UserInfo";
                alert.ObjectId = UD.UserInfoId;
                alert.Headline = string.Format(setup.Alert_Headline, UD.UserName);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, headerBgURL
                                           , somaxLogoURL
                                           , UD.FirstName
                                           , UD.LastName
                                           , UD.UserName
                                           , Password
                                           , spnloginurl, footerURL);

                alert.Details = details;

            }
            alert.Create(userdata.DatabaseKey);
            return alert;
        }
        public void CreateAlert<T>(AlertTypeEnum alertType, long obj_id, string UserName, string Email, string headerBgURL, string somaxLogoURL, string spnloginurl, string footerURL, string Password, string FirstName, string LastName) where T : class //V2-332
        {
            setup = RetrieveAlertInfo(alertType);
            // Check if the alert is active 
            Type type = typeof(T);
            // Both the AlertDefine and AlertSetup IsActive columns must be true
            if (setup.Alert_Active)
            {
                // Process the following for each item in the object id list

                //UserDetails UDRP;
                // Object obj = null;
                // Get the object
                // if (type == typeof(UserDetails))

                //     UDRP = new UserDetails()
                //     {
                //         ClientId = setup.ClientId,
                //         SiteId = setup.SiteId,
                //         UserInfoId = obj_id
                //     };
                // UDRP.RetrieveUserDetailsByUserInfoID(userdata.DatabaseKey);
                // obj = UDRP;

                UserDetails UD;
                Object obj = null;
                // Get the object
                if (type == typeof(UserDetails))
                {
                    UD = new UserDetails()
                    {
                        ClientId = setup.ClientId,
                        SiteId = setup.SiteId,
                        UserInfoId = obj_id
                    };
                    UD.RetrieveUserDetailsByUserInfoID(userdata.DatabaseKey);
                    obj = UD;

                }           

            // Determine the target for the Alert
            SetTargetList(alertType, obj_id, UserName, Email);
                if (this.AlertTargetList.Count > 0)
                {
                    // Create the alert                       
                    Alerts alertUser = null;
                    List<Alerts> alertAll = new List<Alerts>();
                    if (alertType == AlertTypeEnum.ResetPassword)
                    {
                        alertUser = AlertCreateResetPassword(alertType, obj_id, headerBgURL, somaxLogoURL, spnloginurl, footerURL, Password, FirstName, LastName, UserName);
                        alertAll.Add(alertUser);
                        UD = (UserDetails)obj;
                        obj_id = UD.PersonnelId;
                        //obj_id = 12;//personnelid
                    }
                    if (alertAll != null && alertAll.Count > 0)
                    {
                        foreach (var item in alertAll)
                        {
                            if (item.AlertsId > 0)
                            {
                                // alert is created
                                // send to the targets
                                AlertSendToTarget(item, obj_id, UserName, Email);
                            }

                        }
                    }


                }
            }
        }

      
        protected Alerts AlertCreateResetPassword(AlertTypeEnum alert_type, long UserInfoId, string headerBgURL, string somaxLogoURL, string spnloginurl, string footerURL, string Password, string FirstName, string LastName, string UserName)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;
            // NewUserAdded
            if (alert_type == AlertTypeEnum.ResetPassword)
            {
                alert.ObjectName = "ResetPassword";
                alert.ObjectId = UserInfoId;
                alert.Headline = string.Format(setup.Alert_Headline, UserName);
                string details = setup.Alert_Details;
                details = details.Replace("~", "\r\n");
                details = string.Format(details, headerBgURL
                                           , somaxLogoURL
                                           , FirstName
                                           , LastName
                                           , UserName
                                           , Password
                                           , spnloginurl, footerURL);

                alert.Details = details;

            }
            alert.Create(userdata.DatabaseKey);
            return alert;
        }
        #endregion  V2-332
        #region V2-663
        public void SendClientProgressBarPrintingCurrentStatus(int TotalCount, int CurrentPrintingcount, string PrintingCountConnectionID)
        {
            if(!string.IsNullOrEmpty(PrintingCountConnectionID))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub.Hubs.NotificationHub>();
                context.Clients.Client(PrintingCountConnectionID).ProgressBarCurrentStatus(TotalCount, CurrentPrintingcount);
            }

        }
        #endregion

        #region V2-726

        protected void SetTargetList(Object obj, AlertTypeEnum alertType, Type type)
        {
            if (!this.TargetSet)
            {
                this.AlertTargetList = new List<AlertTarget>();
                if (setup.Alert_TargetList)
                {
                    // Retrieve the target list
                    AlertTarget alerttarget = new AlertTarget()
                    {
                        ClientId = setup.ClientId,
                        AlertSetupId = setup.AlertSetupId
                    };
                    AlertTargetList = alerttarget.RetreiveTargetList(userdata.DatabaseKey);
                    foreach (AlertTarget at in AlertTargetList)
                    {
                        GetUserInfoList("personnel", at.UserInfoId);
                    }


                }
                else
                {
                    // Based on the alert - create a target list - could only be one 
                    switch (alertType)
                    {                     
                        //V2-726
                        case AlertTypeEnum.MaterialRequestApprovalNeeded:
                            if (type==typeof(MaterialRequest))
                            {
                                MaterialRequest MR = (MaterialRequest)obj;
                                if (MR.MaterialRequestId > 0)
                                {
                                    AlertTarget t = new AlertTarget();
                                    t.UserInfoId = MR.Requestor_PersonnelId;
                                    AlertTargetList.Add(t);
                                    GetUserInfoList("personnel", t.UserInfoId);
                                }
                            }
                            if (type==typeof(WorkOrder))
                            {
                                WorkOrder WO = (WorkOrder)obj;
                                if (WO.WorkOrderId > 0)
                                {
                                    AlertTarget t = new AlertTarget();
                                    t.UserInfoId = WO.Creator_PersonnelId;
                                    AlertTargetList.Add(t);
                                    GetUserInfoList("personnel", t.UserInfoId);
                                }
                            }

                            break;
                        case AlertTypeEnum.PurchaseRequestApprovalNeeded:
                            if (type==typeof(PurchaseRequest))
                            {
                                PurchaseRequest PR = (PurchaseRequest)obj;
                                if (PR.PurchaseRequestId > 0)
                                {
                                    AlertTarget t = new AlertTarget();
                                    t.UserInfoId = PR.CreatedBy_PersonnelId;
                                    AlertTargetList.Add(t);
                                    GetUserInfoList("personnel", t.UserInfoId);
                                }
                            }
                            
                            break;
                        default:
                            break;
                    }
                    // SOM-1228 - Set the email of the altertargets in the alerttarget list
                    SetEmailList();
                }
            }
            return;
        }
        protected Alerts AlertCreate(Object obj, AlertTypeEnum alert_type, string UserName, Type type)
        {
            Alerts alert = new Alerts();
            alert.ClientId = setup.ClientId;
            alert.AlertDefineId = setup.AlertDefineId;
            alert.From = UserName; //userdata.DatabaseKey.UserName;
            alert.AlertType = setup.Alert_Type;
            alert.IsCleared = false;

            //V2-726 Material Request
            if (alert_type == AlertTypeEnum.MaterialRequestApprovalNeeded)
            {
                if (type==typeof(MaterialRequest))
                {
                    MaterialRequest MR = (MaterialRequest)obj;
                    alert.ObjectName = "MaterialRequest";
                    alert.ObjectId = MR.MaterialRequestId;
                    alert.Headline = string.Format(setup.Alert_Headline, MR.MaterialRequestId);
                    string details = setup.Alert_Details;
                    details = details.Replace("~", "\r\n");
                    details = string.Format(details, MR.MaterialRequestId, MR.Personnel_NameFirst + " " + MR.Personnel_NameLast, MR.Description.Length>500 ? MR.Description.Substring(0, 500) : MR.Description);
                    alert.Details = details;
                }
                if (type==typeof(WorkOrder))
                {
                    WorkOrder wo = (WorkOrder)obj;
                    alert.ObjectName = "WorkOrder";
                    alert.ObjectId = wo.WorkOrderId;
                    alert.Headline = string.Format(setup.Alert_Headline, wo.ClientLookupId);
                    string details = setup.Alert_Details;
                    details = details.Replace("~", "\r\n");
                    details = string.Format(details, wo.ClientLookupId
                                               , wo.CreateBy_PersonnelName
                                               , wo.Description.Length > 500 ? wo.Description.Substring(0, 500) : wo.Description);
                    alert.Details = details;
                }
            }
            alert.Create(userdata.DatabaseKey);
            return alert;
        }
        #endregion
    }
}
