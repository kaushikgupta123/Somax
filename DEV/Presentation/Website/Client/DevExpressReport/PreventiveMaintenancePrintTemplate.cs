using DevExpress.XtraReports.UI;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Client.DevExpressReport
{
    public partial class PreventiveMaintenancePrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public PreventiveMaintenancePrintTemplate()
        {
            InitializeComponent();
        }

        private void xrLabel10_BeforePrint(object sender, CancelEventArgs e)
        {
            XRLabel Instructions = (XRLabel)sender;
            Instructions.Text = GetCurrentColumnValue("spnAddInstructions").ToString();
        }

        private void xrPictureBox2_BeforePrint(object sender, CancelEventArgs e)
        {
            if (Convert.ToBoolean(GetCurrentColumnValue("OnPremise")) && GetCurrentColumnValue("AzureImageUrl").ToString() != "" && !GetCurrentColumnValue("AzureImageUrl").ToString().Contains("Scripts/ImageZoom/images/NoImage.jpg"))
            {
                XRPictureBox xrBox = sender as XRPictureBox;
                string base64String = GetCurrentColumnValue("AzureImageUrl") as string;
                Image img = ByteArrayToImage(Convert.FromBase64String(base64String));
                xrBox.Image = img;
            }
        }
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
