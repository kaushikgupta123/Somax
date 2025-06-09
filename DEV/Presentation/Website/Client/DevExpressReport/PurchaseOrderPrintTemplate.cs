using DevExpress.XtraReports.UI;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;

namespace Client.DevExpressReport
{
    public partial class PurchaseOrderPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public PurchaseOrderPrintTemplate()
        {
            InitializeComponent();
        }
        // RKL
        private void xrTableCell22_BeforePrint(object sender, CancelEventArgs e)
        {
          XRTableCell VendorAddress = (XRTableCell)sender;
          StringBuilder address = new StringBuilder();
          VendorAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
          address.AppendLine(GetCurrentColumnValue("VendorName").ToString());
          var address1 = GetCurrentColumnValue("VendorAddress1").ToString();
          if (address1 != "")
          {
            address.AppendLine(address1);
          }
          var address2 = GetCurrentColumnValue("VendorAddress2").ToString();
          if (address2 != "")
          {
            address.AppendLine(address2);
          }
          var address3 = GetCurrentColumnValue("VendorAddress3").ToString();
          if (address3 != "")
          {
            address.AppendLine(address3);
          }
          var city = GetCurrentColumnValue("VendorAddressCity").ToString();
          var state = GetCurrentColumnValue("VendorAddressState").ToString();
          var zip = GetCurrentColumnValue("VendorAddressPostCode").ToString();
          var country = GetCurrentColumnValue("VendorAddressCountry").ToString();
          var phone = GetCurrentColumnValue("VendorPhoneNumber").ToString();
          if (city != "" || state != "" || zip != "")
          {
            if (city != "")
            {
              address.Append(city + ", ");
            }
            if (state != "")
            {
              address.Append(state + " ");
            }
            if (zip != "")
            {
              address.Append(zip);
            }
          }
          if (country != "")
          {
            address.AppendLine();
            address.Append(country);
          }
          if (phone != "")
          {
            address.AppendLine();
            address.Append(phone);
          }
          VendorAddress.Text = address.ToString();
        }
        // RKL
        private void xrTableCell23_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell ShipTo = (XRTableCell)sender;
            StringBuilder address = new StringBuilder();
            ShipTo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            address.AppendLine(GetCurrentColumnValue("SiteName").ToString());
            var address1 = GetCurrentColumnValue("SiteAddress1").ToString();
            if (address1 != "")
            {
              address.AppendLine(address1);
            }
            var address2 = GetCurrentColumnValue("SiteAddress2").ToString();
            if (address2 != "")
            {
              address.AppendLine(address2);
            }
            var address3 = GetCurrentColumnValue("SiteAddress3").ToString();
            if (address3 != "")
            {
              address.AppendLine(address3);
            }
            var city = GetCurrentColumnValue("SiteAddressCity").ToString();
            var state = GetCurrentColumnValue("SiteAddressState").ToString();
            var zip = GetCurrentColumnValue("SiteAddressPostCode").ToString();
            var country = GetCurrentColumnValue("SiteAddressCountry").ToString();
            if (city != "" || state != "" || zip != "" )
            {
              if (city != "")
              {
                address.Append(city + ", ");
              }
              if (state != "")
              {
                address.Append(state + " ");
              }
              if (zip != "")
              {
                address.Append(zip);
              }
            }
            if (country != "")
            {
              address.AppendLine();
              address.Append(country);
            }
            ShipTo.Text = address.ToString();

        }

        // RKL
        private void xrTableCell24_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell BillTo = (XRTableCell)sender;
            StringBuilder address = new StringBuilder();
            BillTo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            address.AppendLine(GetCurrentColumnValue("BillToName").ToString());
            var address1 = GetCurrentColumnValue("SiteBillToAddress1").ToString();
            if (address1 != "")
            {
              address.AppendLine(address1);
            }
            var address2 = GetCurrentColumnValue("SiteBillToAddress2").ToString();
            if (address2 != "")
            {
              address.AppendLine(address2);
            }
            var address3 = GetCurrentColumnValue("SiteBillToAddress3").ToString();
            if (address3 != "")
            {
              address.AppendLine(address3);
            }
            var city = GetCurrentColumnValue("SiteBillToAddressCity").ToString();
            var state = GetCurrentColumnValue("SiteBillToAddressState").ToString();
            var zip = GetCurrentColumnValue("SiteBillToAddressPostCode").ToString();
            var country = GetCurrentColumnValue("SiteBillToAddressCountry").ToString();
            var BillToComment = GetCurrentColumnValue("SiteBillToComment").ToString();
            if (city != "" || state != "" || zip != "" )
            {
              if (city != "")
              {
                address.Append(city + ", ");
              }
              if (state != "")
              {
                address.Append(state + " ");
              }
              if (zip != "")
              {
                address.Append(zip);
              }
            }
            if (country != "")
            {
              address.AppendLine();
              address.Append(country);
            }
            if (BillToComment != "")
            {
              address.AppendLine();
              address.Append(BillToComment);
            }
            BillTo.Text = address.ToString();

        }

        /*
        private void xrTableCell48_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell VendorAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("VendorAddressCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("VendorAddressState").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("VendorAddressPostCode").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("VendorAddressCountry").ToString();
            if (country != "")
            {
                address = address + country;
            }
            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1);
            }
            VendorAddress.Text = address;

        }

        private void xrTableCell49_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell BillToAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("SiteAddressCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("SiteAddressCountry").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("SiteAddressPostCode").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("SiteAddressState").ToString();
            if (country != "")
            {
                address = address + country;
            }
            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1);
            }
            BillToAddress.Text = address;

        }

        private void xrTableCell50_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell ShipToAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("SiteBillToAddressCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("SiteBillToAddressCountry").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("SiteBillToAddressPostCode").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("SiteBillToAddressState").ToString();
            if (country != "")
            {
                address = address + country;
            }
            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1);
            }
            ShipToAddress.Text = address;
        }
*/
        private void xrPictureBox1_BeforePrint(object sender, CancelEventArgs e)
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

        private void xrPdfContent2_BeforePrint(object sender, CancelEventArgs e)
        {
            if (Convert.ToBoolean(GetCurrentColumnValue("OnPremise")))
            {
                XRPdfContent pdfContent = sender as XRPdfContent;
                string base64String = GetCurrentColumnValue("SASUrl") as string;
                byte[] bytes = Convert.FromBase64String(base64String);
                pdfContent.SourceUrl = null;
                pdfContent.Source = bytes;
            }
        }

        private void xrPdfContent1_BeforePrint(object sender, CancelEventArgs e)
        {
            if (Convert.ToBoolean(GetCurrentColumnValue("OnPremise")))
            {
                XRPdfContent pdfContent = sender as XRPdfContent;
                string base64String = this.DetailReport3.GetCurrentColumnValue("SASUrl") as string;
                byte[] bytes = Convert.FromBase64String(base64String);
                pdfContent.SourceUrl = null;
                pdfContent.Source = bytes;
            }
        }
    }
}
