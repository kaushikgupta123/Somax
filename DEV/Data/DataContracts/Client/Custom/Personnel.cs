/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2017 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-06 SOM-315  Roger Lawton       Retrieve for lookup
* 2016-Oct-31 SOM-642  Roger Lawton       Update personnel info if appropriate
* 2017-Feb-08 SOM-1228 Roger Lawton       Set the email of the altertargets in the alerttarget list
****************************************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data;
using Database.Business;
using Database.Transactions;
using Database;


namespace DataContracts
{
    public partial class Personnel : DataContractBase, IStoredProcedureValidation
    {
        #region properties

        //V2-981
        public List<Personnel> listPersonnel { get; set; }
        //
        public List<b_Personnel> PersonnelList { get; set; }
        public List<long> PersonnelIds { get; set; }
        public List<Notes> NoteList { get; set; }
        public List<FileInfo> AttachmentList { get; set; }
        public List<Contact> ContactList { get; set; }
        // SOM-1228
        public List<AlertTarget> AlertTargetList { get; set; }
        public bool CreateMode { get; set; }
        public string TableName { get; set; }
        // Extended Properties (columns on page displaying data other than what is stored)
        public string Supervisor_ClientLookupId { get; set; }
        public string DefaultStoreRoom_Name { get; set; }
        public string personnelFullName { get; set; }    //SOM-1523
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public string CraftDescription { get; set; }
        // V2-478 Added SecItems and SecProps
        //        Removed ItemName, ItemAccess, ItemCreate, ItemEdit and ItemDelete
        public string SecItems { get; set; }
        public string SecProps { get; set; }
        public string ItemName { get; set; }
        //public bool ItemAccess { get; set; }
        //public bool ItemCreate { get; set; }
        //public bool ItemEdit { get; set; }
        //public bool ItemDelete { get; set; }
        //public string ItemName { get; set; }
        public string LookUpFilterName { get; set; }
        public string Searchtext { get; set; }
        public string PersonnelInitial { get; set; }
        public string UserName { get; set; }

        public string UserSiteName { get; set; }

        public string SiteDescription { get; set; }

        public Int64 EventsId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime?CompleteDate { get; set; }
        public DateTime?ExpireDate { get; set; }

        public string Name { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string ScheduleGroupDescription { get; set; }
        public string CrewDescription { get; set; }
        public string SchiptDescription { get; set; }
        public string CraftClientLookupId { get; set; }
        public int TotalCount { get; set; }
        public DateTime PersonnelAvailabilityDate { get; set; }
        public decimal PAHours { get; set; }
        public string PAShift { get; set; }
        public Int64 PersonnelAvailabilityId { get; set; }
        public Int64 LoginSSOId { get; set; }

        
        /// <summary>
        /// UserInfoId property
        /// </summary>
        [DataMember]
        public string NameFull
        {
            get
            {
                string name = _NameLast.Trim() + ", " + _NameFirst.Trim() + " " + _NameMiddle.Trim();
                return (string.Compare(",", name.Trim()) == 0) ? "" : name;  // Ensure "," won't be returned if _NameLast, _NameFirst, and _NameMiddle are empty
            }
        }

        public string FullName { get; set; }
        

        [DataMember]
        public string LookupIdWithName
        {
            get { return ClientLookupId + " - " + NameFull; }
        }

        public List<Personnel> listOfPersonnel { get; set; }
        public Int64 TimecardId { get; set; }
        public decimal Hours { get; set; }
        public decimal Value { get; set; }
        public string WOClientLookupId { get; set; }
        public DateTime laborstartdate { get; set; }
        public DateTime PersonnelAttendDate { get; set; }
        public decimal PersonnelAttendHours { get; set; }
        public string PersonnelAttendShift { get; set; }
        public string PersonnelAttendShiftDecription { get; set; }
        public Int64 PersonnelAttendanceId { get; set; }
        public string DepartmentDescription { get; set; }
        public string AssetGroup1Names { get; set; }
        public string AssetGroup2Names { get; set; }
        public string AssetGroup3Names { get; set; }
        //V2-720
        public string requestType { get; set; }
        public Int64 ApprovalGroupId { get; set; }
        public string Personnel_ClientLookupId { get; set; } //V2-962

        public int InActiveStatus { get; set; } //V2-1098
        #region V2-1108
        public string AssignedAssetGroup1Names { get; set; }
        public string AssignedAssetGroup2Names { get; set; }
        public string AssignedAssetGroup3Names { get; set; }
        public string AssignedAssetGroup1ClientlookupId { get; set; }
        public string AssignedAssetGroup2ClientlookupId { get; set; }
        public string AssignedAssetGroup3ClientlookupId { get; set; }
        #endregion
        public string DefaultStoreroom { get; set; } //V2-1178
        #endregion
        public List<b_Personnel> ToDatabaseObjectList()
        {
            List<b_Personnel> dbObj = new List<b_Personnel>();
            dbObj = this.PersonnelList;
            return dbObj;
        }

        public List<Personnel> UpdateFromDatabaseObjectlist(List<b_Personnel> dbObjlist)
        {
            List<Personnel> temp = new List<Personnel>();

            Personnel objPer;

            foreach (b_Personnel per in dbObjlist)
            {
                objPer = new Personnel();
                objPer.UpdateFromDatabaseObject(per);
                temp.Add(objPer);
            }

            return (temp);


        }

        public List<Personnel> UpdateFromDatabaseObjectPersonnelPlannerlist(List<b_Personnel> dbObjlist)
        {
            List<Personnel> temp = new List<Personnel>();

            Personnel objPer;

            foreach (b_Personnel per in dbObjlist)
            {
                objPer = new Personnel();
                objPer.UpdateFromDatabasePersonnelPlannerObject(per);
                temp.Add(objPer);
            }

            return (temp);
        }

        public void UpdateFromDatabasePersonnelPlannerObject(b_Personnel dbObj)
        {

            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.PersonnelId = dbObj.PersonnelId;
            // Turn on auditing
            AuditEnabled = true;
        }





        public b_Personnel ToDatabaseObjectExtended()
        {
            b_Personnel dbObj = this.ToDatabaseObject();
            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;
            dbObj.Searchtext = this.Searchtext;
            dbObj.Planner = this.Planner;
            return dbObj;
        }

        public b_Personnel ToDatabasePlannerObjectExtended()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }


        public List<Personnel> RetrievePersonnelListByFilterText(DatabaseKey dbKey)
        {
            RetrievePersonnelListByFilterText trans = new RetrievePersonnelListByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabasePlannerObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectlist(trans.RetPersonnelList));
       }
        // 2012-Mar-23 - RKL
        // Update 
        public void CreateExtended(DatabaseKey dbKey)
        {
            ValidateFor = "PersonnelCreate";
            Validate<Personnel>(dbKey);
            if (IsValid)
            {
                Personnel_CreateExtended trans = new Personnel_CreateExtended()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Personnel = this.ToDatabaseObject();

                trans.Personnel.DefaultStoreRoom_Name = this.DefaultStoreRoom_Name;
                trans.Personnel.Supervisor_ClientLookupId = this.Supervisor_ClientLookupId;
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Personnel);
            }
        }
        // 2012-Mar-19 - RKL
        // Update 
        public void UpdateExtended(DatabaseKey dbKey)
        {
            ValidateFor = "PersonnelCreate";
            Validate<Personnel>(dbKey);
            if (IsValid)
            {

                Personnel_UpdateExtended trans = new Personnel_UpdateExtended()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Personnel = this.ToDatabaseObject();

                trans.Personnel.DefaultStoreRoom_Name = this.DefaultStoreRoom_Name;
                trans.Personnel.Supervisor_ClientLookupId = this.Supervisor_ClientLookupId;
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Personnel);
            }
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "PersonnelCreate")
            {
                // Create a table to hold the columns that need to be validated against their lookup list
                System.Data.DataTable lulist = new DataTable("lulist");
                lulist.Columns.Add("RowID", typeof(Int64));
                lulist.Columns.Add("SiteID", typeof(Int64));
                lulist.Columns.Add("ColumnName", typeof(string));
                lulist.Columns.Add("ColumnValue", typeof(string));
                lulist.Columns.Add("ListName", typeof(string));
                lulist.Columns.Add("ListFilter", typeof(string));
                lulist.Columns.Add("ErrorID", typeof(Int64));
                // Add a row for each column to be validated                                                                                                     
                // Possible future is to process through the properties in the uiconfig and add based on values
                // Crew
               

                Personnel_ValidateExtended trans = new Personnel_ValidateExtended()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    Supervisor_ClientLookupId = string.IsNullOrEmpty(this.Supervisor_ClientLookupId) ? "" : this.Supervisor_ClientLookupId,
                    DefaultStoreroom_Name = string.IsNullOrEmpty(this.DefaultStoreRoom_Name) ? "" : this.DefaultStoreRoom_Name,
                    CreateMode = this.CreateMode,
                    lulist = lulist
                    // 20110039
                    //Type_lookuplist = uicEquipment.Type.Lookup_ListName,
                    //Type_lookuplist_filter = uicEquipment.Type.Lookup_Filter_Property
                };
                trans.Personnel = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

               errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }
            else if (ValidateFor == "ValidateClientLookUp")
            {
                Personnel_ValidateClientLookUpId trans = new Personnel_ValidateClientLookUpId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Personnel = this.ToDatabaseObject();
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
        // SOM-1688
        // Not Used
//        private void UpdateFromDatabaseObjectExtended(b_Personnel dboj, long userinfoid)
//        {
//            UpdateFromDatabaseObject(dboj);
//            Supervisor_ClientLookupId = dboj.Supervisor_ClientLookupId;
//            DefaultStoreRoom_Name = dboj.DefaultStoreRoom_Name;
//            if (dboj.Notes != null)
//            {   
//                // This is not currently used
//                // SOM-1126
//                string tz = "US Eastern Standard Time";
//                this.NoteList = Notes.UpdateFromDatabaseObjectList(dboj.Notes, userinfoid, tz);
//            }
//            else
//            {
//                this.NoteList = new List<Notes>();
//            }
//            if (dboj.FileInfo != null)
//            {
//                // SOM-1156
////                this.AttachmentList = FileInfo.UpdateFromDatabaseObjectList(dboj.FileInfo, userinfoid);
//                this.AttachmentList = FileInfo.UpdateFromDatabaseObjectList(dboj.FileInfo, userinfoid,false);
//            }
//            else
//            {
//                this.AttachmentList = new List<FileInfo>();
//            }
//            if (dboj.Contacts != null)
//            {
//                this.ContactList = Contact.UpdateFromDatabaseObjectList(dboj.Contacts, userinfoid);
//            }

//            //------------------Added By Indusnet Technologies---------------
//            this.CraftDescription = dboj.CraftDescription;
//            //-----------------End Added By Indusnet Technologies------------
//        }
        // 2012-Mar-15 - RKL
        // Retrieve Personnel Information for use on the personnel edit page
        // Modified Personnel Object, Notes, attachments and contracts
        // SOM-1688 - Not used anywhere
        //public void RetrieveByPKExtended(DatabaseKey dbKey)
        //{
        //    Personnel_RetrieveByPKExtended trans = new Personnel_RetrieveByPKExtended();
        //    trans.Personnel = this.ToDatabaseObject();
        //    trans.Personnel.Notes = new List<b_Notes>();
        //    trans.Personnel.FileInfo = new List<b_FileInfo>();
        //    trans.Personnel.Contacts = new List<b_Contact>();
        //    trans.Personnel.TableName = this.TableName;
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    UpdateFromDatabaseObjectExtended(trans.Personnel, dbKey.User.UserInfoId);
        //}

        public List<Personnel> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Personnel_RetrieveClientLookupIdBySearchCriteria trans = new Personnel_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PersonnelList = this.ToDatabaseObjectList();
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> personnelList = new List<Personnel>();
            foreach (b_Personnel personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel()
                {
                    PersonnelId = personnel.PersonnelId,
                    ClientLookupId = personnel.ClientLookupId
                };
                personnelList.Add(tmpPersonnel);
            }

            return personnelList;
        }

        public List<Personnel> RetrieveByPKs(DatabaseKey dbKey)
        {
            Personnel_RetrieveByPKs trans = new Personnel_RetrieveByPKs()
          {
              CallerUserInfoId = dbKey.User.UserInfoId,
              CallerUserName = dbKey.UserName
          };
            trans.PersonnelIds = this.PersonnelIds;
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> personnelList = new List<Personnel>();
            foreach (b_Personnel personnel in trans.PersonnelList)
            {
                Personnel tmp = new Personnel();
                tmp.UpdateFromDatabaseObject(personnel);
                personnelList.Add(tmp);
            }

            return personnelList;
        }

        #region Transactions

        public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey)
        {

            Personnel_RetrieveInitialSearchConfigurationData trans = new Personnel_RetrieveInitialSearchConfigurationData()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            SearchCriteria = trans.SearchCriteria;

            // Add the Dates
            Load_DateSelection();

            // Add the 'Columns'
            Load_ColumnSelection();
        }

        public void CreateInAdmintSite(DatabaseKey dbKey)
        {
            Personnel_CreateInAdminSite trans = new Personnel_CreateInAdminSite();
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Personnel);
        }
        //public List<Personnel> RetrieveAll(DatabaseKey dbKey)
        //{
        //    Personnel_RetrieveAll trans = new Personnel_RetrieveAll();
        //    trans.CallerUserInfoId = dbKey.User.UserInfoId;
        //    trans.CallerUserName = dbKey.UserName;
            
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();

        //    // The create procedure may have populated an auto-incremented key. 
        //   return( UpdateFromDatabaseObjectlist(trans.PersonnelList));
        //}

      // SOM-315 - Retreive For a Lookup List
      public List<Personnel> RetrieveForLookupList(DatabaseKey dbKey)
        {
          Personnel_RetrieveForLookupList trans = new Personnel_RetrieveForLookupList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
          trans.Personnel = this.ToDatabaseObjectExtended();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          // The create procedure may have populated an auto-incremented key. 
          return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }
        // V2 -631 - Retreive For a Lookup List
        public List<Personnel> RetrieveForLookupListV2(DatabaseKey dbKey)
        {
            Personnel_RetrieveForLookupListV2 trans = new Personnel_RetrieveForLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }

        public List<Personnel> RetrieveForPersonalPlannerLookupList(DatabaseKey dbKey)
        {
            Personnel_RetrieveForPlannerLookupList trans = new Personnel_RetrieveForPlannerLookupList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabasePlannerObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectPersonnelPlannerlist(trans.PersonnelList));
        }

        //----------------------------Added By Indusnet Technologies----------------------
        private void UpdateFromDatabaseObjectExtendedForRetrieveByClinet(b_Personnel dboj)
        {
            UpdateFromDatabaseObject(dboj);
            Supervisor_ClientLookupId = dboj.Supervisor_ClientLookupId;
            DefaultStoreRoom_Name = dboj.DefaultStoreRoom_Name;      

            this.CraftDescription = dboj.CraftDescription;
           
        }
        public List<Personnel> UpdateFromDatabaseObjectlistExtendedForRetrieveByClient(List<b_Personnel> dbObjlist)
        {
            List<Personnel> temp = new List<Personnel>();

            Personnel objPer;

            foreach (b_Personnel per in dbObjlist)
            {
                objPer = new Personnel();
                objPer.UpdateFromDatabaseObjectExtendedForRetrieveByClinet(per);
                temp.Add(objPer);
            }

            return (temp);


        }
        public List<Personnel> RetrievePersonnelListByClientId(DatabaseKey dbKey)
        {
            RetrievePersonnelListByClientId trans = new RetrievePersonnelListByClientId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };           
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlistExtendedForRetrieveByClient(trans.RetPersonnelList));
        }
        // SOM-1228
        public void FillAlertTargetEmail(DatabaseKey dbKey)
        {
          RetrievePersonnelFromList trans = new RetrievePersonnelFromList()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
            UseTransaction = false
          };
          System.Data.DataTable pelist = new DataTable("pelist");
          pelist.Columns.Add("RowID", typeof(Int64));
          foreach (AlertTarget t in this.AlertTargetList)
          {
            System.Data.DataRow dr = pelist.NewRow();
            dr[0] = t.UserInfoId;
            pelist.Rows.Add(dr);
          }
          trans.Personnel = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.pelist = pelist;
          trans.Execute();
          List<Personnel> pedata = UpdateFromDatabaseObjectlist(trans.RetPersonnelList);  // Only need personnelid and email address
          foreach (AlertTarget t in this.AlertTargetList)
          {
                if(pedata !=null  && pedata.Count>0)
                {
                    t.email_address = pedata.SingleOrDefault(r => r.PersonnelId == t.UserInfoId).Email;
                }

            }

          return;
        }

        public void ValidateClientLookUpId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientLookUp";
            Validate<Personnel>(dbKey);
        }

        string ValidateFor = string.Empty;

        public List<Personnel> RetrieveForLookupListBySecurityItem(DatabaseKey dbKey)
        {
            Personnel_RetrieveForLookupListBySecurityItem trans = new Personnel_RetrieveForLookupListBySecurityItem()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectExtendedBySecurityItem();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }     
        public b_Personnel ToDatabaseObjectExtendedBySecurityItem()
        {
            // V2-478
            b_Personnel dbObj = this.ToDatabaseObject();
            dbObj.SecItems = this.SecItems;
            dbObj.SecProps = this.SecProps;
            //dbObj.ItemAccess = this.ItemAccess;
            //dbObj.ItemCreate = this.ItemCreate;
            //dbObj.ItemEdit = this.ItemEdit;
            //dbObj.ItemDelete = this.ItemDelete;
            //dbObj.ItemName = this.ItemName;
            //dbObj.UserSec = this.UserSec;
            return dbObj;
        }
        public b_Personnel ToDatabaseObjectExtendedByActiveUserSecurityItem()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;         
            dbObj.SiteId = this.SiteId;
            dbObj.ItemName = this.ItemName;          
            return dbObj;
        }

        public List<Personnel> UpdateFromDatabaseObjectlistExtendedForActiveUserSecurityItem(List<b_Personnel> dbObjlist)
        {
            List<Personnel> temp = new List<Personnel>();

            Personnel objPer;

            foreach (b_Personnel per in dbObjlist)
            {
                objPer = new Personnel();               
                objPer.PersonnelId = per.PersonnelId;
                objPer.ClientLookupId = per.ClientLookupId;
                objPer.FullName = per.FullName;                            
                temp.Add(objPer);
            }

            return (temp);


        }
        //---------------------------End Added By Indusnet Technologies-------------------


        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }

        private void Load_DateSelection()
        {

            List<KeyValuePair<string, string>> search_DateSelection = new List<KeyValuePair<string, string>>();
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
                {
                    search_DateSelection.Add(new KeyValuePair<string, string>(pi.Name, "Name"));    // 20110024
                }

            }
            SearchCriteria.Add("dates", search_DateSelection);
        }

        private void Load_ColumnSelection()
        {
            List<KeyValuePair<string, string>> search_ColumnSelection = new List<KeyValuePair<string, string>>();

            search_ColumnSelection.Add(new KeyValuePair<string, string>("ClientLookupId", "PersonnelId"));         // 20110019,20110024
            search_ColumnSelection.Add(new KeyValuePair<string, string>("NameFull", "NameFull"));
            SearchCriteria.Add("columns", search_ColumnSelection);
        }


        #endregion

        //SOM - 828
        public void UpdateForMultiUserSite(DatabaseKey dbKey)
        {
                Personnel_UpdateForMultiUserSite trans = new Personnel_UpdateForMultiUserSite()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Personnel = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                //UpdateFromDatabaseObject(trans.Personnel);
                this.UpdateIndex = trans.Personnel.UpdateIndex;
            }
        //SOM-642
        public void UpdateFromUserInfo(DatabaseKey dbKey)
        {
          Personnel_UpdateFromUserInfo trans = new Personnel_UpdateFromUserInfo()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };
          trans.Personnel = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          this.UpdateIndex = trans.Personnel.UpdateIndex;
        }
        public void UpdateFromUserInfoAdmin(DatabaseKey dbKey)
        {
            Personnel_UpdateFromUserInfoAdmin trans = new Personnel_UpdateFromUserInfoAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.UpdateIndex = trans.Personnel.UpdateIndex;
        }
        public List<Personnel> RetrieveAll(DatabaseKey dbKey)
        {

            Personnel_RetrieveAll_V2 trans = new Personnel_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> PersonnelList = new List<Personnel>();
            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();

                tmpPersonnel.UpdateFromDatabaseObject(Personnel);
                PersonnelList.Add(tmpPersonnel);
            }
            return PersonnelList;
        }

        public List<Personnel> PersonnelRetrieveForMention(DatabaseKey dbKey)
        {
            Personnel_RetrieveForMention trans = new Personnel_RetrieveForMention()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };

            trans.Personnel = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> PersonnelMentionList = new List<Personnel>();
            foreach (b_Personnel PersonnelMention in trans.PersonnelMentionList)
            {
                Personnel tmpPersonnelMention = new Personnel();
                tmpPersonnelMention.UpdateFromDatabaseObjectforMention(PersonnelMention);

                PersonnelMentionList.Add(tmpPersonnelMention);
            }

            return PersonnelMentionList;
        }
        public Personnel RetrieveChunkSearchV2(DatabaseKey dbKey)
        {
            Personnel_RetrieveV2ChunkSearch trans = new Personnel_RetrieveV2ChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetriveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfPersonnel = new List<Personnel>();

            List<Personnel> personnellist = new List<Personnel>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Personnel p in trans.Personnel.listOfPersonnels)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveV2(p);
                personnellist.Add(tmpPersonnel);
            }
            this.listOfPersonnel.AddRange(personnellist);
            return this;
        }
        public void UpdateFromDatabaseObjectforMention(b_Personnel dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.FullName = dbObj.FullName;
            this.PersonnelInitial = dbObj.PersonnelInitial;
            this.UserName = dbObj.UserName;


        }
        public void UpdateFromDatabaseObjectForRetriveV2(b_Personnel dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ScheduleGroupDescription = dbObj.ScheduleGroupDescription;
            this.SchiptDescription = dbObj.ShiftDescription;
            this.CrewDescription = dbObj.CrewDescription;
            this.CraftDescription = dbObj.CraftDescription;
            this.CraftClientLookupId = dbObj.CraftClientLookupId;
            //V2-1108
            this.AssignedAssetGroup1ClientlookupId = dbObj.AssignedAssetGroup1ClientlookupId;
            this.AssignedAssetGroup2ClientlookupId = dbObj.AssignedAssetGroup2ClientlookupId;
            this.AssignedAssetGroup3ClientlookupId = dbObj.AssignedAssetGroup3ClientlookupId;
            //  this.LoginSSOId = dbObj.LoginSSOId;
            this.TotalCount = dbObj.TotalCount;
        }

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public void UpdateFromDatabaseObjectExtendedForSelectedUser(b_Personnel dboj)
        {
            // UpdateFromDatabaseObject(dboj);
            this.ClientId = dboj.ClientId;
            this.PersonnelId = dboj.PersonnelId;
            this.UserInfoId = dboj.UserInfoId;
            this.SiteId = dboj.SiteId;
            this.UserSiteName = dboj.SiteName;
            this.CraftDescription = dboj.CraftDescription;
            this.SiteDescription = dboj.SiteDescription;

        }
        #endregion


        public void RetrieveByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            Personnel_RetrieveByForeignKeys_V2 trans = new Personnel_RetrieveByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObjectforRetrieveByPKForeignKeys_V2(trans.Personnel);
           
        }


        public void UpdateFromDatabaseObjectforRetrieveByPKForeignKeys_V2(b_Personnel dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameMiddle = dbObj.NameMiddle;
            this.NameLast = dbObj.NameLast;
            this.DepartmentId = dbObj.DepartmentId;
            this.DepartmentDescription = dbObj.DepartmentDescription;
            this.CraftId = dbObj.CraftId;
            this.CraftDescription = dbObj.CraftDescription;
            this.Shift = dbObj.Shift;
            this.Crew = dbObj.Crew;
            this.ScheduleGroup = dbObj.ScheduleGroup;
            this.Planner = dbObj.Planner;
            this.UpdateIndex = dbObj.UpdateIndex;
            this.SchiptDescription = dbObj.ShiftDescription;
            this.ScheduleGroupDescription = dbObj.ScheduleGroupDescription;
            this.CrewDescription = dbObj.CrewDescription;
            this.BasePay = dbObj.BasePay;
            this.InitialPay = dbObj.InitialPay;
            this.StartDate = dbObj.StartDate;
            this.LastSalaryReview = dbObj.LastSalaryReview;
            this.AssetGroup1 = dbObj.AssetGroup1;
            this.AssetGroup2 = dbObj.AssetGroup2;
            this.AssetGroup3 = dbObj.AssetGroup3;
            this.AssetGroup1Names = dbObj.AssetGroup1Names;
            this.AssetGroup2Names = dbObj.AssetGroup2Names;
            this.AssetGroup3Names = dbObj.AssetGroup3Names;
            this.ScheduleEmployee = dbObj.ScheduleEmployee;
            this.ExOracleUserId = dbObj.ExOracleUserId;//V2-831
            this.Value = dbObj.Value;
            this.InactiveFlag = dbObj.InactiveFlag; //V2-1098
            #region V2-1108
            this.AssignedAssetGroup1 = dbObj.AssignedAssetGroup1;
            this.AssignedAssetGroup2 = dbObj.AssignedAssetGroup2;
            this.AssignedAssetGroup3 = dbObj.AssignedAssetGroup3;
            this.AssignedAssetGroup1Names = dbObj.AssignedAssetGroup1Names;
            this.AssignedAssetGroup2Names = dbObj.AssignedAssetGroup2Names;
            this.AssignedAssetGroup3Names = dbObj.AssignedAssetGroup3Names;
            #endregion
            #region V2-1178
            this.DefaultStoreroom = dbObj.DefaultStoreroom;
            this.Default_StoreroomId = dbObj.Default_StoreroomId;
            #endregion
        }

        public static List<Personnel> RetriveEventsByPersonnelId(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrieveEventsByPersonnelId trans = new Personnel_RetrieveEventsByPersonnelId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = Personnel.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Personnel.UpdateFromDatabaseObjectList(trans.PersonnelList);
        }

        public static List<Personnel> UpdateFromDatabaseObjectList(List<b_Personnel> dbObjs)
        {
            List<Personnel> result = new List<Personnel>();

            foreach (b_Personnel dbObj in dbObjs)
            {
                Personnel tmp = new Personnel();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_Personnel dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.EventsId = dbObj.EventsId;
            this.PersonnelId = dbObj.PersonnelId;
            this.Type = dbObj.Type;
            this.Description = dbObj.Description;
            this.CompleteDate = dbObj.CompleteDate;
            this.ExpireDate = dbObj.ExpireDate;            
        }
        public b_Personnel ToDateBaseObjectForRetriveChunkSearch()
        {
            b_Personnel dbObj = this.ToDatabaseObject();

            dbObj.PersonnelId = this.PersonnelId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.Name = this.Name;
            dbObj.SearchText = this.SearchText;
            dbObj.Shift = this.Shift;
            dbObj.ScheduleGroup = this.ScheduleGroup;
            dbObj.CraftClientLookupId = this.CraftClientLookupId;
            dbObj.InActiveStatus = this.InActiveStatus;
            //V2-1108
            dbObj.AssignedAssetGroup1Id = this.AssignedAssetGroup1;
            dbObj.AssignedAssetGroup2Id = this.AssignedAssetGroup2;
            dbObj.AssignedAssetGroup3Id = this.AssignedAssetGroup3;
            return dbObj;
        }
        public static List<Personnel> RetriveLaborsByPersonnelId(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrieveLaborByPersonnelId trans = new Personnel_RetrieveLaborByPersonnelId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = Personnel.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Personnel.UpdateFromDatabaseObjectListForRetrivingLabors(trans.PLList);
        }

        public static List<Personnel> UpdateFromDatabaseObjectListForRetrivingLabors(List<b_Personnel> dbObjs)
        {
            List<Personnel> result = new List<Personnel>();

            foreach (b_Personnel dbObj in dbObjs)
            {
                Personnel tmp = new Personnel();
                tmp.UpdateFromExtendedDatabaseObjectForRetrivingLabors(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObjectForRetrivingLabors(b_Personnel dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.TimecardId = dbObj.TimecardId;
            this.PersonnelId = dbObj.PersonnelId;
            this.laborstartdate = dbObj.laborstartdate;
            this.Hours = dbObj.Hours;
            this.Value = dbObj.Value;
            this.WOClientLookupId = dbObj.WOClientLookupId;
        }


        public static List<Personnel> RetrivePersonnelAvailabilityByPersonnelId(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrievePersonnelAvailabilityByPersonnelId trans = new Personnel_RetrievePersonnelAvailabilityByPersonnelId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = Personnel.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Personnel.UpdateFromDatabaseObjectListForRetrivingPersonnelAvailability(trans.PAList);
        }

        public static List<Personnel> UpdateFromDatabaseObjectListForRetrivingPersonnelAvailability(List<b_Personnel> dbObjs)
        {
            List<Personnel> result = new List<Personnel>();

            foreach (b_Personnel dbObj in dbObjs)
            {
                Personnel tmp = new Personnel();
                tmp.UpdateFromExtendedDatabaseObjectForRetrivingPersonnelAvailability(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectForRetrivingPersonnelAvailability(b_Personnel dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PersonnelAvailabilityId = dbObj.PersonnelAvailabilityId;
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelAvailabilityDate = dbObj.PersonnelAvailabilityDate;
            this.PAHours = dbObj.PAHours;
            this.PAShift = dbObj.PAShift;            
        }


        public static List<Personnel> RetrivePersonnelAttendanceByPersonnelId(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrievePersonnelAttendanceByPersonnelId trans = new Personnel_RetrievePersonnelAttendanceByPersonnelId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = Personnel.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Personnel.UpdateFromDatabaseObjectListForRetrivingPersonnelAttendance(trans.PersonnelAttendList);
        }

        public static List<Personnel> UpdateFromDatabaseObjectListForRetrivingPersonnelAttendance(List<b_Personnel> dbObjs)
        {
            List<Personnel> result = new List<Personnel>();

            foreach (b_Personnel dbObj in dbObjs)
            {
                Personnel tmp = new Personnel();
                tmp.UpdateFromExtendedDatabaseObjectForRetrivingPersonnelAttendance(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectForRetrivingPersonnelAttendance(b_Personnel dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PersonnelAttendanceId = dbObj.PersonnelAttendanceId;
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelAttendDate = dbObj.PersonnelAttendDate;
            this.PersonnelAttendHours = dbObj.PersonnelAttendHours;
            this.PersonnelAttendShift = dbObj.PersonnelAttendShift;
            this.PersonnelAttendShiftDecription = dbObj.PersonnelAttendShiftDecription;
        }

        public void RetrievePersonnelByPersonnelId_V2(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrievePersonnelByPersonnelId_V2 trans = new Personnel_RetrievePersonnelByPersonnelId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = Personnel.ToDatabaseObjectRetrievePersonnelByPersonnelIdV2();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            //UpdateFromDatabaseObject(trans.Personnel);

            this.PersonnelId = trans.Personnel.PersonnelId;
            this.Email = trans.Personnel.Email;
            this.UserName = trans.Personnel.UserName;
            
        }
        public b_Personnel ToDatabaseObjectRetrievePersonnelByPersonnelIdV2()
        {
            b_Personnel dbObj = this.ToDatabaseObject();
            dbObj.PersonnelId = this.PersonnelId;           
            return dbObj;
        }

        #region Retrieve personnel for all active and full user
        public List<Personnel> RetrieveAllPersonnelforActiveandFullUser(DatabaseKey dbKey)
        {
            Personnel_RetrieveAllPersonnelForActiveandFullUser trans = new Personnel_RetrieveAllPersonnelForActiveandFullUser()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> personnellist = new List<Personnel>();

            foreach (b_Personnel WOP in trans.PersonnelList)
            {
                Personnel tmpPer = new Personnel();
                tmpPer.UpdateFromDatabaseObjectForRetriveAllActiveandFullUsersPersonnel(WOP);
                personnellist.Add(tmpPer);
            }
            return personnellist;
        }

        public void UpdateFromDatabaseObjectForRetriveAllActiveandFullUsersPersonnel(b_Personnel dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.NameLast = dbObj.NameLast;
            this.NameFirst = dbObj.NameFirst;
            this.Buyer = dbObj.Buyer;
            this.ScheduleEmployee = dbObj.ScheduleEmployee;
        }
        #endregion

        #region Retrieve for WorkOrder completion wizard tab
        public void RetrieveForWorkOrderCompletionWizard(DatabaseKey dbKey)
        {
            Personnel_RetrieveForWorkOrderCompletionWizard trans = new Personnel_RetrieveForWorkOrderCompletionWizard()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = ToDatabaseObject();
            trans.Personnel.Hours = Hours;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            //UpdateFromDatabaseObjectforRetrieveByPKForeignKeys_V2(trans.Personnel);
            
            ClientLookupId = trans.Personnel.ClientLookupId;
            FullName = trans.Personnel.FullName;
            Value = trans.Personnel.Value;
        }
        #endregion

        #region Retrieve personnel LookupList For ApprovalGroup
        //V2-720
        public List<Personnel> RetrievePersonnelLookupListForApprovalGroup(DatabaseKey dbKey)
        {
            Personnel_LookupListRetrieveForChunkSearch_V2 trans = new Personnel_LookupListRetrieveForChunkSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectForPersonnelLookupListForApprovalGroup();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> personnellist = new List<Personnel>();

            foreach (b_Personnel item in trans.PersonnelList)
            {
                Personnel tmpPer = new Personnel();
                tmpPer.UpdateFromDatabaseObjectForPersonnelLookupListForApprovalGroup(item);
                personnellist.Add(tmpPer);
            }
            return personnellist;
        }
       
        public b_Personnel ToDatabaseObjectForPersonnelLookupListForApprovalGroup()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.FullName = this.FullName;
            dbObj.AssetGroup1 = this.AssetGroup1;
            dbObj.AssetGroup2 = this.AssetGroup2;
            dbObj.AssetGroup3 = this.AssetGroup3;
            dbObj.requestType = this.requestType;
            dbObj.approvalGroupId = this.ApprovalGroupId;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.AssetGroup1Names=this.AssetGroup1Names;
            dbObj.AssetGroup2Names=this.AssetGroup2Names;
            dbObj.AssetGroup3Names=this.AssetGroup3Names;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForPersonnelLookupListForApprovalGroup(b_Personnel dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.FullName = dbObj.FullName;
            this.AssetGroup1 = dbObj.AssetGroup1;
            this.AssetGroup2 = dbObj.AssetGroup2;
            this.AssetGroup3 = dbObj.AssetGroup3;
            this.AssetGroup1Names=dbObj.AssetGroup1Names;
            this.AssetGroup2Names=dbObj.AssetGroup2Names;
            this.AssetGroup3Names=dbObj.AssetGroup3Names;
            this.TotalCount = dbObj.TotalCount;
        }
    #endregion

        #region V2-989
        public List<Personnel> RetrievePartManagementForLookupList(DatabaseKey dbKey)
        {
          Personnel_RetrievePartManagementForLookupList trans = new Personnel_RetrievePartManagementForLookupList()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };
          trans.Personnel = this.ToDatabaseObjectExtended();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          // The create procedure may have populated an auto-incremented key. 
          return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }
        #endregion

        #region V2-798
        public List<Personnel> RetrieveAllActiveForLookupList(DatabaseKey dbKey)
        {
            Personnel_RetrieveAllActiveForLookupList trans = new Personnel_RetrieveAllActiveForLookupList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }
        #endregion

        #region V2-806
        public List<Personnel> RetrieveAllActiveForLookupListForAdmin(DatabaseKey dbKey)
        {
            Personnel_RetrieveAllActiveForLookupListForAdmin trans = new Personnel_RetrieveAllActiveForLookupListForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectExtendedForAdmin();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }
        public b_Personnel ToDatabaseObjectExtendedForAdmin()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public void RetrievePersonnelByPersonnelId_V2ForAdmin(DatabaseKey dbKey, Personnel Personnel)
        {
            Personnel_RetrieveByForeignKeys_V2ForAdmin trans = new Personnel_RetrieveByForeignKeys_V2ForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = this.ToDatabaseObjectRetrievePersonnelByPersonnelIdV2ForAdmin();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Personnel);

        }
        public b_Personnel ToDatabaseObjectRetrievePersonnelByPersonnelIdV2ForAdmin()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }
        #endregion

        #region V2-820
        public List<Personnel> RetrieveForLookupListByMultipleSecurityItem(DatabaseKey dbKey)
        {
            Personnel_RetrieveForLookupListByMultipleSecurityItem trans = new Personnel_RetrieveForLookupListByMultipleSecurityItem()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.ToDatabaseObjectExtendedBySecurityItem();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.PersonnelList));
        }
        #endregion

        #region V2-929
        public List<Personnel> RetrieveLookupListForActiveAdminOrFullUser(DatabaseKey dbKey)
        {
            Personnel_RetrieveLookupListActiveAdminOrFullUser trans = new Personnel_RetrieveLookupListActiveAdminOrFullUser()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetrieveActiveAdminOrFullUser();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUser(Personnel);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }

        public b_Personnel ToDateBaseObjectForRetrieveActiveAdminOrFullUser()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;

            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUser(b_Personnel dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion

        #region V2-950
        public List<Personnel> RetrieveLookupListForActiveAdminOrFullUserPlanner(DatabaseKey dbKey)
        {
            Personnel_RetrieveLookupListActiveAdminOrFullUserPlanner trans = new Personnel_RetrieveLookupListActiveAdminOrFullUserPlanner()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetrieveActiveAdminOrFullUserPlanner();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUserPlanner(Personnel);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }

        public b_Personnel ToDateBaseObjectForRetrieveActiveAdminOrFullUserPlanner()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;

            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUserPlanner(b_Personnel dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
        #region V2-712
        public List<Personnel> RetrievePMAssignPersonneLookupList(DatabaseKey dbKey)
        {
            Personnel_RetrievePMAssignPersonneLookupList trans = new Personnel_RetrievePMAssignPersonneLookupList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetrieveActiveAdminOrFullUser();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUser(Personnel);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }

        #endregion


        #region V2-981
        public List<Personnel> GetAllPersonnelLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Personnel_RetrieveLookupListActive trans = new Personnel_RetrieveLookupListActive()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForPersonnelLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listPersonnel = new List<Personnel>();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveAllActive(Personnel, TimeZone);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }
        public b_Personnel ToDateBaseObjectForPersonnelLookuplistChunkSearch()
        {
            b_Personnel dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllActive(b_Personnel dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.TotalCount = dbObj.TotalCount;
            this.NameFirst = dbObj.NameFirst;
            this.NameLast = dbObj.NameLast;

        }

        #endregion
        #region V2-962
      
        public void DeleteForAdmin(DatabaseKey dbKey)
        {
            Personnel_DeleteForAdmin trans = new Personnel_DeleteForAdmin();
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.customClientid = this.ClientId;
            trans.Execute();
        }
        public void PersonnelCreateforAdmin(DatabaseKey dbKey)
        {
            Personnel_CreateforAdmin trans = new Personnel_CreateforAdmin();
            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.customClientid = this.ClientId;
            trans.Execute();
           
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Personnel);
        }
        public List<Personnel> RetrieveByUserInfoIdForAdminUserManagementChildGrid(DatabaseKey dbKey)
        {
            Personnel_RetrieveByUserInfoIdForAdminUserManagementChildGrid_V2 trans = new Personnel_RetrieveByUserInfoIdForAdminUserManagementChildGrid_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForAdminUserManagementChildGrid();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForAdminUserManagementChildGrid(Personnel);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }
        public b_Personnel ToDateBaseObjectForAdminUserManagementChildGrid()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.UserInfoId = this.UserInfoId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForAdminUserManagementChildGrid(b_Personnel dbObj)
        {
            this.Personnel_ClientLookupId = dbObj.Personnel_ClientLookupId;
            this.CraftId = dbObj.CraftId;
            this.CraftDescription = dbObj.CraftDescription;
            this.UserSiteName = dbObj.SiteName;
            this.SiteId = dbObj.SiteId;
            this.Buyer = dbObj.Buyer;
            this.Planner = dbObj.Planner;
        }
        #endregion
        #region V2-1178

        public List<Personnel> Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2(DatabaseKey dbKey)
        {
            Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2 trans = new Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetrieveChunkSearchActiveAdminOrFullUser();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Personnel> Personnellist = new List<Personnel>();

            foreach (b_Personnel Personnel in trans.PersonnelList)
            {
                Personnel tmpPersonnel = new Personnel();
                tmpPersonnel.UpdateFromDatabaseObjectForRetriveAllActiveAdminOrFullUser(Personnel);
                Personnellist.Add(tmpPersonnel);
            }
            return Personnellist;
        }

        public b_Personnel ToDateBaseObjectForRetrieveChunkSearchActiveAdminOrFullUser()
        {
            b_Personnel dbObj = new b_Personnel();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;

            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.NameFirst = this.NameFirst;
            dbObj.NameLast = this.NameLast;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }
        public void RetriveByPersonnelId(DatabaseKey dbKey)
        {
            Personnel_RetrievePersonnelIdByClientLookupId trans = new Personnel_RetrievePersonnelIdByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Personnel = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //  UpdateFromDatabaseObjectExtended(trans.Personnel);

            this.PersonnelId = trans.Personnel.PersonnelId;
           // this.Email = trans.Personnel.Email;
          //  this.UserName = trans.Personnel.UserName;
        }


        public Personnel RetrievePersonnelIdByClientLookupId(DatabaseKey dbKey)
        {
            Personnel_RetrievePersonnelIdByClientLookupId trans = new Personnel_RetrievePersonnelIdByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Personnel = this.ToDateBaseObjectForRetrievePersonnelIdByClientLookupId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Personnel tmpPersonnel = new Personnel()
            {
                PersonnelId = trans.PersonnelResult.PersonnelId,
                ClientLookupId = trans.PersonnelResult.ClientLookupId,
            };

            return tmpPersonnel;
        }

        public b_Personnel ToDateBaseObjectForRetrievePersonnelIdByClientLookupId()
        {
            b_Personnel dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;

            return dbObj;
        }
        #endregion
    }
}
