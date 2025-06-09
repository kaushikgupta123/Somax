/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Extensions;

using Newtonsoft.Json;

namespace DataContracts
{
    [JsonObject]
    public partial class MasterSanLibraryTask : DataContractBase
    {
       
        public List<MasterSanLibraryTask> RetrieveAllTaskByPrevMaintLibraryId(DatabaseKey dbKey)
        {
            MasterSanLibraryTask_RetrieveByPrevMaintLibraryId trans = new MasterSanLibraryTask_RetrieveByPrevMaintLibraryId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.MasterSanLibraryTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.MasterSanLibraryTaskList);
        }
        public static List<MasterSanLibraryTask> UpdateFromDatabaseObjectList(List<b_MasterSanLibraryTask> dbObjs)
        {
            List<MasterSanLibraryTask> result = new List<MasterSanLibraryTask>();

            foreach (b_MasterSanLibraryTask dbObj in dbObjs)
            {
                MasterSanLibraryTask tmp = new MasterSanLibraryTask();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

    }
}
