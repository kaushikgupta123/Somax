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
* Date        Task ID   Person             Description
* =========== ======== ===================  =========================================================
*
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class PurchasingEvents
    {
        #region PREvents New
        public const string Approved = "Approved";
        public const string Archived = "Archived";
        public const string Canceled = "Canceled";
        public const string POCreate = "POCreate";
        //public const string PRCreate = "PRCreate";
        public const string Denied = "Denied";
        public const string Void = "Void";
        public const string POOpen = "POOpen";
        public const string PROpen = "PROpen";
        public const string Resubmit = "Resubmit";
        public const string EmailToVendor = "EmailToVendor";
        public const string SendForApproval = "SendForApproval";
        public const string ReceiptComplete = "ReceiptComp";
        public const string ReceiptPartial = "ReceiptPart";
        // RKL - Add item for Review Log 
        public const string Import = "Import";
        public const string ImportUpdate = "ImportUpdate";

        public const string ReceiptImport = "ReceiptImport";
        public const string ReceiptImportUpdate = "ReceiptImpUpd";  // RKL - 2020-10-09 Limit is 15 chars
        public const string SentToCoupa = "SentToCoupa";

        public const string PunchOut = "PunchOut";
        public const string OrderRequestSent = "OrderRequestSent";

        public const string SentToSAP = "SentToSAP";
        public const string Reviewed = "Reviewed";
        public const string EDIPOSent = "EDIPOSent";
        #endregion

    }
}