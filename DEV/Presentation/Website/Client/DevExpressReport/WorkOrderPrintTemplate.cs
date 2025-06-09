using DevExpress.XtraReports.UI;

using System;

namespace Client.DevExpressReport
{
    public partial class WorkOrderPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public WorkOrderPrintTemplate()
        {
            InitializeComponent();
        }

        private void xrRichText5_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRRichText CompleteComments = (XRRichText)sender;
            CompleteComments.Html = GetCurrentColumnValue("CompleteComments").ToString();
        }

        private void xrTableCell84_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel SignoffBy = (XRLabel)sender;
            SignoffBy.Text = GetCurrentColumnValue("SignoffBy").ToString();
        }

        private void xrTableCell85_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CompleteDate = (XRLabel)sender;
            CompleteDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrLabel54_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CreateDate = (XRLabel)sender;
            CreateDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrLabel57_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CompleteBy_PersonnelClientLookupId = (XRLabel)sender;
            CompleteBy_PersonnelClientLookupId.Text = GetCurrentColumnValue("CompleteBy_PersonnelClientLookupId").ToString();
        }

        private void xrLabel12_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CompleteDate = (XRLabel)sender;
            CompleteDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrLabel14_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CompleteBy_PersonnelClientLookupId = (XRLabel)sender;
            CompleteBy_PersonnelClientLookupId.Text = GetCurrentColumnValue("CompleteBy_PersonnelClientLookupId").ToString();
        }

        private void xrTableCell110_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel SignoffBy = (XRLabel)sender;
            SignoffBy.Text = GetCurrentColumnValue("SignoffBy").ToString();
        }

        private void xrTableCell112_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRLabel CompleteDate = (XRLabel)sender;
            CompleteDate.Text = GetCurrentColumnValue("CompleteDate").ToString();
        }

        private void xrRichText1_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XRRichText WOCompCriteria = (XRRichText)sender;
            WOCompCriteria.Html = GetCurrentColumnValue("WOCompCriteria").ToString();
        }

        private void SubBand7_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (!string.IsNullOrEmpty(GetCurrentColumnValue("CompleteComments").ToString()) &&
                GetCurrentColumnValue("Status").ToString() == "Complete")
            {
                subBand.Visible = true;
            }
        }

        private void SubBand8_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (Convert.ToBoolean(GetCurrentColumnValue("IsFoodSafetyShow")) == true &&
                Convert.ToBoolean(GetCurrentColumnValue("WOCompCriteriaTab")) == false)
            {
                subBand.Visible = true;
            }
        }

        private void SubBand10_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (Convert.ToBoolean(GetCurrentColumnValue("WOCompCriteriaTab")) == true &&
                GetCurrentColumnValue("Status").ToString() == "Complete")
            {
                subBand.Visible = true;
            }
        }

        private void SubBand11_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SubBand subBand = (SubBand)sender;
            subBand.Visible = false;
            if (Convert.ToInt64(GetCurrentColumnValue("CompleteBy_PersonnelId")) > 0)
            {
                subBand.Visible = true;
            }
        }
    }
}
