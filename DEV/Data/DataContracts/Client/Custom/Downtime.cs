﻿/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using Database;
using Database.Business;
using System;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class Downtime : DataContractBase, IStoredProcedureValidation
    {
        public string PersonnelClientLookupId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public long ParentSiteId { get; set; }
        #region V2-695
        public string ReasonForDownDescription { get; set; } 
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        #endregion
        

        //**v2-695 WO
       
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalMinutesDown { get; set; }
        //**
        public static List<Downtime> UpdateFromDatabaseObjectList(List<b_Downtime> dbObjs)
        {
            List<Downtime> result = new List<Downtime>();

            foreach (b_Downtime dbObj in dbObjs)
            {
                Downtime tmp = new Downtime();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_Downtime dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            // RKL - NOT DOING THIS 
            // TimeSpan timeDiff = (TimeSpan)(this.DateUp - this.DateDown);
            // this.MinutesDown = (decimal)timeDiff.TotalMinutes;
        }

        public b_Downtime ToExtendedDatabaseObject()
        {
            b_Downtime dbObj = this.ToDatabaseObject();
            dbObj.PersonnelClientLookupId = this.PersonnelClientLookupId;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            // RKL - NOT DOING THIS 
            //TimeSpan timeDiff = (TimeSpan)(this.DateUp - this.DateDown);
            //  this.MinutesDown = (decimal)timeDiff.TotalMinutes;
            return dbObj;
        }

        public static List<b_Downtime> ToDatabaseObjectList(List<Downtime> objs)
        {
            List<b_Downtime> result = new List<b_Downtime>();
            foreach (Downtime obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public static List<Downtime> RetriveByEquipmentId(DatabaseKey dbKey, Downtime down)
        {
            Downtime_RetrieveByEquipmentId trans = new Downtime_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Downtime = down.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Downtime.UpdateFromDatabaseObjectList(trans.DowntimeList);
        }
        //**V2-695 wo
        public static List<Downtime> RetriveByWorkOrderId_V2(DatabaseKey dbKey, Downtime down)
        {
            Downtime_RetrieveByWorkOrderId_V2 trans = new Downtime_RetrieveByWorkOrderId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            //trans.Downtime = down.ToDatabaseObject();
            //trans.Downtime = this.ToDateBaseObjectForRetriveByWorkOrderId_V2();
            trans.Downtime = down.ToDateBaseObjectForRetriveByWorkOrderId_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Downtime.WoDownTimeUpdateFromDatabaseObjectList(trans.DowntimeList);
        }
        //--
        public static List<Downtime> WoDownTimeUpdateFromDatabaseObjectList(List<b_Downtime> dbObjs)
        {
            List<Downtime> result = new List<Downtime>();

            foreach (b_Downtime dbObj in dbObjs)
            {
                Downtime tmp = new Downtime();
                tmp.WoDownTimeUpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void WoDownTimeUpdateFromExtendedDatabaseObject(b_Downtime dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.ReasonForDownDescription = dbObj.ReasonForDownDescription;
            this.TotalCount = dbObj.TotalCount;
            this.TotalMinutesDown = dbObj.TotalMinutesDown;
        }

        public b_Downtime ToDateBaseObjectForRetriveByWorkOrderId_V2()
        {
            b_Downtime dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            //dbObj.DowntimeId = this.DowntimeId;
            //dbObj.EquipmentId = this.EquipmentId;
            //dbObj.ActionCode = this.ActionCode;
            //dbObj.DateDown = this.DateDown;
            //dbObj.DateUp = this.DateUp;
            //dbObj.FailureCode = this.FailureCode;
            //dbObj.MinutesDown = this.MinutesDown;
            //dbObj.Notes = this.Notes;
            //dbObj.Operator_PersonnelId = this.Operator_PersonnelId;
            //dbObj.ReasonForDown = this.ReasonForDown;
            dbObj.WorkOrderId = this.WorkOrderId;
            //dbObj.UpdateIndex = this.UpdateIndex;
            //------------------
            dbObj.orderbyColumn = this.OrderbyColumn;
            dbObj.orderBy = this.OrderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }

        //**end wo
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            Downtime_ValidateByClientLookupId trans = new Downtime_ValidateByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Downtime = this.ToDatabaseObject();
            trans.Downtime.ParentSiteId = this.ParentSiteId;
            trans.Downtime.PersonnelClientLookupId = string.IsNullOrEmpty(this.PersonnelClientLookupId) ? "" : this.PersonnelClientLookupId;
            trans.Downtime.WorkOrderClientLookupId = string.IsNullOrEmpty(this.WorkOrderClientLookupId) ? "" : this.WorkOrderClientLookupId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (trans.StoredProcValidationErrorList != null)
            {
                foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                {
                    StoredProcValidationError tmp = new StoredProcValidationError();
                    tmp.UpdateFromDatabaseObject(error);
                    errors.Add(tmp);
                }
            }

            return errors;
        }

        public void CreatePKForeignKeys(DatabaseKey dbKey)
        {
            Validate<Downtime>(dbKey);

            if (IsValid)
            {
                Downtime_CreatePKForeignKeys trans = new Downtime_CreatePKForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Downtime = this.ToDatabaseObject();
                trans.Downtime.ParentSiteId = this.ParentSiteId;
                trans.Downtime.PersonnelClientLookupId = string.IsNullOrEmpty(this.PersonnelClientLookupId) ? "" : this.PersonnelClientLookupId;
                trans.Downtime.WorkOrderClientLookupId = string.IsNullOrEmpty(this.WorkOrderClientLookupId) ? "" : this.WorkOrderClientLookupId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Downtime);
            }
        }

        public void UpdatePKForeignKeys(DatabaseKey dbKey)
        {
            Validate<Downtime>(dbKey);

            if (IsValid)
            {
                Downtime_UpdatePKForeignKeys trans = new Downtime_UpdatePKForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Downtime = this.ToDatabaseObject();
                trans.Downtime.ParentSiteId = this.ParentSiteId;
                trans.Downtime.PersonnelClientLookupId = string.IsNullOrEmpty(this.PersonnelClientLookupId) ? "" : this.PersonnelClientLookupId;
                trans.Downtime.WorkOrderClientLookupId = string.IsNullOrEmpty(this.WorkOrderClientLookupId) ? "" : this.WorkOrderClientLookupId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Downtime);
            }
        }

        public static List<Downtime> RetriveByWorkOrderId(DatabaseKey dbKey, Downtime down)
        {
            Downtime_RetrieveByWorkOrderId trans = new Downtime_RetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Downtime = down.ToDatabaseObject();
           

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Downtime.UpdateFromDatabaseObjectList(trans.DowntimeList);
        }

        #region V2-695
        public static List<Downtime> RetriveByEquipmentId_V2(DatabaseKey dbKey, Downtime down)
        {
            Downtime_RetrieveByEquipmentId_V2 trans = new Downtime_RetrieveByEquipmentId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.Downtime = down.ToDatabaseObject_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Downtime.UpdateFromDatabaseObjectList_V2(trans.DowntimeList);
        }
        public b_Downtime ToDatabaseObject_V2()
        {
            b_Downtime dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.offset1=this.offset1;
            dbObj.nextrow=this.nextrow;
            return dbObj;
        }

        public static List<Downtime> UpdateFromDatabaseObjectList_V2(List<b_Downtime> dbObjs)
        {
            List<Downtime> result = new List<Downtime>();

            foreach (b_Downtime dbObj in dbObjs)
            {
                Downtime tmp = new Downtime();
                tmp.UpdateFromExtendedDatabaseObject_V2(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject_V2(b_Downtime dbObj)
        {
            this.UpdateFromExtendedDatabaseObject(dbObj);
            this.ReasonForDownDescription = dbObj.ReasonForDownDescription;
            this.TotalCount = dbObj.TotalCount;
            this.TotalMinutesDown = dbObj.TotalMinutesDown;
        }
        #endregion
    }
}
