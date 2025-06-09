using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Client.DevExpressReport
{
    public partial class PurchaseRequestPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public PurchaseRequestPrintTemplate()
        {
            InitializeComponent();
        }

        private void xrTableCell19_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell VendorAddress = (XRTableCell)sender;
            string address = "";
            var city=GetCurrentColumnValue("VendorCity").ToString();
            if(city!="")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("VendorState").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("VendorZip").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("VendorCountry").ToString();
            if (country != "")
            {
                address = address + country;
            }
            address = address.Trim();
            if(address.EndsWith(","))
            {
                address=address.Remove(address.Length - 1);
            }
            VendorAddress.Text = address;
        }

        private void xrTableCell21_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell BillToAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("BillToCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("BillToState").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("BillToZip").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("BillToCountry").ToString();
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

        private void xrTableCell20_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell ShipToAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("ShipToCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }

            var state = GetCurrentColumnValue("ShipToState").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("ShipToZip").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            var country = GetCurrentColumnValue("ShipToCountry").ToString();
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
    }
}
