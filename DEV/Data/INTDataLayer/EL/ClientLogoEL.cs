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
    public class ClientLogoEL
    {
        public Int64 ClientId { set; get; }
        public Int64 LogoId { set; get; }
        public Int64 SiteId { set; get; }
        public string Type { set; get; }
        public Byte[] Image { set; get; }
        public string CreateBy { set; get; }
        public DateTime CreateDate { set; get; }
        public string ModifyBy { set; get; }
        public DateTime ModifyDate { set; get; }
        public string CallerUserName { get; set; }
        public Int64 CallerUserInfoId { get; set; }
        int UpdateIndex { get; set; }
    }
}
