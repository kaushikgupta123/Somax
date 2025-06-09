
/*
 * 
 */

using System;
using System.Collections.Generic;
using Database.Business;
using Common.Enumerations;

namespace Database.Transactions
{
    public class Department_RetrieveAllTemplatesWithClient : Department_TransactionBaseClass
    {


        public List<b_Department> DepartmentList { get; set; }


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Department> tmpList = new List<b_Department>();
            b_Department o = new b_Department();

            o.ClientId = dbKey.Client.ClientId;
            o.RetrieveAllTemplatesWithClient(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            DepartmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class Department_CheckKeyAndDeleteByPk : Department_TransactionBaseClass
    {
        public List<b_Department> DepartmentList { get; set; }
        public Int64 DepartmentId { get; set; }
        public bool Flag { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            Department_CheckKeyAndDeleteByPk trans = new Department_CheckKeyAndDeleteByPk()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            Department.CheckKeyAndDeleteByPK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            Flag = Department.Flag;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
        public void UpdateFromDatabaseObject(b_Department dbObj)
        {
            //this.Department.Name= dbObj.Name;
            this.Department.DepartmentId = dbObj.DepartmentId;
            this.Department.InactiveFlag = dbObj.InactiveFlag;
        }
    }

    public class RetrieveDepartmentByClientIdSiteId : Department_TransactionBaseClass
    {

        public List<b_Department> DepartmentList { get; set; }

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
            List<b_Department> tmpArray = null;
            Department.RetrieveByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            DepartmentList = new List<b_Department>();
            foreach (b_Department tmpObj in tmpArray)
            {
                DepartmentList.Add(tmpObj);
            }
        }

    }
    public class RetrieveDepartmentId : Department_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Department.DepartmentId == 0)
            {
                string message = "Department has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Department.RetrieveDepartmentId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }

    }



    public class RetrieveInActiveFlag : Department_TransactionBaseClass
    {

        public List<b_Department> DepartmentList { get; set; }

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
            List<b_Department> tmpArray = null;
            Department.RetrieveInActiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            DepartmentList = new List<b_Department>();
            foreach (b_Department tmpObj in tmpArray)
            {
                DepartmentList.Add(tmpObj);
            }
        }

    }
    

    public class ValidateNewClientLookupIdTransaction : Department_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Department.ValidateByNewClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class ValidateOldClientLookupIdTransaction : Department_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Department.ValidateByOldClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class Department_RetrieveAllCustom : Department_TransactionBaseClass
    {

        public Department_RetrieveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Department> DepartmentList { get; set; }
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
            b_Department[] tmpArray = null;
            b_Department o = new b_Department();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            DepartmentList = new List<b_Department>(tmpArray);
        }
    }
}
