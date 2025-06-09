var rgridfilteritemcount = 0;
var dtReceipt;
var typeValVendor;
$(function () {
    $(document).find('#receiptadvsearchcontainer').find(".sidebar").mCustomScrollbar({
        theme: "minimal"
    });
$(document).on('click', "#downloadreceiptdata", function () {
    $(document).find('.receiptgridexport').trigger('click');
});
});
$(document).on('click', '.dismiss, .overlay', function () {
    $(document).find('#receiptadvsearchcontainer').find(".sidebar").removeClass('active');
    $('.overlay').fadeOut();
});
$(document).on('click', '#receiptssidebarCollapse', function () {
    $(document).find('#receiptadvsearchcontainer').find(".sidebar").addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
});
function generateReceiptDataTable() { 
    var partid = $(document).find('#PartModel_PartId').val();
    var daterange = $(document).find('#partsReceiptModel_receiptdtselector').val();  
    var partclientlookup = $(document).find('#partclientlookupid').text(); 
    var POClientLookupId = LRTrim($("#PurchaseOrder").val()) ;
    var ReceivedDate = LRTrim($("#ReceiptDate").val());
    var VendorClientLookupId = LRTrim($("#rgridReceiptadvsearchVendorClientLookupId").val());
    var VendorName = LRTrim($("#VendorName").val());
    var OrderQuantity = LRTrim($("#Quantity").val());
    var UnitCost = LRTrim($('#UnitCost').val());
    if (typeof dtReceipt !== "undefined") {
        dtReceipt.destroy();
    }
    dtReceipt = $("#receiptsTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[0, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excel',
                filename: partclientlookup + '_Receipts',
                extension: '.xlsx',
                className: "datatable-btn-export receiptgridexport"
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/Parts/populatePartReceipt",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.partid = partid;
                d.daterange = daterange;
                d.POClientLookupId = POClientLookupId;
                d.ReceivedDate = ReceivedDate;
                d.VendorClientLookupId = VendorClientLookupId;
                d.VendorName = VendorName;
                d.OrderQuantity = OrderQuantity;
                d.UnitCost = UnitCost;             
            },
            "dataSrc": function (json) {               
                var myDataSource = json;               
                var Vendor = [];
                var VendorObj;
                for (var i = 0; i < myDataSource.dataAll.length; ++i) {                    
                    VendorObj = new VendorsObjArray(myDataSource.dataAll[i].VendorClientLookupId, myDataSource.dataAll[i].VendorClientLookupId + '-' + myDataSource.dataAll[i].VendorName);
                    Vendor.push(VendorObj);
                }              
                Vendor = Vendor.filter(function (v) { return v.id !== '' });
                var result = [];
                $.each(Vendor, function (i, e) {
                    var matchingItems = $.grep(result, function (item) {
                        return item.id === e.id && item.name === e.name;
                    });
                    if (matchingItems.length === 0) result.push(e);
                });    
                var VenLen = result.length;
                $("#rgridReceiptadvsearchVendorClientLookupId").empty();
                $("#rgridReceiptadvsearchVendorClientLookupId").append("<option value='" + 0 + "'>" + "--Select--" + "</option>");
                for (var i = 0; i < VenLen; i++) {
                    var id = result[i].id;
                    var name = result[i].name;
                    $("#rgridReceiptadvsearchVendorClientLookupId").append("<option value='" + id + "'>" + name + "</option>");
                }             
                if (typeValVendor) {
                    $("#rgridReceiptadvsearchVendorClientLookupId").val(typeValVendor);
                }
                return json.data;
            }
        },
        "columns":
        [
            { "data": "POClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {"data": "ReceivedDate", "autoWidth": true, "bSearchable": true, "bSortable": true},          
            { "data": "VendorClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "VendorName", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "OrderQuantity", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "UnitCost", "autoWidth": true, "bSearchable": true, "bSortable": true }
        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}
$(document).on('change', '#partsReceiptModel_receiptdtselector', function () {
    dtReceipt.state.clear();
    RGridClearAdvanceSearch();
    generateReceiptDataTable();
});

function VendorsObjArray(id, name) {
    this.id = id;
    this.name = name;
}
$(document).on('click', '#btnReceiptDataAdvSrch', function () {
    dtReceipt.state.clear();
    var searchitemhtml = "";
    rgridfilteritemcount = 0
    $('#receiptgridadvsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).hasClass('dtpicker')) {
            $(this).val(ValidateDate($(this).val()));
        }
        if ($(this).val() && $(this).val() != "0") {          
            rgridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times rbtnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#rgridadvsearchfilteritems').html(searchitemhtml);
    $(document).find('#receiptadvsearchcontainer').find(".sidebar").removeClass('active');
    $('.overlay').fadeOut();
    RGridAdvanceSearch();
    typeValVendor = $("#rgridReceiptadvsearchVendorClientLookupId").val();
});
function RGridAdvanceSearch() {  
    var purchaseOrder = LRTrim($('#PurchaseOrder').val());
    var ReceiptDate = ValidateDate($('#ReceiptDate').val());
    var Vendor = $('#rgridReceiptadvsearchVendorClientLookupId').val();
    var VendorName = LRTrim($('#VendorName').val());
    var Quantity = LRTrim($('#Quantity').val());
    var UnitCost = LRTrim($('#UnitCost').val());
    generateReceiptDataTable();
    $('.receiptfilteritemcount').text(rgridfilteritemcount);
}
$(document).on('click', '#rgridClearAdvSearchFilter', function () {
    $(document).find('#partsReceiptModel_receiptdtselector').val("0").trigger('change.select2');
    RGridClearAdvanceSearch();
    generateReceiptDataTable();
});
function RGridClearAdvanceSearch() {
    var rgridfilteritemcount = 0;
    $(document).find("#rgridReceiptadvsearchVendorClientLookupId").val("").trigger('change');
    $('#receiptgridadvsearchsidebar').find('input:text').val('');
    $('.receiptfilteritemcount').text(rgridfilteritemcount);
    $('#rgridadvsearchfilteritems').find('span').html('').removeClass('tagTo');
    typeValVendor = $("#rgridReceiptadvsearchVendorClientLookupId").val();
}
$(document).on('click', '.rbtnCross', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    rgridfilteritemcount--;
    if (searchtxtId == "rgridReceiptadvsearchVendorClientLookupId") {
        typeValVendor = null;
    }
    RGridAdvanceSearch();
});

