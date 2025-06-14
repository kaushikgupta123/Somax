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


using System.Collections.Generic;
using Common.Enumerations;
using System.Linq;
using System.Text;
using Database.Business;

namespace Database.Transactions
{
    public class Client_RetrieveByClientSearch : AbstractTransactionManager
    {
        public Client_RetrieveByClientSearch()
        {
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public List<b_Client> ClientList { get; set; }
        public b_Client Client { get; set; }

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

            Client.RetrieveByClientSearchFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ClientList = new List<b_Client>();
            foreach (b_Client tmpObj in tmpArray)
            {
                ClientList.Add(tmpObj);
            }
        }
    }
}
