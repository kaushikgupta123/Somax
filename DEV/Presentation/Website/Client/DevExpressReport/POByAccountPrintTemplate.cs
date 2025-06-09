using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Client.DevExpressReport
{
    public partial class POByAccountPrintTemplate : DevExpress.XtraReports.UI.XtraReport
    {
        public POByAccountPrintTemplate()
        {
            InitializeComponent();
            GroupField grpPersonnelId = new GroupField("AccountClientLookUpId", XRColumnSortOrder.Ascending);
            this.GroupHeader2.GroupFields.Add(grpPersonnelId);

            GroupField grpPO = new GroupField("ClientLookUpId", XRColumnSortOrder.Ascending);
            this.GroupHeader1.GroupFields.Add(grpPO);
        }

    }
}
