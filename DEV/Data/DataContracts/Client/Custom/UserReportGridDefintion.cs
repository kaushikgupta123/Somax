using Database;
using Database.Business;
using System;
using System.Collections.Generic;
namespace DataContracts
{
   public partial class UserReportGridDefintion : DataContractBase
    {
        #region Properties
        //public Decimal NumofDecPlaces { get; set; }
        //public string NumericFormat { get; set; }
        //public string FilterMethod { get; set; }
        public System.Data.DataTable UserReportList { get; set; }

        #endregion
        public void RetrieveByReportId_V2(DatabaseKey dbKey)
        {
            UserReportGridDefintion_RetrieveByReportId_V2 trans = new UserReportGridDefintion_RetrieveByReportId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.UserReportGridDefintion = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.UserReportGridDefintion);
           
        }

      

        public List<UserReportGridDefintion> RetrieveByReportId(DatabaseKey dbKey)
        {
            UserReportGridDefintion_RetrieveByReportId trans = new UserReportGridDefintion_RetrieveByReportId();
            trans.UserReportGridDefintion = this.ToDatabaseObject();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //this.NumofDecPlaces = trans.UserReportGridDefintion.NumofDecPlaces;
            //this.NumericFormat = trans.UserReportGridDefintion.NumericFormat;
            //this.FilterMethod = trans.UserReportGridDefintion.FilterMethod;
            return UpdateFromDatabaseList(trans.UserReportGridDefintionList);
        }
        //public b_UserReportGridDefintion ToDatabaseObjectForRetrieveByReportId()
        //{
        //    b_UserReportGridDefintion dbObj = new b_UserReportGridDefintion();
        //    dbObj.UserReportGridDefintionId = this.UserReportGridDefintionId;
        //    dbObj.ReportId = this.ReportId;
        //    dbObj.Sequence = this.Sequence;
        //    dbObj.Columns = this.Columns;
        //    dbObj.Heading = this.Heading;
        //    dbObj.Alignment = this.Alignment;
        //    dbObj.Display = this.Display;
        //    dbObj.Required = this.Required;
        //    dbObj.AvailableonFilter = this.AvailableonFilter;
        //    dbObj.IsGroupTotaled = this.IsGroupTotaled;
        //    dbObj.IsGrandTotal = this.IsGrandTotal;
        //    dbObj.LocalizeDate = this.LocalizeDate;
        //    dbObj.IsChildColumn = this.IsChildColumn;         
        //    return dbObj;
        //}

        public List<UserReportGridDefintion> UpdateFromDatabaseList(List<b_UserReportGridDefintion> dbObjs)
        {
            List<UserReportGridDefintion> result = new List<UserReportGridDefintion>();

            foreach (b_UserReportGridDefintion dbObj in dbObjs)
            {
                UserReportGridDefintion tmp = new UserReportGridDefintion();
                tmp.UpdateFromDatabaseObject(dbObj);
                // tmp.ReportEventLogId = dbObj.ReportEventLogId;
                
                result.Add(tmp);
            }
            return result;
        }


        public void UpdateByReportId(DatabaseKey dbKey)
        {
           
                UserReportGridDefintion_UpdateByReportId trans = new UserReportGridDefintion_UpdateByReportId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.UserReportGridDefintion = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

        }

        public void DeleteByReportId(DatabaseKey dbKey)
        {

            UserReportGridDefintion_DeleteByReportId trans = new UserReportGridDefintion_DeleteByReportId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.UserReportGridDefintion = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

        }

        public void UpdateFilterByReportId(DatabaseKey dbKey)
        {
            UserReportGridDefintion_UpdateFilterByReportId trans = new UserReportGridDefintion_UpdateFilterByReportId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.UserReportGridDefintion = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
        public b_UserReportGridDefintion ToExtendedDatabaseObject()
        {
            b_UserReportGridDefintion dbObj = this.ToDatabaseObject();
            //dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            dbObj.UserReportList = this.UserReportList;           
            return dbObj;
        }

        public List<UserReportGridDefintion> RetrieveAllByReportId_V2(DatabaseKey dbKey)
        {
            UserReportGridDefintion_RetrieveAllByReportId_V2 trans = new UserReportGridDefintion_RetrieveAllByReportId_V2();
            trans.UserReportGridDefintion = this.ToDatabaseObjectRetrieveAllByReportId();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseList(trans.UserReportGridDefintionList);
        }

        public b_UserReportGridDefintion ToDatabaseObjectRetrieveAllByReportId()
        {
            b_UserReportGridDefintion dbObj = new b_UserReportGridDefintion();
            dbObj.UserReportGridDefintionId = this.UserReportGridDefintionId;
            dbObj.ReportId = this.ReportId;
            dbObj.Sequence = this.Sequence;
            dbObj.Columns = this.Columns;
            dbObj.Heading = this.Heading;
            dbObj.Alignment = this.Alignment;
            dbObj.Display = this.Display;
            dbObj.Required = this.Required;
            dbObj.AvailableonFilter = this.AvailableonFilter;
            dbObj.IsGroupTotaled = this.IsGroupTotaled;
            dbObj.IsGrandTotal = this.IsGrandTotal;
            dbObj.LocalizeDate = this.LocalizeDate;
            dbObj.IsChildColumn = this.IsChildColumn;          
            dbObj.NumofDecPlaces = this.NumofDecPlaces;
            dbObj.NumericFormat = this.NumericFormat;
            dbObj.FilterMethod = this.FilterMethod;
            return dbObj;
        }

    }
}
