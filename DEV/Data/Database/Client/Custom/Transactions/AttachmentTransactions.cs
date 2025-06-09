/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Attachment.cs - Custom Data Contract 
**************************************************************************************************
* Copyright (c) 2013-2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person         Description
* =========== ========= ============= ==========================================================
* 2018-Nov-05 SOM-1650  Roger Lawton  Added the Attachment_DeleteDbStoredAttachment Class
*                                     Added the  Class
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
  // SOM-1693,SOM-1694, SOM-1695
  // No longer need the Attachment_DeleteDbStoredAttachment class
  // No longer need the Attachment_UpdateForMigrate class
  // No longer need the Attachment_CreateMigrate class
  public class Attachment_RetrieveURLCount : Attachment_TransactionBaseClass
  {
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Attachment.AttachmentId == 0)
      {
        string message = "Attachment has an invalid ID.";
        throw new Exception(message);
      }
    }

        public override void PerformWorkItem()
        {
            Attachment.RetrieveURLCount(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class Attachment_RetrieveURLCount_V2 : Attachment_TransactionBaseClass
    {
        public int URLCount { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Attachment.AttachmentId == 0)
            //{
            //    string message = "Attachment has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            Attachment.AttachmentRetrieveURLCount_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            this.URLCount = Attachment.URLCount;
        }
    }

    public class Attachment_RetrieveAllByFileName : Attachment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Attachment.RetrieveListByFileNameFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

    }
    public class Attachment_RetrieveAllAttachmentsForObject : Attachment_TransactionBaseClass
    {
        public Attachment_RetrieveAllAttachmentsForObject()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public List<b_Attachment> AttachmentList { get; set; }

        public override void Preprocess()
        {
            this.UseTransaction = false;
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
            b_Attachment[] tmpArray = null;
            Attachment.ClientId = this.dbKey.Client.ClientId;
            Attachment.RetrieveAllAttachmentsForObject(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AttachmentList = new List<b_Attachment>(tmpArray);
        }
    }

    public class Attachment_RetrieveAllAttachmentList : Attachment_TransactionBaseClass
    {

        public Attachment_RetrieveAllAttachmentList()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Attachment> AttachmentList { get; set; }

        public override void Preprocess()
        {
            this.UseTransaction = false;
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
            b_Attachment[] tmpArray = null;
            b_Attachment o = new b_Attachment();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveListFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AttachmentList = new List<b_Attachment>(tmpArray);
        }
    }
    public class Attachment_RetrieveProfileAttachments : Attachment_TransactionBaseClass
    {

        public Attachment_RetrieveProfileAttachments()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
            UseTransaction = false;
        }
        public List<b_Attachment> AttachmentList { get; set; }
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
            b_Attachment[] tmpArray = null;
            b_Attachment o = this.Attachment;


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveProfileAttachments(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AttachmentList = new List<b_Attachment>(tmpArray);
        }
    }
    public class Attachment_RetrieveLogo : Attachment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Attachment.RetrieveLogo(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #region V2-716
    public class Attachment_RetrieveMultipleProfileAttachments : Attachment_TransactionBaseClass
    {

        public Attachment_RetrieveMultipleProfileAttachments()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
            UseTransaction = false;
        }
        public List<b_Attachment> AttachmentList { get; set; }
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
            b_Attachment[] tmpArray = null;
            b_Attachment o = this.Attachment;


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveMultipleProfileAttachments(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            AttachmentList = new List<b_Attachment>(tmpArray);
        }
    }

    public class Attachment_RetrieveURLCount_ByObjectAndFileName_V2 : Attachment_TransactionBaseClass
    {
        public int URLCount { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Attachment.AttachmentId == 0)
            //{
            //    string message = "Attachment has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            Attachment.AttachmentRetrieveURLCount_ByObjectAndFileName_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            this.URLCount = Attachment.URLCount;
        }
    }
    #endregion

}
