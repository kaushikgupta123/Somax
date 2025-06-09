$(document).on('change', '#colorselector', function (evt) {
    $(document).find('.tabsArea').hide();
    openCity(evt, $(this).val());
    $('#' + $(this).val()).show();
});
//#region EventLog
$(document).find(".sidebar").mCustomScrollbar({
    theme: "minimal"
});
$(document).on('click', '.dismiss, .overlay', function () {
    $(document).find('.sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
});
$(document).on('click', '#eventSidebarCollapse', function () {
    $(document).find('.sidebar').addClass('active');
    $(document).find('.overlay').fadeIn();
    $(document).find('.collapse.in').toggleClass('in');
    $(document).find('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
var eventLogTable;
function GenerateEventLogGrid() {
    if ($(document).find('#ptEventTable').hasClass('dataTable')) {
        eventLogTable.destroy();
    }
    eventLogTable = $("#ptEventTable").DataTable({
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
            "url": "/PartTransfer/GetEventLogs",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.PartTransferId = LRTrim($('#hiddenparttransid').val());
                d.Event = LRTrim($('#ptEvent').val());
                d.CreatedBy = LRTrim($("#ptcreatedby").val());
                d.Created = LRTrim($("#ptcreated").val());
                d.Quantity = LRTrim($('#ptquantity').val());
                d.Comments = LRTrim($("#ptcomments").val());
            },
            "dataSrc": function (result) {
                return result.data;
            }
        },
        columnDefs: [
            {
                targets: [5],
                render: function (a, b, data, d) {
                    if (data.Event == "Issue")
                        return '<a class="btn btn-outline-primary PrintLibrary gridinnerbutton" title= "Print"> <i class="fa fa-print"></i></a>';
                }
            }
        ],
        "columns":
        [
            { "data": "Event", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "CreatedBy", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "Created", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "Quantity", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            {
                "data": "PartTransferEventLogId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {           
            SetPageLengthMenu();
        }
    });
};

$(document).on('click', '#btneventAdvSrch', function (e) {
    eventadvSearch();
    $(document).find('.sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    eventLogTable.page('first').draw('page');
});
function eventadvSearch() {
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebarevent').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnevtCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#eventadvsearchfilteritems").html(searchitemhtml);
    $(document).find(".filteritemcount").text(selectCount);
}
$(document).on('click', '.btnevtCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "advstatus") {
        typeVal = null;
    }
    eventadvSearch();
    eventLogTable.page('first').draw('page');
});
$(document).on('click', '#eventClearSearch', function () {
    cleareventAdvanceSearch();
    eventLogTable.page('first').draw('page');
});
function cleareventAdvanceSearch() {
    selectCount = 0;
    $('#advsearchsidebarevent').find('input:text').val('');
    $('#advsearchsidebarevent').find("select").val("").trigger('change');
    $(".filteritemcount").text(selectCount);
    $('#eventadvsearchfilteritems').find('span').html('');
    $('#eventadvsearchfilteritems').find('span').removeClass('tagTo');
}

//#endregion
//#region Save Part Transfer
function PartTansferSaveOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferSaveAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion Save Part Transfer
//#region Issue
$(document).on('click', '#btnIssue', function () {
    ResetModal('#issueModal');
    $(document).find('#issueModal').modal('show');
});
function PartsTransferIssueOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferIssueSaveAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#issueModal');
    }
}
//#endregion Issue
//#region Receive
$(document).on('click', '#btnReceive', function () {
    ResetModal('#receiveModal');
    $(document).find('#receiveModal').modal('show');
});
function PartsTransferReceiveOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferReceiveSaveAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, '#receiveModal');
    }
}
//#endregion Receive
//#region Send
$(document).on('click', '#btnSend', function () {
    ResetModal('#sendModal');
    $(document).find('#sendModal').modal('show');
});
function PartsTransferSendOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#sendModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferSentAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data,'#sendModal');
    }
}
//#endregion send

//#region Deny
$(document).on('click', '#liDeny', function () {
    ResetModal('#DenyModal');
    $(document).find('#DenyModal').modal('show');
});
function PartsTransferDenyOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#DenyModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferDenyAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data,'#DenyModal');
    }
}
//#endregion Deny
//#region ForceComplete
function PartsTransferForceCompleteOnSuccess(data) {
    CloseLoader();
    $('.modal').modal('hide');
    if (data.Result == "success") {
        var message;
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferConfirmAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data,'#forceCompleteModal');
    }
}
//#endregion ForceComplete
//#region Cancel
$(document).on('click', '#liCancel', function () {
    ResetModal('#cancelModal');
    $(document).find('#cancelModal').modal('show');
});
function PartsTransferCancelOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#cancelModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferCancelAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data,'#cancelModal');
    }
}
//#endregion Cancel
//#region ConfirmForceComplete
$(document).on('click', '#liConfirmForceComplete', function () {
    ResetModal('#confirmForceCompleteModal');
    $(document).find('#confirmForceCompleteModal').modal('show');
});
function ForceCompleteOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $("#confirmForceCompleteModal").modal('hide');
        $('.modal-backdrop').remove();
        SuccessAlertSetting.text = getResourceValue("PartTransferForceCompleteAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPTDetail(data.PartTransferId, "");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data,'#confirmForceCompleteModal');
    }
}
//#endregion ConfirmForceComplete
//#region Common
function RedirectToPTDetail(PartTransferId) {
    if (PartTransferId == 0) {
        window.location.href = "/PartTransfer/index?page=Inventory_Part_Transfer";
    }
    else {
        $.ajax({
            url: "/PartTransfer/GetPartTransferDetail",
            type: "POST",
            dataType: "html",
            data: { PartTransferId: PartTransferId },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#renderparttransferinfo').html(data);
            },
            complete: function () {
                CloseLoader();
                //SetPTControl();
                SetPartTransferDetailEnvironment();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
}
//#endregion Common
$(document).on('click', '#liForceComplete', function (e) {
    $('#forceCompleteModal').modal('show');
    $(this).blur();
});
$(document).on('click', '.PrintLibrary', function () {
    {
        var data = eventLogTable.row($(this).parents('tr')).data();
        $.ajax({
            url: '/PartTransfer/PrintPartTransfer',
            type: "POST",
            data: { transferNo: data.PartTransferEventLogId },
            responseType: 'arraybuffer',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (result) {
                if (result.success) {
                    PdfPrintPartTransfer(result.pdf);
                }
            },
            complete: function () {
                CloseLoader();
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $(document).find('.chksearch').prop('checked', false);
                $('.itemcount').text(0);
                SelectedWoIdToCancel = [];
            }
        });

    }
});
function PdfPrintPartTransfer(pdf) {
    var blob = b64StrtoBlob(pdf, 'application/pdf');
    var blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, "_blank");
}
function b64StrtoBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];
    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);
        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}
//#endregion


