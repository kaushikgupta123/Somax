using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport
{
    public partial class PartSmallQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string EquipmentBarCode { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string PartLocation { get; set; }
        public string IssueUnit { get; set; }
        public PartSmallQRCodeTemplate()
        {
            InitializeComponent();
            xrBarCode1.Symbology = new QRCodeGenerator();
            xrBarCode1.ShowText = false;
        }
        public void BindData()
        {
            this.xrBarCode1.Text = EquipmentBarCode;
            this.xrTableCell1.Text = ClientLookupId.Length > 18 ? ClientLookupId.Substring(0, 18) + ".." : ClientLookupId;
            this.xrTableCell2.Text = Description.Length > 35 ? Description.Substring(0, 35) + ".." : Description;
            this.xrTableCell5.Text = PartLocation.Length > 35 ? PartLocation.Substring(0, 35) + ".." : PartLocation;
            this.xrTableCell3.Text = IssueUnit.Length > 15 ? "UOM : " + IssueUnit.Substring(0, 15) + ".." : "UOM : " + IssueUnit;
        }
    }
}
