using System;
using System.Collections.Generic;

using Common.Enumerations;

using Database.Business;

namespace Database.Transactions
{

    public class STNotes_RetrieveBySupportTicketId : STNotes_TransactionBaseClass
    {

        public STNotes_RetrieveBySupportTicketId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_STNotes> STNotesList { get; set; }

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
            b_STNotes[] tmpArray = null;

            STNotes.RetrieveBySupportTicketIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            STNotesList = new List<b_STNotes>(tmpArray);
        }
    }
    public class STNotes_CreateInAdminSite : AbstractTransactionManager
    {
        public STNotes_CreateInAdminSite()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_STNotes STNotes { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (STNotes.STNotesId > 0)
            {
                string message = "STNotes has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            STNotes.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
}
