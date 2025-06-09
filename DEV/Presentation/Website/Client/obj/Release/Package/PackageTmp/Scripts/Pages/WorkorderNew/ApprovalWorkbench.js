//#region declaration
var dtTable;
var equiToupdate = [];
var selectCount = 0;
var changedRows = [];
var totalcount = 0;
var selectedcount = 0;
var searchresult = [];
var dataToUpdate = [];
function funcworkorder(woid, workassignedval, workassignedtext, shiftval, shifttext, scheduledate, duration) {
    this.woid = woid;
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
    generateWorkordersDataTable();
    $('#AWBStatus').val(1).trigger('change');
    $('#AWBcreatedates').val(0).trigger('change');
});
$(document).on('click', '#sidebarCollapse', function () {
    $('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
//#endregion
//#region search
$(document).on('click', '#eqsearch-select-all', function (e) {
    var status = LRTrim($('#AWBStatus').val());
    var Createdates = LRTrim($('#AWBcreatedates').val());
    var workorder = LRTrim($('#adv-workworder').val());
    var description = LRTrim($('#adv-Description').val());
    var chargeto = LRTrim($('#adv-ChargeTo').val());
    var chargetoname = LRTrim($('#adv-ChargeToName').val());
    var workassigned = $('#adv-WorkAssigned').val();
    var shift = $('#adv-shift').val();
    var scheduledate = $('#adv-ScheduledDate').val();
    var scheduleduration = LRTrim($('#adv-ScheduledDuration').val());
    var createdate = LRTrim($('#adv-CreateDate').val());
    var createdby = $('#adv-CreatedBy').val();
    searchresult = [];
    var checked = this.checked;
    $.ajax({
        url: '/ApprovalWorkbench/GetWorkOrders',
        data: {
            status: status,
            Createdates: Createdates,
            workorder: workorder,
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
                    searchresult.push(item.WorkOrderId);
                    if (checked) {
                        if (equiToupdate.indexOf(item.WorkOrderId) == -1)
                            equiToupdate.push(item.WorkOrderId);
                        var exist = $.grep(dataToUpdate, function (obj) {
                            return obj.woid === item.WorkOrderId;
                        });
                        if (exist.length == 0) {
                            var workOrderid = item.WorkOrderId;
                            var workAssignedval = item.WorkAssigned_PersonnelId;
                            var workAssignedtext = "";
                            var Shiftval = item.Shift;
                            var Shifttext = "";
                            var scheduleddate = FormatJavascriptDate(item.ScheduledStartDate);
                            var duration = item.ScheduledDuration;
                            var thisworkorder = new funcworkorder(workOrderid, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
                            dataToUpdate.push(thisworkorder);
                        }
                    } else {
                        var i = equiToupdate.indexOf(item.WorkOrderId);
                        equiToupdate.splice(i, 1);
                        dataToUpdate = $.grep(dataToUpdate, function (obj) {
                            return obj.woid !== item.WorkOrderId;
                        });
                    }
                });
            }
        },
        complete: function () {
            $('.itemcount').text(equiToupdate.length);
            dtTable.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearch').prop('checked', 'checked');
                    $('#tblworkbenchapproval').find('select').removeAttr("disabled");
                    $('#tblworkbenchapproval').find('input[type=text]').removeAttr("disabled");
                } else {
                    $(this).find('.chksearch').prop('checked', false);
                    $('#tblworkbenchapproval').find('select').attr("disabled", "disabled").css("background-color", "#fff").css('color', '#333');
                    $('#tblworkbenchapproval').find('input[type=text]').attr("disabled", "disabled").css('color', '#333');;
                }
            });
            CloseLoader();
        }
    });
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
        var index = equiToupdate.indexOf(data.WorkOrderId);
        equiToupdate.splice(index, 1);
        thisTr.removeClass("checked");
        dataToUpdate = $.grep(dataToUpdate, function (obj) {
            return obj.woid !== data.WorkOrderId;
        });
    }
    else {
        equiToupdate.push(data.WorkOrderId);
        selectedcount = selectedcount + equiToupdate.length;
        thisTr.addClass("checked");

         data = dtTable.row($(this).parents('tr')).data();
        var workOrderid = data.WorkOrderId;

        var exist = $.grep(dataToUpdate, function (obj) {
            return obj.woid === workOrderid;
        });
        if (dataToUpdate.length <= 0 && exist.length <= 0) {
            var thisworkorder = new funcworkorder(workOrderid, "", "", "", "", "", "");
            dataToUpdate.push(thisworkorder);
        }
    }
});
var approvalCreatedby;
function generateWorkordersDataTable() {
    if (IsTrigger == true) {
        var status = tempAWBStatus;
        var Createdates = tempAWBcreatedates;
    }
    else {
        status = LRTrim($('#AWBStatus').val());
        Createdates = $('#AWBcreatedates').val();
    }

    var workorder = LRTrim($('#adv-workworder').val());
    var description = LRTrim($('#adv-Description').val());
    var chargeto = LRTrim($('#adv-ChargeTo').val());
    var chargetoname = LRTrim($('#adv-ChargeToName').val());
    var workassigned = $('#adv-WorkAssigned').val();//
    var shift = $('#adv-shift').val();//
    var scheduledate = ValidateDate($('#adv-ScheduledDate').val());//
    var scheduleduration = LRTrim($('#adv-ScheduledDuration').val());
    var createdate = ValidateDate($('#adv-CreateDate').val());//
    var createdby = $('#adv-CreatedBy').val();//
    if (typeof dtTable !== "undefined") {
        dtTable.destroy();
    }
    dtTable = $("#tblworkbenchapproval").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/ApprovalWorkbench/GetWorkOrderMaintGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.status = status;
                d.Createdates = Createdates;
                d.workorder = workorder;
                d.description = description;
                d.chargeto = chargeto;
                d.Chargetoname = chargetoname;
                d.workassigned = workassigned;
                d.shift = shift;
                d.scheduledate = scheduledate;
                d.scheduleduration = scheduleduration;
                d.createdate = createdate;
                d.createdby = createdby;
            },
            "dataSrc": function (result) {
                $("#adv-CreatedBy").empty();
                $("#adv-CreatedBy").append("<option value=''>" + "--Select--" + "</option>");
                for (var i = 0; i < result.CreatedList.length; i++) {
                    var id = result.CreatedList[i];
                    var name = result.CreatedList[i];
                    $("#adv-CreatedBy").append("<option value='" + id + "'>" + name + "</option>");

                }
                if (approvalCreatedby) {
                    $("#adv-CreatedBy").val(approvalCreatedby);
                }
                searchcount = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.WorkOrderId);
                });
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                var newResult = result;
                HidebtnLoaderclass("LoaderDrop");
                return newResult.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "WorkOrderId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#eqsearch-select-all').is(':checked') && totalcount == selectedcount) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-eqid="' + data + '" class="chksearch ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            if (equiToupdate.indexOf(data) != -1) {
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
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_woinfo href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "ChargeToClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true, "width": "10px" },
                { "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "WorkAssigned_PersonnelId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    'render': function (data, type, full, meta) {
                        var result = "";
                        var exist = $.grep(dataToUpdate, function (obj) {
                            return obj.woid === full.WorkOrderId;
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
                        var exist = $.grep(dataToUpdate, function (obj) {
                            return obj.woid === full.WorkOrderId;
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
                    "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    'render': function (data, type, full, meta) {
                        var thisdate = data;
                        var exist = $.grep(dataToUpdate, function (obj) {
                            return obj.woid === full.WorkOrderId;
                        });
                        if (!data) {
                            data = "";
                        }
                        if (exist.length > 0) {
                            data = exist[0].scheduledate;
                        }
                        return "<input type='text' class='dtpicker wodtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'>";
                    }
                },
                {
                    "data": "ScheduledDuration", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                    'render': function (data, type, full, meta) {
                        var thisdate = data;
                        var exist = $.grep(dataToUpdate, function (obj) {
                            return obj.woid === full.WorkOrderId;
                        });
                        if (exist.length > 0) {
                            data = exist[0].duration;
                        }
                        return "<input type='text' class='duration  dt-inline-text decimalinputupto2places' autocomplete='off' value='" + data + "' maskedFormat='20,2'>";
                    }
                },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Createby", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            $(document).find('.select-woassigned,.select-shift').select2({
                minimumResultsForSearch: -1
            });
            if (totalcount != 0 && (totalcount == equiToupdate.length || (searchcount != totalcount && arrayContainsArray(equiToupdate, searchresult) == true))) {
                $('#eqsearch-select-all').prop('checked', true);
            } else {
                $('#eqsearch-select-all').prop('checked', false);
            }
            SetPageLengthMenu();
            IsTrigger = false;
        },
        'rowCallback': function (row, data, index) {
            if (equiToupdate.indexOf(data.WorkOrderId) != -1) {
                $(row).addClass("checked");
            }
            var exist = $.grep(dataToUpdate, function (obj) {
                return obj.woid === data.WorkOrderId;
            });
            if (exist.length > 0 && data.WorkAssigned_PersonnelId != exist[0].workassignedval) {
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
$(document).on('change', '.select-woassigned', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var workOrderid = data.WorkOrderId;
    var workAssignedval = $(this).val();
    var workAssignedtext = $(this).find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(dataToUpdate, function (obj) {
        return obj.woid === workOrderid;
    });
    if (dataToUpdate.length > 0 && exist.length > 0) {
        var objIndex = GetIndex(dataToUpdate, workOrderid);
        dataToUpdate[objIndex].workassignedval = workAssignedval;
        dataToUpdate[objIndex].workassignedtext = workAssignedtext;
    }
    else {
        var thisworkorder = new funcworkorder(workOrderid, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        dataToUpdate.push(thisworkorder);
    }
});
$(document).on('change', '.select-shift', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var workOrderid = data.WorkOrderId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).val();
    var Shifttext = $(this).find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(dataToUpdate, function (obj) {
        return obj.woid === workOrderid;
    });
    if (dataToUpdate.length > 0 && exist.length > 0) {
        var objIndex = GetIndex(dataToUpdate, workOrderid);
        dataToUpdate[objIndex].shiftval = Shiftval;
        dataToUpdate[objIndex].shifttext = Shifttext;
    }
    else {
        var thisworkorder = new funcworkorder(workOrderid, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        dataToUpdate.push(thisworkorder);
    }
});
$(document).on('change', '.wodtpicker', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var workOrderid = data.WorkOrderId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).val();
    var duration = $(this).parents('tr').find('.duration').val();
    var exist = $.grep(dataToUpdate, function (obj) {
        return obj.woid === workOrderid;
    });
    if (dataToUpdate.length > 0 && exist.length > 0) {
        var objIndex = GetIndex(dataToUpdate, workOrderid);
        dataToUpdate[objIndex].scheduledate = scheduleddate;
    }
    else {
        var thisworkorder = new funcworkorder(workOrderid, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        dataToUpdate.push(thisworkorder);
    }
});
$(document).on('change', '.duration', function () {
    $(this).parent('td').css('background-color', '#d7f9c7');
    $(this).css('background-color', '#d7f9c7');
    var data = dtTable.row($(this).parents('tr')).data();
    var workOrderid = data.WorkOrderId;
    var workAssignedval = $(this).parents('tr').find('.select-woassigned').val();
    var workAssignedtext = $(this).parents('tr').find('.select-woassigned').find('option:selected').text();
    var Shiftval = $(this).parents('tr').find('.select-shift').val();
    var Shifttext = $(this).parents('tr').find('.select-shift').find('option:selected').text();
    var scheduleddate = $(this).parents('tr').find('.wodtpicker').val();
    var duration = $(this).val();
    if (duration == "") {
        $(this).val(0);
    }
    var exist = $.grep(dataToUpdate, function (obj) {
        return obj.woid === workOrderid;
    });
    if (dataToUpdate.length > 0 && exist.length > 0) {
        var objIndex = GetIndex(dataToUpdate, workOrderid);
        dataToUpdate[objIndex].duration = duration;
    }
    else {
        var thisworkorder = new funcworkorder(workOrderid, workAssignedval, workAssignedtext, Shiftval, Shifttext, scheduleddate, duration);
        dataToUpdate.push(thisworkorder);
    }
});
$(document).on('click', '.lnk_woinfo', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var WorkOrderId = data.WorkOrderId;
    $.ajax({
        url: "/ApprovalWorkbench/WorkOrderInformation",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { workOrderId: WorkOrderId },
        success: function (data) {
            $('#RenderWOWorkBench').html(data);
        },
        complete: function () {
            ZoomImage($(document).find('#EquipZoom'));
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
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
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnappwobenchAdvSrch', function () {
    searchresult = [];
    dtTable.state.clear();
    AWBAdvSearch();
    $('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    approvalCreatedby = $("#adv-CreatedBy").val();
});
function AWBAdvSearch() {
    var InactiveFlag = false;
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
    generateWorkordersDataTable();
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $(".spnControlCounter").text(selectCount);
}
$(document).on('click', '#liclearadvsearch', function () {
    dataToUpdate = [];
    equiToupdate = [];
    $("#AWBStatus").val(1).trigger('change.select2');
    $("#AWBcreatedates").val("0").trigger('change.select2');
    dtTable.state.clear();
    clearAdvanceSearch();
    generateWorkordersDataTable();
});
function clearAdvanceSearch() {
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find("select").val("").trigger('change');
    selectCount = 0;
    $(".spnControlCounter").text(selectCount);
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
    approvalCreatedby = $("#adv-CreatedBy").val();
}
$(document).on('click', '.btnCross', function () {
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "adv-CreatedBy") {
        approvalCreatedby = null;
    }
    AWBAdvSearch();
});

$(document).on('change', '#AWBStatus', function () {
    if (IsTrigger == false) {
        ShowbtnLoaderclass("LoaderDrop");
        localStorage.setItem("apwbstatus", $('#AWBStatus').val());
        localStorage.setItem("apwcreatedate", $('#AWBcreatedates').val());
        searchresult = [];
        dtTable.state.clear();
        generateWorkordersDataTable();
    }
});

$(document).on('change', '#AWBcreatedates', function () {
    if (IsTrigger == false) {
        ShowbtnLoaderclass("LoaderDrop");
        localStorage.setItem("apwbstatus", $('#AWBStatus').val());
        localStorage.setItem("apwcreatedate", $('#AWBcreatedates').val());
        searchresult = [];
        dtTable.state.clear();
        generateWorkordersDataTable();
    }
});
$(document).on('click', '#btndenypopupshow', function () {
    if (equiToupdate.length == 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    $('#denymodal').modal('show');
});
$(document).on('click', '#btndeny', function () {
    var DeniedReason = LRTrim($('#txtDeniedReason').val());
    var DeniedComments = LRTrim($('#txtDeniedcomments').val());
    var wOIds = equiToupdate;
    if (DeniedReason) {
        equiToupdate = [];
        $.ajax({
            url: '/ApprovalWorkbench/DenyWorkOrder',
            data: {
                wOIds: wOIds,
                Reason: DeniedReason,
                Comments: DeniedComments
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    $('#denymodal').modal('hide');
                    $('#eqsearch-select-all').prop('checked', false);
                    $('.itemcount').text(0);
                    SuccessAlertSetting.text = getResourceValue("AlertDeniedSuccess");
                    swal(SuccessAlertSetting, function () {
                        generateWorkordersDataTable();
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
$(document).on('click', '#btnapprove', function () {
    var WOData = dataToUpdate;
    WOData = dataToUpdate.filter(function (item) {
        return equiToupdate.indexOf(item.woid) > -1;
    });
    if (WOData.length === 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
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
    equiToupdate = [];
    dataToUpdate = [];
    $.ajax({
        url: '/ApprovalWorkbench/ApproveWorkOrder',
        data: WOData,
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "json",
        success: function (data) {
            if (data.Result == "success") {
                $('#denymodal').modal('hide');
                $('#eqsearch-select-all').prop('checked', false);
                $('.itemcount').text(0);
                SuccessAlertSetting.text = getResourceValue("WorkOrderApprovedSuccess");
                swal(SuccessAlertSetting, function () {
                    generateWorkordersDataTable();
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Work Order Information
$(document).on('click', '#btncancel', function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "/Approvalworkbench/Index?page=Maintenance_Work_Order_Approval";
    });
});
//$(document).on('change', '#ChargeType', function () {
//    var type = $(this).val();
//    var option = '';
//    $.ajax({
//        url: getLookUpListByTypeUrl,
//        type: "GET",
//        dataType: 'json',
//        data: { _type: type },
//        beforeSend: function () {
//            ShowLoader();
//        },
//        success: function (data) {
//            option += '<option value="">--Select--</option>';
//            for (var i = 0; i < data.length; i++) {
//                if (type == "Equipment") {
//                    option += '<option value="' + data[i].ChargeToClientLookupId + '">' + data[i].ChargeToClientLookupId + ' - ' + data[i].Name + '</option>';
//                }
//                else if (type == "Location") {
//                    option += '<option value="' + data[i].ChargeToClientLookupId + '">' + data[i].ChargeToClientLookupId + ' - ' + data[i].Name + '</option>';

//                }
//            }
//        },
//        complete: function () {
//            CloseLoader();
//            $(document).find('#ChargeToId').empty().html(option);
//            $(document).find('.select2picker').select2({});
//        },
//        error: function () {
//            CloseLoader();

//        }
//    });
//});
function WOInfoOnSuccess(data) {
    var WorkOrderId = data.WorkOrderId;
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("WorkOrderApprovedSuccess");
        swal(SuccessAlertSetting, function () {
            window.location.href = "/Approvalworkbench/Index?page=Maintenance_Work_Order_Approval";
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
//#endregion
//#region common
function ZoomImage(element) {
    element.elevateZoom(
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
}
function GetIndex(array, workorderid) {
    var index = -1;
    for (var i = 0; i < array.length; ++i) {
        if (array[i].woid == workorderid) {
            index = i;
            break;
        }
    }
    return index;
}
//#endregion
//#region Lookup tree

$(document).on('change', '#ChargeToClientLookupId', function () {
    var EquipmentClientLookupId = $(this).val();
    $(document).find('#hdnApprovalChargeToClientLookupId').val(EquipmentClientLookupId);
});
$(document).on('change', '#ChargeType', function () {
    var type = $(this).val();
    var assetval = $(document).find('#hdnassettreeval').val();
    var option = '';
    if (type == "") {
        $("#imgChargeToTreeLineItem").hide();
    }
    else {
        if (type == "Equipment" && assetval =='True') {
            $("#imgChargeToTreeLineItem").show();
        }
        else {
            $("#imgChargeToTreeLineItem").hide();
        }
    }
    $.ajax({
        url: getLookUpListByTypeUrl,
        type: "GET",
        dataType: 'json',
        data: { _type: type },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            option += '<option value="">--Select--</option>';
            for (var i = 0; i < data.length; i++) {
                if (type === "Equipment") {
                    option += '<option value="' + data[i].ChargeToClientLookupId + '">' + data[i].ChargeToClientLookupId + ' - ' + data[i].Name + '</option>';
                }
                else if (type === "Location") {
                    option += '<option value="' + data[i].ChargeToClientLookupId + '">' + data[i].ChargeToClientLookupId + ' - ' + data[i].Name + '</option>';
                }
            }
            $(document).find('#ChargeToClientLookupId').empty().html(option);
        },
        complete: function () {
            CloseLoader();
            $(document).find('.select2picker').select2({});
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#imgChargeToTreeLineItem', function (e) {
  
    $(this).blur();
    var treeHeaderVal = $(document).find("#treeHeaderVal").val();
    if (treeHeaderVal == "EquipmentTree") {
        generateWoEquipmentTree(-1);
    }
    else {
        generateWoSystemProcessTree(-1);
    }
})

$(document).on('change', '.radSelectWo', function () {
    $(document).find('#hdnApprovalChargeToClientLookupId').val('0');   
    var equipmentClientLookupIdid = $(this).data('clientlookupid');
    $('#woEquipTreeModal').modal('hide');
    $(document).find('#ChargeToClientLookupId').val(equipmentClientLookupIdid).trigger('change');
});
function generateWoEquipmentTree(paramVal) {
   
    $.ajax({
        url: '/PlantLocationTree/WorkOrderEquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {          
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            CloseLoader();
            $('#woEquipTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWo').each(function () {             
                if ($(this).data('clientlookupid') == $(document).find('#hdnApprovalChargeToClientLookupId').val()) {
                    $(this).attr('checked', true);
                }
            });
            
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
function generateWoSystemProcessTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/ProcessSystemTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        cache: false,
        beforeSend: function () {
            ShowLoader();
            $(document).find(".cntTree").html("<b>Processing...</b>");
        },
        success: function (data) {
            $(document).find(".cntTree").html(data);
        },
        complete: function () {
            //CloseLoader();
            //treeTable($(document).find('#tblTree'));
            //$(document).find('.radSelect').each(function () {

            //});
            //$('#woEquipTreeModal').modal('show');

            CloseLoader();
            $('#woEquipTreeModal').modal('show');
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWo').each(function () {
                if ($(this).data('equipmentid') == $(document).find('#hdnApprovalChargeToClientLookupId').val()) {
                    $(this).attr('checked', true);
                }
            });
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
$(document).on('change', '.radSelect', function () {
    var equipmentid = $(this).data('equipmentid');
    $('#woEquipTreeModal').modal('hide');
    $(document).find('#ChargeToId').val(equipmentid).trigger('change');
});
$(document).on('change', '.radEQSelect', function () {
    var equipmentid = $(this).data('equipmentid');
    var procsystemid = $(this).data('procsystemid');
    var name = $(this).data('name');
    if (isNaN(equipmentid)) {
        var initId = equipmentid.substring(0, 8);
    }
    $('#woEquipTreeModal').modal('hide');
    if (initId == "systemid" || initId == "processi") {
        var selectItem = $(document).find('#state').parent('td').text().trim();
        var defaultSelected = false;
        var nowSelected = true;
        var text = name;
        val = procsystemid;
        $('#ChargeToId').append(new Option(text, procsystemid, defaultSelected, nowSelected));
    }
    else {
        $(document).find('#ChargeToId').val(equipmentid).trigger('change');
    }
});
//#endregion