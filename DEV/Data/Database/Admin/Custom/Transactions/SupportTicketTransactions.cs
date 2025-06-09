using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;
namespace Database.Transactions
{
    public class SupportTicket_RetrieveChunkSearch : SupportTicket_TransactionBaseClass
    {
        public List<b_SupportTicket> SupportTicketList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SupportTicket.SupportTicketId < 0)
            {
                string message = "MaterialRequest has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_SupportTicket> tmpList = null;
            SupportTicket.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SupportTicketList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class SupportTicket_CreateInAdminSite : AbstractTransactionManager
    {
        public SupportTicket_CreateInAdminSite()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_SupportTicket SupportTicket { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SupportTicket.SupportTicketId > 0)
            {
                string message = "SupportTicket has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            SupportTicket.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
    public class SupportTicket_RetrieveTags : SupportTicket_TransactionBaseClass
    {
        public b_SupportTicket objSupportTicket { get; set; }
        public List<List<b_SupportTicket>> SupportTicketPersonnelList { get; set; }
        public List<b_SupportTicket> SupportTicketList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<List<b_SupportTicket>> tmpArray = null;
            SupportTicket.RetrieveTags(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SupportTicketPersonnelList = new List<List<b_SupportTicket>>();
            SupportTicketList = new List<b_SupportTicket>();
            foreach (List<b_SupportTicket> tmpObj in tmpArray)
            {
                foreach (b_SupportTicket tmpObj2 in tmpObj)
                {
                    SupportTicketList.Add(tmpObj2);
                }
                SupportTicketPersonnelList.Add(tmpObj);
            }

        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class SupportTicket_RetrieveByPKForAdmin : AbstractTransactionManager
    {
        public SupportTicket_RetrieveByPKForAdmin()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_SupportTicket SupportTicket { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SupportTicket.SupportTicketId == 0)
            {
                string message = "SupportTicket has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            SupportTicket.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
    public class SupportTicket_UpdateInAdminSite : AbstractTransactionManager
    {
        public SupportTicket_UpdateInAdminSite()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_SupportTicket SupportTicket { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SupportTicket.SupportTicketId == 0)
            {
                string message = "SupportTicket has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            SupportTicket.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
}
