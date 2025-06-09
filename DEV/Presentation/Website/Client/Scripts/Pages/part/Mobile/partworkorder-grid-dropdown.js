//var equipTable;
//var eqClientLookupId = "";
//var NameEq = "";
//var Model = "";
//var Type = "";
//var SerialNumber = "";
//var WOGroup1ClientLookupId = "";
//var WOGroup2ClientLookupId = "";
//var WOGroup3ClientLookupId = "";

//function generateWorkOrderDataTable() {
//    var rCount = 0;
//    var visibility;
//    if ($(document).find('#WorkOrderWOTable').hasClass('dataTable')) {
//        equipTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    equipTable = $("#WorkOrderWOTable").DataTable({
//        searching: true,
//        serverSide: true,
//        "pagingType": "full_numbers",
//        "bProcessing": true,
//        "bDeferRender": true,
//        "order": [[0, "asc"]],
//        "pageLength": 10,
//        stateSave: true,
//        language: {
//            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
//        },
//        sDom: 'Btlipr',
//        buttons: [],
//        "orderMulti": true,
//        "ajax": {
//            "url": "/Base/GetWorkOrderLookupListchunksearch",
//            data: function (d) {
//                d.ClientLookupId = eqClientLookupId;
//                d.Name = NameEq;
//                d.Model = Model;
//                d.Type = Type;
//                d.WOGroup1ClientLookupId = WOGroup1ClientLookupId;
//                d.WOGroup2ClientLookupId = WOGroup2ClientLookupId;
//                d.WOGroup3ClientLookupId = WOGroup3ClientLookupId;
//                d.SerialNumber = SerialNumber;
//                d.InactiveFlag = false;
//            },
//            "type": "POST",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                rCount = json.data.length;
//                return json.data;
//            }
//        },
//        "columns":
//            [
//                {
//                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<a class=link_woeqp_detail href="javascript:void(0)">' + data + '</a>'
//                    }
//                },
//                { "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                { "data": "Model", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                {
//                    "data": "Type", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<span class="m-badge--custom">' + data + '</span>'
//                    }
//                },

//                { "data": "WOGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "WOGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "WOGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "SerialNumber", "autoWidth": true, "bSearchable": true, "bSortable": true }
//            ],
//        "rowCallback": function (row, data, index, full) {
//            var colType = this.api().column(3).index('visible');
//            if (data.Type) {
//                var color = "#" + intToARGB(hashCode(LRTrim(data.Type)));
//                $('td', row).eq(colType).find('.m-badge--custom').css('background-color', color).css('color', '#fff');
//            }
//        },
//        initComplete: function () {
//            $(document).find('#tblWorkOrderWOfooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#WorkOrderWOModal').hasClass('show')) {
//                $(document).find('#WorkOrderWOModal').modal("show");
//            }
//            $('#WorkOrderWOTable tfoot th').each(function (i, v) {
//                var colIndex = i;
//                var title = $('#WorkOrderWOTable thead th').eq($(this).index()).text();
//                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
//                if (NameEq) { $('#woequipmentcolindex_1').val(NameEq); }
//                if (Model) { $('#woequipmentcolindex_2').val(Model); }
//                if (Type) { $('#woequipmentcolindex_3').val(Type); }
//                if (WOGroup1ClientLookupId) { $('#woequipmentcolindex_4').val(WOGroup1ClientLookupId); }
//                if (WOGroup2ClientLookupId) { $('#woequipmentcolindex_5').val(WOGroup2ClientLookupId); }
//                if (WOGroup3ClientLookupId) { $('#woequipmentcolindex_6').val(WOGroup3ClientLookupId); }
//                if (SerialNumber) { $('#woequipmentcolindex_7').val(SerialNumber); }
//            });

//            $('#WorkOrderWOTable tfoot th').find('.woequipment').on("keyup", function (e) {
//                if (e.keyCode == 13) {
//                    var thisId = $(this).attr('id');
//                    var colIdx = thisId.split('_')[1];
//                    var searchText = LRTrim($(this).val());
//                    eqClientLookupId = $('#woequipmentcolindex_0').val();
//                    NameEq = $('#woequipmentcolindex_1').val();
//                    Model = $('#woequipmentcolindex_2').val();
//                    Type = $('#woequipmentcolindex_3').val();
//                    WOGroup1ClientLookupId = $('#woequipmentcolindex_4').val();
//                    WOGroup2ClientLookupId = $('#woequipmentcolindex_5').val();
//                    WOGroup3ClientLookupId = $('#woequipmentcolindex_6').val();
//                    SerialNumber = $('#woequipmentcolindex_7').val();
//                    equipTable.page('first').draw('page');
//                }
//            });
//        }
//    });
//}
//function generateWorkOrderDataTable_Mobile() {
//    //var Search = $(document).find('#txtWorkOrderSearch_Mobile').val();
//    WOListlength = 0;
//    $.ajax({
//        "url": "/Dashboard/WorkOrderLookupListView_Mobile",
//        type: "POST",
//        dataType: "html",
//        data: {
//            //Search: Search,
//            //Name: Search
//        },
//        beforeSend: function () {
//            ShowLoader();
//        },
//        success: function (data) {
//            $('#DivWorkOrderSearchScrollViewModal').html(data);
//            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
//        },
//        complete: function () {
//            //BindWorkOrderScrollViewOfMobiScroll();
//            InitializeWOListView_Mobile();
//            //if (!$(document).find('#WorkOrderWOModal').hasClass('show')) {
//            //    $(document).find('#WorkOrderWOModal').modal("show");
//            //}
//            $('#WorkOrderWOModal').addClass('slide-active');
//            CloseLoader();
//        },
//        error: function () {
//            CloseLoader();
//        }
//    });
//}
//function BindWorkOrderScrollViewOfMobiScroll() {
//    var $categoryNav,
//        $contentView

//    // init top tabs
//    $categoryNav = $('#WorkOrder-md-category').mobiscroll().nav({
//        type: 'tab',
//        onItemTap: function (event, inst) {
//            $contentView.mobiscroll('navigate', $('.' + $(event.target).data('page')));
//        }

//    });

//    $contentView = $('#WorkOrder-md-content').mobiscroll().scrollview({
//        layout: 1,
//        paging: true,
//        threshold: 15,
//        cssClass: 'md-page',
//        onAnimationStart: function (event, inst) {
//            var selectedIndex = parseInt((-(event.destinationX / inst.contWidth)).toString(), 10),
//                $selectedItem = $categoryNav.find('li').eq(selectedIndex);

//            if (!$selectedItem.hasClass('mbsc-ms-item-sel')) {
//                $categoryNav.mobiscroll('navigate', $selectedItem);
//            }
//        }
//    });

//    $('.WorkOrder-md-list').mobiscroll().listview({
//        swipe: false,
//        striped: true,
//        enhance: true,
//    });
//    $('#WorkOrderWOModal').trigger('mbsc-enhance');
//}
//$(document).ready(function () {
//    $(window).keydown(function (event) {
//        if (event.keyCode == 13) {
//            event.preventDefault();
//            return false;
//        }
//    });
//});
$(document).on('click', '.scrollview-workordersearch', function (e) {
    //var index_row = $('#WorkOrderWOTable tr').index($(this).closest('tr')) - 1;
    //var row = $(this).parents('tr');
    //var td = $(this).parents('tr').find('td');
    //var data = equipTable.row(row).data();
    if ($(document).find('#txtChargeToId').length > 0) {
        $(document).find('#txtChargeToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val($(this).data('id'));
        //$('#workOrderModel_ChargeTo_Name').val($(this).data('chargetoname'));
        $(document).find('#txtChargeToId').closest('form').valid();
    }
    //else if ($(document).find('#ChargeToClientLookupId').length > 0) {
    //    // work request dynamic and completion wizard
    //    $(document).find('#ChargeToClientLookupId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
    //    //.removeClass('input-validation-error');// work request dynamic        
    //    $(document).find('#ChargeToClientLookupId').parents('div').eq(0).css('display', 'block');
    //    $(document).find('#ChargeToId').val($(this).data('id'));
    //    $(document).find('#ChargeToId').parents('div').eq(0).css('display', 'none');
    //    $(document).find('#ChargeToClientLookupId').closest('form').valid();
    //    // work request dynamic and completion wizard
    //}

    //$(document).find("#WorkOrderWOModal").modal('hide');
    $('#partcheckoutWorkOrderModalHide').trigger('click');
});
$(document).on('click', '#partcheckoutWorkOrderModalHide', function () {
    //$(document).find('#WorkOrderWOModal').modal("hide");
    WOListlength = 0;
    $('#partcheckoutWorkOrderModal').removeClass('slide-active');
    $('#txtWorkOrderSearch_Mobile').val('');
    //$('#DivWorkOrderSearchScrollViewModal').html('');
});
//$(document).on('hidden.bs.modal', '#WorkOrderWOModal', function () {
//    $('#txtWorkOrderSearch_Mobile').val('');
//    $('#DivWorkOrderSearchScrollViewModal').html('');
//});
$(document).on("keyup", '#txtWorkOrderSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#WOListViewForSearch").html("");
        InitializeWOListView_Mobile();
    }
});
var WOListView,
    WOListlength = 0,
    PageLength = 100;
function InitializeWOListView_Mobile() {
    WOListView = $('#WOListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindWODataForListView();
    $('#WorkOrderWOModal').addClass('slide-active');
    //$('#WorkOrderWOModal').trigger('mbsc-enhance');
}
$(document).on('click', '#btnWOLoadMore', function () {
    $(this).hide();
    WOListlength += PageLength;
    InitializeWOListView_Mobile();
});
function BindWODataForListView() {
    var Search = $(document).find('#txtWorkOrderSearch_Mobile').val();
    $.ajax({
        "url": "/Parts/GetWorkOrderLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: WOListlength,
            Length: PageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            WOListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-workordersearch" data-chargetoname="' + item.ChargeTo + '" data-id="' + item.WorkOrderId + '"; data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.ChargeTo + ')</li>';
                WOListView.add(null, mobiscroll.$(lihtml));
            }
            if ((WOListlength + PageLength) < data.recordsTotal) {
                $('#btnWOLoadMore').show();
            }
            else {
                $('#btnWOLoadMore').hide();
            }
        },
        complete: function () {
            WOListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}