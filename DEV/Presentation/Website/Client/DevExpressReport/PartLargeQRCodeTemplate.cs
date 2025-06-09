using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport
{
    public partial class PartLargeQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string EquipmentBarCode { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string PartLocation { get; set; }
        public string IssueUnit { get; set; }
        public string MinQty { get; set; }
        public string MaxQty { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public PartLargeQRCodeTemplate()
        {
            InitializeComponent();
            xrBarCode1.Symbology = new QRCodeGenerator();
            xrBarCode1.ShowText = false;
        }
        public void BindData()
        {
            this.xrBarCode1.Text = EquipmentBarCode;
            this.xrTableCell1.Text = ClientLookupId;
            this.xrTableCell2.Text = Description.Length > 35 ? Description.Substring(0, 35) + ".." : Description;
            this.xrTableCell13.Text = PartLocation.Length > 70 ? PartLocation.Substring(0, 70) + ".." : PartLocation;
            var IssueUnitText = IssueUnit.Length > 8 ? " UOM : " + IssueUnit.Substring(0, 8) + ".." : " UOM : " + IssueUnit;
            this.xrTableCell3.Text = "MIN : " + MinQty + " MAX : " + MaxQty + IssueUnitText;
            this.xrTableCell5.Text = Manufacturer.Length > 29 ? "MFG : " + Manufacturer.Substring(0, 29) + ".." : "MFG : " + Manufacturer;
            this.xrTableCell6.Text = ManufacturerId.Length > 29 ? "MFG ID : " + ManufacturerId.Substring(0, 29) + ".." : "MFG ID : " + ManufacturerId;
        }
    }
}
