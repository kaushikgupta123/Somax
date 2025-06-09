/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-12 SOM-363    Roger Lawton      Modified CreateByPKForeignKeys to use from 
*                                          WorkOrderEdit
*                                          Added TCValue 
* 2015-Nov-04 SOM-839    Roger Lawton      Changed the CreateByPKForeignKeys and CreateByPKForeignKeys
*                                          to not validate.  The validation is done in the 
*                                          row validation method.  Added the TimeCard_Validate 
*                                          method to expose the Validate base method
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;
using Newtonsoft.Json;

namespace DataContracts
{
     [JsonObject]
    public partial class Timecard : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string PersonnelClientLookupId { get; set; }
        public string AccountClientLookupId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string ChargeToPrimeClientLookupId { get; set; }
        public string CraftClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string NameLast { get; set; }
        public long SiteId { get; set; }
        public string VMRSWorkAccomplishedCode { get; set; }
        public long SanitationJobId { get; set; } //V2-1071
        public string NameFull
        {
            get
            {
                string name = NameLast.Trim() + ", " + NameFirst.Trim() + " " + NameMiddle.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }
        public string FullName
        {
            get
            {
                string name = NameFirst.Trim() + " " + NameLast.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }
        public decimal TCValue
        {
          get
          {
            decimal value = (this.Hours * this.BasePay);
            if (this.OvertimeValue > 0)
            {
              value = value * this.OvertimeValue;
            }
            return value;
          }
        }
        public string Name { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }
        #endregion

        public static List<Timecard> UpdateFromDatabaseObjectList(List<b_Timecard> dbObjs)
        {
            List<Timecard> result = new List<Timecard>();

            foreach (b_Timecard dbObj in dbObjs)
            {
                Timecard tmp = new Timecard();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_Timecard dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.NameMiddle = dbObj.NameMiddle;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.CraftClientLookupId = dbObj.CraftClientLookupId;
            this.SiteId = dbObj.SiteId;
            this.VMRSWorkAccomplishedCode = dbObj.VMRSWorkAccomplishedCode;
            //this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
        }

        public b_Timecard ToExtendedDatabaseObject()
        {
            b_Timecard dbObj = this.ToDatabaseObject();
            //dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.PersonnelClientLookupId = this.PersonnelClientLookupId;
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            dbObj.NameMiddle = this.NameMiddle;
            dbObj.AccountClientLookupId = this.AccountClientLookupId;
            dbObj.CraftClientLookupId = this.CraftClientLookupId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }

        public static List<b_Timecard> ToDatabaseObjectList(List<Timecard> objs)
        {
            List<b_Timecard> result = new List<b_Timecard>();
            foreach (Timecard obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public static List<Timecard> RetriveByWorkOrderId(DatabaseKey dbKey, Timecard timecard)
        {
            Timecard_RetrieveByWorkOrderId trans = new Timecard_RetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = timecard.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Timecard.UpdateFromDatabaseObjectList(trans.TimecardList);
        }
        public static List<Timecard> RetriveByServiceOrderId(DatabaseKey dbKey, Timecard timecard)
        {
            Timecard_RetrieveByServiceOrderId trans = new Timecard_RetrieveByServiceOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = timecard.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Timecard.UpdateFromDatabaseObjectList(trans.TimecardList);
        }
        /**********************
         * 
         */

        public static List<Timecard> RetriveByPKWithPersonal(DatabaseKey dbKey, Timecard timecard)
        {
            Timecard_RetrieveByPKWithPersonal trans = new Timecard_RetrieveByPKWithPersonal()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = timecard.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Timecard.UpdateFromDatabaseObjectList(trans.TimecardList);
        }
        /******************************************************************/

        public static List<Timecard> RetriveByPersonnelId(DatabaseKey dbKey, Timecard timecard)
        {
            Timecard_RetrieveByPersonnelId trans = new Timecard_RetrieveByPersonnelId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = timecard.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Timecard> result = new List<Timecard>();

            foreach (b_Timecard dbObj in trans.TimecardList)
            {
              Timecard tmp = new Timecard();
              tmp.UpdateFromDatabaseObject(dbObj);
              tmp.ChargeToPrimeClientLookupId = dbObj.ChargeToPrimeClientLookupId;
              tmp.AccountClientLookupId = dbObj.AccountClientLookupId;
              tmp.CraftClientLookupId = dbObj.CraftClientLookupId;
              tmp.SiteId = dbObj.SiteId;
              result.Add(tmp);
            }
            return result;
         }

        public void RetriveByPKForeignKeys(DatabaseKey dbKey)
        {
            Timecard_RetrieveByPKForeignKeys trans = new Timecard_RetrieveByPKForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            //trans.WorkOrderTask = this.ToDatabaseObject();
            trans.Timecard = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.Timecard);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            Timecard_ValidateByClientLookupId trans = new Timecard_ValidateByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Timecard = this.ToDatabaseObject();
            trans.Timecard.AccountClientLookupId = string.IsNullOrEmpty(this.AccountClientLookupId) ? "" : this.AccountClientLookupId;
            trans.Timecard.PersonnelClientLookupId = string.IsNullOrEmpty(this.PersonnelClientLookupId) ? "" : this.PersonnelClientLookupId;
            trans.Timecard.CraftClientLookupId = string.IsNullOrEmpty(this.CraftClientLookupId) ? "" : this.CraftClientLookupId;
            trans.Timecard.SiteId = this.SiteId;

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
        //SOM-363 - Modified to user from WorkOrderEdit 
        public bool Timecard_Validate(DatabaseKey dbkey)
        {
          Validate<Timecard>(dbkey);
          return IsValid;
        }
        public void CreateByPKForeignKeys(DatabaseKey dbKey)
        {
            //Validate<Timecard>(dbKey);

            if (IsValid)
            {
                Timecard_CreateByForeignKeys trans = new Timecard_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Timecard = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromExtendedDatabaseObject(trans.Timecard);
            }
        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            //Validate<Timecard>(dbKey);

            if (IsValid)
            {
                Timecard_UpdateByForeignKeys trans = new Timecard_UpdateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Timecard = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromExtendedDatabaseObject(trans.Timecard);
            }
        }

        public List<Timecard> RetrieveByWorkOrderId(DatabaseKey dbKey)
        {
            Timecard_RetrieveByWorkOrderId trans = new Timecard_RetrieveByWorkOrderId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Timecard = new b_Timecard { ChargeToId_Primary = this.ChargeToId_Primary };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.TimecardList);
        }
        public List<Timecard> RetrieveBySanId(DatabaseKey dbKey)
        {
            Timecard_RetrieveBySanId trans = new Timecard_RetrieveBySanId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Timecard = new b_Timecard { ChargeToId_Primary = this.ChargeToId_Primary };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.TimecardList);
        }
        //SOM-1249
        public List<Timecard> RetriveBy_SanitationJobId(DatabaseKey dbKey)
        {
            Timecard_RetrieveBySaniatationId trans = new Timecard_RetrieveBySaniatationId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = this.ToExtendedDatabaseObjectForSanitationJob();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Timecard.UpdateFromDatabaseObjectList(trans.TimecardList);
        }

        public b_Timecard ToExtendedDatabaseObjectForSanitationJob()
        {
            b_Timecard dbObj = this.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            ChargeType_Primary = this.ChargeType_Primary;
            ChargeToId_Primary = this.ChargeToId_Primary;

            return dbObj;
        }
        public void CreateByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            if (IsValid)
            {
                Timecard_CreateByForeignKeys_V2 trans = new Timecard_CreateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Timecard = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromExtendedDatabaseObject(trans.Timecard);
            }
        }
        public void UpdateByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            if (IsValid)
            {
                Timecard_UpdateByForeignKeys_V2 trans = new Timecard_UpdateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Timecard = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromExtendedDatabaseObject(trans.Timecard);
            }
        }

        public static List<Timecard> RetriveByWorkOrderIdForMaintananceWorkbenchDetails(DatabaseKey dbKey, Timecard timecard)
        {
            Timecard_RetrieveByWorkOrderIdForMaintananceWorkbenchDetails trans = new Timecard_RetrieveByWorkOrderIdForMaintananceWorkbenchDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Timecard = timecard.ToExtendedDatabaseObjectForMaintananceWorkbenchDetails();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Timecard.UpdateFromDatabaseObjectListForMaintananceWorkbenchDetails(trans.TimecardList);
        }
        public b_Timecard ToExtendedDatabaseObjectForMaintananceWorkbenchDetails()
        {
            b_Timecard dbObj = this.ToDatabaseObject();
            dbObj.PersonnelClientLookupId = this.PersonnelClientLookupId;
            dbObj.Name = this.Name;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            return dbObj;
        }
        public static List<Timecard> UpdateFromDatabaseObjectListForMaintananceWorkbenchDetails(List<b_Timecard> dbObjs)
        {
            List<Timecard> result = new List<Timecard>();

            foreach (b_Timecard dbObj in dbObjs)
            {
                Timecard tmp = new Timecard();
                tmp.UpdateFromExtendedDatabaseObjectForMaintananceWorkbenchDetails(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObjectForMaintananceWorkbenchDetails(b_Timecard dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.Name = dbObj.Name;
            this.TotalCount = dbObj.TotalCount;
        }

        #region Labour Hours

        public List<Timecard> RetrieveSumOfLabourHours(DatabaseKey dbKey)
        {
            TimeCard_RetrieveSumOfLabourHours trans = new TimeCard_RetrieveSumOfLabourHours()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Timecard = this.ToDatabaseObjectRecordsCountByCreateDate();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectRecordsCountByCreateDate(trans.timeCardSumOfHours);
        }
        public b_Timecard ToDatabaseObjectRecordsCountByCreateDate()
        {
            b_Timecard dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;            
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }


        public List<Timecard> UpdateFromDatabaseObjectRecordsCountByCreateDate(List<b_Timecard> dbObjs)
        {
            List<Timecard> result = new List<Timecard>();

            foreach (b_Timecard dbObj in dbObjs)
            {
                Timecard tmp = new Timecard();
                tmp.UpdateFromExtendedDatabastRetriveByWorkOrderIdV2eObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabastRetriveByWorkOrderIdV2eObject(b_Timecard dbObj)
        {            
            this.TotalCount = dbObj.TotalCount;
            this.Name = dbObj.Name;

        }

        #endregion

        public void UpdateFromDatabaseObjectTimeCardPrintExtended(b_Timecard dbObj)
        {
           // this.UpdateFromDatabaseObject(dbObj);
            this.ChargeToId_Primary = dbObj.ChargeToId_Primary;
            this.Hours = dbObj.Hours;
            this.StartDate = dbObj.StartDate;
          this.BasePay = dbObj.BasePay;
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.NameMiddle = dbObj.NameMiddle;
        }
        public void UpdateFromDatabaseObjectTimeCardForDevExpressPrintExtended(b_Timecard dbObj)
        {
            // this.UpdateFromDatabaseObject(dbObj);
            this.ChargeToId_Primary = dbObj.ChargeToId_Primary;
            this.Hours = dbObj.Hours;
            this.StartDate = dbObj.StartDate;
            this.BasePay = dbObj.BasePay;
            this.PersonnelClientLookupId = dbObj.PersonnelClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.NameMiddle = dbObj.NameMiddle;
            this.SanitationJobId = dbObj.SanitationJobId;
        }
    }
}
