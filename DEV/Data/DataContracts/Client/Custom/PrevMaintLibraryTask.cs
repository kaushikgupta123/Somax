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
//using Common.Interfaces;
//using Business.Localization;
//using DataContracts.PaginatedResultSet;

using Newtonsoft.Json;

namespace DataContracts
{
    [JsonObject]
    public partial class PrevMaintLibraryTask : DataContractBase, IStoredProcedureValidation
    {
       
        public List<PrevMaintLibraryTask> RetrieveAllTaskByPrevMaintLibraryId(DatabaseKey dbKey)
        {
            PrevMaintLibraryTask_RetrieveByPrevMaintLibraryId trans = new PrevMaintLibraryTask_RetrieveByPrevMaintLibraryId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PrevMaintLibraryTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.PrevMaintLibraryTaskList);
        }
        public static List<PrevMaintLibraryTask> UpdateFromDatabaseObjectList(List<b_PrevMaintLibraryTask> dbObjs)
        {
            List<PrevMaintLibraryTask> result = new List<PrevMaintLibraryTask>();

            foreach (b_PrevMaintLibraryTask dbObj in dbObjs)
            {
                PrevMaintLibraryTask tmp = new PrevMaintLibraryTask();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
    }
}
