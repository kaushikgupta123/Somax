var PrentiveMaintenanceForcastdt;
//#region Preventive Maintenance Forcast Search
$(function () {
    $(document).find('.select2picker').select2({});
    $(document).find('#gridadvsearchassign').select2({});
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay', function () {
        $('.sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $(document).on('click', '#PMFsidebarCollapse', function () {
        $('.sidebar').addClass('active');
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
    $(document).find('#btnProcess').trigger('click');
});
$.validator.setDefaults({ ignore: null });
$('input, form').blur(function () {
    if ($(this).closest('form').length > 0) {
        $(this).valid();
    }
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
$(document).on('change', "#ScheduleType", function (e) {
    var thisVal = $(this).val();
    var onDemandDropDown = $(document).find('#pMForcastModel_OnDemandGroup');
    if (thisVal == "OnDemand") {
        $.ajax({
            url: "/PMForcast/GetOnDemandGroupList",
            type: "GET",
            dataType: 'json',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                onDemandDropDown.empty();
                onDemandDropDown.append("<option value=''>" + "--Select--" + "</option>");
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        onDemandDropDown.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                    }
                }
            },
            complete: function () {
                onDemandDropDown.removeAttr('disabled');
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
        $('#OnDemandId').attr('style', 'display:flex');
    }
    else {
        var onDemandDropDown = $(document).find('#pMForcastModel_OnDemandGroup');
        onDemandDropDown.empty();
        onDemandDropDown.append("<option value=''>" + "--Select--" + "</option>");
        onDemandDropDown.attr('disabled', 'disabled');
        $('#OnDemandId').attr('style', 'display:none');
    }
});
//#region V2-1075
function getSwalTimeOutPopAlert() {
   var timeoutwaitmsg = "It will take up to " + ReportTimeOut + " minutes - please be patient";
    CloseLoader();
    swal({
        title: "",
        text: timeoutwaitmsg,
        imageUrl: "../content/Images/image_1197421.gif",
        showConfirmButton: false
    });
}
function getSwalTimeoutErrorPopAlert() {
        ErrorAlertSetting.text = "The time span is too long to run - please modify your selection criteria";;
    swal(ErrorAlertSetting, function () {
    });
}
//#endregion V2-1075
function ExecuteProcess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var ScheduleType = $(document).find("#ScheduleType").val();
        var ForcastDate = ValidateDate($(document).find("#pMForcastModel_ForecastDate").val());
        var OnDemandGroup = $(document).find("#pMForcastModel_OnDemandGroup").val();
        var searchText = LRTrim($('#txtPMFsearchbox').val());
        var ClientLookupId = $(document).find("#PMFPmId").val();
        var Description = $(document).find("#PMFDescription").val();
        var SchedueledDate = ValidateDate($(document).find("#PMFScheduled").val());
        var AssignedTo_PersonnelClientLookupId = $(document).find("#PMFAssignedTo").val();
        var ChargeToClientLookupId = $(document).find("#PMFChargeTo").val();
        var ChargeToName = $(document).find("#PMFChargeToName").val();
        var Duration = $(document).find("#PMFJobDuration").val();
        var EstLaborHours = $(document).find("#PMFLaborHours").val();
        var EstLaborCost = $(document).find("#PMFLaborCost").val();
        var EstOtherCost = $(document).find("#PMFOtherCost").val();
        var EstMaterialCost = $(document).find("#PMFMaterialCost").val();
        var TypeList = $(document).find("#PMFType").val();
        $('#finselectcontainer').show();
        if ($(document).find('#tblPMForcastGrid').hasClass('dataTable')) {
            PrentiveMaintenanceForcastdt.destroy();
        }
        PrentiveMaintenanceForcastdt = $("#tblPMForcastGrid").DataTable({
            colReorder: true,
            rowGrouping: true,
            searching: true,
            "pagingType": "full_numbers",
            "bProcessing": true,
            "bDeferRender": true,
            serverSide: true,
            "order": [[1, "asc"]],
            stateSave: false,
            language: {
                url: "/base/GetDataTableLanguageJson?nGrid=" + true,
            },
            sDom: 'Btlipr',
            buttons:
                [
                    {
                        extend: 'excel',
                        filename: ClientLookupId + '_PreventiveMaintenanceForcast',
                        extension: '.xlsx',
                        className: "datatable-btn-export PreventiveMaintenanceForcastgridexport"
                    }
                ],
            "orderMulti": true,
            "ajax": {
                url: "/PMForcast/GetPreventiveMainForcastGrid",
                "type": "post",
                "datatype": "json",
                beforeSend: function () {
                    getSwalTimeOutPopAlert();
                    }, 
                data: function (d) {
                    d.SearchText = searchText;
                    d.ScheduleType = ScheduleType;
                    d.ForcastDate = ForcastDate;
                    d.OnDemandGroup = OnDemandGroup;
                    d.ClientLookupId = ClientLookupId;
                    d.Description = Description;
                    d.SchedueledDate = SchedueledDate;
                    d.AssignedTo_PersonnelClientLookupId = AssignedTo_PersonnelClientLookupId;
                    d.ChargeToClientLookupId = ChargeToClientLookupId;
                    d.ChargeToName = ChargeToName;
                    d.Duration = Duration;
                    d.EstLaborHours = EstLaborHours;
                    d.EstLaborCost = EstLaborCost;
                    d.EstOtherCost = EstOtherCost;
                    d.EstMaterialCost = EstMaterialCost;
                    d.assignedPMS = $("#gridadvsearchassign").val();
                    d.requiredDate = LRTrim($("#gridadvsearchrequiredate").val());
                    //V2-1082
                    d.downRequired = $("#txtDownRequired").val();
                    d.shifts = $("#gridadvsearchshift").val();
                    d.TypeList = TypeList;
                },
                "dataSrc": function (result) {
                    if (result.timeoutError == "Timeout") {
                        getSwalTimeoutErrorPopAlert();
                        return result.data;
                    }
                    else {
                        swal.close();
                        if (result.data.length < 1) {
                            $(document).find('#downloadPMForcastdata').prop('disabled', true);
                        }
                        else {
                            $(document).find('#downloadPMForcastdata').prop('disabled', false);
                        }
                        return result.data;
                    }
                }, error: function (xhr, status, error) {
                    if (xhr.responseText.indexOf("request timed out") != -1) {
                      PrentiveMaintenanceForcastdt.destroy();
                      getSwalTimeoutErrorPopAlert();
                    }
                    else { swal.close(); CloseLoader(); }

                },
                global: true
            },
            "columns":
                [
                    {
                        "data": "PrevMaintSchedId",
                        "bVisible": true,
                        "bSortable": false,
                        "autoWidth": false,
                        "bSearchable": false,
                        "mRender": function (data, type, row) {
                            if (row.ChildCount > 0) {
                                return '<img class="PrevMaintAssignGrid" id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                            }
                            else {
                                return '';
                            }
                        }
                    },
                    { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
                    { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                        "mRender": function (data, type, row) {
                            return "<div class='text-wrap width-400'>" + data + "</div>";
                        }
                    },
                    { "data": "SchedueledDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                    { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                    {
                        "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "sClass": "ghover",
                        "mRender": function (data, type, full, meta) {

                            return "<span>" + full.AssignedMultiple + "</span><span class='tooltipgrid' id=" + full.PrevMaintSchedId + ">" + data + "</span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";



                        }
                    },
                    { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                    { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                    { "data": "Duration", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                    { "data": "EstLaborHours", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
                    { "data": "EstLaborCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
                    { "data": "EstOtherCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" },
                    { "data": "EstMaterialCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11" },
                    {
                        "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "12", "className": "text-center",
                        "mRender": function (data, type, row) {
                            if (data == false) {
                                return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                    '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                            }
                            else {
                                return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                    '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                            }
                        }
                    },
                    {
                        "data": "Shift", "autoWidth": true, "bSearchable": true, "name": "13", "bSortable": true,
                        mRender: function (data, type, full, meta) {
                            if (data == null) {
                                data = "";
                            } else {
                                data = data;
                            }
                            return "<div class='text-wrap width-150'>" + data + "</div>";
                        }
                    },
                    {
                        "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "14",
                    }
                ],
            initComplete: function () {
                SetPageLengthMenu();            
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$('#btnmainPMFsearch').on('click', function () {
    clearAdvanceSearch();
    PrentiveMaintenanceForcastdt.state.clear();
    GetForcastList();
});
function GetForcastList() {
    var ScheduleType = $(document).find("#ScheduleType").val();
    var ForcastDate = ValidateDate($(document).find("#pMForcastModel_ForecastDate").val());
    var OnDemandGroup = $(document).find("#pMForcastModel_OnDemandGroup").val();
    var searchText = LRTrim($('#txtPMFsearchbox').val());
    var ClientLookupId = $(document).find("#PMFPmId").val();
    var Description = $(document).find("#PMFDescription").val();
    var SchedueledDate = ValidateDate($(document).find("#PMFScheduled").val());
    var AssignedTo_PersonnelClientLookupId = $(document).find("#PMFAssignedTo").val();
    var ChargeToClientLookupId = $(document).find("#PMFChargeTo").val();
    var ChargeToName = $(document).find("#PMFChargeToName").val();
    var Duration = $(document).find("#PMFJobDuration").val();
    var EstLaborHours = $(document).find("#PMFLaborHours").val();
    var EstLaborCost = $(document).find("#PMFLaborCost").val();
    var EstOtherCost = $(document).find("#PMFOtherCost").val();
    var EstMaterialCost = $(document).find("#PMFMaterialCost").val();
    var assignedPMS = $("#gridadvsearchassign").val();
    var requiredDate = LRTrim($("#gridadvsearchrequiredate").val());
    var TypeList = $(document).find("#PMFType").val();
    if (!ForcastDate) {
        return false;
    }
    if ($(document).find('#tblPMForcastGrid').hasClass('dataTable')) {
        PrentiveMaintenanceForcastdt.destroy();
    }
    PrentiveMaintenanceForcastdt = $("#tblPMForcastGrid").DataTable({
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
        buttons:
            [
                {
                    extend: 'excel',
                    filename: ClientLookupId + '_PreventiveMaintenanceForcast',
                    extension: '.xlsx',
                    className: "datatable-btn-export PreventiveMaintenanceForcastgridexport"
                }
            ],
        "orderMulti": true,
        "ajax": {
            url: "/PMForcast/GetPreventiveMainForcastGrid",
            "type": "post",
            "datatype": "json",
            beforeSend: function () {
                getSwalTimeOutPopAlert();
            }, 
            data: function (d) {
                d.SearchText = searchText;
                d.ScheduleType = ScheduleType;
                d.ForcastDate = ForcastDate;
                d.OnDemandGroup = OnDemandGroup;
                d.ClientLookupId = ClientLookupId;
                d.Description = Description;
                d.SchedueledDate = SchedueledDate;
                d.AssignedTo_PersonnelClientLookupId = AssignedTo_PersonnelClientLookupId;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
                d.ChargeToName = ChargeToName;
                d.Duration = Duration;
                d.EstLaborHours = EstLaborHours;
                d.EstLaborCost = EstLaborCost;
                d.EstOtherCost = EstOtherCost;
                d.EstMaterialCost = EstMaterialCost;
                d.assignedPMS = assignedPMS;
                d.requiredDate = requiredDate;
                //V2-1082
                d.downRequired = $("#txtDownRequired").val();
                d.shifts = $("#gridadvsearchshift").val();
                d.TypeList = TypeList;
            },
            "dataSrc": function (result) {
                if (result.timeoutError == "Timeout") {
                    getSwalTimeoutErrorPopAlert();
                    return result.data;
                }
                else {
                    swal.close();
                    if (result.data.length < 1) {
                        $(document).find('#downloadPMForcastdata').prop('disabled', true);
                    }
                    else {
                        $(document).find('#downloadPMForcastdata').prop('disabled', false);
                    }
                    return result.data;
                }
            }, error: function (xhr, status, error) {
                if (xhr.responseText.indexOf("request timed out") != -1) {
                    PrentiveMaintenanceForcastdt.destroy();
                    getSwalTimeoutErrorPopAlert();
                }
                else { swal.close(); CloseLoader(); }

            },
            global: true
        },
        "columns":
            [
                {
                    "data": "PrevMaintSchedId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img class="PrevMaintAssignGrid" id="' + data + '" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "SchedueledDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4", "sClass": "ghover",
                    "mRender": function (data, type, full, meta) {

                        return "<span>" + full.AssignedMultiple + "</span><span class='tooltipgrid' id=" + full.PrevMaintSchedId + ">" + data + "</span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                    }
                },

                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
                { "data": "Duration", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "7" },
                { "data": "EstLaborHours", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "8" },
                { "data": "EstLaborCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "9" },
                { "data": "EstOtherCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "10" },
                { "data": "EstMaterialCost", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "11" },
                {
                    "data": "DownRequired", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "12", "className": "text-center",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" class="status" onclick="return false"><span></span></label>';
                        }
                        else {
                            return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                                '<input type="checkbox" checked="checked" class="status" onclick="return false"><span></span></label>';
                        }
                    }
                },
                {
                    "data": "Shift", "autoWidth": true, "bSearchable": true, "name": "13", "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        if (data == null) {
                            data = "";
                        } else {
                            data = data;
                        }
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                {
                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "14",
                }
            ],
        initComplete: function () {
            SetPageLengthMenu(); 
        }
    });
}
//#endregion
//#region Advance Search
function clearAdvanceSearch() {
    $("#PMFPmId").val("");
    $("#PMFDescription").val("");
    $("#PMFScheduled").val("");
    $("#PMFAssignedTo").val("");
    $("#PMFChargeTo").val("");
    $("#PMFChargeToName").val("");
    $("#PMFJobDuration").val("");
    $("#PMFLaborHours").val("");
    $("#PMFLaborCost").val("");
    $("#PMFOtherCost").val("");
    $("#PMFMaterialCost").val("");
}
//#endregion
//#region Advance Search
$(document).on('click', "#btnPMForcastDataAdvSrch", function (e) {
    PrentiveMaintenanceForcastdt.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#txtPMFsearchbox').val('');
    $('#advsearchsidebarPMForcast').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('.sidebar').removeClass('active');
    $('#PMForcastadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    GetForcastList();
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
$(document).on('click', '#liClearAdvSearchFilterAVPMF', function () {
    $("#txtPMFsearchbox").val("");
    clearAdvanceSearch();
    GetForcastList();
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarPMForcast').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    $("#gridadvsearchassign").val("").trigger('change');
    $(document).find("#txtDownRequired").val("").trigger('change.select2');
    $(document).find("#gridadvsearchshift").val("").trigger('change.select2');
    $(document).find("#PMFType").val("").trigger('change.select2');
}
//#endregion
//#region Export
$(document).on('click', "#downloadPMForcastdata", function () {
    $(document).find('.PreventiveMaintenanceForcastgridexport').trigger('click');
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            if (this.context[0].sInstance == "tblPMForcastGrid") {
                var currestsortedcolumn = $('#tblPMForcastGrid').dataTable().fnSettings().aaSorting[0][0];
                var coldir = $('#tblPMForcastGrid').dataTable().fnSettings().aaSorting[0][1];
                var colname = $('#tblPMForcastGrid').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
                var ScheduleType = $(document).find("#ScheduleType").val();
                var ForcastDate = ValidateDate($(document).find("#pMForcastModel_ForecastDate").val());
                var OnDemandGroup = $(document).find("#pMForcastModel_OnDemandGroup").val();
                var searchText = LRTrim($('#txtPMFsearchbox').val());
                var ClientLookupId = $(document).find("#PMFPmId").val();
                var Description = $(document).find("#PMFDescription").val();
                var SchedueledDate = ValidateDate($(document).find("#PMFScheduled").val());
                var AssignedTo_PersonnelClientLookupId = $(document).find("#PMFAssignedTo").val();
                var ChargeToClientLookupId = $(document).find("#PMFChargeTo").val();
                var ChargeToName = $(document).find("#PMFChargeToName").val();
                var Duration = $(document).find("#PMFJobDuration").val();
                var EstLaborHours = $(document).find("#PMFLaborHours").val();
                var EstLaborCost = $(document).find("#PMFLaborCost").val();
                var EstOtherCost = $(document).find("#PMFOtherCost").val();
                var EstMaterialCost = $(document).find("#PMFMaterialCost").val();
                var assignedPMS = $("#gridadvsearchassign").val();
                var requiredDate = LRTrim($("#gridadvsearchrequiredate").val());
                var timeoutError = "";
                var TypeList = $(document).find("#PMFType").val();
                var jsonResult = $.ajax({
                    url: '/PMForcast/GetPreventiveMainForcastGridPrintData?page=all',
                    "type": "post",
                      beforeSend: function () {
                          getSwalTimeOutPopAlert();
                    }, 
                    data: {
                        SearchText: searchText,
                        ScheduleType: ScheduleType,
                        ForcastDate: ForcastDate,
                        OnDemandGroup: OnDemandGroup,
                        ClientLookupId: ClientLookupId,
                        Description: Description,
                        SchedueledDate: SchedueledDate,
                        AssignedTo_PersonnelClientLookupId: AssignedTo_PersonnelClientLookupId,
                        ChargeToClientLookupId: ChargeToClientLookupId,
                        ChargeToName: ChargeToName,
                        Duration: Duration,
                        EstLaborHours: EstLaborHours,
                        EstLaborCost: EstLaborCost,
                        EstOtherCost: EstOtherCost,
                        EstMaterialCost: EstMaterialCost,
                        _colname: currestsortedcolumn,
                        _coldir: coldir,
                        assignedPMS: assignedPMS,
                        requiredDate: requiredDate,
                        downRequired: $("#txtDownRequired").val(),
                        shifts: $("#gridadvsearchshift").val(),
                        TypeList: TypeList

                    },
                    success: function (result) {
                      
                        result = JSON.parse(result);
                        timeoutError = result.timeoutError;
                        if (result.timeoutError == "Timeout") {
                            getSwalTimeoutErrorPopAlert();
                        }
                        else {
                            swal.close();
                        }
                    }, 
                    error: function (xhr, status, error) {
                        if (xhr.responseText.indexOf("request timed out") != -1) {
                             PrentiveMaintenanceForcastdt.destroy();
                            getSwalTimeoutErrorPopAlert();
                        }
                        else { swal.close(); }

                    },
                    async: false
                });
               
                if (timeoutError === "Timeout") {
                    return null;
                    //to stop from downloading empty file and there is also a console error that is not any issue in exporting data--
                    //error due to not returning in body and header structured way of export if structured way followed then it will download empty file
                }
                else {
                   

                    var thisdata = JSON.parse(jsonResult.responseText).data;
                    var visiblecolumnsIndex = $("#tblPMForcastGrid thead tr th").map(function (key) {
                        return this.getAttribute('data-th-index');
                    }).get();
                    var d = [];

                    $.each(thisdata, function (index, item) {
                        if (item.ScheduleType != null) {
                            item.ScheduleType = item.ScheduleType;
                        }
                        else {
                            item.ScheduleType = "";
                        }
                        if (item.ClientLookupId != null) {
                            item.ClientLookupId = item.ClientLookupId;
                        }
                        else {
                            item.ClientLookupId = "";
                        }
                        if (item.Description != null) {
                            item.Description = item.Description;
                        }
                        else {
                            item.Description = "";
                        }

                        if (item.SchedueledDate != null) {
                            item.SchedueledDate = item.SchedueledDate;
                        }
                        else {
                            item.SchedueledDate = "";
                        }

                        if (item.RequiredDate != null) {
                            item.RequiredDate = item.RequiredDate;
                        }
                        else {
                            item.RequiredDate = "";
                        }
                        if (item.Assigned != null) {
                            item.Assigned = item.Assigned;
                        }
                        else {
                            item.Assigned = "";
                        }                        
                        if (item.ChargeToClientLookupId != null) {
                            item.ChargeToClientLookupId = item.ChargeToClientLookupId;
                        }
                        else {
                            item.ChargeToClientLookupId = "";
                        }
                        if (item.ChargeToName != null) {
                            item.ChargeToName = item.ChargeToName;
                        }
                        else {
                            item.ChargeToName = "";
                        }
                        if (item.EstLaborHours != null) {
                            item.EstLaborHours = item.EstLaborHours;
                        }
                        else {
                            item.EstLaborHours = "";
                        }
                        if (item.EstLaborCost != null) {
                            item.EstLaborCost = item.EstLaborCost;
                        }
                        else {
                            item.EstLaborCost = "";
                        }
                        if (item.EstOtherCost != null) {
                            item.EstOtherCost = item.EstOtherCost;
                        }
                        else {
                            item.EstOtherCost = "";
                        }
                        if (item.EstMaterialCost != null) {
                            item.EstMaterialCost = item.EstMaterialCost;
                        }
                        else {
                            item.EstMaterialCost = "";
                        }
                        if (item.Assigned != null) {
                            item.Assigned = item.Assigned;
                        }
                        else {
                            item.Assigned = "";
                        }
                        if (item.DownRequired != null) {
                            item.DownRequired = item.DownRequired;
                        }
                        if (item.Shift != null) {
                            item.Shift = item.Shift;
                        }
                        else {
                            item.Shift = "";
                        }
                        var fData = [];
                        $.each(visiblecolumnsIndex, function (index, inneritem) {
                            var key = Object.keys(item)[inneritem];
                            var value = item[key]
                            fData.push(value);
                        });
                        d.push(fData);
                    })

                    return {
                        body: d,
                        header: $("#tblPMForcastGrid thead tr th").map(function (key) {
                            return this.innerHTML;
                        }).get()
                    };
                }
            }
        }
    });
})
//#endregion

//#region V2-712
//#region Inner Grid

$(document).on('click', '.PrevMaintAssignGrid', function (e) {

    var tr = $(this).closest('tr');
    var row = PrentiveMaintenanceForcastdt.row(tr);
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {

        this.src = "../../Images/details_close.png";
        var PrevMaintSchedId = $(this).attr("rel");
        $.ajax({
            url: "/PreventiveMaintenance/GetPrevMaintInnerGrid",
            data: {
                PrevMaintSchedId: PrevMaintSchedId
            },
            beforeSend: function () {
                ShowLoader();
            },
            dataType: 'html',
            success: function (json) {
                row.child(json).show();
                dtinnerGrid = row.child().find('.PrevMaintinnerDataTable').DataTable(
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

//#endregion
//#endregion
//#region V2-1013
$('#tblPMForcastGrid').on('mouseenter', '.ghover', function (e) {
    var rowData = PrentiveMaintenanceForcastdt.row(this).data();
    if (rowData != undefined) {
        var ChildCount = rowData.ChildCount;
        var thise = $(this);
        if (LRTrim(thise.find('.tooltipgrid').text()).length > 0 && ChildCount > 1) {
            thise.find('.tooltipgrid').attr('style', 'display :block !important;');
            return;
        }
    }

});
$('#tblPMForcastGrid').on('mouseleave', '.ghover', function (e) {
    $(this).find('.tooltipgrid').attr('style', 'display :none !important;');
});
//#endregion