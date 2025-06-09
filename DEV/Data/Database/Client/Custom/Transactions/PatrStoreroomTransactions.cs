/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class PartStoreroom_RetrieveByPartId : PartStoreroom_TransactionBaseClass
    {
        public List<b_PartStoreroom> PartStoreroomList { get; set; }

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
            List<b_PartStoreroom> tmpArray = null;

            PartStoreroom.RetrieveByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartStoreroomList = new List<b_PartStoreroom>();
            foreach (b_PartStoreroom tmpObj in tmpArray)
            {
                PartStoreroomList.Add(tmpObj);
            }
        }
    }

    public class PartStoreroom_ValidateByPartId : PartStoreroom_TransactionBaseClass
    {
        public PartStoreroom_ValidateByPartId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public bool CreateMode { get; set; }
        public System.Data.DataTable lulist { get; set; }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                PartStoreroom.ValidateByPartId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class PartStoreroom_UpdateByPartId : PartStoreroom_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartStoreroom.PartStoreroomId == 0)
            {
                string message = "PartStoreroom has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartStoreroom.UpdateByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartStoreroom.PartStoreroomId > 0);
        }
    }

    public class PartStoreroom_RetrieveByPartStoreroomId : PartStoreroom_TransactionBaseClass
    {
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
            base.UseTransaction = false;

            PartStoreroom.RetrieveByPartStoreroomId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);


        }
    }
    #region V2-670
    public class PartStoreroom_SearchForMultiPartStoreroomChildGridV2 : PartStoreroom_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartStoreroom.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_PartStoreroom tmpList = null;
            PartStoreroom.RetrieveForChildGridByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class PartStoreroom_RetrieveByStoreroomIdAndPartId : PartStoreroom_TransactionBaseClass
    {
        public List<b_PartStoreroom> PartStoreroomList { get; set; }
        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //    //if (PartStoreroom.PartStoreroomId == 0)
        //    //{
        //    //    string message = "PartStoreroom has an invalid ID.";
        //    //    throw new Exception(message);
        //    //}
        //}
        public override void PerformWorkItem()
        {
            List<b_PartStoreroom> tmpList = null;

            PartStoreroom.RetrieveByStoreroomIdAndPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartStoreroomList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class PartStoreroom_ValidateStoreroomIdTransaction : PartStoreroom_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            PartStoreroom.ValidateStoreroomId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    #endregion
    #region V2-751
    public class PartStoreroom_RetrieveIssuingStoreroomListForPartTransferRequest : PartStoreroom_TransactionBaseClass
    {
        public List<b_PartStoreroom> PartStoreroomList { get; set; }
        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //    //if (PartStoreroom.PartStoreroomId == 0)
        //    //{
        //    //    string message = "PartStoreroom has an invalid ID.";
        //    //    throw new Exception(message);
        //    //}
        //}
        public override void PerformWorkItem()
        {
            List<b_PartStoreroom> tmpList = null;

            PartStoreroom.RetrieveIssuingStoreroomListForPartTransferRequest(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartStoreroomList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region V2-1025
    public class PartStoreroom_StoreroomGridChildDetailsViewByPartStoreroomIdV2 : PartStoreroom_TransactionBaseClass
    {

        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //    //if (PartStoreroom.PartId == 0)
        //    //{
        //    //    string message = "Part has an invalid ID.";
        //    //    throw new Exception(message);
        //    //}
        //}
        public override void PerformWorkItem()
        {
            b_PartStoreroom tmpdata = null;
            PartStoreroom.RetrieveForChildDetailsViewByPartStoreroomId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpdata);
            PartStoreroom = tmpdata;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion


    #region  V2-1059
    public class PartStoreroom_ValidateSameStoreroomIdTransaction : PartStoreroom_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            PartStoreroom.ValidateSameStoreroomId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class PartStoreroom_RetrieveChunkSearchAutoTRGenerationTransaction : PartStoreroom_TransactionBaseClass
    {
        public List<b_PartStoreroom> PartStoreroomList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartStoreroom.PartStoreroomId > 0)
            {
                string message = "PartStoreroom has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_PartStoreroom> tmpList = null;
            PartStoreroom.RetrieveChunkSearchForAutoTRGeneration(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PartStoreroomList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region V2-1070
    public class PartStoreroom_ValidateByInactivateorActivate : PartStoreroom_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            PartStoreroom.ValidateByInactivateorActivatePartForStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    #endregion
}


