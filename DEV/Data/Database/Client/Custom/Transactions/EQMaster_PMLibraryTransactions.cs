/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-18 SOM-106  Roger Lawton       Added 
****************************************************************************************************
*/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Data.Database;
//using Data.Database.Business;
//using System.Data.SqlClient;
using Common.Enumerations;
using Common.Structures;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class EQMaster_PMLibrary_RetrieveListByEQMasterId : EQMaster_PMLibrary_TransactionBaseClass
    {
        public List<b_EQMaster_PMLibrary> EQMasterPMList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_EQMaster_PMLibrary> tmpArray = null;

            EQMaster_PMLibrary.RetrieveListByEQMasterId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EQMasterPMList = new List<b_EQMaster_PMLibrary>();
            foreach (b_EQMaster_PMLibrary tmpObj in tmpArray)
            {
                EQMasterPMList.Add(tmpObj);
            }
        }
    }

    public class EQMaster_PMLibrary_CreateByFK : EQMaster_PMLibrary_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            EQMaster_PMLibrary.CreateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(EQMaster_PMLibrary.EQMaster_PMLibraryId > 0);
        }
    }

    public class EQMaster_PMLibrary_UpdateByPK : EQMaster_PMLibrary_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EQMaster_PMLibrary.EQMaster_PMLibraryId == 0)
            {
                string message = "EQMaster_PMLibrary has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            EQMaster_PMLibrary.UpdateByFK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class EQMaster_PMLibrary_ValidateByClientlookupId : EQMaster_PMLibrary_TransactionBaseClass
    {
        public EQMaster_PMLibrary_ValidateByClientlookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                EQMaster_PMLibrary.ValidateByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
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

}
