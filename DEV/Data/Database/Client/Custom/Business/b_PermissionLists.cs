/*
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
 * Business Class which retrieves permission lookup lists for the user
 * ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ========= ================== ===================================
 * 2011-Nov-29 201100000 Roger Lawton       Created partial class file
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the User table.
    /// </summary>
    [Serializable()]
    public partial class b_PermissionLists
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_PermissionLists()
        {
            ClientId = 0;
            Sites = null;
            Areas = null;
            Departments = null;
            Storerooms = null;
        }

        public List<b_Site> Sites { get; set; }
        public List<b_Area> Areas { get; set; }
        public List<b_Department> Departments { get; set; }
        public List<b_Storeroom> Storerooms { get; set; }
        public long ClientId { get; set; }
        public bool IsFound { get; set; }
    }
}