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
* =========== ======== =================== =========================================================
* 2014-Oct-06 SOM-359  Roger Lawton        Modified "AwaitingApproval" to "AwaitApproval"
*                                          Due to the limit of 15 characters in the column
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class WorkOrderStatusConstants
    {
        public const string WorkRequest = "WorkRequest";
        public const string Approved = "Approved";
        public const string Scheduled = "Scheduled";
        public const string AwaitingApproval = "AwaitApproval";
        public const string Complete = "Complete";
        public const string Denied = "Denied";
        public const string Canceled = "Canceled";
        public const bool Wo_AutoGenerateEnabled = true;
        public const string Planning = "Planning";
       
    }
}