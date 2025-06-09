/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton        Localized the Status
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class ReviewLog : DataContractBase
    {
      
        #region Public Variables
        
       
        public string Reviewed_By { get; set; }
        public string Function_Display { get; set; }
        #endregion


        public void UpdateFromDatabaseObjectExtended(b_ReviewLog dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Reviewed_By = dbObj.Reviewed_By;
            if (dbObj.ReviewDate != null && dbObj.ReviewDate != DateTime.MinValue)
            {
                this.ReviewDate = dbObj.ReviewDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.ReviewDate = dbObj.ReviewDate;
            }
            switch (this.Function)
            {
                case Common.Constants.ReviewLogConstants.Approved:
                    this.Function_Display = "Approved";
                    break;
                case Common.Constants.ReviewLogConstants.Canceled:
                    this.Function_Display = "Canceled";
                    break;
                case Common.Constants.ReviewLogConstants.Completed:
                    this.Function_Display = "Completed";
                    break;
                case Common.Constants.ReviewLogConstants.Created:
                    this.Function_Display = "Created";
                    break;
                case Common.Constants.ReviewLogConstants.Denied:
                    this.Function_Display = "Denied";
                    break;
                case Common.Constants.ReviewLogConstants.Email:
                    this.Function_Display = "Email";
                    break;
                case Common.Constants.ReviewLogConstants.EnterpriseApprove:
                    this.Function_Display = "Enterprise Approve";
                    break;
                case Common.Constants.ReviewLogConstants.ImportEmailPO:
                    this.Function_Display = "Imported Email PO";
                    break;
                case Common.Constants.ReviewLogConstants.ImportPO:
                    this.Function_Display = "Import PO";
                    break;
                case Common.Constants.ReviewLogConstants.Returned:
                    this.Function_Display = "Returned";
                    break;
                case Common.Constants.ReviewLogConstants.Reviewed:
                    this.Function_Display = "Reviewed";
                    break;
                case Common.Constants.ReviewLogConstants.SiteApprove:
                    this.Function_Display = "Site Approved";
                    break;
                default:
                    this.Function_Display = string.Empty;
                    break;
            }
        }


        public List<ReviewLog> Retrieve_LogDetailsByPMRId(DatabaseKey dbKey, string Timezone)
        {
            ReviewLog_RetrieveByPMRId trans = new ReviewLog_RetrieveByPMRId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ReviewLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ReviewLog> reviewLogList = new List<ReviewLog>();
            foreach (b_ReviewLog reviewLog in trans.reviewLogList)
            {
                ReviewLog tmpreviewLog = new ReviewLog();

                tmpreviewLog.UpdateFromDatabaseObjectExtended(reviewLog, Timezone);
                reviewLogList.Add(tmpreviewLog);
            }
            return reviewLogList;

        }

    }

}
