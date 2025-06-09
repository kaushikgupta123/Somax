using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;
//using Common.Interfaces;
//using Business.Localization;
//using DataContracts.PaginatedResultSet;

//using DevExpress.Data;
//using DevExpress.Data.Filtering;
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class EventInfo
    {
        public int CustomQueryDisplayId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string Status_Display { get; set; }
        public string WOClientLookupId { get; set; }
        public string EquipClientLookupId { get; set; }       
        public int TotalOpenCount { get; set; }
        public int OpenAssetCount { get; set; }
        public int MonitoredAssetCount { get; set; }
        public int QueryId { get; set; }
        public int EventCount { get; set; }

        public string IotClientlookupId { get; set; }

        public List<EventInfo> listOfEventInfoforIot { get; set; }


        public List<EventInfo> RetrieveAllForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            EventInfo_RetrieveAllForSearch trans = new EventInfo_RetrieveAllForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDateBaseObjectForRetriveAllForSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<EventInfo> eventinfolist = new List<EventInfo>();
            foreach (b_EventInfo eventinfo in trans.EventInfoList)
            {
                EventInfo tmpeventinfo = new EventInfo();
                tmpeventinfo.UpdateFromDatabaseObjectForRetriveAllForSearch(eventinfo, TimeZone);
                eventinfolist.Add(tmpeventinfo);
            }
            return eventinfolist;
        }

        public EventInfo RetrieveByPKForeignkey(DatabaseKey dbKey, string TimeZone)
        {
            EventInfo_RetrieveByPKForeignkey trans = new EventInfo_RetrieveByPKForeignkey()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDateBaseObjectForRetrieveByPKForeignkey();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            b_EventInfo beventinfo = trans.EventInfoList.Count>0? trans.EventInfoList[0]:null;            
            this.UpdateFromDatabaseObjectForRetrieveByPKForeignkey(beventinfo, TimeZone);
           
            return this;
        }

        public b_EventInfo ToDateBaseObjectForRetriveAllForSearch()
        {
            b_EventInfo dbObj = this.ToDatabaseObject();

            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }

        public b_EventInfo ToDateBaseObjectForRetrieveByPKForeignkey()
        {
            b_EventInfo dbObj = this.ToDatabaseObject();            
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_EventInfo dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ProcessBy_Personnel = dbObj.ProcessBy_Personnel;

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.WOClientLookupId = dbObj.WOClientLookupId;
            switch (this.Status)
            {
                case Common.Constants.EventStatusConstants.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.EventStatusConstants.Processed:
                    this.Status_Display = "Processed";
                    break;

                default:
                    this.Status_Display = string.Empty;
                    break;
            }

        }

        public void UpdateFromDatabaseObjectForRetrieveByPKForeignkey(b_EventInfo dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ProcessBy_Personnel = dbObj.ProcessBy_Personnel;

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.WOClientLookupId = dbObj.WOClientLookupId;
            this.EquipClientLookupId = dbObj.EquipClientLookupId;
            switch (this.Status)
            {
                case Common.Constants.EventStatusConstants.Open:
                    this.Status_Display = "Open";
                    break;
                case Common.Constants.EventStatusConstants.Processed:
                    this.Status_Display = "Processed";
                    break;

                default:
                    this.Status_Display = string.Empty;
                    break;
            }
        }

        public int RetrieveEventInfoCountByStatus(DatabaseKey dbKey)
        {
            EventInfo_RetrieveByInfoStatus trans = new EventInfo_RetrieveByInfoStatus()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

           
            return trans.EventInfoStatusCount;
        }

        public List<int> RetrieveAPMCountHozBar(DatabaseKey dbKey)
        {
            EventInfo_RetrieveAPMCountHozBar trans = new EventInfo_RetrieveAPMCountHozBar()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.EventCountList;
        }

        public List<EventInfo> RetrieveAPMBarChart(DatabaseKey dbKey)
        {
            EventInfo_RetrieveForAPMBarChart trans = new EventInfo_RetrieveForAPMBarChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDatabaseObject();
            trans.EventInfo.QueryId = this.QueryId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<EventInfo> eventinfolist = new List<EventInfo>();
            foreach (b_EventInfo eventinfo in trans.EventInfoList)
            {
                EventInfo tmpeventinfo = new EventInfo();
                tmpeventinfo.UpdateFromDatabaseObjectForAPMChart(eventinfo);
                eventinfolist.Add(tmpeventinfo);
            }
            return eventinfolist;
            
        }

        public List<EventInfo> RetrieveAPMDoughChart(DatabaseKey dbKey)
        {
            EventInfo_RetrieveForAPMDoughChart trans = new EventInfo_RetrieveForAPMDoughChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDatabaseObject();
            trans.EventInfo.QueryId = this.QueryId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<EventInfo> eventinfolist = new List<EventInfo>();
            foreach (b_EventInfo eventinfo in trans.EventInfoList)
            {
                EventInfo tmpeventinfo = new EventInfo();
                tmpeventinfo.UpdateFromDatabaseObjectForAPMChart(eventinfo);
                eventinfolist.Add(tmpeventinfo);
            }
            return eventinfolist;
           
        }
        public void UpdateFromDatabaseObjectForAPMChart(b_EventInfo dbObj)
        {
            this.EventCount = dbObj.EventCount;
            this.FaultCode = dbObj.FaultCode;
            this.Disposition = dbObj.Disposition;
        }

        public EventInfo RetrieveAPMCountHozBarV2(DatabaseKey dbKey) 
        {
            EventInfo_RetrieveAPMCountHozBarV2 trans = new EventInfo_RetrieveAPMCountHozBarV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // rkl - 2019-08-15 
            //this.UpdateFromDatabaseObjectForAPMCountHozBar(trans.EventInfo);
            this.UpdateFromDatabaseObjectForAPMCountHozBar(trans.EventInfoList[0]);

            return this;
        }
        public void UpdateFromDatabaseObjectForAPMCountHozBar(b_EventInfo dbObj)
        {
            this.TotalOpenCount = dbObj.TotalOpenCount;
            this.OpenAssetCount = dbObj.OpenAssetCount;
            this.MonitoredAssetCount = dbObj.MonitoredAssetCount;
        }


        public List<EventInfo> EventInfo_RetrieveForIoT(DatabaseKey dbKey, string TimeZone)
        {
            EventInfo_RetrieveForIoT trans = new EventInfo_RetrieveForIoT()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.EventInfo = this.ToDateBaseObjectForRetrieveForIoT();
            trans.EventInfo.IotClientlookupId = this.IotClientlookupId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEventInfoforIot = new List<EventInfo>();
             foreach (b_EventInfo EventInfo in trans.EventInfo.listOfEventInfo)
            {
                EventInfo tmpEventInfo = new EventInfo();

                tmpEventInfo.UpdateFromDatabaseObjectForRetrieveForIoT(EventInfo, TimeZone);
                listOfEventInfoforIot.Add(tmpEventInfo);
            }
            // RKL - 2024-Dec-17
            // The items from the listOfEventInfoforIot list are being added
            // to the list twice - comment out the next line
            //this.listOfEventInfoforIot.AddRange(listOfEventInfoforIot);
            return this.listOfEventInfoforIot;
        }
       


        public b_EventInfo ToDateBaseObjectForRetrieveForIoT()
        {
            b_EventInfo dbObj = this.ToDatabaseObject();
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForRetrieveForIoT(b_EventInfo dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.ProcessBy_Personnel = dbObj.ProcessBy_Personnel;

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            
        }


    }
}
