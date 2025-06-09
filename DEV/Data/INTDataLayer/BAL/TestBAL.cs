using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOMAX.G4.Data.INTDataLayer.DAL;
using SOMAX.G4.Data.INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
namespace SOMAX.G4.Data.INTDataLayer.BAL
{

  public  class TestBAL
    {
        //public static int InsertTest(TestEL objMemberQuestion, string conString)
        //{
        //    int QuestionID = 0;

        //    int outp = 0;
        //    ProcedureExecute proc = new ProcedureExecute("Ztest");
        //    proc.AddNVarcharPara("@Mode", 100, "testInsert");
        //    proc.AddBigIntegerPara("@Field", objMemberQuestion.Field);
        //    proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);

        //    QuestionID = proc.RunActionQuery(conString);
        //    QuestionID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
        //    return QuestionID;
        //}

        public DataTable getmenu(long UserId,string Connstr)
        {
            DataTable dt = null;
            ProcedureExecute proc = new ProcedureExecute("select * from [dbo].[UserMenu] Where [UserMenu].[UserInfoId]="+UserId.ToString(), true);
            dt = proc.GetTable(Connstr);
            
            return (dt);
        }
    }
}
