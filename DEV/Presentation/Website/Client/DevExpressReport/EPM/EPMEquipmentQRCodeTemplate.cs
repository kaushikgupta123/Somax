using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport.EPM
{
    public partial class EPMEquipmentQRCodeTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public string EquipmentBarCode { get; set; }
        public string ClientLookupId { get; set; }
        public EPMEquipmentQRCodeTemplate()
        {
            InitializeComponent();
            // Set the barcode's symbology to Code 39
            //xrBarCode1.Symbology = new Code39Generator();
        }
        public void BindData()
        {
            this.xrBarCode1.Text = EquipmentBarCode;
            this.xrLabel3.Text = ClientLookupId;
        }
    }
}
