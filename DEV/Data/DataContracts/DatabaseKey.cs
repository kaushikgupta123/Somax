﻿/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */

/*
 *  Modified by Indusnet Technologies
 *  The fileds AccessClientId and AccessClient used to be access data of a client from admin section which is accessed
 *  from somax user i.e. user of somax client.
 */ 

using System;

namespace DataContracts
{
    [Serializable()]
    public class DatabaseKey
    {
        #region Member Variables
        public string AdminConnectionString { get; set; }
        public string ClientConnectionString { get; set; }
        public Client Client { get; set; }
        public UserInfo User { get; set; }
        public Personnel Personnel { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string AdminDeploymentEnvironment { get; set; }
        public string ClientDeploymentEnvironment { get; set; }
        public string CodeDeploymentEnvironment { get; set; }
        public string EncryptionKey { get; set; }
        //V2-730
        public ApprovalGroupSettings ApprovalGroupSettings { get; set; }

        //-------------  Added By Indusnet Technologies---------------
        public long AccessingClientId { get; set; }
        public Client AccessingClient { get; set; }
        public bool _IsAccessClientData = false;
        public bool IsAccessClientData
        {
            get { return _IsAccessClientData; }
            set {
                if (value == true && AccessingClientId > 0)
                {
                    AccessingClient = new Client();
                    AccessingClient.CreatedClientId = AccessingClientId;
                    AccessingClient.RetrieveBySomaxAdmin(this);
                }
                else
                {
                    AccessingClient = null;
                    AccessingClientId = -1;
                }
                     _IsAccessClientData = value;

                 }
        }
        //------------ End Added By Indusnet Technologies-----------------
        #endregion

        #region Constructors
        public DatabaseKey()
        {
            Client = new Client();
            User = new UserInfo();
            Personnel = new Personnel();
            ////V2-730 Start
            ApprovalGroupSettings = new ApprovalGroupSettings();
            //End

            //------------Added By Indusnet technology------------------
            AccessingClient = null;
            AccessingClientId = 0;
            IsAccessClientData = false;
            //-----------End Added By Indusnet technology--------------
        }
        #endregion

        #region Property

        #region Localization
        public string Localization
        {
            get
            {
                //V2-534
                string lz = !string.IsNullOrEmpty(User.Localization) ? User.Localization : Client.Localization;
                if (!(lz == "en-us" || lz == "fr-fr"))
                {
                    lz = "en-us";
                }
                return lz;
                //V2-534

                //return !string.IsNullOrEmpty(User.Localization) ? User.Localization : Client.Localization;
            }
        }

        #endregion
        

        #region UIConfiguration

        public string UIConfiguration
        {
            get
            {
                return !string.IsNullOrEmpty(User.UIConfiguration) ? User.UIConfiguration : Client.UIConfiguration;
            }
        } 

        #endregion

        #endregion

        #region Public Methods
        public Database.DatabaseKey ToTransDbKey() 
        {
            Database.DatabaseKey dbkey = new Database.DatabaseKey();
            dbkey.AdminConnectionString = AdminConnectionString;
            if(string.IsNullOrEmpty(ClientConnectionString))
            {
                dbkey.ClientConnectionString = AdminConnectionString;
            }
            else
            {
                dbkey.ClientConnectionString = ClientConnectionString;
            }
            
            dbkey.Client = Client.ToDatabaseObject();
            dbkey.Personnel = Personnel.ToDatabaseObject();
            dbkey.User = User.ToDatabaseObject();
            dbkey.UserName = UserName;

            //--------------Added By Indusnet Technologies--------------------
            dbkey.AccessingClient = this.AccessingClient!=null?this.AccessingClient.ToDatabaseObject():null;
            dbkey.IsAccessClientData = this.IsAccessClientData;
            //-------------End Added By Indusnet Technologies-----------------


            return dbkey;

        }
        #endregion
        
    }
}
