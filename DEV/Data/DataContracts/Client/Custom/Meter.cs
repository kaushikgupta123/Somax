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
    public partial class Meter : DataContractBase, IStoredProcedureValidation
    {
        #region Properties      
        public bool CreateMode { get; set; }
        public List<long> PMWOList { get; set; }  // SOM-928
        public long PersonnelId { get; set; }  // SOM-928
        public string PMWOClientLookupIds { get; set; } //V2-784
        #region V2-950
        public int OffSetVal { get; set; } 
        public int NextRow { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int TotalCount { get; set; }
        #endregion
        #endregion
        #region Transactions

        public void CreateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<Meter>(dbKey);

            if (IsValid)
            {
                Meter_CreateByForeignKeys trans = new Meter_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Meter = this.ToDatabaseObject();               
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Meter);
            }
        }

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey)
        {
            Meter_RetrieveByForeignKeys trans = new Meter_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Meter = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Meter);
            trans.Meter.ClientId = this.ClientId;
            trans.Meter.MeterId = this.MeterId;
            trans.Meter.SiteId = this.SiteId;
            trans.Meter.AreaId = this.AreaId;
            trans.Meter.DepartmentId = this.DepartmentId;
            trans.Meter.StoreroomId = this.StoreroomId;
            trans.Meter.Name = this.Name;
            trans.Meter.ClientLookupId = this.ClientLookupId;
            trans.Meter.ReadingCurrent = this.ReadingCurrent;
            trans.Meter.ReadingDate = this.ReadingDate;
            trans.Meter.ReadingLife = this.ReadingLife;
            trans.Meter.ReadingMax = this.ReadingMax;
            trans.Meter.ReadingUnits = this.ReadingUnits;
            trans.Meter.ReadingBy = this.ReadingBy;
            trans.Meter.Type = this.Type;
            trans.Meter.UpdateIndex = this.UpdateIndex;
            trans.dbKey = dbKey.ToTransDbKey();
        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<Meter>(dbKey);
            if (IsValid)
            {

                Meter_UpdateByForeignKeys trans = new Meter_UpdateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Meter = this.ToDatabaseObject();

                trans.Meter.ClientId = this.ClientId;
                trans.Meter.MeterId = this.MeterId;
                trans.Meter.SiteId = this.SiteId;
                trans.Meter.AreaId = this.AreaId;
                trans.Meter.DepartmentId = this.DepartmentId;
                trans.Meter.StoreroomId = this.StoreroomId;
                trans.Meter.Name = this.Name;
                trans.Meter.ClientLookupId = this.ClientLookupId;
                trans.Meter.ReadingCurrent = this.ReadingCurrent;
                trans.Meter.ReadingDate = this.ReadingDate;
                trans.Meter.ReadingLife = this.ReadingLife;
                trans.Meter.ReadingMax = this.ReadingMax;
                trans.Meter.ReadingUnits = this.ReadingUnits;
                trans.Meter.ReadingBy = this.ReadingBy;
                trans.Meter.Type = this.Type;
                trans.Meter.UpdateIndex = this.UpdateIndex;
                trans.dbKey = dbKey.ToTransDbKey();

                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Meter);
            }
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            // 20110039 
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
            int rowid = 0;
            string filter = "";          
            
            Meter_ValidateByClientLookupId trans = new Meter_ValidateByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,               
                lulist = lulist
                // 20110039
            };
            trans.Meter = this.ToDatabaseObject();
            /**/
            trans.CreateMode = this.CreateMode;
            /**/

            trans.Meter.ClientId = this.ClientId;
            trans.Meter.MeterId = this.MeterId;
            trans.Meter.SiteId = this.SiteId;
            trans.Meter.AreaId = this.AreaId;
            trans.Meter.DepartmentId = this.DepartmentId;
            trans.Meter.StoreroomId = this.StoreroomId;
            trans.Meter.Name = this.Name;
            trans.Meter.ClientLookupId = this.ClientLookupId;
            trans.Meter.ReadingCurrent = this.ReadingCurrent;
            trans.Meter.ReadingDate = this.ReadingDate;
            trans.Meter.ReadingLife = this.ReadingLife;
            trans.Meter.ReadingMax = this.ReadingMax;
            trans.Meter.ReadingUnits = this.ReadingUnits;
            trans.Meter.ReadingBy = this.ReadingBy;
            trans.Meter.Type = this.Type;
            trans.Meter.UpdateIndex = this.UpdateIndex;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoredProcValidationError> errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationError_List);

            return errors;
        }

        public List<Meter> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Meter_RetrieveClientLookupIdBySearchCriteria trans = new Meter_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Meter = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Meter> Meters = new List<Meter>();
            foreach (b_Meter eq in trans.MeterList)
            {
                Meter tmpEq = new Meter()
                {
                    MeterId = eq.MeterId,
                    ClientLookupId = eq.ClientLookupId
                };
                Meters.Add(tmpEq);
            }

            return Meters;
        }

        public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey)
        {

            Meter_RetrieveInitialSearchConfigurationData trans = new Meter_RetrieveInitialSearchConfigurationData()
            {
                // RKL - Could we modify the dbkey.set method of abstract transaction manager to set these two?
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            SearchCriteria = trans.Search_Criteria;          
        }


        public List<Meter> RetrieveAllFromDatabase(DatabaseKey dbKey)
        {
            Meter_RetriveAllCustom trans = new Meter_RetriveAllCustom
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.User.UserName,
                ClientId = dbKey.Personnel.ClientId
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Meter> meterLst = new List<Meter>();
            Meter m;

            trans.MeterList.ForEach(x =>
            {
                m = new Meter();
                m.UpdateFromDatabaseObject(x);
                meterLst.Add(m);
            });

            return meterLst;

        }
        //-SOM 928
        public void CreateByForeignKeys(DatabaseKey dbKey)
        {
             Validate<Meter>(dbKey);

             if (IsValid)
             {
                 Meter_Create trans = new Meter_Create();
                 trans.Meter = this.ToDatabaseObject();
                 trans.dbKey = dbKey.ToTransDbKey();
                 trans.Execute();

                 // The create procedure may have populated an auto-incremented key. 
                 UpdateFromDatabaseObject(trans.Meter);
             }
        }

        //-Som 928
        public void ActiveInactiveByprimaryKey(DatabaseKey dbKey)
        {
            Meter_ActiveInactiveByPrimaryKey trans = new Meter_ActiveInactiveByPrimaryKey();
            trans.Meter = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Meter);
        }

        //-Som 928
        public void GeneratePMWorkOrders(DatabaseKey dbKey)
        {
          Meter_GeneratePMWorkOrders trans = new Meter_GeneratePMWorkOrders();
          trans.Meter = this.ToDatabaseObjectExtended();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          // The create procedure may have populated an auto-incremented key. 
          UpdateFromDatabaseObject(trans.Meter);
          this.PMWOList = trans.Meter.PMWOList;
          this.PMWOClientLookupIds = trans.Meter.PMWOClientLookupIds;//V2-784
        }

        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }
        private void Load_DateSelection()
        {

            List<KeyValuePair<string, string>> search_DateSelection = new List<KeyValuePair<string, string>>();
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.Name != "CreateDate" || pi.Name!="CreateBy")
                {
                    if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
                    {
                        search_DateSelection.Add(new KeyValuePair<string, string>(pi.Name, "Last Reading"));    // 20110024
                    }
                }

            }
            SearchCriteria.Add("dates", search_DateSelection);
        }      

        #endregion

        public static List<Meter> UpdateFromDatabaseObjectList(List<b_Meter> dbObjs)
        {
            List<Meter> result = new List<Meter>();

            foreach (b_Meter dbObj in dbObjs)
            {
                Meter tmp = new Meter();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void RetrieveByClientLookUpId(DatabaseKey dbKey)
        {
            Meter_RetrieveByClientLookUpId trans = new Meter_RetrieveByClientLookUpId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Meter = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Meter);
        }
        [Obsolete]
        public List<Meter> RetrieveForSearchBySiteAndReadingDate(DatabaseKey dbKey)
        {
            Meters_SearchBySiteAndReadingDate trans = new Meters_SearchBySiteAndReadingDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Meter = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Meter> meterList = new List<Meter>();

            foreach (b_Meter meter in trans.MeterList)
            {
                Meter tmpMeter = new Meter();
                tmpMeter.UpdateFromDatabaseObjectExtended(meter);
                meterList.Add(tmpMeter);               
                
            }
            return meterList;
        }

        public b_Meter ToDatabaseObjectExtended()
        {
            b_Meter dbObj = this.ToDatabaseObject();
            dbObj.DateRange = this.DateRange;
            dbObj.DateColumn = this.DateColumn;
            dbObj.PersonnelClientLookupId = this.PersonnelClientLookUpId;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtended(b_Meter dbObj)
        {         
            this.UpdateFromDatabaseObject(dbObj);
            this.DateRange = dbObj.DateRange;
            this.DateColumn = dbObj.DateColumn;
            this.PersonnelClientLookUpId = dbObj.PersonnelClientLookupId;
            this.PersonnelId = dbObj.PersonnelId;
        }

        public b_Meter ToDatabaseObjectExtendedReadingDateV2()
        {
            b_Meter dbObj = new b_Meter();
            dbObj.ClientId = this.ClientId;           
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtendedReadingDateV2(b_Meter dbObj)
        {

            this.ClientId = dbObj.ClientId;
            this.MeterId = dbObj.MeterId;
            this.SiteId = dbObj.SiteId;           
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.ReadingCurrent = dbObj.ReadingCurrent;
            this.ReadingDate = dbObj.ReadingDate;
            this.ReadingLife = dbObj.ReadingLife;
            this.ReadingMax = dbObj.ReadingMax;
            this.ReadingUnits = dbObj.ReadingUnits;
            this.ReadingBy = dbObj.ReadingBy;
            this.Type = dbObj.Type;          
            this.PersonnelClientLookUpId = dbObj.PersonnelClientLookupId;            
        }
        #region Properties

        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string PersonnelClientLookUpId { get; set; }
        #endregion

        #region V2
        public List<Meter> RetrieveForSearchBySiteAndReadingDate_V2(DatabaseKey dbKey)
        {
            Meters_SearchBySiteAndReadingDate_V2 trans = new Meters_SearchBySiteAndReadingDate_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
        trans.Meter = this.ToDatabaseObjectExtendedReadingDateV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Meter> meterList = new List<Meter>();

            foreach (b_Meter meter in trans.MeterList)
            {
                Meter tmpMeter = new Meter();
                tmpMeter.UpdateFromDatabaseObjectExtendedReadingDateV2(meter);
                meterList.Add(tmpMeter);                
            }
            return meterList;
        }
        #endregion

        #region V2-950
        public List<Meter> RetrieveForTableLookupList_V2(DatabaseKey dbKey)
        {
            Meters_RetrieveForTableLookupList_V2 trans = new Meters_RetrieveForTableLookupList_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Meter = this.ToDatabaseObjectRetrieveForTableLookupListV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Meter> meterList = new List<Meter>();

            foreach (b_Meter meter in trans.MeterList)
            {
                Meter tmpMeter = new Meter();
                tmpMeter.UpdateFromDatabaseObjectRetrieveForTableLookupListV2(meter);
                meterList.Add(tmpMeter);                
            }
            return meterList;
        }
        public b_Meter ToDatabaseObjectRetrieveForTableLookupListV2()
        {
            b_Meter dbObj = new b_Meter();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ClientLookupId= this.ClientLookupId;
            dbObj.Name = this.Name;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy= this.OrderBy;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectRetrieveForTableLookupListV2(b_Meter dbObj)
        {

            this.ClientId = dbObj.ClientId;
            this.MeterId = dbObj.MeterId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Name = dbObj.Name;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.ReadingCurrent = dbObj.ReadingCurrent;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
    }

}
