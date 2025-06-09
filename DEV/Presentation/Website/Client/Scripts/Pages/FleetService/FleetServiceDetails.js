var LaborDataTable = [];
var PartsDataTable = [];
var OthersDataTable = [];
//var ServiceOrderId = 0;
var StatusArray = [];

function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "SOAttachments":
            generateAttachmentDataTable();
            hideAudit();
            $('.tabcontent').hide();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementById('btnnblock').querySelectorAll(".tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementById('btnnblock').querySelectorAll(".tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}

function openLineItem(evt, cityName, ServiceOrderLineItemId) {
    evt.preventDefault();
    var ServiceOrderId = document.getElementById('ServiceOrderId').value;

    switch (cityName) {
        case 'Labor':
            PopulateLaborDataTable($('#LaborDataTable_' + ServiceOrderLineItemId), ServiceOrderId, ServiceOrderLineItemId);
            break;
        case 'Parts':
            PopulatePartsDataTable($('#PartsDataTable_' + ServiceOrderLineItemId), ServiceOrderId, ServiceOrderLineItemId);
            break;
        case 'Other':
            PopulateOthersDataTable($('#OtherDataTable_' + ServiceOrderLineItemId), ServiceOrderId, ServiceOrderLineItemId);
            break;
    }

    var i, tabcontent, tablinks;
    tabcontent = document.getElementById('ServiceLineItem_' + ServiceOrderLineItemId).querySelectorAll(".tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementById('ServiceLineItem_' + ServiceOrderLineItemId).querySelectorAll(".tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName + '_' + ServiceOrderLineItemId).style.display = "block";

    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}

//#region Restoring state for datatable
function RestoreDefaultState() {
    if (StatusArray.length > 0) {
        var i = 0;
        for (i = 0; i < StatusArray.length; i++) {
            $(document).find('#' + StatusArray[i]).trigger('click');
        }
        StatusArray = [];
    }
}
$(document).on('click', '.addstate', function () {
    StatusArray = [];
    var elem = $(document).find('.FleetLineItemTabsArea').find('.tablinks.active');
    var i = 0;
    for (i = 0; i < elem.length; i++) {
        StatusArray.push(elem.eq(i).attr('id'));
    }
});
$(document).on('click', '.clearstate', function () {
    StatusArray = [];
    var areaChargeToId = "";
    $(document).find('#AddLabourModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
$(document).on('click', '.clearparterrordiv', function () {
    var areaChargeToId = "";
    $(document).find('#AddIssuePartModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
//#endregion

//#region Event Log
function Activity(ServiceOrderId) {
    $.ajax({
        "url": "/FleetService/LoadActivity",
        data: { 'ServiceOrderId': ServiceOrderId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            $(document).find('#activityitems').html(data);
            $(document).find("#activityList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            $(document).find('#activitydataloader').hide();
            var ftext = '';
            var bgcolor = '';
            $(document).find('#activityitems').find('.activity-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + ' !important;border:1px solid ' + bgcolor + ' !important;');
                ftext = thistext;
            });
            LoadComments(ServiceOrderId);
        }
    });
}
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
//#endregion

//#region Comments
function LoadComments(ServiceOrderId) {
    $.ajax({
        "url": "/FleetService/LoadComments",
        data: { 'ServiceOrderId': ServiceOrderId },
        type: "POST",
        datatype: "json",
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
        }
    });
}
function RedirectToDetail(ServiceOrderId, mode) {
    $.ajax({
        url: "/FleetService/FleetServiceDetails",
        type: "POST",
        dataType: 'html',
        data: { 'ServiceOrderId': ServiceOrderId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetservicemaincontainer').html(data);
            var IsRedirectFromAssetDetails = localStorage.getItem("IsRedirectFromAssetDetails");
            if (IsRedirectFromAssetDetails == "Redirect") {
                $(document).find('#spnlinkToSearch').text("Service Order");
                //localStorage.removeItem('IsRedirectFromAssetDetails');
            }
            else {
                $(document).find('#spnlinkToSearch').text(localStorage.getItem("ServiceOrderstatustext"));
            }

            //$(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            colorarray = [];
            PopulateLineItems(ServiceOrderId);
            Activity(ServiceOrderId);

            if (mode === "attachment") {
                $('#Attachmentt').trigger('click');
                $('#colorselector').val('Attachment');
            }
            if (mode === "overview") {
                $(document).find("#fleetServiceOverview").trigger('click');
                $(document).find("#tabRequest").addClass("active");
                $('#Request').show();
                $('#Completion').hide();
            }

            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('.imageDropZone').show();
                $('#Overview').hide();
                $('.tabcontent').hide();
                $(document).find('#Overview').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            SetFleetServiceDetailEnvironment();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion

//#region CKEditor
$(document).on("focus", "#txtcommentsnew", function () {
   
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('txtcomments');
    $("#txtcommentsnew").hide();
    $(".ckeditorfield").show();
});
$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var ServiceOrderId = $(document).find('#ServiceOrderId').val();
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    var noteId = 0;
    
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/FleetService/AddComments',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                'ServiceOrderId': ServiceOrderId,
                content: data,
                'ClientLookupId': ClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        //RedirectToDetail(ServiceOrderId, '');
                        LoadComments(ServiceOrderId);
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                
                ClearEditor();
                $("#txtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {
    
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#txtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });
    
    $("#txtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('txtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('txtcommentsEdit', rawHTML);
    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});

$(document).on('click', '.deletecomments', function () {
    DeleteNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {
    var data = getDataFromTheEditor();
    var ServiceOrderId = $(document).find('#ServiceOrderId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var ClientLookupId = $(document).find('#ClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/FleetService/AddComments',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { 'ServiceOrderId': ServiceOrderId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    //RedirectToDetail(ServiceOrderId, '');
                    LoadComments(ServiceOrderId);
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            //ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {
    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
            data: {
                _notesId: notesId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        //RedirectToDetail($(document).find('#ServiceOrderId').val(), '');
                        LoadComments($(document).find('#ServiceOrderId').val());
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion

//#region Side Bar Panel
function RemovedTabsActive() {
    var tablinks;
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

}
function hideAudit() {
    $('#auditlogcontainer').removeClass('col-xl-6').addClass('col-xl-12').hide();
}
function ShowAudit() {
    $('#auditlogcontainer').removeClass('col-xl-12').addClass('col-xl-6').show();
}
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    var myLineChart;
    var WObyPriorityChart;
    var fontSize = 12;

    $(document).find(".tabsArea").hide();
    $(document).find("ul.vtabs li:first").addClass("active").show();
    $(document).find(".tabsArea:first").show();
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    //#region Equiment Edit
    $(document).on('click', '#ImgShowAccount', function (e) {
        $("#ImgCloseAccount").show();
        $("#dvAccountContainer").show();
        $("#ImgShowAccount").hide();
    });
    $(document).on('click', '#ImgCloseAccount', function (e) {
        $("#dvAccountContainer").hide();
        $("#ImgShowAccount").show();
        $("#ImgCloseAccount").hide();
    });
    //#endregion
    $('#tabselector2').on('change', function (evt) {
        var cityName = $(this).val();
        openCity(evt, cityName);
    });
});
$(document).on('click', '.setImage', function () {
    var imageName = $(this).data('image');
    var EquimentId = $(document).find('#FleetServiceModel_EquipmentId').val();
    var ServiceOrderId = $(document).find('#FleetServiceModel_ServiceOrderId').val();
    $.ajax({
        url: '../base/SaveUploadedFileToServer',
        type: 'POST',
        data: { 'fileName': imageName, objectId: ServiceOrderId, TableName: "ServiceOrder", AttachObjectName: "ServiceOrder" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
            $('#EquipZoom').attr('src', data.imageurl);
            $('.equipImg').attr('src', data.imageurl);
            $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
            $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
                {
                    zoomType: "window",
                    lensShape: "round",
                    lensSize: 1000,
                    zoomWindowFadeIn: 500,
                    zoomWindowFadeOut: 500,
                    lensFadeIn: 100,
                    lensFadeOut: 100,
                    easing: true,
                    scrollZoom: true,
                    zoomWindowWidth: 450,
                    zoomWindowHeight: 450
                });
            $("#EquipZoom").on('load', function () {
                CloseLoader();
                ShowImageSaveSuccessAlert();
            });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");    
                ShowErrorAlert(errorMessage);

            }
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#deleteImg', function () {
    var EquimentId = $('#FleetServiceModel_EquipmentId').val();
    var ServiceOrderId = $('#FleetServiceModel_ServiceOrderId').val();
    var ClientOnPremise = $('#FleetServiceModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(EquimentId, ServiceOrderId);
    }
    else {
        DeleteAzureImage(EquimentId, ServiceOrderId);
    }
   
});
function DeleteAzureImage(EquimentId, ServiceOrderId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetService/DeleteImageFromAzure',
            type: 'POST',
            data: { _ServiceOrderId: ServiceOrderId, TableName: "ServiceOrder", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToDetail(ServiceOrderId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteOnPremiseImage(EquimentId, ServiceOrderId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetService/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _ServiceOrderId: ServiceOrderId, TableName: "ServiceOrder", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToDetail(ServiceOrderId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var equipmentid = $(document).find('#FleetServiceModel_EquipmentId').val();
    var ClientlookUpId = $(document).find('#FleetServiceModel_ClientLookupId').val();
    var Name = $('#FleetServiceModel_AssetName').val();
    var Meter1Type = $('#FleetServiceModel_Meter1Type').val();
    var Meter1CurrentReading = $('#FleetServiceModel_Meter1CurrentReading').val();
    var Meter2Type = $('#FleetServiceModel_Meter2Type').val();
    var Meter2CurrentReading = $('#FleetServiceModel_Meter2CurrentReading').val();
    var ScheduleDate = $('#FleetServiceModel_ScheduleDate').val();
    var Assigned = $('#FleetServiceModel_Assigned').val();
    var CompleteDate = $('#FleetServiceModel_CompleteDate').val();
    var ServiceOrderId = $(document).find('#FleetServiceModel_ServiceOrderId').val();
    var Assign_PersonnelId = $(document).find('#FleetServiceModel_Assign_PersonnelId').val();

    var Meter1Units = $(document).find('#FleetServiceModel_Meter1Units').val();
    var Meter2Units = $(document).find('#FleetServiceModel_Meter2Units').val();
    var Meter1DayDiff = $(document).find('#CompleteServiceOrderModel_Meter1DayDiff').val();
    var Meter2DayDiff = $(document).find('#CompleteServiceOrderModel_Meter2DayDiff').val();
    var EquipmentClientLookupId = $(document).find('#FleetServiceModel_EquipmentClientLookupId').val();
    var Status = $(document).find('#FleetServiceModel_Status').val();
    //equipmentid = 107266;
    $.ajax({
        url: "/FleetService/ShowAddAttachment",
        type: "GET",
        dataType: 'html',
        data: {
            EquipmentId: equipmentid,
            ClientlookUpId: ClientlookUpId,
            Name: Name,
            Meter1Type: Meter1Type,
            Meter1CurrentReading: Meter1CurrentReading,
            Meter2Type: Meter2Type,
            Meter2CurrentReading: Meter2CurrentReading,
            ScheduleDate: ScheduleDate,
            Assigned: Assigned,
            CompleteDate: CompleteDate,
            ServiceOrderId: ServiceOrderId,
            Assign_PersonnelId: Assign_PersonnelId,
            Meter1Units: Meter1Units,
            Meter2Units: Meter2Units,
            Meter1DayDiff: Meter1DayDiff,
            Meter2DayDiff: Meter2DayDiff,
            EquipmentClientLookupId: EquipmentClientLookupId,
            Status: Status

        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetservicemaincontainer').html(data);
            var IsRedirectFromAssetDetails = localStorage.getItem("IsRedirectFromAssetDetails");
            if (IsRedirectFromAssetDetails == "Redirect") {
                $(document).find('#spnlinkToSearch').text("Service Order");
            }
            else {
                $(document).find('#spnlinkToSearch').text(localStorage.getItem("ServiceOrderstatustext"));
            }
            $.validator.unobtrusive.parse(document);
        },
        complete: function () {
            ZoomImage($(document).find('#EquipZoom'));
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('submit', "#frmeqpattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmeqpattachmentadd');
    var ServiceOrderId = $('#attachmentModel_ServiceOrderId').val();
    if (!$('#frmeqpattachmentadd').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/FleetService/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var EquipmentId = data.equipmentid;
                SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToDetail(ServiceOrderId, "attachment");
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });
});

function generateAttachmentDataTable() {
    var equipmentid = $(document).find('#FleetServiceModel_EquipmentId').val();
    var ServiceOrderId = $(document).find('#FleetServiceModel_ServiceOrderId').val();
    if ($(document).find('#soAttachmentTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    var visibility;
    var attchCount = 0;
    dtAttachmentTable = $("#soAttachmentTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/PopulateAttachment?ServiceOrderId=" + ServiceOrderId,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                if (attchCount > 0) {
                    $(document).find('#asstAttachmentCount').show();
                    $(document).find('#asstAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#asstAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "false") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', '.delAttchBttn', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    DeleteServiceOrderAttachment(data.FileAttachmentId);
});
function DeleteServiceOrderAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/FleetService/DeleteAttachment',
            data: {
                _fileAttachmentId: fileAttachmentId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/FleetService/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });
});
$(document).on('click', "#btnattachmentcancel", function () {
    var ServiceOrderId = $('#attachmentModel_ServiceOrderId').val();
    swal(CancelAlertSetting, function () {
        RedirectToDetail(ServiceOrderId, "attachment");
    });
});

$(document).on('click', ".lithisfleetService", function () {
    var soID = $(this).attr('data-val');
    RedirectToDetail(soID, "attachment");
});

//#endregion

//#region Line Item
function PopulateLineItems(ServiceOrderId) {
    $.ajax({
        url: "/FleetService/ServiceLineItemDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { 'ServiceOrderId': ServiceOrderId },
        success: function (data) {
            $('#LineItemData').html(data);
            CloseLoader();
        },
        complete: function () {
            $(document).find('#lineitemataloader').hide();
            $(document).find('.select2picker').select2({});
            RestoreDefaultState();
            $.validator.unobtrusive.parse(document);
            $('.select2picker, form').change(function () {
                var areaddescribedby = $(this).attr('aria-describedby');
                if ($(this).valid()) {
                    if (typeof areaddescribedby != 'undefined') {
                        $('#' + areaddescribedby).hide();
                    }
                }
                else {
                    if (typeof areaddescribedby != 'undefined') {
                        $('#' + areaddescribedby).show();
                    }
                }
            });
        },
        error: function () {
            $(document).find('#lineitemataloader').hide();
        }
    });
}
$(document).on('click', '#btnAddLineItem', function (e) {
    $.ajax({
        url: "/FleetService/AddLineItem",
        type: "GET",
        dataType: 'html',
        data: {
            'ServiceOrderId': $('#ServiceOrderId').val(),
            'CliemtLookupId': $('#ClientLookupId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LineItemPopUp').html(data);
            $('#AddLineItemModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).valid()) {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).hide();
            }
        }
        else {
            if (typeof areaddescribedby != 'undefined') {
                $('#' + areaddescribedby).show();
            }
        }
    });
    $(document).find('.select2picker').select2({});
    SetFixedHeadStyle();
}

function LineItemAddOnSuccess(data) {
    if (data.Result === "success") {
        if (data.Mode === "Add") {
            SuccessAlertSetting.text = getResourceValue("LineItemAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LineUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            if (data.Mode === "Add") {
                $("#AddLineItemModalpopup").modal('hide');
            }
            PopulateLineItems(data.ServiceOrderId);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.btnDeleteLineItem', function (e) {
    var ServiceOrderLineItemId = $(this).data('id');
    var ServiceOrderId = $('#ServiceOrderId').val();
    swal(CancelAlertSettingForCallback, function () {
        if (ValidateForDelete(ServiceOrderLineItemId) === true) {

            $.ajax({
                url: "/FleetService/DeleteLineItem",
                type: "POST",
                dataType: 'JSON',
                data: {
                    'ServiceOrderId': ServiceOrderId,
                    'ServiceOrderLineItemId': ServiceOrderLineItemId
                },
                async: false,
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    if (data.Result === true) {
                        SuccessAlertSetting.text = getResourceValue("LineItemDeletedAlert");
                        swal(SuccessAlertSetting, function () {
                            PopulateLineItems(ServiceOrderId);
                        });
                    }
                },
                complete: function () {
                    CloseLoader();
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    });
});
function ValidateForDelete(ServiceOrderLineItemId) {
    var DeleteAllowed = true;
    $.ajax({
        url: "/FleetService/ValidateForDeleteLineItem",
        type: "POST",
        dataType: 'JSON',
        data: {
            'ServiceOrderId': $('#ServiceOrderId').val(),
            'ServiceOrderLineItemId': ServiceOrderLineItemId
        },
        async: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.DeleteAllowed === false) {
                DeleteAllowed = data.DeleteAllowed;
                var msg = '';
                if (data.LabourExists === true)
                    msg = getResourceValue('LabourExistsErrMsg');
                if (data.PartExists === true)
                    msg = msg + "\n" + getResourceValue('PartIssueExistsErrMsg');
                if (data.OthersExists === true)
                    msg = msg + "\n" + getResourceValue('OtherExistsErrMsg');
                ShowErrorAlert(msg);
                return false;
            }
            return true;
        },
        complete: function () {
            if (DeleteAllowed === false) {
                CloseLoader();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
    return DeleteAllowed;
}
$(document).on('change', '.fleetlinesystem', function () {
    var system = $(this).val();
    var id = 'FleetServiceLineItemModel_Assembly';
    $(document).find('#' + id).val('').trigger('change');
    PopulateHierarchicalData('VMRS_Code', system, id);
});
$(document).on('change', '.fleetsystemdetail', function () {
    var system = $(this).val();
    var id = 'Assembly_' + $(this).attr('id').split('_')[1];
    $(document).find('#' + id).val('').trigger('change');
    PopulateHierarchicalData('VMRS_Code', system, id);
});
function PopulateHierarchicalData(listname, level1Value, targetid) {
    $.ajax({
        url: '/FleetService/GetHierarchicalList',
        type: 'POST',
        data: {
            ListName: listname,
            List1Value: level1Value
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var elem = $(document).find('#' + targetid);
            elem.html('');
            if (data.Result === "success") {
                elem.append('<option value="">--Select--</option>');
                $.each(data.data, function (i, item) {
                    elem.append('<option value="' + item.Value + '">' + item.Text + '</option>');
                });
            }
        },
        error: function () {
            CloseLoader();
        },
        complete: function () {
            $(document).find('.select2picker').select2({});
            CloseLoader();
        },
    });
}
//#endregion

//#region Labour
function PopulateLaborDataTable(TableElem, ServiceOrderId, ServiceOrderLineItemId) {
    var Editvisibility = EditRights;
    var rCount = 0;
    if (TableElem.hasClass('dataTable')) {
        LaborDataTable[ServiceOrderLineItemId].destroy();
    }
    LaborDataTable[ServiceOrderLineItemId] = TableElem.DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/PopulateLabor",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            },
            data: function (d) {
                d.ServiceOrderId = ServiceOrderId;
                d.ServiceOrderLineItemId = ServiceOrderLineItemId;

            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (Editvisibility == "True") {
                        return '<a data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-primary addLaborBttn addstate gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a data-timecardid=' + data.TimecardId + ' data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-success editLaborBttn addstate gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a data-timecardid=' + data.TimecardId + ' data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-danger delLaborBttn addstate gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return '';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StartDate", "type": "date" },
                { "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "VMRSWorkAccomplishedCode", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Action", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" }
            ],
        initComplete: function () {
            var column = this.api().column(4);
            if (Editvisibility == "False") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $('#Labor_' + ServiceOrderLineItemId + ' button.addLaborBttn').hide();
            }
            else {
                if (Editvisibility == "True") {
                    $('#Labor_' + ServiceOrderLineItemId + ' button.addLaborBttn').show();
                    $('#Labor_' + ServiceOrderLineItemId + ' button.addLaborTimerBttn').show();
                }
                else {
                    $('#Labor_' + ServiceOrderLineItemId + ' button.addLaborBttn').hide();
                    $('#Labor_' + ServiceOrderLineItemId + ' button.addLaborTimerBttn').hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addLaborBttn', function (e) {
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddEditLabor",
        type: "GET",
        dataType: 'html',
        data: {
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LabourPopUp').html(data);
            $('#AddLabourModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editLaborBttn', function (e) {
    var TimeCardId = $(this).data('timecardid');
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddEditLabor",
        type: "GET",
        dataType: 'html',
        data: {
            'TimeCardId': TimeCardId,
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LabourPopUp').html(data);
            $('#AddLabourModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            $('#PersonnelID').val($('#ServiceOrderLabour_PersonnelID').val());
        },
        error: function () {
            CloseLoader();
        }
    });
});

function LabourAddOnSuccess(data) {
    if (data.Result === "success") {
        if (data.Mode === "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            $("#AddLabourModalpopup").modal('hide');
            RedirectToDetail(data.ServiceOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '.delLaborBttn', function (e) {
    var TimeCardId = $(this).data('timecardid');
    var ServiceOrderId = $('#ServiceOrderId').val();
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: "/FleetService/DeleteLabour",
            type: "POST",
            dataType: 'JSON',
            data: {
                'TimeCardId': TimeCardId
            },
            async: false,
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result === "success") {
                    SuccessAlertSetting.text = getResourceValue("laborDeleteSuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToDetail(ServiceOrderId, "overview");
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });

    });
});

//#endregion

//#region Issue part
function PopulatePartsDataTable(TableElem, ServiceOrderId, ServiceOrderLineItemId) {
    if (TableElem.hasClass('dataTable')) {
        PartsDataTable[ServiceOrderLineItemId].destroy();
    }
    PartsDataTable[ServiceOrderLineItemId] = TableElem.DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/PopulatePart",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.ServiceOrderId = ServiceOrderId;
                d.ServiceOrderLineItemId = ServiceOrderLineItemId;
            }
        },
        "columns":
            [
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "TransactionDate",
                    "type": "date "

                },
                { "data": "VMRSFailureCode", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addIssuePartBttn', function (e) {
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddIssueParts",
        type: "GET",
        dataType: 'html',
        data: {
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val(),
            'ClientLookupId': $('#ClientLookupId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#IssuePartPopUp').html(data);
            $('#AddIssuePartModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});

function IssuePartAddOnSuccess(data) {
    if (data.Result === "success") {
        SuccessAlertSetting.text = getResourceValue("PartIssueAddAlert");
        swal(SuccessAlertSetting, function () {
            $("#AddIssuePartModalpopup").modal('hide');
            RedirectToDetail(data.ServiceOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion

//#region Others
function PopulateOthersDataTable(TableElem, ServiceOrderId, ServiceOrderLineItemId) {
    var rCount = 0;
    var Editvisibility = EditRights;
    var IsVendorcolShow = true;
    if (TableElem.hasClass('dataTable')) {
        OthersDataTable[ServiceOrderLineItemId].destroy();
    }
    OthersDataTable[ServiceOrderLineItemId] = TableElem.DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/PopulateOthers",
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                IsVendorcolShow = response.isVendorcolShow;
                rCount = response.data.length;
                return response.data;
            },
            data: function (d) {
                d.ServiceOrderId = ServiceOrderId;
                d.ServiceOrderLineItemId = ServiceOrderLineItemId;

            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (Editvisibility == "True") {
                        return '<a data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-primary addOtherBttn addstate gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a data-othercostid=' + data.OtherCostsId + ' data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-success editOtherBttn addstate gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a data-othercostid=' + data.OtherCostsId + ' data-lineitemid=' + data.ServiceOrderLineItemId + ' class="btn btn-outline-danger delOtherBttn addstate gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';

                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Source", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Action", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" }
            ],
        initComplete: function () {

            var column = this.api().column(6);
            if (Editvisibility == "False") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $('#Other_' + ServiceOrderLineItemId + ' button.addOtherBttn').hide();
            }
            else {
                if (Editvisibility == "True") {
                    $('#Other_' + ServiceOrderLineItemId + ' button.addOtherBttn').show();
                }
                else {
                    $('#Other_' + ServiceOrderLineItemId + ' button.addOtherBttn').hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.addOtherBttn', function (e) {
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddEditOthers",
        type: "GET",
        dataType: 'html',
        data: {
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#OtherCostPopUp').html(data);
            $('#AddOtherCostModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();

        },
        error: function () {
            CloseLoader();
        }
    });
});


function OthersAddOnSuccess(data) {
    if (data.Result === "success") {
        if (data.Mode === "add") {
            SuccessAlertSetting.text = getResourceValue("OthersAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("OthersUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            $("#AddOtherCostModalpopup").modal('hide');
            RedirectToDetail(data.ServiceOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', '#sovopengrid', function () {
    generateSoVendorDataTable();
});


$(document).on('click', '.editOtherBttn', function (e) {
    var OtherCostId = $(this).data('othercostid');
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddEditOthers",
        type: "GET",
        dataType: 'html',
        data: {
            'OtherCostsId': OtherCostId,
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#OtherCostPopUp').html(data);
            $('#AddOtherCostModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            $(document).find('.select2picker').select2({});
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('click', '.delOtherBttn', function (e) {
    var OtherCostId = $(this).data('othercostid');
    var LineItemId = $(this).data('lineitemid');
    var ServiceOrderId = $('#ServiceOrderId').val();
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: "/FleetService/DeleteOther",
            type: "POST",
            dataType: 'JSON',
            data: {
                'OtherCostsId': OtherCostId
            },
            async: false,
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result === "success") {
                    SuccessAlertSetting.text = getResourceValue("OtherCostsDeleteSuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        //PopulateOthersDataTable($('#OtherDataTable_' + LineItemId), ServiceOrderId, LineItemId);
                        RedirectToDetail(ServiceOrderId, "overview");
                    });
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });

    });
});
//#endregion


//#region Action Menu
$(document).on('click', '.RemoveSchedule', function () {
    var ServiceOrderId = $(this).attr('data-id');
    swal({
        title: getResourceValue("spnAreyousure"),
        text: "Please confirm to remove schedule.",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        ShowLoader();
        var params = {
            ServiceOrderId: ServiceOrderId

        };
        objServiceOrderSchedule = JSON.stringify({ 'objServiceOrderSchedule': params });
        $.ajax({
            url: '/FleetService/RemoveScheduleSO',
            data: { ServiceOrderId: ServiceOrderId },
            contentType: 'application/json; charset=utf-8',
            type: "GET",
            datatype: "json",
            success: function (data) {
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("spnServiceorderSuccessfullyRemovedScheduled");
                    swal(SuccessAlertSetting, function () {
                        RedirectToDetail(ServiceOrderId, "overview");
                    }
                    );
                }
                else {
                    GenericSweetAlertMethod(data.error);
                }
            }
            , complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', '.AddSchedule', function () {
    var serviceorderid = $(this).attr('data-id');
    var scheduledStartDate = $(this).attr('scheduledStartDate');
    var scheduledDuration = $(this).attr('ScheduledDuration');
    $(document).find('#FleetServiceModel_ServiceOrderId').val(parseInt(serviceorderid));
    $(document).find('#Schedulestartdate').val(scheduledStartDate);
    $(document).find('.select2picker').select2({});
    $.ajax({
        "url": '/FleetService/PopulatePopUpJs',
        "data": { ServiceOrderId: serviceorderid },
        "type": 'GET',
        "dataType": 'json',
        "beforeSend": function () { ShowLoader() },
        "success": function (data) {
            var assgList = new Array();
            for (var i = 0; i < data.AssignedList.length; i++) {
                assgList[i] = data.AssignedList[i].Value;
            }
            $('#ddlUser').val(assgList).trigger("change.select2");
        },
        "complete": function () {

            $(document).find('#ScheduleModal').modal({ backdrop: 'static', keyboard: false });
            addControls();
            CloseLoader();
            SetControls();
        }
    });

});
function addControls() {
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: new Date()
    }).inputmask('mm/dd/yyyy');
}

function SoScheduleAddOnSuccess(data) {
    $('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ScheduleUpdateAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            if ($(document).find('#ActiveCard').length > 0) {
                assgList = [];
                $(document).find('#Schedulestartdate').val("").trigger('change');
                $('#ddlUser').val("").trigger("change.select2");

            }
            else {
                assgList = [];
                $(document).find('#Schedulestartdate').val("").trigger('change');
                $('#ddlUser').val("").trigger("change.select2");
                RedirectToDetail(data.serviceorderid, "overview");
            }
        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
        pageno = workOrdersSearchdt.page.info().page;
        dtTable.page(pageno).draw('page');
    }
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlUser').val(null).trigger("change.select2");
    $(document).find('#ScheduleModal').modal('hide');
}

var serviceOrderIdCan = 0;
$(document).on('click', '.soCancel', function () {
    $(document).find("#txtCancelReasonSelect").removeClass("input-validation-error");
    $(document).find('.select2picker').select2({});
    if ($(document).find('#actionCancelSO').length > 0) {
        serviceOrderIdCan = $(document).find('#FleetServiceModel_ServiceOrderId').val();
        $(document).find('#cancelModalDetailsPage').modal({ backdrop: 'static', keyboard: false });
    }
    else {
        serviceOrderIdCan = $(document).find('#FleetServiceModel_ServiceOrderId').val();
        $(document).find('#cancelModalDetailsPage').modal({ backdrop: 'static', keyboard: false });
    }
    $(document).find('#txtCancelReasonSelect').val("").trigger("change.select2");

});

function CancelServiceAddOnSuccess(data) {
    CloseLoader();
    var SId = $(document).find('#CancelServiceOrderModal_ServiceOrderId').val();
    $(document).find("#txtCancelReasonSelect").removeClass("input-validation-error");
    if (data.data == "success") {
        SuccessAlertSetting.text = getResourceValue("jobCancelsuccessmsg");
        if ($(document).find('.crdVu').length == 0) {
            assignedUsers = "";
            localStorage.setItem('ASSIGNEDUSERSLIST', assignedUsers);
            personnelList = "";
        }
        swal(SuccessAlertSetting, function () {
            $(document).find('#cancelModalDetailsPage').modal('hide');
            RedirectToDetail(SId, "overview");
            $('.modal-backdrop').remove();
        });
    }
    else {
        GenericSweetAlertMethod(data.data);
        pageno = dtTable.page.info().page;
        dtTable.page(pageno).draw('page');
    }
}

$(document).on('click', '#SoReopen', function () {
    swal({
        title: getResourceValue("spnAreyousure"),
        text: "Please confirm to reopen.",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var Status = $(document).find('#FleetServiceModel_Status').val();
        var clientLookupId = $(document).find('#FleetServiceModel_ClientLookupId').val();
        var serviceId = $(document).find('#FleetServiceModel_ServiceOrderId').val();
        $.ajax({
            url: '/FleetService/ReopenJob',
            data: { ServiceOrderId: serviceId, Status: Status, clientLookupId: clientLookupId },
            type: "GET",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                //$('#cancelModalDetailsPage').modal('hide');
                if (data.data == "success") {
                    SuccessAlertSetting.text = getResourceValue("ServiceOrderRepoensuccessmsg");
                    swal(SuccessAlertSetting, function () {
                        CloseLoader();
                        RedirectToDetail(serviceId, "overview");
                    });
                }
                else {
                    GenericSweetAlertMethod(data.data);
                    pageno = dtTable.page.info().page;
                    dtTable.page(pageno).draw('page');
                }
            },
            complete: function () {
                //RemoveUpdateAreaInfo();
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});

//#region Complete SO
$(document).on('click', '.SoComplete', function () {
    $(document).find('#CompleteServiceOrderModal').modal({ backdrop: 'static', keyboard: false });
    ResetErrorDiv($(document).find('#frmrecordmeterreading'));
    var Meter1Type = $("#FleetServiceModel_Meter1Type").val();
    var Meter2Type = $("#FleetServiceModel_Meter2Type").val();
    var Meter1CurrentReading = $("#FleetServiceModel_Meter1CurrentReading").val();
    var Meter2CurrentReading = $("#FleetServiceModel_Meter2CurrentReading").val();

    var Meter1DayDiff = $("#CompleteServiceOrderModel_Meter1DayDiff").val();
    var Meter2DayDiff = $("#CompleteServiceOrderModel_Meter2DayDiff").val();
    $(document).find("#CompleteServiceOrderModel_SOMeter1CurrentReading").removeClass("input-validation-error");
    $(document).find("#CompleteServiceOrderModel_SOMeter2CurrentReading").removeClass("input-validation-error");
    $(document).find('#CompleteServiceOrderModel_SOMeter1CurrentReading').val('0').trigger('change');
    $(document).find('#CompleteServiceOrderModel_SOMeter2CurrentReading').val('0').trigger('change');
    hideFleetControls(Meter1Type, Meter2Type);
    $(document).find(".dtpickerNew").datepicker({
        showOn: 'button',
        buttonImageOnly: true,
        maxDate: new Date(),
        buttonImage: '/Images/calender.png'
    });
    $('.dtpickerNew').datepicker('setDate', new Date());
    var timerVal = moment().format('hh:mm A');
    $(document).find('.timerId').timepicker(
        {
            template: 'dropdown',
            minuteStep: 1,
            showMeridian: true,
            defaultTime: timerVal
        });
    var M1unit = $("#FleetServiceModel_Meter1Units").val();
    if (M1unit != "") {
        M1unit = M1unit.substring(0, 2);
    }
    var M2unit = $("#FleetServiceModel_Meter2Units").val();
    if (M2unit != "") {
        M2unit = M2unit.substring(0, 2);
    }

    $(document).find('#spnMeter1dayDiff').text(getResourceValue("LastReadingAlert") + ': ' + Meter1CurrentReading + ' ' + M1unit + ' (' + Meter1DayDiff + '   ' + getResourceValue("DaysAgoAlert") + ')');
    $(document).find('#spnMeter2dayDiff').text(getResourceValue("LastReadingAlert") + ': ' + Meter2CurrentReading + ' ' + M2unit + ' (' + Meter2DayDiff + '   ' + getResourceValue("DaysAgoAlert") + ')');
    $(document).find('#CompleteServiceOrderModel_Meter1Void').prop('checked', false);
    $(document).find('#CompleteServiceOrderModel_Meter2Void').prop('checked', false);
    $(document).find('#spnMeter1dayDiff').hide();
    $(document).find('#spnMeter2dayDiff').hide();
    // SetItemControls();
});

function hideFleetControls(meter1Type, meter2Type) {
    if (meter1Type != null && meter1Type != '') {
        $(document).find('#liOdometer').show();
        $(document).find('#liOdometerVoid').show();
        $(document).find('#CompleteServiceOrderModel_MetersAssociated').val('single');
    }
    else {
        $(document).find('#liOdometer').hide();
        $(document).find('#liOdometerVoid').hide();
    }
    if (meter2Type != null && meter2Type != '') {
        $(document).find('#liHour').show();
        $(document).find('#liHourVoid').show();
        $(document).find('#CompleteServiceOrderModel_MetersAssociated').val('both');
    }
    else {
        $(document).find('#liHour').hide();
        $(document).find('#liHourVoid').hide();
        $(document).find('#CompleteServiceOrderModel_SOMeter2CurrentReading').val("1");
        $(document).find('#CompleteServiceOrderModel_SOMeter2CurrentReading').removeAttr('disabled');
    }
    if (meter1Type != null && meter1Type != '' || meter2Type != null && meter2Type != '') {
        $("#errmsg").css("display", "none");
        $("#btnAddMeterRecord").removeAttr("disabled", "disabled");
    }
    else {
        $("#errmsg").css("display", "block");
        $("#btnAddMeterRecord").attr("disabled", "disabled");
    }
}
function CompleteServiceAddOnSuccess(data) {
    CloseLoader();
    var SId = $(document).find('#FleetServiceModel_ServiceOrderId').val();
    $(document).find("#CompleteServiceOrderModel_SOMeter1CurrentReading").removeClass("input-validation-error");
    $(document).find("#CompleteServiceOrderModel_SOMeter2CurrentReading").removeClass("input-validation-error");
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("ServiceOrderCompletedAlert");
        swal(SuccessAlertSetting, function () {
            $("#CompleteServiceOrderModal").modal('hide');
            titleText = getResourceValue("AlertActive");
            RedirectToDetail(SId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        if ($(document).find("#CompleteServiceOrderModel_Meter1Void").prop("checked") == false && parseFloat($(document).find("#FM1CurrentReading").val()) >= parseFloat($(document).find("#CompleteServiceOrderModel_SOMeter1CurrentReading").val())) {
            $(document).find('#spnMeter1dayDiff').show();
            $(document).find("#CompleteServiceOrderModel_SOMeter1CurrentReading").addClass("input-validation-error");
        }
        else {
            $(document).find('#spnMeter1dayDiff').hide();
        }
        if ($(document).find("#CompleteServiceOrderModel_Meter2Void").prop("checked") == false && parseFloat($(document).find("#FM2CurrentReading").val()) >= parseFloat($(document).find("#CompleteServiceOrderModel_SOMeter2CurrentReading").val())) {
            $(document).find('#spnMeter2dayDiff').show();
            $(document).find("#CompleteServiceOrderModel_SOMeter2CurrentReading").addClass("input-validation-error");
        }
        else {
            $(document).find('#spnMeter2dayDiff').hide();
        }


    }
}
//#endregion
//#endregion

//#region Multiple Assigned
$(document).on('mouseover', '.assignedItem', function (e) {
    var thise = $(this);
    if (LRTrim(thise.find('.tooltipcards').text()).length > 0) {
        thise.find('.tooltipcards').attr('style', 'display :block !important;');
        return;
    }
    ServiceOrderId = $(this).attr('id');
    var innerText = this.innerText.trim();
    var waPersonnelId = $(this).attr('waPersonnelId');
    if (waPersonnelId == -1) {
        $.ajax({
            "url": "/FleetService/PopulateHover",
            "data": {
                ServiceOrderId: ServiceOrderId
            },
            "dataType": "json",
            "type": "POST",
            "beforeSend": function (data) {
                thise.find('.loadingImg').show();
            },
            "success": function (data) {
                if (data.personnelList != null) {
                    $('#spn' + ServiceOrderId).html(data.personnelList);
                    $('.assignedFName').css('display', 'none');
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipcards').attr('style', 'display :block !important;');
            }
        });
    }
});
//#endregion


$(document).on('click', '.ResetAllContent', function () {
    $(".errormessage").hide();
    $("#CompleteServiceOrderModal").modal('hide');
    $(document).find("#CompleteServiceOrderModel_SOMeter1CurrentReading").removeClass("input-validation-error");
    $(document).find("#CompleteServiceOrderModel_SOMeter2CurrentReading").removeClass("input-validation-error");
});

//#region Clear Validation Error
$(document).on('click', '.btncancelschedule', function () {
    var areaChargeToId = $(document).find("#ddlUser").attr('aria-describedby');
    $('#' + areaChargeToId).hide();
});
$(document).on('click', '.clearerrdiv', function () {
    var areaChargeToId = $(document).find("#ServiceOrderOthers_Source").attr('aria-describedby');
    $('#' + areaChargeToId).hide();
});

$(document).on('click', '.Cancelclearerr', function () {
    var areaChargeToId = $(document).find("#txtCancelReasonSelect").attr('aria-describedby');
    $('#' + areaChargeToId).hide();
});
$(document).on('click', '.cancellineitem', function () {
    var areaChargeToId = "";
    $(document).find('#AddLineItemModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
//#endregion

//#region Service order history
//V2-420
var dtServiceOrderHistoryTable;
var popup_ServiceOrderID = "";
var popup_AssetId = "";
var popup_AssetName = "";
var popup_Status = "";
var popup_Type = "";
var popup_CreatedDate = "";
var popup_CompletedDate = "";

$(document).on('click', '#actionServiceOrderHistory', function () {
    generateServiceOrderHistoryTable();
});

function generateServiceOrderHistoryTable() {
    if ($(document).find('#serviceOrderHistoryTable').hasClass('dataTable')) {
        dtServiceOrderHistoryTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtServiceOrderHistoryTable = $("#serviceOrderHistoryTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[6, "desc"]],
        "pageLength": 10,
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/ServiceOrderHistoryData",
            data: function (d) {
                d.AssetID = $('#FleetServiceModel_EquipmentId').val();
                d.ServiceOrderId = $('#ServiceOrderId').val();
                d.ClientLookupId = popup_ServiceOrderID;
                d.AssetClientLookupId = popup_AssetId;
                d.AssetName = popup_AssetName;
                d.Status = popup_Status;
                d.Type = popup_Type;
                d.CreatedDate = popup_CreatedDate;
                d.CompletedDate = popup_CompletedDate;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ServiceOrderId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": true,
                    "name": "0",
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img class="serviceorderchild" id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                //{ "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "1",//
                    "mRender": function (data, type, row) {
                        return '<a class=link_serviceorderhistory_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "AssetName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Open) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }

                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CompleteDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblSOHistoryfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#serviceOrderHistoryModal').hasClass('show')) {
                $(document).find('#serviceOrderHistoryModal').modal("show");
            }

            $('#serviceOrderHistoryTable tfoot th').each(function (i, v) {
                var colIndex = i;
                if (colIndex > 0) {
                    var title = $('#serviceOrderHistoryTable thead th').eq($(this).index()).text();
                    if (colIndex === 6 || colIndex === 7) {
                        $(this).html('<input type="text" class="tfootsearchtxt dtpickerNew" id="colindexso_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    }
                    else {
                        $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindexso_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    }
                    if (popup_ServiceOrderID) { $('#colindexso_1').val(popup_ServiceOrderID); }
                    if (popup_AssetId) { $('#colindexso_2').val(popup_AssetId); }
                    if (popup_AssetName) { $('#colindexso_3').val(popup_AssetName); }
                    if (popup_Status) { $('#colindexso_4').val(popup_Status); }
                    if (popup_Type) { $('#colindexso_5').val(popup_Type); }
                    if (popup_CreatedDate) { $('#colindexso_6').val(popup_CreatedDate); }
                    if (popup_CompletedDate) { $('#colindexso_7').val(popup_CompletedDate); }
                }
            });

            $('#serviceOrderHistoryTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    //EqClientLookupId = $('#colindex_0').val();
                    popup_ServiceOrderID = $('#colindexso_1').val();
                    popup_AssetId = $('#colindexso_2').val();
                    popup_AssetName = $('#colindexso_3').val();
                    popup_Status = $('#colindexso_4').val();
                    popup_Type = $('#colindexso_5').val();
                    popup_CreatedDate = $('#colindexso_6').val();
                    popup_CompletedDate = $('#colindexso_7').val();
                    dtServiceOrderHistoryTable.page('first').draw('page');
                }
            });

            $(document).find(".dtpickerNew").datepicker({
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true,
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '/Images/calender.png'
            }).inputmask('mm/dd/yyyy');
        }
    });
}

$(document).on('click', '.serviceorderchild', function (e) {
    var tr = $(this).closest('tr');
    var row = dtServiceOrderHistoryTable.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var ServiceOrderID = $(this).attr("rel");
        $.ajax({
            url: "/FleetService/GetServiceOrderHistoryInnerGrid",
            data: {
                ServiceOrderID: ServiceOrderID
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerHistoryGrid = row.child().find('.ServiceOrderHistoryinnerDataTable').DataTable(
                    {
                        "order": [[0, "asc"]],
                        paging: false,
                        searching: false,
                        "bProcessing": true,
                        responsive: true,
                        scrollY: 300,
                        "scrollCollapse": true,
                        sDom: 'Btlipr',
                        language: {
                            url: "/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],
                        "columnDefs": [
                            { className: 'text-right', targets: [3] }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api(),
                                // Total over all pages
                                total = api.column(4).data().reduce(function (a, b) {
                                    return parseFloat(a) + parseFloat(b);
                                }, 0);
                            // Update footer
                            $(api.column(4).footer()).html(total.toFixed(2));
                        },
                        initComplete: function () {
                            tr.addClass('shown');
                            row.child().find('.dataTables_scroll').addClass('tblchild-scroll');
                            CloseLoader();
                        }
                    });
            }
        });

    }
});

$(document).on('click', '.link_serviceorderhistory_detail', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var ServiceOrderId = dtServiceOrderHistoryTable.row(row).data().ServiceOrderId;
    $(document).find('#serviceOrderHistoryModal').modal("hide");
    RedirectToDetail(ServiceOrderId, 'overview');
});
//#endregion

//#region Scheduled Service 
//V2-421
var dtScheduledServiceTable;
var popup_ServiceTaskDesc = "";
var ScheduledServiceIds = [];
var ValidationArray = [];
$(document).on('click', '#btnScheduledService', function () {
    generateScheduledServiceTable();
});

function generateScheduledServiceTable() {
    ScheduledServiceIds = [];
    ValidationArray = [];
    $(".updateArea").hide();
    if ($(document).find('#scheduledServiceTable').hasClass('dataTable')) {
        dtScheduledServiceTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtScheduledServiceTable = $("#scheduledServiceTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "pageLength": 10,
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetService/ScheduledServiceGridData",
            data: function (d) {
                d.AssetID = $('#FleetServiceModel_EquipmentId').val();
                d.ServiceTaskDesc = popup_ServiceTaskDesc;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ScheduledServiceId",
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    "name": "1",
                    'render': function (data, type, full, meta) {
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';

                    }
                },
                { "data": "ServiceTask", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "Schedule", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "NextDue", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                { "data": "LastCompletedstr", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" }
            ],
        "rowCallback": function (row, data, index, full) {
        },
        initComplete: function () {
            $(document).find('#tblScheduledfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#scheduledServiceModal').hasClass('show')) {
                $(document).find('#scheduledServiceModal').modal("show");
            }

            $('#scheduledServiceTable tfoot th').each(function (i, v) {
                var colIndex = i;
                //var title = $('#scheduledServiceTable thead th').eq($(this).index()).text();
                if (colIndex === 1) {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindexss_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                    if (popup_ServiceTaskDesc) { $('#colindexss_1').val(popup_ServiceTaskDesc); }
                }
            });

            $('#scheduledServiceTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    //EqClientLookupId = $('#colindex_0').val();
                    popup_ServiceTaskDesc = $('#colindexss_1').val();
                    dtScheduledServiceTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('change', '.chksearch', function () {
    var data = dtScheduledServiceTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = ScheduledServiceIds.indexOf(data.ScheduledServiceId);
        ScheduledServiceIds.splice(index, 1);
        ValidationArray.splice(index, 1);
    }
    else {
        ScheduledServiceIds.push(data.ScheduledServiceId);
        var tmp = {
            ScheduledServiceId: data.ScheduledServiceId,
            ServiceTask: data.ServiceTask,
            RepairReason: data.RepairReason,
            System: data.System,
            Assembly: data.Assembly
        };
        ValidationArray.push(tmp);
    }
    if (ScheduledServiceIds.length > 0) {
        $(".updateArea").fadeIn();
    }
    else {
        $(".updateArea").hide();
    }
    $('.itemcount').text(ScheduledServiceIds.length);
});

$(document).on('click', '#btnAddScheduledService', function () {
    var ServiceOrderId = $('#FleetServiceModel_ServiceOrderId').val();
    if (ValidationForExistingData() === true) {
        if (ScheduledServiceIds.length > 0) {
            $.ajax({
                url: "/FleetService/BulkLineItemAdd",
                data: {
                    'ScheduledServiceIds': ScheduledServiceIds,
                    'ServiceOrderId': ServiceOrderId
                },
                type: "POST",
                beforeSend: function () {
                    ShowLoader();
                },
                datatype: "html",
                success: function (data) {
                    if (data.Result === 'success') {
                        ScheduledServiceIds = [];
                        ValidationArray = [];
                        $(".updateArea").hide();
                        SuccessAlertSetting.text = getResourceValue('TotalAlert') + ' ' + data.UpdatedItemCount + ' : ' + getResourceValue("LineItemAddedAlert");
                        swal(SuccessAlertSetting, function () {
                            $("#scheduledServiceModal").modal('hide');
                            PopulateLineItems(ServiceOrderId);
                        });
                    }
                },
                complete: function () {
                    CloseLoader();
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    }
});
function ValidationForExistingData() {
    var finalmsg = "";
    $.each(ValidationArray, function (i, item) {
        if (item.System === "" || item.RepairReason === "" || item.Assembly === "") {
            finalmsg = finalmsg + "" + "<li style='color: #d43f3a;'><i class='fa fa-circle bull'></i>" + item.ServiceTask + "</li>";           
        }
    });

    if (finalmsg !== "") {
        finalmsg = "<ul style='list-style: none;padding: 0;margin: 0;text-align:left;'>" + finalmsg + "</ul>";
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: "<div style='color: #d43f3a;text-align:left;'>" + getResourceValue('spnValidScheduledService') + " :</div>" + finalmsg,
            html: true,
            type: 'error',
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
        });
    }
    else {
        return true;
    }
}
//#endregion

//#region Labour timer
var stoptime = false;
var startDt;
var endDt;

function stopTimer() {
    if (stoptime === false) {
        stoptime = true;
    }    
}

function timerCycle() {
    if (stoptime === false) {        
        endDt = new Date();

        var res = Math.abs(endDt - startDt) / 1000;

        // get total days between two dates
        //var days = Math.floor(res / 86400);
        //document.write("<br>Difference (Days): " + days);

        // get hours        
        var hours = Math.floor(res / 3600) % 24;
        //document.write("<br>Difference (Hours): " + hours);

        // get minutes
        var minutes = Math.floor(res / 60) % 60;
        //document.write("<br>Difference (Minutes): " + minutes);

        // get seconds
        var seconds = Math.floor(res % 60);

        if (seconds < 10 || seconds === 0) {
            seconds = '0' + seconds;
        }
        if (minutes < 10 || minutes === 0) {
            minutes = '0' + minutes;
        }
        if (hours < 10 || hours === 0) {
            hours = '0' + hours;
        }

        $('#ShowTimer').html(hours + ':' + minutes + ':' + seconds);
        $('#TimeSpan').val(hours + ':' + minutes + ':' + seconds);

        setTimeout(timerCycle, 1000);
    }
}


$(document).on('click', '.addLaborTimerBttn', function (e) {
    var LineItemId = $(this).data('lineitemid');
    $.ajax({
        url: "/FleetService/AddLaborTimer",
        type: "POST",
        dataType: 'html',
        data: {
            'ServiceOrderLineItemId': LineItemId,
            'ServiceOrderId': $('#ServiceOrderId').val()
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#LabourTimerPopUp').html(data);
            $('#AddLabourTimerModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetControls();
            startDt = new Date();
            endDt = startDt;
            stoptime = false;
            timerCycle();
            $('.hiddenElements').hide();
            $('#btnLabourTimerStop').show();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on("click", ".btnLabourTimercancel", function () {
    stopTimer();
    var areaChargeToId = "";
    $(document).find('#AddLabourTimerModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
$(document).on("click", "#btnLabourTimerStop", function () {
    stopTimer();
    $('.hiddenElements').show();
    $('#btnLabourTimerStop').hide();
});
function LabourTimerAddOnSuccess(data) {
    if (data.Result === "success") {
        if (data.Mode === "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            $("#AddLabourTimerModalpopup").modal('hide');
            RedirectToDetail(data.ServiceOrderId, "overview");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion