//#region declaration
var dtTable;
var selectCount = 0;
var ApproveSelectedItemArray = [];
var denySelectedItemArray = [];
var totalcount = 0;
var selectedcount = 0;
var searchresult = [];
function funcSanitationApproval(SanitationJobId, workassignedval, workassignedtext, shiftval, shifttext, scheduledate, duration) {
    this.SanitationJobId = SanitationJobId;
    this.workassignedval = workassignedval;
    this.workassignedtext = workassignedtext;
    this.shiftval = shiftval;
    this.shifttext = shifttext;
    this.scheduledate = scheduledate;
    this.duration = duration;
}
//#endregion
//#region INIT
var IsTrigger = false;
var tempAWBStatus = 1;
var tempAWBcreatedates = 0;
$(function () {
    ShowbtnLoaderclass("LoaderDrop");
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).find('.select2picker').select2({});

    IsTrigger = true;
    generateSanitationApprovalWBDataTable();
    $('#AWBStatus').val(1).trigger('change');
    $('#AWBcreatedates').val(0).trigger('change');


    if (denySelectedItemArray.length > 0) {
        $('#btnApprove').removeProp("disabled");
        $('#btndenypopupshow').removeProp("disabled");
    } else {
        $('#btnApprove').prop("disabled", "disabled");
        $('#btndenypopupshow').prop("disabled", "disabled");
    }
});
$(document).on('change', '#AWBStatus', function () {
    if (IsTrigger == false) {   
        ShowbtnLoaderclass("LoaderDrop");
        searchresult = [];
        dtTable.state.clear();
        typeValCreated = $("#adv-CreatedBy").val();
        generateSanitationApprovalWBDataTable();
    }
});

$(document).on('change', '#AWBcreatedates', function () {
    if (IsTrigger == false) {
        ShowbtnLoaderclass("LoaderDrop");
        searchresult = [];
        dtTable.state.clear();
        typeValCreated = $("#adv-CreatedBy").val();
        generateSanitationApprovalWBDataTable();
    }

});
$(document).on('click', '#sidebarCollapse', function () {
    $('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
//#endregion
//#region search
var typeValCreated;
function generateSanitationApprovalWBDataTable() {
    if ($(document).find('#tblworkbenchapproval').hasClass('dataTable'))
    {
        dtTable.destroy();
    }  
    dtTable = $("#tblworkbenchapproval").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "pagingType": "full_numbers",
        "order": [[1, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/SanitationApprovalWB/GetSanitationAppWBGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.status = IsTrigger == true ? tempAWBStatus : $('#AWBStatus').val();
                d.Createdates = IsTrigger == true ? tempAWBcreatedates : $('#AWBcreatedates').val();
                d.JobId = LRTrim($('#adv-JobId').val());
                d.description = LRTrim($('#adv-Description').val());
                d.chargeto = LRTrim($('#adv-ChargeTo').val());
                d.Chargetoname = LRTrim($('#adv-ChargeToName').val());
                d.workassigned = $('#adv-WorkAssigned').val();
                d.shift = $('#adv-shift').val();
                d.scheduledate = ValidateDate($('#adv-ScheduledDate').val());
                d.scheduleduration = LRTrim($('#adv-ScheduledDuration').val());
                d.createdate = ValidateDate($('#adv-CreateDate').val());
                d.createdby = $('#adv-CreatedBy').val();
            },
            "dataSrc": function (result) {
                searchcount = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.SanitationJobId);
                });
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                var newResult = result;
                $("#adv-CreatedBy").empty();
                $("#adv-CreatedBy").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.cbList.length; i++) {
                    var id = result.cbList[i];
                    var name = result.cbList[i];
                    $("#adv-CreatedBy").append("<option value='" + id + "'>" + name + "</option>");
                }
                if (typeValCreated) {
                    $("#adv-CreatedBy").val(typeValCreated);
                }
                HidebtnLoaderclass("LoaderDrop");
                return newResult.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "SanitationJobId",
                orderable: false,
                "bSortable": false,
                className: 'select-checkbox',
                targets: 0,
                'className': 'dt-body-center',
                'render': function (data, type, full, meta) {
                    if ($('#eqsearch-select-all').is(':checked') && totalcount == selectedcount) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {
                        if (denySelectedItemArray.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },
            { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "10px" },
            {
                "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                mRender: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "ChargeTo_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "10px" },
            { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "AssignedTo_PersonnelId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                'render': function (data, type, full, meta) {
                    var result = "";
                    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });
                    result = "<select class='select-woassigned dt-inline-dropdown'>";
                    result += "<option value='0' selected>--Select--</option>"
                    $(full.WorkAssignedList).each(function (index, value) {
                        if (exist.length > 0 && value.value == exist[0].workassignedval) {
                            result += "<option value='" + exist[0].workassignedval + "' selected>" + exist[0].workassignedtext + "</option>"
                        }
                        else if (value.value == data) {
                            result += "<option value='" + value.value + "' selected>" + value.label + "</option>"
                        }
                        else {
                            result += "<option value='" + value.value + "'>" + value.label + "</option>"
                        }
                    });
                    result += "</select>";
                    return result;
                }
            },
            {
                "data": "Shift", "autoWidth": true, "bSearchable": true, "bSortable": true,
                'render': function (data, type, full, meta) {
                    var result = "";
                    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });
                    result = "<select class='select-shift dt-inline-dropdown'>";
                    result += "<option value='0' selected>--Select--</option>"
                    $(full.ShiftListdropDown).each(function (index, value) {
                        if (exist.length > 0 && value.value == exist[0].shiftval) {
                            result += "<option value='" + exist[0].shiftval + "' selected>" + exist[0].shifttext + "</option>"
                        }
                        else if (value.value == data) {
                            result += "<option value='" + value.value + "' selected>" + value.label + "</option>"
                        }
                        else {
                            result += "<option value='" + value.value + "'>" + value.label + "</option>"
                        }
                    });
                    result += "</select>";
                    return result;
                }
            },
            {
                "data": "ScheduledDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                'render': function (data, type, full, meta) {
                    var thisdate = data;
                    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });
                    if (!data) {
                        data = "";
                    }
                    if (exist.length > 0) {
                        data = exist[0].scheduledate;
                    }
                    return "<input type='text' class='dtpicker wodtpicker dt-inline-text' autocomplete='off' readonly='readonly' value='" + data + "'>";
                }
            },
            {
                "data": "ScheduledDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                'render': function (data, type, full, meta) {
                    var thisdate = data;
                    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                        return obj.SanitationJobId === full.SanitationJobId;
                    });
                    if (exist.length > 0) {
                        data = exist[0].duration;
                    }
                    return "<input type='text' class='duration decimalinputupto2places dt-inline-text' autocomplete='off' value='" + data + "' maskedFormat='20,2'>";
                }
            },
            { "data": "CreateBy_PersonnelId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },

        ],
        initComplete: function () {
            SetPageLengthMenu();
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            if (totalcount!=0 && (totalcount == ApproveSelectedItemArray.length || (searchcount != totalcount && arrayContainsArray(ApproveSelectedItemArray, searchresult) == true))) {
                $('#eqsearch-select-all').prop('checked', true);
            } else {
                $('#eqsearch-select-all').prop('checked', false);
            }
            $(document).find('.select-woassigned,.select-shift').select2({
                minimumResultsForSearch: -1
            });
            IsTrigger = false;
        },
        'rowCallback': function (row, data, index) {
            if (ApproveSelectedItemArray.indexOf(data.SanitationJobId) != -1) {
                $(row).addClass("checked");

            }
            var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                return obj.SanitationJobId === data.SanitationJobId;
            });

            if (exist.length > 0 && data.AssignedTo_PersonnelId != exist[0].workassignedval) {
                $('td:eq(5)', row).css('background-color', '#d7f9c7');
                $('td:eq(5)', row).find('.select-woassigned').css('background-color', '#d7f9c7');
                $('td:eq(5)', row).css('color', '#fff');
            }
            if (exist.length > 0 && exist[0].shiftval != "0" && data.Shift != exist[0].shiftval) {

                $('td:eq(6)', row).css('background-color', '#d7f9c7');
                $('td:eq(6)', row).find('.select-shift').css('background-color', '#d7f9c7');
            }
            if (exist.length > 0 && (exist[0].scheduledate != null && exist[0].scheduledate != "") && data.ScheduledStartDate != exist[0].scheduledate) {

                $('td:eq(7)', row).css('background-color', '#d7f9c7');
                $('td:eq(7)', row).find('.wodtpicker').css('background-color', '#d7f9c7');
            }
            if (exist.length > 0 && data.ScheduledDuration != exist[0].duration) {

                $('td:eq(8)', row).css('background-color', '#d7f9c7');
                $('td:eq(8)', row).find('.duration').css('background-color', '#d7f9c7');
            }
        },
        "drawCallback": function (oSettings) {
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
            $(document).find('.select-woassigned,.select-shift').select2({
                minimumResultsForSearch: -1
            });
        }
    });
}

$(document).on('click', '#btnappwobenchAdvSrch', function () {
    searchresult = [];
    dtTable.state.clear();
    typeValCreated = $("#adv-CreatedBy").val();
    AWBAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
});

function AWBAdvSearch() {
    var InactiveFlag = false;
    $("#txtEqpDataSrch").val("");
    var searchitemhtml = "";
    selectCount = 0;

    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    generateSanitationApprovalWBDataTable();

    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $(".spnControlCounter").text(selectCount);
}

$(document).on('click', '#liclearadvsearch', function () {
    run = true;
    denySelectedItemArray = [];
    ApproveSelectedItemArray = [];
    $('#AWBStatus').val(1).trigger('change.select2');
    $('#AWBcreatedates').val(0).trigger('change.select2');
    dtTable.state.clear();
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    selectCount = 0;
    $(".spnControlCounter").text(selectCount);
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
    $(document).find("#adv-CreatedBy").val("").trigger('change');
    typeValCreated = $("#adv-CreatedBy").val();
}
$(document).on('click', '.btnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    if (searchtxtId == "adv-CreatedBy") {
        typeValCreated = null;
    }
    selectCount--;
    AWBAdvSearch();
});

$(document).on('click', "#btnApprove", function () {
    var WOData = ApproveSelectedItemArray;
    if (ApproveSelectedItemArray.length == 0)
        return;
    var WOData = ApproveSelectedItemArray;
    WOData = ApproveSelectedItemArray.filter(function (item) {
        return denySelectedItemArray.indexOf(item.SanitationJobId) > -1;
    });

    var validduration = true;
    $.each(WOData, function (index, item) {
        if (item.duration != "" && isNaN(item.duration) == true) {
            validduration = false;
        }
    });
    if (validduration == false) {
        ShowErrorAlert("Please select valid Scheduled Duration.");
        return false;
    }
    WOData = JSON.stringify({ 'WOData': WOData });
    ApproveSelectedItemArray = [];
    denySelectedItemArray = [];
    $.ajax({
        url: '/SanitationApprovalWB/ApproveSanitationWB',
        data: WOData,
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data.Result == "success") {             
                $('#chlAllSWBApproval').prop('checked', false);
                $('.itemcount').text(0);
                SuccessAlertSetting.text = getResourceValue("approvedsuccessmsg");
                swal(SuccessAlertSetting, function () {
                    generateSanitationApprovalWBDataTable();
                });               
            }
            else {
                SuccessAlertSetting.text = getResourceValue("UpdateAlert");
                swal(SuccessAlertSetting, function () {
                    return false;
                });
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btndenypopupshow', function () {
    if (denySelectedItemArray.length == 0)
        return;
    $('#denymodal').modal('show');
});
$(document).on('click', "#btndeny", function () {
    var DeniedReason = $('#txtDeniedReason').val();
    var DeniedComments = $('#txtDeniedcomments').val();
    var wOIds = denySelectedItemArray;
    
    if (DeniedReason) {
        denySelectedItemArray = [];
        $.ajax({
            url: '/SanitationApprovalWB/DenySanitationWB',
            data: {
                wOIds: wOIds,
                DeniedReason: DeniedReason,
                DeniedComments: DeniedComments
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    denySelectedItemArray = [];
                    $('#denymodal').modal('hide');
                    $('#btndenypopupshow').prop("disabled", "disabled");
                    $('#btnApprove').prop("disabled", "disabled");
                    $('#eqsearch-select-all').prop('checked', false);
                    $('.itemcount').text(0);
                    SuccessAlertSetting.text = getResourceValue("AlertDeniedSuccess");
                    swal(SuccessAlertSetting, function () {
                        generateSanitationApprovalWBDataTable();
                    });                 
                }
                else {

                    swal({
                        title: getResourceValue("CommonErrorAlert"),
                        text: getResourceValue("UpdateAlert"),
                        type: "error",
                        showCancelButton: false,
                        confirmButtonClass: "btn-sm btn-danger",
                        cancelButtonClass: "btn-sm",
                        confirmButtonText: getResourceValue("SaveAlertOk")                       

                    }, function () {
                        return false;
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    }
    else {
        swal({
            title: getResourceValue("ttlNoReasonSelected"),
            text: getResourceValue("AlertSelectDenied"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
    }
});
$(document).on('change', '.chksearch', function () {
    var thisTr = $(this).closest("tr");
    var data = dtTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('#eqsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        selectedcount--;
        var index = denySelectedItemArray.indexOf(data.SanitationJobId);
        denySelectedItemArray.splice(index, 1);
        ApproveSelectedItemArray.splice(index, 1);
        ApproveSelectedItemArray = $.grep(ApproveSelectedItemArray, function (obj) {
            return obj.SanitationJobId !== data.SanitationJobId;
        });
    }
    else {
        thisTr.addClass("checked");
        selectedcount = selectedcount + denySelectedItemArray.length;
        var item = new funcSanitationApproval(data.SanitationJobId, data.workassignedval, data.workassignedtext, data.shiftval, data.shifttext, data.scheduledate, data.duration);
        denySelectedItemArray.push(data.SanitationJobId);
        var data = dtTable.row($(this).parents('tr')).data();
        var SanitationJobId = data.SanitationJobId;
        var exist = $.grep(ApproveSelectedItemArray, function (obj) {
            return obj.SanitationJobId === SanitationJobId;
        });
        if (ApproveSelectedItemArray.length > 0 && exist.length > 0) {
        }
        else {
            var thissanitation = new funcSanitationApproval(SanitationJobId, "", "", "", "", "", "");
            ApproveSelectedItemArray.push(thissanitation);
        }
    }
    if (denySelectedItemArray.length > 0) {
        $('#btnApprove').removeAttr("disabled");
        $('#btndenypopupshow').removeAttr("disabled");
    } else {
        $('#btnApprove').prop("disabled", "disabled");
        $('#btndenypopupshow').prop("disabled", "disabled");
    }
    $('.itemcount').text(denySelectedItemArray.length);
});
$(document).on('click', '#eqsearch-select-all', function (e) {
    var status = $('#AWBStatus').val();
    var Createdates = $('#AWBcreatedates').val();
    var JobId = $('#adv-JobId').val();
    var description = $('#adv-Description').val();
    var chargeto = $('#adv-ChargeTo').val();
    var chargetoname = $('#adv-ChargeToName').val();
    var workassigned = $('#adv-WorkAssigned').val();
    var shift = $('#adv-shift').val();
    var scheduledate = $('#adv-ScheduledDate').val();
    var scheduleduration = $('#adv-ScheduledDuration').val();
    var createdate = $('#adv-CreateDate').val();
    var createdby = $('#adv-CreatedBy').val();
    searchresult = [];
    var checked = this.checked;
    $.ajax({
        url: '/SanitationApprovalWB/GetAppWBGrid',
        data: {
            status: status,
            Createdates: Createdates,
            JobId: JobId,
            description: description,
            chargeto: chargeto,
            Chargetoname: chargetoname,
            workassigned: workassigned,
            shift: shift,
            scheduledate: scheduledate,
            scheduleduration: scheduleduration,
            createdate: createdate,
            createdby: createdby
        },
        async: true,
        type: "GET",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    searchresult.push(item.SanitationJobId);
                    if (checked) {
                        if (denySelectedItemArray.indexOf(item.SanitationJobId) == -1)
                            denySelectedItemArray.push(item.SanitationJobId);

                        var exist = $.grep(ApproveSelectedItemArray, function (obj) {
                            return obj.SanitationJobId === item.SanitationJobId;
                        });
                        if (exist.length == 0) {
                            var scheduleddate = "";
                            var SanitationJobId = item.SanitationJobId;
                            var workAssignedval = item.WorkAssigned_PersonnelId;
                            var workAssignedtext = "";
                            var Shiftval = item.Shift;
                            var Shifttext = "";
                            if (item.ScheduledStartDate != null) {
                                scheduleddate = FormatJavascriptDate(item.ScheduledStartDate);
                            }
                            else {
                                scheduleddate = "";
                            }
                            var duration = item.ScheduledDuration;
                            var thisApproveItem = new funcSanitationApproval(SanitationJobId, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
                            ApproveSelectedItemArray.push(thisApproveItem);                           
                        }

                    } else {
                        var i = denySelectedItemArray.indexOf(item.SanitationJobId);
                        denySelectedItemArray.splice(i, 1);
                        ApproveSelectedItemArray = $.grep(ApproveSelectedItemArray, function (obj) {
                            return obj.SanitationJobId !== item.SanitationJobId;
                        });
                    }
                });
            }
        },
        complete: function () {
            $('.itemcount').text(denySelectedItemArray.length);
            dtTable.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $('#btndenypopupshow').removeAttr("disabled");
                    $('#btnApprove').removeAttr("disabled");
                    $(this).find('.chksearch').prop('checked', 'checked');
                    $('#tblworkbenchapproval').find('select').removeAttr("disabled");
                    $('#tblworkbenchapproval').find('input[type=text]').removeAttr("disabled");
                } else {
                    $('#btndenypopupshow').prop("disabled", "disabled");
                    $('#btnApprove').prop("disabled", "disabled");
                    $(this).find('.chksearch').prop('checked', false);
                    $('#tblworkbenchapproval').find('select').attr("disabled", "disabled").css("background-color", "#fff").css('color', '#333');
                    $('#tblworkbenchapproval').find('input[type=text]').attr("disabled", "disabled").css('color', '#333');
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.select-woassigned', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');   
    var data = dtTable.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var workAssignedval = $(this).val();
    var workAssignedtext = $(this).find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (ApproveSelectedItemArray.length > 0 && exist.length > 0) {      
        var index = -1;
        for (var i = 0; i < ApproveSelectedItemArray.length; ++i) {
            if (ApproveSelectedItemArray[i].SanitationJobId == SanitationJobId) {
                index = i;
                break;
            }
        }
        ApproveSelectedItemArray[index].workassignedval = workAssignedval;
        ApproveSelectedItemArray[index].workassignedtext = workAssignedtext;
    }
    else {
        var thisApproval = new funcSanitationApproval(SanitationJobId, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        ApproveSelectedItemArray.push(thisApproval);
    }
});
$(document).on('change', '.select-shift', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).val();
    var Shifttext = $(this).find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (ApproveSelectedItemArray.length > 0 && exist.length > 0) {      
        var index = -1;
        for (var i = 0; i < ApproveSelectedItemArray.length; ++i) {
            if (ApproveSelectedItemArray[i].SanitationJobId == SanitationJobId) {
                index = i;
                break;
            }
        }
        ApproveSelectedItemArray[index].shiftval = Shiftval;
        ApproveSelectedItemArray[index].shifttext = Shifttext;
    }
    else {
        var thisApproval = new funcSanitationApproval(SanitationJobId, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        ApproveSelectedItemArray.push(thisApproval);
    }
});
$(document).on('change', '.wodtpicker', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');   
    var data = dtTable.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (ApproveSelectedItemArray.length > 0 && exist.length > 0) {       
        var index = -1;
        for (var i = 0; i < ApproveSelectedItemArray.length; ++i) {
            if (ApproveSelectedItemArray[i].SanitationJobId == SanitationJobId) {
                index = i;
                break;
            }
        }
        ApproveSelectedItemArray[index].scheduledate = scheduleddate;
    }
    else {
        var thisApproval = new funcSanitationApproval(SanitationJobId, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        ApproveSelectedItemArray.push(thisApproval);
    }
});
$(document).on('change', '.duration', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var SanitationJobId = data.SanitationJobId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).val();
    if (duration == "") {
        $(this).val(0);
    }
    var exist = $.grep(ApproveSelectedItemArray, function (obj) {
        return obj.SanitationJobId === SanitationJobId;
    });
    if (ApproveSelectedItemArray.length > 0 && exist.length > 0) {       
        var index = -1;
        for (var i = 0; i < ApproveSelectedItemArray.length; ++i) {
            if (ApproveSelectedItemArray[i].SanitationJobId === SanitationJobId) {
                index = i;
                break;
            }
        }
        ApproveSelectedItemArray[index].duration = duration;
    }
    else {
        var thisApproval = new funcSanitationApproval(SanitationJobId, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        ApproveSelectedItemArray.push(thisApproval);
    }
});
//#endregion
//#region Common
//#endregion