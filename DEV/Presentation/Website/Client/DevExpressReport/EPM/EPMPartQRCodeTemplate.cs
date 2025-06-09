using DataContracts;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport.EPM
{
    public partial class EPMPartQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string PartBarCode { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string IssueUnit { get; set; }
        public EPMPartQRCodeTemplate()
        {
            InitializeComponent();
        }
        public void BindData()
        {
            this.xrBarCode1.Text = PartBarCode;
            this.xrLabel1.Text = ClientLookupId.Length > 18 ? ClientLookupId.Substring(0, 18) + ".." : ClientLookupId;
            this.xrLabel2.Text = Description.Length > 35 ? Description.Substring(0, 35) + ".." : Description;
            this.xrLabel4.Text = IssueUnit;
        }
    }
}
