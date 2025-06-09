using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class ErrorMessageConstants
    {
        public const string No_Record_Found = "An error has occured. Please contact the administrator.";
        public const string ACCOUNT_IACTIVE = "Account is inactive.";
        public const string EXCEED_FAILURE_ATTEMPTS = "Account has been locked due to multiple failed login attempts. Please contact your administrator to have the account unlocked.";        
        public const string WRONG_PASSWORD = "Password is incorrect"; //V2-468
        public const string Passwords_Not_Match = "Password Not Match";
        public const string ResetPasswordSuccess = "Password Reset Successfully";
        public const string Password_Reset_Fail = "Password Reset Fail";
        public const string Temporary_Password_Expired = "Temporary password expired";  /*V2-332*/
        public const string UserName_Incorrect = "User Name is incorrect";//V2-468
        public const string Unauthorised_Msg= "User is not authorized to login to the system";
        public const string UNAUTHORIZEDGOOGLEACCOUNT = "is not registered with Somax.";
        public const string Client_InActive_Msg = "User system has been inactivated and is unavailable.";//V2-858
        public const string Site_InActive_Msg = "User system has been inactivated and is unavailable.";//V2-858
        public const string UserNotRegistered = "User is not registered with Somax.";
        public const string Authentication_Failed= "Authentication Failed";//1079
        public const string ServiceUnavailable = "ServiceUnavailable";//1079
        public const string The_Process_Is_Not_Enabled_For_The_Client = "The process is not enabled for the client";//1079
        public const string Process_Completed_Successfully = "Process Completed Successfully";//1079
        public const string File_Exported_To_SFTP= "File exported to SFTP";//1079
        public const string Schedule_Record_Exists = "A Schedule record exists for this master and charge to";
        public const string Invalid_Preventive_Maintenance_Id = "Invalid Preventive Maintenance Id";
        public const string Not_All_Work_Orders_Generated_Successfully="Not all Work Orders generated successfully";
        public const string The_Search_Returned_No_Results="The search returned no results.";
        public const string Failed_to_return_part= "Failed to return part ";
        public const string Json_file_not_match_required_schema = "The json file does not match required schema.";//1079
        public const string Json_data_inserted_into_temporary_table = "Json data inserted into temporary table.";//1079
        public const string Conversion_json_data_temporary_tables_failed = "Conversion of json data to temporary data tables failed.";//1079
        public const string Process_Complete_EPM_Invoice_Import_Successfully = "Process Completed – EPM Invoice import successful";//1079
        public const string Refence_schema_requred_json_validation_not_found = "The refence schema requred for json validation is not found.";//1079
        public const string No_file_found_source_directory= "No file found at source directory.";//1079
        public const string File_Read_failure = "File read failure.";//1079
        public const string Data_Is_null = "Data is null.";//1079
        public const string Error_parsing_file = "Error parsing file";//1079
        public const string Vendor_Punchout_URL_Not_Empty = "Vendor Punchout URL can't be empty."; //1119


    }
}
