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
using Newtonsoft.Json;

namespace DataContracts
{
    public partial class MemorizeSearch : DataContractBase
    {
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public bool IsClear { get; set; }

        public b_MemorizeSearch ToDatabaseObjectExtended()
        {
            b_MemorizeSearch dbObj = this.ToDatabaseObject();
            dbObj.CreateDate = this.CreateDate;
            dbObj.CreateBy = this.CreateBy;
            dbObj.IsClear = this.IsClear;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectforsearch(b_MemorizeSearch dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CreateDate = dbObj.CreateDate;
            this.CreateBy = dbObj.CreateBy;

        }
        public List<MemorizeSearch> MemorizeSearchRetrieveForSearch(DatabaseKey dbKey)
        {
            MemorizeSearch_RetrieveForSearch trans = new MemorizeSearch_RetrieveForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };

            trans.MemorizeSearch = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<MemorizeSearch> MemorizeSearchList = new List<MemorizeSearch>();
            foreach (b_MemorizeSearch MemorizeSearch in trans.MemorizeSearchList)
            {
                MemorizeSearch tmpMemorizeSearch = new MemorizeSearch();
                tmpMemorizeSearch.UpdateFromDatabaseObjectforsearch(MemorizeSearch);

                MemorizeSearchList.Add(tmpMemorizeSearch);
            }

            return MemorizeSearchList;
        }


        public List<MemorizeSearch> MemorizeSearchRetrieveafterCreateAndDelete(DatabaseKey dbKey)
        {
            RetrieveafterCreateAndDelete trans = new RetrieveafterCreateAndDelete()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };

            trans.MemorizeSearch = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<MemorizeSearch> MemorizeSearchList = new List<MemorizeSearch>();
            foreach (b_MemorizeSearch MemorizeSearch in trans.MemorizeSearchList)
            {
                MemorizeSearch tmpMemorizeSearch = new MemorizeSearch();
                tmpMemorizeSearch.UpdateFromDatabaseObjectforsearch(MemorizeSearch);

                MemorizeSearchList.Add(tmpMemorizeSearch);
            }

            return MemorizeSearchList;
        }
    }
}
