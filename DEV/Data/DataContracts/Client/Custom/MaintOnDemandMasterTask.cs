/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2018 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{

    public partial class MaintOnDemandMasterTask : DataContractBase
    {
        public List<MaintOnDemandMasterTask> MaintOnDemandMasterTask_RetrieveByProcedureID(DatabaseKey dbKey)
        {
            MaintOnDemandMasterTaskRetrieveAll trans = new MaintOnDemandMasterTaskRetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.MaintOnDemandMasterTask = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<MaintOnDemandMasterTask> maintOnDemandMasterTaskList = new List<MaintOnDemandMasterTask>();
            foreach (b_MaintOnDemandMasterTask maintOnDemandMasterTask in trans.MaintOnDemandMasterTaskList)
            {
                MaintOnDemandMasterTask tmpMaintOnDemandMasterTask = new MaintOnDemandMasterTask();

                tmpMaintOnDemandMasterTask.UpdateFromDatabaseObject(maintOnDemandMasterTask);
                maintOnDemandMasterTaskList.Add(tmpMaintOnDemandMasterTask);
            }
            return maintOnDemandMasterTaskList;

        }

    }
}
