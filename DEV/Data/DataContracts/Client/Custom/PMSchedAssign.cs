using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class PMSchedAssign:DataContractBase
    {
        public long SiteId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public string ClientLookupId { get; set; }
        public string PersonnelFullName { get; set; }
        public int TotalCount { get; set; }
        public List<PMSchedAssign> RetrivePMSchedAssignByPMSchedId(DatabaseKey dbKey)
        {
            PMSchedAssign_RetriveByPMSchedId trans = new PMSchedAssign_RetriveByPMSchedId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PMSchedAssign = this.ToDatabaseExtendObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PMSchedAssign> pmSchedAssignlist = new List<PMSchedAssign>();

            foreach (b_PMSchedAssign obj in trans.PMSchedAssignList)
            {
                PMSchedAssign tmpobj = new PMSchedAssign();
                tmpobj.UpdateFromDatabaseObjectExtended(obj);
                pmSchedAssignlist.Add(tmpobj);
            }
            return pmSchedAssignlist;
        }

        public b_PMSchedAssign ToDatabaseExtendObject()
        {
            b_PMSchedAssign dbObj = this.ToDatabaseObject();
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtended(b_PMSchedAssign dbObj)
        {
            this.PMSchedAssignId = dbObj.PMSchedAssignId;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.PersonnelId = dbObj.PersonnelId;
            this.PersonnelFullName = dbObj.PersonnelFullName;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.TotalCount = dbObj.TotalCount;
            
        }

        #region V2-1204
        public List<PMSchedAssign> RetrieveByPMSchedId_V2(DatabaseKey dbKey)
        {
            RetrieveByPMSchedId_V2 trans = new RetrieveByPMSchedId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PMSchedAssign = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PMSchedAssignList);
        }

        public static List<PMSchedAssign> UpdateFromDatabaseObjectList(List<b_PMSchedAssign> dbObjs)
        {
            List<PMSchedAssign> result = new List<PMSchedAssign>();

            foreach (b_PMSchedAssign dbObj in dbObjs)
            {
                PMSchedAssign tmp = new PMSchedAssign();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        #endregion
    }


}
