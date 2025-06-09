var order = '1';
var orderDir = 'asc';
function LoadMaterialRequestTab() {
    MaterialRequestTab();
}
function MaterialRequestTab() {
    $.ajax({
        url: '/Approval/LoadMaterialRequest',
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
                $(document).find('#MaterialRequestDiv').html(data);        
                LoadMaterialRequestSearchGrid();
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

//#region MaterialRequest
var MaterialRequestSearchGridTable;
function LoadMaterialRequestSearchGrid() {
    $.ajax({
        url: '/Approval/Load_MaterialRequestSearchGrid',
        type: 'POST',
        dataType: 'html',
        
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $(document).find('#MaterialRequestSearchGrid').html(data);
            }
        },
        complete: function () {
            CloseLoader();
            PopulateMaterialRequestSearchGrid();
        },
        error: function (err) {
            CloseLoader();
        }
    });

    
}

function PopulateMaterialRequestSearchGrid() {
    var DownCount = 0; 
    var filterType = $(document).find('#FilterTypeMR').val();
    if ($(document).find('#Tbl_materialRequestSearchGrid').hasClass('dataTable')) {
        MaterialRequestSearchGridTable.destroy();
    }
    MaterialRequestSearchGridTable = $("#Tbl_materialRequestSearchGrid").DataTable({
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
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Material Request List',
                orientation: 'landscape',
                pageSize: 'A2',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'materialrequest-search'
                }
            },
            {
                extend: 'print',
                title: 'Material Request List',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'materialrequest-search'
                }
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Material Request List',
                extension: '.csv',
                exportOptions: {
                    columns: ':visible',
                    gridtoexport: 'materialrequest-search'
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
                    gridtoexport: 'materialrequest-search'
                },
                orientation: 'landscape',
                pageSize: 'A4',
                title: 'Material Request List'
            }
        ],
        "orderMulti": true,
        "ajax": {          
            "url": '/Approval/PopulateMaterialRequestSearchGrid',
            "type": "POST",

            data: function (d) {
                d.FilterTypeCase = filterType;
            },

            "datatype": "json",
            "dataSrc": function (response) {
                let colOrder = MaterialRequestSearchGridTable.order();
                orderDir = colOrder[0][1];
                order = colOrder[0][0];
                if (response.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                    $(document).find('#spnNumberOfMaterialRequests').removeClass("badge badge-light rounded-circle tabBadgeMat").text("");
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                    if ($(document).find('#spnNumberOfMaterialRequests').hasClass("tabBadgeMat")) {
                        $(document).find('#spnNumberOfMaterialRequests').text(response.recordsTotal);
                    } else {
                        $(document).find('#spnNumberOfMaterialRequests').addClass("badge badge-light rounded-circle tabBadgeMat").text(response.recordsTotal);
                    }
                }
                return response.data;
            }
        },
        
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "name": "0",
                    "mRender": function (data, type, full, row) {
                        if (data != '') {
                            return '<div  class="width-100"><a class="lnk_materialrequest" href="javascript:void(0)">' + data + '</a></div>'
                        } else {
                            return '<div  class="width-100"><a class="lnk_materialrequest" href="javascript:void(0)">Non Stock</a></div>'
                        }
                       
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3"},
                { "data": "TotalCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4"},
                { "data": "Date", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5"},
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6"},

            ],
       
        initComplete: function () {
            CloseLoader();
            SetPageLengthMenu();           
        }
    });
}

$(document).on('click', '.lnk_materialrequest', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = MaterialRequestSearchGridTable.row(row).data();
    var MaterialRequestId = data.MaterialRequestId;
    var EstimatedCostsId = data.EstimatedCostsId;
    var MaterialRequestClientlookup = data.ClientLookupId;
    if (MaterialRequestClientlookup == '') { MaterialRequestClientlookup = 'Non Stock' }
    var ApprovalGroupId = data.ApprovalGroupId;
    var ClientLookupId = data.MaterialRequestClientlookup;
    $(".lnk_materialrequest").parents('tr').removeAttr("style");
    row.css("background-color", "#eceff1");
    $.ajax({
        url: "/Approval/MaterialRequestDetails",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { MaterialRequestId: MaterialRequestId },
        success: function (data) {
            $('#MaterialRequestDetails').html(data);
            $("#txtMRclientlookup").text(MaterialRequestClientlookup);
            $(document).find('#hdnApprovalGroupId').val(ApprovalGroupId);
            $(document).find('#hdnClientLookupId').val(ClientLookupId);
            $(document).find('#hdnEstimatedCostsId').val(EstimatedCostsId);       
        },
        complete: function () {
            CloseLoader();
        }
    });
});

//#endregion

//#region Approve ans Deny
$(document).on('click', '#btnMaterialrequestApprove', function () {
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
        var EstimatedCostsId = $(document).find('#hdnEstimatedCostsId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();

        MultiLevelApproveMR(EstimatedCostsId, ApprovalGroupId, clientLookupId)

    });
});

function MultiLevelApproveMR(EstimatedCostsId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelApproveMR',
        data: {
            MaterialRequestId: EstimatedCostsId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.data == "success") {
                if (data.ApproverList.length > 0) {
                    $.ajax({
                        url: "/Approval/SendMRForMultiLevelApproval",
                        type: "POST",
                        dataType: 'html',
                        data: { Approvers: data.ApproverList, EstimatedCostsId: EstimatedCostsId, ApprovalGroupId: ApprovalGroupId },
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
                        url: "/Approval/MultiLevelFinalApproveMR",
                        type: "GET",
                        datatype: "json",
                        data: { EstimatedCostsId: EstimatedCostsId, ApprovalGroupId: ApprovalGroupId },
                        beforeSend: function () {
                            ShowLoader();
                        },
                        success: function (data) {                           
                            if (data.data === "success") {                               
                                if (data.ApprovalGroupId >= 0) {
                                    SuccessAlertSetting.text = getResourceValue("spnMaterialRequestSuccessfullyApproved");                                    
                                    swal(SuccessAlertSetting, function () {                                      
                                        fxloadcurrentTab("MaterialRequest");
                                    });
                                }

                            }
                            else {
                                ShowGenericErrorOnAddUpdate(data);
                            }
                        },
                        complete: function () {
                            SetControls();
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
    SetFixedHeadStyle();
};
function SendMRForMultiLevelApprovalOnSuccess(data) {
    $(document).find('#MultiLevelApproverListModalpopup').modal('hide');
    var EstimatedCostsId = data.EstimatedCostsId;
    if (data.data === "success") {
        if (data.ApprovalGroupId >= 0) {
            SuccessAlertSetting.text = getResourceValue("SendApprovalSuccessAlert");
            swal(SuccessAlertSetting, function () {
                CloseLoader();
                fxloadcurrentTab("MaterialRequest");
            });
        }

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }

}

$(document).on('click', '#btnMaterialrequestDeny', function () {
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
        var EstimatedCostsId = $(document).find('#hdnEstimatedCostsId').val();
        var ApprovalGroupId = $(document).find('#hdnApprovalGroupId').val();
        var clientLookupId = $(document).find("#hdnClientLookupId").val();

        MultiLevelDenyMR(EstimatedCostsId, ApprovalGroupId, clientLookupId)

    });
});
function MultiLevelDenyMR(EstimatedCostsId, ApprovalGroupId, clientLookupId) {
    $.ajax({
        url: '/Approval/MultiLevelDenyMR',
        data: {
            EstimatedCostsId: EstimatedCostsId, ApprovalGroupId: ApprovalGroupId, ClientLookupId: clientLookupId
        },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            if (data.data == "success") {
                SuccessAlertSetting.text = getResourceValue("spnMaterialRequestSuccessfullyDenied");
                swal(SuccessAlertSetting, function () {
                    fxloadcurrentTab("MaterialRequest");
                });
            }
            else {
                //GenericSweetAlertMethod(getResourceValue("FailedAlert"));
                ErrorAlertSetting.text = getResourceValue("FailedAlert");
                swal(ErrorAlertSetting, function () {
                    fxloadcurrentTab("MaterialRequest");
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