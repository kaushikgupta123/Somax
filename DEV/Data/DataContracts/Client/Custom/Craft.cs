/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2014-Oct-02 SOM-354  Roger Lawton       Changed method name from RetrieveAllBySiteName to 
*                                         RetrieveAllForSite
*                                         Removed SIteName property - not needed
*                                         Added Validation Methods
* 2015-Nov-05 SOM-844  Roger Lawton       Added Delete_Inactivate Method
*                                         Cleaned Up
****************************************************************************************************
 */

using System.Collections.Generic;

using Database;
using Database.Business;
using Common.Constants;
using System.Linq;

namespace DataContracts
{

    public partial class Craft : DataContractBase, IStoredProcedureValidation
    {
        public List<b_Craft> CraftList { get; set; }
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public int validatetype { get; set; }
        private const int Validate_Add = 1;
        private const int Validate_Save = 2;

        public static List<Craft> UpdateFromDatabaseObjectList(List<b_Craft> dbObjs)
        {
            List<Craft> result = new List<Craft>();

            foreach (b_Craft dbObj in dbObjs)
            {
                Craft tmp = new Craft();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);

            }
            return result;
        }

        public static List<Craft> UpdateFromDatabaseObject(List<b_Craft> dbObjs)
        {
            List<Craft> result = new List<Craft>();

            foreach (b_Craft dbObj in dbObjs)
            {
                Craft tmp = new Craft();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);

            }
            return result;
        }

        public List<b_Craft> ToDatabaseObjectList()
        {
            List<b_Craft> dbObj = new List<b_Craft>();
            dbObj = this.CraftList;
            return dbObj;
        }

        public List<Craft> RetriveAll(DatabaseKey dbKey)
        {
            Craft_RetrieveAll trans = new Craft_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };

            //trans.cr = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Craft.UpdateFromDatabaseObjectList(trans.CraftList);
        }

        // SOM-354
        public void ValidateAdd(DatabaseKey dbKey)
        {
            validatetype = Validate_Add;
            Validate<Craft>(dbKey);
        }

        // SOM-354
        public void ValidateSave(DatabaseKey dbKey)
        {
            validatetype = Validate_Save;
            Validate<Craft>(dbKey);
        }

        // SOM-354
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            // Add
            if (validatetype == Validate_Add)
            {
                Craft_ValidateInsert trans = new Craft_ValidateInsert()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Craft = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            // Save 
            if (validatetype == Validate_Save)
            {
                Craft_ValidateSave trans = new Craft_ValidateSave()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Craft = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            return errors;
        }

        public void UpdateFromDatabaseObjectExtended(b_Craft dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
        }
        public b_Craft ToDatabaseObjectExtended()
        {
            b_Craft dbObj = this.ToDatabaseObject();
            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;

            return dbObj;
        }

        //SOM-354
        public List<Craft> RetriveAllForSite(DatabaseKey dbKey)
        {
            Craft_RetrieveForSite trans = new Craft_RetrieveForSite()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Craft = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Craft.UpdateFromDatabaseObject(trans.CraftList);
        }
        public List<string> Delete_Inactivate(DatabaseKey dbKey)
        {
            Craft_DeleteInactivate trans = new Craft_DeleteInactivate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Craft = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Craft);
            // We are returned and error message
            // 1107 - Craft {0} has been deleted
            // 1108 - Craft {0} is referenced in the system.  It has been inactivated instead of deleted.
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            ErrorMessages = new List<string>();

            if (trans.StoredProcValidationErrorList != null)
            {
                foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                {
                    StoredProcValidationError tmp = new StoredProcValidationError();
                    tmp.UpdateFromDatabaseObject(error);
                    // Retrieve the error message based on the error code from localization
                    Localizations _DcLoc = new Localizations();
                    _DcLoc.LocaleId = dbKey.Localization;//"en-us"
                    _DcLoc.ResourceSet = LocalizeResourceSetConstants.StoredProcValidation;
                    List<Localizations> locValid = _DcLoc.RetrieveByResourceSet(dbKey).Where(v => v.ResourceId == tmp.ErrorCode.ToString()).ToList();
                    //Business.Localization.ValidationError locValidationError = loc.StoredProcValidation.ValidationError.Find(v => v.Code == tmp.ErrorCode);
                    //string errorMessage = locValidationError.Message.Replace("{0}", tmp.Arg0).Replace("{1}", tmp.Arg1).Replace("{2}", tmp.TableName);
                    //ErrorMessages.Add(errorMessage);
                    if (locValid != null && locValid.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(locValid.First().Value))
                        {
                            string errorMessage = locValid.First().Value.Replace("{0}", tmp.Arg0).Replace("{1}", tmp.Arg1).Replace("{2}", tmp.TableName);

                            ErrorMessages.Add(errorMessage);
                        }
                    }
                    else
                    {
                        string errormessage = string.Format("Unable to locate error message {0} in the localization file", tmp.ErrorCode);
                        ErrorMessages.Add(errormessage);
                    }
                }
            }
            return ErrorMessages;
        }
        #region V2-962
        public List<Craft> RetriveAllForSiteForAdmin(DatabaseKey dbKey)
        {
            Craft_RetrieveForSiteForAdmin trans = new Craft_RetrieveForSiteForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Craft = this.ToDatabaseObject();
            trans.CustomClientId = this.ClientId;
            trans.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Craft.UpdateFromDatabaseObject(trans.CraftList);
        }
        #endregion
    }
}