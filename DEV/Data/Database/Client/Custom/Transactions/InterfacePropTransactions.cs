using Common.Enumerations;
using Common.Structures;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{


  public class InterfaceProp_CheckInterfaceIsActive : InterfaceProp_TransactionBaseClass
  {

    public override void PerformLocalValidation()
    {
            base.UseTransaction = false;
            base.PerformLocalValidation();

    }

    public override void PerformWorkItem()
    {
      InterfaceProp.CheckInterfaceIsActive(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Postprocess()
    {
      //throw new NotImplementedException();
    }
    public override void Preprocess()
    {
      /*throw new NotImplementedException()*/;
    }
  }
  public class InterfaceProp_RetrieveInterfaceProperties : InterfaceProp_TransactionBaseClass
  {

    public override void PerformLocalValidation()
    {
      base.UseTransaction = false;
      base.PerformLocalValidation();

    }

    public override void PerformWorkItem()
    {
      InterfaceProp.RetrieveInterfaceProperties(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Postprocess()
    {
      //throw new NotImplementedException();
    }
    public override void Preprocess()
    {
      /*throw new NotImplementedException()*/
      ;
    }
  }

    public class InterfaceProp_RetrieveInterfacePropertiesByClientIdSiteId : AbstractTransactionManager
    {

        public InterfaceProp_RetrieveInterfacePropertiesByClientIdSiteId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_InterfaceProp> InterfacePropList { get; set; }

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
            b_InterfaceProp[] tmpArray = null;
            b_InterfaceProp o = new b_InterfaceProp();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;

            o.RetrieveInterfacePropertiesByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            InterfacePropList = new List<b_InterfaceProp>(tmpArray);
        }
    }
}


