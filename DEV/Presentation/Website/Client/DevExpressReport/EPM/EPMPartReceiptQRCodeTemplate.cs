using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport.EPM
{
    public partial class EPMPartReceiptQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string PartBarCode { get; set; }
        public string PartClientLookupId { get; set; }
        public string WOClientLookupId { get; set; }
        public string WOCreateBy { get; set; }
        public string Description { get; set; }
        public string UOMConversion { get; set; }
        public EPMPartReceiptQRCodeTemplate()
        {
            InitializeComponent();
        }
        public void BindData()
        {
            this.xrBarCode1.Text = PartBarCode;
            string woCreateBy = WOCreateBy.Length > 20 ? WOCreateBy.Substring(0, 20) + ".." : WOCreateBy;
            this.xrLabel1.Text = "WO# " + WOClientLookupId + " " + woCreateBy;
            this.xrLabel2.Text = "I/O: " + UOMConversion;
            string partClientLookupId = PartClientLookupId.Length > 18 ? PartClientLookupId.Substring(0, 18) + ".." : PartClientLookupId;
            string description = Description.Length > 35 ? Description.Substring(0, 35) + ".." : Description;
            this.xrLabel3.Text = partClientLookupId + " " + description;
        }
    }
}
