/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* Grid Layout 
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Aug-07 SOM-1049 Roger Lawton       Added new class to store/retrieve grid layout information
****************************************************************************************************
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
    /// <summary>
    /// Business object that stores a record from the GridLayout table.
    /// </summary>
    public partial class GridDataLayout : DataContractBase 
    {

        #region Transaction Methods
        
        public void RetrievebyGridSiteUser(DatabaseKey dbKey) 
        {
            GridDataLayout_RetrievebyGridSiteUser trans = new GridDataLayout_RetrievebyGridSiteUser();
            trans.GridDataLayout = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.GridDataLayout);
        }

        #endregion
    }
}
