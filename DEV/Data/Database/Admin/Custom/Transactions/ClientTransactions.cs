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
using Database.Business;
using Common.Enumerations;

/*
 * Note: The generated code explicitly sets the client ID based on the user's current client ID. 
 * That logic will prevent super users from loading other clients. To avoid this, all generated
 * code has been moved to the custom section, and the Client_TransactionBaseClass has been modified
 * to check super user status before setting the client ID.
 */

namespace Database.Transactions
{
    //--------------------Added By Indusnet Technologies--------------------------------------------
    public class Client_CreateBySomaxAdmin : Client_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Client.CreatedClientId > 0)
            //{
            //    string message = "Client has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            Client.InsertIntoDatabaseBySomaxAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            // System.Diagnostics.Debug.Assert(Client.ClientId > 0);
        }
    }

    public class Client_SecurityGroupCreate : Client_TransactionBaseClass
    {

        public new string ConnectionString { get; set; }

        public string SecurityGroupName { get; set; }

        public long retSecurityGroupId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            base.m_ConnectionString = ConnectionString;
        }
        public override void PerformWorkItem()
        {
          retSecurityGroupId= Client.CreateClientSecurityGroup(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, SecurityGroupName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Client.ClientId > 0);
        }
    }
    public class Client_RetrieveBySomaxAdmin : Client_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
          
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Client.RetrieveByPKFromDatabaseBySomaxAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
          
        }
    }

    public class Client_RetrieveBySomaxAdmin_V2 : Client_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Client.RetrieveByPKFromDatabaseBySomaxAdmin_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }

    public class Client_UpdateBySomaxAdmin : Client_TransactionBaseClass
    {
        public long vClientId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Client.ClientId == 0)
            //{
            //    string message = "Client has an invalid ID.";
            //    throw new Exception(message);
            //}
            Client.ClientId = vClientId;
           // ChangeLog.ClientId = vClientId;
        }

        public override void PerformWorkItem()
        {
            Client.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
   
    //--------------------End Added By Indusnet Technologies--------------------------------------------


    public class Client_Validate : Client_TransactionBaseClass
    {

        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

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
            List<b_StoredProcValidationError> tmpList = null;
            
            Client.ValidateFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            StoredProcValidationErrorList = tmpList;

        }
    }

    public class Admin_RetrieveClientChunkSearchV2 : Client_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation(); 
        }
        public override void PerformWorkItem()
        {
            b_Client tmpList = null;
            Client.RetrieveClientChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class Client_RetrieveAllActive : Client_TransactionBaseClass
    {

        public Client_RetrieveAllActive()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Client> ClientList { get; set; }

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
            b_Client[] tmpArray = null;
            //b_Client o = new b_Client();


            // Explicitly set tenant id from dbkey


            Client.RetrieveAllActiveClientFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ClientList = new List<b_Client>(tmpArray);
        }


    }

    public class Retrieve_CountforCompanyName : Client_TransactionBaseClass
    {

        public List<b_Client> countList { get; set; }

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
            List<b_Client> tmpArray = null;
            Client.RetrieveCountforCompanyName(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            countList = new List<b_Client>();
            foreach (b_Client tmpObj in tmpArray)
            {
                countList.Add(tmpObj);
            }
        }


    }
    #region V2-964
    public class Client_CreateBySomaxAdminV2 : Client_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Client.CreatedClientId > 0)
            //{
            //    string message = "Client has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            Client.InsertIntoDatabaseBySomaxAdminV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            // System.Diagnostics.Debug.Assert(Client.ClientId > 0);
        }
    }
    #endregion
}
