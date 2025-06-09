using Client.Common.Constants;
using Client.Models.PurchaseOrder;

using DevExpress.XtraReports.UI;

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Client.DevExpressReport.EPM
{
    public partial class EPMPurchaseOrderPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        string clientlookup = "";
        public EPMPurchaseOrderPrintTemplate()
        {
            InitializeComponent();
        }
        // This method handles the BeforePrint event for the PageHeader control.
        // It toggles the visibility of SubBand1 and SubBand2 based on the "Number" column value.
        private void PageHeader_BeforePrint(object sender, CancelEventArgs e)
        {
            // Check if the current "Number" column value is different from the last stored value.
            if (GetCurrentColumnValue("Number").ToString() != clientlookup)
            {
                // Update the stored value with the current "Number" column value.
                clientlookup = GetCurrentColumnValue("Number").ToString();
                // Show SubBand1 and hide SubBand2.
                SubBand1.Visible = true;
                SubBand2.Visible = false;
            }
            else
            {
                // Hide SubBand1 and show SubBand2.
                SubBand1.Visible = false;
                SubBand2.Visible = true;
            }
        }
        // This method handles the BeforePrint event for the xrLabel2 control.
        // It sets the text of the label to display the current page number and total page count.
        private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            label.Text = $"Page {PrintingSystem.Document.Pages} / {PrintingSystem.PageCount}";
        }

        // This method handles the BeforePrint event for the xrTableCell34 control.
        // It concatenates multiple vendor address fields into a single multiline string.
        private void xrTableCell34_BeforePrint(object sender, CancelEventArgs e)
        {
            string text = GetCurrentColumnValue("VendorAddress1").ToString();
            if (GetCurrentColumnValue("VendorAddress2").ToString() != "")
            {
                text = text + "\n" + GetCurrentColumnValue("VendorAddress2").ToString();
            }
            if (GetCurrentColumnValue("VendorAddress3").ToString() != "")
            {
                text = text + "\n" + GetCurrentColumnValue("VendorAddress3").ToString();
            }
            xrTableCell34.Multiline = true;
            xrTableCell34.Text = text;
            xrTableCell34.CanGrow = true;
            xrTableCell34.CanShrink = true;
        }
        // This method handles the BeforePrint event for the xrTableCell47 control.
        // It concatenates multiple vendor address fields into a single multiline string.
        private void xrTableCell47_BeforePrint(object sender, CancelEventArgs e)
        {
            string text = GetCurrentColumnValue("SiteAddress1").ToString();
            if (GetCurrentColumnValue("SiteAddress2").ToString() != "")
            {
                text = text + "\n" + GetCurrentColumnValue("SiteAddress2").ToString();
            }
            if (GetCurrentColumnValue("SiteAddress3").ToString() != "")
            {
                text = text + "\n" + GetCurrentColumnValue("SiteAddress3").ToString();
            }
            xrTableCell47.Multiline = true;
            xrTableCell47.Text = text;
            xrTableCell47.CanGrow = true;
            xrTableCell47.CanShrink = true;
        }

        // This method handles the BeforePrint event for the xrTableCell40 control.
        // It concatenates the vendor's city, state, and postal code into a single string.
        private void xrTableCell40_BeforePrint(object sender, CancelEventArgs e)
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
            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1);
            }
            VendorAddress.Text = address;
        }
        // This method handles the BeforePrint event for the xrTableCell53 control.
        // It concatenates the vendor's city, state, and postal code into a single string.
        private void xrTableCell53_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell SiteAddress = (XRTableCell)sender;
            string address = "";
            var city = GetCurrentColumnValue("SiteAddressCity").ToString();
            if (city != "")
            {
                address = address + city + ", ";
            }
            var state = GetCurrentColumnValue("SiteAddressState").ToString();
            if (state != "")
            {
                address = address + state + ", ";
            }
            var zip = GetCurrentColumnValue("SiteAddressPostCode").ToString();
            if (zip != "")
            {
                address = address + zip + ", ";
            }
            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1);
            }
            SiteAddress.Text = address;
        }
        private void xrTableCell93_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            string text = "";
            if (cell != null)
            {
                // Split the text into lines
                var lines = cell.Text.Split(new[] { "~/\n" }, StringSplitOptions.None);
                // Remove vblank lines
                var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                // Join the non-empty lines back into a single string
                text = string.Join(Environment.NewLine, nonEmptyLines);
            }
            xrTableCell93.Multiline = true;
            xrTableCell93.Text = text;
            xrTableCell93.CanGrow = true;
            xrTableCell93.CanShrink = true;

        }
    }
}
