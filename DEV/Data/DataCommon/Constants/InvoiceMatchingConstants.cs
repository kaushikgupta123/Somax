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
* =========== ======== =================== =========================================================
*
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class InvoiceMatchStatus
    {
        public const string Open = "Open";
        public const string AuthorizedToPay = "AuthorizedToPay";
        public const string Paid = "Paid";
        public const string ChangeInvoice = "ChangeInvoice";
        public const string Reopen = "Reopen";
    }
}