/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    
    public class PartHistoryReview_Retrieve : AbstractTransactionManager
    {
        public List<b_PartHistoryReview> PartHistoryReviewList { get; set; }
        public b_PartHistoryReview PartHistoryReview { get; set; }

        public PartHistoryReview_Retrieve()
        {
           
            UseDatabase = DatabaseTypeEnum.Client;
        }
     
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartHistoryReview == null)
            {
                string message = "PartHistoryReview has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PartHistoryReview.ClientId = this.dbKey.Client.ClientId;
   
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_PartHistoryReview> tmpArray = null;

            PartHistoryReview.RetrievePartHistoryReviewFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartHistoryReviewList = new List<b_PartHistoryReview>();
            foreach (b_PartHistoryReview tmpObj in tmpArray)
            {
                PartHistoryReviewList.Add(tmpObj);
            }
        }
        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    #region V2-760
    public class PartHistoryReview_Retrieve_V2 : AbstractTransactionManager
    {
        public List<b_PartHistoryReview> PartHistoryReviewList { get; set; }
        public b_PartHistoryReview PartHistoryReview { get; set; }

        public PartHistoryReview_Retrieve_V2()
        {

            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartHistoryReview == null)
            {
                string message = "PartHistoryReview has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PartHistoryReview.ClientId = this.dbKey.Client.ClientId;

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_PartHistoryReview> tmpArray = null;

            PartHistoryReview.RetrievePartHistoryReviewFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartHistoryReviewList = new List<b_PartHistoryReview>();
            foreach (b_PartHistoryReview tmpObj in tmpArray)
            {
                PartHistoryReviewList.Add(tmpObj);
            }
        }
        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }
    #endregion V2-760
}
