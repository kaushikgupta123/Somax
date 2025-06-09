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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;

namespace Transactions
{
    public class Part_Vendor_Xref_RetrieveListByPartId : Part_Vendor_Xref_TransactionBaseClass
    {
        public List<b_Part_Vendor_Xref> PartVendorList { get; set; }

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
            List<b_Part_Vendor_Xref> tmpArray = null;

            Part_Vendor_Xref.RetrieveListByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartVendorList = new List<b_Part_Vendor_Xref>();
            foreach (b_Part_Vendor_Xref tmpObj in tmpArray)
            {
                PartVendorList.Add(tmpObj);
            }
        }
    }
    public class Part_Vendor_Xref_RetrieveListByVendorId : Part_Vendor_Xref_TransactionBaseClass
    {
        public List<b_Part_Vendor_Xref> PartVendorList { get; set; }

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
            List<b_Part_Vendor_Xref> tmpArray = null;

            Part_Vendor_Xref.RetrieveListByVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartVendorList = new List<b_Part_Vendor_Xref>();
            foreach (b_Part_Vendor_Xref tmpObj in tmpArray)
            {
                PartVendorList.Add(tmpObj);
            }
        }
    }
    public class Part_Vendor_Xref_RetrieveByPKExtended : Part_Vendor_Xref_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part_Vendor_Xref.Part_Vendor_XrefId == 0)
            {
                string message = "Part_Vendor_Xref has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Part_Vendor_Xref.RetrieveByPKExtended(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class Part_Vendor_Xref_ValidateByDuplicacy : Part_Vendor_Xref_TransactionBaseClass
    {
        public Part_Vendor_Xref_ValidateByDuplicacy()
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


                Part_Vendor_Xref.Part_Vendor_Xref_ValidateByDuplicacy(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    #region V2-1119 Part_Vendor_Xref Retrieve For ShoppingCartDataImport Transaction
    public class Part_Vendor_Xref_RetrieveForShoppingCartDataImport : Part_Vendor_Xref_TransactionBaseClass
    {
        public List<b_Part_Vendor_Xref> PartVendorList { get; set; }
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
            List<b_Part_Vendor_Xref> tmpArray = null;

            Part_Vendor_Xref.RetrieveForShoppingCartDataImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartVendorList = new List<b_Part_Vendor_Xref>();
            foreach (b_Part_Vendor_Xref tmpObj in tmpArray)
            {
                PartVendorList.Add(tmpObj);
            }
        }
    }

    #region V2-1119 Add or Update Part/Vendor Cross-Reference when Processing Shopping Cart Item
    public class Part_Vendor_Xref_Create_Update_Punchout_V2 : Part_Vendor_Xref_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part_Vendor_Xref.Part_Vendor_XrefId > 0)
            {
                string message = "Part_Vendor_Xref has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Part_Vendor_Xref.Part_Vendor_Xref_Create_Update_Punchout_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
        public override void Postprocess()
        {
            //base.Postprocess();
            //System.Diagnostics.Debug.Assert(Part_Vendor_Xref.Part_Vendor_XrefId > 0);
        }
    }
    #endregion
    #endregion
}
