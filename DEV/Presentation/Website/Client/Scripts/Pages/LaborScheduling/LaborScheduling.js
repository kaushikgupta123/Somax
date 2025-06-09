var LaborSchedulingSelectedItemArray = [];
var LaborAvailableSelectedItemArray = [];
var LaborScheduledt;
var LaborAvailabledt;
var WorkSchedlIds = [];
var WorkOrderIds = [];
var AWOGridTotalGridItem = 0;
var flag = 0;
function LaborSchedulingSelectedItem(WorkOrderSchedId, ClientLookupId, Description, Type, Hours, Status, Date) {
    this.WorkOrderSchedId = WorkOrderSchedId;
    this.ClientLookupId = ClientLookupId;
    this.Description = Description;
    this.Type = Type;
    this.Hours = Hours;
    this.Status = Status;
    this.Date = Date;
}
function LaborAvailableSelectedItem(WorkOrderId, ClientLookupId, ChargeTo, ChargeToName, Description, Status, Priority,
    DownRequired, Assigned, Type, StartDate, Duration, RequiredDate) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.ChargeTo = ChargeTo;
    this.ChargeToName = ChargeToName;
    this.Description = Description;
    this.Status = Status;
    this.Priority = Priority;
    this.DownRequired = DownRequired;
    this.Assigned = Assigned;
    this.Type = Type;
    this.StartDate = StartDate;
    this.Duration = Duration;
    this.Status = Status;
    this.RequiredDate = RequiredDate;
}
$(document).ready(function () {
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});


    var ScheduledStartDate = localStorage.getItem("ScheduledStartDate");
    var PersonnelId = localStorage.getItem("LaborPersonalId");

    if (!ScheduledStartDate || !PersonnelId) {
        LaborScheduledt = $("#tblLabourGrid").DataTable({
            searching: false,
            paging: false,
            "ordering": false,
            language: {
                url: "/base/GetDataTableLanguageJson?nGrid=" + true,
            },
            sDom: 'tlipr'
        });
    }
    else {
        if (ScheduledStartDate) {
            $(document).find('#LaborSchedulingModel_Date').val(ScheduledStartDate);
        }
        if (PersonnelId) {
            $('#LaborSchedulingModel_PersonnelId').val(PersonnelId).trigger('change.select2');
            PopulateScheduledLabor();
        }
        localStorage.removeItem("LaborPersonalId");
        localStorage.removeItem("ScheduledStartDate");
    }
});
$(document).on('change', "#LaborSchedulingModel_PersonnelId", function () {
    LaborSchedulingSelectedItemArray = [];
    WorkSchedlIds = [];
    PopulateScheduledLabor();
});
$(document).on('change', "#LaborSchedulingModel_Date", function () {
    LaborSchedulingSelectedItemArray = [];
    WorkSchedlIds = [];
    PopulateScheduledLabor();
});
$(document).on('click', '#labschidselectall', function (e) {
    var checked = this.checked;
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = $(document).find("#LaborSchedulingModel_Date").val();
    if (!PersonnelId || LaborScheduledt.data().count() == 0) {
        return false;
    }
    $.ajax({
        url: "/LaborSchedulingDaily/GetLaborScheduling",
        data: {
            PersonnelId: PersonnelId,
            Date: Date
        },
        async: true,
        type: "GET",
        datatype: "json",
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    var found = LaborSchedulingSelectedItemArray.some(function (el) {
                        return el.WorkOrderSchedId === item.WorkOrderSchedId;
                    });
                    if (checked) {
                        if (WorkSchedlIds.indexOf(item.WorkOrderSchedId) == -1)
                            WorkSchedlIds.push(item.WorkOrderSchedId);
                        var itemLS = new LaborSchedulingSelectedItem(item.WorkOrderSchedId, item.ClientLookupId, item.Description, item.Type, item.Hours, item.Status, item.Date);
                        if (!found) { LaborSchedulingSelectedItemArray.push(itemLS); }
                    } else {
                        var i = WorkSchedlIds.indexOf(item.WorkOrderSchedId);
                        WorkSchedlIds.splice(i, 1);
                        if (found) {
                            LaborSchedulingSelectedItemArray = LaborSchedulingSelectedItemArray.filter(function (el) {
                                return el.WorkOrderSchedId !== item.WorkOrderSchedId;
                            });
                        }
                    }
                });
            }
        },
        complete: function () {
            LaborScheduledt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearch').prop('checked', 'checked');
                }
                else {
                    $(this).find('.chksearch').prop('checked', false);
                }
            });
        }
    });
});

function PopulateScheduledLabor() {
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = ValidateDate($(document).find("#LaborSchedulingModel_Date").val());
    if (PersonnelId == "") {
        PersonnelId = "0";
    }
    if (!Date) {
        return false;
    }
    if (PersonnelId != null && PersonnelId != '' && Date != null && Date != '') {
        if ($(document).find('#tblLabourGrid').hasClass('dataTable')) {
            LaborScheduledt.destroy();
        }
        LaborScheduledt = $("#tblLabourGrid").DataTable({
            colReorder: true,
            rowGrouping: true,
            searching: true,
            "pagingType": "full_numbers",
            "bProcessing": true,
            "bDeferRender": true,
            serverSide: true,
            "order": [[1, "asc"]],
            stateSave: true,
            language: {
                url: "/base/GetDataTableLanguageJson?nGrid=" + true,
            },
            sDom: 'Btlipr',
            buttons: [],
            "orderMulti": true,
            "ajax": {
                "url": "/LaborSchedulingDaily/GetLaborSchedulingMainGrid",
                "type": "post",
                "datatype": "json",
                data: function (d) {
                    d.PersonnelId = PersonnelId;
                    d.Date = Date;
                },
                "dataSrc": function (result) {
                    totalhours = result.totalHours;
                    if (totalhours != 0) {
                        $(document).find("#th-total").text(result.totalHours);
                    }
                    else {
                        $(document).find("#th-total").text('');
                    }
                    if (result.data.length == 0) {
                        $(document).find('#labschidselectall').prop('checked', false);
                    }
                    return result.data;
                },
                global: true
            },
            "columns":
                [
                    {
                        "data": "WorkOrderSchedId",
                        orderable: false,
                        "bSortable": false,
                        className: 'select-checkbox dt-body-center',
                        targets: 0,
                        'render': function (data, type, full, meta) {
                            if ($('#labschidselectall').is(':checked')) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearch ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            } else {
                                var found = LaborSchedulingSelectedItemArray.some(function (el) {
                                    return el.WorkOrderSchedId === data;
                                });
                                if (found) {
                                    return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearch ' + data + '"  value="'
                                        + $('<div/>').text(data).html() + '">';
                                }
                                else {
                                    return '<input type="checkbox" name="id[]" data-LSid="' + data + '" class="chksearch ' + data + '"  value="'
                                        + $('<div/>').text(data).html() + '">';
                                }
                            }
                        },
                    },
                    {
                        "data": "ClientLookupId",
                        "autoWidth": true,
                        "bSearchable": true,
                        "bSortable": true,
                        //V2-838
                        "className": "text-left",
                        "mRender": function (data, type, full, row) {
                            if (full['PartsOnOrder'] > 0) {
                                return '<div  class="width-100"><a class="lnk_workorder">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                            } else {
                                return '<div  class="width-100"><a class="lnk_workorder">' + data + '</a></div>'
                            }
                        }
                    },
                    {
                        "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                        mRender: function (data, type, full, meta) {
                            return "<div class='text-wrap width-400'>" + data + "</div>";
                        }
                    },
                    { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                    {
                        "data": "Hours", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'test',
                        'render': function (data, type, full, meta) {
                            return "<div style='width:110px !important;'><input type='text' style='width:90px !important; float:left;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: relative; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: relative; top: 8px; right:-3px; color:red;display:none;' title='test djhgjdfd kjhfhdksh'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:relative;top: 8px; right:-3px;display:none;' /></div>";
                        }
                    },
                    { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                    { "data": "Date", "autoWidth": true, "bSearchable": true, "bSortable": true }
                ],
            "columnDefs": [
                { width: "3%", targets: 0 }
            ],
            initComplete: function () {
                SetPageLengthMenu();
            }
        });
    }
}
var enterhit = 0;
$(document).on('blur', '.grd-hours', function (event) {
    if (enterhit == 1)
        return;
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = ValidateDate($(document).find("#LaborSchedulingModel_Date").val());
    var row = $(this).parents('tr');
    var data = LaborScheduledt.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    thstextbox.siblings('.is-saved-check').hide();

    if (thstextbox.val() == "") {
        thstextbox.val('0');
        thstextbox.siblings('.is-saved-times').show().attr('title', 'Please enter a value for Hours.');
        return;
    }
    else if ($.isNumeric(thstextbox.val()) === false) {
        thstextbox.siblings('.is-saved-times').show().attr('title', 'Please enter a valid value.');
        return;
    }
    else if (thstextbox.val() > 999999.99) {
        thstextbox.siblings('.is-saved-times').show().attr('title', 'Maximum Value is 999999.99');
        return;
    }

    $.ajax({
        url: '/LaborSchedulingDaily/UpdateWoHours',
        data: {
            WorkOrderSchedId: data.WorkOrderSchedId,
            hours: $(this).val(),
            PersonnelId: PersonnelId,
            Date: Date
        },
        type: "POST",
        "datatype": "json",
        beforeSend: function () {
            thstextbox.siblings('.is-saved-loader').show();
        },
        success: function (data) {
            thstextbox.siblings('.is-saved-loader').hide();
            if (data.Result == 'success') {
                thstextbox.siblings('.is-saved-check').show();
                $(document).find("#th-total").text(data.totalHours);
            }
            else {
                thstextbox.siblings('.is-saved-times').show();
            }
        }
    });
});
$(document).on('keypress', '.grd-hours', function (event) {
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = ValidateDate($(document).find("#LaborSchedulingModel_Date").val());
    var row = $(this).parents('tr');
    var data = LaborScheduledt.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    thstextbox.siblings('.is-saved-check').hide();
    if (keycode == '13') {
        enterhit = 1;
        if (thstextbox.val() == "") {
            thstextbox.val('0');
            thstextbox.siblings('.is-saved-times').show().attr('title', 'Please enter a value for Hours.');
            return;
        }
        else if ($.isNumeric(thstextbox.val()) === false) {
            thstextbox.siblings('.is-saved-times').show().attr('title', 'Please enter a valid value.');
            return;
        }
        else if (thstextbox.val() > 999999.99) {
            thstextbox.siblings('.is-saved-times').show().attr('title', 'Maximum Value is 999999.99');
            return;
        }
        thstextbox.blur();
        $.ajax({
            url: '/LaborSchedulingDaily/UpdateWoHours',
            data: {
                WorkOrderSchedId: data.WorkOrderSchedId,
                hours: $(this).val(),
                PersonnelId: PersonnelId,
                Date: Date
            },
            type: "POST",
            "datatype": "json",
            beforeSend: function () {
                thstextbox.siblings('.is-saved-loader').show();
            },
            success: function (data) {
                thstextbox.siblings('.is-saved-loader').hide();
                if (data.Result == 'success') {
                    thstextbox.siblings('.is-saved-check').show();
                    $(document).find("#th-total").text(data.totalHours);
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                enterhit = 0;
            }
        });
    }
    event.stopPropagation();
});
$(document).on('click', "#btnRemove", function () {
    if (WorkSchedlIds.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var WorkSchedlVals = WorkSchedlIds;
        var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
        var Date = $(document).find("#LaborSchedulingModel_Date").val();
        swal(CancelAlertSetting, function () {
            $.ajax({
                url: '/LaborSchedulingDaily/DeleteLabor',
                data: {
                    WorkSchedlIds: WorkSchedlVals,
                    PersonnelId: PersonnelId,
                    Date: Date
                },
                type: "POST",
                "datatype": "json",
                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                    LaborScheduledt.state.clear();
                    LaborSchedulingSelectedItemArray = [];
                    WorkSchedlIds = [];
                    PopulateScheduledLabor();
                    SuccessAlertSetting.text = getResourceValue("RemovedSuccessfullyAlert");
                    swal(SuccessAlertSetting, function () {
                    });
                },
                complete: function () {
                    CloseLoader();
                }
            });
        });
    }
});
$(document).on('change', '.chksearch', function () {
    var data = LaborScheduledt.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = WorkSchedlIds.indexOf(data.WorkOrderSchedId);
        WorkSchedlIds.splice(index, 1);
        var el = $('#labschidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        LaborSchedulingSelectedItemArray = LaborSchedulingSelectedItemArray.filter(function (el) {
            return el.WorkOrderSchedId !== data.WorkOrderSchedId;
        });
    }
    else {
        WorkSchedlIds.push(data.WorkOrderSchedId);
        var item = new LaborSchedulingSelectedItem(data.WorkOrderSchedId, data.ClientLookupId, data.Description, data.Type, data.Hours, data.Status, data.Date);
        var found = LaborSchedulingSelectedItemArray.some(function (el) {
            return el.WorkOrderSchedId === data.WorkOrderSchedId;
        });
        if (!found) { LaborSchedulingSelectedItemArray.push(item); }
    }
});
//#region Available work order
$(document).on('click', "#btnAvailableWork", function () {
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = ValidateDate($(document).find("#LaborSchedulingModel_Date").val());
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    if (!Date) {
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: getResourceValue("alertValiddate"),
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
        });
        return false;
    }
    if (!PersonnelId) {
        swal({
            title: getResourceValue("CommonErrorAlert"),
            text: getResourceValue("alertPleaseSelectPersonnel"),
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-danger",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
        });
        return false;
    }
    localStorage.setItem("ScheduledStartDate", Date);
    localStorage.setItem("LaborPersonalId", PersonnelId);
    $.ajax({
        url: "/LaborSchedulingDaily/AvailableWorkOrders",
        type: "POST",
        datatype: "json",
        data: {
            PersonnelId: PersonnelId,
            Date: Date
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderlabourschedule').html(data);
        },
        complete: function () {
            SetAdvSearchControl();            
            GenerateAvailableLabor();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function SetAdvSearchControl() {
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).find('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
}

function GenerateAvailableLabor(flag) {
    if ($(document).find('#tblAvailGrid').hasClass('dataTable')) {
        LaborAvailabledt.destroy();
    }
    LaborAvailabledt = $(document).find("#tblAvailGrid").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/LaborSchedulingDaily/GetAvailableWorkOrderMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($("#AWOWorkOrder").val());
                d.ChargeTo = LRTrim($("#AWOChargeTo").val());
                d.ChargeToName = LRTrim($("#AWOChargeToName").val());
                d.Description = LRTrim($("#AWODescription").val());
                d.Status = $("#AWOStatus").val();
                d.Priority = $('#AWOPriority').val();
                d.DownRequired = $('#AWODownRequired').val();
                d.Assigned = $('#AWOAssigned').val();
                d.Type = $("#AWOWOType").val();
                d.StartDate = LRTrim($("#AWOStartDate").val());
                d.Duration = LRTrim($("#AWODuration").val());
                d.RequiredDate = LRTrim($('#AWORequiredDate').val());
                d.flag = flag;
            },
            "dataSrc": function (result) {
                AWOGridTotalGridItem = result.data.length;
                return result.data;
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
                        if ($('#labavlidselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = LaborAvailableSelectedItemArray.some(function (el) {
                                return el.WorkOrderId === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-LSid="' + data + '" class="chksearchAvl ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    },
                },
                //V2-838
                //{ "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "className": "text-left",
                    "mRender": function (data, type, full, row) {
                        if (full['PartsOnOrder'] > 0) {
                            return '<div  class="width-100"><a class="lnk_workorder" href="javascript:void(0)">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                        } else {
                            return '<div  class="width-100"><a class="lnk_workorder" href="javascript:void(0)">' + data + '</a></div>'
                        }
                    }
                },
                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    },
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //V2-984
                /*{ "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true },*/
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "sClass": "ghover",
                    mRender: function (data, type, full, meta) {
                        if (full.WorkAssigned_PersonnelId == -1) {
                            return "<span>" + data + "</span><span class='tooltipgrid' id=" + full.WorkOrderId + "></span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                        }
                        else {
                            return "<span>" + data + "</span>";
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StartDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Duration", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-right' },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        },
        'rowCallback': function (row, data, index) {
            if (data.Status == "Scheduled") {
                $(row).find('td').css('background-color', '#36a3f7b3');
                $(row).find('td').css('color', '#fff');
            }
        }
    });
}
$(document).on('click', '#labavlidselectall', function (e) {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    WorkOrderIds = [];
    var checked = this.checked;
    $.ajax({
        url: "/LaborSchedulingDaily/GetLaborAvailable",
        async: true,
        type: "GET",
        datatype: "json",
        data: {
            ClientLookupId : LRTrim($("#AWOWorkOrder").val()),
            ChargeTo : LRTrim($("#AWOChargeTo").val()),
            ChargeToName: LRTrim($("#AWOChargeToName").val()),
            Description : LRTrim($("#AWODescription").val()),
            Status : $("#AWOStatus").val(),
            Priority : $('#AWOPriority').val(),
            DownRequired : $('#AWODownRequired').val(),
            Assigned : $('#AWOAssigned').val(),
            Type : $("#AWOWOType").val(),
            StartDate : LRTrim($("#AWOStartDate").val()),
            Duration : LRTrim($("#AWODuration").val()),
            RequiredDate : LRTrim($('#AWORequiredDate').val()),
            flag : flag
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    var found = LaborAvailableSelectedItemArray.some(function (el) {
                        return el.WorkOrderId === item.WorkOrderId;
                    });
                    if (checked) {
                        if (WorkOrderIds.indexOf(item.WorkOrderId) == -1)
                            WorkOrderIds.push(item.WorkOrderId);

                        var itemLS = new LaborAvailableSelectedItem(item.WorkOrderId, item.ClientLookupId, item.ChargeTo, item.ChargeToName, item.Description, item.Status, item.Priority,
                            item.DownRequired, item.Assigned, item.Type, item.StartDate, item.Duration, item.RequiredDate);
                        if (!found) { LaborAvailableSelectedItemArray.push(itemLS); }
                    } else {
                        var i = WorkOrderIds.indexOf(item.WorkOrderId);
                        WorkOrderIds.splice(i, 1);
                        if (found) {
                            LaborAvailableSelectedItemArray = LaborAvailableSelectedItemArray.filter(function (el) {
                                return el.WorkOrderId !== item.WorkOrderId;
                            });
                        }
                    }
                });
            }
        },
        complete: function () {
            LaborAvailabledt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearchAvl').prop('checked', 'checked');
                } else {
                    $(this).find('.chksearchAvl').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearchAvl', function () {
    var data = LaborAvailabledt.row($(this).parents('tr')).data();
    if (!this.checked) {
        var index = WorkOrderIds.indexOf(data.WorkOrderId);
        WorkOrderIds.splice(index, 1);
        var el = $('#labavlidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        LaborAvailableSelectedItemArray = LaborAvailableSelectedItemArray.filter(function (el) {
            return el.WorkOrderId !== data.WorkOrderId;
        });
    }
    else {
        WorkOrderIds.push(data.WorkOrderId);
        var item = new LaborAvailableSelectedItem(data.WorkOrderId, data.ClientLookupId, data.ChargeTo, data.ChargeToName, data.Description, data.Status, data.Priority,
            data.DownRequired, data.Assigned, data.Type, data.StartDate, data.Duration, data.RequiredDate);
        var found = LaborAvailableSelectedItemArray.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        if (!found) { LaborAvailableSelectedItemArray.push(item); }
    }
    if (AWOGridTotalGridItem == LaborAvailableSelectedItemArray.length) {
        $('#labavlidselectall').prop('checked', 'checked');
    }
    else {
        $('#labavlidselectall').prop('checked', false);
    }
});
//#region Advance Search
$(document).on('click', "#btnAvailableLaborDataAdvSrch", function (e) {
    var searchitemhtml = "";
    hGridfilteritemcount = 0;
    $('#txtLAsearchbox').val('');
    $('#advsearchsidebarAvailableLabor').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        //if ($(this).val()) {
        //    hGridfilteritemcount++;
        //    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        //}
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#ApproveWOadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    LaborAvailabledt.page('first').draw('page');
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterAVlLab', function () {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    clearAdvanceSearch();
    LaborAvailabledt.page('first').draw('page');
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarAvailableLabor').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion
$(document).on('click', '#btnAWOCancel', function (e) {
    swal(CancelAlertSetting, function () {
        RedirectToLabourSchedulingDetail();
    });
});
$(document).on('click', '#btnAddAvailableWO', function (e) {
    var WorkOrderVals = WorkOrderIds;
    var PersonnelId = $(document).find("#LaborSchedulingModel_PersonnelId").val();
    var Date = $(document).find("#LaborSchedulingModel_Date").val();

    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    if (WorkOrderVals.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $.ajax({
            url: '/LaborSchedulingDaily/AddAvailableWorkOrder',
            data: {
                WorkOrderIds: WorkOrderVals,
                PersonnelId: PersonnelId,
                Date: Date,
                flag: flag
            },
            type: "POST",
            "datatype": "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.length == 0) {
                    SuccessAlertSetting.text = getResourceValue("ScheduledWOAddAlert");
                    swal(SuccessAlertSetting, function () {
                    });
                }
                else {
                    var errorMessage = [];
                    $.each(data, function (index, item) {
                        errorMessage.push(item.Message);
                        //errorHtml = errorHtml + "<li><i class='fa fa-circle bull'></i>" + item.Message + "</li>";
                    });
                    GenericSweetAlertMethod(errorMessage);
                }
                $(document).find('#labavlidselectall').prop('checked', false);
                $(document).find('.chksearchAvl').prop('checked', false);
                LaborAvailabledt.state.clear();
                LaborAvailableSelectedItemArray = [];
                WorkOrderIds = [];
                GenerateAvailableLabor(flag);
            },
            complete: function () {
                CloseLoader();
            }
        });
    }
});
function RedirectToLabourSchedulingDetail() {
    window.location.href = "/LaborSchedulingDaily/index?page=Maintenance_Work_Order_Labor_Scheduling";
}
$(document).on('click', '#AlreadyScheduledId', function (e) {
    LaborAvailabledt.state.clear();
    if (!this.checked) {       
        GenerateAvailableLabor(0);
    }
    else {       
        GenerateAvailableLabor(1);
    }
});
function GetFormattedToday() {
    var todayTime = new Date();
    var month = todayTime.getMonth() + 1;
    var day = todayTime.getDate();
    var year = todayTime.getFullYear();
    return month + "/" + day + "-" + year;
}
//#endregion

