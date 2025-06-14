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
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System.Collections.Generic;
using Common.Enumerations;
using Database.Business;
using System;

namespace Database.Transactions
{

    public class FileInfo_RetrieveByOwnerId : FileInfo_TransactionBaseClass
    {

        public FileInfo_RetrieveByOwnerId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_FileInfo> FileInfoList { get; set; }

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
            b_FileInfo[] tmpArray = null;

            FileInfo.RetrieveByOwnerIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            FileInfoList = new List<b_FileInfo>(tmpArray);
        }
    }

    public class FileInfo_RetrieveByObjectId : FileInfo_TransactionBaseClass
    {

        public FileInfo_RetrieveByObjectId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_FileInfo> FileInfoList { get; set; }

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
            b_FileInfo[] tmpArray = null;

            FileInfo.RetrieveByObjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            FileInfoList = new List<b_FileInfo>(tmpArray);
        }
    }
    public class FileInfo_RetrieveAllForClient : FileInfo_TransactionBaseClass
    {

        public FileInfo_RetrieveAllForClient()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Admin;
        }


        public List<b_FileInfo> FileInfoList { get; set; }

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
            long cl_save = FileInfo.ClientId;
            base.PerformLocalValidation();
            FileInfo.ClientId = cl_save;
        }

        public override void PerformWorkItem()
        {
            b_FileInfo[] tmpArray = null;

            FileInfo.RetrieveForClient(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            FileInfoList = new List<b_FileInfo>(tmpArray);
        }
    }

    public class FileInfo_RetrieveByFileInfoId : FileInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (FileInfo.FileInfoId == 0)
            {
                string message = "FileInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            FileInfo.RetrieveByFileInfoIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class FileInfo_RetrieveForMigrate : FileInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            long clid = FileInfo.ClientId;
            base.PerformLocalValidation();
            if (FileInfo.FileInfoId == 0)
            {
                string message = "FileInfo has an invalid ID.";
                throw new Exception(message);
            }
            FileInfo.ClientId = clid;
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            FileInfo.RetrieveForMigrate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
