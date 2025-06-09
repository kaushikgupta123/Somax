/*
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
 * Date        Task ID   Person             Description
 * =========== ======== =================== =======================================================
 * 2012-Feb-02 20120001 Roger Lawton        Changed to support additional columns on search
**************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts.LookupListResultSet
{
    public class LocationLookupListTransactionParameters
    {
        public LocationLookupListTransactionParameters()
        {
            ClientLookupId = "";
            Name = "";
            SiteId = 0;
        }

        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }

    public class LocationLookupListResultSet 
    {
        // Making the Items list public allows this class to be used by other processes that do not databind, such as PDF report generation.
        public List<Location> Items;

        private int CountValue;
        private int StartingIndex;
        private LocationLookupListTransactionParameters settings;
        private DatabaseKey key;

        public LocationLookupListResultSet()
        {
            Items = new List<Location>();
            resultsUpdated = false;
        }

        private bool resultsUpdated;

        public void UpdateSettings(DatabaseKey dbKey, LocationLookupListTransactionParameters parameters)
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
        [Obsolete]
        public void RetrieveResultsAll()
        {
            settings.PageNumber = 0;
            settings.ResultsPerPage = 1000000; // Limit to first 1,000,000 results. Note that SQL doesn't like int.MaxValue.
            RetrieveResults();
        }

        [Obsolete]
        public void RetrieveResults()
        {
            Location_RetrieveLookupListBySearchCriteria trans = new Location_RetrieveLookupListBySearchCriteria()
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

            foreach (b_Location l in trans.LocationList)
            {
                Location loc = new Location();
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

        //public event EventHandler<ServerModeExceptionThrownEventArgs> ExceptionThrown;

        //public int FindIncremental(CriteriaOperator expression, string value, int startIndex, bool searchUp, bool ignoreStartRow, bool allowLoop)
        //{
        //    throw new NotImplementedException();
        //}

        //public System.Collections.IList GetAllFilteredAndSortedRows()
        //{
        //    throw new NotImplementedException();
        //}

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
        //  throw new NotImplementedException();
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
        //  throw new NotImplementedException();
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
            Location_RetrieveLookupListBySearchCriteria_V2 trans = new Location_RetrieveLookupListBySearchCriteria_V2()
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

            foreach (b_Location l in trans.LocationList)
            {
                Location loc = new Location();
                loc.UpdateFromDatabaseObjectRetrieveLookupListBySearchCriteriaV2(l);
                Items.Add(loc);
            }
            resultsUpdated = true;
        }
        #endregion
    }
}
