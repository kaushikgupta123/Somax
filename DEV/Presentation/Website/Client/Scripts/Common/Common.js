jQuery.extend({
    getChartValues: function (url, pData) {
        pData = pData || "";
        var result = null;
        $.ajax({
            url: url,
            type: 'get',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: pData,
            success: function (data) {
                result = data;
            },
            error: function (jqXHR, exception) {
                if (jqXHR.status == 888) {
                    window.location.href = "/Error/Index";
                }
            }
        });
        return result;
    }
});

////#region UiConfig Custom Validator
//function UIConfigVal(ViewName, ColumnName, Required, Hide) {
//    this.ViewName = ViewName;
//    this.ColumnName = ColumnName;
//    this.Required = Required;
//    this.Hide = Hide;
//}
//var validateControls = [];
//function getAllViews(viewname) {
//    $.ajax({
//        url: "/Base/UiConfigAllColumns",
//        type: "POST",
//        dataType: "json",
//        async: false,
//        beforeSend: function () {
//            ShowLoader();
//        },
//        data: { viewName: viewname },
//        success: function (data) {
//            $.each(data.vList, function (index, value) {
//                var UIConfigValobj = new UIConfigVal(value.ViewName, value.ColumnName, value.Required, value.Hide);
//                validateControls.push(UIConfigValobj);
//            });
//        },
//        complete: function () {
//            CloseLoader();
//        }
//    });
//}
////#endregion
