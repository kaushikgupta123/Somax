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
* 2016-Jun-03 SOM-999  Roger Lawton        Change "Emergency" to "Unplanned"
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class WorkOrderSourceTypes
    {
        public const string PreventiveMaint = "PreventiveMaint";
        public const string OnDemand = "OnDemand";
        public const string Corrective = "Corrective";
        public const string Emergency = "Unplanned";
        //    public const string Emergency = "Emergency";
        public const string WorkRequest = "WorkRequest";
        public const string SanitationRequest = "SanitationReq";
        public const string FollowUp = "FollowUp";
        public const string External = "External";
        public const string Sanitation = "Sanitation";
    }

}