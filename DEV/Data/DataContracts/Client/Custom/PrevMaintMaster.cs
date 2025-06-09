/*
******************************************************************************
* PROPRIETARY DATA 
******************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Date        JIRA Item Person         Description
* =========== ========= ============= =============================================================
* 2020-Apr-06 SOM-1737  Roger Lawton  Copy from PM Library
***************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;


namespace DataContracts
{
    public partial class PrevMaintMaster :DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Inactive { get; set; }
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }
        public Int32 CaseNo { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string Chargeto { get; set; }
        public string ChargetoName { get; set; }
        public Int32 FilterType { get; set; }
        public Int64 FilterValue { get; set; }
        public string SearchText { get; set; }
        public Int64 EquipmentId { get; set; }
        public Int64 LocationId { get; set; }
        public Int64 AssignedId { get; set; }
        public Int32 UpdateIndexOut { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 ChildCount { get; set; }
        #endregion

        #region Private Variables
        private bool m_validateClientLookupId;
        private bool m_validateLinkClientLookupId;
        private bool m_ValidateChangeClientLookupId;
        #endregion

        #region Transactions


        public void RetrieveByForeignKey(DatabaseKey dbKey)
        {
            PreventiveMaintenance_RetrieveByForeignKeys trans = new PreventiveMaintenance_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PrevMaintMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PrevMaintMaster);


            this.CreateBy = trans.PrevMaintMaster.CreateBy;
            this.CreateDate = trans.PrevMaintMaster.CreateDate;
            this.ModifyBy = trans.PrevMaintMaster.ModifyBy;
            this.ModifyDate = trans.PrevMaintMaster.ModifyDate;
        }

        public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey)
        {

            PreventiveMaintenance_RetrieveInitialSearchConfigurationData trans = new PreventiveMaintenance_RetrieveInitialSearchConfigurationData ()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            SearchCriteria = trans.SearchCriteria;            
        }       


        public List<PrevMaintMaster> PreventiveMaintenanceRetrieveToSearchCriteria(DatabaseKey dbKey, int filtertype, int filtervalue)
        {
            PreventiveMaintenance_RetrieveToSearchCriteria trans = new PreventiveMaintenance_RetrieveToSearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.FilterType = filtertype;
            trans.FilterValue = filtervalue;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PrevMaintMaster> PrevMaintMasterList = new List<PrevMaintMaster>();         



            foreach (b_PrevMaintMaster prevMaintMaster in trans.SearchResult)
            {
                PrevMaintMaster tmpPrevMaintMaster = new PrevMaintMaster();

                tmpPrevMaintMaster.UpdateFromDatabaseObjectExtended(prevMaintMaster);
                PrevMaintMasterList.Add(tmpPrevMaintMaster);
            }
            

            return PrevMaintMasterList;
        }


        public void UpdateFromDatabaseObjectExtended(b_PrevMaintMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.UpdateIndexOut = dbObj.UpdateIndexOut;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
            switch (dbObj.InactiveFlag)
            {
                case true:
                    Inactive = "True";//loc.ActiveMethod.True;
                    break;
                case false:
                    Inactive = "False";//loc.ActiveMethod.False;
                    break;
                default:
                    break;
            }
        }

        public void ValidateAdd(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<PrevMaintMaster>(dbKey);

        }
        public void ValidateLink(DatabaseKey dbKey)
        {
            m_validateLinkClientLookupId = true;
            Validate<PrevMaintMaster>(dbKey);

        }
        public void ValidateChangeLookupId(DatabaseKey dbKey)
        {
            m_ValidateChangeClientLookupId = true;
            Validate<PrevMaintMaster>(dbKey);         

        }

        public void DeletePreventiveMaintenanceMasterDetails(DatabaseKey dbKey)
        {
            PreventiveMaintenance_DeleteMasterDetails trans = new PreventiveMaintenance_DeleteMasterDetails() 
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PrevMaintMaster = new b_PrevMaintMaster();
            trans.PrevMaintMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }       

        public void CreatePrevMaintMaster_CreateFromPMLibrary(DatabaseKey dbKey)
        {
            PreventiveMaintenance_CreateFromPMLibrary trans = new PreventiveMaintenance_CreateFromPMLibrary()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PrevMaintMaster = new b_PrevMaintMaster();
            trans.PrevMaintMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.UpdateFromDatabaseObject(trans.PrevMaintMaster);
        }

        // SOM-1737 
        public void CreatePrevMaintMaster_CopyFromPMLibrary(DatabaseKey dbKey)
        {
            CreatePrevMaintMaster_CopyFromPMLibrary trans = new CreatePrevMaintMaster_CopyFromPMLibrary()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PrevMaintMaster = new b_PrevMaintMaster();
            trans.PrevMaintMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.UpdateFromDatabaseObject(trans.PrevMaintMaster);
        }

        /*Add by Indusnet Technologies End*/
        #endregion



        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateClientLookupId)
            {
                PreventiveMaintenance_ValidateAdd trans = new PreventiveMaintenance_ValidateAdd()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintMaster = this.ToDatabaseObject();
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
            if (m_validateLinkClientLookupId)
            {
                PreventiveMaintenance_ValidateLink trans = new PreventiveMaintenance_ValidateLink()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintMaster = this.ToDatabaseObject();
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
            if (m_ValidateChangeClientLookupId)
            {
                PreventiveMaintenance_ValidateByClientlookupIdForChange trans = new PreventiveMaintenance_ValidateByClientlookupIdForChange()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PrevMaintMaster = this.ToDatabaseExtendObject();
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

        public b_PrevMaintMaster ToDatabaseExtendObject()
        {
            b_PrevMaintMaster dbObj = this.ToDatabaseObject();
            dbObj.CaseNo = this.CaseNo;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.FilterType = this.FilterType;
            dbObj.FilterValue = this.FilterValue;
            dbObj.SearchText = this.SearchText;
            dbObj.Chargeto = this.Chargeto;
            dbObj.ChargetoName = this.ChargetoName;
            return dbObj;
        }

        public List<PrevMaintMaster> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            PreventiveMaintenance_ChunkSearch trans = new PreventiveMaintenance_ChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PrevMaintMaster = this.ToDatabaseExtendObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PrevMaintMaster> prevMaintMasterlist = new List<PrevMaintMaster>();

            foreach (b_PrevMaintMaster obj in trans.PrevMaintMasterList)
            {
                PrevMaintMaster tmpobj = new PrevMaintMaster();                
                tmpobj.UpdateFromDatabaseObjectExtended(obj);
                prevMaintMasterlist.Add(tmpobj);
            }
            return prevMaintMasterlist;
        }

        public void UpdateForClientLookupId(DatabaseKey dbKey)
        {

            PrevMaint_UpdateForClientLookupId_V2 trans = new PrevMaint_UpdateForClientLookupId_V2();
            trans.PrevMaintMaster = this.ToDatabaseExtendObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObjectExtended(trans.PrevMaintMaster);
        }

    }
}
