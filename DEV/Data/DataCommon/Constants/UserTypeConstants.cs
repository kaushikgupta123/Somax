/**************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2020 by SOMAX Inc..
* All rights reserved. 
***************************************************************************************************
* User Type Constants 
***************************************************************************************************
* Date        Log Entry Person         Description
* =========== ========= ============== ============================================================
* 2020-Oct-27 V2-415    Roger Lawton   Remove the Inventory User Type - No Longer Supported
*                                      Add the User Type APMUser
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class UserTypeConstants
    {
        public const string Full = "Full";
        public const string Reference = "Reference";
        public const string WorkRequest = "WorkRequest";
        //public const string Inventory = "Inventory";      // V2-415
        public const string Buyer = "Buyer";
        public const string Admin = "Admin";
        public const string Enterprise = "Enterprise";
        public const string SOMAXAdmin = "SOMAXAdmin";
        public const string Production = "Production";     //V2-613 
    }
}
