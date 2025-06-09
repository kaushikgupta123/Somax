/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Feb-17 SOM-562  Roger Lawton       Part/Vendor Cross-References not showing up
****************************************************************************************************
*/

using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class Equipment_Parts_Xref_RetrieveByEquipmentId : Equipment_Parts_Xref_TransactionBaseClass
    {
        public List<b_Equipment_Parts_Xref> Equipment_Parts_XrefList { get; set; }

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
            List<b_Equipment_Parts_Xref> tmpArray = null;

            Equipment_Parts_Xref.RetrieveByEquipmentIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Equipment_Parts_XrefList = new List<b_Equipment_Parts_Xref>();
            foreach (b_Equipment_Parts_Xref tmpObj in tmpArray)
            {
                Equipment_Parts_XrefList.Add(tmpObj);
            }
        }
    }

    public class Equipment_Parts_Xref_RetrieveByEquipmentId_V2 : Equipment_Parts_Xref_TransactionBaseClass
    {
        public List<b_Equipment_Parts_Xref> Equipment_Parts_XrefList { get; set; }

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
            List<b_Equipment_Parts_Xref> tmpArray = null;

            Equipment_Parts_Xref.RetrieveByEquipmentIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Equipment_Parts_XrefList = new List<b_Equipment_Parts_Xref>();
            foreach (b_Equipment_Parts_Xref tmpObj in tmpArray)
            {
                Equipment_Parts_XrefList.Add(tmpObj);
            }
        }
    }

    // SOM-562 - Begin
    public class Equipment_Parts_Xref_RetrieveByPartId : Equipment_Parts_Xref_TransactionBaseClass
    {
      public List<b_Equipment_Parts_Xref> Equipment_Parts_XrefList { get; set; }

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
        List<b_Equipment_Parts_Xref> tmpArray = null;

        Equipment_Parts_Xref.RetrieveByPartIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

        Equipment_Parts_XrefList = new List<b_Equipment_Parts_Xref>();
        foreach (b_Equipment_Parts_Xref tmpObj in tmpArray)
        {
          Equipment_Parts_XrefList.Add(tmpObj);
        }
      }
    }

    // SOM-562 - End

    public class Equipment_Parts_Xref_ValidateByClientLookupId : Equipment_Parts_Xref_TransactionBaseClass
    {
        public Equipment_Parts_Xref_ValidateByClientLookupId()
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


                Equipment_Parts_Xref.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                        ref errors);

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

    public class Equipment_Parts_Xref_CreatePKForeignKeys : Equipment_Parts_Xref_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment_Parts_Xref.Equipment_Parts_XrefId > 0)
            {
                string message = "Equipment_Parts_Xref has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Equipment_Parts_Xref.InsertIntoDatabaseByPKForeignKeys(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Equipment_Parts_Xref.Equipment_Parts_XrefId > 0);
        }
    }

    #region V2-1007
    public class Equipment_Parts_Xref_RetrieveByEquipmentIdPartId_V2 : Equipment_Parts_Xref_TransactionBaseClass
    {
        public List<b_Equipment_Parts_Xref> Equipment_Parts_XrefList { get; set; }

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
            List<b_Equipment_Parts_Xref> tmpArray = null;

            Equipment_Parts_Xref.RetrieveByEquipmentIdPartIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Equipment_Parts_XrefList = new List<b_Equipment_Parts_Xref>();
            foreach (b_Equipment_Parts_Xref tmpObj in tmpArray)
            {
                Equipment_Parts_XrefList.Add(tmpObj);
            }
        }
    }
    #endregion

}
