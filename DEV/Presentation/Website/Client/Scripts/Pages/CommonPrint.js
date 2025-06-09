function onCustomizeExportOptions(s, e) {
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.XLS);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.XLSX);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.RTF);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.MHT);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.HTML);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.Text);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.CSV);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.Image);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.DOCX);
}
function onCustomizeReportExportOptions(s, e) {   
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.RTF);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.MHT);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.HTML);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.Text);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.CSV);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.Image);
    e.HideFormat(DevExpress.Reporting.Viewer.ExportFormatID.DOCX);
}
function onCustomizeMenuActions(s, e) {
    var highlightEditingFieldsAction = e.GetById(DevExpress.Reporting.Viewer.ActionId.HighlightEditingFields);
    if (highlightEditingFieldsAction)
        highlightEditingFieldsAction.visible = false;
}
function onCustomizeBeforeRender(s, e) {
    $(window).on('beforeunload', function (e) {
        s.Close();
    });
}