using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Client.DevExpressReport
{
    public partial class WorkOrderNewPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public WorkOrderNewPrintTemplate()
        {
            InitializeComponent();
        }

        private void xrRichText3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRRichText WOCompCriteria = (XRRichText)sender;
            WOCompCriteria.Html = GetCurrentColumnValue("WOCompCriteria").ToString();
        }

        private void xrTableCell156_BeforePrint(object sender, CancelEventArgs e)
        {
            XRLabel SignoffBy = (XRLabel)sender;
            SignoffBy.Text = GetCurrentColumnValue("SignoffBy").ToString();
        }

        private void xrTableCell158_BeforePrint(object sender, CancelEventArgs e)
        {
            XRLabel CompleteDate = (XRLabel)sender;
            CompleteDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrLabel12_BeforePrint(object sender, CancelEventArgs e)
        {
            XRLabel CompleteDate = (XRLabel)sender;
            CompleteDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrLabel14_BeforePrint(object sender, CancelEventArgs e)
        {
            XRLabel CompleteBy_PersonnelClientLookupId = (XRLabel)sender;
            CompleteBy_PersonnelClientLookupId.Text = GetCurrentColumnValue("CompleteBy_PersonnelClientLookupId").ToString();
        }

        private void SubBand6_BeforePrint(object sender, CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (Convert.ToBoolean(GetCurrentColumnValue("WOCompCriteriaTab")) == true &&
                GetCurrentColumnValue("Status").ToString() == "Complete")
            {
                subBand.Visible = true;
            }
        }

        private void SubBand7_BeforePrint(object sender, CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (Convert.ToInt64(GetCurrentColumnValue("CompleteBy_PersonnelId")) > 0)
            {
                subBand.Visible = true;
            }
        }

        private void xrPictureBox1_BeforePrint(object sender, CancelEventArgs e)
        {
            bool onpremise = Convert.ToBoolean(GetCurrentColumnValue("OnPremise"));
            if (Convert.ToBoolean(GetCurrentColumnValue("OnPremise")))
            {
                XRPictureBox xrBox = sender as XRPictureBox;
                string base64String = this.DetailReport5.GetCurrentColumnValue("PhotoUrl") as string;
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

        private void xrPdfContent2_BeforePrint(object sender, CancelEventArgs e)
        {
            if (Convert.ToBoolean(GetCurrentColumnValue("OnPremise")))
            {
                XRPdfContent pdfContent = sender as XRPdfContent;
                string base64String = this.DetailReport10.GetCurrentColumnValue("SASUrl") as string;
                byte[] bytes = Convert.FromBase64String(base64String);
                pdfContent.SourceUrl = null;
                pdfContent.Source = bytes;
            }
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

        private void xrRichText2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRRichText instruction=(XRRichText)sender;
            instruction.Html = GetCurrentColumnValue("Instructions").ToString();
        }
    }
}
