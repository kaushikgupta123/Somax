var workRequestTable;
var order = '1';
var orderDir = 'asc';
function LoadWorkRequestTab() {
    WorkRequestTab();
}
function WorkRequestTab() {
    $.ajax({
        url: '/Approval/LoadWorkRequest',
        type: 'POST',
        dataType: 'html',
        //data: {
        //    FiterType:  $(document).find('#FilterType').val();
        //},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#WorkRequestDiv').html(data);
                generateWorkRequestListDataTable();
            }
        },
        complete: function () {
            CloseLoader();

        },
        error: function (err) {
            CloseLoader();
        }
    });
}

function generateWorkRequestListDataTable() {
    var FilterType = $(document).find('#FilterTypeWR').val();
    var approverID = $(document).find('#hdnApporverId').val();
    if ($(document).find('#workRequestSearchTable').hasClass('dataTable')) {
        workRequestTable.destroy();
    }
    workRequestTable = $("#workRequestSearchTable").DataTable({
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
        //stateSave: true,
        scrollX: true,
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Work Request List',
                orientation: 'landscape',
                pageSize: 'A2',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'workrequest-search'
                }
            },
            {
                extend: 'print',
                title: 'Work Request List',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'workrequest-search'
                }
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Work Request List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'workrequest-search'
                }
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                customize: function (doc) {
                    doc.defaultStyle.alignment = 'left';
                    doc.styles.tableHeader.alignment = 'left';
                },
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'workrequest-search'
                },
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Work Request List'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Approval/GetApprovalRouteWRListGrid",
            "type": "POST",
            data: { approverId: approverID, FilterType: FilterType },
            "datatype": "json",
            "dataSrc": function (result) {
                let colOrder = workRequestTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                    $(document).find('#spnNumberOfWorkRequests').removeClass("badge badge-light rounded-circle tabBadgeWR").text("");
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                    if ($(document).find('#spnNumberOfWorkRequests').hasClass("tabBadgeWR")) {
                        $(document).find('#spnNumberOfWorkRequests').text(result.recordsTotal);
                    } else {
                        $(document).find('#spnNumberOfWorkRequests').addClass("badge badge-light rounded-circle tabBadgeWR").text(result.recordsTotal);
                    }
                }

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        "columns":
            [
                /*{ "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },*/
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, full, row) {
                            return '<div  class="width-100"><a class="lnk_workrequest" href="javascript:void(0)">' + data + '</a></div>'
                    }
                },
                { "data": "ChargeTo", "autoWidth": true,"bSearchable": true, "bSortable": true},
                { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right"},
                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right"},
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "Date","type": "date"},
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
           
        }
    });
}



$(document).on('click', '.lnk_workrequest', function (e) {
        var workOrderId;
        var row = $(this).parents('tr');
    var data = workRequestTable.row(row).data();
    workOrderId = data.WorkOrderId;
    var ApprovalGroupId = data.ApprovalGroupId;
    var ClientLookupId = data.ClientLookupId;
    $(document).find('#hdnWorkOrderId').val(workOrderId);
    
    $(".lnk_workrequest").parents('tr').removeAttr("style");
    row.css("background-color", "#eceff1");
    $.ajax({
        url: "/Approval/WorkRequestDetailsView",
        type: "POST",
        data: { workOrderId: workOrderId },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('.workRequestView').html(data);
            $(document).find('#hdnApprovalGroupId').val(ApprovalGroupId);
            $(document).find('#hdnClientLookupId').val(ClientLookupId);
            $("#txtWRclientlookup").text(ClientLookupId);
        },
        complete: function () {
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});

//#region Approve ans Deny
$(document).on('click', '#btnworkrequestApprove', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToApprove"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var WorkOrderId = $(document).find('#hdnWorkOrderId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();
        
        MultiLevelApproveWR(WorkOrderId, ApprovalGroupId, clientLookupId)
        
    });
});

function MultiLevelApproveWR(WorkOrderId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelApproveWR',
        data: {
            WorkRequestId: WorkOrderId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            
            $('.sweet-overlay').fadeOut();
            $('.showSweetAlert').fadeOut();
            if (data.data == "success") {
                if (data.ApproverList.length > 0) {
                    //console.log(ApproverObjList);
                    $.ajax({
                        url: "/Approval/SendWRForMultiLevelApproval",
                        type: "POST",
                        dataType: 'html',
                        data: { Approvers: data.ApproverList, WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            $('#MultiLevelApproverListPopUp').html(data);
                            $('#MultiLevelApproverListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
                        },
                        complete: function () {
                            SetControls();
                            CloseLoader();
                        },
                        error: function (jqXHR, exception) {
                            CloseLoader();
                        }
                    });
                }
                else {
                    $.ajax({
                        url: "/Approval/MultiLevelFinalApproveWorkRequest",
                        type: "GET",
                        datatype: "json",
                        data: { WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {
                            //$(document).find('#MultiLevelApproverListModalpopup').modal('hide');
                            var WorkOrderId = data.WorkOrderId;
                            if (data.data === "success") {
                                if (data.ApprovalGroupId >= 0) {
                                    SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyApproved");
                                    swal(SuccessAlertSetting, function () {
                                        CloseLoader();

                                        fxloadcurrentTab("WorkRequest");
                                    });
                                }
                                
                            }
                            else {
                                ShowGenericErrorOnAddUpdate(data);
                            }
                        },
                        complete: function () {
                            SetControls();
                            CloseLoader();
                        },
                        error: function (jqXHR, exception) {
                            CloseLoader();
                        }
                    });
                }
            }
            else {
                GenericSweetAlertMethod(data.data);
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
        }
    });
};

function SetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if ($(this).closest('form').length > 0) {
            if ($(this).valid()) {
                if (typeof areaddescribedby !== 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby !== 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
    $(document).find('.select2picker').select2({});
    //ZoomImage($(document).find('#EquipZoom'));
    //$(document).find('.dtpicker').datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
    //    "dateFormat": "mm/dd/yy",
    //    autoclose: true
    //}).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
};
function SendWRForMultiLevelApprovalOnSuccess(data) {
    $(document).find('#MultiLevelApproverListModalpopup').modal('hide');
    var WorkOrderId = data.WorkOrderId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                fxloadcurrentTab("WorkRequest");
            });
        }
        
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
    
}

$(document).on('click', '#btnworkrequestDeny', function () {
    swal({
        title: getResourceValue("CancelAlertSure"),
        text: getResourceValue("spnPleaseConfirmToDeny"),
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonClass: "btn-sm btn-primary",
        cancelButtonClass: "btn-sm",
        confirmButtonText: getResourceValue("CancelAlertYes"),
        cancelButtonText: getResourceValue("CancelAlertNo")
    }, function () {
        var WorkOrderId = $(document).find('#hdnWorkOrderId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();
        
        MultiLevelDenyWR(WorkOrderId, ApprovalGroupId, clientLookupId)
        
    });
});
function MultiLevelDenyWR(WorkOrderId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelDenyWOJob',
        data: {
            WorkOrderId: WorkOrderId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            
            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyDenied");
                swal(SuccessAlertSetting, function () {
                    fxloadcurrentTab("WorkRequest");
                });
            }
            else {
                //GenericSweetAlertMethod(getResourceValue("FailedAlert"));
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    fxloadcurrentTab("WorkRequest");
                });
            }
        },
        complete: function () {
            CloseLoader();
            SetControls();
        }
    });
}
//#endregion
