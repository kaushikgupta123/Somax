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
****************************************************************************************************
 */
using Database.Business;
using System.Collections.Generic;

namespace Database.Transactions
{
    public class Localizations_RetrieveByResourceSet : Localizations_TransactionBaseClass
    {

        public List<b_Localizations> LocalizationsList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_Localizations> tmpList = null;
            Localizations.RetrieveByResourceSetFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            LocalizationsList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }






}

