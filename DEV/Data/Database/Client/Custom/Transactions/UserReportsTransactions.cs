using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;
namespace Database
{
   
    public class Retrieve_CountforReportName : UserReports_TransactionBaseClass
    {

        public List<b_UserReports> countList { get; set; }
        //public b_UserReports UserReports { get; set; }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_UserReports> tmpArray = null;
          UserReports.RetrieveCountforReportName(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            countList = new List<b_UserReports>();
            foreach (b_UserReports tmpObj in tmpArray)
            {
                countList.Add(tmpObj);
            }
        }


    }


    public class UserReports_RetrieveByGroup : UserReports_TransactionBaseClass
    {

        public UserReports_RetrieveByGroup()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_UserReports> UserReportsList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_UserReports> tmpArray = null;
            UserReports.RetrieveByGroup(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            UserReportsList = tmpArray;
        }
    }
}
