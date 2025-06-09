function widgetinfo(Widgetid, OrderPosition, Display) {
    this.Widgetid = Widgetid;
    this.OrderPosition = OrderPosition;
    this.Display = Display;
}
function LoadDashboard() {
    var DashboardId = document.getElementById("DashboardId").value;
    if (DashboardId == null || DashboardId == "") {
        DashboardId = 0;
    }
    window.location = '/Dashboard/RedirectfromDashboardChange?DashboardId=' + DashboardId;
}

$(document).on('click', '.checkContent', function () {
    var x = "";
    var lbltext = $(this).data('text')
    var lblval = $(this).val();
    if ($(this).prop('checked') == true) {
        x = '<li data-val="' + lblval + '" id="selected_' + lblval + '"><label data-wigtid="' + lblval + '">' + lbltext + '</label></li>';
        $(document).find("#PresenterList").append(x);
    }
    else {
        if ($("#selected_" + lblval).length) {

            $('#PresenterList #selected_' + lblval + '').remove();
        }
        else {
            // alert("not exists");
        }
    }
    $(document).find('#lblCounter').text($("#PresenterList").find('li').length > 0 ? $("#PresenterList").find('li').length : "None");
    if ($("#PresenterList").find('li').length == 0) {        
        $('#btnRemovewidget').attr('disabled', 'disabled');
    }
    else {
        $('#btnRemovewidget').removeAttr('disabled');        
    }
});
$(document).on('click', '#liCustomize', function () {

    $(document).find("#PresenterList li").remove();
    $(document).find(".cntSource li").remove();
    var avluncheckitem = "";
    var avlcheckitem = "";
    //For Printing Available Widgets
    for (let k = 0; k < dashboardcontentlistarr.length; ++k) {
        var avlwid = dashboardcontentlistarr[k].WidgetListingId;
        var avlwname = dashboardcontentlistarr[k].Name;
        if (dashboardcontentlistarr[k].Display == true && dashboardcontentlistarr[k].Required == false) {
            avlcheckitem = "";     
            avlcheckitem = '<li class="active" style="display:block;"><a>'+
                '<label class="m-checkbox m-checkbox--bold m-checkbox--state-success checkbox" title="Name">' +
                '<input type="checkbox" checked="checked" class="checkContent" value="' + avlwid + '" data-text="' + avlwname + '">' + avlwname + '<span></span></label></a></li>'
        }
        else if (dashboardcontentlistarr[k].Display == true && dashboardcontentlistarr[k].Required == true) {
            avlcheckitem = "";
            avlcheckitem = '<li class="active" style="display:none;"><a>' +
                '<label class="m-checkbox m-checkbox--bold m-checkbox--state-success checkbox" title="Name">' +
                '<input type="checkbox" checked="checked" class="checkContent" value="' + avlwid + '" data-text="' + avlwname + '">' + avlwname + '<span></span></label></a></li>'
        }
        else {
            avlcheckitem = "";
            avlcheckitem = '<li class="active" style="display:block;"><a>' +
                '<label class="m-checkbox m-checkbox--bold m-checkbox--state-success checkbox" title="Name">' +
                '<input type="checkbox" class="checkContent" value="' + avlwid + '" data-text="' + avlwname + '">' + avlwname + '<span></span></label></a></li>'
        }
        $(document).find(".cntSource").append(avlcheckitem);
    }

    //For Printing Selected Widgets

    for (let i = 0; i < sortDashboardcontentlist.length; ++i) {
        var wid = sortDashboardcontentlist[i].WidgetListingId;
        var wname = sortDashboardcontentlist[i].Name;

        if (sortDashboardcontentlist[i].Display == true && sortDashboardcontentlist[i].Required == false) {
            var selectedli = '<li data-val="' + wid + '" id="selected_' + wid + '"><label data-wigtid="' + wid + '">' + wname + '</label></li>';
            $(document).find("#PresenterList").append(selectedli);
        }
    }
    if (isDefault === "True")
        $('#SetAsDefault').prop('checked', true)
    else
        $('#SetAsDefault').prop('checked', false)
    $(document).find('#lblCounter').text($("#PresenterList").find('li').length > 0 ? $("#PresenterList").find('li').length : "None");
    if ($("#PresenterList").find('li').length == 0) {        
        $('#btnRemovewidget').attr('disabled', 'disabled');
    }
    else {
        $('#btnRemovewidget').removeAttr('disabled');        
    }
});
$(document).on('click', '#btnRemovewidget', function (e) {
    var widgetId = $('#PresenterList li.activeCol').find('label').attr('data-wigtid');
    $('#PresenterList li.activeCol').remove();
    $('.cntSource li a input:checkbox[value="' + widgetId + '"]').prop('checked', false);
    $(document).find('#lblCounter').text($("#PresenterList").find('li').length > 0 ? $("#PresenterList").find('li').length : "None");
    e.preventDefault();
    if ($("#PresenterList").find('li').length == 0) {
        $('#btnRemovewidget').attr('disabled', 'disabled');
    }
    else {
        $('#btnRemovewidget').removeAttr('disabled');
    }
});
$(document).on('click', '.saveConfig', function () {
    var DashboardListingId = $("#DashboardlistingId").val();
    var SetAsDefault = $('#SetAsDefault').is(':checked');
    var widgetArray = dashboardcontentlistjson;
    for (let i = 0; i < widgetArray.length; ++i) {
        if (widgetArray[i].Required == false) {
            widgetArray[i].Position = -1;
            widgetArray[i].Display = false;
        }

    }
    $("#PresenterList").find('li').each(function (index, item) {
        var widgetId = $(this).find('label').attr('data-wigtid');

        var widget = widgetArray.filter(function (x) { return x.WidgetListingId.toString() === widgetId })[0];
        widget.Position = index;
        widget.Display = true;
    });

    AddorUpdateDashboardsettings(DashboardListingId, SetAsDefault, widgetArray);
});
function AddorUpdateDashboardsettings(DashboardListingId, IsDefault, widgetArray, IsDrag = false) {
    $.ajax({
        "url": "/Dashboard/AddorUpdateDashboardsettings",
        "data": {
            DashboardListingId: DashboardListingId,
            IsDefault: IsDefault,
            SettingsInfo: JSON.stringify(widgetArray)
        },
        "dataType": "json",
        "type": "POST",
        "beforeSend": function () {
            ShowLoader();
        },
        "success": function () {
            if (IsDrag === false) {
                LoadDashboard();
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
}

//#region Sortable
$(function () {
    SetFunsionChartGlobalSettings();
    document.getElementById('DashboardList').style.display = 'block';

    if (_isLoggedInFromMobile == false) {
        $('#widget').sortable({
            //handle: ".sortableClass",
            //cancel: "#compsearchBttnNewDrop",
            animation: 500,
            cursor: "move",
            update: function (event, ui) {
                //onSort: function () {
                var widgetArray = dashboardcontentlistjson;
                for (let i = 0; i < widgetArray.length; ++i) {
                    widgetArray[i].Position = -1;
                }
                $('#widget > div').each(function (i, item) {
                    var widget = widgetArray.filter(function (x) { return x.WidgetListingId.toString() === item.id })[0];
                    widget.Position = i;
                });
                sortDashboardcontentlist = dashboardcontentlistjson.sort(SortByPosition);
                var DashboardListingId = $("#DashboardlistingId").val();
                var IsDefault = false;
                if (isDefault === 'True') {
                    IsDefault = true;
                }
                AddorUpdateDashboardsettings(DashboardListingId, IsDefault, widgetArray, true);
            }
        });
    }
    //$("#widget").disableSelection();
});


//function updateWidgetOrder() {
//    $('#widget > div').each(function (i, item) {
//        var WidgetListingId = $(this).attr('id');
//        var index = sortDashboardcontentlist.findIndex(x => x.WidgetListingId == WidgetListingId);
//        if (index > -1) {
//            sortDashboardcontentlist[index].Position = i;
//        }       
//    });
//    sortDashboardcontentlist = dashboardcontentlistjson.sort(SortByPosition);
//}
//#endregion