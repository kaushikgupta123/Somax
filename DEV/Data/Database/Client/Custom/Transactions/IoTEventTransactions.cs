using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class IotEvent_RetrieveChunkSearchFromDetails : IoTEvent_TransactionBaseClass
    {
        public List<b_IoTEvent> IoTEventList { get; set; }
        public long ClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            IoTEvent.ClientId = IoTEvent.ClientId;
        }
        public override void PerformWorkItem()
        {
            List<b_IoTEvent> tmpList = null;
            IoTEvent.RetrieveChunkSearchIotEventDetails(this.Connection, this.Transaction, ClientId, CallerUserInfoId, CallerUserName, ref tmpList);
            IoTEventList = new List<b_IoTEvent>();
            foreach (var item in tmpList)
            {
                IoTEventList.Add(item);
            }


        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class IoTEvent_RetrieveByPKForeignkey : IoTEvent_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (IoTEvent.IoTEventId < 0)
            {
                string message = "EventInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            IoTEvent.RetrieveByPKForeignkey(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

}
