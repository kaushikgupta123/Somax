/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-15  SOM-369   Roger Lawton      Added SiteId as a property - needed for some actions
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class UserEL
    {
        public Int64 ClientId
        { set; get; }
        // SOM-369
        public Int64 SiteId
        { set; get; }

        public Int64 UserInfoId
        { set; get; }
        public string UserFullName
        { set; get; }
        public string ClientCompanyName { set; get; }
        public Int64 PerformedById
        { set; get; }
        public Int64 RequestorId
        { set; get; }
        public Int64 PersonnelId
        { set; get; }
    }
}
