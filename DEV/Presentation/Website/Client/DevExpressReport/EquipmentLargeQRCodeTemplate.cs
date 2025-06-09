using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport
{
    public partial class EquipmentLargeQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string EquipmentBarCode { get; set; }
        public string ClientLookupId { get; set; }
        public string EquipName { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public EquipmentLargeQRCodeTemplate()
        {
            InitializeComponent();
            xrBarCode1.Symbology = new QRCodeGenerator();
            xrBarCode1.ShowText = false;
        }
        public void BindData()
        {
            this.xrBarCode1.Text = EquipmentBarCode;
            this.xrTableCell1.Text = ClientLookupId.Length > 24 ? ClientLookupId.Substring(0, 24) + ".." : ClientLookupId;
            this.xrTableCell2.Text = EquipName;
            this.xrTableCell3.Text = SerialNumber.Length > 19 ? "Serial#: " + SerialNumber.Substring(0, 19) + ".." : "Serial#: " + SerialNumber;
            this.xrTableCell5.Text = Make.Length > 19 ? Make.Substring(0, 19) + ".." : Make;
            this.xrTableCell6.Text = Model.Length > 19 ? Model.Substring(0, 19) + ".." : Model;
        }
    }
}
