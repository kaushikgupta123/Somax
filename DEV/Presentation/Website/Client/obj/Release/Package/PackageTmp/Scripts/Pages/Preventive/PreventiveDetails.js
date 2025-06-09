//#region Schedule
var dtScheduleTable;
var exclusionDaysArr = [];
var exclusionDaysStringHidden = "";
var LoadScheduleGridState = false;
var ScheduleGridStateJsonString = "";
//#region V2-1212
var ChargeToClientLookupId;
var ChargeToName;
var Frequency;
var NextDueDate;
var WorkOrder_ClientLookupId;
var LastScheduled;
var LastPerformed;
var Meter_ClientLookupId;
var OnDemandGroup;
//#endregion
$(document).on('click', '.addscheduleBttn', function () {
    Addschedule();
});
$(document).on('click', '.editscheduleBttn', function () {
    LoadScheduleGridState = true;
    exclusionDaysArr = [];
    exclusionDaysStringHidden = "";
    var data = dtScheduleTable.row($(this).parents('tr')).data();
    var PrevMaintScheId = data.PrevMaintScheId;
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    var ScheduleType = $('#preventiveMaintenanceModel_ScheduleType').val();
    var PrevMaintLibraryID = $('#preventiveMaintenanceModel_PrevMaintLibraryId').val();
    if (ScheduleType == "Calendar") {
        var url = "/PreventiveMaintenance/EditScheduleRecordsDynamicCalendar";
    }
    else if (ScheduleType == "Meter") {
        var url = "/PreventiveMaintenance/EditScheduleRecordsDynamicMeter";
    }
    else if (ScheduleType == "OnDemand") {
        var url = "/PreventiveMaintenance/EditScheduleRecordsDynamicOnDemand";
    }
    $.ajax({
        url: url,
        type: "POST",
        dataType: 'html',
        data: { PrevMaintMasterID: prevMasterId, PrevMaintScheId: PrevMaintScheId, ClientLookupId: ClientLookupId, ScheduleType: ScheduleType, PrevMaintLibraryID: PrevMaintLibraryID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
            $("#imgChargeToTreeLineItem").show();
        },
        complete: function () {
            $(document).find(".multi-select2picker").select2({

            });
            SetPreventiveControls();
            $('#Frequency').on('blur', function () {
                if ($('#FrequencyType').val() == 'Days' && $('#Frequency').val() == '1') {
                    $('#exclusionDays').prop('disabled', false);
                    $('#exclusionDaysdynamic').prop('disabled', false);
                }
                else {
                    $("#ExclusionDaysString").val('').trigger('change');
                    $("#ExclusionDaysStringDynamic").val('').trigger('change');
                    $('#exclusionDays').prop('disabled', true);
                    $('#exclusionDaysdynamic').prop('disabled', true);
                }
            });
            exclusionDaysStringHidden = $('#ExclusionDaysStringHdn').val();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delscheduleBttn', function () {
    var data = dtScheduleTable.row($(this).parents('tr')).data();
    var PrevMaintScheId = data.PrevMaintScheId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteScheduleRecords',
            data: {
                PrevMaintScheId: PrevMaintScheId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("scheduledRecordDeleteSuccessAlert"));
                    dtScheduleTable.state.clear();
                }
            },
            complete: function () {
                GenerateScheduleGrid();
                CloseLoader();
            }
        });
    });
});
$(document).on('change', '#scheduleRecords_ChargeType', function () {
    var type = $(this).val();
    $(document).find('#txtChargeTo').val('');
    $(document).find('#scheduleRecords_ChargeToName').val('');
    selectedclientLookupId = "";
    if (type == "Equipment") {
        $('#scheduleRecords_ChargeToClientLookupId').css("width", "90%");
        $("#imgChargeToTreeLineItem").show();
    }
    else {
        $("#imgChargeToTreeLineItem").hide();
        $('#scheduleRecords_ChargeToClientLookupId').css("width", "100%");
    }
});
$(document).on('change', '#prevMaintReassignModel_PersonnelId', function () {
    $(this).removeClass("input-validation-error");
});
$(document).on('click', '#btnAddschedule', function () {
    selectedclientLookupId = "";
    Addschedule();
});
$(document).on('click', "#btnschedulescancel", function () {
    var prevMasterID = $(document).find('#scheduleRecords_PrevMaintMasterId').val();
    RedirectToDetailOncancel(prevMasterID, "schedule");
});
$(document).on('click', "#brdschrecords", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);
});
function Addschedule() {
    exclusionDaysArr = [];
    exclusionDaysStringHidden = "";
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    var ScheduleType = $('#preventiveMaintenanceModel_ScheduleType').val();
    var PrevMaintLibraryID = $('#preventiveMaintenanceModel_PrevMaintLibraryId').val();
    if (ScheduleType == "Calendar") {
        var url = "/PreventiveMaintenance/AddPMSRecordsDynamic_Calendar";
    }
    else if (ScheduleType == "Meter") {
        var url = "/PreventiveMaintenance/AddPMSRecordsDynamic_Meter";
    }
    else if (ScheduleType == "OnDemand") {
        var url = "/PreventiveMaintenance/AddPMSRecordsDynamic_OnDemand";
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: 'html',
        data: { PrevMaintMasterID: prevMasterId, ClientLookupId: ClientLookupId, ScheduleType: ScheduleType, PrevMaintLibraryID: PrevMaintLibraryID },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            $('#Frequency').on('blur', function () {
                if ($('#FrequencyType').val() == 'Days' && $('#Frequency').val() == '1') {
                    $('#exclusionDays').prop('disabled', false);
                    $('#exclusionDaysdynamic').prop('disabled', false);
                }
                else {
                    $('#exclusionDays').prop('disabled', true);
                    $('#exclusionDaysdynamic').prop('disabled', true);
                }
            });
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function GenerateScheduleGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Schedule;
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ScheduleType = $('#preventiveMaintenanceModel_ScheduleType').val();
    if ($(document).find('#tblScheduleRecords').hasClass('dataTable')) {
        dtScheduleTable.destroy();
    }
    dtScheduleTable = $("#tblScheduleRecords").DataTable({
        colReorder: false,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            ScheduleGridStateJsonString = JSON.stringify(data);
        },
        stateLoadCallback: function () {
            if (LoadScheduleGridState == true && ScheduleGridStateJsonString.length > 0) {
                LoadScheduleGridState = false;
                return JSON.parse(ScheduleGridStateJsonString);
            }
            return "";
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenance/PopulateScheduleRecords",
            "type": "POST",
            "datatype": "json",

            data: function (d) {
                d.PrevMasterID = prevMasterId;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
                d.ChargeToName = ChargeToName;
                d.Frequency = Frequency;
                d.NextDueDate = NextDueDate;
                d.WorkOrder_ClientLookupId = WorkOrder_ClientLookupId;
                d.LastScheduled = LastScheduled;
                d.LastPerformed = LastPerformed;
                d.Meter_ClientLookupId = Meter_ClientLookupId;
                d.OnDemandGroup = OnDemandGroup;
            },
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            },
            global: true
        },
        columnDefs: [
            {
                "data": null,
                targets: [10], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        // Mod V2-1161
                        // If PlanningRequired is false, an additional Assignments button is shown.
                        if (data.PlanningRequired == true && prventivemaintancesecurity_PlannerRequiredForUsePlanning == "True") {
                            return '<button type="button" class="btn btn-blue tblscheduleactionbtns" style="border:0;outline:0"><strong>...</strong></button>' +
                                '<div id="' + data.PrevMaintScheId + '" class="actionbtndiv ' + data.PrevMaintScheId + '" style="display:none;">' +
                                '<a class="btn btn-blue addscheduleBttn gridinnerbutton" style="margin-left: 0;" title="Add">Add</a>' +
                                '<a class="btn btn-blue editscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Edit">Edit</a>' +
                                '<a class="btn btn-blue delscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Delete">Delete</a>' +
                                '</div >' +
                                '<div class="maskaction ' + data.PrevMaintScheId + '" data-id="' + data.PrevMaintScheId + '"  onclick="funcCloseActionbtn()"></div>'
                        }
                        else {
                            return '<button type="button" class="btn btn-blue tblscheduleactionbtns" style="border:0;outline:0"><strong>...</strong></button>' +
                                '<div id="' + data.PrevMaintScheId + '" class="actionbtndiv ' + data.PrevMaintScheId + '" style="display:none;">' +
                                '<a class="btn btn-blue addscheduleBttn gridinnerbutton" style="margin-left: 0;" title="Add">Add</a>' +
                                '<a class="btn btn-blue editscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Edit">Edit</a>' +
                                '<a class="btn btn-blue delscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Delete">Delete</a>' +
                                '<a class="btn btn-blue assignmentBttn gridinnerbutton" style="margin-left: 0;" title= "Assignments">Assignments</a>' +
                                '</div >' +
                                '<div class="maskaction ' + data.PrevMaintScheId + '" data-id="' + data.PrevMaintScheId + '"  onclick="funcCloseActionbtn()"></div>'
                        }
                        //V2-712                       
                        return '<button type="button" class="btn btn-blue tblscheduleactionbtns" style="border:0;outline:0"><strong>...</strong></button>' +
                            '<div id="' + data.PrevMaintScheId + '" class="actionbtndiv ' + data.PrevMaintScheId + '" style="display:none;">' +
                            '<a class="btn btn-blue addscheduleBttn gridinnerbutton" style="margin-left: 0;" title="Add">Add</a>' +
                            '<a class="btn btn-blue editscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Edit">Edit</a>' +
                            '<a class="btn btn-blue delscheduleBttn gridinnerbutton" style="margin-left: 0;" title= "Delete">Delete</a>' +
                            '<a class="btn btn-blue assignmentBttn gridinnerbutton" style="margin-left: 0;" title= "Assignments">Assignments '+data.PlanningRequired+'</a>' +
                            '</div >' +
                            '<div class="maskaction ' + data.PrevMaintScheId + '" data-id="' + data.PrevMaintScheId + '"  onclick="funcCloseActionbtn()"></div>'
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                {
                    "data": "PrevMaintScheId",
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
                {
                    "data": "ChargeToClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true

                },
                { "data": "ChargeToName", "autoWidth": false, "bSearchable": true, "bSortable": true }, 
                //V2-848 start               
                {
                    "data": "Frequency",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return row.Frequency + ' ' + row.FrequencyType;
                    }
                },
                { "data": "NextDueDate", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "WorkOrder_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "LastScheduled", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "LastPerformed", "autoWidth": false, "bSearchable": true, "bSortable": true },

                { "data": "Meter_ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "OnDemandGroup", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center click-open" }
            ],
        initComplete: function () {           
            var column = this.api().column(10);
            var freCol = this.api().column(3);
            var DDCol = this.api().column(4);
            var metCol = this.api().column(8);
            var OnDemandCol = this.api().column(9)
            if (visibility == "False") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddschedule").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddschedule").show();
                }
                else {
                    $("#btnAddschedule").hide();
                }
            }
            if (ScheduleType != "Calendar") {
                freCol.visible(false);
                DDCol.visible(false);
            }
            else {
                freCol.visible(true);
                DDCol.visible(true);
            }
            if (ScheduleType != "Meter") {
                metCol.visible(false);
            }
            else {
                metCol.visible(true);
            }
            if (ScheduleType != "OnDemand") {
                OnDemandCol.visible(false);
            }
            else {
                OnDemandCol.visible(true);
            }
            $(document).find('#tblScheduleRecordsfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            //#region V2-1212
            $('#tblScheduleRecords tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblScheduleRecords thead th').eq($(this).index()).text();
                var colid = $(this).attr('data-id');
                if (colid == "colindex_4" || colid == "colindex_6" || colid == "colindex_7") {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt dtpicker" id=' + colid + ' /><i class="fa fa-search dropSearchIcon"></i>');
                }
                else if (title != "" && title != "Action") {
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id=' + colid + ' /><i class="fa fa-search dropSearchIcon"></i>');
                }
                if (ChargeToClientLookupId) { $('#colindex_1').val(ChargeToClientLookupId); }
                if (ChargeToName) { $('#colindex_2').val(ChargeToName); }
                if (Frequency) { $('#colindex_3').val(Frequency); }
                if (NextDueDate) { $('#colindex_4').val(NextDueDate); }
                if (WorkOrder_ClientLookupId) { $('#colindex_5').val(WorkOrder_ClientLookupId); }
                if (LastScheduled) { $('#colindex_6').val(LastScheduled); }
                if (LastPerformed) { $('#colindex_7').val(LastPerformed); }
                if (Meter_ClientLookupId) { $('#colindex_8').val(Meter_ClientLookupId); }
                if (OnDemandGroup) { $('#colindex_9').val(OnDemandGroup); }
            });

            $('#tblScheduleRecords tfoot th').find('.tfootsearchtxt').on("change", function (e) {
                var thisId = $(this).attr('id');
                var colIdx = thisId.split('_')[1];
                var searchText = LRTrim($(this).val());
                ChargeToClientLookupId = $('#colindex_1').val();
                ChargeToName = $('#colindex_2').val();
                Frequency = $('#colindex_3').val();
                NextDueDate = $('#colindex_4').val();
                WorkOrder_ClientLookupId = $('#colindex_5').val();
                LastScheduled = $('#colindex_6').val();
                LastPerformed = $('#colindex_7').val();
                Meter_ClientLookupId = $('#colindex_8').val();
                OnDemandGroup = $('#colindex_9').val();
                dtScheduleTable.page('first').draw('page');
            });

            $(document).find('.dtpicker').datepicker({
                changeMonth: true,
                changeYear: true,
                "dateFormat": "mm/dd/yy",
                autoclose: true
            }).inputmask('mm/dd/yyyy');
            //#endregion
        }
    });
}
$(document).on('change', '#PlanningRequired', function () {
    if ($('#IsPlanner_PersonnelIdRequired').val() =='False') {
    var textBox = $('#Planner_PersonnelId'); // Targeting the input by ID
    var label = $('span.label:contains("Planner")'); // Finds the label containing 'Planner'
    if ($(this).is(':checked')) {
        textBox.addClass('required');
        textBox.attr('title', getResourceValue("PlanningRequiredAlert")); // Set the tooltip

        // Change label color to red
        label.css({
            'color': 'red',
            'font-weight': 'bold'
        });
    } else {
        textBox.removeClass('required');
        textBox.removeAttr('title'); // Remove the tooltip

        // Reset label color
         label.css({
            'color': '',
            'font-weight': ''
        });
    }
    // Re-trigger validation to apply changes
        textBox.valid();
    }
});
function ScheduleRecordsAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ScheduleAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ScheduleUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.prevemaintmasterid, "schedule");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#openpmschedulechargetogrid", function () {
    var textChargeToId = $("#scheduleRecords_ChargeType option:selected").val();
    if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    else if (textChargeToId == "Location") { generateLocationDataTable(); }
});

//#region Modal Tree
$(document).on('click', '#imgChargeToTreeLineItem', function (e) {
    $(this).blur();
    generateWoEquipmentTree(-1);
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
            $('#prevEdipTreeModal').modal('show');
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            $(document).find('.radSelectWo').each(function () {


                if ($(document).find('#hdnChargeTo').val() == '0' || $(document).find('#hdnChargeTo').val() == '') {

                    if ($(this).data('clientlookupid') === selectedclientLookupId) {
                        $(this).attr('checked', true);
                    }

                }
                else {

                    if ($(this).data('clientlookupid') == $(document).find('#hdnChargeTo').val() || $(this).data('equipmentid') == $(document).find('#hdnChargeTo').val()) {
                        $(this).attr('checked', true);
                    }

                }

            });
            //-- V2-518 collapse all element
            // looking for the collapse icon and triggered click to collapse
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAHFJREFUeNpi/P//PwMlgImBQsA44C6gvhfa29v3MzAwOODRc6CystIRbxi0t7fjDJjKykpGYrwwi1hxnLHQ3t7+jIGBQRJJ6HllZaUUKYEYRYBPOB0gBShKwKGA////48VtbW3/8clTnBIH3gCKkzJgAGvBX0dDm0sCAAAAAElFTkSuQmCC"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

$(document).on('change', '.radSelect', function () {
    var clientLookupId = $(this).data('clientlookupid');

    $('#prevEdipTreeModal').modal('hide');
    $(document).find('#txtChargeTo').val(clientLookupId);

});
var selectedclientLookupId = "";
$(document).on('change', '.radSelectWo', function () {
    selectedclientLookupId = $(this).data('clientlookupid');

    $('#prevEdipTreeModal').modal('hide');
    $(document).find('#txtChargeTo').val(selectedclientLookupId);
    $(document).find('#txtChargeTo').removeClass('input-validation-error');
    var chargetoname = $(this).data('itemname');
    chargetoname = chargetoname.substring(0, chargetoname.length - 1);
    $(document).find('#scheduleRecords_ChargeToName').val(chargetoname);
    $(document).find('#hdnChargeTo').val('0');
});
//#endregion

//#endregion
//#region Tasks
var dtTaskTable;
var LoadTaskGridState = false;
var TaskGridStateJsonString = "";
$(document).on('click', '.addTaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    AddPmTask();
});
$(document).on('click', '.editTaskBttn', function () {
    LoadTaskGridState = true;
    var data = dtTaskTable.row($(this).parents('tr')).data();
    EditPmTask(data.PrevMaintTaskId);
});
$(document).on('click', '.delTaskBttn', function () {
    var data = dtTaskTable.row($(this).parents('tr')).data();
    DeletePmTask(data.PrevMaintTaskId);
});
$(document).on('click', "#brdpmtask", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);
});
function generateTasksGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_Task;
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var prevMaintLibraryId = $(document).find('#preventiveMaintenanceModel_PrevMaintLibraryId').val();
    if ($(document).find('#tblTasks').hasClass('dataTable')) {
        dtTaskTable.destroy();
    }
    dtTaskTable = $("#tblTasks").DataTable({
        colReorder: false,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            TaskGridStateJsonString = JSON.stringify(data);
        },
        stateLoadCallback: function () {
            if (LoadTaskGridState == true && TaskGridStateJsonString.length > 0) {
                LoadTaskGridState = false;
                return JSON.parse(TaskGridStateJsonString);
            }
            return "";
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenance/PopulateTasks",
            data: function (d) {
                d.PrevMasterID = prevMasterId;
                d.prevMaintLibraryId = prevMaintLibraryId;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [4], render: function (a, b, data, d) {                   
                    if (prevMaintLibraryId !== '0') {  
                        return "";
                    }
                    else {
                        return '<a class="btn btn-outline-primary addTaskBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editTaskBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delTaskBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "TaskNumber", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-150'>" + data + "</div>";
                    }
                },
                { "data": "ChargeTypeText", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False" || prevMaintLibraryId > 0) {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddtask").hide();
            }
            else {
                if (visibility == "True" && prevMaintLibraryId < 1) {
                    $("#btnAddtask").show();
                }
                else {
                    $("#btnAddtask").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnAddtask", function () {
    AddPmTask();
});
function AddPmTask() {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/AddPMTasks",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function PmTaskAddOnSuccess(data) {
    CloseLoader();
    if (data.data == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("TaskAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("TaskUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.pmid, "tasks");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
function EditPmTask(taskid) {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditTasks",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, _taskId: taskid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeletePmTask(taskId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteTasks',
            data: {
                taskNumber: taskId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("taskDeleteSuccessAlert"));
                    dtTaskTable.state.clear();
                }
            },
            complete: function () {
                generateTasksGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnpmtaskcancel", function () {
    var prevMasterID = $(document).find('#prevMaintTaskModel_PrevMaintMasterId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(prevMasterID, "tasks");
    });
});
$(document).on('change', '#prevMaintTaskModel_ChargeType', function () {
    $(document).find('#txtChargeTo').val('');

});
$(document).on('click', "#openpmtaskchargetogrid", function () {
    var textChargeToId = $("#prevMaintTaskModel_ChargeType option:selected").val();
    if (textChargeToId == "Equipment") { generateEquipmentDataTable(); }
    else if (textChargeToId == "Location") { generateLocationDataTable(); }
});
//#endregion
//#region Notes
var pmNotesTable;
$(document).on('click', '.editNoteBttn', function () {
    var data = pmNotesTable.row($(this).parents('tr')).data();
    EditPmNote(data.NotesId);
});
$(document).on('click', '.delNoteBttn', function () {
    var data = pmNotesTable.row($(this).parents('tr')).data();
    DeletePmNote(data.NotesId);
});
$(document).on('click', "#brdpmnotes", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);   
});
function generateNotesGrid() {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var visibility = prventivemaintancesecurity_Notes;
    if ($(document).find('#tblPMNotes').hasClass('dataTable')) {
        pmNotesTable.destroy();
    }
    pmNotesTable = $("#tblPMNotes").DataTable({
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
            "url": "/PreventiveMaintenance/PopulateNotes?PrevMasterID=" + prevMasterID,
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a class="btn btn-outline-success editNoteBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delNoteBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }


                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ModifiedDate", "type": "date " },
                {
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            SetPageLengthMenu();
            if (visibility == "True") {
                $("#btnAddpmNote").show();
                var column = this.api().column(3);
                column.visible(true);
            }
            else {
                $("#btnAddpmNote").hide();
                var column = this.api().column(3);
                column.visible(false);
            }
        }
    });
}
$(document).on('click', "#btnAddpmNote", function () {
    AddPMNotes();
});
$(document).on('click', "#brdnotes", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToDetailOncancel(prevMasterID, "notes");
});
function AddPMNotes() {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();

    $.ajax({
        url: "/PreventiveMaintenance/AddPMNotes",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function PMNotesAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddNoteAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateNoteAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.PrevMasterID, "notes");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditPmNote(notesid) {
    var pmid = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditNotes",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: pmid, _notesId: notesid, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeletePmNote(notesId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteNotes',
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
                    pmNotesTable.state.clear();
                    ShowDeleteAlert(getResourceValue("noteDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateNotesGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnpmnotescancel", function () {
    var prevMasterID = $(document).find('#notesModel_PrevMasterID').val();
    RedirectToPmDetail(prevMasterID, "notes");
});
//#endregion
//#region Attachments
var dtpmAttachmentTable;
function generateAttachmentsGrid() {
    var attchCount = 0;
    var visibility;
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if ($(document).find('#tblPMAttachments').hasClass('dataTable')) {
        dtpmAttachmentTable.destroy();
    }
    dtpmAttachmentTable = $("#tblPMAttachments").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
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
            "url": "/PreventiveMaintenance/PopulateAttachments?prevMasterID=" + prevMasterID,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                visibility = response.Attachsecurity;
                if (attchCount > 0) {
                    $(document).find('#prevAttachmentCount').show();
                    $(document).find('#prevAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#prevAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-success editPmAttchBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delPmAttachment gridinnerbutton" title = "Delete" > <i class="fa fa-trash"></i></a> ';
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="' + '/PreventiveMaintenance/DowloadPmAttachment?_fileinfoId=' + row.FileInfoId + '"  target="_blank">' + row.FullName + '</a>'
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                /*V2-949*/
                {
                    "data": "PrintwithForm", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center",
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
                    "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (visibility == false) {
                $("#btnAddpmAttachment").hide();
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                $("#btnAddpmAttachment").show();
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtpmAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/PreventiveMaintenance/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
$(document).on('click', "#btnAddpmAttachment", function () {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/AddAttachments",
        type: "GET",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function AttachmentPmAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("attachmentUpdateSuccessAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.pmid, "attachments");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', ".delPmAttachment", function () {
    var data = dtpmAttachmentTable.row($(this).parents('tr')).data();
    var FileAttachmentId = data.FileAttachmentId;
    DeletePmAttachment(FileAttachmentId);
});
function DeletePmAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeletePMAttachments',
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
                    dtpmAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentsGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnpmattachmentcancel", function () {
    var pmId = $(document).find('#attachmentModel_PrevMaintMasterId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(pmId, "attachments");
    });
});
$(document).on('click', "#brdpmattachment", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID, "attachments");

});
//#region V2-1208
$(document).on('change', '#attachmentModel_FileContent', function () {
    var val = $(this).val();
    var fileName = val.replace(/^.*[\\\/]/, '');
    var fileExt = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt == 'PDF' || fileExt == 'pdf') {
        $('#attachmentModel_PrintwithForm').removeAttr('disabled');
    }
    else {
        $('#attachmentModel_PrintwithForm').prop('checked', false);
        $('#attachmentModel_PrintwithForm').attr("disabled", true);
    }
});
//#endregion
//#region V2-949
$(document).on('click', '.editPmAttchBttn', function () {
    var data = dtpmAttachmentTable.row($(this).parents('tr')).data();
    EditPMAttachment(data.FileAttachmentId);
});
function EditPMAttachment(FileAttachmentId) {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditAttachment",
        type: "GET",
        dataType: 'html',
        data: { FileAttachmentId: FileAttachmentId, prevMasterID: prevMasterID, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
//#endregion
//#endregion
//#region EstimatesPart
$(document).on('click', "#openpartidgrid", function () {
    generatePrevPartsXrefDataTable();
});
var dtEstimatesPartTable;
//$(document).on('click', '#btnAddEstimatedPart', function () {
//    AddEstimatePart();
//});
$(document).on('click', "#btnspartcancel", function () {
    var prevMasterID = $(document).find('#estimatePart_PrevMaintMasterId').val();
    RedirectToDetailOncancel(prevMasterID, "estimatesPart");
});
$(document).on('click', "#brdestimateparts", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);
});
//$(document).on('click', '.addEstimatesPartBttn', function () {
//    AddEstimatePart();
//});
$(document).on('click', '.editEstimatesPartBttn', function () {
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    var CategoryId = data.CategoryId;
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditEstimatesPart",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintMasterID: prevMasterId, EstimatedCostsId: EstimatedCostsId, ClientLookupId: ClientLookupId, CategoryId: CategoryId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEstimatedParts').hide();
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            $.validator.setDefaults({ ignore: null });
            $.validator.unobtrusive.parse(document);
            $('input, form').blur(function () {
                $(this).valid();
            });
            $(document).find('.select2picker').select2({}).attr('readonly', true);
            $(document).find('.dtpicker').datepicker({
                "dateFormat": "mm/dd/yy",
                autoclose: true
            });
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delEstimatesPartBttn', function () {
    var data = dtEstimatesPartTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteEstimatesPart',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("MaterialRequestDeletedAlert"));
                    dtEstimatesPartTable.state.clear();
                }
            },
            complete: function () {
                generateEstimatesPartGrid();
                CloseLoader();
            }
        });
    });
});
function AddEstimatePart() {
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/AddEstimatesPMPart",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintMasterID: prevMasterId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function generateEstimatesPartGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_EstimatePart;
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if ($(document).find('#tblEstimatesPart').hasClass('dataTable')) {
        dtEstimatesPartTable.destroy();
    }
    dtEstimatesPartTable = $("#tblEstimatesPart").DataTable({
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
            "url": "/PreventiveMaintenance/PopulateEstimatesPart?PrevMasterID=" + prevMasterId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a data-toggle="modal" data-target="#AddEstimatedParts" class="btn btn-outline-primary addEstimatesPartBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editEstimatesPartBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delEstimatesPartBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddEstimatedPart").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddEstimatedPart").show();
                }
                else {
                    $("#btnAddEstimatedPart").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function EstimapePartAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("PartAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("PartUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.prevemaintmasterid, "estimatesPart");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#region V2-1151 Material Request Support
//#region Add PartInInventory

$(document).on('click', "#selectidpartininventory", function (e) {
    e.preventDefault();
    $('.modal-backdrop').remove();
    if ($(document).find('#IsUseMultiStoreroom').val()=="True") {
        PopulateStorerooms();
    }
    else {
        GoToAddPartInInventory();
    }
});
function PopulateStorerooms() {
    $(document).find('#AddEstimatedParts').modal('hide');
    $.ajax({
        url: "/PreventiveMaintenance/PopulateStorerooms",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#StoreroomListPopUp').html(data);
            $('#StoreroomListModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            SetPreventiveControls();
            CloseLoader();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function SelctStoreroomOnSuccess(data) {
    $('.modal-backdrop').remove();
    if (data.data === "success") {
        $(document).find('#StoreroomListModalpopup').hide();
        GoToAddPartInInventory();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click','#btnSubmitStoreroomcancel',function () {
    var areaChargeToId = "";
    $(document).find('#StoreroomListModalpopup select').each(function (i, item) {
        areaChargeToId = $(document).find("#" + item.getAttribute('id')).attr('aria-describedby');
        $('#' + areaChargeToId).hide();
    });
});
function GoToAddPartInInventory() {
    var PreventiveMaintainId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var PMClientLookupId = $(document).find('#preventiveMaintenanceModel_ClientLookupId').val();
    var StoreroomId = 0;
    if ($(document).find('#StoreroomId').val() != "undefined" && $(document).find('#StoreroomId').val() > 0) {
        StoreroomId = $(document).find('#StoreroomId').val();
    }
    $.ajax({
        url: "/PreventiveMaintenance/AddPartInInventory",
        type: "POST",
        dataType: "html",
        data: { PreventiveMaintainId: PreventiveMaintainId, PMClientLookupId: PMClientLookupId,StoreroomId: StoreroomId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
            SetFixedHeadStyle();
        },
        complete: function () {
            CloseLoader();
            ShowCardViewWO();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click','#sidebarCollapseMaterialRequest', function () {
    $('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
//#endregion
//#region Add PartNotInInventory
$(document).on('click', "#selectidpartnotininventory", function (e) {
    $('.modal-backdrop').remove();
    AddMRPartNotInInventory();
});

function AddMRPartNotInInventory() {
    var preventiveMaintId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var clientLookupId = $(document).find('#preventiveMaintenanceModel_ClientLookupId').val();
    
    $.ajax({
        url: "/PreventiveMaintenance/AddMaterialRequestPartNotInInventory",
        type: "GET",
        dataType: 'html',
        data: { PreventiveMaintId: preventiveMaintId, ClientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddEstimatedParts').hide();
            $('#PartNotInInventoryPopUp').html(data);
            $('#AddPartNotInInventoryModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            CloseLoader();
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function MRPartNotInInventoryAddOnSuccess(data) {
    var pmId = data.pmId;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(pmId, "estimatesPart")
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);

    }
}
//#endregion
function EditMaterialRequestOnSuccess(data) {
    CloseLoader();
    var pmId = data.prevMaintId;
    if (data.Result == "success") {
        $(document).find('#AddPartNotInInventoryModalpopup').modal("hide");
        SuccessAlertSetting.text = getResourceValue("MaterialRequestUpdatedAlert");
        swal(SuccessAlertSetting, function () {
            $('.modal-backdrop').remove();
            RedirectToPmDetail(pmId, "estimatesPart");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#endregion
//#region EstimatesLabor
var dtEstimatesLaborTable;
$(document).on('click', "#btnslaborcancel", function () {
    var prevMasterID = $(document).find('#estimateLabor_PrevMaintMasterId').val();
    RedirectToDetailOncancel(prevMasterID, "estimatesLabor");
});
$(document).on('click', '#btnAddpEstimatesLabor', function () {
    AddEstimateLabor();
});
$(document).on('click', '.addEstimatesLaborBttn', function () {
    AddEstimateLabor();
});
$(document).on('click', "#brdestimatelabor", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);    
});
$(document).on('click', '.editEstimatesLaborBttn', function () {
    var data = dtEstimatesLaborTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditEstimatesLabor",
        type: "POST",
        dataType: 'html',
        data: { PrevMaintMasterID: prevMasterId, EstimatedCostsId: EstimatedCostsId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {

            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.delEstimatesLaborBttn', function () {
    var data = dtEstimatesLaborTable.row($(this).parents('tr')).data();
    var EstimatedCostsId = data.EstimatedCostsId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteEstimatesLabor',
            data: {
                EstimatedCostsId: EstimatedCostsId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("laborDeleteSuccessAlert"));
                    dtEstimatesLaborTable.state.clear();
                }
            },
            complete: function () {
                generateEstimatesLaborGrid();
                CloseLoader();
            }
        });
    });
});
function AddEstimateLabor() {
    var prevMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/AddEstimatesPMLabor",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterId, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function EstimapeLaborAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LaborAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LaborUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.prevemaintmasterid, "estimatesLabor");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function generateEstimatesLaborGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_EstimateLabor;
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if ($(document).find('#tblEstimatesLabor').hasClass('dataTable')) {
        dtEstimatesLaborTable.destroy();
    }
    dtEstimatesLaborTable = $("#tblEstimatesLabor").DataTable({
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
            "url": "/PreventiveMaintenance/PopulateEstimatesLabor?PrevMasterID=" + prevMasterId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a class="btn btn-outline-primary addEstimatesLaborBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editEstimatesLaborBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delEstimatesLaborBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Duration", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == "False") {
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                var column = this.api().column(5);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpEstimatesLabor").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddpEstimatesLabor").show();
                }
                else {
                    $("#btnAddpEstimatesLabor").hide();
                }
            }
            SetPageLengthMenu();
        }
    });

}
//#endregion
//#region EstimatesOther
var dtEstimatesOtherTable;
var vendorClientLookupId;
$(document).on('click', '.addEstimatesOthBttn', function () {
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    AddPmEstOth();
});
$(document).on('click', '.editEstimatesOthBttn', function () {
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    vendorClientLookupId = data.VendorClientLookupId;
    EditPmEstOth(data.EstimatedCostsId);
});
$(document).on('click', '.delEstimatesOthBttn', function () {
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    DeletePmEstOth(data.EstimatedCostsId);
});
$(document).on('click', '#btnAddpEstimatesOther', function () {
    var data = dtEstimatesOtherTable.row($(this).parents('tr')).data();
    AddPmEstOth();
});
$(document).on('click', "#brdestimateother", function () {
    var prevMasterID = $(this).attr('data-val');
    RedirectToPmDetail(prevMasterID);    
});
function generateEstimatesOtherGrid() {
    var rCount = 0;
    var visibility = prventivemaintancesecurity_EstimateOther;
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if ($(document).find('#tblEstimatesOther').hasClass('dataTable')) {
        dtEstimatesOtherTable.destroy();
    }
    dtEstimatesOtherTable = $("#tblEstimatesOther").DataTable({
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
            "url": "/PreventiveMaintenance/PopulateEstimatesOther?PrevMasterID=" + prevMasterId,
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        columnDefs: [
            {
                "data": null,
                targets: [6], render: function (a, b, data, d) {
                    if (visibility == "True") {
                        return '<a class="btn btn-outline-primary addEstimatesOthBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editEstimatesOthBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delEstimatesOthBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "Source", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "VendorClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": false, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "UnitCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "Quantity", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "TotalCost", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            var column;
            column = this.api().column(1);
            if (UseVendorMaster == "True") {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (visibility == "False") {
                column = this.api().column(5);
                column.visible(false);
            }
            else {
                column = this.api().column(5);
                column.visible(true);
            }
            if (rCount > 0) {
                $("#btnAddpEstimatesOther").hide();
            }
            else {
                if (visibility == "True") {
                    $("#btnAddpEstimatesOther").show();
                }
                else {
                    $("#btnAddpEstimatesOther").hide();
                }
            }
            SetPageLengthMenu();
        }
    });
}
function AddPmEstOth() {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/AddEstimatesPMOther",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, ClientLookupId: ClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}
function PmEstOtherAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("EstimateOtherAddAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("EstimateOtherUpdateAlert");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(data.pmid, "estimatesOther");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}
function EditPmEstOth(costid) {
    var prevMasterID = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var ClientLookupId = $('#preventiveMaintenanceModel_ClientLookupId').val();
    $.ajax({
        url: "/PreventiveMaintenance/EditEstimatesOther",
        type: "POST",
        dataType: 'html',
        data: { PrevMasterID: prevMasterID, CostId: costid, ClientLookupId: ClientLookupId, VendorClientLookupId: vendorClientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            SetPreventiveControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function DeletePmEstOth(costId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeleteEstimatesOther',
            data: {
                estimatedCostId: costId
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("otherDeleteSuccessAlert"));
                    dtEstimatesOtherTable.state.clear();
                }
            },
            complete: function () {
                generateEstimatesOtherGrid();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnpmestothercancel", function () {
    var prevMasterID = $(document).find('#estimateOtherModel_PrevMaintMasterId').val();
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(prevMasterID, "estimatesOther");
    });
});
$(document).on('click', '#estvopengrid', function () {
    generatePreVendorDataTable();
});
//#endregion
//#region EstimatesSummery
var dtEstimateSummeryTable;
function generateEstimatesSummeryGrid() {
    var prevMasterId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    if ($(document).find('#tblEstimateSummery').hasClass('dataTable')) {
        dtEstimateSummeryTable.destroy();
    }
    dtEstimateSummeryTable = $("#tblEstimateSummery").DataTable({
        colReorder: true,
        rowGrouping: true,
        paging: true,
        "bProcessing": true,
        "bDeferRender": true,
        "ordering": false,
        "pagingType": "full_numbers",
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        sDom: 'Btlipr',
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenance/PopulateEstimatesSummery?PrevMasterID=" + prevMasterId,
            "type": "POST",
            "datatype": "json",
        },
        "columns":
            [
                { "data": "TotalPartCost", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "TotalLaborHours", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "TotalCraftCost", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "TotalExternalCost", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "TotalInternalCost", "autoWidth": true, "bSearchable": false, "bSortable": false },
                { "data": "TotalSummeryCost", "autoWidth": true, "bSearchable": false, "bSortable": false },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
//#endregion
//#region Common
function RedirectToDetailOncancel(prevMasterID, mode) {
    swal(CancelAlertSetting, function () {
        RedirectToPmDetail(prevMasterID, mode);
    });
}
//#endregion
//#region Reassign SchedulingPM
var PMSGridTotalGridItem = 0;
var PMSSelectedItemArray = [];
var PrevMainIds = [];
var PMAssignIds = [];
var IndexIds = [];
function PMSSelectedItem(PrevMaintSchedId, AssignedTo, PMID, Description, ChargeTo, ChargeToName, NextDue, PMSAssignId, IndexId) {
    this.PrevMaintSchedId = PrevMaintSchedId;
    this.AssignedTo = AssignedTo;
    this.PMID = PMID;
    this.Description = Description;
    this.ChargeTo = ChargeTo;
    this.ChargeToName = ChargeToName;
    this.NextDue = NextDue;
    this.PMSAssignId = PMSAssignId;
    this.IndexId = IndexId;
}
$(document).find('.dtpicker').datepicker({
    changeMonth: true,
    changeYear: true,
    "dateFormat": "mm/dd/yy",
    autoclose: true
}).inputmask('mm/dd/yyyy');
var PMSchedulingReassigndt;
$(document).on('click', '#liReassign', function () {
    $.ajax({
        "url": "/PreventiveMaintenance/PMSchedulingReassignGrid",
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpreventive').html(data);
        },
        complete: function () {
            $.validator.unobtrusive.parse(document);
            SetAdvSearchControl();
            GeneratePMSchedulingReassign();
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
    $(document).find('#btnmainPMSsearch').on('click', function () {
        PMSchedulingReassigndt.state.clear();
        pmclearAdvanceSearch();
        GeneratePMSchedulingReassign();
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
function GeneratePMSchedulingReassign() {
    var AssignedTo = LRTrim($("#PMSAssignedTo").val());
    var PMID = LRTrim($("#PMSPMID").val());
    var Description = LRTrim($("#PMSDescripton").val());
    var ChargeTo = LRTrim($("#PMSChargeTo").val());
    var ChargeToName = LRTrim($("#PMSChargeToName").val());
    var NextDue = LRTrim($('#PMSNextDue').val());    
    if (typeof PMSchedulingReassigndt !== "undefined") {
        PMSchedulingReassigndt.destroy();
    }
    PMSchedulingReassigndt = $(document).find("#tblScheduleReAssignGrid").DataTable({
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
            "url": "/PreventiveMaintenance/GetPMSchedulingReassign",
            "type": "post",
            "datatype": "json",
            data: function (d) {               
                d.AssignedTo = AssignedTo;
                d.PMID = PMID;
                d.Description = Description;
                d.ChargeTo = ChargeTo;
                d.ChargeToName = ChargeToName;
                d.NextDue = NextDue;
            },
            "dataSrc": function (result) {
                PMSGridTotalGridItem = result.data.length;
                if (PMSGridTotalGridItem < 1) {
                    $(document).find('#btnReassign').hide();
                }
                else {
                    $(document).find('#btnReassign').show();
                }
                return result.data;
            },
            global: true
        },
        "columns":
            [               
                {
                    "data": "IndexId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
                    targets: 0,
                    'render': function (data, type, full, meta) {
                        if ($('#pmScheduleReAssignidselectall').is(':checked')) {
                            return '<input type="checkbox" checked="checked" name="id[]" data-PMid="' + data + '" class="chksearchPMS ' + data + '"  value="'
                                + $('<div/>').text(data).html() + '">';
                        } else {
                            var found = PMSSelectedItemArray.some(function (el) {
                                return el.IndexId === data;
                            });
                            if (found) {
                                return '<input type="checkbox" checked="checked" name="id[]" data-PMid="' + data + '" class="chksearchPMS ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                            else {
                                return '<input type="checkbox" name="id[]" data-PMid="' + data + '" class="chksearchPMS ' + data + '"  value="'
                                    + $('<div/>').text(data).html() + '">';
                            }
                        }
                    },

                },
                { "data": "AssignedTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PMID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "ChargeTo", "autoWidth": true, "bSearchable": true, "bSortable": true
                },
                {
                    "data": "ChargeToName", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "NextDue", "autoWidth": true, "bSearchable": true, "bSortable": true }

            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', '#pmScheduleReAssignidselectall', function (e) {
    var checked = this.checked;
    $.ajax({
        url: "/PreventiveMaintenance/GetPMSAll",
        async: true,
        type: "GET",
        data: {            
            AssignedTo: LRTrim($("#PMSAssignedTo").val()),
            PMID: LRTrim($("#PMSPMID").val()),
            Description: LRTrim($("#PMSDescripton").val()),
            ChargeTo: LRTrim($("#PMSChargeTo").val()),
            ChargeToName: LRTrim($("#PMSChargeToName").val()),
            NextDue: LRTrim($('#PMSNextDue').val())
        },
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data) {
                $.each(data, function (index, item) {
                    var found = PMSSelectedItemArray.some(function (el) {                        
                        return el.IndexId === item.IndexId;
                    });
                    if (checked) {                        
                        if (IndexIds.indexOf(item.IndexId) == -1) {
                            PrevMainIds.push(item.PrevMaintSchedId);
                            PMAssignIds.push(item.PMSAssignId);
                            IndexIds.push(item.IndexId);
                        }

                        var itemLS = new PMSSelectedItem(item.PrevMaintSchedId, item.AssignedTo, item.PMID, item.Description, item.ChargeTo, item.ChargeToName, item.NextDue, item.PMSAssignId, item.IndexId);
                        if (!found) { PMSSelectedItemArray.push(itemLS); }
                    } else {                        
                        var i = IndexIds.indexOf(item.IndexId);
                        PrevMainIds.splice(i, 1);
                        PMAssignIds.splice(i, 1);
                        IndexIds.splice(i, 1);
                        if (found) {
                            PMSSelectedItemArray = PMSSelectedItemArray.filter(function (el) {                               
                                return el.IndexId !== item.IndexId;
                            });
                        }
                    }
                });
            }
        },
        complete: function () {
            PMSchedulingReassigndt.column(0).nodes().to$().each(function (index, item) {
                if (checked) {
                    $(this).find('.chksearchPMS').prop('checked', 'checked');
                } else {
                    $(this).find('.chksearchPMS').prop('checked', false);
                }
            });
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearchPMS', function () {
    var data = PMSchedulingReassigndt.row($(this).parents('tr')).data();
    if (!this.checked) {       
        var index = IndexIds.indexOf(data.IndexId);
        PrevMainIds.splice(index, 1);
        PMAssignIds.splice(index, 1);
        IndexIds.splice(index, 1);
        var el = $('#pmScheduleReAssignidselectall').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        PMSSelectedItemArray = PMSSelectedItemArray.filter(function (el) {            
            return el.IndexId !== data.IndexId;
        });
    }
    else {
        PrevMainIds.push(data.PrevMaintSchedId);
        PMAssignIds.push(data.PMSAssignId);
        IndexIds.push(data.IndexId);
        var item = new PMSSelectedItem(data.PrevMaintSchedId, data.AssignedTo, data.PMID, data.Description, data.ChargeTo, data.ChargeToName, data.NextDue, data.PMSAssignId, data.IndexId);
        var found = PMSSelectedItemArray.some(function (el) {            
            return el.IndexId === data.IndexId;
        });
        if (!found) { PMSSelectedItemArray.push(item); }
    }
    if (PMSGridTotalGridItem == PMSSelectedItemArray.length) {
        $('#pmScheduleReAssignidselectall').prop('checked', 'checked');
    }
    else {
        $('#pmScheduleReAssignidselectall').prop('checked', false);
    }
});
//#region Advance Search
$(document).on('click', "#btnPMSDataAdvSrch", function (e) {
    PMSchedulingReassigndt.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0;    
    $('#advsearchsidebarPMScheduleReassign').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossHistory" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#PMSadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    GeneratePMSchedulingReassign();
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
$(document).on('click', '#pmClearAdvSearchFilter', function () {   
    pmclearAdvanceSearch();
    GeneratePMSchedulingReassign();
});
function pmclearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarPMScheduleReassign').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion
$(document).on('click', '#ReassignPMScheduleID', function (e) {
    var PrevMainVals = PrevMainIds;
    var PMAssignVals = PMAssignIds;
    if (PrevMainVals.length <= 0) {
        ShowGridItemSelectionAlert();
        return false;
    }
    else {
        $(document).find('#prevMaintReassignModel_PrevMainIdsList').val(PrevMainVals.join(','));
        $(document).find('#prevMaintReassignModel_PMSchedAssignIdsList').val(PMAssignVals.join(','));
        $(document).find('form').find("select").removeClass("input-validation-error");
        $('#ReassignPMSchedulePage').modal('show');
        $(this).blur();
    }
});
function UpdatePMSScheduleReassignOnSuccess(data) {
    if (data.Issuccess) {
        PMSchedulingReassigndt.state.clear();
        PMSSelectedItemArray = [];
        PrevMainIds = [];
        PMAssignIds = [];
        IndexIds = [];
        CloseLoader();
        $('#ReassignPMSchedulePage').modal('hide');
        $('.modal-backdrop').remove();
        PMSchedulingReassigndt.state.clear();
        SuccessAlertSetting.text = getResourceValue("ReassignAlert");
        swal(SuccessAlertSetting, function () {
            pmclearAdvanceSearch();
            $('#pmScheduleReAssignidselectall').prop('checked', false);
            GeneratePMSchedulingReassign();
        });
        $('.select2picker').change(function () {
            $('this').removeClass("input-validation-error");
        });
    }

}
$(document).on('click', '#btnReassignPMScheduleCancel', function (e) {
    swal(CancelAlertSetting, function () {
        window.location.href = "/PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search";
    });
});
$(document).on('click', '#btnPMSFilterByEquipmentId', function (e) {
    $('#EquipmentPMModal').modal('hide');
    equipmentId = $(document).find('#prevMaintReassignModel_EquipmentId').val();
    $('#txtsearchbox').val('');
    locationId = 0;
    assignedId = 0;
    prevSearchTable.page('first').draw('page');
});
$(document).on('click', '#btnPMSFilterByLocation', function (e) {
    $('#LocationPMModal').modal('hide');
    equipmentId = 0;
    $('#txtsearchbox').val('');
    locationId = $(document).find('#prevMaintReassignModel_LocationId').val();
    assignedId = 0;
    prevSearchTable.page('first').draw('page');
});
$(document).on('click', '#btnPMSFilterByAssigned', function (e) {
    $('#AssignedPMModal').modal('hide');
    equipmentId = 0;
    $('#txtsearchbox').val('');
    locationId = 0;
    assignedId = $(document).find('#prevMaintReassignModel_PersonnelId').val();
    prevSearchTable.page('first').draw('page');
});
//#endregion
//#region Options
$(document).on('click', '#DeletePM', function () {
    var PMid = $(this).attr('data-id');
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeletePM',
            data: {
                _PMid: PMid
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    SuccessAlertSetting.text = getResourceValue("DeletePMSuccessfullyAlert");
                    swal(SuccessAlertSetting, function () {
                        window.location.href = "../PreventiveMaintenance/Index?page=Maintenance_Preventive_Maintenance_Search";
                    });
                }
                else {
                    GenericSweetAlertMethod(data);
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
$(document).on('click', '#ChangePreventiveMaintenenceId', function (e) {
    var clientlookupid = $(document).find('#hiddenclientlookupid').val();
    $(document).find('#txtPreventiveId').val(clientlookupid).removeClass('input-validation-error');
    $('#changePreventiveIDModalDetailsPage').modal('show');
    $(this).blur();
    $.validator.unobtrusive.parse(document);
});
function PrevMaintOnSuccess(data) {
    $('#changePreventiveIDModalDetailsPage').modal('hide');
    var prevMasterID = $(document).find('#hiddenprevmaintid').val();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePMAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToPmDetail(prevMasterID, "overview");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
//#endregion

$(document).on("click", "#AddPrevMaintInstruction", function () {

    var PrevMaintId = $(document).find('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    $.ajax({
        type: 'GET',
        url: '/PreventiveMaintenance/PopulateInstructions',
        beforeSend: function () {
            ShowLoader();
            ClearEditorById('prevmainttxtinstructions');
        },
        data: {
            PrevMaintId: PrevMaintId
        },
        dataType: 'json',
        success: function (data) {
            if (data) {
                $(document).find('#hiddenInstructionId').val(data.instructionid);
                LoadCkEditorById('prevmainttxtinstructions', function (editor) {
                    SetDataById('prevmainttxtinstructions', data.contents);
                    $(document).find('#hiddenInstructionId').val(data.instructionid);
                    $(document).find('#InstructionContent').val(data.contents);
                });
            }
        },
        complete: function () {
            $("#pminstructmodal").modal('show');
            CloseLoader();
        }
    })

});

$(document).on('click', '#btnsaveinstruction', function () {
    const data = getDataFromEditorById('prevmainttxtinstructions');

    var instructionId = $(document).find('#hiddenInstructionId').val();

    if (instructionId <= 0 && LRTrim(data) == "") {
        ShowErrorAlert(getResourceValue("InstructionRequired"))
        return false;
    }
    $(document).find('#InstructionContent').val(data);
});
function PMInstructionsOnSuccess(data) {
    if (data && data.Result == "success") {
        var alertmessage = '';
        if (data.mode == "add") {
            alertmessage = getResourceValue("InstructionsAddAlert");
        }
        else {
            alertmessage = getResourceValue("InstructionsUpdateAlert");
        }
        ShowSuccessAlert(alertmessage);
        $('.InstructionCancel').click();
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
        CloseLoader();
    }
}

$(document).on('click', '.InstructionCancel', function () {
    $("#pminstructmodal").modal('hide');
    ClearEditorById('prevmainttxtinstructions');
});


//#endregion

//#region Exclusion Days
$(document).on('change', '#FrequencyType', function () {
    if ($('option:selected', this).val() == 'Days' && $('#Frequency').val() == '1') {
        $('#exclusionDays').prop('disabled', false);
        $('#exclusionDaysdynamic').prop('disabled', false);
    }
    else {
        $("#ExclusionDaysString").val('').trigger('change');
        $("#ExclusionDaysStringDynamic").val('').trigger('change');
        $('#exclusionDays').prop('disabled', true);
        $('#exclusionDaysdynamic').prop('disabled', true);
    }
});

$('#Frequency').on('blur', function () {
    if ($('#FrequencyType').val() == 'Days' && $('#Frequency').val() == '1') {
        $('#exclusionDays').prop('disabled', false);
        $('#exclusionDaysdynamic').prop('disabled', false);
    }
    else {
        $("#ExclusionDaysString").val('').trigger('change');
        $("#ExclusionDaysStringDynamic").val('').trigger('change');
        $('#exclusionDays').prop('disabled', true);
        $('#exclusionDaysdynamic').prop('disabled', true);
    }
});

$(document).on('change', '#ExclusionDaysString', function () {
    var exclusionDaysString = $("#ExclusionDaysString").val();
    exclusionDaysArr = exclusionDaysString;
    exclusionDaysStringHidden = exclusionDaysString.join();
    $('.daycb').prop('checked', false);

    $.each(exclusionDaysString, function (index, value) {
        if (value == '0') {
            $('#SundayCB').prop('checked', true);
        }
        if (value == '1') {
            $('#MondayCB').prop('checked', true);
        }
        if (value == '2') {
            $('#TuesdayCB').prop('checked', true);
        }
        if (value == '3') {
            $('#WednesdayCB').prop('checked', true);
        }
        if (value == '4') {
            $('#ThursdayCB').prop('checked', true);
        }
        if (value == '5') {
            $('#FridayCB').prop('checked', true);
        }
        if (value == '6') {
            $('#SaturdayCB').prop('checked', true);
        }
    });
});

$(document).on('click', '#exclusionDays', function () {
    $.ajax({
        url: "/PreventiveMaintenance/ExclusionDays",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#ExclusionDaysModal').html(data);
            $('#ExclusionDaysModalDialog').modal("show");
            if (exclusionDaysStringHidden) {
                exclusionDaysArr = exclusionDaysStringHidden.split(',');
            }
        },
        complete: function () {
            $(document).find(".multi-select2picker").select2({

            });
            SetPreventiveControls();
            if (exclusionDaysArr.length > 0) {
                $('#ExclusionDaysString').val(exclusionDaysArr).trigger('change')
            }
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('hidden.bs.modal', '#exclusionDays', function () {

});

//#endregion

//#region Dev express print
$(document).on('click', '#PrintPM', function () {
    var PMArray = [];
    var PrevMaintMasterId = $('#preventiveMaintenanceModel_PrevMaintMasterId').val();
    var PrevMaintLibraryId = $('#preventiveMaintenanceModel_PrevMaintLibraryId').val();
    var obj = {
        "PrevMaintMasterId": PrevMaintMasterId,
        "PrevMaintLibraryId": PrevMaintLibraryId
    };
    PMArray.push(obj);
    var jsonResult = {
        "PrevMaintMasterIds": PMArray
    };
    $.ajax({
        url: '/PreventiveMaintenance/SetPrintPMList',
        data: JSON.stringify(jsonResult),
        type: "POST",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (result) {
            if (result.success) {
                window.open("/PreventiveMaintenance/GeneratePMPrint", "_blank");
            }
        },
        complete: function () {
            CloseLoader();
            PMArray = [];
        }
    });
});
//#endregion

//#region V2-919
var IsFromEquipment = false;
var RedirectEquipmentId;
var EquipmentClientLookupId;
$(document).ready(function () {
    if ($(document).find("#IsFromEquipment").val() == "True") {
        IsFromEquipment = true;
        EquipmentClientLookupId = $(document).find("#linkToEquipment").text();
        RedirectEquipmentId = $(document).find("#EquipmentId").val();
    }
});
$(document).on('click', '#linkToEquipment', function (e) {
    clearDropzone();
    var EquipmentId = $(document).find('#EquipmentId').val();
    window.location.href = "../Equipment/DetailFromWorkOrder?EquipmentId=" + EquipmentId;
});
//#endregion
//#region V2-712
var dtPMScheduleAssignTable;
var pmSchedHoursEditArray = [];
var currentPMSchedId;
$(document).on('click', '.tblscheduleactionbtns', function (e) {
    var row = $(this).parents('tr');
    var data = dtScheduleTable.row(row).data();
    currentPMSchedId = data.PrevMaintScheId;
    $(document).find("#" + data.PrevMaintScheId + "").show();
    $(document).find("." + data.PrevMaintScheId).show();
});
function funcCloseActionbtn() {
    $(document).find(".actionbtndiv").hide();
    $(document).find(".maskaction").hide();
}
//#region Inner Grid

$(document).on('click', '.PrevMaintAssignGrid', function (e) {

    var tr = $(this).closest('tr');
    var row = dtScheduleTable.row(tr);
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

//#region Assignment Grid
$(document).on('click', '.assignmentBttn', function () {
    var data = dtScheduleTable.row($(this).parents('tr')).data();
    var PrevMaintScheId = data.PrevMaintScheId;
    GeneratePMSchedulingAssignGrid(PrevMaintScheId);
});
function GeneratePMSchedulingAssignGrid(PrevMaintScheId) {
    pmSchedHoursEditArray = [];
    if ($(document).find('#tblPMScheduleAssign').hasClass('dataTable')) {
        dtPMScheduleAssignTable.destroy();
    }
    dtPMScheduleAssignTable = $("#tblPMScheduleAssign").DataTable({
        colReorder: false,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PreventiveMaintenance/PopulatePMScheduleAssignRecords?PMScheduleId=" + PrevMaintScheId,
            "type": "POST",           
            "datatype": "json",
            "dataSrc": function (response) {
                rCount = response.data.length;
                $(document).find("#pMSchedAssignModel_PrevMaintSchedId").val(PrevMaintScheId);
                if (rCount > 0) {
                    $(document).find("#AddPMSAssignmentbtn").hide();
                }
                else {
                    $(document).find("#AddPMSAssignmentbtn").show();
                }
                return response.data;
                
            },          
        },
        columnDefs: [
            {
                "data": null,
                targets: [3], render: function (a, b, data, d) {
                    return '<a class="btn btn-outline-primary addpmscheduleassignBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                        '<a class="btn btn-outline-danger delpmscheduleassignBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';

                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": false, "bSearchable": true, "bSortable": true },
                { "data": "PersonnelFullName", "autoWidth": false, "bSearchable": true, "bSortable": true },
                {
                    "data": "ScheduledHours", "autoWidth": false, "bSearchable": true, "bSortable": false,
                    'render': function (data, type, row) {
                        if (pmSchedHoursEditArray.indexOf(row.PMSchedAssignId) != -1) {
                            return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-schedulehours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:block;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                        } else {
                            return "<div style='width:110px !important; position:relative;'><input type='text' style='width:90px !important;text-align:right;' class='duration  dt-inline-text decimalinputupto2places grd-schedulehours' autocomplete='off' value='" + data + "' maskedFormat='6,2'><i class='fa fa-check-circle is-saved-check' style='float: right; position: absolute; top: 8px; right:-3px; color:green;display:none;' title='success'></i><i class='fa fa-times-circle is-saved-times' style='float: right; position: absolute; top: 8px; right:-3px; color:red;display:none;'></i><img src='../Content/Images/grid-cell-loader.gif' class='is-saved-loader' style='float:right;position:absolute;top: 8px; right:-3px;display:none;' /></div>";
                        }
                    }
                },
                { "data": "ClientLookupId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            CloseLoader();
            SetPageLengthMenu();
            if (!$(document).find('#PMScheduleAssignModal').hasClass('show')) {
                $(document).find('#PMScheduleAssignModal').modal({ backdrop: 'static', keyboard: false, show: true });
            }
        }
    });
}

$(document).on('click', '.closepmsagrid', function () {
    $(document).find('#pMSchedAssignModel_PrevMaintSchedId').val();
    GenerateScheduleGrid();

})
//#endregion

//#region Add Assignment
$(document).on('click', '.addpmscheduleassignBttn', function () {
    var data = dtPMScheduleAssignTable.row($(this).parents('tr')).data();    
    var PrevMaintSchedId = $("#pMSchedAssignModel_PrevMaintSchedId").val();
    AddPmScheduleAssignment(PrevMaintSchedId);
});

function AddPmScheduleAssignment(PrevMaintSchedId) {

    $.ajax({
        url: "/PreventiveMaintenance/AddPMScheduleAssignment",
        type: "GET",
        dataType: 'html',
        data: { PmSchedId:PrevMaintSchedId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#AddPMScheduleAssign').html(data);
            if (!$(document).find('#AddPMScheduleAssignModal').hasClass('show')) {
                $(document).find('#AddPMScheduleAssignModal').modal({ backdrop: 'static', keyboard: false, show: true });
            }
        },
        complete: function () {
            CloseLoader();
            SetAssignmentControls();
        },
        error: function () {
            CloseLoader();
        }
    });
}

function SetAssignmentControls() {
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

function PMSchedAssignmentInfoOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("PmScheduledAssignmentAddAlert");
        swal(SuccessAlertSetting, function () {
            GeneratePMSchedulingAssignGrid(data.pmSchedId);
            $(document).find('#AddPMScheduleAssignModal').modal("hide");

        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region ScheduleHour edit

var enterhit = 0;
var oldHour = 0;
$(document).on('blur', '.grd-schedulehours', function (event) {
    if (enterhit == 1)
        return;
    var row = $(this).parents('tr');
    var data = dtPMScheduleAssignTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();   

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
            url: '/PreventiveMaintenance/UpdatePmScheduleAssignHours',
            data: {
                PMSchedAssignId: data.PMSchedAssignId,
                hours: $(this).val()
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
                    pmSchedHoursEditArray.push(dtPMScheduleAssignTable.row(row).data().PMSchedAssignId);
                    pageno = dtPMScheduleAssignTable.page.info().page;
                    dtPMScheduleAssignTable.page(pageno).draw('page');
                }
                else {
                    thstextbox.siblings('.is-saved-times').show();
                }
            },
            complete: function () {
                oldHour = 0;
                CloseLoader();
            }
        });
    }
    else {
        oldHour = 0;
    }
});
$(document).on('keypress', '.grd-schedulehours', function (event) {
    var row = $(this).parents('tr');
    var data = dtPMScheduleAssignTable.row(row).data();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    var thstextbox = $(this);
    thstextbox.siblings('.is-saved-times').hide();    
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
                url: '/PreventiveMaintenance/UpdatePmScheduleAssignHours',
                data: {
                    PMSchedAssignId: data.PMSchedAssignId,
                    hours: $(this).val()
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
                        pmSchedHoursEditArray.push(dtPMScheduleAssignTable.row(row).data().PMSchedAssignId);
                        pageno = dtPMScheduleAssignTable.page.info().page;
                        dtPMScheduleAssignTable.page(pageno).draw('page');
                    }
                    else {
                        thstextbox.siblings('.is-saved-times').show();
                    }
                },
                complete: function () {
                    enterhit = 0;
                    oldHour = 0;
                    CloseLoader();
                }
            });
        }
        else {
            oldHour = 0;
        }
    }
    event.stopPropagation();
});
$(document).on('focus', '.grd-schedulehours', function (event) {
    oldHour = $(this).val();
});
//#endregion

//#region delete Assignment
$(document).on('click', '.delpmscheduleassignBttn', function () {
    var data = dtPMScheduleAssignTable.row($(this).parents('tr')).data();
    var PMSchedAssignId = data.PMSchedAssignId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/PreventiveMaintenance/DeletePmSchedAssign',
            data: {
                pmSchedAssignId: PMSchedAssignId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("PmScheduledAssignmentDeleteAlert"));
                    dtPMScheduleAssignTable.state.clear();
                }
            },
            complete: function () {
                GeneratePMSchedulingAssignGrid(data.PrevMaintSchedId);
                CloseLoader();
            }
        });
    });
});
//#endregion
//#endregion


//#region V2-950
$(document).on('click', '#exclusionDaysdynamic', function () {
    $.ajax({
        url: "/PreventiveMaintenance/ExclusionDaysForDynamic",
        type: "GET",
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#ExclusionDaysModal').html(data);
            $('#ExclusionDaysModalDialog').modal({ backdrop: 'static', keyboard: false }, 'show');
            if (exclusionDaysStringHidden) {
                exclusionDaysArr = exclusionDaysStringHidden.split(',');
            }
        },
        complete: function () {
            $(document).find(".multi-select2picker").select2({

            });
            SetPreventiveControls();
            if (exclusionDaysArr.length > 0) {
                $('#ExclusionDaysStringDynamic').val(exclusionDaysArr).trigger('change')
            }
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', "#btnschedulescanceldynamic", function () {
    var prevMasterID = $(document).find('#PMSPrevMaintMasterId').val();
    RedirectToDetailOncancel(prevMasterID, "schedule");
});

$(document).on('click', '#btnSaveExcludeDOW', function () {
    var PrevMaintSchedId = $("#EditPMSRecordsModelDynamic_Calendar_PrevMaintSchedId").val();
    var exclusionDaysString = $("#ExclusionDaysStringDynamic").val();
    if (exclusionDaysString.length == 7) {
        var errorMessage = getResourceValue("MoreThanSixDaysAlert");
        ShowErrorAlert(errorMessage);
        return false;
    }
    else {
        exclusionDaysArr = exclusionDaysString;
        exclusionDaysStringHidden = exclusionDaysString.join();
        $.ajax({
            url: "/PreventiveMaintenance/SaveExcludeDOW",
            type: "POST",
            dataType: 'json',
            data: { PrevMaintSchedId: PrevMaintSchedId, ExclusionDaysString: exclusionDaysArr },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    $('.cancelExclusionDaysModal').trigger('click');
                    SuccessAlertSetting.text = getResourceValue("ExcludeDaysUpdateAlert");
                    swal(SuccessAlertSetting, function () {

                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function (jqXHR, exception) {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '.cancelExclusionDaysModal', function () {
    $("#ExclusionDaysStringDynamic").val(exclusionDaysArr);
    $('.modal-backdrop').remove();
    $(document).find('#ExclusionDaysModalDialog').modal('hide');
});

//#endregion
