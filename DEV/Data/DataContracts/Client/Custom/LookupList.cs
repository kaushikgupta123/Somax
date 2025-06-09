/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* NOTES
* 2014-Nov-23 - This method should NOT be needed - Lookup lists should not have to be retrieved 
*               by callbacks as they should be retrieved completely the first time  
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =========================================================
* 2011-Nov-29 201100000 Roger Lawton       Added RetrieveList method
* 2014-Aug-02 SOM-263   Roger Lawton       Added Additional Information
* 2014-Nov-23 SOM-453   Roger Lawton       Modified Retrieve All method to use new txn that retrieves
*                                          Active items only (inactiveflag = 0)
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class LookupList:DataContractBase
    {

        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }

        #region Transaction Methods

        public List<LookupList> RetrieveList(DatabaseKey dbKey, string listname, string listfilter, long ParentSiteID, string txtSearch)
        {
            RetrieveLookupList trans = new RetrieveLookupList()
            {
                //CallerUserInfoId = dbKey.User.UserInfoId,
                //CallerUserName = dbKey.UserName,
                listfilter = listfilter,
                listname = listname,
                ParentSiteID = ParentSiteID,
                txtSearch = txtSearch
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<b_LookupList> lookup = trans.result;
            List<LookupList> result = new List<LookupList>();     
            foreach(b_LookupList li in lookup)
            {
              LookupList temp = new LookupList();
              temp.UpdateFromDatabaseObjectExtended(li);
              result.Add(temp);
            }
            return result;
        }

        // SOM-453
        public List<LookupList> RetrieveAll(DatabaseKey dbKey)
        {
            //LookupList_RetrieveAll trans = new LookupList_RetrieveAll()
            LookupList_RetrieveAllActive trans = new LookupList_RetrieveAllActive()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName           
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<b_LookupList> lookup = trans.LookupListList;

            List<LookupList> result = new List<LookupList>();

            foreach (b_LookupList li in lookup)
            {
                LookupList temp = new LookupList()
                {
                    ClientId = li.ClientId,
                    AreaId = li.AreaId,
                    DepartmentId = li.DepartmentId,
                    LookupListId = li.LookupListId,
                    SiteId = li.SiteId,
                    Description = li.Description,
                    ListName = li.ListName,
                    ListValue = li.ListValue,
                    Filter = li.Filter
                };
                result.Add(temp);
            }
            return result;

        }

        public List<KeyValuePair<string,string>> RetrieveLookupListByFilterText(DatabaseKey dbKey)
        {
            RetrieveLookupListByFilterText trans = new RetrieveLookupListByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LookupList = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

          //  List<KeyValuePair<string, string>> lookup = trans.RetLookUpList;



            return(trans.RetLookUpList);

        }

        public void UpdateFromDatabaseObjectExtended(b_LookupList dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
        }
        public b_LookupList ToDatabaseObjectExtended()
        {
            b_LookupList dbObj = this.ToDatabaseObject();
            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;
            return dbObj;
        }

/*
        public List<LookupList> RetrieveList(string connectionString, string listName, string filter)
        {
            List<LookupList> result = new List<LookupList>();
            int i = 0;

            switch (listName.ToLower())
            {
                case "equip type":
                    result.Add(get(i++, "AIR CONDITIONER", "AIR CONDITIONER", ""));
                    result.Add(get(i++, "BAG TYER", "BAG TYER", ""));
                    result.Add(get(i++, "BAGGER", "BAGGER", ""));
                    result.Add(get(i++, "BOILER", "BOILER", ""));
                    result.Add(get(i++, "BULK DELIVERY", "BULK DELIVERY", ""));
                    break;
                case "unit measure":
                    result.Add(get(i++, "Bag", "BAG", ""));
                    result.Add(get(i++, "Barrel", "BRL", ""));
                    result.Add(get(i++, "Box", "BOX", ""));
                    result.Add(get(i++, "Case", "CS", ""));
                    result.Add(get(i++, "Centimeter", "CM", ""));
                    result.Add(get(i++, "Cycles", "CYCL", ""));
                    result.Add(get(i++, "Degrees Centigrade", "DEGC", ""));
                    result.Add(get(i++, "Degrees Farenheight", "DEGF", ""));
                    result.Add(get(i++, "Dozen", "DOZ", ""));
                    result.Add(get(i++, "Each", "EACH", ""));
                    result.Add(get(i++, "Feet", "FEET", ""));
                    result.Add(get(i++, "Gallon", "GAL", ""));
                    result.Add(get(i++, "Gross (144)", "GROS", ""));
                    result.Add(get(i++, "Hours", "HRS", ""));
                    result.Add(get(i++, "Imperial Gallon", "IGAL", ""));
                    result.Add(get(i++, "Inch", "IN", ""));
                    result.Add(get(i++, "Liter", "LTR", ""));
                    result.Add(get(i++, "Meter", "ME", ""));
                    result.Add(get(i++, "Miles", "MILE", ""));
                    result.Add(get(i++, "Milliliter", "ML", ""));
                    result.Add(get(i++, "Millimeter", "MM", ""));
                    result.Add(get(i++, "Minutes", "MINS", ""));
                    result.Add(get(i++, "Ounce", "OZ", ""));
                    result.Add(get(i++, "Pair", "PR", ""));
                    result.Add(get(i++, "Per Hundred", "C", ""));
                    result.Add(get(i++, "Pint", "PT", ""));
                    result.Add(get(i++, "Pound", "POUNDS", ""));
                    result.Add(get(i++, "Pounds", "POUNDS", ""));
                    result.Add(get(i++, "Quart", "QT", ""));
                    result.Add(get(i++, "Roll", "RL", ""));
                    result.Add(get(i++, "Rpm", "RPM", ""));
                    result.Add(get(i++, "Set", "SET", ""));
                    result.Add(get(i++, "Skid", "SKID", ""));
                    result.Add(get(i++, "Ton", "TON", ""));
                    result.Add(get(i++, "Yard", "YD", ""));
                    break;
                case "alpha":
                    result.Add(get(i++, "alpha", "alpha", "alpha-one", ""));
                    result.Add(get(i++, "alpha", "alpha", "alpha-two", ""));
                    result.Add(get(i++, "alpha", "alpha", "alpha-three", ""));
                    result.Add(get(i++, "alpha", "alpha", "alpha-four", ""));
                    break;
                case "beta":
                    result.Add(get(i++, "beta", "beta", "beta-one", ""));
                    result.Add(get(i++, "beta", "beta", "beta-two", ""));
                    result.Add(get(i++, "beta", "beta", "beta-three", ""));
                    result.Add(get(i++, "beta", "beta", "beta-four", ""));
                    result.Add(get(i++, "beta", "beta", "beta-five", ""));
                    break;
                case "delta":
                    result.Add(get(i++, "delta", "delta", "delta-one", ""));
                    result.Add(get(i++, "delta", "delta", "delta-two", ""));
                    result.Add(get(i++, "delta", "delta", "delta-three", ""));
                    result.Add(get(i++, "delta", "delta", "delta-four", ""));
                    result.Add(get(i++, "delta", "delta", "delta-five", ""));
                    break;
                case "gamma":
                    switch (filter.ToLower())
                    {


                        case "delta-one":
                            result.Add(get(i++, "gamma", "gamma", "1-gamma-delta-one-a", "delta-one"));
                            result.Add(get(i++, "gamma", "gamma", "2-gamma-delta-one-b", "delta-one"));
                            break;
                        case "delta-two":
                            result.Add(get(i++, "gamma", "gamma", "3-gamma-delta-two-a", "delta-two"));
                            result.Add(get(i++, "gamma", "gamma", "4-gamma-delta-two-c", "delta-two"));
                            result.Add(get(i++, "gamma", "gamma", "5-gamma-delta-two-e", "delta-two"));
                            break;
                        case "delta-three":
                            result.Add(get(i++, "gamma", "gamma", "6-gamma-delta-three-a", "delta-three"));
                            result.Add(get(i++, "gamma", "gamma", "7-gamma-delta-three-b", "delta-three"));
                            result.Add(get(i++, "gamma", "gamma", "8-gamma-delta-three-c", "delta-three"));
                            result.Add(get(i++, "gamma", "gamma", "9-gamma-delta-three-d", "delta-three"));
                            break;
                        case "delta-four":
                            result.Add(get(i++, "gamma", "gamma", "1-gamma-delta-four-b", "delta-four"));
                            result.Add(get(i++, "gamma", "gamma", "2-gamma-delta-four-d", "delta-four"));
                            break;
                        case "delta-five":
                            result.Add(get(i++, "gamma", "gamma", "a-gamma-delta-one-a", "delta-one"));
                            result.Add(get(i++, "gamma", "gamma", "b-gamma-delta-one-b", "delta-one"));
                            result.Add(get(i++, "gamma", "gamma", "c-gamma-delta-one-c", "delta-one"));
                            result.Add(get(i++, "gamma", "gamma", "d-gamma-delta-one-d", "delta-one"));
                            result.Add(get(i++, "gamma", "gamma", "e-gamma-delta-one-e", "delta-one"));
                            break;
                    }
                    break;
            }

            return result;
        }
*/
        private LookupList get(int id, string description, string listValue, string filter)
        {
            return get(id, description, description, listValue, filter);
        }

        private LookupList get(int id, string description, string listName, string listValue, string filter)
        {
            return new LookupList()
            {
                ClientId = 1,
                AreaId = 1,
                DepartmentId =1,
                LookupListId = id,
                SiteId = 1,
                Description = listValue,
                ListName = listName, 
                ListValue = listValue, 
                Filter = filter
            };
        }

        public void Createtemplate(DatabaseKey dbKey)
        {
            LookupList_CreateTemplate trans = new LookupList_CreateTemplate();
            trans.LookupList = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();           
        }

        public List<LookupList> GetLookUpListByListName(DatabaseKey dbKey)
        {
            LookUpList_GetLookUpListByListName trans = new LookUpList_GetLookUpListByListName()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                ListName=this.ListName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<b_LookupList> lookup = trans.LookupListList;

            List<LookupList> result = new List<LookupList>();

            foreach (b_LookupList li in lookup)
            {
                LookupList temp = new LookupList()
                {
                    Description = li.Description,
                    ListName = li.ListName,
                    ListValue = li.ListValue
                };
                result.Add(temp);
            }
            return result;

        }

        public int DeleteByLookupListId(DatabaseKey dbKey)
        {
            LookupList_DeleteByLookupListId_V2 trans = new LookupList_DeleteByLookupListId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                
            };
            trans.LookupList = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.Count;
        }
        #endregion
    }
}
