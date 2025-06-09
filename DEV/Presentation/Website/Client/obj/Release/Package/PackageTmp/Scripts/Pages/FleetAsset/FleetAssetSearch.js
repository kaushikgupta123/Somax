var dtTable;
var btnActiveStatus = false;
var selectCount = 0;
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var VehicletypeValFleetAsset;
var activeStatus;
var run = false;
var titleText = '';
var order = '0';
var orderDir = 'asc';
var AssetAvailability;
//Search Retention
var gridname = "FleetAsset_Search";
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}

$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    $('#sampleDatepicker').datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        yearRange: "-90:+00"
    });
    ShowbtnLoaderclass("LoaderDrop");
    $("#fleetassetGridAction :input").attr("disabled", "disabled");
    ShowbtnLoader("btnsortmenu");
    $("#action").click(function () {
        $(".actionDrop").slideToggle();
    });
    $(".actionDrop ul li a").click(function () {
        $(".actionDrop").fadeOut();
    });
    $("#action").focusout(function () {
        $(".actionDrop").fadeOut();
    });
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#dismiss, .overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    generateFleetAssetDataTable();
    $("#btnupdateequip").click(function () {
        $(".actionDrop2").slideToggle();
    });
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnupdateequip").focusout(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnFleetAssetDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        VehicletypeValFleetAsset = $("#ddlVehicleType").val();
        AssetAvailability = $("#ddlassetAvailability").val();
        FleetAssetAdvSearch();
        dtTable.page('first').draw('page');
    });
    $(document).on('click', '.link_fleetasset_detail', function (e) {
        e.preventDefault();
        titleText = $('#fleetassetsearchtitle').text();
        var index_row = $('#fleetassetSearch tr').index($(this).closest('tr')) - 1;
        var row = $(this).parents('tr');
        var td = $(this).parents('tr').find('td');
        var data = dtTable.row(row).data();
        $.ajax({
            url: "/FleetAsset/FleetAssetDetails",
            type: "POST",
            dataType: "html",
            beforeSend: function () {
                ShowLoader();
            },
            data: { EquipmentId: data.EquipmentId },
            success: function (data) {

                $('#fleetassetmaincontainer').html(data);
                $(document).find('#spnlinkToSearch').text(titleText);
            },
            complete: function () {
                LoadComments(data.EquipmentId);
                SetFleetAssetDetailEnvironment();
            }
        });
    });
    $(document).on('click', "ul.vtabs li", function () {
        $(document).find("ul.vtabs li").removeClass("active");
        $(document).find(this).addClass("active");
        $(document).find(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(document).find(activeTab).fadeIn();
        return false;
    });
    $(document).on('click', "#btnCancelAddFleetAsset", function () {
        swal(CancelAlertSetting, function () {
            window.location.href = "../FleetAsset/Index?page=Fleet_asset";
        });
    });
    $(document).on('click', "#btnfleetasseteditcancel", function () {
        var equipmentid = $(document).find('#FleetAssetModel_EquipmentID').val();
        swal(CancelAlertSetting, function () {
            RedirectToFleetAssetDetail(equipmentid, "equipment");
        });
    });
    $(document).on('click', '.AddFleetAsset', function () {
        $.ajax({
            url: "/FleetAsset/AddFleetAsset",
            type: "GET",
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#fleetassetmaincontainer').html(data);
            },
            complete: function () {
                SetFleetAssetControls();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
    $(document).on('click', "#fleetassetedit", function () {

        var equipmentid = LRTrim($(document).find('#FleetAssetModel_EquID').val());
        var ClientlookUpId = $(document).find('#FleetAssetModel_ClientLookupId').val();
        var Name = $('#FleetAssetModel_Name').val();
        $.ajax({
            url: '/FleetAsset/EditFleetAsset',
            data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name },
            type: "POST",
            datatype: "html",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#fleetassetmaincontainer').html(data);
                IsMeterTypeContainsValue();
            },
            complete: function () {
                CloseLoader();
                $.validator.setDefaults({ ignore: '.ignorevalidation' });
                $.validator.unobtrusive.parse(document);
                $('.select2picker, form').change(function () {
                    var areaddescribedby = $(this).attr('aria-describedby');
                    if (typeof areaddescribedby != 'undefined') {
                        $('#' + areaddescribedby).show();
                    }
                });
                ZoomImage($(document).find('#EquipZoom'));
                $(document).find('.select2picker').select2({});
                $(document).find('.dtpicker').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    "dateFormat": "mm/dd/yy",
                    autoclose: true
                }).inputmask('mm/dd/yyyy');
                SetFixedHeadStyle();
                $("form").submit(function () {
                    var activetagid = $('.vtabs li.active').attr('id');
                    if (activetagid == 'equipmentabwarrantytab') {
                        $('#equipmentabdidtab').trigger('click');
                    }
                });
            }
        });
    });

    function IsMeterTypeContainsValue() {
        var meter1Type = $("#FleetAssetModel_Meter1Type").val();
        if (meter1Type) {
            $(document).find('#FleetAssetModel_Meter1Units').removeAttr('disabled');
            $(document).find('#FleetAssetModel_Meter2Type').removeAttr('disabled');
        }
        var meter2Type = $("#FleetAssetModel_Meter2Type").val();
        if (meter2Type) {
            $(document).find('#FleetAssetModel_Meter2Units').removeAttr('disabled');
        }
    }

    $(document).on('click', "#btnSaveAnotherOpen,#btnSave", function () {
        if ($(document).find("form").valid()) {
            checkDivContainsData();
            return;
        }
        else {

            var errorTab = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
            if (errorTab == "Identifications") {
                $('#equipmentabdidtab').trigger('click');
            }
            else if (errorTab == "Dimensions") {
                $('#dimentiontab').trigger('click');
            }
            else if (errorTab == "EngineandTransmission") {
                $('#engineandTransmissiontab').trigger('click');
            }
            else if (errorTab == "WheelsandTires") {
                $('#wheelsandTirestab').trigger('click');
            }
            else if (errorTab == "Fluids") {
                $('#fluidstab').trigger('click');
            }
            else if (errorTab == "Setup") {
                $('#setuptab').trigger('click');
            }

        }
    });
    $(document).on('click', "#btneditfleetasset", function () {
        if ($(document).find("form").valid()) {
            checkDivContainsData();
            return;
        }
        else {

            var errorTab = $(document).find(".input-validation-error").parents('div:eq(0)').attr('id');
            if (errorTab == "Identifications") {
                $('#equipmentabdidtab').trigger('click');
            }
            else if (errorTab == "Dimensions") {
                $('#dimentiontab').trigger('click');
            }
            else if (errorTab == "EngineandTransmission") {
                $('#engineandTransmissiontab').trigger('click');
            }
            else if (errorTab == "WheelsandTires") {
                $('#wheelsandTirestab').trigger('click');
            }
            else if (errorTab == "Fluids") {
                $('#fluidstab').trigger('click');
            }
        }
    });
    //From FleetAssetDimention
    var isfleetDimensionData = false;
    var isfleetEngineData = false;
    var isfleetFluidsData = false;
    var isfleetWheelData = false;
    function checkDivContainsData() {
        isfleetDimensionData = false;
        isfleetEngineData = false;
        isfleetFluidsData = false;
        isfleetWheelData = false;

        $('.flt_dim').each(function () {
            if (this.value != "") {
                isfleetDimensionData = true;
                return;
            }
        });
        if (isfleetDimensionData == false) {
            $('.flt_dimdec').each(function () {
                if (this.value != "0") {
                    isfleetDimensionData = true;
                    return;
                }
            });
        }

        //From FleetAssetEngin
        $('.flt_eng').each(function () {
            if (this.value != "") {
                isfleetEngineData = true;
                return;
            }
        });
        if (isfleetEngineData == false) {
            $('.flt_engdec').each(function () {
                if (this.value != "0") {
                    isfleetEngineData = true;
                    return;
                }
            });
        }

        //From FleetAssetFluid
        $('.flt_fue').each(function () {
            if (this.value != "") {
                isfleetFluidsData = true;
                return;
            }
        });
        if (isfleetFluidsData == false) {
            $('.flt_fuedec').each(function () {
                if (this.value != "0") {
                    isfleetFluidsData = true;
                    return;
                }
            });
        }

        //From FleetAssetWheel
        $('.flt_whe').each(function () {
            if (this.value != "") {
                isfleetWheelData = true;
                return;
            }
        });
        if (isfleetWheelData == false) {
            $('.flt_whedec').each(function () {
                if (this.value != "0") {
                    isfleetWheelData = true;
                    return;
                }
            });
        }
        $(document).find("#FleetAssetModel_isfleetDimensionData").val(isfleetDimensionData);
        $(document).find("#FleetAssetModel_isfleetEngineData").val(isfleetEngineData);
        $(document).find("#FleetAssetModel_isfleetFluidsData").val(isfleetFluidsData);
        $(document).find("#FleetAssetModel_isfleetWheelData").val(isfleetWheelData);
    }

    $(document).on('keyup', "#FleetAssetModel_EquID", function () {
        ResetErrorDiv();
    });
    $(document).find('.select2picker').select2({});
    var fleetassetstatus = localStorage.getItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS");
    if (fleetassetstatus) {
        activeStatus = fleetassetstatus;
        $('#fleetassetsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == activeStatus && $(this).attr('id') != '0') {
                $('#fleetassetsearchtitle').text($(this).text());
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        localStorage.setItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS", "1");
        fleetassetstatus = localStorage.getItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS");
        activeStatus = fleetassetstatus;
        $('#fleetassetsearchListul li').first().addClass('activeState');
        $('#fleetassetsearchtitle').text(getResourceValue("AlertActive"));
    }
});


$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});

function generateFleetAssetDataTable() {
    var printCounter = 0;
    var LocalizedAvailability = "";
    if ($(document).find('#fleetassetSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#fleetassetSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "stateSave": true,
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                //Search Retention
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
                //Search Retention
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {

            //Search Retention
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
                        callback(LayoutInfo);
                        if (json.FilterInfo) {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));

                        }
                    }
                    else {
                        callback(json);
                    }
                }
            });
            //Search Retention
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Fleet Asset List'
            },
            {
                extend: 'print',
                title: 'Fleet Asset List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Fleet Asset List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Fleet Asset List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/FleetAsset/GetFleetAssetGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS");
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.Make = LRTrim($("#Make").val());
                d.Model = LRTrim($("#ModelNumber").val());
                d.VIN = LRTrim($("#VIN").val());
                d.VehicleType = (VehicletypeValFleetAsset != null) ? VehicletypeValFleetAsset : LRTrim($('#ddlVehicleType').val());
                d.AssetAvailability = (AssetAvailability != null) ? AssetAvailability : LRTrim($('#ddlassetAvailability').val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.orderDir = Array.from(d.order).length > 0 ? Array.from(d.order).shift().dir : 'asc';
                d.order = order;
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];
                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnFleetAssetExport').prop('disabled', true);
                }
                else {
                    $(document).find('#btnFleetAssetExport').prop('disabled', false);
                }
                if (totalcount < result.recordsTotal)
                    totalcount = result.recordsTotal;
                if (totalcount != result.recordsTotal)
                    selectedcount = result.recordsTotal;

                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                return result.data;
            }
        },
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        "columns":
            [
                {
                    "data": "ClientLookupId",
                    "autoWidth": false,
                    "bSearchable": true,
                    "bSortable": true,
                    className: 'text-left',
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=link_fleetasset_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
                { "data": "VIN", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
                { "data": "VehicleType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
                {
                    "data": "RemoveFromService", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                    "mRender": function (data, type, row) {
                        if (data == false) {
                            LocalizedAvailability = getResourceValue("InServiceNameAlerts");
                            return "<span class='m-badge m-badge-grid-cell m-badge--green m-badge--wide'>" + LocalizedAvailability + "</span >";
                        }
                        else {
                            LocalizedAvailability = getResourceValue("OutofServiceNameAlerts");
                            return "<span class='m-badge m-badge-grid-cell m-badge--red m-badge--wide'>" + LocalizedAvailability + "</span >";
                        }
                    }
                },

                { "data": "RemoveFromServiceDate", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
                { "data": "VehicleYear", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Make", "autoWidth": true, "bSearchable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true }
            ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();         
            $("#fleetassetGridAction :input").removeAttr("disabled");
            $("#fleetassetGridAction :button").removeClass("disabled");
            DisableExportButton($("#fleetassetSearch"), $(document).find('#btnFleetAssetExport'));
        }
    });
}
$(document).on('click', '#fleetassetSearch_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#fleetassetSearch_length .searchdt-menu', function () {
    run = true;
});
$('#fleetassetSearch').find('th').click(function () {
    run = true;
    order = $(this).data('col');
    currentorderedcolumn = order;
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var eqid = LRTrim($("#EquipmentID").val());
            var name = LRTrim($("#Name").val());
            var make = LRTrim($("#Make").val());
            var model = LRTrim($("#ModelNumber").val());
            var vehicleType = $('#ddlVehicleType').val();
            if (!vehicleType) {
                vehicleType = "";
            }
            var AssetAvailability = $('#ddlassetAvailability').val();
            if (!AssetAvailability) {
                AssetAvailability = "";
            }
            var vin = LRTrim($("#VIN").val());
            dtTable = $("#fleetassetSearch").DataTable();
            var info = dtTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var length = $('#fleetassetSearch').dataTable().length;
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/FleetAsset/GetFleetAssetPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    customQueryDisplayId: localStorage.getItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS"),
                    start: start,
                    length: lengthMenuSetting,
                    _ClientLookupId: eqid,
                    _Name: name,
                    _Make: make,
                    _Model: model,
                    _Vin: vin,
                    _VehicleType: vehicleType,
                    AssetAvailability: AssetAvailability,
                    _colname: order,
                    _coldir: orderDir,
                    _searchText: searchText
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#fleetassetSearch thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.ClientLookupId != null) {
                    item.ClientLookupId = item.ClientLookupId;
                }
                else {
                    item.ClientLookupId = "";
                }
                if (item.Name != null) {
                    item.Name = item.Name;
                }
                else {
                    item.Name = "";
                }
                if (item.Year != null) {
                    item.Year = item.Year;
                }
                else {
                    item.Year = "";
                }
                if (item.Make != null) {
                    item.Make = item.Make;
                }
                else {
                    item.Make = "";
                }
                if (item.Model != null) {
                    item.Model = item.Model;
                }
                else {
                    item.Model = "";
                }
                if (item.VehicleType != null) {
                    item.VehicleType = item.VehicleType;
                }
                else {
                    item.VehicleType = "";
                }
                if (item.RemoveFromService != null) {
                    item.RemoveFromService = item.RemoveFromService;
                }
                else {
                    item.RemoveFromService = "";
                }
                if (item.RemoveFromServiceDate != null) {
                    item.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                else {
                    item.RemoveFromServiceDate = "";
                }
                if (item.VIN != null) {
                    item.VIN = item.VIN;
                }
                else {
                    item.VIN = "";
                }


                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key];
                    fData.push(value);
                });
                d.push(fData);
            });
            return {
                body: d,
                header: $("#fleetassetSearch thead tr th").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
    $("#EqpBulksidebar").mCustomScrollbar({
        theme: "minimal"
    });
});

function FleetAssetAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {

        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
        if ($(this).attr('id') == "ddlVehicleType") {
            if ($(this).val() == null && VehicletypeValFleetAsset != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }
        if ($(this).attr('id') == "ddlassetAvailability") {
            if ($(this).val() == null && AssetAvailability != null) {
                selectCount++;
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
        }

    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    var optionVal = $(document).find("#equipDropdown").val();
    if (optionVal == "1") {
        InactiveFlag = true;
    }
    $("#dvFilterSearchSelect2").html(searchitemhtml);
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    $('#txtColumnSearch').val('');
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
function clearAdvanceSearch() {
    var temp2 = "Select Type";
    $("#ddlVehicleType").val("").trigger('change');
    $('.adv-item').val("");
    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');
    VehicletypeValFleetAsset = $("#ddlVehicleType").val();
    AssetAvailability = $("#ddlassetAvailability").val();
}
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "ddlVehicleType") {
        VehicletypeValFleetAsset = null;
    }
    if (searchtxtId == "ddlassetAvailability") {
        AssetAvailability = null;
    }

    FleetAssetAdvSearch();
    dtTable.page('first').draw('page');
});
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
function RedirectToFleetAssetDetail(EquipmentId, mode) {
    $.ajax({
        url: "/FleetAsset/FleetAssetDetails",
        type: "POST",
        dataType: 'html',
        data: { EquipmentId: EquipmentId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#fleetassetmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            LoadComments(EquipmentId);
            SetFleetAssetDetailEnvironment();
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('#FleetAssettab').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
                $(document).find('#FleetAssetOverview').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            if (mode === "attachment") {
                $('#Attachmentt').trigger('click');
                $('#colorselector').val('Attachment');
            }
            if (mode === "parts") {
                $('#Partst').trigger('click');
                $('#colorselector').val('PartsContainer');
            }
            if (mode === "equipment") {
                $(document).find('#FleetAssetOverview').trigger('click');
                $('#colorselector').val('FleetAssettab');
            }

        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetFleetAssetDetailEnvironment() {
    CloseLoader();
    ZoomImage($(document).find('#EquipZoom'));
    Dropzone.autoDiscover = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: 'Drag files here to upload, or click to select one',
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: "Do you want to remove this file ?",
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);

            }
        });
    }
    SetFixedHeadStyle();
}

function FleetAssetEditOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("FleetAssetUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToFleetAssetDetail(data.EquipmentId, "equipment");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function FleetAssetAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("FleetAssetAddAlert");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                RedirectToFleetAssetDetail(data.EquipmentId, "equipment");
            });
        }
        else {
            ResetErrorDiv();
            $(document).find('#equipmentabdidtab').addClass('active').trigger('click');
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('change', '#FleetAssetModel_Meter1Type', function () {

    var Meter1Typeval = $(this).val();
    if (Meter1Typeval) {
        $(document).find('#FleetAssetModel_Meter1Units').removeAttr('disabled');
        $(document).find('#FleetAssetModel_Meter2Type').removeAttr('disabled');
    }
    else {
        $(document).find('#FleetAssetModel_Meter1Units').val('').trigger('change').attr('disabled', 'disabled');
        $(document).find('#FleetAssetModel_Meter2Units').val('').trigger('change').attr('disabled', 'disabled');
        $(document).find('#FleetAssetModel_Meter2Type').val('').trigger('change').attr('disabled', 'disabled');
        $(document).find('#FleetAssetModel_Meter2Type').val(null).trigger('change.select2');

        var areaddescribedby = $(document).find("#FleetAssetModel_Meter1Units").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#FleetAssetModel_Meter1Units").removeClass("input-validation-error");

    }
});
$(document).on('change', '#FleetAssetModel_Meter2Type', function () {

    var Meter2Typeval = $(this).val();
    if (Meter2Typeval) {
        $(document).find('#FleetAssetModel_Meter2Units').removeAttr('disabled');
    }
    else {
        $(document).find('#FleetAssetModel_Meter2Units').val('').trigger('change').attr('disabled', 'disabled');

    }
});

$(document).on('change', '#FleetAssetModel_Meter1Units', function () {
    if ($(document).find("#FleetAssetModel_Meter1Units").val().length > 0 && $(document).find("#FleetAssetModel_Meter1Type").val().length > 0) {
        var areaddescribedby = $(document).find("#FleetAssetModel_Meter1Units").attr('aria-describedby');
        if (typeof areaddescribedby !== 'undefined') {
            $('#' + areaddescribedby).hide();
        }
        $(document).find('form').find("#FleetAssetModel_Meter1Units").removeClass("input-validation-error");
    }
    else {
        var arectoaddescribedby = $(document).find("#FleetAssetModel_Meter1Units").attr('aria-describedby');
        if (typeof arectoaddescribedby !== 'undefined') {
            $('#' + arectoaddescribedby).show();
        }
    }
});
$(function () {
    $(document).on('click', '#FleetAssetOverview', function () {
        $('#FleetAssettab').show();
        $('.tabcontentIdentification').show();
        RemovedTabsActive();
        $('#btnIdentification').addClass('active');
        ShowAudit();
    });
    $(document).on('click', '#anchPhoto', function () {
        hideAudit();
    });
    $(document).on('click', '#photot', function () {
        $('#FleetAssettab').hide();
        $('.tabcontent2').hide();
    });
});
function SetFleetAssetControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
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
    ZoomImage($(document).find('#EquipZoom'));
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}

//#region Options
$(document).on('click', '#ActivateInactivateFleetAsset', function () {
    var eqid = $(document).find('#FleetAssetModel_EquID').val();
    var inactiveFlag = $(document).find('#FleetAssetStatus').val();
    var clientLookupId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    $.ajax({
        url: "/FleetAsset/ValidateFleetAssetStatusChange",
        type: "POST",
        dataType: "json",
        data: { _eqid: eqid, inactiveFlag: inactiveFlag, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (inactiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("ActivateFleetAssetAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("InActivateFleetAssetAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/FleetAsset/UpdateFleetAssetStatus",
                            type: "POST",
                            dataType: "json",
                            data: {
                                _eqid: eqid,
                                inactiveFlag: inactiveFlag
                            },
                            beforeSend: function () {
                                ShowLoader();
                            },
                            success: function (data) {
                                if (data.result == 'success') {
                                    if (inactiveFlag == "True") {
                                        SuccessAlertSetting.text = getResourceValue("FleetAssetActiveSuccessAlert");
                                        localStorage.setItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS", "1");
                                        titleText = getResourceValue("AlertActive");
                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("FleetAssetInActiveSuccessAlert");
                                        localStorage.setItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS", "2");
                                        titleText = getResourceValue("AlertInactive");
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        $.ajax({
                                            url: "/FleetAsset/FleetAssetDetails",
                                            type: "POST",
                                            dataType: "html",
                                            beforeSend: function () {
                                                ShowLoader();
                                            },
                                            data: { EquipmentId: $(document).find('#FleetAssetModel_EquID').val() },
                                            success: function (data) {

                                                $('#fleetassetmaincontainer').html(data);

                                                $(document).find('#spnlinkToSearch').text(titleText);
                                            },
                                            complete: function () {
                                                LoadComments($(document).find('#FleetAssetModel_EquID').val());
                                                SetFleetAssetDetailEnvironment();
                                            }
                                        });
                                    });
                                }
                                else {
                                    ShowGenericErrorOnAddUpdate(data);
                                }
                            },
                            complete: function () {
                                CloseLoader();
                            },
                            error: function () {
                                CloseLoader();
                            }
                        });
                    }
                });
            }
            else {
                GenericSweetAlertMethod(data);
            }
        },
        complete: function () {
            CloseLoader();
        },
        error: function (jqxhr) {
            CloseLoader();
        }
    });
});

$(document).on('click', '#changeFleetAsset', function () {
    var eqid = $(this).attr('data-eqid');
    var eqlookupid = $("#txtEquipmentId").val();
    var match = /^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.\\-]+$/g.test(eqlookupid);
    var errormsgtitle = "";
    var errormsgtext = "";
    if (eqlookupid == "") {
        errormsgtitle = getResourceValue("NoEquipmentIDAlert");
        errormsgtext = getResourceValue("EquipmentIDErrorAlert");
        swal({
            title: errormsgtitle,
            text: errormsgtext,
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }
    else if (match == false) {
        errormsgtitle = getResourceValue("NovalidEquipmentIDAlert");
        errormsgtext = getResourceValue("EquipmentIDRegErrAlert");
        swal({
            title: errormsgtitle,
            text: errormsgtext,
            type: "warning",
            showCancelButton: false,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("SaveAlertOk"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        });
    }
    else {
        swal({
            title: getResourceValue("CancelAlertSure"),
            text: getResourceValue("RecordUpdateFleetAlert"),
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-sm btn-primary",
            cancelButtonClass: "btn-sm",
            confirmButtonText: getResourceValue("CancelAlertYes"),
            cancelButtonText: getResourceValue("CancelAlertNo")
        }, function () {
            $('#changeFleetAssetIDModalDetailsPage').hide();
            $.ajax({
                url: '/FleetAsset/ChangeFleetAssetId',
                data: {
                    _eqid: eqid,
                    eqlookupid: eqlookupid
                },
                beforeSend: function () {
                    ShowLoader();
                },
                type: "POST",
                datatype: "json",
                success: function (data) {
                    $('#changeFleetAssetIDModalDetailsPage').modal('hide');
                    if (data.Result == "success") {
                        SuccessAlertSetting.text = getResourceValue("FleetAssetUpdateAlert");
                        swal(SuccessAlertSetting, function () {
                            RedirectToFleetAssetDetail(eqid, "equipment");
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
    }
});

$(document).on('click', '#optchangeequipid', function (e) {
    var clientlookupid = $(document).find('#hiddenclientlookupid').val();
    $(document).find('#txtEquipmentId').val(clientlookupid);
    $('#changeFleetAssetIDModalDetailsPage').modal('show');
    $(this).blur();
});
//#endregion

//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable,true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion

//#region CKEditor
$(document).on("focus", "#fleetassettxtcommentsNew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    //ClearEditor();
    LoadCkEditor('fleetassettxtcomments');
    $("#fleetassettxtcommentsNew").hide();
    $(".ckeditorfield").show();
});

$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }

    var EquipmentId = $(document).find('#FleetAssetModel_EquID').val();
    var EqClientLookupId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    var noteId = 0;

    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/FleetAsset/AddOrUpdateComment',
            type: 'POST',
            beforeSend: function () {
                ShowLoader();
            },
            data: {
                EquipmentId: EquipmentId,
                content: data,
                EqClientLookupId: EqClientLookupId,
                userList: selectedUsers,
                noteId: noteId,
            },
            success: function (data) {
                if (data.Result == "success") {
                    var message;
                    if (data.mode == "add") {
                        SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                    }
                    else {
                        SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                    }
                    swal(SuccessAlertSetting, function () {
                        RedirectToFleetAssetDetail(EquipmentId, "equipment");
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {

                ClearEditor();
                $("#fleetassettxtcommentsNew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
                selectedUnames = [];
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    }
});
$(document).on('click', '#commandCancel', function () {

    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    ClearEditor();
    $("#fleetassettxtcommentsNew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });

    $("#fleetassettxtcommentsNew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('fleetassettxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('fleetassettxtcommentsEdit', rawHTML);

    $(document).find('.ckeditorarea').hide();
    notesitem.find('.ckeditorarea').show();
    notesitem.find('.kt-notes__body').hide();
    notesitem.find('.commenteditdelearea').hide();
});
$(document).on('click', '.deletecomments', function () {
    DeleteEqNote($(this).attr('id'));
});
$(document).on('click', '.btneditcomments', function () {

    var data = getDataFromTheEditor();
    var EquipmentId = $(document).find('#FleetAssetModel_EquID').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var EqClientLookupId = $(document).find('#FleetAssetModel_ClientLookupId').val();
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/FleetAsset/AddOrUpdateComment',
        type: 'POST',
        beforeSend: function () {
            ShowLoader();
        },
        data: { EquipmentId: EquipmentId, content: LRTrim(data), noteId: noteId, updatedindex: updatedindex },
        success: function (data) {
            if (data.Result == "success") {
                var message;
                if (data.mode == "add") {
                    SuccessAlertSetting.text = getResourceValue("CommentAddAlert");
                }
                else {
                    SuccessAlertSetting.text = getResourceValue("CommentUpdateAlert");
                }
                swal(SuccessAlertSetting, function () {
                    RedirectToFleetAssetDetail(EquipmentId, "equipment");

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
            }
        },
        complete: function () {
            // ClearEditorEdit();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

});
$(document).on('click', '.btncommandCancel', function () {

    ClearEditorEdit();
    $(document).find('.ckeditorarea').hide();
    $(this).parents('.kt-notes__item').find('.kt-notes__body').show();
    $(this).parents('.kt-notes__item').find('.commenteditdelearea').show();
});
function DeleteEqNote(notesId) {
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/Base/DeleteComment',
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
                    SuccessAlertSetting.text = getResourceValue("CommentDeleteAlert");
                    swal(SuccessAlertSetting, function () {
                        RedirectToFleetAssetDetail($(document).find('#FleetAssetModel_EquID').val(), "equipment");
                    });
                }
            },
            complete: function () {
                CloseLoader();
            }
        });
    });
}
//#endregion
//#region Comment
var colorarray = [];
function colorobject(string, color) {
    this.string = string;
    this.color = color;
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
function LoadComments(EquipmentID) {
    $.ajax({
        "url": "/FleetAsset/LoadComments",
        data: { EquipmentId: EquipmentID },
        type: "POST",
        datatype: "json",
        success: function (data) {
            var getTexttoHtml = textToHTML(data);
            $(document).find('#commentstems').html(getTexttoHtml);
            $(document).find("#commentsList").mCustomScrollbar({
                theme: "minimal"
            });
        },
        complete: function () {
            var ftext = '';
            var bgcolor = '';
            $(document).find('#commentsdataloader').hide();
            $(document).find('#commentstems').find('.comment-header-item').each(function () {
                var thistext = LRTrim($(this).text());
                if (ftext == '' || ftext != thistext) {
                    var bgcolorarr = colorarray.filter(function (a) {
                        return a.string == thistext;
                    });
                    if (bgcolorarr.length == 0) {
                        bgcolor = getRandomColor();
                        var thisval = new colorobject(thistext, bgcolor);
                        colorarray.push(thisval);
                    }
                    else {
                        bgcolor = bgcolorarr[0].color;
                    }
                }
                $(this).attr('style', 'color:' + bgcolor + '!important;border:1px solid' + bgcolor + '!important;');
                ftext = LRTrim($(this).text());
            });
            var loggedinuserinitial = LRTrim($('#hdr-comments-add').text());
            var avlcolor = colorarray.filter(function (a) {
                return a.string == loggedinuserinitial;
            });
            if (avlcolor.length == 0) {
                $('#hdr-comments-add').attr('style', 'border:1px solid #264a7c !important;').show();
            }
            else {
                $('#hdr-comments-add').attr('style', 'color:' + avlcolor[0].color + ' !important;border:1px solid ' + avlcolor[0].color + '!important;').show();
            }
            $('.kt-notes__body a').attr('target', '_blank');
        }
    });
}
//#endregion

//#region Dropdown toggle   
$(document).on('click', "#spnDropToggle", function () {
    $(document).find('#searcharea').show("slide");
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searcharea');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slide");
    }
});
$(document).mouseup(function (e) {
    var container = $(document).find('#searchBttnNewDrop');
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.hide("slideToggle");
    }
});
//#endregion

//#region New Search button
$(document).on('keyup', '#fleetassetsearctxtbox', function (e) {
    var tagElems = $(document).find('#fleetassetsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.fleetassetsearchdrpbox', function (e) {
    run = true;
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#fleetassetsearchtitle').text($(this).text());
    }
    else {
        $('#fleetassetsearchtitle').text("Asset");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("FLEETASSETSEARCHGRIDDISPLAYSTATUS", optionval);
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        dtTable.page('first').draw('page');
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'FleetAsset' },
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
        data: { tableName: 'FleetAsset', searchText: txtSearchval, isClear: isClear },
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
    activeStatus = 0;
    var txtSearchval = LRTrim($(document).find('#txtColumnSearch').val());
    if (txtSearchval) {
        GenerateSearchList(txtSearchval, false);
        var searchitemhtml = "";
        searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $('#txtColumnSearch').attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
        $("#dvFilterSearchSelect2").html(searchitemhtml);
    }
    else {
        run = true;
        generateFleetAssetDataTable();
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
$(document).on('click', '#linkToSearch', function () {
    window.location.href = "../FleetAsset/Index?page=Fleet_Asset";
});
//#endregion

//#region //Search Retention
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
            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);
}
//#endregion
//#region //Search Fleet Asset QR OnSuccess
function FleetAssetQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        window.open('/FleetAsset/QRCodeGenerationUsingRotativa?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printFADetailsQrCodeModal').modal('hide');
      
        //-- when called from grid   for multiple  select     
        //equiToupdate = [];
        //equiToClientLookupIdbarcode = [];
        //if ($(document).find('#fleetassetSearch').find('.chksearch').length > 0) {
        //    $('#btnPrintRotativaQr').prop("disabled", "disabled");
        //    $('.itemQRcount').text(0);
        //    $('.itemcount').text(0);
        //    $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
        //    $(document).find(".updateArea").hide();
        //    $(document).find(".actionBar").fadeIn();
        //}
        //--
    }
}
    //#endregion