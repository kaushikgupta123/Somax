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
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2012-Mar-23          Roger Lawton        Added Lookuplist validation
 ******************************************************************************
 */

using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;

namespace Database.StoredProcedure
{

  /// <summary>
  /// Access the usp_Personnel_CreateExtended stored procedure.
  /// </summary>
  public class usp_Personnel_CreateExtended
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_Personnel_CreateExtended";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_Personnel_CreateExtended()
    {
    }

    /// <summary>
    /// Static method to call the usp_Personnel_UpdateByPK stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="updateIndexOut">int that contains the value of the @UpdateIndexOut parameter</param>
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Personnel obj
    )
    {
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetOutputParameter(SqlDbType.BigInt, "PersonnelId");
      command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
      command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
      command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Address1", obj.Address1, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Address2", obj.Address2, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Address3", obj.Address3, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "AddressCity", obj.AddressCity, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "AddressCountry", obj.AddressCountry, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "AddressPostCode", obj.AddressPostCode, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "AddressState", obj.AddressState, 63);
      command.SetInputParameter(SqlDbType.Decimal, "ApprovalLimitPO", obj.ApprovalLimitPO);
      command.SetInputParameter(SqlDbType.Decimal, "ApprovalLimitWO", obj.ApprovalLimitWO);
      command.SetInputParameter(SqlDbType.Decimal, "BasePay", obj.BasePay);
      command.SetInputParameter(SqlDbType.BigInt, "CraftId", obj.CraftId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Crew", obj.Crew, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CurrentLevel", obj.CurrentLevel, 15);
      command.SetInputParameter(SqlDbType.DateTime2, "DateofBirth", obj.DateofBirth);
      command.SetInputParameter(SqlDbType.BigInt, "Default_StoreroomId", obj.Default_StoreroomId);
      command.SetInputParameter(SqlDbType.Decimal, "DistancefromWork", obj.DistancefromWork);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
      command.SetInputParameter(SqlDbType.Bit, "Floater", obj.Floater);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Gender", obj.Gender, 15);
      command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
      command.SetStringInputParameter(SqlDbType.NVarChar, "InitialLevel", obj.InitialLevel, 15);
      command.SetInputParameter(SqlDbType.Decimal, "InitialPay", obj.InitialPay);
      command.SetInputParameter(SqlDbType.DateTime2, "LastSalaryReview", obj.LastSalaryReview);
      command.SetStringInputParameter(SqlDbType.NVarChar, "MaritalStatus", obj.MaritalStatus, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameFirst", obj.NameFirst, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameLast", obj.NameLast, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "NameMiddle", obj.NameMiddle, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Phone1", obj.Phone1, 31);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Phone2", obj.Phone2, 31);
      command.SetInputParameter(SqlDbType.Bit, "Planner", obj.Planner);
      command.SetInputParameter(SqlDbType.Bit, "Scheduler", obj.Scheduler);
      command.SetInputParameter(SqlDbType.Bit, "ScheduleEmployee", obj.ScheduleEmployee);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Section", obj.Section, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "SocialSecurityNumber", obj.SocialSecurityNumber, 15);
      command.SetInputParameter(SqlDbType.DateTime2, "StartDate", obj.StartDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
      command.SetInputParameter(SqlDbType.BigInt, "Supervisor_PersonnelId", obj.Supervisor_PersonnelId);
      command.SetInputParameter(SqlDbType.DateTime2, "TerminationDate", obj.TerminationDate);
      command.SetStringInputParameter(SqlDbType.NVarChar, "TerminationReason", obj.TerminationReason, 15);
      command.SetStringInputParameter(SqlDbType.NVarChar, "Supervisor_ClientLookupId", obj.Supervisor_ClientLookupId, 63);
      command.SetStringInputParameter(SqlDbType.NVarChar, "DefaultStoreroom_Name", obj.DefaultStoreRoom_Name, 15);

      // Execute stored procedure.
      command.ExecuteNonQuery();

      if (!string.IsNullOrEmpty(command.Parameters["@PersonnelId"].Value.ToString()))
      {
        obj.PersonnelId = (long)command.Parameters["@PersonnelId"].Value;
      }

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
    }
  }
}