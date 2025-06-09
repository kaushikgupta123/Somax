var dtTable;
var btnActiveStatus = false;
var selectCount = 0;
var equiToupdate = [];
var selectedcount = 0;
var totalcount = 0;
var searchcount = 0;
var searchresult = [];
var equiToClientLookupIdbarcode = [];
var DepartmentValEquipment;
var LineValEquipment;
var SystemInfoValEquipment;
var activeStatus;
var run = false;
var equipmentidForTree = -1;
var titleText = '';
var assetGroup1;
var assetGroup2;
var assetGroup3;
var AssetAvailability;
var filterinfoarray = [];
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
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $("#equipGridAction :input").attr("disabled", "disabled");
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
    if (IsAdd != "True" && IsAddDynamic != "True") {
        //#region Load Grid With Status
        var displaymode = localStorage.getItem("ASSETVIEW");
        if (displaymode == 'CV') {
            $(document).find('#Active').hide();
            //$(document).find('#tblworkorders').hide();
            var thisstate = JSON.parse(localStorage.getItem("ASSETCURRENTCARDVIEWSTATE"));
            srchgrdcardcurrentpage = thisstate.currentpage;
            srchcardviewstartvalue = thisstate.start;
            srchcardviewlength = thisstate.length;
            currentorderedcolumn = thisstate.currentorderedcolumn;
            currentorder = thisstate.order;
            $('#layoutsortmenu').text("Layout : " + "Card View");
            layoutType = 1;
            $('#btnsortmenu').text(thisstate.sorttext);
            GetDatatableLayout();
            generateEquipmentDataTable();
            ShowCardView();
        }
        else {
            $(document).find('#euipmentSearch').show();
            generateEquipmentDataTable();
        }
        //#endregion
    }
    $("#btnupdateequip").click(function () {
        $(".actionDrop2").slideToggle();
    });
    $(".actionDrop2 ul li a").click(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnupdateequip").focusout(function () {
        $(".actionDrop2").fadeOut();
    });
    $("#btnEqpDataAdvSrch").on('click', function (e) {
        run = true;
        $(document).find('#txtColumnSearch').val('');
        searchresult = [];
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
        DepartmentValEquipment = $("#ddlDepartment").val();
        LineValEquipment = $("#ddlLine").val();
        SystemInfoValEquipment = $("#ddlSystemInfo").val();
        AssetAvailability = $("#ddlassetAvailability").val();
        EquipmentAdvSearch();
        if (layoutType == 1) {
            srchcardviewstartvalue = 0;
            srchgrdcardcurrentpage = 1;

            LayoutFilterinfoUpdate();
            ShowCardView();
        }
        else {
            dtTable.page('first').draw('page');
        }
    });
    $(document).on('click', '.link_equi_detail', function (e) {
        e.preventDefault();
        var EquipmentId;
        titleText = $('#assetsearchtitle').text();
        if (layoutType == 1) {
            EquipmentId = $(this).attr('id');
        }
        else {
            var row = $(this).parents('tr');
            var data = dtTable.row(row).data();
            EquipmentId = data.EquipmentId;
        }
        $.ajax({
            url: "/Equipment/EquipmentDetails",
            type: "POST",
            dataType: "html",
            beforeSend: function () {
                ShowLoader();
            },
            data: { EquipmentId: EquipmentId },
            success: function (data) {
                $('#equipmentmaincontainer').html(data);
                $(document).find('#spnlinkToSearch').text(titleText);
            },
            complete: function () {
                LoadComments(EquipmentId);
                LoadImages(EquipmentId);
                wobyTypeGraphData();
                generateEquipGraph();
                SetEquipmentDetailEnvironment();
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
    $(document).on('click', "#btnCancelAddEquipment", function () {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Equipment/Index?page=Maintenance_Assets";
        });
    });
    $(document).on('click', "#eqeditcancel", function () {
        var equipmentid = $(document).find('#EquipModel_EquipmentID').val();
        swal(CancelAlertSetting, function () {
            RedirectToEquipmentDetail(equipmentid, "equipment");
        });
    });
    $(document).on('click', '.AddEquip', function () {
        $.ajax({
            url: "/Equipment/AddEquipmentDynamicView",
            type: "POST",
            dataType: 'html',
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $('#equipmentmaincontainer').html(data);
            },
            complete: function () {
                SetEquimentControls();
                checkformvalid();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
    
    $(document).on('click', "#equipmentedit", function () {
        var equipmentid = LRTrim($(document).find('#EquipData_EquipmentId').val());
        var ClientlookUpId = $(document).find('#EquipData_ClientLookupId').val();
        var Name = $('#EquipData_Name').val();
        var Status = $('#EquipData_Status').val();
        var isRemoveFromService = $('#EquipData_RemoveFromService').val();
        $.ajax({
            url: '/Equipment/EditEquipmentDynamic',
            data: { EquipmentId: equipmentid, ClientlookUpId: ClientlookUpId, Name: Name, isRemoveFromService: isRemoveFromService,Status: Status},
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
                $(document).find('.dtpicker:not(.readonly)').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    "dateFormat": "mm/dd/yy",
                    autoclose: true
                }).inputmask('mm/dd/yyyy');
                SetFixedHeadStyle();
               
            }
        });
    });
    $(document).on('keyup', "#EquipModel_EquipmentID", function () {
        ResetErrorDiv();
    });
    $(document).find('.select2picker').select2({});
    var assetstatus = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS");
    if (assetstatus) {
        activeStatus = assetstatus;
        $('#astsearchListul li').each(function (index, value) {
            if ($(this).attr('id') == activeStatus && $(this).attr('id') != '0') {
                $('#assetsearchtitle').text($(this).text());
                localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT", $(this).text()); 
                $(".searchList li").removeClass("activeState");
                $(this).addClass('activeState');
            }
        });
    }
    else {
        localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS", "1");
        assetstatus = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS");
        activeStatus = assetstatus;
        $('#astsearchListul li').first().addClass('activeState');
        $('#assetsearchtitle').text(getResourceValue("AlertActive"));
        localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT", getResourceValue("AlertActive"));
    }
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
      
        if ($(this).closest('form').length > 0) {
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
        }
    });
});
$('#btnEqpDataSrch').on('click', function () {
    searchresult = [];
    clearAdvanceSearch();
    dtTable.page('first').draw('page');
});
$(document).on('click', '#eqsearch-select-all', function (e) {
    equiToClientLookupIdbarcode = [];
    equiToupdate = [];
    var srcData = $('#txtColumnSearch').val();
    var eqid = LRTrim($("#EquipmentID").val());
    var name = LRTrim($("#Name").val());
    var location = LRTrim($("#Location").val());
    var serial_number = LRTrim($("#SerialNumber").val());
    var type = $('#ddlType').val();
    if (!type) {
        type = "";
    }
    var make = LRTrim($("#Make").val());
    var model = LRTrim($("#ModelNumber").val());

    var laborAccountClientLookupId = LRTrim($("#AccountSearchId").val());
    var assest_number = LRTrim($("#AssetsNumber").val());
    var area = LRTrim($("#Area").val());

    var assetGroup1Id = $(document).find('#AssetGroup1Id').val();
    if (!assetGroup1Id) {
        assetGroup1Id = "";
    }
    var assetGroup2Id = $(document).find('#AssetGroup2Id').val();
    if (!assetGroup2Id) {
        assetGroup2Id = "";
    }
    var assetGroup3Id = $(document).find('#AssetGroup3Id').val();
    if (!assetGroup3Id) {
        assetGroup3Id = "";
    }
    searchText = LRTrim($(document).find('#txtColumnSearch').val());
    searchresult = [];
    var checked = this.checked;
    $.ajax({
        url: '/Equipment/GetEquipment',
        data: {
            inactiveFlag: localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS"),
            SearchText: srcData,
            ClientLookupId: eqid,
            Name: name,
            Location: location,
            SerialNumber: serial_number,
            Type: type,
            Make: make,
            Model: model,
            LaborAccountClientLookupId: laborAccountClientLookupId,
            AssetNumber: assest_number,
            Area_Desc: area,
            AssetGroup1Id: assetGroup1Id,
            AssetGroup2Id: assetGroup2Id,
            AssetGroup3Id: assetGroup3Id
           
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
                    searchresult.push(item.EquipmentId);
                    if (checked) {
                        if (equiToupdate.indexOf(item.EquipmentId) == -1) {
                            equiToupdate.push(item.EquipmentId);
                            equiToClientLookupIdbarcode.push(item.ClientLookupId + '][' + item.Name + '][' + item.SerialNumber + '][' + item.Make + '][' + item.Model);
                        }
                    } else {
                        var i = equiToupdate.indexOf(item.EquipmentId);
                        equiToupdate.splice(i, 1);
                        equiToClientLookupIdbarcode.splice(i, 1);
                    }
                });
            }
        },
        complete: function () {
            $('.itemcount').text(equiToupdate.length);
            if (checked) {
                $(".actionBar").hide();
                $(".updateArea").fadeIn();
                $('#btnupdateequip').removeAttr("disabled");
                $('#printQrcode').removeAttr("disabled");
                $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', 'checked');
            } else {
                $(".updateArea").hide();
                $(".actionBar").fadeIn();
                $('#btnupdateequip').prop("disabled", "disabled");
                $('#printQrcode').prop("disabled", "disabled");
                $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            }
            CloseLoader();
        }
    });
});
$(document).on('change', '.chksearch', function () {
    var data = dtTable.row($(this).parents('tr')).data();
    if (!this.checked) {
        var el = $('.DTFC_LeftHeadWrapper').find('#eqsearch-select-all').get(0);
        if (el && el.checked && ('indeterminate' in el)) {
            el.indeterminate = true;
        }
        selectedcount--;
        var index = equiToupdate.indexOf(data.EquipmentId);
        equiToupdate.splice(index, 1);
        equiToClientLookupIdbarcode.splice(index, 1);
    }
    else {
        equiToupdate.push(data.EquipmentId);
        selectedcount = selectedcount + equiToupdate.length;
        equiToClientLookupIdbarcode.push(data.ClientLookupId + '][' + data.Name + '][' + data.SerialNumber + '][' + data.Make + '][' + data.Model);
    }
    if (equiToupdate.length > 0) {
        $(".actionBar").hide();
        $(".updateArea").fadeIn();

        $('#btnupdateequip').removeAttr("disabled");
        $('#printQrcode').removeAttr("disabled");
    }
    else {
        $(".updateArea").hide();
        $(".actionBar").fadeIn();
        $('#btnupdateequip').prop("disabled", "disabled");
        $('#printQrcode').prop("disabled", "disabled");
        $(document).find('.DTFC_LeftHeadWrapper').find('#eqsearch-select-all').prop('indeterminate', false);
        $(document).find('.DTFC_LeftHeadWrapper').find('#eqsearch-select-all').prop('checked', false);
    }
    $('.itemcount').text(equiToupdate.length);
});
$('#printQrcode').on('click', function () {
    var equipClientLookups = equiToClientLookupIdbarcode;
    if (equipClientLookups.length > 50) {
        var datamsg = 'Cannot Print more than 50 items';
        GenericSweetAlertMethod(datamsg);
        return false;
    }
    else {
        if ($('#EPMInvoiceImportInUse').val() == "True") {
            generateQRforEPM(equipClientLookups);
        }
        else {
            $.ajax({
                url: "/Equipment/EqupmentDetailsQRcode",
                data: {
                    EquipClientLookups: equipClientLookups
                },
                type: "POST",
                beforeSend: function () {
                    ShowLoader();
                },
                datatype: "html",
                success: function (data) {
                    $('#printqrcodemodalcontainer').html(data);
                },
                complete: function () {
                    $('#printEqDetailsQrCodeModal').modal('show');
                    $('#btnEqGenerateQr').prop("disabled", "disabled");
                    $(document).find('.itemQRcount').text(equipClientLookups.length);
                    $(document).find('.itemQRcount').parent().css('display', 'block');
                    CloseLoader();
                },
                error: function () {
                    CloseLoader();
                }
            });
        }
    }
});


function EquipmentQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
        var smallLabel = $('#SmallLabel').prop('checked');
        window.open('/Equipment/QRCodeGenerationUsingDevExpress?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printEqDetailsQrCodeModal').modal('hide');
        equiToupdate = [];
        equiToClientLookupIdbarcode = [];
        //-- when called from grid         
        if ($(document).find('#euipmentSearch').find('.chksearch').length > 0) {
            $('#printQrcode').prop("disabled", "disabled");
            $('.itemQRcount').text(0);
            $('.itemcount').text(0);
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            $(document).find(".updateArea").hide();
            $(document).find(".actionBar").fadeIn();
        }
        //--
    }
}

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
var order = '1';
var orderDir = 'asc';
function generateEquipmentDataTable() {
    var businessType;
    var printCounter = 0;
    if ($(document).find('#euipmentSearch').hasClass('dataTable')) {
        dtTable.destroy();
    }
    dtTable = $("#euipmentSearch").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        serverSide: true,
        "pagingType": "input",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        "stateSave": true,
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
                        GridName: "Equipment_Search",
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
            var o;
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: "Equipment_Search"
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    selectCount = 0;
                    if (json.LayoutInfo !== '') {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo !== '') {
                            setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                        }
                    }
                    else {
                        callback(json.LayoutInfo);
                    }

                }
            });
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 2
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Asset List'
            },
            {
                extend: 'print',
                title: 'Asset List'
            },

            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Asset List',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                },
                css: 'display:none',
                title: 'Asset List',
                orientation: 'landscape',
                pageSize: 'A3'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Equipment/GetequipmentGridData",
            "type": "post",
            "datatype": "json",
            cache: false,
            data: function (d) {
                d.customQueryDisplayId = localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS");
                d.ClientLookupId = LRTrim($("#EquipmentID").val());
                d.Name = LRTrim($("#Name").val());
                d.Location = LRTrim($("#Location").val());

                //<!--(Added on 25/06/2020)-->
                d.AssetGroup1Id = LRTrim($('#AssetGroup1Id').val());
                d.AssetGroup2Id = LRTrim($("#AssetGroup2Id").val());
                d.AssetGroup3Id = LRTrim($("#AssetGroup3Id").val());
                d.LaborAccountClientLookupId = LRTrim($("#AccountSearchId").val());
                //<!--(Added on 25/06/2020)-->

                d.SerialNumber = LRTrim($("#SerialNumber").val());
                d.Type = LRTrim($('#ddlType').val());
                d.Make = LRTrim($("#Make").val());
                d.Model = LRTrim($("#ModelNumber").val());
                d.AssetNumber = LRTrim($("#AssetsNumber").val());
                d.SearchText = LRTrim($(document).find('#txtColumnSearch').val());
                d.Order = order;
                d.AssetAvailability = (AssetAvailability != null) ? AssetAvailability : LRTrim($('#ddlassetAvailability').val());
            },
            "dataSrc": function (result) {
                let colOrder = dtTable.order();
                orderDir = colOrder[0][1];

                searchcount = result.recordsTotal;
                if (result.data.length < 1) {
                    $(document).find('#btnAssetExport,#eqsearch-select-all').prop('disabled', true);
                }
                else {
                    $(document).find('#btnAssetExport,#eqsearch-select-all').prop('disabled', false);
                }
                if (result.data && result.data.length) {
                    businessType = result.data[0].BusinessType;
                }
                $.each(result.data, function (index, item) {
                    searchresult.push(item.EquipmentId);
                });
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
                    "data": "EquipmentId",
                    orderable: false,
                    "bSortable": false,
                    className: 'select-checkbox dt-body-center',
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
                    "orderable": true,
                    "bSortable": true,
                    "autoWidth": false,
                    "bSearchable": true,
                    className: 'text-left',
                    "name": "0",
                    "mRender": function (data, type, row) {
                        return '<a class=link_equi_detail href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "Name", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "1" },
                { "data": "Location", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "2" },
                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "3" },
                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "4" },
                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "5" },              
                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "Type", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true, "name": "7" },
                { "data": "Make", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "Model", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
                { "data": "LaborAccountClientLookupId", "autoWidth": true, "bSearchable": false, "orderable": true, "bSortable": true },
                { "data": "AssetNumber", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
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

            ],
        columnDefs: [
            {
                targets: [0, 1],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            if (totalcount != 0 && (totalcount == equiToupdate.length || (searchcount != totalcount && arrayContainsArray(equiToupdate, searchresult) == true))) {
                $('#eqsearch-select-all').prop('checked', true);
            } else {
                $('#eqsearch-select-all').prop('checked', false);
            }
            $("#equipGridAction :input").removeAttr("disabled");
            $("#equipGridAction :button").removeClass("disabled");
            DisableExportButton($("#euipmentSearch"), $(document).find('#btnAssetExport,#eqsearch-select-all'));
        }
    });
}
$(document).on('click', '#euipmentSearch_paginate .paginate_button', function () {
    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        srchcardviewlength = $(document).find('#searchcardviewpagelengthdrp').val();
        srchcardviewstartvalue = srchcardviewlength * currentselectedpage;
        var lastpage = parseInt($(document).find('#spntotalpages').text());

        if ($(this).attr('id') == 'tbl_previous') {
            if (currentselectedpage == 1) {
                return false;
            }
            srchcardviewstartvalue = srchcardviewlength * (currentselectedpage - 2);
            srchgrdcardcurrentpage = srchgrdcardcurrentpage - 1;
        }
        else if ($(this).attr('id') == 'tbl_next') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            srchcardviewstartvalue = srchcardviewlength * (currentselectedpage);
            srchgrdcardcurrentpage = srchgrdcardcurrentpage + 1;
        }
        else if ($(this).attr('id') == 'tbl_first') {
            if (currentselectedpage == 1) {
                return false;
            }
            srchgrdcardcurrentpage = 1;
            srchcardviewstartvalue = 0;
        }
        else if ($(this).attr('id') == 'tbl_last') {
            if (currentselectedpage == lastpage) {
                return false;
            }
            srchgrdcardcurrentpage = parseInt($(document).find('#spntotalpages').text());
            srchcardviewstartvalue = srchcardviewlength * (srchgrdcardcurrentpage - 1);
        }

        LayoutUpdate('pagination');
        ShowCardView();
    }
    run = true;
});
$(document).on('change', '#euipmentSearch_length .searchdt-menu', function () {
    run = true;
});
$('#euipmentSearch').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }
});

$(function () {
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var eqid = LRTrim($("#EquipmentID").val());
            var name = LRTrim($("#Name").val());
            var location = LRTrim($("#Location").val());
            var serial_number = LRTrim($("#SerialNumber").val());
            var type = $('#ddlType').val();
            if (!type) {
                type = "";
            }
            var department = $('#ddlDepartment').val();
            if (!department) {
                department = "";
            }
            var line = $('#ddlLine').val();
            if (!line) {
                line = "";
            }
            var systemInfo = $('#ddlSystemInfo').val();
            if (!systemInfo) {
                systemInfo = "";
            }
            var AssetAvailability = $('#ddlassetAvailability').val();
            if (!AssetAvailability) {
                AssetAvailability = "";
            }
            dtTable = $("#euipmentSearch").DataTable();
            var info = dtTable.page.info();
            var start = info.start;
            var lengthMenuSetting = info.length;
            var currestsortedcolumn = order;
            var length = $('#euipmentSearch').dataTable().length;
            var coldir = orderDir;
            var make = LRTrim($("#Make").val());
            var model = LRTrim($("#ModelNumber").val());
            var laborAccountClientLookupId = LRTrim($("#AccountSearchId").val());
            var assest_number = LRTrim($("#AssetsNumber").val());
            var searchText = LRTrim($(document).find('#txtColumnSearch').val());
            var jsonResult = $.ajax({
                "url": "/Equipment/GetEquipmentPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    customQueryDisplayId: localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS"),
                    start: start,
                    length: lengthMenuSetting,
                    _ClientLookupId: eqid,
                    _Name: name,
                    _Location: location,
                    _SerialNumber: serial_number,
                    _Type: type,
                    _Make: make,
                    _Model: model,
                    _LaborAccountClientLookupId: laborAccountClientLookupId,
                    _AssetNumber: assest_number,
                    _AssetGroup1Id: LRTrim($('#AssetGroup1Id').val()),
                    _AssetGroup2Id: LRTrim($("#AssetGroup2Id").val()),
                    _AssetGroup3Id: LRTrim($("#AssetGroup3Id").val()),
                    _colname: currestsortedcolumn,
                    _coldir: coldir,
                    _searchText: searchText,
                    AssetAvailability: AssetAvailability
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#euipmentSearch thead tr th").not(":eq(0)").map(function (key) {
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
                if (item.Location != null) {
                    item.Location = item.Location;
                }
                else {
                    item.Location = "";
                }
                if (item.AsseteGroup1ClientLookupId != null) {
                    item.AsseteGroup1ClientLookupId = item.AsseteGroup1ClientLookupId;
                }
                else {
                    item.AsseteGroup1ClientLookupId = "";
                }
                if (item.AsseteGroup2ClientLookupId != null) {
                    item.AsseteGroup2ClientLookupId = item.AsseteGroup2ClientLookupId;
                }
                else {
                    item.AsseteGroup2ClientLookupId = "";
                }
                if (item.AsseteGroup3ClientLookupId != null) {
                    item.AsseteGroup3ClientLookupId = item.AsseteGroup3ClientLookupId;
                }
                else {
                    item.AsseteGroup3ClientLookupId = "";
                }
                if (item.SerialNumber != null) {
                    item.SerialNumber = item.SerialNumber;
                }
                else {
                    item.SerialNumber = "";
                }
                if (item.Type != null) {
                    item.Type = item.Type;
                }
                else {
                    item.Type = "";
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
                if (item.LaborAccountClientLookupId != null) {
                    item.LaborAccountClientLookupId = item.LaborAccountClientLookupId;
                }
                else {
                    item.LaborAccountClientLookupId = "";
                }
                if (item.AssetNumber != null) {
                    item.AssetNumber = item.AssetNumber;
                }
                else {
                    item.AssetNumber = "";
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
                header: $("#euipmentSearch thead tr th").not(":eq(0)").find('div').map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
    $("#EqpBulksidebar").mCustomScrollbar({
        theme: "minimal"
    });
});

function EquipmentAdvSearch() {
    var InactiveFlag = false;
    $('#txtColumnSearch').val('');
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
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
    $("#ddlType").val("").trigger('change');
    $("#ddlDepartment").val("").trigger('change');
    $("#ddlLine").val("").trigger('change');
    $("#ddlSystemInfo").val("").trigger('change');
    $("#ddlassetAvailability").val("").trigger('change');
    $('.adv-item').val("");

    $("#AssetGroup1Id").val("").trigger('change');
    $("#AssetGroup2Id").val("").trigger('change');
    $("#AssetGroup3Id").val("").trigger('change');
    $("#AccountSearchId").val("").trigger('change');

    selectCount = 0;
    $("#spnControlCounter").text(selectCount);
    $('#liSelectCount').text(selectCount + ' filters applied');
    newEle = "";
    $('#dvFilterSearchSelect2').find('span').html('');
    $('#dvFilterSearchSelect2').find('span').removeClass('tagTo');

    DepartmentValEquipment = $("#ddlDepartment").val();
    LineValEquipment = $("#ddlLine").val();
    SystemInfoValEquipment = $("#ddlSystemInfo").val();
    AssetAvailability = $("#ddlassetAvailability").val();
}
$(document).on('click', '#sidebarCollapse', function () {
    $('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find("#AssetGroup1Id").rules('remove', 'required');
    $(document).find("#ddlType").rules('remove', 'required');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossed = $(this).parent().attr('id');
    var searchtxtId = btnCrossed.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    selectCount--;
    if (searchtxtId == "ddlDepartment") {
        DepartmentValEquipment = null;
    }

    if (searchtxtId == "ddlLine") {
        LineValEquipment = null;
    }

    if (searchtxtId == "ddlSystemInfo") {
        SystemInfoValEquipment = null;
    }
    if (searchtxtId == "ddlassetAvailability") {
        AssetAvailability = null;
    }
    EquipmentAdvSearch();
    if (layoutType == 1) {
        srchcardviewstartvalue = 0;
        srchgrdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        dtTable.page('first').draw('page');
    }
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
function RedirectToEquipmentDetail(EquipmentId, mode) {
    $.ajax({
        url: "/Equipment/EquipmentDetails",
        type: "POST",
        dataType: 'html',
        data: { EquipmentId: EquipmentId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#equipmentmaincontainer').html(data);
            $(document).find('#spnlinkToSearch').text(titleText);
        },
        complete: function () {
            LoadComments(EquipmentId);
            LoadImages(EquipmentId);
            SetEquipmentDetailEnvironment();
            if (mode === "AzureImageReload" || mode === "OnPremiseImageReload") {
                $('#overviewcontainer').hide();
                $('#Equipmenttab').hide();
                $('.tabcontent2').hide();
                $('#auditlogcontainer').hide();
                $('.imageDropZone').show();
                $(document).find('#btnnblock').removeClass("col-xl-6");
                $(document).find('#btnnblock').addClass("col-xl-12");
                $(document).find('#EquipmentOverview').removeClass("active");
                $(document).find('#photot').addClass("active");
            }
            if (mode === "notes") {
                $(document).find('#Notest').trigger('click');
                $('#colorselector').val('Notes');
            }
            if (mode === "attachment") {
                $('#Attachmentt').trigger('click');
                $('#colorselector').val('Attachment');
            }
            if (mode === "techspec") {
                $('#Tech-Specs').trigger('click');
                $('#colorselector').val('TechSpecs');
            }
            if (mode === "parts") {
                $('#Partst').trigger('click');
                $('#colorselector').val('PartsContainer');
            }
            if (mode === "downtime") {
                $('#Downtimet').trigger('click');
                $('#colorselector').val('Downtime');
            }
            if (mode === "equipment") {
                $(document).find('#EquipmentOverview').trigger('click');
                $('#colorselector').val('Equipmenttab');
            }
            if (mode === "sensor") {
                $('#Sensorst').trigger('click');
                $('#colorselector').val('divSensors');
            }
        },
        error: function () {
            CloseLoader();
        }
    });
}
function SetEquipmentDetailEnvironment() {
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

$(document).on('change', '#EquipModel_AssetGroup1Id', function () {
    var asset1val = $(this).val();
    if (asset1val) {
        $(document).find('#EquipModel_AssetGroup2Id').removeAttr('disabled');
    }
    else {
        $(document).find('#EquipModel_AssetGroup2Id').val('').trigger('change').attr('disabled', 'disabled');
    }
});
$(document).on('change', '#EquipModel_AssetGroup2Id', function () {
    var asset2val = $(this).val();
    if (asset2val) {
        $(document).find('#EquipModel_AssetGroup3Id').removeAttr('disabled');
    }
    else {
        $(document).find('#EquipModel_AssetGroup3Id').val('').trigger('change').attr('disabled', 'disabled');
    }
});

function EquipmentAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                RedirectToEquipmentDetail(data.EquipmentId, "equipment");
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
                $(document).find('#EquipModel_AssetGroup2Id').val('').trigger('change').attr('disabled', 'disabled');
                $(document).find('#EquipModel_AssetGroup3Id').val('').trigger('change').attr('disabled', 'disabled');
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
checkformvalid();
function checkformvalid() {
    $("#eqpaddform").submit(function () {
        if ($(this).valid()) {
            return;
        }
        else {
            var activetagid = $(document).find('.vtabs li.active').attr('id');
            if (activetagid == 'equipmentabwarrantytab') {
                $(document).find('#equipmentabdidtab').trigger('click');
            }
        }
    });
}
var IsDetailFromWorkOrder = false;
$(function () {
    //#region V2-808
     IsDetailFromWorkOrder = $('#IsDetailFromWorkOrder').val();
    if (IsDetailFromWorkOrder == 'True') {
        var _isLoggedInFromMobile = CheckLoggedInFromMob();
        if (_isLoggedInFromMobile === true) {
            $('#divfileAsset').remove();
            $('#lifileAssetForMobile').show();
        }
        else {
            $('#lifileAssetForMobile').remove();
            $('#divfileAsset').show();
        }
    }
    //#endregion
    $(document).on('click', '#EquipmentOverview', function () {
        $('#overviewcontainer').show();
        $('#Equipmenttab').show();
        $('.tabcontent2').show();
        $('#btnIdentification').addClass('active');
        wobyTypeGraphData();
        generateEquipGraph();
        ShowAudit();
        $('#Identification').show();
        $('#Warranty').hide();
        $('#btnWarranty').removeClass('active');
    });
    $(document).on('click', '#anchPhoto', function () {
        hideAudit();
    });
    $(document).on('click', '#photot', function () {
        $('#overviewcontainer').hide();
        $('#Equipmenttab').hide();
        $('.tabcontent2').hide();
    });
});
function SetEquimentControls() {
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
    $(document).find('.dtpicker:not(.readonly)').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    SetFixedHeadStyle();
}

//#region Options
$(document).on('click', '#ActivateInactivateEquipment', function () {
    var eqid = $(document).find('#EquipData_EquipmentId').val();
    var inactiveFlag = $(document).find('#EqpStatus').val();
    var clientLookupId = $(document).find('#EquipData_ClientLookupId').val();
    $.ajax({
        url: "/Equipment/ValidateEqpStatusChange",
        type: "POST",
        dataType: "json",
        data: { _eqid: eqid, inactiveFlag: inactiveFlag, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                if (inactiveFlag == "True") {
                    CancelAlertSetting.text = getResourceValue("ActivateAssetAlert");
                }
                else {
                    CancelAlertSetting.text = getResourceValue("InActivateAssetAlert");
                }
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/Equipment/UpdateEquStatus",
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
                                        SuccessAlertSetting.text = getResourceValue("AssetActiveSuccessAlert");
                                    }
                                    else {
                                        SuccessAlertSetting.text = getResourceValue("AssetInActiveSuccessAlert");
                                    }
                                    swal(SuccessAlertSetting, function () {
                                        $.ajax({
                                            url: "/Equipment/EquipmentDetails",
                                            type: "POST",
                                            dataType: "html",
                                            beforeSend: function () {
                                                ShowLoader();
                                            },
                                            data: { EquipmentId: $(document).find('#EquipData_EquipmentId').val() },
                                            success: function (data) {
                                                $('#equipmentmaincontainer').html(data);
                                                $(document).find('#spnlinkToSearch').text(titleText);
                                            },
                                            complete: function () {
                                                LoadComments($(document).find('#EquipData_EquipmentId').val());
                                                wobyTypeGraphData();
                                                generateEquipGraph();
                                                SetEquipmentDetailEnvironment();
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

$(document).on('click', '#DeleteEquipment', function () {
    var eqid = $(this).attr('data-id');
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/Equipment/DeleteEquipment',
            data: {
                _eqid: eqid
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "POST",
            datatype: "json",
            success: function (data) {
                if (data.Result == "parentexist") {
                    GenericSweetAlertMethod(data.errorList);
                }
                else if (data.Result == "update") {
                    GenericSweetAlertMethod("History exists for the equipment and it has been marked as inactive");
                }
                else if (data.Result == "delete") {
                    swal(SuccessAlertSetting, function () {
                        window.location.href = "../Equipment/Index?page=Maintenance_Assets";
                    });
                }
                else {
                    var msgEror = getResourceValue("UpdateAlert");
                    ShowGenericErrorOnAddUpdate(msgEror);
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
function ChangeEquipmentIdOnSuccess(data) {
    $('#changeEquipmentIDModalDetailsPage').modal('hide');
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentUpdateAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToEquipmentDetail($('#EquipData_EquipmentId').val(), "equipment");
        });
    }
    else {
        GenericSweetAlertMethod(data);
    }
}
//#region equipment scrap
$(document).on('click', '#ScrapAsset', function () {
    var eqid = $(document).find('#EquipData_EquipmentId').val();
    var inactiveFlag = $(document).find('#EqpStatus').val();
    var clientLookupId = $(document).find('#EquipData_ClientLookupId').val();
    $.ajax({
        url: "/Equipment/ValidateEquipmentStatusScrapped",
        type: "POST",
        dataType: "json",
        data: { _eqid: eqid, inactiveFlag: inactiveFlag, clientLookupId: clientLookupId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data.validationStatus == true) {
                CancelAlertSetting.text = getResourceValue("ScrapAssetAlert");
                swal(CancelAlertSetting, function (isConfirm) {
                    if (isConfirm == true) {
                        $.ajax({
                            url: "/Equipment/UpdateEquipmentStatustoScrap",
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
                                    SuccessAlertSetting.text = getResourceValue("AssetScrappedSuccessAlert");
                                    swal(SuccessAlertSetting, function () {
                                        $.ajax({
                                            url: "/Equipment/EquipmentDetails",
                                            type: "POST",
                                            dataType: "html",
                                            beforeSend: function () {
                                                ShowLoader();
                                            },
                                            data: { EquipmentId: $(document).find('#EquipData_EquipmentId').val() },
                                            success: function (data) {
                                                $('#equipmentmaincontainer').html(data);
                                                $(document).find('#spnlinkToSearch').text(titleText);
                                            },
                                            complete: function () {
                                                LoadComments($(document).find('#EquipData_EquipmentId').val());
                                                wobyTypeGraphData();
                                                generateEquipGraph();
                                                SetEquipmentDetailEnvironment();
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
//#endregion

//#endregion
//#region ModalTree
$(document).on('click', '#BindTreeLookupEquipment', function (e) {
    ShowLoader();
    $(this).blur();
    // $('#myEquipmentTreeModal').modal('show');V2-846
    generateEquipmentTree(-1);//V2-1134
    // generateEquipTreeTable();V2-846
});
$(document).on('click', '#optchangeequipid', function (e) {
    var clientlookupid = $(document).find('#OldClientLookupId').val();
    $(document).find('#txtEquipmentId').val(clientlookupid).removeClass('input-validation-error');
    $('#changeEquipmentIDModalDetailsPage').modal('show');
    $.validator.unobtrusive.parse(document);
    $(this).blur();
});
function generateEquipmentTree(paramVal) {
    $.ajax({
        url: '/PlantLocationTree/EqEquipmentHierarchyTree',
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
            $('#myEquipmentTreeModal').modal('show');
        },
        complete: function () {
            CloseLoader();
            treeTable($(document).find('#tblTree'));
            var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
            if (equipmentidForTree == -1 && EquipmentId) {
                equipmentidForTree = parseInt(EquipmentId);
            }
            $(document).find('.radEQSelect').each(function () {
                if ($(this).data('equipmentid') === equipmentidForTree) {
                    $(this).attr('checked', true);
                    $(this).removeAttr('title');
                    $(this).parent('label').removeAttr('title');
                    $(this).parent('label').find('span').removeAttr('title');
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


$(document).on('change', '.radEQSelect', function () {
    var equipmentid = $(this).data('equipmentid');
    equipmentidForTree = $(this).data('equipmentid');

    $('#myEquipmentTreeModal').modal('hide');
    RedirectToEquipmentDetail(equipmentid, "equipment");
});
//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(dtTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0, 1];
    funCustozeSaveBtn(dtTable, colOrder);
    run = true;
    dtTable.state.save(run);
});
//#endregion
//#region BulkUpdate
$('#btnEqpbulkUpdate').on('click', function () {
    var eqpIds = equiToupdate;
    $.ajax({
        url: "/Equipment/ShowEquipBulkUpdate",
        data: {
            EquipmentIds: eqpIds
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {
            $('#equipbulkupdatemodalcontainer').html(data);
        },
        complete: function () {
            SetEquimentControls();
            $('#EqpBulksidebar').addClass('active');
            $('.overlay').fadeIn();
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'false');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '#btnCancelEqpBulkUpdate,#dismiss, .overlay', function () {
    $('#EqpBulksidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function EqpBulkUploadOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        var message = "Total Records Updated : " + data.UpdatedItemCount;
        SuccessAlertSetting.text = message;
        swal(SuccessAlertSetting, function () {
            equiToupdate = [];
            $('#EqpBulksidebar').removeClass('active');
            $('.overlay').fadeOut();
            $(document).find('.chksearch,#eqsearch-select-all').prop("checked", false);
            $('.updateArea').hide();
            $(".actionBar").fadeIn();
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            dtTable.page(dtTable.page.info() + 1).draw('page');
            equiToClientLookupIdbarcode = [];
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data, "#EqpBulksidebar");
    }
}
//#endregion BulkUpdate

$(document).on("click", "#btnactivedept", function () {
    var deptid = $(document).find('#EquipDeptId').val();
    if (deptid) {
        $(document).find('#EquipModel_DeptID').val(deptid).trigger('change.select2');
        $('#activeDepartmentModal').modal('hide');
    }
    else {
        swal({
            title: getResourceValue("ValidationDepartment"),
            text: getResourceValue("ValidationDepartment"),
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
});
//#endregion
//#region CKEditor
$(document).on("focus", "#equtxtcommentsnew", function () {
    $(document).find('.ckeditorarea').show();
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.ckeditorarea').hide();
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    LoadCkEditor('equtxtcomments');
    $("#equtxtcommentsnew").hide();
    $(".ckeditorfield").show();

});

$(document).on('click', '#btnsavecommands', function () {
    var selectedUsers = [];
    const data = getDataFromTheEditor();
    if (!data) {
        return false;
    }
    var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
    var EqClientLookupId = $(document).find('#EquipData_ClientLookupId').val();
    var noteId = 0;
    if (LRTrim(data) == "") {
        return false;
    }
    else {
        $(document).find('.ck-content').find('.mention').each(function (item, index) {
            selectedUsers.push($(index).data('user-id'));
        });
        $.ajax({
            url: '/Equipment/AddOrUpdateComment',
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
                        RedirectToEquipmentDetail(EquipmentId, "equipment");
                    });
                }
                else {
                    ShowGenericErrorOnAddUpdate(data);
                    CloseLoader();
                }
            },
            complete: function () {
                ClearEditor();
                $("#equtxtcommentsnew").show();
                $(".ckeditorfield").hide();
                selectedUsers = [];
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
    $("#equtxtcommentsnew").show();
    $(".ckeditorfield").hide();
});
$(document).on('click', '.editcomments', function () {
    $(document).find(".ckeditorarea").each(function () {
        $(this).html('');
    });

    $("#equtxtcommentsnew").show();
    $(".ckeditorfield").hide();
    var notesitem = $(this).parents('.kt-notes__item').eq(0);
    notesitem.find('.ckeditorarea').html(CreateEditorHTML('equtxtcommentsEdit'));
    var noteitem = $(document).find('.editcomments').parents('.kt-notes__item');
    noteitem.find('.kt-notes__body').show();
    noteitem.find('.commenteditdelearea').show();
    var rawHTML = $.parseHTML($(this).parents('.kt-notes__item').find('.kt-notes__body').find('.originalContent').html());
    LoadCkEditorEdit('equtxtcommentsEdit', rawHTML);
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
    var EquipmentId = $(document).find('#EquipData_EquipmentId').val();
    var noteId = $(this).parents('.kt-notes__item').find('.editcomments').attr('id');
    var updatedindex = $(this).parents('.kt-notes__item').find('.hdnupdatedindex').val();
    $.ajax({
        url: '/Equipment/AddOrUpdateComment',
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
                    RedirectToEquipmentDetail(EquipmentId, "equipment");

                });
            }
            else {
                ShowGenericErrorOnAddUpdate(data);
                CloseLoader();
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
                        RedirectToEquipmentDetail($(document).find('#EquipData_EquipmentId').val(), "equipment");
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
        "url": "/Equipment/LoadComments",
        data: { EquipmentId: EquipmentID },
        type: "POST",
        datatype: "json",
        beforeSend: function () { $(document).find('#commentsdataloader').show(); },
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
$(document).on('keyup', '#astsearctxtbox', function (e) {
    var tagElems = $(document).find('#astsearchListul').children();
    $(tagElems).hide();
    for (var i = 0; i < tagElems.length; i++) {
        var tag = $(tagElems).eq(i);
        if ($(tag).text().toLowerCase().includes($(this).val().toLowerCase()) == true || $(this).val().toLowerCase().includes($(tag).text().toLowerCase()) == true) {
            $(tag).show();
        }
    }
});
$(document).on('click', '.astsearchdrpbox', function (e) {
    equiToClientLookupIdbarcode = [];
    equiToupdate = [];
    $(document).find('input[name=select_all]').prop('checked', false);
    $(".updateArea").hide();
    $(".actionBar").fadeIn();
    $('#btnupdateequip').prop("disabled", "disabled");
    $('#printQrcode').prop("disabled", "disabled");
    $('.itemcount').text(equiToupdate.length);
    if ($(document).find('#txtColumnSearch').val() !== '')
        $("#dvFilterSearchSelect2").html('');
    $(document).find('#txtColumnSearch').val('');
    run = true;
    if ($(this).attr('id') != '0') {
        $('#assetsearchtitle').text($(this).text());
    }
    else {
        $('#assetsearchtitle').text("Asset");
    }
    $(".searchList li").removeClass("activeState");
    $(this).addClass('activeState');
    $(document).find('#searcharea').hide("slide");
    var optionval = $(this).attr('id');
    localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS", optionval);
    localStorage.setItem("EQUIPMENTSEARCHGRIDDISPLAYSTEXT", $(this).text());
    if (layoutType == 1) {
        srchcardviewstartvalue = 0;
        srchgrdcardcurrentpage = 1;

        LayoutFilterinfoUpdate();
        ShowCardView();
    }
    else {
        if (optionval.length !== 0) {
            ShowbtnLoaderclass("LoaderDrop");
            dtTable.page('first').draw('page');
        }
    }
});
$(document).on('click', "#SrchBttnNew", function () {
    $.ajax({
        url: '/Base/PopulateNewSearchList',
        type: 'GET',
        data: { tableName: 'Equipment' },
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
    $.ajax({
        url: '/Base/ModifyNewSearchList',
        type: 'POST',
        data: { tableName: 'Equipment', searchText: txtSearchval, isClear: isClear },
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
                if (layoutType == 2) {
                    dtTable.page('first').draw('page');
                    CloseLoader();
                }
                else {
                    srchcardviewstartvalue = 0;
                    srchgrdcardcurrentpage = 1;

                    LayoutFilterinfoUpdate();
                    ShowCardView();
                }
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
    run = true;
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
        if (layoutType == 2) {
            generateEquipmentDataTable();
        }
        else {
            srchcardviewstartvalue = 0;
            srchgrdcardcurrentpage = 1;
            GetDatatableLayout();
            LayoutFilterinfoUpdate();
            ShowCardView();
        }
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
    window.location.href = "../Equipment/Index?page=Maintenance_Assets";
});
//#endregion

//#region V2-389
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

function EquipmentAddDynamicOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("EquipmentSaveAlerts");
        if (data.Command == "save") {
            swal(SuccessAlertSetting, function () {
                titleText = getResourceValue("AlertActive");
                RedirectToEquipmentDetail(data.EquipmentId, "equipment");
            });
        }
        else {
            ResetErrorDiv();
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").not("#colorselector").val("").trigger('change.select2');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
                $(document).find('form').find(".ClearVendorModalPopupGridData").css("display", "none");
                $(document).find('form').find(".ClearAccountModalPopupGridData").css("display", "none");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
function EquipmentEditDynamicOnSuccess(data) {
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
$(document).on('click', "#editcanceldynamic", function () {
    var equipmentid = $(document).find('#EditEquipment_EquipmentId').val();
    swal(CancelAlertSetting, function () {
        RedirectToEquipmentDetail(equipmentid, "equipment");
    });
});
//#endregion 


//#region V2-639 For Return Scrap Asset 
$(document).on('click', '#ReturnScrapEquipment', function () {
    var eqid = $(document).find('#EquipData_EquipmentId').val();
    var inactiveFlag = $(document).find('#EqpStatus').val();
    var clientLookupId = $(document).find('#EquipData_ClientLookupId').val();
   
    CancelAlertSetting.text = getResourceValue("ReturnScrapAssetAlert");
   
    swal(CancelAlertSetting, function (isConfirm) {
        if (isConfirm == true) {
            $.ajax({
                url: "/Equipment/UpdateAssetForReturnScrap",
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
                            SuccessAlertSetting.text = getResourceValue("AssetReturnScrapSuccessAlert");
                        }
                        swal(SuccessAlertSetting, function () {
                            $.ajax({
                                url: "/Equipment/EquipmentDetails",
                                type: "POST",
                                dataType: "html",
                                beforeSend: function () {
                                    ShowLoader();
                                },
                                data: { EquipmentId: $(document).find('#EquipData_EquipmentId').val() },
                                success: function (data) {
                                    $('#equipmentmaincontainer').html(data);
                                    $(document).find('#spnlinkToSearch').text(titleText);
                                },
                                complete: function () {
                                    LoadComments($(document).find('#EquipData_EquipmentId').val());
                                    wobyTypeGraphData();
                                    generateEquipGraph();
                                    SetEquipmentDetailEnvironment();
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

});
//#endregion
//#region V2-853 Reset Grid
$('#liResetGridClearBtn').click(function (e) {
    CancelAlertSetting.text = getResourceValue("ResetGridAlertMessage");
    swal(CancelAlertSetting, function () {
        var localstorageKeys = [];
        localstorageKeys.push("EQUIPMENTSEARCHGRIDDISPLAYSTATUS");
        localstorageKeys.push("EQUIPMENTSEARCHGRIDDISPLAYSTEXT");
        localstorageKeys.push("equipmentlayoutTypeval");
        localstorageKeys.push("ASSETVIEW");
        localstorageKeys.push("ASSETCURRENTCARDVIEWSTATE");
        DeleteGridLayout('Equipment_Search', dtTable, localstorageKeys);
        GenerateSearchList('', true);
        window.location.href = "../Equipment/Index?page=Maintenance_Assets";
    });
});
//#endregion

//#region V2-1023
$(document).on('keyup', '#euipmentSearch_paginate .paginate_input', function () {
    //#region V2-1159
    if (layoutType == 1) {
        var currentselectedpage = parseInt($(document).find('#txtcurrentpage').val());
        var lastpage = parseInt($(document).find('#spntotalpages').text());
        if (currentselectedpage > lastpage) {
            currentselectedpage = lastpage;
        }
        if (currentselectedpage < 1) {
            currentselectedpage = 1;
        }
        srchcardviewlength = $(document).find('#searchcardviewpagelengthdrp').val();
        srchcardviewstartvalue = srchcardviewlength * (currentselectedpage - 1);
        srchgrdcardcurrentpage = currentselectedpage;

        LayoutUpdate('pagination');
        ShowCardView();
    }
    //#endregion
    run = true;
});
//#endregion

//#region V2-846
function generateEquipTreeTable() {
    if ($(document).find('#tblTree').hasClass('dataTable')) {
        dtEquipTreeTable.destroy();
    }
    dtEquipTreeTable = $("#tblTree").DataTable({
        colReorder: {
            fixedColumnsLeft: 2
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        scrollX: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/EquipmentHierarchyTreeGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#txtSearch').val());
            },
            "dataSrc": function (result) {
                var i = 0;
                totalcount = result.recordsTotal;
                return result.data;
            },
            global: true
        },
        "columns":
            [
                {
                    "data": "EquipmentId",
                    "bVisible": true,
                    "bSortable": false,
                    "autoWidth": false,
                    "bSearchable": false,
                    "mRender": function (data, type, row) {
                        if (row.ChildCount > 0) {
                            return '<img id="' + data + '" src="../../Images/details_open.png" class="showChildTree" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "ClientLookupId",
                    "autoWidth": true,
                    "bSearchable": false,
                    "bSortable": false,
                    "className": "text-left",
                    "name": "1"
                },
                {
                    "data": "EquipmentId", "autoWidth": true, "bSearchable": false, "bSortable": false, "name": "2",
                    "mRender": function (data, type, row) {
                        if ($(document).find("#EquipData_EquipmentId").val() == data) {
                            return '<input type="checkbox" name="id[]" checked="checked" data-equipmentid="' + data + '" class="radEQSelect">';
                        }
                        else {
                            return '<input type="checkbox" name="id[]" data-equipmentid="' + data + '" class="radEQSelect">';
                        }
                    }
                }
            ],
        columnDefs: [
            {
                targets: [0, 1, 2],
                className: 'noVis'
            }
        ],
        initComplete: function (settings, json) {
            SetPageLengthMenu();
            CloseLoader();
        }
    });
}
$(document).on('click', '.showChildTree', function (e) {
    var tr = $(this).closest('tr');
    var row;
    if ($(this).hasClass('innerChild')) {
        row = dtinnerGrid.row(tr);
    }
    else {
        row = dtEquipTreeTable.row(tr);
    }
    if (this.src.match('details_close')) {
        this.src = "../../Images/details_open.png";
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        this.src = "../../Images/details_close.png";
        var EquipmentID = $(this).attr("rel");
        fetchChildData(EquipmentID, row, 1); // Pass the initial level (1)
        tr.addClass('shown');
    }
});
function fetchChildData(parentId, parentRow, level) {
    $.ajax({
        url: '/Base/GetEquipmentTreeChildGridData', 
        method: 'GET',
        data: { EquipmentId: parentId },
        success: function (response) {
            var childContent = '<table class="display childTable" cellpadding="5" cellspacing="0" border="0" style="padding-left:10px;">' +
                '<thead style="display:none;"><tr><th></th><th></th><th></th></tr></thead><tbody>';

            for (var i = 0; i < response.length; i++) {
                var childData = response[i];
                var expandbtn = '';
                var equipmentId = $(document).find("#EquipData_EquipmentId").val();
                var chkbox = '';
                if (childData.ChildCount > 0) {
                    expandbtn = '<img id="' + childData.EquipmentId + '" src="../../Images/details_open.png" alt="expand/collapse" class="innerChild" rel="' + childData.EquipmentId + '" style="cursor: pointer;" />';
                }
                if (childData.EquipmentId == equipmentId) {
                    chkbox = '<input type="checkbox" name="id[]" data-equipmentid="' + childData.EquipmentId + '" checked="checked" class="radEQSelect">';
                }
                else {
                    chkbox = '<input type="checkbox" name="id[]" data-equipmentid="' + childData.EquipmentId + '" class="radEQSelect">';
                }
                childContent += '<tr>' +
                    '<td class="details-control">' + 
                    expandbtn +
                    '</td>' +
                    '<td>' + childData.ClientLookupId + '</td>' +
                    '<td>' + chkbox +'</td>' +
                    '</tr>';
            }

            childContent += '</tbody></table>';

            parentRow.child(childContent).show();

            parentRow.child().find('table.childTable').DataTable({
                paging: false,
                searching: false,
                ordering: false,
                "bInfo": false,
            });
            // Add click handler for new child rows
            parentRow.child().find('.innerChild').on('click', function () {
                var tr = $(this).closest('tr');
                var rowId = $(this).attr('rel');
                var childRow = parentRow.child().find('table').DataTable().row(tr);

                // Ensure that the correct child row is handled
                if (childRow.child.isShown()) {
                    this.src = "../../Images/details_open.png";
                    // This row is already open - close it
                    childRow.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    this.src = "../../Images/details_close.png";
                    // Open this row
                    fetchChildData(rowId, childRow, level + 1); // Recursively load the next level
                    tr.addClass('shown');
                }
            });
        },
        error: function (xhr, status, error) {
            console.log('Failed to fetch child data:', error);
        }
    });
}
//#region V2-1134
//$(document).on('change', '#txtSearch', function (e) {
//    generateEquipTreeTable();// V2-846
//});
//#endregion
//#endregion

//#region V2-1115
$(document).on('click', '#printEqDetailsEPMQrCode', function () {
    var equips = $('#qRCodeModel_EquipmentIdsList_0_').val();
    generateQRforEPM(equips)
});
function generateQRforEPM(equipClientLookups) {
    $.ajax({
        type: "POST",
        url: "/Equipment/SetEquipmentIdlistforEPM",
        data: {
            EquipClientLookups: equipClientLookups
        },
        success: function (data) {
            if (data.success === 0) {
                window.open('/Equipment/GenerateEPMEquipmentQRcode', '_blank');
                equiToupdate = [];
                equiToClientLookupIdbarcode = [];
                //-- when called from grid         
                if ($(document).find('#euipmentSearch').find('.chksearch').length > 0) {
                    $('#printQrcode').prop("disabled", "disabled");
                    $('.itemQRcount').text(0);
                    $('.itemcount').text(0);
                    $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
                    $(document).find(".updateArea").hide();
                    $(document).find(".actionBar").fadeIn();
                }
                //--
            }
        },
        error: function (xhr, status, error) {
            console.error("Error generating QR code:", error);
        }
    });
}
//#endregion
//#region V2-1159 Change View
var layoutType = 2;
var srchcardviewstartvalue = 0;
var srchcardviewlength = 10;
var srchgrdcardcurrentpage = 1;
var currentorderedcolumn = 1;
var currentorder = 'asc';
var layoutVal;
$(document).on('click', "#cardviewliLayout", function () {
    if (layoutType == 1) {
        return;
    }
    ShowbtnLoader("layoutsortmenu");
    layoutType = 1;
    localStorage.setItem("equipmentlayoutTypeval", layoutType);
    layoutVal = $(document).find('#cardviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#Active').hide();
    $(document).find('#ActiveCard').show();
    HidebtnLoader("layoutsortmenu");
    $(document).find('#liCustomize').prop("disabled", true);
    var info = dtTable.page.info();
    var pageclicked = info.page;
    srchcardviewlength = info.length;
    srchcardviewstartvalue = srchcardviewlength * pageclicked;
    srchgrdcardcurrentpage = pageclicked + 1;
    currentorderedcolumn = order;
    currentorder = orderDir;
    //In browser when Card view selected then loading page to stay Card view (End)
    GetDatatableLayout();
    ShowCardView();
    var currentcardviewstate = new cardviewstate(srchgrdcardcurrentpage, srchcardviewstartvalue, srchcardviewlength, currentorderedcolumn, $('#btnsortmenu').text(), currentorder);
    localStorage.setItem("ASSETCURRENTCARDVIEWSTATE", JSON.stringify(currentcardviewstate));
    localStorage.setItem("ASSETVIEW", "CV");
});
function cardviewstate(currentpage, start, length, currentorderedcolumn, sorttext, order) {
    this.currentpage = currentpage;
    this.start = start;
    this.length = length;
    this.currentorderedcolumn = currentorderedcolumn;
    this.sorttext = sorttext;
    this.order = order;
}
function ShowCardView() {
    $.ajax({
        url: '/Equipment/GetCardViewData',
        type: 'POST',
        data: {
            currentpage: srchgrdcardcurrentpage,
            start: srchcardviewstartvalue,
            length: srchcardviewlength,
            currentorderedcolumn: currentorderedcolumn,
            currentorder: currentorder,
            CustomQueryDisplayId: localStorage.getItem("EQUIPMENTSEARCHGRIDDISPLAYSTATUS"),
            ClientLookupId: LRTrim($("#EquipmentID").val()),
            Name: LRTrim($("#Name").val()),
            Location: LRTrim($("#Location").val()),
            AssetGroup1Id: LRTrim($('#AssetGroup1Id').val()),
            AssetGroup2Id: LRTrim($("#AssetGroup2Id").val()),
            AssetGroup3Id: LRTrim($("#AssetGroup3Id").val()),
            LaborAccountClientLookupId: LRTrim($("#AccountSearchId").val()),
            SerialNumber: LRTrim($("#SerialNumber").val()),
            Type: LRTrim($('#ddlType').val()),
            Make: LRTrim($("#Make").val()),
            Model: LRTrim($("#ModelNumber").val()),
            AssetNumber: LRTrim($("#AssetsNumber").val()),
            AssetAvailability: (AssetAvailability != null) ? AssetAvailability : LRTrim($('#ddlassetAvailability').val()),
            SearchText: LRTrim($(document).find('#txtColumnSearch').val())
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $(document).find('#ActiveCard').show();
            $(document).find('#ActiveCard').html(data).show();
            $(document).find('#euipmentSearch_paginate li').each(function (index, value) {
                $(this).removeClass('active');
                if ($(this).data('currentpage') == srchgrdcardcurrentpage) {
                    $(this).addClass('active');
                }
            });
        },
        complete: function () {
            if ($(document).find('#spnNoData').length > 0) {
                $(document).find('.import-export').prop('disabled', true);
            }
            else {
                $(document).find('.import-export').prop('disabled', false);
            }
            //#region Page Navigation Show Hide
            if ($(document).find('#spntotalpages').text() <= 1) {
                $(document).find('.pagenavdiv').hide();
            }
            else {
                $(document).find('.pagenavdiv').show();
            }
            //#endregion

            $(document).find('#searchcardviewpagelengthdrp').select2({ minimumResultsForSearch: -1 }).val(srchcardviewlength).trigger('change.select2');
            HidebtnLoader("layoutsortmenu");
            HidebtnLoader("SrchBttnNew");
            HidebtnLoader("sidebarCollapse");
            HidebtnLoader("txtColumnSearch");
            HidebtnLoader("AddEquip");

            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', "#tableviewliLayout", function () {
    if (layoutType == 2) {
        return;
    }
    $(document).find('#euipmentSearch').show();
    layoutType = 2;
    localStorage.setItem("equipmentlayoutTypeval", layoutType);
    ShowbtnLoader("layoutsortmenu");
    layoutVal = $(document).find('#tableviewliLayout').text();
    $('#layoutsortmenu').text("Layout" + " : " + layoutVal);
    $(document).find('#ActiveCard').hide();
    $(document).find('#Active').show();
    $('#liCustomize').prop("disabled", false);
    HidebtnLoader("layoutsortmenu");
    localStorage.removeItem("ASSETVIEW");
    localStorage.removeItem("ASSETCURRENTCARDVIEWSTATE");
    if (dtTable) {
        dtTable.page.len(srchcardviewlength).order([[currentorderedcolumn, currentorder]]).page(srchgrdcardcurrentpage - 1).draw('page');
        $(document).find('#euipmentSearch_length .searchdt-menu').val(srchcardviewlength).trigger('change.select2');
    }
    else {
        generateEquipmentDataTable();
    }

});

$(document).on('change', '#searchcardviewpagelengthdrp', function () {
    srchcardviewlength = $(this).val();
    srchgrdcardcurrentpage = parseInt(srchcardviewstartvalue / srchcardviewlength) + 1;
    srchcardviewstartvalue = parseInt((grdcardcurrentpage - 1) * srchcardviewlength) + 1;

    LayoutUpdate('pagelength');
    ShowCardView();
});
//#region load CardView with previous state
var DefaultLayoutInfo = '{"time":currentTime,"start":0,"length":10,"order":[[1,"asc"]],"search":{"search":"","smart":true,"regex":false,"caseInsensitive":true},"columns":[],"ColReorder":[]}';
function GetDatatableLayout() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "Equipment_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var LayoutInfo = JSON.parse(json.LayoutInfo);
                var pageclicked = (LayoutInfo.start / LayoutInfo.length);
                srchcardviewlength = LayoutInfo.length;
                srchcardviewstartvalue = srchcardviewlength * pageclicked;
                srchgrdcardcurrentpage = pageclicked + 1;
                order = LayoutInfo.order[0][0];
                orderDir = LayoutInfo.order[0][1];

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                }
            }
            else {
                DefaultLayoutInfo = DefaultLayoutInfo.replace('currentTime', new Date().getTime());
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Equipment_Search",
                        LayOutInfo: (DefaultLayoutInfo),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
//#endregion
//#region Card view griddatalayout update
//For column , order , page and page length change
function LayoutUpdate(area) {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "Equipment_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {

            hGridfilteritemcount = 0;
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                if (area === 'column' || area === 'order') {
                    gridstate.order[0] = [currentorderedcolumn, currentorder];
                }
                else if (area === 'pagination') {//
                }
                else if (area === 'pagelength') {
                    gridstate.length = srchcardviewlength;
                }

                if (json.FilterInfo !== '') {
                    setsearchui(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $("#spnControlCounter"), $("#dvFilterSearchSelect2"));
                }
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Equipment_Search",
                        LayOutInfo: JSON.stringify(gridstate)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
function LayoutFilterinfoUpdate() {
    $.ajax({
        "url": "/Base/GetLayout",
        "data": {
            GridName: "WorkOrder_Search"
        },
        "async": false,
        "dataType": "json",
        "success": function (json) {
            if (json.LayoutInfo !== '') {
                var gridstate = JSON.parse(json.LayoutInfo);
                gridstate.start = cardviewstartvalue;
                var filterinfoarray = getfilterinfoarray($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": gridStateSaveUrl,
                    "data": {
                        GridName: "Equipment_Search",
                        LayOutInfo: JSON.stringify(gridstate),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "async": false,
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
        }
    });
}
//#endregion
//#endregion