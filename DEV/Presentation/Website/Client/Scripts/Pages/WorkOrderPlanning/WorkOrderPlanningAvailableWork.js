//#region Available Work
var RLAvailabledt;
var RLAvailableSelectedItemArray = [];
var woAvailableorder = '1';
var woAvailableDir = 'asc';
var selectedcount = 0;
var totalcountAW = 0;
var searchcount = 0;
var searchresult = [];
$(document).on('click', '#btnAvailableWorkRL', function () {
    RLAvailableSelectedItemArray = [];
    AWOGridTotalGridItemRL = [];
    WorkOrderRLIds = [];
    $.ajax({
        url: "/WorkOrderPlanning/AvailableWorkOrders",
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AvailableWorkRL').html(data);
            $(document).find('#AvailableWorkRLModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            GenerateAvailableRL();
            CloseLoader();
            $("#ApproveWORLadvsearchcontainer .sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            $(document).find('#WorkOrderPlanStartDate').val(PlanStartDate);
            $(document).find('#WorkOrderPlanEndDate').val(PlanEndDate);
        },
        error: function () {
            CloseLoader();
        }
    });
});

function GenerateAvailableRL(flag) {
    if ($(document).find('#tblAvailGridRL').hasClass('dataTable')) {
        RLAvailabledt.destroy();
    }
    RLAvailabledt = $(document).find("#tblAvailGridRL").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/WorkOrderPlanning/GetAvailableWorkOrderMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.WorkOrderPlanId = $(document).find('#workorderPlanningModel_WorkOrderPlanId').val();
                d.ClientLookupId = LRTrim($("#AWOWorkOrder").val());
                d.ChargeTo = LRTrim($("#AWOChargeTo").val());
                d.ChargeToName = LRTrim($("#AWOChargeToName").val());
                d.Description = LRTrim($("#AWODescription").val());
                d.Status = LRTrim($("#AWOStatus").val());
                d.Priority = LRTrim($('#AWOPriority').val());
                d.Type = LRTrim($("#AWOWOType").val());
                d.flag = flag;
            },
            "dataSrc": function (result) {
                AWOGridTotalGridItemRL = result.recordsTotal;
                $.each(result.data, function (index, item) {
                    searchresult.push(item.WorkOrderId);
                });
                let woAvailableorder = RLAvailabledt.order();
                woAvailableDir = woAvailableorder[0][1];
                searchcount = result.recordsTotal;
                if (totalcountAW < result.recordsTotal)
                    totalcountAW = result.recordsTotal;
                if (totalcountAW != result.recordsTotal)
                    selectedcount = result.recordsTotal;
                if (totalcountAW != 0 && (totalcountAW == WorkOrderRLIds.length || (searchcount != totalcountAW && arrayContainsArray(WorkOrderRLIds, searchresult) == true))) {
                    $('#RLavlidselectall').prop('checked', true);
                } else {
                    $('#RLavlidselectall').prop('checked', false);
                }
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
                'render': function (data, type, full, meta) {
                    if ($('#RLavlidselectall').is(':checked') && totalcountAW == selectedcount) {
                        return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvlRL ' + data + '"  value="'
                            + $('<div/>').text(data).html() + '">';
                    } else {
                        if (WorkOrderRLIds.indexOf(data) != -1) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-LSid="' + data + '" class="chksearchAvlRL ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-LSid="' + data + '" class="chksearchAvlRL ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        }
                    }
                }
            },        
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-200'>" + data + "</div>";
                }
            },
            { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            addControlsRL();
            if (totalcountAW != 0 && (totalcountAW == WorkOrderRLIds.length || (searchcount != totalcountAW && arrayContainsArray(WorkOrderRLIds, searchresult) == true))) {
                $('#RLavlidselectall').prop('checked', true);
            } else {
                $('#RLavlidselectall').prop('checked', false);
            }
        },
        'rowCallback': function (row, data, index) {
            if (data.Status == "Scheduled") {
                $(row).find('td').css('background-color', '#36a3f7b3');
                $(row).find('td').css('color', '#fff');
            }
        }
    });
}

$(document).on('click', '#RLavlidselectall', function (e) {
    flag = 0;
    WorkOrderRLIds = [];  
    searchresult = [];
    var checked = this.checked;
    var colname = woAvailableorder;
    var coldir = woAvailableDir;
    $.ajax({
        "url": "/WorkOrderPlanning/GetResourceListAvailable",      
        data:
        {
           colname: colname,
          coldir:coldir,
           WorkOrderPlanId : $(document).find('#workorderPlanningModel_WorkOrderPlanId').val(),
          ClientLookupId :LRTrim($("#AWOWorkOrder").val()),
           ChargeTo: LRTrim($("#AWOChargeTo").val()),
           ChargeToName : LRTrim($("#AWOChargeToName").val()),
           Description : LRTrim($("#AWODescription").val()),
           Status: LRTrim($("#AWOStatus").val()),
           Priority : LRTrim($('#AWOPriority').val()),
            Type : LRTrim($("#AWOWOType").val()),
           flag : flag         
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
                        if (WorkOrderRLIds.indexOf(item.WorkOrderId) == -1) {
                            WorkOrderRLIds.push(item.WorkOrderId);
                            var itemRL = new RLAvailableSelectedItem(item.WorkOrderId, item.ClientLookupId, item.ChargeTo, item.ChargeToName, item.Description, item.Status, item.Priority,
                                item.Duration, item.RequiredDate);
                            RLAvailableSelectedItemArray.push(itemRL);

                        }
                    } else {
                        var i = WorkOrderRLIds.indexOf(item.WorkOrderId);
                        WorkOrderRLIds.splice(i, 1);
                        RLAvailableSelectedItemArray.splice(i, 1);
                    }
                });
            }

        },
        complete: function () {
           RLAvailabledt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearchAvlRL').prop('checked', 'checked');
                } else {
                    $(this).find('.chksearchAvlRL').prop('checked', false);
                }
           });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearchAvlRL', function () {
    var data = RLAvailabledt.row($(this).parents('tr')).data();
    if (!this.checked) {
        selectedcount--;
        var index = WorkOrderRLIds.indexOf(data.WorkOrderId);
        WorkOrderRLIds.splice(index, 1);
        var el = $('#RLavlidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        RLAvailableSelectedItemArray = RLAvailableSelectedItemArray.filter(function (el) {
            return el.WorkOrderId !== data.WorkOrderId;
        });
    }
    else {
        WorkOrderRLIds.push(data.WorkOrderId);
        selectedcount = selectedcount + WorkOrderRLIds.length;
        var item = new RLAvailableSelectedItem(data.WorkOrderId, data.ClientLookupId, data.ChargeTo, data.ChargeToName, data.Description, data.Status, data.Priority,
            data.Duration, data.RequiredDate);
        var found = RLAvailableSelectedItemArray.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        if (!found) { RLAvailableSelectedItemArray.push(item); }
    }
    if (AWOGridTotalGridItemRL == RLAvailableSelectedItemArray.length) {
        $('#RLavlidselectall').prop('checked', 'checked');
    }
    else {
        $('#RLavlidselectall').prop('checked', false);
    }
});
function RLAvailableSelectedItem(WorkOrderId, ClientLookupId, ChargeTo, ChargeToName, Description, Status, Priority, ScheduledDurations, RequiredDate) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.ChargeTo = ChargeTo;
    this.ChargeToName = ChargeToName;
    this.Description = Description;
    this.Status = Status;
    this.Priority = Priority;
    this.ScheduledDurations = ScheduledDurations;
    this.RequiredDate = RequiredDate;
}

var SelectedScheduledDurationsAssignedRL = [];
var SelectedLookupIdToAssignedRL = [];
var SelectedStatusAssignedRL = [];
var SelectedWoIdToAssignedRL = [];
$(document).on('click', '#btnRLAddAvailableWO', function (e) {
    var WorkOrderValsRL = WorkOrderRLIds;
    if (WorkOrderValsRL.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        for (var i = 0; i < RLAvailableSelectedItemArray.length; i++) {
            SelectedWoIdToAssignedRL.push(RLAvailableSelectedItemArray[i].WorkOrderId);           
            SelectedLookupIdToAssignedRL.push(RLAvailableSelectedItemArray[i].ClientLookupId);
            SelectedStatusAssignedRL.push(RLAvailableSelectedItemArray[i].Status);
            SelectedScheduledDurationsAssignedRL.push(RLAvailableSelectedItemArray[i].ScheduledDurations);
        }
        $(document).find('#AssignRLstartdate').val("").trigger('change');
        $('#ddlAssUserRL').val("").trigger("change.select2");
        $(document).find('#availableWorkAssignRLModel_WorkOrderIds').val(SelectedWoIdToAssignedRL);     
        $(document).find('#availableWorkAssignRLModel_ClientLookupIds').val(SelectedLookupIdToAssignedRL);
        $(document).find('#availableWorkAssignRLModel_Status').val(SelectedStatusAssignedRL);
        $(document).find('#availableWorkAssignRLModel_ScheduledDurations').val(SelectedScheduledDurationsAssignedRL);
        $(document).find('#availableWorkAssignRLModel_WorkOrderPlanStartDate').val(PlanStartDate);
        $(document).find('#availableWorkAssignRLModel_WorkOrderPlanEndDate').val(PlanEndDate);
        $(document).find('.select2picker').select2({});
        $(document).find('#AvailableWorkAssignRLModal').modal({ backdrop: 'static', keyboard: false, show: true });
        SetControlsRL();
        addControlsRL();
        $(document).find('form').find("#AssignRLstartdate").removeClass("input-validation-error");
    }
});
$(document).on("click", "#AvailablesidebarCollapseRL", function () {
    $('#ApproveWORLadvsearchcontainer .sidebar').addClass('active');
    $('.overlay2').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
function SetControlsRL() {
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
}
function addControlsRL() {
    $(document).find('.dtpickerRL').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
function AvailableWorkRLAddOnSuccess(data) {
    if (data.data === "success") {
        $(document).find('#AvailableWorkRLModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("AvailableWorkAssignSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            $(document).find('#AssignRLstartdate').val("").trigger('change');
            $('#ddlAssUserRL').val("").trigger("change.select2");
            if ($('.tablinks.active').eq(0).data('tab') === 'ResourceList') {
                dtTableRL.page('first').draw('page');
            }
            else if ($('.tablinks.active').eq(0).data('tab') === 'ResourceCalender') {
                calendar.refetchEvents();
            }
        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
    }
    RLAvailableSelectedItemArray = [];
    AWOGridTotalGridItemRL = []; 
    SelectedScheduledDurationsAssignedRL = [];
    SelectedLookupIdToAssignedRL = [];
    SelectedStatusAssignedRL = [];
    SelectedWoIdToAssignedRL = [];
    WorkOrderRLIds = [];
    $(document).find('#AssignRLstartdate').val("").trigger('change');
    $('#ddlAssUserRL').val(null).trigger("change.select2");
    $(document).find('#AvailableWorkAssignRLModal').modal('hide');
    $(document).find('#RLavlidselectall').prop('checked', false);
}
$(document).on('click', '.btnAssRLcancelmod', function () {

    var areaddescribedby = $(document).find("#ddlAssUserRL").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlAssUserRL").removeClass("input-validation-error");
    $(document).find('form').find("#AssignRLstartdate").removeClass("input-validation-error");

});


//#region Advance Search
$(document).on('click', "#btnAvailableRLDataAdvSrch", function (e) {
    WorkOrderRLIds = [];
    searchresult = [];
    var searchitemRLhtml = "";
    hGridfilterRLitemcount = 0;
    $('#txtRLsearchbox').val('');
    $('#advsearchsidebarAvailableRL').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val()) {
            hGridfilterRLitemcount++;
            searchitemRLhtml = searchitemRLhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistoryRL" aria-hidden="true"></a></span>';
        }
    });
    searchitemRLhtml = searchitemRLhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritemsAWORL').html(searchitemRLhtml);
    $('#ApproveWORLadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
    GridAdvanceSearchRL();
});
function GridAdvanceSearchRL() {
    var IsChecked = $(document).find('#AlreadyScheduledIdRL').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    RLAvailabledt.page('first').draw('page');
    $('.AWOfilteritemcountRL').text(hGridfilterRLitemcount);
}

$(document).on('click', '#liClearAdvSearchFilterAVlRLAWO', function () {
    var IsChecked = $(document).find('#AlreadyScheduledIdRL').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    clearAdvanceSearchAWORL();
    RLAvailabledt.page('first').draw('page');
});
function clearAdvanceSearchAWORL() {
    var filteritemcount = 0;
    $('#advsearchsidebarAvailableRL').find('input:text').val('');
    $('.AWOfilteritemcountRL').text(filteritemcount);  
    $('#advsearchfilteritemsAWORL').find('span').html('');
    $('#advsearchfilteritemsAWORL').find('span').removeClass('tagTo');
}

$(document).on('click', '.btnRLAWOClose,#btnRLAWOCancel', function () {
    RLAvailableSelectedItemArray = [];
    AWOGridTotalGridItemRL = [];   
    SelectedScheduledDurationsAssignedRL = [];
    SelectedLookupIdToAssignedRL = [];
    SelectedStatusAssignedRL = [];
    SelectedWoIdToAssignedRL = [];
    WorkOrderRLIds = [];
});
$(document).on('click', '.btnCrossHistoryRL', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilterRLitemcount--;
    GridAdvanceSearchRL();
});
$(document).on('click', '#dismissAW', function () {
    $(document).find('#ApproveWORLadvsearchcontainer .sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
});
//#endregion
//#endregion