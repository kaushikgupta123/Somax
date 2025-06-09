//#region Global variables
var StartScheduledDate = '';
var EndScheduledDate = '';
var PersonnelList = '';
var gridname = "ListLaborScheduling_Search";
var CustomQueryDisplayId = "1";
var run = false;
var selectedcount = 0;
var woToupdate = [];
var WoCheckedItem = [];
var selectCount = 0;
var LaborAvailabledt;
var dtTable;
var today = $.datepicker.formatDate('mm/dd/yy', new Date());
var LaborAvailableSelectedItemArray = [];
var WorkOrderIds = [];
var AWOGridTotalGridItem = 0;
var flag = 0;
var ModelOpened = false;
var OldCustomQueryDisplayId = "0";
var WoHoursEditArray = [];
var WoScheduleEditArray = [];
//#endregion
$(function () {
    $(".actionBar").fadeIn();
    $("#lsGridAction :input").attr("disabled", "disabled");
    $(document).find('.dtpickerNew').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true,
        minDate: new Date()
    }).inputmask('mm/dd/yyyy');
    ShowbtnLoaderclass("LoaderDrop");
    ShowbtnLoader("btnsortmenu");
    $(document).find('.select2picker').select2({});
    GetUserAssignedDropdown();//V2-562 
    $(document).on('click', "#action", function () {
        $(".actionDrop").slideToggle();
    });
    $(document).on('focusout', "#action", function () {
        $(".actionDrop").fadeOut();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {

        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();

    });
    $('.overlay2').on('click', function () {
        $('#ApproveWOadvsearchcontainer').find('.sidebar').removeClass('active');
        $(document).find('.overlay2').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    $(document).find(".groupBlock").css("display", "inline-block");
    $(document).find(".assignedBlock").css("display", "inline-block");
    $(document).find(".scheduledBlock").css("display", "inline-block");
    $(document).find("#sidebarCollapse").css("display", "inline-block");
    $(document).find("#SrchBttnNew").css("display", "inline-block");
    $(document).find("#btnAvailableWork").css("display", "inline-block");
    $(document).find("#btnWoExport").css("display", "inline-block");
    $(document).find("#btnClear").css("display", "inline-block");
    generateListLaborSchedulingDataTable();
    addControls();
});
//#region Multiple Select Drowpdown  With CheckBox V2-562
var Select2MultiCheckBoxObj = [];
var id_selectElement = 'ddlUser';
var staticWordInID = 'state_';
var Totalchecked = 0;
var Allselected = false;
function GetUserAssignedDropdown() {
    $.map($('#' + id_selectElement + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObj(option.value, false);
    });

    function formatResult(state) {
        if (Select2MultiCheckBoxObj.length > 0) {
            var stateId = staticWordInID + state.id;
            let index = Select2MultiCheckBoxObj.findIndex(x => x.id == state.id);
            if (index > -1) {
                var checkbox = "";
                if (index == 0) {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUser " id="state_0" type="checkbox" ' + (Select2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkboxstate_0 "> ' + state.text + '</label></div>', { id: 'state_0' });
                }
                else {
                    checkbox = $('<div class="checkbox"><input class="select2Checkbox checkboxAssignedUser" id="' + stateId + '" type="checkbox" ' + (Select2MultiCheckBoxObj[index]["IsChecked"] ? 'checked' : '') +
                        '><label for="checkbox' + stateId + '"> ' + state.text + '</label></div>', { id: stateId });
                }

                return checkbox;
            }
        }
    }

    let optionSelect2 = {
        templateResult: formatResult,
        closeOnSelect: false,
        allowClear: true
        /* width: '100%'*/
    };

    let $select2 = $(document).find("#" + id_selectElement).select2(optionSelect2);
    $("#" + id_selectElement).find("option[value='']").attr("disabled", 'disabled');
    getCalMultiplecheck();
    $select2.on('select2:close', function () {

    });
    $select2.on("select2:select", function (event) {
        $("#" + staticWordInID + event.params.data.id).prop("checked", true);
        AddItemInSelect2MultiCheckBoxObj(event.params.data.id, true);
        //If all options are slected then selectAll option would be also selected.

        if (Select2MultiCheckBoxObj.filter(x => x.IsChecked === false).length === 1) {
            AddItemInSelect2MultiCheckBoxObj(0, true);
            $("#" + staticWordInID + "0").prop("checked", true);
        }
        Totalchecked = Totalchecked + 1;
        getCalMultiplecheck();
    });

    $select2.on("select2:unselect", function (event) {
        $("#" + staticWordInID + "0").prop("checked", false);
        AddItemInSelect2MultiCheckBoxObj(0, false);
        $("#" + staticWordInID + event.params.data.id).prop("checked", false);
        AddItemInSelect2MultiCheckBoxObj(event.params.data.id, false);
        Totalchecked = Totalchecked - 1;
        getCalMultiplecheck();
    });

    $(document).on("click", "#" + staticWordInID + "0", function () {
        var b = $("#" + staticWordInID + "0").is(':checked');
        IsCheckedAllOption(b);
        if (b == true) {
            Totalchecked = $('.checkboxAssignedUser:checked').length - 1;
        } else {
            Totalchecked = $('.checkboxAssignedUser:checked').length;
        }
        getCalMultiplecheck();
        $("#" + id_selectElement).select2("close");
    });
    $(document).on("click", ".checkboxAssignedUser", function (event) {
        let selector = "#" + this.id;
        let isChecked = false;
        if (this.id == "state_0") {
            isChecked = Select2MultiCheckBoxObj[Select2MultiCheckBoxObj.findIndex(x => x.id == "")]['IsChecked'] ? true : false;
        } else {
            isChecked = Select2MultiCheckBoxObj[Select2MultiCheckBoxObj.findIndex(x => x.id == this.id.replaceAll(staticWordInID, ''))]['IsChecked'] ? true : false;
        }
        $(selector).prop("checked", isChecked);
        getCalMultiplecheck();
    });
    $(document).on("click", ".assignedBlock span  .select2-selection__rendered", function (event) {
        $(".assignedBlock span ul li .select2-search__field").val("");
    });
    $(document).on("click", ".assignedBlock span  .select2-selection__rendered .select2-selection__clear", function (event) {
        IsCheckedAllOption(false);
        Totalchecked = $('.checkboxAssignedUser:checked').length;
        getCalMultiplecheck();
        $("#" + id_selectElement).select2("close");

    });
    $(document).on('focusout', '.assignedBlock span ul li .select2-search__field', function (e) {
        getCalMultiplecheck();
    });
    $(document).on('keypress', '.assignedBlock span ul li .select2-search__field', function (e) {
        var tcount = Totalchecked;
        var ftcount = "";
        if (tcount == 0) {
            ftcount = "All Personnel";
        }
        else {
            ftcount = tcount.toString() + " People";
        }
        var text = $(".assignedBlock span ul li .select2-search__field").val();
        $(".assignedBlock span ul li .select2-search__field").val(text.replace(ftcount, ''));
        $('.assignedBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    });


}
function AddItemInSelect2MultiCheckBoxObj(id, IsChecked) {
    if (Select2MultiCheckBoxObj.length > 0) {
        let index = Select2MultiCheckBoxObj.findIndex(x => x.id == id);
        if (index > -1) {
            Select2MultiCheckBoxObj[index]["IsChecked"] = IsChecked;
        }
        else {
            Select2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
        }
    }
    else {
        Select2MultiCheckBoxObj.push({ "id": id, "IsChecked": IsChecked });
    }
}
function IsCheckedAllOption(trueOrFalse) {
    $.map($('#' + id_selectElement + ' option'), function (option) {
        AddItemInSelect2MultiCheckBoxObj(option.value, trueOrFalse);
    });
    $('#' + id_selectElement + " > option").not(':first').prop("selected", trueOrFalse);
    //This will select all options and adds in Select2
    $("#" + id_selectElement).trigger("change");//This will effect the changes
    /* $(".select2-results__option").not(':first').attr("aria-selected", trueOrFalse);*/
    //This will make grey color of selected options

    $("input[id^='" + staticWordInID + "']").prop("checked", trueOrFalse);
}
function getCalMultiplecheck() {
    $('.assignedBlock span ul li .select2-search__field[placeholder=""]').attr('style', 'width:100%');
    $(".assignedBlock span  .select2-selection__rendered").find('.select2-selection__choice').remove();
    Allselected = $("#" + staticWordInID + "0").prop('checked') ? true : false;
    var tcount = Totalchecked;
    var ftcount = "";
    if (tcount == 0) {
        ftcount = "All Personnel";
    }
    else {
        ftcount = tcount.toString() + " People";
    }
    $(".assignedBlock span ul li .select2-search__field").val(ftcount);

}
//#endregion Multiple Select Drowpdown  With CheckBox V2-562
var order = '0';
var orderDir = 'asc';
function generateListLaborSchedulingDataTable() {
    WoHoursEditArray = [];
    WoScheduleEditArray = [];
    if ($(document).find('#ListLaborSchedulingSearchTable').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#ListLaborSchedulingSearchTable").DataTable({
        //colReorder: {
        //    fixedColumnsLeft: 2
        //},
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
        },
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Labor Scheduling List'
            },
            {
                extend: 'print',
                title: 'Labor Scheduling List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Labor Scheduling List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Labor Scheduling List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/LaborScheduling/GetListLaborSchedulingGridData",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.CustomQueryDisplayId = CustomQueryDisplayId;
                d.ClientLookupId = LRTrim($("#EquipmentId").val());
                d.Name = LRTrim($("#Name").val());
                d.Description = LRTrim($("#Description").val());
                d.RequiredDate = LRTrim($("#RequiredDate").val());
                d.StartScheduledDate = ValidateDate(StartScheduledDate);
                d.EndScheduledDate = ValidateDate(EndScheduledDate);
                d.Type = LRTrim($("#Type").val());
                d.PersonnelList = PersonnelList;
                d.Order = $("#GroupingLabor").val();//V2-524 Grouping
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];
                if (result.data.length < 1) {
                    $(document).find('.import-export').prop('disabled', true);
                }
                else {
                    $(document).find('.import-export').prop('disabled', false);
                }

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            },
            global: true
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
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
                        return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="chksearch"  value="'
                            + $('<div/>').text(data).html() + '">';
                    }
                },
                {
                    "data": "PersonnelName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": false,
                    "className": "text-left",
                    "name": "1"

                },
                {
                    "data": "WorkOrderClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": false,
                    "className": "text-left",
                    "name": "2",
                    //V2-838
                    "mRender": function (data, type, full, row) {
                        if (full['PartsOnOrder'] > 0) {
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                        } else {
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '</a></div>'
                        }
                    }

                },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": false, "name": "3",
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-300'>" + data + "</div>";
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "4", },
                {
                    "data": "ScheduledStartDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date", "className": "text-left Personnel", "name": "5",
                    'render': function (data, type, row) {
                        if (WoScheduleEditArray.indexOf(row.WorkOrderScheduleId) != -1) {
                            return "<div style='width:110px !important; position:relative;'><input type='text'  class='dtpicker Scheduleddtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important; position:relative;'><input type='text'  class='dtpicker Scheduleddtpicker dt-inline-text' readonly='readonly' autocomplete='off' value='" + data + "'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-22px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-22px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-22px;display:none;' /></div>";
                        }
                    }
                },
                {
                    "data": "ScheduledHours", "autoWidth": true, "bSearchable": true, "bSortable": false,
                    'render': function (data, type, row) {
                        if (WoHoursEditArray.indexOf(row.WorkOrderScheduleId) != -1) {
                            return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-hours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                        }
                    }
                },
                {
                    "data": "RequiredDate", "autoWidth": true, "bSearchable": true, "bSortable": false, "type": "date", "name": "7"
                },
                {
                    "data": "EquipmentClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "8"
                },
                {
                    "data": "ChargeTo_Name", "autoWidth": true, "bSearchable": true, "bSortable": false, "name": "9"
                }

            ],
        'rowCallback': function (row, data, dataIndex) {
            var found = WoCheckedItem.some(function (el) {
                return el.WorkOrderSchedId == data.WorkOrderScheduleId;
            });
            if (found) {
                $(row).find('input[type="checkbox"]').prop('checked', true);
            }

        },
        initComplete: function () {
            //Hide column based on the grouping
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                "dateFormat": "mm/dd/yy",
                autoclose: true,
                changeMonth: true,
                changeYear: true
            });
            var GroupingId = $("#GroupingLabor").val();
            SetPageLengthMenu();
            $("#lsGridAction :input").removeAttr("disabled");
            DisableExportButton($("#ListLaborSchedulingSearchTable"), $(document).find('.import-export'));
        },
        "drawCallback": function (settings, data) {

            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            var aData = new Array();
            var info = dtTable.page.info();
            var GroupingValue = $("#GroupingLabor").val();
            var thisgroupId = 0;
            if (GroupingValue == "0") {
                thisgroupId = 1;
                api.column(1).visible(true);
                api.column(5).visible(true);
            }
            else {
                thisgroupId = 5;
                api.column(1).visible(true);
                api.column(5).visible(true);
            }

            var thisgroup = $(this).find('td').eq(thisgroupId).text();
            var last = thisgroup;

            var start = info.start;
            var end = info.end;

            api.column(thisgroupId).data().each(function (group, i) {
                if ((last.toUpperCase() == group.toUpperCase() && i == 0)) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan=' + 10 + '>' + group + '</td></tr>'
                    );
                    last = group;
                }
                if (GroupingValue == "1") {
                    if ((last.toUpperCase() != group.toUpperCase())) {
                        $(rows).eq(i).before(
                            '<tr class="group"><td colspan=' + 10 + '>' + group + '</td></tr>'
                        );
                        last = group;
                    }
                }


                last = group;
                var TotalCount = getData[0].TotalCount;
                var tablelength = api.column(1).data().length;
                var PerIDNextValue = getData[i].PerIDNextValue;
                var PersonnelId = getData[i].PersonnelId;
                var PerNextValue = getData[i].PerNextValue;
                var pageLength = info.length;

                if (GroupingValue == "0") { //Print only at the time of assigned
                    if (last.toUpperCase() == group.toUpperCase() /*&& i != 0*/ && PerIDNextValue != PersonnelId) {
                        if (PerNextValue != "" && (pageLength - 1) != i)
                            $(rows).eq(i).after(
                                '<tr class="group"><td colspan=' + 10 + '>' + PerNextValue + '</td></tr>'
                            );
                    }
                }

                //Print Grand Total Hour at the end for both grouping

                if (end == TotalCount && tablelength == (i + 1)) {
                    var rowhtml_grabdtotal = rowCreateGrandTotal();
                    var sum_GrandTotal = getData[i].GrandTotalHour;
                    var thiscolumn_GrandTotal = "ScheduledHours";
                    rowhtml_grabdtotal = rowhtml_grabdtotal.replace("ScheduledHours", GetFormattednumber(thiscolumn_GrandTotal[0], sum_GrandTotal));
                    $(rows).eq(i).after(rowhtml_grabdtotal);
                }

                if (GroupingValue == "0") { //Print Total Hours when grouping by Assigned
                    if ((last.toUpperCase() !== PerNextValue.toUpperCase() || PerNextValue == "") || PerIDNextValue != PersonnelId) {
                        var rowhtml = rowCreate('');
                        var sum = getData[i].SumPersonnelHour;
                        var thiscolumn = "ScheduledHours";
                        rowhtml = rowhtml.replace("ScheduledHours", GetFormattednumber(thiscolumn[0], sum));
                        $(rows).eq(i).after(rowhtml);
                    }
                }
                else { ////Print Total Hours when grouping by Schedule date
                    var SDNextValue = getData[i].SDNextValue;
                    if (last !== SDNextValue || SDNextValue == "") {
                        var rowhtml_date = rowCreate('');
                        var sum_date = getData[i].SumScheduledateHour;
                        var thiscolumn_date = "ScheduledHours";
                        rowhtml_date = rowhtml_date.replace("ScheduledHours", GetFormattednumber(thiscolumn_date[0], sum_date));
                        $(rows).eq(i).after(rowhtml_date);
                    }
                }

            });
            // SetPageLengthMenu();
            CloseLoader();

            if (GroupingValue == "0") {
                api.column(1).visible(false);
                api.column(5).visible(true);
            }
            else {
                api.column(5).visible(false);
                api.column(1).visible(true);
            }
            $('#ListLaborSchedulingSearchTable').dataTable({ "bSort": false });
            $(document).find('.dtpicker').datepicker({
                minDate: 0,
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
        }

    });
}
function intVal(i) {
    return typeof i === 'string' ?
        i.replace(/[\$,]/g, '') * 1 :
        typeof i === 'number' ?
            i : 0;
};
function rowCreate(GrandTotal) {

    var changedColumns = dtTable.settings()[0].aoColumns;
    var rowstring = '<tr style="color:#575962 !important;">';
    for (var i = 0; i < changedColumns.length - 1; i++) {
        if (changedColumns[i].bVisible) {
            if (changedColumns[i + 1].data == "ScheduledHours")
                if (GrandTotal != "") {
                    rowstring = rowstring + '<td align = "left" style="font-weight:500;padding-left:1px">' + 'Grand Total : ' + changedColumns[i + 1].data + '</td>';
                }
                else {
                    rowstring = rowstring + '<td style="text-align:right !important;font-weight: 400 !important;color: #000 !important;position: relative;height: 18px;"><div style="position:absolute;width:88px;left:0;top:0;min-height:34px;line-height:34px;">' + changedColumns[i + 1].data + '</div></td>';
                }

            else
                rowstring = rowstring + "<td></td>";
        }
    }
    rowstring = rowstring + '</tr>';

    return rowstring;
};
function rowCreateGrandTotal() {
    var changedColumns = dtTable.settings()[0].aoColumns;
    var rowstring = '<tr style="color:#575962 !important;">';
    for (var i = 0; i < changedColumns.length - 2; i++) {
        if (changedColumns[i + 2].bVisible) {
            if (changedColumns[i + 2].data == "ScheduledHours") {
                rowstring = rowstring + '<td align="right" style="font-weight:500;padding-left:1px">Grand Total : </td><td style="text-align:right !important;font-weight: 400 !important;color: #000 !important;position: relative;height: 18px;"><div style="position:absolute;width:88px;left:0;top:0;min-height:34px;line-height:34px;">' + changedColumns[i + 2].data + '</div></td>';
            }
            else
                rowstring = rowstring + "<td></td>";
        }
    }
    rowstring = rowstring + '</tr>';
    return rowstring;
};
function GetFormattednumber(column, numberToFormat) {
    var result = numberToFormat;
    var format = column.NumericFormat;
    if (format) {
        format = format.toUpperCase();
        if (format == plain) {
            if (column.NumofDecPlaces > 0) {
                return getFlooredFixed(numberToFormat, column.NumofDecPlaces);
            }
            return result;
        }
        else if (format == number && column.NumofDecPlaces) {
            return parseFloat(numberToFormat).toLocaleString(undefined, { minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
        }
        else if (format == currency) {
            var decimelplaces = 0;
            if (column.NumofDecPlaces != 0) {
                decimelplaces = column.NumofDecPlaces;
            }
            if (column.CurrencyCode) {
                return parseFloat(numberToFormat).toLocaleString(column.SiteLocalization, { style: 'currency', currency: column.CurrencyCode, minimumFractionDigits: decimelplaces, maximumFractionDigits: decimelplaces });
            }

        }
        else if (format == percentage) {
            if (column.NumofDecPlaces) {
                return parseFloat(numberToFormat).toLocaleString(undefined, { style: 'percent', minimumFractionDigits: column.NumofDecPlaces, maximumFractionDigits: column.NumofDecPlaces });
            }

        }
    }
    return result;
}
$(document).on('click', '#ListLaborSchedulingSearchTable_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#ListLaborSchedulingSearchTable_length .searchdt-menu', function () {
    run = true;
});
$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var PersonnelList = $(document).find('#ddlUser').val();
            var jsonResult = $.ajax({
                "url": "/LaborScheduling/GetListLaborSchedulingPrintData",
                "type": "post",
                "datatype": "json",
                data: {
                    CustomQueryDisplayId: CustomQueryDisplayId,
                    ClientLookupId: LRTrim($("#EquipmentId").val()),
                    Name: LRTrim($("#Name").val()),
                    Description: LRTrim($("#Description").val()),
                    RequiredDate: LRTrim($("#RequiredDate").val()),
                    StartScheduledDate: ValidateDate(StartScheduledDate),
                    EndScheduledDate: ValidateDate(EndScheduledDate),
                    Type: LRTrim($("#Type").val()),
                    "PersonnelList": PersonnelList,
                    SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
                    colname: order,
                    coldir: orderDir

                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#ListLaborSchedulingSearchTable thead tr th").not(":eq(0)").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.PersonnelName != null) {
                    item.PersonnelName = item.PersonnelName;
                }
                else {
                    item.PersonnelName = "";
                }
                if (item.WorkOrderClientLookupId != null) {
                    item.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
                }
                else {
                    item.WorkOrderClientLookupId = "";
                }
                if (item.Description != null) {
                    item.Description = item.Description;
                }
                else {
                    item.Description = "";
                }

                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
                }
                if (item.ScheduledStartDate != null) {
                    item.ScheduledStartDate = item.ScheduledStartDate;
                }
                else {
                    item.ScheduledStartDate = "";
                }
                if (item.ScheduledHours != null) {
                    item.ScheduledHours = item.ScheduledHours;
                }
                else {
                    item.ScheduledHours = "";
                }
                if (item.RequiredDate != null) {
                    item.RequiredDate = item.RequiredDate;
                }
                else {
                    item.RequiredDate = "";
                }

                if (item.EquipmentClientLookupId != null) {
                    item.EquipmentClientLookupId = item.EquipmentClientLookupId;
                }
                else {
                    item.EquipmentClientLookupId = "";
                }

                if (item.ChargeTo_Name != null) {
                    item.ChargeTo_Name = item.ChargeTo_Name;
                }
                else {
                    item.ChargeTo_Name = "";
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
                header: $("#ListLaborSchedulingSearchTable thead tr th").not(":eq(0)").find('div').map(function (key) {
                    if (this.innerHTML) {
                        return this.innerHTML;
                    }
                }).get()
            };
        }
    });
});

$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    $('#mask').trigger('click');
});
$(document).on('click', '#liPrint,#liPdf,#liExcel', function () {
    var thisid = $(this).attr('id');
    var TableHaederProp = [];
    var PersonnelList = $(document).find('#ddlUser').val();
    function table(property, title) {
        this.property = property;
        this.title = title;
    }
    $("#ListLaborSchedulingSearchTable thead tr th").map(function (key) {
        var thisdiv = $(this).find('div');
        var tablearr = new table(this.getAttribute('data-th-prop'), thisdiv.html());
        TableHaederProp.push(tablearr);
    });
    var params = {
        tableHaederProps: TableHaederProp,
        colname: $("#GroupingLabor").val(),
        coldir: orderDir,
        CustomQueryDisplayId: CustomQueryDisplayId,
        ClientLookupId: LRTrim($("#EquipmentId").val()),
        Name: LRTrim($("#Name").val()),
        Description: LRTrim($("#Description").val()),
        RequiredDate: LRTrim($("#RequiredDate").val()),
        StartScheduledDate: ValidateDate(StartScheduledDate),
        EndScheduledDate: ValidateDate(EndScheduledDate),
        Type: LRTrim($("#Type").val()),
        "PersonnelList": PersonnelList,
        SearchText: LRTrim($(document).find('#txtColumnSearch').val()),
    };
    LSPrintParams = JSON.stringify({ 'LSPrintParams': params });
    $.ajax({
        "url": "/LaborScheduling/SetPrintData",
        "data": LSPrintParams,
        contentType: 'application/json; charset=utf-8',
        "dataType": "json",
        "type": "POST",
        "success": function () {
            if (thisid == 'liPdf') {
                window.open('/LaborScheduling/ExportASPDF?d=d', '_self');
            }
            else if (thisid == 'liPrint') {
                window.open('/LaborScheduling/ExportASPDF', '_blank');
            }
            else if (thisid == 'liExcel') {
                window.open('/LaborScheduling/ExportASPDF?d=excel', '_self');
            }
            return;
        }
    });
    $('#mask').trigger('click');
});
//#region Additional Search
var filterinfoarray = [];
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
function getfilterinfoarray(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
    });
    return filterinfoarray;
}
function setsearchui(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            else if (item.key == 'advrequiredDatedaterange' && item.value !== '') {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }
            if (item.key == 'RequiredDate') {
                $("#RequiredDate").trigger('change.select2');
            }
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}

var SelectTimePeriod = '';
$('#ScheduledDate').on('select2:open', function (e) {

    SelectTimePeriod = $(this).val();
});

$('#ScheduledDate').on('select2:close', function (e) {

    //if (SelectTimePeriod == $(this).val()) {
    //    $('#ScheduledDate').val($(this).val()).trigger("change");
    //}
});
$(".canceldtpicker").on('click', function (e) {
    /* $('#ScheduledDate').val(OldCustomQueryDisplayId).trigger("change.select2");*/
});
$(document).on('change', '#ScheduledDate', function () {
    var thisval = $(this).val()
    CustomQueryDisplayId = thisval;
    ModelOpened = false;
    ChangeListLSHeaderInfo();



});
$(document).on('click', '#btntimeperiod', function (e) {
    ModelOpened = true;
    OldCustomQueryDisplayId = '6';
    $(document).find('#DateRangeScheduledModal').modal('hide');
    ChangeListLSHeaderInfo();

});
$("#btnLSDataAdvSrch").on('click', function (e) {
    run = true;
    $(document).find('#txtColumnSearch').val('');
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    LSAdvSearch();
    dtTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    LSAdvSearch();
    dtTable.page('first').draw('page');
});
$(document).on('change', '#ddlUser', function () {
    PersonnelList = $(this).val();

    ChangeListLSHeaderInfo();


});

function ChangeListLSHeaderInfo() {
    dtTable.page('first').draw('page');
}
function LSAdvSearch() {

    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).attr('id') != 'advrequiredDatedaterange') {
            if ($(this).val()) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        else {
            if ($(this).val()) {
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $("#advsearchfilteritems").html(searchitemhtml);
    $('#_advrequiredDatedaterange').hide();
    $(".filteritemcount").text(selectCount);
}
function clearAdvanceSearch() {
    selectCount = 0;
    $("#EquipmentId").val("");
    $('#Name').val("");
    $("#Description").val("");
    $("#RequiredDate").val("").trigger('change');
    $("#Type").val("");
    $("#advsearchfilteritems").html('');    
    $(".filteritemcount").text(selectCount);

}
//#endregion
//$('#ListLaborSchedulingSearchTable').find('th').click(function () {
//    run = true;
//    order = $(this).data('col');
//});

//#region Multi Select
$(document).on('change', '.chksearch', function () {
    var data = dtTable.row($(this).parents('tr')).data();

    if (!this.checked) {
        selectedcount--;
        woToupdate = woToupdate.filter(function (el) {
            return el.WorkOrderId !== data.WorkOrderId;
        });
        WoCheckedItem = WoCheckedItem.filter(function (wo) {
            return wo.WorkOrderSchedId !== data.WorkOrderScheduleId;
        });
    }
    else {
        var item = new SelectedItemData(data.WorkOrderId, data.WorkOrderClientLookupId, data.WoStatus, data.WorkOrderScheduleId, data.ScheduledHours);
        var found = woToupdate.some(function (el) {
            return el.WorkOrderId === data.WorkOrderId;
        });
        WoCheckedItem.push(item);
        if (!found) { woToupdate.push(item); }
        selectedcount = selectedcount + woToupdate.length;
    }
    if (woToupdate.length > 0 || WoCheckedItem.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();

    }
    else {
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
    }
    $('.itemcount').text(WoCheckedItem.length);
});

function SelectedItemData(WorkOrderId, ClientLookupId, Status, WorkOrderSchedId, ScheduledHours) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.Status = Status;
    this.WorkOrderSchedId = WorkOrderSchedId;
    this.ScheduledHours = ScheduledHours;
};
//#region Reschedule 
var SelectedScheduledHoursToSchedule = [];
var SelectedLookupIdToSchedule = [];
var SelectedStatusSchedule = [];
var SelectedWoIdToSchedule = [];
$(document).on('click', '#btnReschedule', function () {
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val(null).trigger("change.select2");
    var found = false;

    for (var i = 0; i < woToupdate.length; i++) {
        SelectedWoIdToSchedule.push(woToupdate[i].WorkOrderId);
        SelectedLookupIdToSchedule.push(woToupdate[i].ClientLookupId);
        SelectedStatusSchedule.push(woToupdate[i].Status);
        SelectedScheduledHoursToSchedule.push(woToupdate[i].ScheduledHours);
    }
    ShowbtnLoader("schedulesortmenu");
    HidebtnLoader("schedulesortmenu");
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val("").trigger("change.select2");
    $(document).find('#woRescheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
    $(document).find('#woRescheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);
    $(document).find('#woRescheduleModel_Status').val(SelectedStatusSchedule);
    $(document).find('#woRescheduleModel_ScheduledDurations').val(SelectedScheduledHoursToSchedule);
    $(document).find('.select2picker').select2({});
    addControls();
    $(document).find('#ScheduleModal').modal({ backdrop: 'static', keyboard: false, show: true });

    $(document).find('form').find("#Schedulestartdate").removeClass("input-validation-error");


});

$(document).on('click', '.btncancelmod', function () {

    var areaddescribedby = $(document).find("#ddlSchUser").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlSchUser").removeClass("input-validation-error");
    $(document).find('form').find("#Schedulestartdate").removeClass("input-validation-error");

});
function ReScheduleAddOnSuccess(data) {
    $('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("RescheduleUpdateSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            pageno = dtTable.page.info().page;
            dtTable.page(pageno).draw('page');
            $(document).find('#Schedulestartdate').val("").trigger('change');
            $('#ddlSchUser').val("").trigger("change.select2");
            SelectedScheduledHoursToSchedule = [];
            SelectedStatusSchedule = [];

        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);

    }
    $(document).find('.chksearch').prop('checked', false);
    $(document).find('.itemcount').text(0);
    woToupdate = [];
    WoCheckedItem = [];
    SelectedLookupIdToSchedule = [];
    SelectedWoIdToSchedule = [];
    $(document).find('#Schedulestartdate').val("").trigger('change');
    $('#ddlSchUser').val(null).trigger("change.select2");
    $(document).find('#ScheduleModal').modal('hide');
}
//#endregion

//#region Remove Schedule
$(document).on('click', '#btnRemove', function () {
    if (WoCheckedItem.length < 1) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        var found = false;
        for (var i = 0; i < WoCheckedItem.length; i++) {
            if (WoCheckedItem[i].Status === 'Scheduled') {
                found = true;
                break;
            }

        }
        if (found === false) {
            var errorMessage = getResourceValue("WorkOrderRemovescheduleScheduledAlert");
            ShowErrorAlert(errorMessage);
            return false;
        }
        else {
            for (var i = 0; i < WoCheckedItem.length; i++) {
                SelectedWoIdToSchedule.push(WoCheckedItem[i].WorkOrderId);
                SelectedLookupIdToSchedule.push(WoCheckedItem[i].WorkOrderId);
            }
            ShowbtnLoader("schedulesortmenu");
            HidebtnLoader("schedulesortmenu");
            $(document).find('#woRescheduleModel_WorkOrderIds').val(SelectedWoIdToSchedule);
            $(document).find('#woRescheduleModel_ClientLookupIds').val(SelectedLookupIdToSchedule);


            swal({
                title: getResourceValue("spnAreyousure"),
                text: getResourceValue("ConfirmRemoveScheduleAlert"),
                type: "warning",
                showCancelButton: true,
                closeOnConfirm: false,
                confirmButtonClass: "btn-sm btn-primary",
                cancelButtonClass: "btn-sm",
                confirmButtonText: getResourceValue("CancelAlertYes"),
                cancelButtonText: getResourceValue("CancelAlertNo")
            }, function () {
                ShowLoader();
                var jsonResult = {
                    "list": WoCheckedItem
                };
                $.ajax({
                    url: '/LaborScheduling/RemoveScheduleList',
                    type: "POST",
                    datatype: "json",
                    data: JSON.stringify(jsonResult),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        ShowLoader();
                    },
                    success: function (data) {
                        if (data.data == "success") {
                            SuccessAlertSetting.text = getResourceValue("spnWorkorderSuccessfullyRemovedScheduled");
                            swal(SuccessAlertSetting, function () {
                                pageno = dtTable.page.info().page;
                                dtTable.page(pageno).draw('page');
                            });
                        }
                        else {
                            GenericSweetAlertMethod(data.data);

                        }
                        $(".updateArea").hide();
                        $(".actionBar").fadeIn();
                        $(document).find('.chksearch').prop('checked', false);
                        $(document).find('.itemcount').text(0);
                        woToupdate = [];
                        WoCheckedItem = [];
                        SelectedLookupIdToSchedule = [];
                        SelectedWoIdToSchedule = [];

                    },
                    complete: function () {
                        CloseLoader();
                    },
                    error: function () {
                        CloseLoader();
                    }
                });
            });
        }
    }
});
//#endregion
//#region Reassign

var SelectedWoScheduleIds = '';
$(document).on('click', '#btnReassign', function () {
    $('#ddlReassUser').val(null).trigger("change.select2");
    for (var i = 0; i < WoCheckedItem.length; i++) {
        SelectedWoScheduleIds += WoCheckedItem[i].WorkOrderSchedId + ',';
    }
    $(document).find('#reassignModel_WorkOrderSchedIds').val(SelectedWoScheduleIds.trim(','));
    addControls();
    $(document).find('#ReassignModal').modal({ backdrop: 'static', keyboard: false, show: true });


});

$(document).on('click', '.btnAssigncancelmod', function () {

    var areaddescribedby = $(document).find("#ddlReassUser").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlReassUser").removeClass("input-validation-error");


});
function ReassignOnSuccess(data) {
    $('#ReassignModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("ReassignSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            $(".updateArea").hide();
            $(".actionBar").fadeIn();
            pageno = dtTable.page.info().page;
            dtTable.page(pageno).draw('page');
            $('#ddlReassUser').val("").trigger("change.select2");

        });
    }
    else if (data.NotAssignedWorkOrderClientLookupIdsList != null && data.NotAssignedWorkOrderClientLookupIdsList.length > 0) {
        var errormessage = data.NotAssignedWorkOrderClientLookupIdsList.length > 1 ? "WorkOrders" : "Workorder";
        CloseLoader();
        GenericSweetAlertMethod(errormessage + " " + data.NotAssignedWorkOrderClientLookupIdsList + " " + getResourceValue("WorkOrderScheduleReassignAlert"));
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
        pageno = dtTable.page.info().page;
        dtTable.page(pageno).draw('page');
        $('#ddlReassUser').val("").trigger("change.select2");
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);
    }
    $(document).find('.chksearch').prop('checked', false);
    $(document).find('.itemcount').text(0);
    woToupdate = [];
    WoCheckedItem = [];
    SelectedWoScheduleIds = '';
    $(document).find('#reassignModel_WorkOrderSchedIds').val('');
}
//#endregion
//#endregion

//#region New Search button


$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'ListLaborScheduling' },
        beforeSend: function () {
            ShowbtnLoader("SrchBttnNew");
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)" id= "mem_' + i + '"' + '><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
            $(document).find('#searchBttnNewDrop').show("slideToggle");
        },
        complete: function () {
            HidebtnLoader("SrchBttnNew");
        },
        error: function () {
            HidebtnLoader("SrchBttnNew");
        }
    });
});
function GenerateSearchList(txtSearchval, isClear) {
    run = true;
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'ListLaborScheduling', searchText: txtSearchval, isClear: isClear },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            var i; var str = '';
            for (i = 0; i < data.searchOptionList.length; i++) {
                str += '<li><a href="javascript:void(0)"><i class="fa fa-search" style="font-size: 1rem;position: relative;top: -1px;left: 0px;"></i> &nbsp;' + data.searchOptionList[i] + '</a></li>';
            }
            UlSearchList.innerHTML = str;
        }
        ,
        complete: function () {
            if (isClear == false) {
                dtTable.page('first').draw('page');
                CloseLoader();
            }
            else {
                CloseLoader();
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('keyup', '#txtColumnSearch', function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == 13) {
        TextSearch();
    }
    else {
        event.preventDefault();
    }
});
$(document).on('click', '.txtSearchClick', function () {
    TextSearch();
});
function TextSearch() {
    clearAdvanceSearch();
    //activeStatus = 0;
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#advsearchfilteritems").html(searchitemhtml);
    }
    else {
        run = true;
        generateListLaborSchedulingDataTable();
    }
    var container = $(document).find('#searchBttnNewDrop');
    container.hide("slideToggle");
}
$(document).on('click', '#UlSearchList li', function () {
    var v = LRTrim($(this).text());
    $(document).find('#txtColumnSearch').val(v);
    TextSearch();
});
$(document).on('click', '#cancelText', function () {
    $(document).find('#txtColumnSearch').val('');
});
$(document).on('click', '#clearText', function () {
    GenerateSearchList('', true);
});

//#endregion

//#endregion

//#region Available Work
$(document).on('click', '#btnAvailableWork', function () {
    LaborAvailableSelectedItemArray = [];
    AWOGridTotalGridItem = [];
    WorkOrderIds = [];
    $.ajax({
        url: "/LaborScheduling/AvailableWorkOrders",
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AvailableWork').html(data);
            $(document).find('#AvailableWorkModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetAdvSearchControl()
            GenerateAvailableLabor();
            CloseLoader();
            $("#ApproveWOadvsearchcontainer .sidebar").mCustomScrollbar({
                theme: "minimal"
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#region V2 - 984
function SetAdvSearchControl() {
    $("#sidebar2").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismissAW, .overlay').on('click', function () {
        $('#sidebar2').removeClass('active');
        $('.overlay2').fadeOut();
    });
    $(document).find('#AvailablesidebarCollapse').on('click', function () {
        $('#sidebar2').addClass('active');
        $('.overlay2').fadeIn();
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
//#region gridassign
$('#tblAvailGrid').on('mouseenter', '.ghover', function (e) {
    var rowData = LaborAvailabledt.row(this).data();
    var workOrderId = rowData.WorkOrderId;
    var thise = $(this);
    if (rowData.WorkAssigned_PersonnelId == -1) {
        if (LRTrim(thise.find('.tooltipassigngrid').text()).length > 0) {
            thise.find('.tooltipassigngrid').attr('style', 'display :block !important;');
            return;
        }
    }
    if (rowData.WorkAssigned_PersonnelId == -1) {
        $.ajax({
            "url": "/LaborScheduling/PopulateHover",
            "data": {
                workOrderId: workOrderId
            },
            "dataType": "json",
            "type": "POST",
            "beforeSend": function (data) {
                thise.find('.loadingImg').show();
            },
            "success": function (data) {
                if (data.personnelList != null) {
                    $('#' + workOrderId).text(data.personnelList);
                }
            },
            "complete": function () {
                thise.find('.loadingImg').hide();
                thise.find('.tooltipassigngrid').attr('style', 'display :block !important;');
            }
        });
    }
});

$('#tblAvailGrid').on('mouseleave', '.ghover', function (e) {
    $(this).find('.tooltipgrid').attr('style', 'display :none !important;');
});
//#endregion
//#endregion

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
            "url": "/LaborScheduling/GetAvailableWorkOrderMainGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.ClientLookupId = LRTrim($("#AWOWorkOrder").val());
                d.ChargeTo = LRTrim($("#AWOChargeTo").val());
                d.ChargeToName = LRTrim($("#AWOChargeToName").val());
                d.Description = LRTrim($("#AWODescription").val());
                d.Status = $("#AWOStatus").val();
                d.Priority = $('#AWOPriority').val();
                d.Type = $("#AWOWOType").val();
                d.RequiredDate = LRTrim($('#AWORequiredDate').val());
                d.Assigned = $("#AWOAssigned").val();
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
                    }
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
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '<span style="margin-left:8px"  class="m-badge m-badge--purple">' + full['PartsOnOrder'] + '</a></div>';
                        } else {
                            return '<div  class="width-100"><a class="lnk_workorder" style="text-decoration: none; cursor:default;color:black" href="javascript:void(0)">' + data + '</a></div>'
                        }
                    }
                },

                { "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "Assigned", "autoWidth": true, "bSearchable": true, "bSortable": true, "sClass": "ghover",
                    mRender: function (data, type, full, meta) {
                        if (full.WorkAssigned_PersonnelId == -1) {
                            return "<span>" + data + "</span><span class='tooltipassigngrid' id=" + full.WorkOrderId + "></span><span class='loadingImg' style='display:none !important;'><img src='/Images/lineLoader.gif' style='width:55px;height:auto;position:absolute;left:6px;top:30px;'></span>";

                        }
                        else {
                            return "<span>" + data + "</span>";
                        }
                    }
                },
                { "data": "Status", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Priority", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
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

$(document).on('click', '#AlreadyScheduledId', function (e) {
    LaborAvailabledt.state.clear();
    if (!this.checked) {
        GenerateAvailableLabor(0);
    }
    else {
        GenerateAvailableLabor(1);
    }
});
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
        url: "/LaborScheduling/GetLaborAvailable",
        async: true,
        type: "GET",
        datatype: "json",
        data: {
            flag:flag
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
function LaborAvailableSelectedItem(WorkOrderId, ClientLookupId, ChargeTo, ChargeToName, Description, Status, Priority, Type, RequiredDate) {
    this.WorkOrderId = WorkOrderId;
    this.ClientLookupId = ClientLookupId;
    this.ChargeTo = ChargeTo;
    this.ChargeToName = ChargeToName;
    this.Description = Description;
    this.Status = Status;
    this.Priority = Priority;
    this.Type = Type;
    this.RequiredDate = RequiredDate;
}

var SelectedLookupIdToAssigned = [];
var SelectedStatusAssigned = [];
var SelectedWoIdToAssigned = [];
$(document).on('click', '#btnLSAddAvailableWO', function (e) {
    var WorkOrderVals = WorkOrderIds;
    if (WorkOrderVals.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        for (var i = 0; i < LaborAvailableSelectedItemArray.length; i++) {
            SelectedWoIdToAssigned.push(LaborAvailableSelectedItemArray[i].WorkOrderId);
            SelectedLookupIdToAssigned.push(LaborAvailableSelectedItemArray[i].ClientLookupId);
            SelectedStatusAssigned.push(LaborAvailableSelectedItemArray[i].Status);
        }
        $(document).find('#Assignstartdate').val("").trigger('change');
        $('#ddlAssUser').val("").trigger("change.select2");
        $(document).find('#availableWorkAssignModel_WorkOrderIds').val(SelectedWoIdToAssigned);
        $(document).find('#availableWorkAssignModel_ClientLookupIds').val(SelectedLookupIdToAssigned);
        $(document).find('#availableWorkAssignModel_Status').val(SelectedStatusAssigned);
        $(document).find('.select2picker').select2({});
        $(document).find('#AvailableWorkAssignModal').modal({ backdrop: 'static', keyboard: false, show: true });
        SetControls();
        addControls();
        $(document).find('form').find("#Assignstartdate").removeClass("input-validation-error");
    }
});
//$('#AvailablesidebarCollapse').on('click', function () {
$(document).on("click", "#AvailablesidebarCollapse", function (e) {
    e.preventDefault();
    $('#ApproveWOadvsearchcontainer .sidebar').addClass('active');
    $('.overlay2').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    //addControl();
});
function SetControls() {
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
}
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
function AvailableWorkAddOnSuccess(data) {
    $('#ScheduleModal').modal('hide');
    if (data.data === "success") {
        SuccessAlertSetting.text = getResourceValue("AvailableWorkAssignSuccessAlert");
        swal(SuccessAlertSetting, function () {
            CloseLoader();
            LaborAvailabledt.page('first').draw('page');
            $(document).find('#Assignstartdate').val("").trigger('change');
            $('#ddlAssUser').val("").trigger("change.select2");
            dtTable.page('first').draw('page');
        });
    }
    else {
        CloseLoader();
        GenericSweetAlertMethod(data.data);

    }
    LaborAvailableSelectedItemArray = [];
    AWOGridTotalGridItem = [];
    SelectedLookupIdToAssigned = [];
    SelectedStatusAssigned = [];
    SelectedWoIdToAssigned = [];
    WorkOrderIds = [];
    if ($('#labavlidselectall').is(':checked'))
    {
        $('#labavlidselectall').prop('checked', false);
    }
    $('.chksearchAvl').prop('checked', false);
    $(document).find('#Assignstartdate').val("").trigger('change');
    $('#ddlAssUser').val(null).trigger("change.select2");
    $(document).find('#AvailableWorkAssignModal').modal('hide');

}
$(document).on('click', '.btnAsscancelmod', function () {

    var areaddescribedby = $(document).find("#ddlAssUser").attr('aria-describedby');
    if (typeof areaddescribedby !== 'undefined') {
        $('#' + areaddescribedby).hide();
    }
    $(document).find('form').find("#ddlAssUser").removeClass("input-validation-error");
    $(document).find('form').find("#Assignstartdate").removeClass("input-validation-error");

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
        //V2-984
        //if ($(this).val()) {
        //    hGridfilteritemcount++;
        //    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        //}
        if ([].concat($(this).val()).filter(function (valueOfFilter) { return valueOfFilter != ''; }).length) {
            hGridfilteritemcount++;
            var s = $(this).attr('id');
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritemsAWO').html(searchitemhtml);
    $('#ApproveWOadvsearchcontainer').find('.sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();
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
    $('.AWOfilteritemcount').text(hGridfilteritemcount);
}

$(document).on('click', '#liClearAdvSearchFilterAVlLabAWO', function () {
    var IsChecked = $(document).find('#AlreadyScheduledId').is(":checked");
    if (IsChecked) {
        flag = 1;
    }
    else {
        flag = 0;
    }
    clearAdvanceSearchAWO();
    LaborAvailabledt.page('first').draw('page');
});
function clearAdvanceSearchAWO() {
    var filteritemcount = 0;
    $('#advsearchsidebarAvailableLabor').find('input:text').val('');
    $('.AWOfilteritemcount').text(filteritemcount);
    $('#advsearchfilteritemsAWO').find('span').html('');
    $('#advsearchfilteritemsAWO').find('span').removeClass('tagTo');


}
$(document).on('click', '.btnCrossHistory', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('hide.bs.modal', '#AvailableWorkModal', function (event) {
    $('#AvailableWorkModal').empty();
    $(document).find('#AvailableWork').html('');
});
//#endregion
//#region Dropdown toggle   
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});


//#endregion
//#endregion

//#region Grouping Dropdown Change
$(document).on('change', '#GroupingLabor', function () {
    run = true;
    dtTable.page('first').draw('page');

});
//#endregion

//#region Estimated hour edit

var enterhit = 0;
var oldHour = 0;
var oldScheduledDate = '';
$(document).on('blur', '.grd-hours', function (event) {
    if (enterhit == 1)
        return;
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    //thstextbox.siblings('.is-saved-check').hide();

    if (thstextbox.val() == "") {
        thstextbox.val('0');
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValueHoursAlert"));
        return;
    }
    else if ($.isNumeric(thstextbox.val()) === false) {
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValidValueAlert"));
        return;
    }
    else if (thstextbox.val() > 999999.99) {
        thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("MaximumValue999999.99Alert"));
        return;
    }

    if (oldHour != $(this).val()) {
        $.ajax({
            url: '/LaborScheduling/UpdateWoHoursScheduleDate',
            data: {
                WorkOrderSchedId: data.WorkOrderScheduleId,
                hours: $(this).val(),
                ScheduleDate: data.ScheduledStartDate
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
                    WoHoursEditArray.push(dtTable.row(row).data().WorkOrderScheduleId);
                    pageno = dtTable.page.info().page;
                    dtTable.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldHour = 0;
            }
        });
    }
    else {
        oldHour = 0;
    }
});
$(document).on('keypress', '.grd-hours', function (event) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    //thstextbox.siblings('.is-saved-check').hide();
    if (keycode == '13') {
        enterhit = 1;
        if (thstextbox.val() == "") {
            thstextbox.val('0');
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValueHoursAlert"));
            return;
        }
        else if ($.isNumeric(thstextbox.val()) === false) {
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("EnterValidValueAlert"));
            return;
        }
        else if (thstextbox.val() > 999999.99) {
            thstextbox.siblings('.is-saved-times').show().attr('title', getResourceValue("MaximumValue999999.99Alert"));
            return;
        }
        thstextbox.blur();

        if (oldHour != $(this).val()) {
            $.ajax({
                url: '/LaborScheduling/UpdateWoHoursScheduleDate',
                data: {
                    WorkOrderSchedId: data.WorkOrderScheduleId,
                    hours: $(this).val(),
                    ScheduleDate: data.ScheduledStartDate
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
                        WoHoursEditArray.push(dtTable.row(row).data().WorkOrderScheduleId);
                        pageno = dtTable.page.info().page;
                        dtTable.page(pageno).draw('page');
                    }
                    else {
                        thstextbox.siblings('.is-saved-times').show();
                    }
                },
                complete: function () {
                    enterhit = 0;
                    oldHour = 0;
                }
            });
        }
        else {
            oldHour = 0;
        }
    }
    event.stopPropagation();
});
$(document).on('focus', '.grd-hours', function (event) {
    oldHour = $(this).val();
});
//#endregion

//#region Scheduled Date edit

$(document).on('change', '.Scheduleddtpicker', function (event) {
    var row = $(this).parents('tr');
    var data = dtTable.row(row).data();
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();
    //thstextbox.siblings('.is-saved-check').hide();

    if (oldScheduledDate != $(this).val()) {
        $.ajax({
            url: '/LaborScheduling/UpdateWoHoursScheduleDate',
            data: {
                WorkOrderSchedId: data.WorkOrderScheduleId,
                hours: data.ScheduledHours,
                ScheduleDate: $(this).val()
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
                    WoScheduleEditArray.push(dtTable.row(row).data().WorkOrderScheduleId);
                    pageno = dtTable.page.info().page;
                    dtTable.page(pageno).draw('page');

                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                enterScheduleDatehit = 0;
                oldScheduledDate = '';
            }
        });
    }
    else {
        oldScheduledDate = '';
    }
});
$(document).on('focus', '.Scheduleddtpicker', function () {
    oldScheduledDate = $(this).val();
});
//#endregion
$(document).on('change', '#ddlSchUser', function () {
    if ($(document).find("#ddlSchUser").val().length > 0 && $(document).find("#ddlSchUser").val().length > 0) {
        var areaddescribedby = $(document).find("#ddlSchUser").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#ddlSchUser").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#ddlSchUser").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }
    }
});
$(document).on('change', '#Schedulestartdate', function () {
    if ($(document).find("#Schedulestartdate").val().length > 0 && $(document).find("#Schedulestartdate").val().length > 0) {
        var areaddescribedby = $(document).find("#Schedulestartdate").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#Schedulestartdate").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#Schedulestartdate").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }
    }
});

$(document).on('change', '#ddlAssUser', function () {
    if ($(document).find("#ddlAssUser").val().length > 0 && $(document).find("#ddlAssUser").val().length > 0) {
        var areaddescribedby = $(document).find("#ddlAssUser").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#ddlAssUser").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#ddlAssUser").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }
    }
});
$(document).on('change', '#Assignstartdate', function () {
    if ($(document).find("#Assignstartdate").val().length > 0 && $(document).find("#Assignstartdate").val().length > 0) {
        var areaddescribedby = $(document).find("#Assignstartdate").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#Assignstartdate").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#Assignstartdate").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }
    }
});

//$('#dismissAW, .overlay2').on('click', function () {
$(document).on('click', '#dismissAW', function () {
    $(document).find('#ApproveWOadvsearchcontainer .sidebar').removeClass('active');
    $(document).find('.overlay2').fadeOut();

});

$(document).on('click', '#idListView', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/LaborScheduling/ListView';
    }

});
$(document).on('click', '#idCalendarView', function () {
    if (!$(this).hasClass('active')) {
        window.location = '/LaborScheduling/CalendarView';
    }
});

//#region V2-1102 Clear Button
$(document).on('click', '#btnClear', function () {
    clearAdvanceSearch();
    $('.select2-selection__clear').trigger('click');
    $(document).find("#GroupingLabor").val("0").trigger('change');
    $(document).find("#ScheduledDate").val("1").trigger('change');
});
//#endregion
