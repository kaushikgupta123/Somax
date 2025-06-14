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


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Database;
using Database.Business;


namespace Database
{
    public class SiteMaintenance_RetrieveOutage : SiteMaintenance_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SiteMaintenance.RetrieveOutageSiteMaintenance(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #region //Add on 09/07/2020 System Unavailable Message
    //Add on 09/07/2020 System Unavailable Message
    public class SiteMaintenance_RetrieveNexcSch : SiteMaintenance_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SiteMaintenance.SiteMaintenanceId == 0)
            //{
            //    string message = "SiteMaintenance has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SiteMaintenance.RetrieveNextSchSiteMaintenance(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.SiteMaintenance.TimeZone, this.SiteMaintenance.SameDay);
        }
    }
    //Add on 09/07/2020 System Unavailable Message
    #endregion

    #region WebSiteMaintainenceMessage Details V2-994

    public class SiteMaintenance_RetrieveById_V2 : SiteMaintenance_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SiteMaintenance.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class SiteMaintenance_RetrieveChunkSearchFromDetails : SiteMaintenance_TransactionBaseClass
    {
        public List<b_SiteMaintenance> siteMaintenanceList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_SiteMaintenance> tmpList = null;
            SiteMaintenance.RetrieveChunkSearchSiteMaintenanceDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            siteMaintenanceList = new List<b_SiteMaintenance>();
            foreach (var item in tmpList)
            {
                siteMaintenanceList.Add(item);
            }
        }
    }
    #endregion
}
