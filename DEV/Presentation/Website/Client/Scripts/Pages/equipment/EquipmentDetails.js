var dtTable;
var dtChildrenTable;
var SelectChildrenTable;
var dtSensorTable;
var dtPartsTable;
var dtDownTimeTable;
var dtPMTable;
var dtWOCompleteTable;
var dtPartIssueTable;
var dtWOActiveTable;
var techSpecsTable;
var editor;
var SensorDataTable;
var PartsDataTable;
var DowntimeDataTable;
var PMDataTable;
var WOCompleteTable;
var TechSpecsTable;
var SensorPopupTable;
var _EquipmentId;
var dtNotesTable;
var dtAttachmentTable;
var _ObjectId;

var colorArray = ["#fe0000", "#ff7f00", "#fffe01", "#00bd3f", "#0068ff", "#7a01e6", "#d300c9", "#940100", "#066d7c", "#66cbff"];

function openCity2(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
function openCity(evt, cityName) {
    evt.preventDefault();
    $('#overviewcontainer').hide();
    $('#Equipmenttab').hide();
    switch (cityName) {
        case "Photos":
            LoadImages($('#EquipData_EquipmentId').val());
            break;
        case "Children":
            generateChildrenDataTable();
            hideAudit();
            break;
        case "Notes":
            generateNoteDataTable();
            hideAudit();
            break;
        case "Attachment":
            generateAttachmentDataTable();
            hideAudit();
            break;
        case "divSensors":
            generateSensorDataTable();
            hideAudit();
            break;
        case "TechSpecs":
            generateTechSpecsTable();
            hideAudit();
            break;
        case "PartsContainer":
            generatePartsDataTable();
            hideAudit();
            break;
        case "Downtime":
            generateDowntimeDataTable();
            hideAudit();
            break;
        case "PMList":
            generatePMDataTable();
            hideAudit();
            break;
        case "WOActive":
            generateWOActiveDataTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "WOComplete":
            generateWOCompleteTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "PartIssues":
            generatePartIssueTable();
            $(document).find(".sidebar").mCustomScrollbar({
                theme: "minimal"
            });
            hideAudit();
            break;
        case "Equipment":
            $('#overviewcontainer').show();
            $('#Equipmenttab').show();
            $('.tabcontent2').show();
            $('#btnIdentification').addClass('active');
            ShowAudit();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent2");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks2");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
}
function hideAudit() {
    $('#btnnblock').removeClass('col-xl-6').addClass('col-xl-12');
    $('#auditlogcontainer').removeClass('col-xl-6').addClass('col-xl-12').hide();
}
function ShowAudit() {
    $('#btnnblock').removeClass('col-xl-12').addClass('col-xl-6');
    $('#auditlogcontainer').removeClass('col-xl-12').addClass('col-xl-6').show();
}
$(document).ready(function () {
    $(document).find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '.dismiss, .overlay,#dismiss', function () {
        $('.sidebar,#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    var myLineChart;
    var WObyPriorityChart;
    var fontSize = 12;
    Chart.defaults.global.defaultFontColor = '#575962';
    Chart.defaults.global.defaultFontFamily = "Roboto";
    wobyTypeGraphData();
    generateEquipGraph();
    generateChildrenDataTable();
    $(document).on('click', '#btnAddDownTime', function (e) {
        var equipmentid = $(document).find('#EquipData_EquipmentId').val();
        AddDowntime(equipmentid);
    });
    $(document).on('click', '#btnAddParts', function (e) {
        var equipmentid = $(document).find('#EquipData_EquipmentId').val();
        AddParts(equipmentid);
    });
    //#region Tech Spec
    $(document).on('click', '#btnAddTechSpecs', function (e) {
        var EquipmentId = $('#EquipData_EquipmentId').val();
        AddTechSpecs(0, EquipmentId, 0, 0, 'add');
    });
    $(document).on('click', "#Techspeccancelbutton", function () {
        var equipmentid = $(document).find('#techSpecsModel_EquipmentId').val();
        swal(CancelAlertSetting, function () {
            RedirectToEquipmentDetail(equipmentid, "techspec");
        });
    });
    //#endregion
    //#region 780
    var equipmentSearchstatus = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT");
    if ($('#IsDetailFromWorkOrder').val() != undefined && $('#IsDetailFromWorkOrder').val().toLowerCase() === 'true') {
        $(document).find('#spnlinkToSearch').text(equipmentSearchstatus);
        var EquimentId = $('#EquipData_EquipmentId').val();
        LoadComments(EquimentId);
        LoadImages(EquimentId);
        if ($('button[data-id="equipDtDropdown"]').length > 0) {
            $('button[data-id="equipDtDropdown"]').remove();
        }
        if ($('button[data-id="wotypeDropdown"]').length > 0) {
            $('button[data-id="wotypeDropdown"]').remove();
        }
        SetEquipmentDetailEnvironment();
        titleText = equipmentSearchstatus;
    }
    //#endregion
    //#region V2-1147
  
    if ($('#IsDetailFromNotification').val() != undefined && $('#IsDetailFromNotification').val().toLowerCase() === 'true') {
        //reset the value of EQUIPMENTSEARCHGRIDDISPLAYSTATUS 
        
        localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS", "1");
        assetstatus = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS");
        activeStatus = assetstatus;
        $('#astsearchListul li').first().addClass('activeState');
        $('#assetsearchtitle').text(getResourceValue("AlertActive"));
      
        localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT", getResourceValue("AlertActive"));
        var equipmentSearchstatus = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT");
        $(document).find('#spnlinkToSearch').text(equipmentSearchstatus);
        var EquimentId = $('#EquipData_EquipmentId').val();
        LoadComments(EquimentId);
        LoadImages(EquimentId);
        if ($('button[data-id="equipDtDropdown"]').length > 0) {
            $('button[data-id="equipDtDropdown"]').remove();
        }
        if ($('button[data-id="wotypeDropdown"]').length > 0) {
            $('button[data-id="wotypeDropdown"]').remove();
        }
        SetEquipmentDetailEnvironment();
        titleText = equipmentSearchstatus;
    }
    //#endregion
    $(document).on('click', '#btnSelectSensor', function (e) {
        AddNewSensor();
    });
    $(document).on('click', '#example-select-all-sensor', function (e) {
        var checked = this.checked;
        SensorPopupTable.column(0).nodes().to$().each(function (index) {
            if (checked) {
                $(this).find('.isSelect').prop('checked', 'checked');
            } else {
                $(this).find('.isSelect').prop('checked', false);
            }
        });
        SensorPopupTable.draw();
        $('#sensorSearchTable').on('change', 'input[type="checkbox"]', function () {
            if (!this.checked) {
                var el = $(document).find('#example-select-all-sensor').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    });
    $(document).on('click', '#btnAddSensor', function () {
        var EquipmentId = $('#EquipData_EquipmentId').val();
        var eqIDs = SensorPopupTable.column(0).nodes().to$().map(function () {
            if ($(this).find('.isSelect').is(':checked')) {
                return $(this).find('.isSelect').data('eqid');
            }
        }).get().join(',');

        if (eqIDs !== "") {
            $.ajax({
                url: '/Equipment/AddSensor',
                data: { _SensorIds: eqIDs, EquipmentId: EquipmentId },
                type: "GET",
                datatype: "json",

                beforeSend: function () {
                    ShowLoader();
                },
                success: function (data) {
                },
                complete: function () {
                    CloseLoader();
                    dtSensorTable.destroy();
                    generateSensorDataTable();
                    $('#mySensorModal').modal('hide');
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
        else {
            swal({
                title: getResourceValue("NoRowsSelectAlert"),
                text: getResourceValue("SelectSensorAlerts"),
                type: "warning",
                confirmButtonClass: "btn-sm btn-primary",
                confirmButtonText: getResourceValue("SaveAlertOk"),
            });
            return false;
        }
    });
    $(document).on('click', '#btnSelectChildren', function (e) {
        if ($(document).find('#searchTable3').hasClass('dataTable')) {
            SelectChildrenTable.destroy();
        }
        generateSelectChildrenTable();
        $(document).find('#searchTable3').on('change', 'input[type="checkbox"]', function () {
            if (!this.checked) {
                var el = $(document).find('#example-select-all').get(0);
                if (el && el.checked && ('indeterminate' in el)) {
                    el.indeterminate = true;
                }
            }
        });
    });
    $(document).on('click', '#example-select-all', function (e) {
        var checked = this.checked;
        SelectChildrenTable.column(0).nodes().to$().each(function (index) {
            if (checked) {
                $(this).find('.isSelect').prop('checked', 'checked');
            } else {
                $(this).find('.isSelect').prop('checked', false);
            }
        });
        SelectChildrenTable.draw();
    });
    $(document).find(".tabsArea").hide();
    $(document).find("ul.vtabs li:first").addClass("active").show();
    $(document).find(".tabsArea:first").show();
    $(document).on('change', '#colorselector', function (evt) {
        $(document).find('.tabsArea').hide();
        openCity(evt, $(this).val());
        $('#' + $(this).val()).show();
    });
    //#region Equiment Edit
    $(document).on('click', '#ImgShowAccount', function (e) {
        $("#ImgCloseAccount").show();
        $("#dvAccountContainer").show();
        $("#ImgShowAccount").hide();
    });
    $(document).on('click', '#ImgCloseAccount', function (e) {
        $("#dvAccountContainer").hide();
        $("#ImgShowAccount").show();
        $("#ImgCloseAccount").hide();
    });
    var PlantLocationId = -1;
    $(document).on('focus', '.txtTreeView', function () {
        $('#equipTreeModal').modal('show');
        $(this).blur();
        generateTree(-1);
    });
    $(document).on('click', '#pldArray', function (e) {
        $('#equipTreeModal').modal('show');
        $(this).blur();
        generateTree(-1);
    });
    function generateTree(paramVal) {
        $.ajax({
            url: '/PlantLocationTree/PlantLocationTree',
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
                treeTable($(document).find('#tblTree'));
                $(document).find('.radSelect').each(function () {
                    if ($(this).data('plantlocationid') === PlantLocationId)
                        $(this).attr('checked', true);
                });
            },
            error: function (xhr) {
                alert('error');
            }
        });
    }
    $(document).on('change', '.radSelect', function () {
        PlantLocationId = $(this).data('plantlocationid');
        var Description = $(this).data('description');
        var equipmentid = $(document).find('#EquipData_EquipmentId').val();
        $.ajax({
            url: '/PlantLocationTree/MapPlantLocationTree',
            datatype: "json",
            type: "post",
            contenttype: 'application/json; charset=utf-8',
            async: false,
            cache: false,
            data: { EquipmentId: equipmentid, 'plantLocationId': PlantLocationId },
            success: function () {
                $('#equipTreeModal').modal('hide');
                $(document).find('#EquipData_PlantLocationDescription').val(Description);
            }
        });
    });
    //#endregion
    $('#tabselector2').on('change', function (evt) {
        var cityName = $(this).val();
        openCity(evt, cityName);
    });
});
function StartValidation() {
    onLoginBegin();
    var EquipmentId = $('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Type = $('#EquipData_Type').val();
    var valid = true;
    if (!EquipmentId) {
        $('#EquipData_ClientLookupId').addClass('input-validation-error');
        valid = false;
    }
    if (!Name) {
        $('#EquipData_Name').addClass('input-validation-error');
        valid = false;
    }
    if (!Type) {
        $('#EquipData_Type').addClass('input-validation-error');
        valid = false;
    }
    if (!valid) {
        ScrollToClass('pushDiv1');
    }
    return valid;
}
function EquipmentEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.new-custom-select', function () {
    if ($(this).find('#equipDtDropdown').length > 0) {
        $('#equipmentdowntimechartloader').show();
        $("#chartdiv").prev().hide();
        generateEquipGraph();
    }
    else if ($(this).find('#wotypeDropdown').length > 0) {
        $('#workorderbytypechartloader').show();
        $('#workorderchartdiv').prev().hide();
        wobyTypeGraphData();
    }
});
function generateEquipGraph() {
    if ($('#chartdiv').length == 0) {
        return;
    }
    if (typeof myLineChart !== "undefined") {
        myLineChart.destroy();
    }
    var tframe = parseInt($(document).find('#equipDtDropdown').val());
    var timeframe = (isNaN(tframe)) ? 1 : tframe;
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var c = document.getElementById("chartdiv");
    var ctx = c.getContext("2d");
    $.ajax({
        type: "POST",
        url: "/Equipment/EquipMentChartData",
        dataType: "json",
        async: true,
        data: {
            timeframe: timeframe,
            EquipmentId: EquipmentId
        },
        success: function (tData) {
            if (tData.datasets != null && tData.datasets.length > 0) {
                $("#chartdiv").show();
                $("#chartdiv").prev().hide();
                myLineChart = new Chart(ctx, {
                    type: 'bar',
                    data: tData,
                    options: {
                        hover: {
                            mode: false
                        },
                        title: {
                            display: false,
                        },
                        tooltips: {
                            intersect: false,
                            mode: 'nearest',
                            xPadding: 10,
                            yPadding: 10,
                            caretPadding: 10,
                            titleFontSize: 12,
                            bodyFontSize: 10,
                            titleFontStyle: 'normal'
                        },
                        legend: {
                            display: false
                        },
                        responsive: true,
                        maintainAspectRatio: false,
                        barRadius: 4,
                        scales: {
                            xAxes: [{
                                display: true,
                                gridLines: true,
                                scaleLabel: {
                                    labelString: 'Asset',
                                    display: true,
                                    fontSize: 12,
                                    padding: 8,

                                },
                                ticks: {
                                    display: false
                                }
                            }],
                            yAxes: [{
                                display: true,
                                gridLines: true,
                                ticks: {
                                    beginAtZero: true,
                                    fontSize: 9,
                                    fontColor: "#575962",
                                    fontStyle: "bold"
                                },
                                scaleLabel: {
                                    labelString: 'Down Time In Minutes',
                                    display: true,
                                    fontSize: 12,
                                    padding: 5
                                }
                            }]
                        },
                        layout: {
                            padding: {
                                left: 0,
                                right: 0,
                                top: 0,
                                bottom: 0
                            }
                        }
                    }
                });
            }
            else {
                $("#chartdiv").prev().show();
            }
        },
        error: function () {

        },
        complete: function () {
            $('#equipmentdowntimechartloader').hide();
        }
    });
}
function wobyTypeGraphData() {
    var count = 0;
    if ($('#workorderchartdiv').length == 0) {
        return;
    }
    var count = 0;
    $('#js-legend').html('');
    $('#workorderbytypechartloader').show();

    if (typeof WObyTypeChart !== "undefined") {
        WObyTypeChart.destroy();
    }
    var wotimeframe = {
        wotimeframe: parseInt($('#wotypeDropdown option:selected').val())
    };
    var backgroundcolors = [];
    $.ajax({
        type: "GET",
        url: "/Equipment/WObyTypeData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        async: true,
        data: {
            wotimeframe: wotimeframe.wotimeframe
        },
        error: function () {

        },
        complete: function () {
            $('#workorderbytypechartloader').hide();
        }
    });
    function OnSuccess(response) {
        count = response.labels.length;
        if ($('#workorderchartdiv').length == 0) {
            return;
        }
        if (response.series.length > 0) {
            $('#workorderchartdiv').prev().hide();
            $('#js-legend').show();
            if (response.length == 0) {
                $('#workorderchartdiv').prev().show();
                return;
            }
            var data = {
                labels: response.labels,
                datasets:
                    [
                        {
                            data: response.series,
                            backgroundColor: colorArray.slice(0, count),
                        }]
            };
            count = response.labels.length;
            WObyTypeChart = new Chart(document.getElementById('workorderchartdiv').getContext("2d"), {
                plugins: [{
                    beforeDraw: function (chart, options) {
                        var width = chart.chart.width,
                            height = chart.chart.height,
                            ctx = chart.chart.ctx;
                        ctx.restore();
                        var fontSize = (height / 8).toFixed(2);
                        ctx.font = fontSize + "px 'Roboto', sans-serif";
                        ctx.textBaseline = "middle";
                        ctx.fillStyle = "grey";
                        var text = count,
                            textX = Math.round((width - ctx.measureText(text).width) / 2),
                            textY = height / 2;

                        ctx.fillText(text, textX, textY);
                        ctx.save();
                    }
                }],
                type: 'doughnut',
                data: data,
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        display: false,
                    },
                    tooltips: {
                        mode: 'nearest',
                        intersect: false,
                        position: 'nearest',
                        xPadding: 10,
                        yPadding: 10,
                        caretPadding: 10,
                        titleFontSize: 12,
                        bodyFontSize: 10,
                        titleFontStyle: 'normal'
                    },
                    cutoutPercentage: 70
                }
            });
            document.getElementById('js-legend').innerHTML = WObyTypeChart.generateLegend();
        }
        else {
            $('#js-legend').hide();
            $('#workorderchartdiv').prev().show();
        }
    }
}
function OpenWorkOrders_db() {
    $.ajax({
        type: "GET",
        url: "/Equipment/WOCompleteOrSchedule",
        dataType: "JSON",
        success: function (data) {
            $('#openwocount').text(data.thisCount);
            $('#openwopercentage').text(data.percentage + '%');
            $('#owoprogressbar').css('width', data.percentage + '%')
        },
    });
}
function WorkRequest_db() {
    $.ajax({
        type: "GET",
        url: "/Equipment/WorkRequest",
        dataType: "JSON",
        success: function (data) {
            $('#woReqcount').text(data.thisCount);
            $('#woReqpercentage').text(data.percentage + '%');
            $('#woReqprogressbar').css('width', data.percentage + '%')
        },
    });
}
function OverDuPm_db() {
    $.ajax({
        type: "GET",
        url: "/Equipment/OverDuePm",
        dataType: "JSON",
        success: function (data) {
            $('#overDuePMCount').text(data.thisCount);
            $('#overDuePMpercentage').text(data.percentage + '%');
            $('#overDuePMprogressbar').css('width', data.percentage + '%')
        },
    });
}
$('#searchTable3 tbody').on('change', 'input[type="checkbox"]', function () {
    if (!this.checked) {
        var el = $('#example-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
    }
});
function generateChildrenDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var visibility;
    if ($(document).find('#childrentable').hasClass('dataTable')) {
        dtChildrenTable.destroy();
    }
    dtChildrenTable = $("#childrentable").DataTable({
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
            "url": "/Equipment/GetAllChildrenEquipmentList_SensorChunkSearch",
            "type": "POST",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.ClientLookupId = LRTrim($("#colindex_0").val());
                d.Name = LRTrim($("#colindex_1").val());
                d.SerialNumber = LRTrim($('#colindex_2').val());
                d.Type = LRTrim($("#colindex_3").val());
                d.Make = LRTrim($("#colindex_4").val());
                d.Model = LRTrim($('#colindex_5').val());
            },
            "datatype": "json",
            "dataSrc": function (response) {
                visibility = response.SecurityEditVal;
                return response.data;
            }
        },
        columnDefs: [{
            "data": "EquipmentId",
            orderable: false,
            targets: 6,
            'render': function (data, type, row) {
                if (visibility) {
                    return '<a class="btn btn-outline-danger delBtnchildren gridinnerbutton" title= "Delete"> <i class="fa fa-trash"></i></a>';
                }
                else {
                    return "";
                }
            }
        }],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "EquipmentId", "autoWidth": true, "bSearchable": false, "bSortable": false, "className": "text-center" },
            ],
        initComplete: function (data, recordsTotal) {
            $(document).find('#tbleqpfooter').show();
            if (visibility == false) {
                $("#btnSelectChildren").hide();
                var column = this.api().column(6);
                column.visible(false);
            }
            else {
                $("#btnSelectChildren").show();
                var column = this.api().column(6);
                column.visible(true);
            }
            SetPageLengthMenu();
            $('#childrentable tfoot th').each(function (i, v) {
                if (i >= 0 && i<6) {
                    var colIndex = i;                    
                    $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');                   
                }
            });

             $(document).ready(function () {
                $('#childrentable').find('.tfootsearchtxt').on('keyup', function (e) {
                    if (e.keyCode === 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        dtChildrenTable.page('first').draw('page');                       
                    }
                });

              
            });
            return;
        }
    });
}


function generateSensorDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    if ($(document).find('#sensorTable').hasClass('dataTable')) {
        dtSensorTable.destroy();
    }
    dtSensorTable = $("#sensorTable").DataTable({
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
            "url": "/Equipment/GetEquipment_SensorChunkSearch",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }

        },
        columnDefs: [],
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return '<a class=lnk_IoTDevicedetails href="javascript:void(0)">' + data + "</a>";
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SensorType", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "LastReading", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-right" },
                { "data": "SensorUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "LastReadingDate", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        "initComplete": function (settings, json) {
            SetPageLengthMenu();
        },
    });
}

//#region Iot Device Link Details
$(document).on('click', '.lnk_IoTDevicedetails', function (e) {
    var row = $(this).parents('tr');
    var data = dtSensorTable.row(row).data();
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var IoTDeviceId = data.IoTDeviceId;
    window.location.href = "../Devices/DetailFromEquipment?IoTDeviceId=" + IoTDeviceId + "&EquipmentId=" + equipmentid + "&EquipmentClientLookupId=" + ClientlookUpId;
});
//#endregion
function generatePartsDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsTable.destroy();
    }
    dtPartsTable = $("#partsTable").DataTable({
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
            "url": "/Equipment/GetEquipment_Parts",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.PartClientLookUpId = LRTrim($("#PartClientLookUpId").val());
                d.Description = LRTrim($("#Description").val());
                d.StockType = LRTrim($('#StockType').val());

            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.partSecurity;
                rCount = json.data.length;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5],
                render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-primary addBtnParts gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnParts gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnParts gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
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
                    "data": "Part_ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=link_part_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Part_Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "QuantityNeeded", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "QuantityUsed", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Comment", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            var column = this.api().column(5);
            if (visibility == false) {
                column.visible(false);
            }
            else {
                column.visible(true);
            }
            if (rCount > 0 || visibility == false) { $("#btnAddParts").hide(); }
            else {
                $("#btnAddParts").show();
            }
            SetPageLengthMenu();
        }
    });
}
function generateDowntimeDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibilityCreate;
    var visibilityEdit;
    var visibilityDelete;
    if ($(document).find('#downtimeTable').hasClass('dataTable')) {
        dtDownTimeTable.destroy();
    }
    dtDownTimeTable = $("#downtimeTable").DataTable({
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
            "url": "/Equipment/GetEquipment_Downtime",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                visibilityCreate = json.secDownTimeAdd;
                visibilityEdit = json.secDownTimeEdit;
                visibilityDelete = json.secDownTimeDelete;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    var actionButtonhtml = "";
                    if (visibilityCreate == true) {
                        actionButtonhtml = '<a class="btn btn-outline-primary addEquipmentDownTimeBttn gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>';
                        if (visibilityEdit == true) {
                            actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-success editEquipmentDownTimeBttn gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';
                        }
                    }
                    if (visibilityDelete == true) {
                        actionButtonhtml = actionButtonhtml + '<a class="btn btn-outline-danger deleteEquipmentDownTimeBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    return actionButtonhtml;
                }
            }
        ],
        "columns":
            [
                { "data": "WorkOrderClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "DateDown",
                    "type": "date "
                },
                { "data": "MinutesDown", "autoWidth": true, "bSearchable": true, "bSortable": true, "class": "text-right" },
                { "data": "ReasonForDownDescription", "autoWidth": true, "bSearchable": true, "bSortable": true },//V2-695
                { "data": "EquipmentId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        "footerCallback": function (row, data, start, end, display) {

            var api = this.api();
            var rows = api.rows().nodes();
            var getData = api.rows({ page: 'current' }).data();
            // Total over all pages
            total = api.column(2).data().reduce(function (a, b) {
                return parseFloat(a) + parseFloat(b);
            }, 0);
            // Update footer
            $("#downtimeTablefoot").empty();
            if (data.length != 0) {
                var footer = "";
                //V2-775
                //if (end == getData[0].TotalCount) {
                //    footer = '<tr><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(4) + '</th><th></th><th></th></tr><tr><th></th><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                //    $("#downtimeTablefoot").empty().append(footer);
                //}
                //else {
                //    footer = '<tr><th></th><th style="text-align: left!important; font-weight: 500!important; color:#0b0606!important">Total</th><th style = "text-align: right!important; font-weight: 500!important; color: #0b0606!important; padding: 0px 10px 0px 0px!important" >' + total.toFixed(4) + '</th><th></th><th></th></tr>'
                //    $("#downtimeTablefoot").empty().append(footer);
                //}
                if (end == getData[0].TotalCount) {
                    footer = '<tr><th></th><th style="text-align: left !important; font-weight: 500 !important; color: #0b0606 !important">Grand Total</th><th style="text-align: right !important; font-weight: 500 !important; color: #0b0606 !important; padding: 0px 10px 0px 0px !important">' + getData[0].TotalMinutesDown.toFixed(4) + '</th><th></th> <th></th></tr>'
                    $("#downtimeTablefoot").empty().append(footer);
                }
            }
        },
        initComplete: function () {
            if (visibilityCreate == false && visibilityDelete == false) {
                var column = this.api().column(4);
                column.visible(false);
            }
            else {
                var column = this.api().column(4);
                column.visible(true);
            }
            if (rCount > 0 || visibilityCreate == false) { $("#btnAddDownTime").hide(); }
            else {
                $("#btnAddDownTime").show();
            }
            SetPageLengthMenu();
        },

    });
}

function generatePMDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var visibility;
    if ($(document).find('#pmTable').hasClass('dataTable')) {
        dtPMTable.destroy();
    }
    dtPMTable = $("#pmTable").DataTable({
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
            "url": "/Equipment/GetEquipment_PMList",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "dataSrc": function (json) {
                visibility = json.pmSecurity;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-success btnPmListEdit gridinnerbutton" title="Edit"><i class="fa fa-pencil"></i></a>';
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
                    "data": "ClientLookupId",
                    "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return '<a class=lnk_preventivemaintenancedetails href="javascript:void(0)">' + data + "</a>";
                    }
                },
                {
                    "data": "Description",
                    "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "LastScheduled", "type": "date " },
                { "data": "LastPerformed", "type": "date " },
                { "data": "AssignedTo_PersonnelName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "PrevMaintMasterId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == true) {
                var column = this.api().column(5);
                column.visible(true);
            }
            else {
                var column = this.api().column(5);
                column.visible(false);
            }
            SetPageLengthMenu();
        }
    });
}
//#region PM List Link Details
$(document).on('click', '.lnk_preventivemaintenancedetails', function (e) {
    var row = $(this).parents('tr');
    var data = dtPMTable.row(row).data();
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var PrevMasterId = data.PrevMaintMasterId;
    window.location.href = "../PreventiveMaintenance/DetailFromEquipment?PrevMaintMasterId=" + PrevMasterId + "&EquipmentId=" + equipmentid + "&EquipmentClientLookupId=" + ClientlookUpId;
});
//#endregion
function generatePartIssueTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var srcData = LRTrim($("#txtPartIssueSearchbox").val());
    var PartClientLookupId = LRTrim($("#piGridadvsearchPartId").val());
    var Description = LRTrim($("#piGridadvsearchDescription").val());
    var ChargeToClientLookupId = LRTrim($("#piGridadvsearchChargeTo").val());
    var UnitofMeasure = LRTrim($("#piGridadvsearchUnit").val());
    var IssuedTo = LRTrim($("#piGridadvsearchIssuedTo").val());
    var TransactionDate = LRTrim($("#piGridadvsearchDate").val());
    var TransactionQuantity = LRTrim($("#piGridadvsearchQuantity").val());
    var Cost = LRTrim($("#piGridadvsearchCosts").val());
    if ($(document).find('#partIssuesTable').hasClass('dataTable')) {
        dtPartIssueTable.destroy();
    }
    dtPartIssueTable = $("#partIssuesTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_PartIssued",
            "type": "POST",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.srcData = srcData;
                d.PartClientLookupId = PartClientLookupId;
                d.Description = Description;
                d.ChargeToClientLookupId = ChargeToClientLookupId;
                d.UnitofMeasure = UnitofMeasure;
                d.IssuedTo = IssuedTo;
                d.TransactionDate = TransactionDate;
                d.TransactionQuantity = TransactionQuantity;
                d.Cost = Cost;
            },
            "datatype": "json",
            "dataSrc": function (json) {
                return json.data;
            }
        },
        "columns":
            [
                { "data": "PartClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "TransactionDate", "type": "date ", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ChargeToClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "TransactionQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "UnitofMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Cost", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "IssuedTo", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
function generateSelectChildrenTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();

    if ($(document).find('#searchTable3').hasClass('dataTable')) {
        SelectChildrenTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    SelectChildrenTable = $("#searchTable3").DataTable({
        columnDefs: [{
            "data": "EquipmentId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="isSelect" value="'
                    + $('<div/>').text(data).html() + '">';
            }
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        colReorder: true,
        rowGrouping: true,
        searching: true,
        'bPaginate': true,
        "bProcessing": true,
        dom: 'rtip',
        "pagingType": "full_numbers",
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        buttons: [],
        "filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetAllFreeEquipmentList",
            data: { EquipmentId: EquipmentId },
            "type": "GET",
            "datatype": "json",
        },
        "columns":
            [
                {},
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },

            ],
        initComplete: function () {
            $("#equipChildGridModal").modal('show');
            mcxDialog.closeLoading();
            SetPageLengthMenu();


            $('#searchTable3 tfoot th').each(function (i, v) {
                if (i >= 0) {
                    var colIndex = i;
                    var title = $('#searchTable3 thead th').eq($(this).index()).text();
                    $(this).html('<input type="text" class="popupSearch" id="colindex_' + colIndex + '"  /><i class="fa fa-search dropSearchIcon"></i>');
                }
            });
            $(document).ready(function () {
                $('#searchTable3').find('.popupSearch').on('keyup', function (e) {
                    if (e.keyCode === 13) {
                        var thisId = $(this).attr('id');
                        var colIdx = thisId.split('_')[1];
                        var searchText = LRTrim($(this).val());
                        SelectChildrenTable.column(colIdx)
                            .search(searchText)
                            .draw();
                    }
                });
            });
            return;
        }
    });
}


function generateTechSpecsTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    var techSpecsSecurity;
    if ($(document).find('#techSpecsTable').hasClass('dataTable')) {
        techSpecsTable.destroy();
    }
    techSpecsTable = $("#techSpecsTable").DataTable({
        responsive: true,
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
            "url": "/Equipment/GetEquipment_TechSpecs",
            "type": "POST",
            data: { EquipmentId: EquipmentId },
            "datatype": "json",
            "cache": false,
            "dataSrc": function (json) {
                rCount = json.data.length;
                techSpecsSecurity = json.techSpecsSecurity;
                return json.data;
            }
        },
        columnDefs: [
            {
                targets: [4], render: function (a, b, data, d) {
                    if (techSpecsSecurity == true) {
                        return '<a class="btn btn-outline-primary addBtnTechSpecs gridinnerbutton" title="Add"><i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editBttnTechSpecs gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger delBtnTechSpecs gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                    else {
                        return "";
                    }
                }
            }
        ],
        "columns":
            [
                { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SpecValue", "autoWidth": true, "bSearchable": true, "bSortable": true },
                //{ "data": "UnitOfMeasure", "autoWidth": true, "bSearchable": true, "bSortable": true },     /*V2-295*/
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                { "data": "Comments", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    data: "TechSpecId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
                }
            ],
        initComplete: function () {
            if (techSpecsSecurity == true) {
                var column = this.api().column(4);
                column.visible(true);
            }
            else {
                var column = this.api().column(4);
                column.visible(false);
            }
            if (rCount > 0 || techSpecsSecurity == false) { $("#btnAddTechSpecs").hide(); }
            else {
                $("#btnAddTechSpecs").show();
            }
            SetPageLengthMenu();
        }
    });
}
function generateSensorPopupTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    if ($(document).find('#sensorSearchTable').hasClass('dataTable')) {
        SensorPopupTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    SensorPopupTable = $("#sensorSearchTable").DataTable({
        columnDefs: [{
            "data": "SensorID",
            orderable: false,
            className: 'select-checkbox',
            targets: 0,
            'render': function (data, type, full, meta) {
                return '<input type="checkbox" name="id[]" data-eqid="' + data + '" class="isSelect" value="'
                    + $('<div/>').text(data).html() + '">';
            }
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [[1, 'asc']],
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        sDom: 'Btlipr',
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        buttons: [],
        //"filter": true,
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/BindSensorList",
            "type": "GET",
            data: { EquipmentId: EquipmentId },
            "datatype": "json"
        },
        "columns":
            [
                { className: 'text-center' },
                { "data": "SensorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "SensorID", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            mcxDialog.closeLoading();
            $('#mySensorModal').modal('show');
            SetPageLengthMenu();
        }
    });
}
function clearDropzone() {
    deleteServer = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        Dropzone.forElement("div#dropzoneForm").destroy();
    }
}
$(document).on('click', '.setImage', function () {
    var imageName = $(this).data('image');
    var EquimentId = $('#EquipData_EquipmentId').val();
    $.ajax({
        url: '../base/SaveUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: EquimentId, TableName: "Equipment", AttachObjectName: "Equipment" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                $('#EquipZoom').attr('src', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                $(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
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
                $("#EquipZoom").on('load', function () {
                    CloseLoader();
                    ShowImageSaveSuccessAlert();
                });
            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);
            }
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#region Children
$(document).on('click', '.delBtnchildren', function () {
    var data = dtChildrenTable.row($(this).parents('tr')).data();
    DeleteChildRow(data.EquipmentId);
});
$(document).on('click', '#btnAddChildren', function () {
    var eqIDs = SelectChildrenTable.column(0).nodes().to$().map(function () {
        if ($(this).find('.isSelect').is(':checked')) {
            return $(this).find('.isSelect').data('eqid');
        }
    }).get().join(',');
    var EquipmentId = $('#EquipData_EquipmentId').val();
    if (eqIDs !== "") {
        $.ajax({
            url: '/Equipment/AddSelectedChildren',
            data: { ListofIds: eqIDs, EquipmentId: EquipmentId },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
            },
            complete: function () {
                CloseLoader();
                dtChildrenTable.state.clear();
                generateChildrenDataTable();
                $("#equipChildGridModal").modal('hide');
            }
        });
    }
    else {
        swal({
            title: getResourceValue("NoRowsSelectAlert"),
            text: getResourceValue("SelectChildrenAlerts"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
function DeleteChildRow(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteChild',
            data: { _eqid: eqid },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    dtChildrenTable.state.clear();
                    ShowDeleteAlert(getResourceValue("childDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateChildrenDataTable();
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Down Time
$(document).on('click', '.addEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    AddDowntime(data.EquipmentId);
});
$(document).on('click', '.editEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    EditDowntime(data.DowntimeId);
});
$(document).on('click', '.deleteEquipmentDownTimeBttn', function () {
    var data = dtDownTimeTable.row($(this).parents('tr')).data();
    DeleteDowntime(data.DowntimeId);
});
function AddDowntime(eqid) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/RedirectDowntime',
        data: { EquipmentId: eqid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "POST",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        }
    });
}
function DownTimeAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("alertDownTimeAdded");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "downtime");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnAddDowntimeCancel,#btnEditDowntimeCancel", function () {
    var EquimentId = $('#downTimeModel_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToEquipmentDetail(EquimentId, "downtime");
    });
});
function EditDowntime(DowntimeId) {
    var EquimentId = $('#EquipData_EquipmentId').val();
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/ShowDownTimeEdit',
        data: { EquipmentId: EquimentId, DownTimeId: DowntimeId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        },
        error: function (jqXHR, exception) {
        }
    });
}
function DeleteDowntime(DowntimeId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DownTimeDelete',
            data: {
                _DowntimeId: DowntimeId
            },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("downtimeDeleteSuccessAlert"));
                    dtDownTimeTable.state.clear;
                }
                else {
                    dtDownTimeTable.destroy();
                    generateDowntimeDataTable();
                    return;
                }
            },
            complete: function () {
                generateDowntimeDataTable();
                CloseLoader();
            }
        });
    });
}
function DownTimeEditOnSuccess(data) {
    SuccessAlertSetting.text = getResourceValue("UpdateDowntimeAlerts");
    if (data.Result == "success") {
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "downtime");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Photos
$(document).on('click', '#deleteImg', function () {
    var EquimentId = $('#EquipData_EquipmentId').val();
    var ClientOnPremise = $('#EquipmentSummaryModel_ClientOnPremise').val();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseImage(EquimentId);
    }
    else {
        DeleteAzureImage(EquimentId);
    }

});

function DeleteOnPremiseImage(EquimentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteImageFromOnPremise',
            type: 'POST',
            data: { _EquimentId: EquimentId, TableName: "Equipment", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureImage(EquimentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteImageFromAzure',
            type: 'POST',
            data: { _EquimentId: EquimentId, TableName: "Equipment", Profile: true, Image: true },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToEquipmentDetail(EquimentId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Tech Spec
$(document).on('click', '.addBtnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    AddTechSpecs(data.TechSpecId, data.EquipmentId, data.Equipment_TechSpecsId, data.updatedindex, "add");
});
$(document).on('click', '.editBttnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    AddTechSpecs(data.TechSpecId, data.EquipmentId, data.Equipment_TechSpecsId, data.updatedindex, "update");
});
$(document).on('click', '.delBtnTechSpecs', function () {
    var data = techSpecsTable.row($(this).parents('tr')).data();
    DeleteTechSpecs(data.Equipment_TechSpecsId);
});
function TechSpecAddOnSuccess(data) {
    if (data.Result == "success") {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("AddTechspecsAlerts");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("UpdateTechspecsAlerts");
        }
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "techspec");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function AddTechSpecs(techSpecId, eqid, equipTechSpecId, updatedindex, mode) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/AddTechSpecs',
        data: { EquipmentId: eqid, TechMode: mode, TechSpecId: equipTechSpecId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        },
        error: function () {
        }
    });
}
function DeleteTechSpecs(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/TechSpecsDelete',
            data: {
                _EquipmentTechSpecsId: eqid
            },
            type: "POST",
            beforeSend: function () {
                ShowLoader();
            },
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    techSpecsTable.state.clear;
                    ShowDeleteAlert(getResourceValue("techspecsDeleteSuccessAlert"));
                }
                else {
                    techSpecsTable.state.clear;
                    generateTechSpecsTable();
                    return;
                }
            },
            complete: function () {
                generateTechSpecsTable();
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Attachment
$(document).on('click', '.delAttchBttn', function () {
    var data = dtAttachmentTable.row($(this).parents('tr')).data();
    DeleteEquipmentAttachment(data.FileAttachmentId);
});
function DeleteEquipmentAttachment(fileAttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteAttachment',
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
                    dtAttachmentTable.state.clear();
                    ShowDeleteAlert(getResourceValue("attachmentDeleteSuccessAlert"));
                }
                else {
                    ShowErrorAlert(data.Message);
                }
            },
            complete: function () {
                generateAttachmentDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btnAddAttachment", function () {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: "/Equipment/ShowAddAttachment",
        type: "GET",
        dataType: 'html',
        data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function generateAttachmentDataTable() {
    var equipmentid = $(document).find('#EquipData_EquipmentId').val();
    if ($(document).find('#AttachTable').hasClass('dataTable')) {
        dtAttachmentTable.destroy();
    }
    var visibility;
    var attchCount = 0;
    dtAttachmentTable = $("#AttachTable").DataTable({
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

            "url": "/Equipment/PopulateAttachment?EquipmentId=" + equipmentid,
            "type": "post",
            "datatype": "json",
            "dataSrc": function (response) {
                attchCount = response.recordsTotal;
                visibility = response.Attachsecurity;
                if (attchCount > 0) {
                    $(document).find('#asstAttachmentCount').show();
                    $(document).find('#asstAttachmentCount').html(attchCount);
                }
                else {
                    $(document).find('#asstAttachmentCount').hide();
                }
                return response.data;
            }
        },
        columnDefs: [
            {
                targets: [5], render: function (a, b, data, d) {
                    if (visibility == true) {
                        return '<a class="btn btn-outline-danger delAttchBttn gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                }
            }
        ],
        "columns":
            [
                {
                    "data": "Subject", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    }
                },
                {
                    "data": "FileName",
                    "autoWidth": true,
                    "bSearchable": true,
                    "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_download_attachment href="javascript:void(0)"  target="_blank">' + row.FullName + '</a>';
                    }
                },
                { "data": "FileSizeWithUnit", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "OwnerName", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "type": "date " },
                { "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center" }
            ],
        initComplete: function () {
            if (visibility == false) {
                $("#btnAddAttachment").hide();
                var column = this.api().column(5);
                column.visible(false);
            }
            else {
                $("#btnAddAttachment").show();
                var column = this.api().column(5);
                column.visible(true);
            }
            SetPageLengthMenu();
        }
    });
}
$(document).on('submit', "#frmeqpattachmentadd", function (e) {
    e.preventDefault();
    var form = document.querySelector('#frmeqpattachmentadd');
    if (!$('#frmeqpattachmentadd').valid()) {
        return;
    }
    var data = new FormData(form);
    $.ajax({
        type: "POST",
        url: "/Equipment/AddAttachment",
        data: data,
        processData: false,
        contentType: false,
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                var EquipmentId = data.equipmentid;
                if (data.IsduplicateAttachmentFileExist) {
                    ShowErrorAlert(getResourceValue("AttachmentFileExistAlerts"));
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("AddAttachmentAlerts");
                    swal(SuccessAlertSetting, function () {
                        RedirectToEquipmentDetail(EquipmentId, "attachment");
                    });
                }

            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });
});
$(function () {
    $(document).on('click', "#btnattachmentcancel", function () {
        var equipmentid = $(document).find('#attachmentModel_EquipmentId').val();
        swal(CancelAlertSetting, function () {
            RedirectToEquipmentDetail(equipmentid, "attachment");
        });
    });
});
$(document).on('click', '.lnk_download_attachment', function (e) {
    e.preventDefault();
    var row = $(this).parents('tr');
    var data = dtAttachmentTable.row(row).data();
    var FileAttachmentId = data.FileAttachmentId;
    $.ajax({
        type: "post",
        url: '/Base/IsOnpremiseCredentialValid',
        success: function (data) {
            if (data === true) {
                window.location = '/Equipment/DownloadAttachment?_fileinfoId=' + FileAttachmentId;
            }
            else {
                ShowErrorAlert(getResourceValue("NotAuthorisedDownloadFileAlert"));
            }
        }

    });

});
//#endregion
//#region Parts
$(document).on('click', '.addBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    AddParts(data.EquipmentId);
});
$(document).on('click', '.editBttnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    EditParts(data.EquipmentId, data.Equipment_Parts_XrefId, encodeURIComponent(data.Part_ClientLookupId), data.QuantityNeeded, data.QuantityUsed, (encodeURIComponent(data.Comment)).replace(/%20/g, "&#32;"), data.UpdatedIndex, "update");
});
$(document).on('click', '.delBtnParts', function () {
    var data = dtPartsTable.row($(this).parents('tr')).data();
    DeleteParts(data.Equipment_Parts_XrefId);
});
function AddParts(eqid) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/PartsAdd',
        data: { EquipmentId: eqid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "json",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        }
    });
}
$(document).on('click', '#Partsaddcancelbutton,#partseditcancelbutton', function (e) {
    var equipmentid = $(document).find('#partsSessionData_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToEquipmentDetail(equipmentid, "parts");
    });
});
function PartAddOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("AddPartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "parts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EditParts(eqid, Equipment_Parts_XrefId, Part_ClientLookupId, QuantityNeeded, QuantityUsed, Comment, UpdatedIndex) {
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var Name = $('#EquipData_Name').val();
    var Status = $('#EquipData_Status').val();
    var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/PartsEdit',
        data: { EquipmentId: eqid, Equipment_Parts_XrefId: Equipment_Parts_XrefId, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService, Status: Status },
        type: "GET",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        }
    });
};
function PartsEditOnSuccess(data) {
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdatePartsAlerts");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.equipmentid, "parts");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function DeleteParts(eqid) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/PartsDelete',
            data: { _EquipmentPartSpecsId: eqid },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.Result == "success") {
                    ShowDeleteAlert(getResourceValue("partDeleteSuccessAlert"));
                }
            },
            complete: function () {
                dtPartsTable.state.clear;
                generatePartsDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', '.link_part_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtPartsTable.row(row).data();
    var partId = data.PartId;
    var equipmentId = data.EquipmentId; //V2-1007
    clearDropzone();
    var UseMultiStoreroom = $(document).find('#UseMultiStoreroom').val();
    if (UseMultiStoreroom == 'False')
        window.location.href = "../Parts/PartsDetailFromEquipment?partId=" + partId + '&equipmentId=' + equipmentId; //V2-1007
    else
        window.location.href = "../MultiStoreroomPart/MultiStoreroomPartsDetailFromEquipment?partId=" + partId + '&equipmentId=' + equipmentId; //V2-1007
        
});

$(document).on('click', "#partsidebarCollapse", function () {
    $('#partadvsearchcontainer').find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
function PartGridAdvanceSearch() {
    generatePartsDataTable();
    $('.partitemcount').text(filteritemcount);
}
function PartGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find('#advsearchsidebar').find('input:text').val('');
    $(document).find('.partitemcount').text(filteritemcount);
    $("#StockType").val("").trigger('change');
    $(document).find('#partadvsearchfilteritems').find('span').html('');
    $(document).find('#partadvsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '#partClearAdvSearchFilter', function () {
    filteritemcount = 0;
    dtPartsTable.state.clear();
    PartGridclearAdvanceSearch();
    PartGridAdvanceSearch();
});

$(document).on('click', "#btnPartDataAdvSrch", function (e) {
    dtPartsTable.state.clear();
    var searchitemhtml = "";
    filteritemcount = 0

    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            filteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPart" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#partadvsearchfilteritems').html(searchitemhtml);
    $('#partadvsearchcontainer').find('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    PartGridAdvanceSearch();
});

$(document).on('click', '.btnCrossPart', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    filteritemcount--;
    PartGridAdvanceSearch();
});
//#endregion
//#region PMList
$(document).on('click', '.btnPmListEdit', function () {
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    var row = $(this).parents('tr');
    var data = dtPMTable.row(row).data();
    var PrevMasterId = data.PrevMaintMasterId;
    clearDropzone();
    window.location.href = "../PreventiveMaintenance/DetailFromEquipment?PrevMaintMasterId=" + PrevMasterId + "&EquipmentId=" + equipmentid + "&EquipmentClientLookupId=" + ClientlookUpId;
});
//#endregion
//#region WOActive
function generateWOActiveDataTable() {
    var srcData = LRTrim($("#txtWTAsearchbox").val());
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var ClientLookupId = LRTrim($("#wgridActiveadvsearchWorkOrderId").val());
    var Description = LRTrim($("#wgridActiveadvsearchDescription").val());
    var WorkAssigned_PersonnelClientLookupId = LRTrim($("#wgridActiveadvsearchWorkAssigned").val());
    var Status_Display = LRTrim($("#wgridActiveadvsearchStatus").val());
    var Type = LRTrim($("#wgridActiveadvsearchType").val());
    var CreateDate = LRTrim($('#wgridActiveadvsearchCreateDate').val());
    if ($(document).find('#woActiveTable').hasClass('dataTable')) {
        dtWOActiveTable.destroy();
    }
    dtWOActiveTable = $("#woActiveTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_WOActive",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.srcData = srcData;
                d.ClientLookupId = ClientLookupId;
                d.Description = Description;
                d.WorkAssigned_PersonnelClientLookupId = WorkAssigned_PersonnelClientLookupId;
                d.Status_Display = Status_Display;
                d.Type = Type;
                d.CreateDate = CreateDate;
            },
            "dataSrc": function (json) {

                $("#wgridActiveadvsearchStatus").empty();
                $("#wgridActiveadvsearchStatus").append("<option value=''>" + "--Select--" + "</option>");
                var status = [];
                for (var key in json.data) {
                    if (status.indexOf(json.data[key].Status_Display) == -1) {
                        status.push(json.data[key].Status_Display);
                    }
                }
                for (statusVal in status) {
                    var name = status[statusVal];
                    $("#wgridActiveadvsearchStatus").append("<option value='" + name + "'>" + getStatusValue(name) + "</option>");
                }
                if (Status_Display) {
                    $("#wgridActiveadvsearchStatus").val(Status_Display);
                }

                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_workorderactive href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "WorkAssigned_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnWOActiveDataAdvSrch", function (e) {
    dtWOActiveTable.state.clear();
    var searchitemhtml = "";
    woaGridfilteritemcount = 0;
    $('#txtWTAsearchbox').val('');
    $('#advsearchsidebarWoActive').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            woaGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWOActive" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#woactiveadvsearchfilteritems').html(searchitemhtml);
    $('#woactiveadvsearchcontainer').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    WOAGridAdvanceSearch();
});
$(document).on('click', '.btnCrossWOActive', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    woaGridfilteritemcount--;
    WOAGridAdvanceSearch();
}); 
$(document).on('click', '#woactiveliClearAdvSearchFilter', function () {
    $("#txtWTAsearchbox").val("");
    dtWOActiveTable.state.clear();
    WOAGridclearAdvanceSearch();
    generateWOActiveDataTable();
});
function WOAGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find('#advsearchsidebarWoActive').find('input:text').val('');
    $(document).find('.woactivefilteritemcount').text(filteritemcount);
    $(document).find('#wgridActiveadvsearchStatus').val('').trigger('change.select2');
    $(document).find('#woactiveadvsearchfilteritems').find('span').html('');
    $(document).find('#woactiveadvsearchfilteritems').find('span').removeClass('tagTo');
}
function WOAGridAdvanceSearch() {
    generateWOActiveDataTable();
    $('.woactivefilteritemcount').text(woaGridfilteritemcount);
}
$(document).on('click', "#woactivesidebarCollapse", function () {
    $('#woactiveadvsearchcontainer').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#btnWoActiveSearch", function () {
    WOAGridclearAdvanceSearch()
    dtWOActiveTable.state.clear();
    generateWOActiveDataTable();
});
$(document).on('click', '.lnk_workorderactive', function (e) {
    var row = $(this).parents('tr');
    var data = dtWOActiveTable.row(row).data();
    var workorderId = data.WorkOrderId;
    clearDropzone();
    localStorage.setItem("equipmentSearchstatus", titleText);
    localStorage.setItem("workorderstatus", '0');
    localStorage.setItem("workorderstatustext", getResourceValue("spnWorkOrder"));
    window.location.href = "../WorkOrder/DetailFromEquipment?workOrderId=" + workorderId;
});
//#endregion
//#region WOComplete
function generateWOCompleteTable() {
    var srcData = LRTrim($("#txtWTOsearchbox").val());
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var ClientLookupId = LRTrim($("#wgridCompleteadvsearchWorkOrderId").val());
    var Description = LRTrim($("#wgridCompleteadvsearchDescription").val());
    var WorkAssigned_PersonnelClientLookupId = LRTrim($("#wgridCompleteadvsearchWorkAssigned").val());
    var Status_Display = LRTrim($("#wgridCompleteadvsearchStatus").val());
    var Type = LRTrim($("#wgridCompleteadvsearchType").val());
    var CreateDate = LRTrim($('#wgridCompleteadvsearchCreateDate').val());
    if ($(document).find('#woCompleteTable').hasClass('dataTable')) {
        dtWOCompleteTable.destroy();
    }
    dtWOCompleteTable = $("#woCompleteTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetEquipment_WOComplete",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.EquipmentId = EquipmentId;
                d.srcData = srcData;
                d.ClientLookupId = ClientLookupId;
                d.Description = Description;
                d.WorkAssigned_PersonnelClientLookupId = WorkAssigned_PersonnelClientLookupId;
                d.Status_Display = Status_Display;
                d.Type = Type;
                d.CreateDate = CreateDate;

            },
            "dataSrc": function (json) {
                $("#wgridCompleteadvsearchStatus").empty();
                $("#wgridCompleteadvsearchStatus").append("<option value=''>" + "--Select--" + "</option>");
                var status = [];
                for (var key in json.data) {
                    if (status.indexOf(json.data[key].Status_Display) == -1) {
                        status.push(json.data[key].Status_Display);
                    }
                }
                for (statusVal in status) {
                    var name = status[statusVal];
                    $("#wgridCompleteadvsearchStatus").append("<option value='" + name + "'>" + getStatusValue(name) + "</option>");
                }
                if (Status_Display) {
                    $("#wgridCompleteadvsearchStatus").val(Status_Display);
                }
                return json.data;
            }
        },
        "columns":
            [
                {
                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    "mRender": function (data, type, row) {
                        return '<a class=lnk_workordercomplete href="javascript:void(0)">' + data + '</a>';
                    }
                },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "WorkAssigned_PersonnelClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    render: function (data, type, row, meta) {

                        if (data == statusCode.Approved) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--yellow m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Canceled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--orange m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Complete) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Denied) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--purple m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.Scheduled) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--blue m-badge--wide'>" + getStatusValue(data) + "</span >";
                        }
                        else if (data == statusCode.WorkRequest) {
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide' style='width:95px;' >" + getStatusValue(data) + "</span >";
                        }
                        else {
                            return getStatusValue(data);
                        }
                    }
                },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "CreateDate", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('click', "#btnWOCompleteDataAdvSrch", function (e) {
    dtWOCompleteTable.state.clear();
    var searchitemhtml = "";
    wocGridfilteritemcount = 0;
    $('#txtWTOsearchbox').val('');
    $('#advsearchsidebarWoComplete').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            wocGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossWOComplete" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#wocompletesearchfilteritems').html(searchitemhtml);
    $('#wocompleteadvsearchcontainer').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    WOCGridAdvanceSearch();
});
$(document).on('click', '.btnCrossWOComplete', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    wocGridfilteritemcount--;
    WOCGridAdvanceSearch();
});
$(document).on('click', '#wocompleteClearAdvSearchFilter', function () {
    $("#txtWTOsearchbox").val("");
    dtWOCompleteTable.state.clear();
    WOCGridclearAdvanceSearch();
    generateWOCompleteTable();
});
function WOCGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find('#advsearchsidebarWoComplete').find('input:text').val('');
    $(document).find('.wocompletefilteritemcount').text(filteritemcount);
    $(document).find('#wgridCompleteadvsearchStatus').val('').trigger('change.select2');
    $(document).find('#wocompletesearchfilteritems').find('span').html('');
    $(document).find('#wocompletesearchfilteritems').find('span').removeClass('tagTo');
}
function WOCGridAdvanceSearch() {
    generateWOCompleteTable();
    $('.wocompletefilteritemcount').text(wocGridfilteritemcount);
}
$(document).on('click', "#wocompletesidebarCollapse", function () {
    $('#wocompleteadvsearchcontainer').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#btnwocompletesearch", function () {
    WOCGridclearAdvanceSearch()
    dtWOCompleteTable.state.clear();
    generateWOCompleteTable();
});
$(document).on('click', '.lnk_workordercomplete', function (e) {
    var row = $(this).parents('tr');
    var data = dtWOCompleteTable.row(row).data();
    var workorderId = data.WorkOrderId;
    localStorage.setItem("equipmentSearchstatus", titleText);
    localStorage.setItem("workorderstatus", '4');
    localStorage.setItem("workorderstatustext", getResourceValue("spnCompleteWorkOrder"));
    window.location.href = "../WorkOrder/DetailFromEquipment?workOrderId=" + workorderId;
});
//#endregion

//#region PartIssues
function PartIssuesGridclearAdvanceSearch() {
    var filteritemcount = 0;
    $(document).find('#advsearchsidebarPartIssues').find('input:text').val('');
    $(document).find('.partissuefilteritemcount').text(filteritemcount);
    $(document).find('#partissuesearchfilteritems').find('span').html('');
    $(document).find('#partissuesearchfilteritems').find('span').removeClass('tagTo');
}
function PIGridAdvanceSearch() {
    generatePartIssueTable();
    $('.partissuefilteritemcount').text(piGridfilteritemcount);
}
$(document).on('click', '.btnCrossPartIssues', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    piGridfilteritemcount--;
    PIGridAdvanceSearch();
});
$(document).on('click', "#partIssuesidebarCollapse", function () {
    $('#partissueadvsearchcontainer').find('.sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#btnpartissuesearch", function () {
    PartIssuesGridclearAdvanceSearch();
    dtPartIssueTable.state.clear();
    generatePartIssueTable();
});
$(document).on('click', "#btnPartIssuesDataAdvSrch", function (e) {
    dtPartIssueTable.state.clear();
    var searchitemhtml = "";
    piGridfilteritemcount = 0
    $('#txtPartIssueSearchbox').val('');

    $('#advsearchsidebarPartIssues').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {
            piGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossPartIssues" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#partissuesearchfilteritems').html(searchitemhtml);
    $('#partissueadvsearchcontainer').find('.sidebar').removeClass('active');
    $('.overlay').fadeOut();
    PIGridAdvanceSearch();
});
$(document).on('click', '#partissueClearAdvSearchFilter', function () {
    $("#txtPartIssueSearchbox").val("");
    piGridfilteritemcount = 0;
    dtPartIssueTable.state.clear();
    PartIssuesGridclearAdvanceSearch();
    PIGridAdvanceSearch();
});
//#endregion

$(document).on('click', '.lithisequipment', function () {
    var equipmentid = $(this).attr('data-val');
    RedirectToEquipmentDetail(equipmentid, "equipment");
});
//#region Sensor Functions
$(document).on('click', '.addSensorBttn', function () {
    AddNewSensor();
});
$(document).on('click', '.editSensorBttn', function () {
    var data = dtSensorTable.row($(this).parents('tr')).data();
    EditSensor(data.EquipmentId, data.SensorId, data.Equipment_Sensor_XrefId);
});
$(document).on('click', '.delSensorBttn', function () {
    var data = dtSensorTable.row($(this).parents('tr')).data();
    DeleteSensor(data.EquipmentId, data.Equipment_Sensor_XrefId);
});
function AddNewSensor() {
    $('#sensorSearchTable tfoot th').each(function (i, v) {
        if (i > 0) {
            var colIndex = i;
            var title = $('#sensorSearchTable thead th').eq($(this).index()).text();
            $(this).html('<input type="text" class="popupSearch" placeholder="Search ' + title + '" id="colindex_' + colIndex + '" />');
        }
    });
    if ($(document).find('#sensorSearchTable').hasClass('dataTable')) {
        SensorPopupTable.destroy();
    }
    generateSensorPopupTable();
}
function EditSensor(equipmentid, sensorid, xref) {
    $.ajax({
        url: '/Equipment/ShowEditSensor',
        type: "GET",
        dataType: 'html',
        data: {
            EquipmentId: equipmentid,
            SensorId: sensorid,
            Equipment_Sensor_XrefId: xref
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
        },
        error: function (jqXHR, exception) {

            CloseLoader();
        }
    });
}
function SensorEditOnSuccess(data) {
    if (data.Result == "success") {
        var EquipmentId = data.equipmentid;
        SuccessAlertSetting.text = getResourceValue("SensorUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(EquipmentId, "sensor");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function DeleteSensor(EquipmentId, xref) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteSensor',
            data: {
                _xref: xref
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    dtSensorTable.state.clear();
                    ShowDeleteAlert(getResourceValue("sensorDeleteSuccessAlert"));
                }
            },
            complete: function () {
                generateSensorDataTable();
                CloseLoader();
            }
        });
    });
}
$(document).on('click', "#btncancelsensoredit", function () {
    var equipmentid = $(document).find('#sensorGridDataModel_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToEquipmentDetail(equipmentid, "sensor");
    });
});
//#endregion    
$(document).on('click', '.lnkNotesDetails', function () {
    var msg_details = $(this).next().val();
    $(document).find('#NotesContent').text(msg_details);
    $(document).find('#Noteslogdetail').modal('show');
});
var dtPartsXRefTable;
function generatePartsXRefDataTable() {
    var EquipmentId = $('#EquipData_EquipmentId').val();
    var rCount = 0;
    var visibility;
    if ($(document).find('#partsTable').hasClass('dataTable')) {
        dtPartsXRefTable.destroy();
    }
    dtPartsXRefTable = $("#partsTable").DataTable({
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
            url: dataTableLocalisationUrl,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetEquipmentPartsXrefGridData",
            data: { EquipmentId: EquipmentId },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
            [
                { "data": "CleintLookUpId", "autoWidth": true, "bSearchable": true, "bSortable": true },
                {
                    "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true,
                    mRender: function (data, type, full, meta) {
                        return "<div class='text-wrap width-400'>" + data + "</div>";
                    }
                },
                { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "ManufacturerID", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "StockType", "autoWidth": true, "bSearchable": true, "bSortable": true },
            ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

//#region   Remove from Asset availability
function RemoveAvailabilityUpdateOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("RemoveServiceAlerts");
        swal(SuccessAlertSetting, function () {
            $("#RemoveAssetAvailabilityModal").modal('hide');
            RedirectToEquipmentDetail(data.EquipmentId, "equipment");
            $('.modal-backdrop').hide();
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region Return to Asset Availability
$(document).on('click', '#btnAstReturn', function () {
    var EquipmentId = $(this).attr('data-equipid');
    $.ajax({
        type: "POST",
        url: "/Equipment/ReturnToservice",
        data: { EquipmentId: EquipmentId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.Result == "success") {
                SuccessAlertSetting.text = getResourceValue("ReturnToServiceAlerts");
                swal(SuccessAlertSetting, function () {
                    RedirectToEquipmentDetail(EquipmentId, "equipment");
                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (xhr) {
            CloseLoader();
        }
    });

});
//#endregion

//#region Update from Asset availability
function UpdateAvailabilityUpdateOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("UpdateAvailabilityAlerts");
        swal(SuccessAlertSetting, function () {
            $("#UpdateAssetAvailabilityModal").modal('hide');
            RedirectToEquipmentDetail(data.EquipmentId, "equipment");
            $('.modal-backdrop').hide();
        });

    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion

//#region Clear form
$(document).on('click', '.Cancelclearerr', function () {
    $(document).find('form').find("#ddlReasonCode").val("OutforRepair").trigger('change');
    $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
    $(document).find('form').find("select").removeClass("input-validation-error");
    $(document).find('form').trigger("reset");

});
//#endregion

//#region Repairable Spare

//#region Repairable Spare Wizard
var RepairableSpareAssetWizard = function () {

    var wizardEl = $('#m_wizard');
    var formEl = $('#formRepairableSpareAsset');
    var wizard;

    var initWizard = function () {
        wizard = wizardEl.mWizard({
            startStep: 1
        });

        wizard.on('beforeNext', function (wizard) {
            var isvalid = formEl.valid();
            if (isvalid === false) {
                if (wizard.currentStep == 1 && $('#AssetInformationStep').find('.input-validation-error').length == 0) {
                    return true;
                }
                else if (wizard.currentStep == 2 && $('#AssignmentStep').find('.input-validation-error').length == 0) {
                    return true;
                }
                else if (formEl.find('.input-validation-error').length > 0) {
                    return false;
                }
                return false;
            }
        });


        wizard.on('change', function (wizard) {

        });
    }

    return {
        // public functions
        init: function () {
            wizardEl = $('#m_wizard');
            formEl = $('#formRepairableSpareAsset');

            initWizard();
        }
    };
}();
//#endregion Wizard

$(document).on('click', '#RepairableSpareAction', function () {
    var eqID = $(this).data('id');
    $.ajax({
        url: "/Equipment/RepairableSpareAssetWizard",
        type: "POST",
        data: { 'EquipmentId': eqID },
        dataType: 'html',
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#RepairableSpareAssetDiv').html(data);
        },
        complete: function () {
            SetEquimentControls();
            RepairableSpareAssetWizard.init();

            $('#RepairableSpareAssetWizardModal').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        error: function () {
            CloseLoader();
        }
    });
});

$(document).on('hidden.bs.modal', '#RepairableSpareAssetWizardModal', function (e) {
    $('#RepairableSpareAssetDiv').html('');
});

$(document).on('change', '#RepairableSpareStatus', function () {
    var value = $('#RepairableSpareStatus :selected').val();
    if (value == "Unassigned") {
        $("#li_AssigntoAssetId").show();
        $("#li_Location").show();
        $(document).find("#hdnIsAssigned").val(false);
        // $(document).find('#AssigntoAssetId').val('');
    }
    else if (value == "Assigned") {
        $("#li_AssigntoAssetId").show();
        $("#li_Location").hide();
        $(document).find("#hdnIsAssigned").val(true);
        // $(document).find('#AssigntoAssetId').val('');
    }
    else {
        $("#li_AssigntoAssetId").hide();
        $("#li_Location").hide();
    }
    $(document).find('#RepairableSpareModel_AssignedAssetClientlookupId').val('');
    $(document).find('#RepairableSpareModel_AssignedAssetId').val('');
});
function RepairableSpareAssetOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        $('#RepairableSpareAssetWizardModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("RepairableSpareAssetAddAlert"); //'Repairable spare asset added successfully'; 
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.EquipmentId, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#RepairableAssetEdit", function () {
    var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
    //var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
    //var Name = $('#EquipData_Name').val();
    //var Status = $('#EquipData_Status').val();
    //var isRemoveFromService = $('#EquipData_RemoveFromService').val();
    $.ajax({
        url: '/Equipment/EditRepairableSpareAsset',
        data: { EquipmentId: equipmentid },
        type: "POST",
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
        },
        complete: function () {
            CloseLoader();
            SetEquimentControls();
           
        }
    });
});
function UpdateRepairableSpareAssetOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("RepairableSpareAssetUpdateAlert"); //'Repairable spare asset updated successfully';
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail(data.EquipmentId, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnCancelRepairableSpareEdit", function () {
    var equipmentid = $(document).find('#RepairableSpareModel_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToEquipmentDetail(equipmentid, "equipment");
    });
});
//#endregion

//#region Assign and UnAssign from details
$(document).on('click', '.openRepairableAsset', function (e) {
    var equipid = $(document).find('#EquipData_EquipmentId').val();
    $(document).find('#assignmentModel_EquipmentId').val(equipid);
    var Assigned = $(this).data('isassigned');
    if (Assigned) {
        $(document).find("#hdnIsAssigned").val(true);
        $(document).find("#RepairableSpareStatusAssign").val("Assigned");
    }
    else {
        $(document).find("#hdnIsAssigned").val(false);
        $(document).find("#RepairableSpareStatusAssign").val("Unassigned");
    }
    $('#assignmentModel_AssignedAssetClientlookupId').val('').removeClass('input-validation-error');
    $('#assignmentModel_AssignedAssetId').val('').removeClass('input-validation-error');
    $('#assignmentModel_Location').val('').removeClass('input-validation-error');
    $('#AssignUnAssignModal').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});

function AssignmentUpdateOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = "Assignment updated successfully.";//getResourceValue("EquipmentUpdateAlert");
        swal(SuccessAlertSetting, function () {
            $('#AssignUnAssignModal').modal('hide');
            RedirectToEquipmentDetail(data.EquipmentId, "equipment");
            $('.modal-backdrop').hide();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}


$(document).on('click', '#btnAssignmentViewLog', function () {
    generateAssignmentViewLogDataTable();
});
//#endregion


//#region Multiple Photo Upload
function CompressImage(files, imageName) {
    new Compressor(files, {
        quality: 0.6,
        convertTypes: ['image/png'],
        convertSize: 100000,
        // The compression process is asynchronous,
        // which means you have to access the `result` in the `success` hook function.
        success(result) {
            if (result.size < files.size) {
                SaveCompressedImage(result, imageName);
            }
            else {
                SaveCompressedImage(files, imageName);
            }
            console.log('file name ' + result.name + ' after compress ' + result.size);
        },
        error(err) {
            console.log(err.message);
        },
    });
}
function SaveCompressedImage(data, imageName) {
    var AddPhotoFileData = new FormData();
    AddPhotoFileData.append('file', data, imageName);
    $.ajax({
        url: '../base/SaveUploadedFile',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: AddPhotoFileData,
        success: function (result) {
            //alert(result);
            var EquimentId = $('#EquipData_EquipmentId').val();
            SaveMultipleUploadedFileToServer(EquimentId, imageName);

            $('#files').val('');
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('change', '#files', function () {
    var _isMobile = CheckLoggedInFromMob();
    var val = $(this).val();
    var imageName = val.replace(/^.*[\\\/]/, '')
    //console.log(imageName);
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    var fileExt = imageName.substr(imageName.lastIndexOf('.') + 1).toLowerCase();
    if (fileExt != 'jpeg' && fileExt != 'jpg' && fileExt != 'png' && fileExt != 'JPEG' && fileExt != 'JPG' && fileExt != 'PNG') {
        ShowErrorAlert(getResourceValue("spnValidImage"));
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else if (this.files[0].size > (1024 * 1024 * 10)) {
        ShowImageSizeExceedAlert();
        $('#files').val('');
        //e.preventDefault();
        return false;
    }
    else {
        //alert('Hello');
        swal(AddImageAlertSetting, function () {
            if (window.FormData !== undefined) {
                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    console.log('file name ' + files[i].name + ' before compress ' + files[i].size);
                    if (_isMobile == true) {
                        var EquimentId = $('#EquipData_EquipmentId').val();
                        var imgname = EquimentId + "_" + Math.floor((new Date()).getTime() / 1000);
                        CompressImage(files[i], imgname + "." + fileExt);
                    }
                    else {
                        CompressImage(files[i], imageName);
                    }

                    //fileData.append(files[i].name, files[i]);
                }
            }
            else {
                //alert("FormData is not supported.");
            }
        });
    }
});
function SaveMultipleUploadedFileToServer(EquimentId, imageName) {
    $.ajax({
        url: '../base/SaveMultipleUploadedFileToServer',
        type: 'POST',

        data: { 'fileName': imageName, objectId: EquimentId, TableName: "Equipment", AttachObjectName: "Equipment" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result == "0") {
                CloseLoader();
                ShowImageSaveSuccessAlert();
            }
            else if (data.result == "1") {
                CloseLoader();
                var errorMessage = getResourceValue("ImageExistAlert");
                ShowErrorAlert(errorMessage);

            }
            else {
                CloseLoader();
                var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
                ShowErrorAlert(errorMessage);

            }
        },
        complete: function () {
            LoadImages(EquimentId);
        },
        error: function () {
            CloseLoader();
        }
    });
}
//endregion

//#region Show Images
var cardviewstartvalue = 0;
var cardviewlwngth = 10;
var grdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var layoutTypeWO = 1;
function LoadImages(EquipmentId) {
    $.ajax({
        url: '/Equipment/GetImages',
        type: 'POST',
        data: {
            currentpage: grdcardcurrentpage,
            start: cardviewstartvalue,
            length: cardviewlwngth,
            EquipmentId: EquipmentId
        },
        beforeSend: function () {
            $(document).find('#imagedataloader').show();
        },
        success: function (data) {
            /*if (data.TotalCount > 0) {*/
            $(document).find('#ImageGrid').show();
            $(document).find('#EquipmentImages').html(data).show();
            $(document).find('#tblimages_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == grdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
            //}
            //else {
            //    $(document).find('#ImageGrid').hide();
            //}
        },
        complete: function () {
            $(document).find('#imagedataloader').hide();
            $(document).find('#cardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(cardviewlwngth).trigger('change.select2');
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('change', '#cardviewpagelengthdrp', function () {
    var EquimentId = $('#EquipData_EquipmentId').val();
    cardviewlwngth = $(this).val();
    grdcardcurrentpage = parseInt(cardviewstartvalue / cardviewlwngth) + 1;
    cardviewstartvalue = parseInt((grdcardcurrentpage - 1) * cardviewlwngth) + 1;
    LoadImages(EquimentId);

});
$(document).on('click', '#tblimages_paginate .paginate_button', function () {
    var EquimentId = $('#EquipData_EquipmentId').val();
    if (layoutTypeWO == 1) {
        var currentselectedpage = parseInt($(document).find('#tblimages_paginate .pagination').find('.active').text());
        cardviewlwngth = $(document).find('#cardviewpagelengthdrp').val();
        cardviewstartvalue = cardviewlwngth * (parseInt($(this).find('.page-link').text()) - 1);
        var lastpage = parseInt($(this).prev('li').data('currentpage'));

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage - 2);
            grdcardcurrentpage = grdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            cardviewstartvalue = cardviewlwngth * (currentselectedpage);
            grdcardcurrentpage = grdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            grdcardcurrentpage = 1;
            cardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            grdcardcurrentpage = parseInt($(this).prevAll('li').eq(1).text());
            cardviewstartvalue = cardviewlwngth * (grdcardcurrentpage - 1);
        }
        else {
            grdcardcurrentpage = $(this).data('currentpage');
        }
        LoadImages(EquimentId);

    }
    else {
        run = true;
    }
});
//#endregion
//#region Set As Default
$(document).on('click', '#selectidSetAsDefault', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var EquimentId = $('#EquipData_EquipmentId').val();
    $('.modal-backdrop').remove();
    $.ajax({
        url: '../base/SetImageAsDefault',
        type: 'POST',

        data: { AttachmentId: AttachmentId, objectId: EquimentId, TableName: "Equipment" },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.result === "success") {
                //$('.modal-backdrop').remove();
                //$('#' + AttachmentId).hide();
                $('#EquipZoom').attr('src', data.imageurl);
                $('#EquipZoom').attr('data-zoom-image', data.imageurl);
                $('.equipImg').attr('src', data.imageurl);
                //$(document).find('#AzureImage').append('<a href="javascript:void(0)" id="deleteImg" class="trashIcon" title="Delete"><i class="fa fa-trash"></i></a>');
                $('#EquipZoom').data('zoomImage', data.imageurl).elevateZoom(
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
                $("#EquipZoom").on('load', function () {
                    //LoadImages(EquimentId);
                    CloseLoader();
                    ShowImageSetSuccessAlert();
                });
                //RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                //ShowImageSaveSuccessAlert();
            }
            //else {
            //    CloseLoader();
            //    //var errorMessage = getResourceValue("NotAuthorisedUploadFileAlert");
            //    //ShowErrorAlert(errorMessage);

            //}
        },
        complete: function () {
            //CloseLoader();
            LoadImages(EquimentId);
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region Delete Image
$(document).on('click', '#selectidDelete', function () {
    var AttachmentId = $(this).attr('dataid');
    $('#' + AttachmentId).hide();
    var EquimentId = $('#EquipData_EquipmentId').val();
    var ClientOnPremise = $('#EquipmentSummaryModel_ClientOnPremise').val();
    $('.modal-backdrop').remove();
    if (ClientOnPremise == 'True') {
        DeleteOnPremiseMultipleImage(EquimentId, AttachmentId);
    }
    else {
        DeleteAzureMultipleImage(EquimentId, AttachmentId);
    }
});

function DeleteOnPremiseMultipleImage(EquimentId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromOnPremise',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: EquimentId, TableName: "Equipment" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    RedirectToEquipmentDetail(EquimentId, "OnPremiseImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
function DeleteAzureMultipleImage(EquimentId, AttachmentId) {
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '../base/DeleteMultipleImageFromAzure',
            type: 'POST',
            data: { AttachmentId: AttachmentId, objectId: EquimentId, TableName: "Equipment" },
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data === "success" || data === "not found") {
                    //LoadImages(EquimentId);
                    RedirectToEquipmentDetail(EquimentId, "AzureImageReload");
                    ShowImageDeleteSuccessAlert();
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion

