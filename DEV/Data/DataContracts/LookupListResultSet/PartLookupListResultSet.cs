using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Business;
using Data.DataContracts;
using Data.Database;
using Data.Database.Business;

namespace DataContracts.LookupListResultSet
{
    public class PartLookupListTransactionParameters
    {
        public PartLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Description = "";
            SiteId = 0;
            PartId = "";
            Manufacturer = "";
            ManufacturerId = "";
            StockType = "";

        }

        public string PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        public string Manufacturer { get; set; }  //Developed by INT .Update  -1
        public string ManufacturerId { get; set; }  //Developed by INT .Update  -1

        public string StockType { get; set; }  //Developed by INT .Update  -1
        public string UPCCode { get; set; }  //Developed by INT .Update  -1
                                             // public string Location1_1 { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class PrevMaintLibraryLookupListResultSetTransactionParameters
    {
        public PrevMaintLibraryLookupListResultSetTransactionParameters()
        {
            ClientLookupId = "";
            Description = "";
        }

        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }
    public class ManufacturerLookupListResultSetTransactionParameters
    {
        public ManufacturerLookupListResultSetTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
        }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class PartLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Part> Items;

        private int CountValue;
        private int StartingIndex;
        private PartLookupListTransactionParameters settings;
        private DatabaseKey key;

        public PartLookupListResultSet()
        {
            Items = new List<Part>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, PartLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            Part_RetrieveLookupListBySearchCriteria trans = new Part_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                PartId = settings.PartId,
                ClientLookupId = settings.ClientLookupId,
                /* Developed by INT .Update  -1*/
                Manufacturer = settings.Manufacturer,
                ManufacturerId = settings.ManufacturerId,
                StockType = settings.StockType,
                UPCCode = settings.UPCCode,
                // Location1_1=settings.Location1_1,
                /*End*/
                Description = settings.Description,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Part p in trans.PartList)
            {
                Part part = new Part();
                part.UpdateFromDatabaseObject(p);
                Items.Add(part);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }

    public class VendorLookupListTransactionParameters
    {
        public VendorLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
            SiteId = 0;
        }

        public string PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        //V2-759
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
    }

    public class EquipmentLookupListTransactionParameters
    {
        public EquipmentLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
            Model = "";

            SiteId = 0;
            /*Subhajit 06-29*/
            Type = "";
            SerialNumber = "";
        }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        //Subhajit 06-29
        public string Type { get; set; }
        public string SerialNumber { get; set; }

    }

    public class FleetAssetLookupListTransactionParameters
    {
        public FleetAssetLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
            Model = "";

            SiteId = 0;
           
            Make = "";
            VIN = "";
        }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
             
        public string Make { get; set; }
        public string VIN { get; set; }

    }

    public class PartMasterNumberTransactionParameters
    {
        public PartMasterNumberTransactionParameters()
        {
            PartMasterId = "";
            ClientLookupId = "";
            LongDescription = "";
        }

        public string ClientLookupId { get; set; }
        public string PartMasterId { get; set; }
        public string LongDescription { get; set; }
        public Int64 SiteId { get; set; }
        public Int64 ClientId { get; set; }
        public string RequestType { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class PartMasterSomaxPartLookupTransactionParameters
    {
        public PartMasterSomaxPartLookupTransactionParameters()
        {
            ClientId = 0;
            SiteId = 0;
            PartMasterId = "";
            ClientLookupId = "";
        }

        public string ClientLookupId { get; set; }
        public string PartMasterId { get; set; }
        public string Description { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 SiteId { get; set; }
        public string RequestType { get; set; }


        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        
    }

    public class ManufactureLookupListResultSetTransactionParameters
    {
        public ManufactureLookupListResultSetTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
        }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class AccountLookupListTransactionParameters
    {
        public AccountLookupListTransactionParameters()
        {
            //PersonnelId = "";
            ClientLookupId = "";
            Name = "";
            SiteId = 0;
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class FleetIssuesLookupListTransactionParameters
    {
        public FleetIssuesLookupListTransactionParameters()
        {
            Date = "";
            Description = "";
            Status = "";
            SiteId = 0;
            EquipmentId = 0;
            Defects = "";
            
        }        
        public string Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public long SiteId { get; set; }
        public long EquipmentId { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        public string Defects { get; set; }
       

    }

    public class AssetAvailibilityLogLookupListTransactionParameters
    {
        public AssetAvailibilityLogLookupListTransactionParameters()
        {
            ObjectId = 0;
            TransactionDate = "";
            Event = "";
            ReturnToService = "";
            SiteId = 0;
            Reason = "";
            ReasonCode = "";
            PersonnelName = "";

        }
        public long ObjectId { get; set; }
        public string TransactionDate { get; set; }
        public string Event { get; set; }
        public string ReturnToService { get; set; }
        public long SiteId { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        public string Reason { get; set; }
        public string ReasonCode { get; set; }

        public string PersonnelName { get; set; }


    }


    #region PreventiveMaintenanceLookupList
    public class PrevMaintLibraryLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<PrevMaintLibrary> Items;

        private int CountValue;
        private int StartingIndex;
        private PrevMaintLibraryLookupListResultSetTransactionParameters settings;
        private DatabaseKey key;

        public PrevMaintLibraryLookupListResultSet()
        {
            Items = new List<PrevMaintLibrary>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, PrevMaintLibraryLookupListResultSetTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            PrevMaintLibrary_RetrieveLookupListBySearchCriteria trans = new PrevMaintLibrary_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                ClientLookupId = settings.ClientLookupId,
                Description = settings.Description,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_PrevMaintLibrary pl in trans.PrevMaintLibraryList)
            {
                PrevMaintLibrary loc = new PrevMaintLibrary();
                loc.UpdateFromDatabaseObject(pl);
                Items.Add(loc);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

       // public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }

    #endregion PreventiveMaintenanceLookupList


    #region ManufacturerLookupList
    public class ManufacturerLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<ManufacturerMaster> Items;

        private int CountValue;
        private int StartingIndex;
        private ManufacturerLookupListResultSetTransactionParameters settings;
        private DatabaseKey key;

        public ManufacturerLookupListResultSet()
        {
            Items = new List<ManufacturerMaster>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, ManufacturerLookupListResultSetTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            ManufacturerMaster_RetrieveLookupListBySearchCriteria trans = new ManufacturerMaster_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_ManufacturerMaster pl in trans.ManufacturerMasterList)
            {
                ManufacturerMaster loc = new ManufacturerMaster();
                loc.UpdateFromDatabaseObject(pl);
                Items.Add(loc);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        // public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion ManufacturerLookupList

    #region VendorLookupList
    public class VendorLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Vendor> Items;

        private int CountValue;
        private int StartingIndex;
        private VendorLookupListTransactionParameters settings;
        private DatabaseKey key;

        public VendorLookupListResultSet()
        {
            Items = new List<Vendor>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, VendorLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults_V2();
        }

        [Obsolete]
        public void RetrieveResults()
        {
            Vendor_RetrieveLookupListBySearchCriteria trans = new Vendor_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Vendor v in trans.VendorList)
            {
                Vendor vendor = new Vendor();
                vendor.UpdateFromDatabaseObject(v);
                Items.Add(vendor);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

       // public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults_V2();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults_V2();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region V2
        public void RetrieveResults_V2()
        {
            Vendor_RetrieveLookupListBySearchCriteria_V2 trans = new Vendor_RetrieveLookupListBySearchCriteria_V2()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Vendor v in trans.VendorList)
            {
                Vendor vendor = new Vendor();
                vendor.UpdateFromDatabaseObjectRetrieveLookupListBySearchCriteriaV2(v);
                vendor.totalCount = trans.ResultCount;
                Items.Add(vendor);
            }
            resultsUpdated = true;
        }
        #endregion
    }


    public class VendorInternalLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Vendor> Items;

        private int CountValue;
        private int StartingIndex;
        private VendorLookupListTransactionParameters settings;
        private DatabaseKey key;

        public VendorInternalLookupListResultSet()
        {
            Items = new List<Vendor>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, VendorLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }
        public void RetrieveResults()
        {
            Vendor_RetrieveLookupListBySearchCriteriaAndInternal_V2 trans = new Vendor_RetrieveLookupListBySearchCriteriaAndInternal_V2()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Vendor v in trans.VendorList)
            {
                Vendor vendor = new Vendor();
                vendor.UpdateFromDatabaseObject(v);
                Items.Add(vendor);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        // public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    public class PunchOutVendorInternalLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Vendor> Items;

        private int CountValue;
        private int StartingIndex;
        private VendorLookupListTransactionParameters settings;
        private DatabaseKey key;

        public PunchOutVendorInternalLookupListResultSet()
        {
            Items = new List<Vendor>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, VendorLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            PunchOutVendor_RetrieveLookupListBySearchCriteria trans = new PunchOutVendor_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                AddressCity= settings.AddressCity,
                AddressState= settings.AddressState,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Vendor v in trans.VendorList)
            {
                Vendor vendor = new Vendor();
                vendor.UpdateFromDatabaseObject(v);
                Items.Add(vendor);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        // public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }

    #endregion
    #region AccountLookupList


    public class AccountLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Account> Items;

        private int CountValue;
        private int StartingIndex;
        private AccountLookupListTransactionParameters settings;
        private DatabaseKey key;

        public AccountLookupListResultSet()
        {
            Items = new List<Account>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, AccountLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            Account_RetrieveLookupListBySearchCriteria trans = new Account_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Account a in trans.AccountList)
            {
                Account account = new Account();
                account.UpdateFromDatabaseObject(a);
                Items.Add(account);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

     
        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

      
        public void Refresh()
        {
            throw new NotImplementedException();
        }

     

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

     

        #endregion
    }

    #endregion

    #region WorkOrderLookupList

    public class WorkOrderLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<WorkOrder> Items;

        private int CountValue;
        private int StartingIndex;
        private WorkOrderLookupListTransactionParameters settings;
        private DatabaseKey key;

        public WorkOrderLookupListResultSet()
        {
            Items = new List<WorkOrder>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, WorkOrderLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            WorkOrder_RetrieveLookupListBySearchCriteria trans = new WorkOrder_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Description = settings.Description,
                ChargeTo_Name = settings.ChargeTo_Name,
                WorkAssigned_Name = settings.WorkAssigned_Name,
                Requestor_Name = settings.Requestor_Name,
                Status = settings.Status,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_WorkOrder w in trans.WorkOrderList)
            {
                WorkOrder workorder = new WorkOrder();
                workorder.UpdateFromDatabaseObject(w);
                Items.Add(workorder);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }


        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }


        public void Refresh()
        {
            throw new NotImplementedException();
        }



        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }



        #endregion
    }

    public class WorkOrderLookupListResultSet_V2 //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<WorkOrder> Items;

        private int CountValue;
        private int StartingIndex;
        private WorkOrderLookupListTransactionParameters settings;
        private DatabaseKey key;

        public WorkOrderLookupListResultSet_V2()
        {
            Items = new List<WorkOrder>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, WorkOrderLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            WorkOrder_RetrieveLookupListBySearchCriteria_V2 trans = new WorkOrder_RetrieveLookupListBySearchCriteria_V2()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Description = settings.Description,
                ChargeTo_Name = settings.ChargeTo_Name,
                WorkAssigned_Name = settings.WorkAssigned_Name,
                Requestor_Name = settings.Requestor_Name,
                Status = settings.Status,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_WorkOrder w in trans.WorkOrderList)
            {
                WorkOrder workorder = new WorkOrder();              
                workorder.WorkOrderId = w.WorkOrderId;
                workorder.ClientLookupId = w.ClientLookupId;
                workorder.ChargeTo_Name = w.ChargeTo_Name;
                workorder.Description = w.Description;
                workorder.Status = w.Status;
                workorder.Type = w.Type;
                workorder.WorkAssigned_Name = w.WorkAssigned_Name;
                workorder.Requestor_Name = w.Requestor_Name;

                Items.Add(workorder);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }


        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }


        public void Refresh()
        {
            throw new NotImplementedException();
        }



        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }



        #endregion
    }

    public class WorkOrderLookupListTransactionParameters
    {
        public WorkOrderLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Description = "";
            SiteId = 0;
        }

        public string WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }



    #endregion

    #region EquipmentLookupList
    public class EquipmentLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Equipment> Items;

        private int CountValue;
        private int StartingIndex;
        private EquipmentLookupListTransactionParameters settings;
        private DatabaseKey key;

        public EquipmentLookupListResultSet()
        {
            Items = new List<Equipment>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, EquipmentLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            Equipment_RetrieveLookupListBySearchCriteria trans = new Equipment_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                Model = settings.Model,
                Type = settings.Type,
                SerialNumber = settings.SerialNumber,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Equipment e in trans.EquipmentList)
            {
                Equipment eq = new Equipment();
                eq.UpdateFromDatabaseObject(e);
                Items.Add(eq);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion

    #region PartMasterLookupList
    public class PartMasterNumberResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<PartMaster> Items;

        private int CountValue;
        private int StartingIndex;
        private PartMasterNumberTransactionParameters settings;
        private DatabaseKey key;

        public PartMasterNumberResultSet()
        {
            Items = new List<PartMaster>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, PartMasterNumberTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            PartMaster_RetrieveNumberLookup trans = new PartMaster_RetrieveNumberLookup()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName

            };
            trans.PMaster = new b_PartMaster();
            trans.PMaster.ClientId = settings.ClientId;
            trans.PMaster.SiteId = settings.SiteId;
            trans.PMaster.RequestType = settings.RequestType;
            trans.PMaster.ClientLookupId = settings.ClientLookupId;
            trans.PMaster.LongDescription = settings.LongDescription;
            trans.PMaster.pageNumber = settings.PageNumber;
            trans.PMaster.resultsPerPage = settings.ResultsPerPage;
            trans.PMaster.orderColumn = settings.OrderColumn ?? "";
            trans.PMaster.orderDirection = settings.OrderDirection ?? "";
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_PartMaster e in trans.PartMasterList)
            {
                PartMaster pm = new PartMaster();
                pm.UpdateFromDatabaseObject(e);
                Items.Add(pm);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion

    #region PartToReplaceLookupList
    public class PartMasterSomaxPartLookupResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<PartMaster> Items;

        private int CountValue;
        private int StartingIndex;
        private PartMasterSomaxPartLookupTransactionParameters settings;
        private DatabaseKey key;

        public PartMasterSomaxPartLookupResultSet()
        {
            Items = new List<PartMaster>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, PartMasterSomaxPartLookupTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            PartMaster_RetrieveSomaxPartLookup trans = new PartMaster_RetrieveSomaxPartLookup()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName

            };
            trans.PMaster = new b_PartMaster();
            trans.PMaster.ClientId = settings.ClientId;
            trans.PMaster.SiteId = settings.SiteId;
            trans.PMaster.RequestType = settings.RequestType;
            trans.PMaster.ClientLookupId = settings.ClientLookupId;
            trans.PMaster.Description = settings.Description;
            trans.PMaster.pageNumber = settings.PageNumber;
            trans.PMaster.resultsPerPage = settings.ResultsPerPage;
            trans.PMaster.orderColumn = settings.OrderColumn ?? "";
            trans.PMaster.orderDirection = settings.OrderDirection ?? "";
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_PartMaster e in trans.PartMasterList)
            {
                PartMaster pm = new PartMaster();
                pm.UpdateFromDatabaseObject(e);
                pm.Description = e.Description;

                Items.Add(pm);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

       // public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

       // public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion

    #region ManufacturerLookupList
    public class ManufactureLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<ManufacturerMaster> Items;

        private int CountValue;
        private int StartingIndex;
        private ManufactureLookupListResultSetTransactionParameters settings;
        private DatabaseKey key;

        public ManufactureLookupListResultSet()
        {
            Items = new List<ManufacturerMaster>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, ManufactureLookupListResultSetTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            ManufacturerMaster_RetrieveLookupListBySearchCriteria trans = new ManufacturerMaster_RetrieveLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();

            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_ManufacturerMaster l in trans.ManufacturerMasterList)
            {
                ManufacturerMaster loc = new ManufacturerMaster();
                loc.UpdateFromDatabaseObject(l);
                Items.Add(loc);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

       // public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion

    #region FleetAssetLookupList
    public class FleetAssetLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Equipment> Items;

        private int CountValue;
        private int StartingIndex;
        private FleetAssetLookupListTransactionParameters settings;
        private DatabaseKey key;

        public FleetAssetLookupListResultSet()
        {
            Items = new List<Equipment>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, FleetAssetLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            Equipment_FleetLookupListBySearchCriteria trans = new Equipment_FleetLookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ClientLookupId = settings.ClientLookupId,
                Name = settings.Name,
                Model = settings.Model,
                //Type = settings.Type,
                //SerialNumber = settings.SerialNumber,
                Make = settings.Make,
                VIN = settings.VIN,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();            
            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_Equipment e in trans.EquipmentList)
            {
                Equipment eq = new Equipment();
                eq.UpdateFromDatabaseObject(e);
                
                Items.Add(eq);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion


    #region FleetIssuesLookupList
    public class FleetIssuesLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<FleetIssues> Items;

        private int CountValue;
        private int StartingIndex;
        private FleetIssuesLookupListTransactionParameters settings;
        private DatabaseKey key;

        public FleetIssuesLookupListResultSet()
        {
            Items = new List<FleetIssues>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, FleetIssuesLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            FleetIssue_LookupListBySearchCriteria trans = new FleetIssue_LookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                EquipmentId=settings.EquipmentId,
                Date = settings.Date,
                Description = settings.Description,
                Status = settings.Status,                
                Defects = settings.Defects,                
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();
            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_FleetIssues e in trans.FleetIssuesList)
            {
                FleetIssues FI = new FleetIssues();
                FI.UpdateFromDatabaseObject(e);

                Items.Add(FI);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion


    #region FleetAssetLookupList
    public class AssetAvailabilityLogLookupListResultSet //: IListServer
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<AssetAvailabilityLog> Items;

        private int CountValue;
        private int StartingIndex;
        private AssetAvailibilityLogLookupListTransactionParameters settings;
        private DatabaseKey key;

        public AssetAvailabilityLogLookupListResultSet()
        {
            Items = new List<AssetAvailabilityLog>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, AssetAvailibilityLogLookupListTransactionParameters parameters)
        {
            key = dbKey;

            if (settings != null)
            {
                // Previous invocations may have set the order column & direction already through the Apply() method.
                // If so, keep those values unless the calling mechanism has specified them explicitly.
                parameters.OrderColumn = (parameters.OrderColumn ?? settings.OrderColumn);
                parameters.OrderDirection = (parameters.OrderDirection ?? settings.OrderDirection);
            }

            settings = parameters;
            Items.Clear();
        }

        /// <summary>
        /// The RetrieveResultsAll method quickly allows for returning full result set by requesting a single page with all values.
        /// </summary>
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        public void RetrieveResults()
        {
            AssetAvailabilityLog_LookupListBySearchCriteria trans = new AssetAvailabilityLog_LookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ObjectId = settings.ObjectId,
                TransactionDate = settings.TransactionDate,
                Event = settings.Event,
                ReturnToService = settings.ReturnToService,
                Reason = settings.Reason,
                ReasonCode = settings.ReasonCode,
                PersonnelName = settings.PersonnelName,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();
            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_AssetAvailabilityLog a in trans.AssetAvailabilityLogList)
            {
                AssetAvailabilityLog aal = new AssetAvailabilityLog();
                aal.UpdateFromDatabaseObjectAssetAvailabilityLog_V2(a);

                Items.Add(aal);
            }
            resultsUpdated = true;
        }

        public void RetrieveAvailabilityLogResults()
        {
            AssetAvailabilityLog_LookupListBySearchCriteria trans = new AssetAvailabilityLog_LookupListBySearchCriteria()
            {
                CallerUserInfoId = key.User.UserInfoId,
                CallerUserName = key.UserName,
                SiteId = settings.SiteId,
                ObjectId = settings.ObjectId,
                TransactionDate = settings.TransactionDate,
                Event = settings.Event,
                ReturnToService = settings.ReturnToService,
                Reason = settings.Reason,
                ReasonCode = settings.ReasonCode,
                PersonnelName = settings.PersonnelName,
                PageNumber = settings.PageNumber,
                ResultsPerPage = settings.ResultsPerPage,
                OrderColumn = settings.OrderColumn ?? "",
                OrderDirection = settings.OrderDirection ?? ""
            };
            trans.dbKey = key.ToTransDbKey();
            trans.Execute();
            CountValue = trans.ResultCount;
            StartingIndex = settings.PageNumber * settings.ResultsPerPage;

            foreach (b_AssetAvailabilityLog a in trans.AssetAvailabilityLogList)
            {
                AssetAvailabilityLog aal = new AssetAvailabilityLog();
                aal.UpdateFromDatabaseObject(a);

                Items.Add(aal);
            }
            resultsUpdated = true;
        }

        #region IListServer Members

        /// <summary>
        /// This method is invoked whenever the sorting order is changed (by clicking on the column header). The information received here should be persisted so that it can be 
        /// referenced by the "public object this[int index]" method.
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <param name="sortInfo"></param>
        /// <param name="groupCount"></param>
        /// <param name="groupSummaryInfo"></param>
        /// <param name="totalSummaryInfo"></param>
        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    //throw new NotImplementedException();

        //    if (sortInfo != null)
        //    {
        //        foreach (ServerModeOrderDescriptor sorter in sortInfo)
        //        {
        //            // Note: We're getting the result in the format "[column] direction". 
        //            // We'll need to break this down to set properly
        //            string[] parts = sorter.ToString().Split(']');
        //            parts[0] = parts[0].Replace("[", "").Trim();
        //            parts[1] = parts[1].Trim();

        //            settings.OrderColumn = parts[0];
        //            settings.OrderDirection = parts[1];
        //            resultsUpdated = false;
        //        }
        //    }
        //}

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        public System.Collections.IList GetAllFilteredAndSortedRows()
        {
            throw new NotImplementedException();
        }

        //public List<ListSourceGroupInfo> GetGroupInfo(ListSourceGroupInfo parentGroup)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetRowIndexByKey(object key)
        {
            throw new NotImplementedException();
        }

        public object GetRowKey(int index)
        {
            throw new NotImplementedException();
        }

        public List<object> GetTotalSummary()
        //        public Dictionary<object, object> GetTotalSummary()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator expression, int maxCount, bool includeFilteredOut)
        //{
        //    throw new NotImplementedException();
        //}

        //public event EventHandler<ServerModeInconsistencyDetectedEventArgs> InconsistencyDetected;

        //public int LocateByExpression(CriteriaOperator expression, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        //public int LocateByValue(CriteriaOperator expression, object value, int startIndex, bool searchUp)
        //{
        //    throw new NotImplementedException();
        //}

        public void Refresh()
        {
            throw new NotImplementedException();
        }
        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch)
        //{
        //  throw new NotImplementedException();
        //}

        //public bool PrefetchRows(ListSourceGroupInfo[] groupsToPrefetch, System.Threading.CancellationToken t)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is invoked to retrieve the individual element at the selected index. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// In addition, note that the index can be any value from 0 to N-1, where N is the number returned by
        /// the Count method, but the JIT method should only return values that will be expected in this call.
        /// The Count() method should also populate the expected result set so that it can be accessed here.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (!resultsUpdated || (index >= (StartingIndex + Items.Count) || index < StartingIndex))
                {
                    Items.Clear();
                    settings.PageNumber = (index / settings.ResultsPerPage);

                    RetrieveResults();

                    //if (Items.Count < settings.ResultsPerPage)
                    //{
                    //    if (Items.Count < CountValue) { CountValue = Items.Count; }
                    //}

                    StartingIndex = index;
                }

                if (Items.Count <= (index - StartingIndex)) { return null; }
                return Items[index - StartingIndex];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns the total number of elements in the result set. This class must 
        /// be properly written to load the data in a just-in-time manner the first time it is called, 
        /// and the loaded information must persist so that it is not re-retrieved on additional invokations. 
        /// 
        /// The Count() method should also populate the expected result set so that it can be accessed by the this() property.
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                if (!resultsUpdated)
                {
                    RetrieveResults();
                }

                return CountValue;
            }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        //public object[] GetUniqueColumnValues(CriteriaOperator valuesExpression, int maxCount, CriteriaOperator filterExpression, bool ignoreAppliedFilter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Apply(CriteriaOperator filterCriteria, ICollection<ServerModeOrderDescriptor[]> sortInfo, int groupCount, ICollection<ServerModeSummaryDescriptor> groupSummaryInfo, ICollection<ServerModeSummaryDescriptor> totalSummaryInfo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
    #endregion

}
