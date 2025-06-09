//var equipTable;
//var eqClientLookupId = "";
//var NameEq = "";
//var Model = "";
//var Type = "";
//var SerialNumber = "";
//var LocGroup1ClientLookupId = "";
//var LocGroup2ClientLookupId = "";
//var LocGroup3ClientLookupId = "";

//function generateLocationDataTable() {
//    var rCount = 0;
//    var visibility;
//    if ($(document).find('#LocationWOTable').hasClass('dataTable')) {
//        equipTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    equipTable = $("#LocationWOTable").DataTable({
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
//            "url": "/Base/GetLocationLookupListchunksearch",
//            data: function (d) {
//                d.ClientLookupId = eqClientLookupId;
//                d.Name = NameEq;
//                d.Model = Model;
//                d.Type = Type;
//                d.LocGroup1ClientLookupId = LocGroup1ClientLookupId;
//                d.LocGroup2ClientLookupId = LocGroup2ClientLookupId;
//                d.LocGroup3ClientLookupId = LocGroup3ClientLookupId;
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

//                { "data": "LocGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "LocGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "LocGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
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
//            $(document).find('#tblLocationWOfooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#LocationWOModal').hasClass('show')) {
//                $(document).find('#LocationWOModal').modal("show");
//            }
//            $('#LocationWOTable tfoot th').each(function (i, v) {
//                var colIndex = i;
//                var title = $('#LocationWOTable thead th').eq($(this).index()).text();
//                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
//                if (NameEq) { $('#woequipmentcolindex_1').val(NameEq); }
//                if (Model) { $('#woequipmentcolindex_2').val(Model); }
//                if (Type) { $('#woequipmentcolindex_3').val(Type); }
//                if (LocGroup1ClientLookupId) { $('#woequipmentcolindex_4').val(LocGroup1ClientLookupId); }
//                if (LocGroup2ClientLookupId) { $('#woequipmentcolindex_5').val(LocGroup2ClientLookupId); }
//                if (LocGroup3ClientLookupId) { $('#woequipmentcolindex_6').val(LocGroup3ClientLookupId); }
//                if (SerialNumber) { $('#woequipmentcolindex_7').val(SerialNumber); }
//            });

//            $('#LocationWOTable tfoot th').find('.woequipment').on("keyup", function (e) {
//                if (e.keyCode == 13) {
//                    var thisId = $(this).attr('id');
//                    var colIdx = thisId.split('_')[1];
//                    var searchText = LRTrim($(this).val());
//                    eqClientLookupId = $('#woequipmentcolindex_0').val();
//                    NameEq = $('#woequipmentcolindex_1').val();
//                    Model = $('#woequipmentcolindex_2').val();
//                    Type = $('#woequipmentcolindex_3').val();
//                    LocGroup1ClientLookupId = $('#woequipmentcolindex_4').val();
//                    LocGroup2ClientLookupId = $('#woequipmentcolindex_5').val();
//                    LocGroup3ClientLookupId = $('#woequipmentcolindex_6').val();
//                    SerialNumber = $('#woequipmentcolindex_7').val();
//                    equipTable.page('first').draw('page');
//                }
//            });
//        }
//    });
//}
function generateLocationDataTable_Mobile() {
    //var Search = $(document).find('#txtLocationSearch_Mobile').val();
    LocListlength = 0;
    $.ajax({
        "url": "/Dashboard/LocationLookupListView_Mobile",
        type: "POST",
        dataType: "html",
        data: {
            //Search: Search,
            //Name: Search
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivLocationSearchScrollViewModal').html(data);
            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            //BindLocationScrollViewOfMobiScroll();
            InitializeLocListView_Mobile();
            //if (!$(document).find('#LocationWOModal').hasClass('show')) {
            //    $(document).find('#LocationWOModal').modal("show");
            //}
            $('#LocationWOModal').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//function BindLocationScrollViewOfMobiScroll() {
//    var $categoryNav,
//        $contentView

//    // init top tabs
//    $categoryNav = $('#Location-md-category').mobiscroll().nav({
//        type: 'tab',
//        onItemTap: function (event, inst) {
//            $contentView.mobiscroll('navigate', $('.' + $(event.target).data('page')));
//        }

//    });

//    $contentView = $('#Location-md-content').mobiscroll().scrollview({
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

//    $('.Location-md-list').mobiscroll().listview({
//        swipe: false,
//        striped: true,
//        enhance: true,
//    });
//    $('#LocationWOModal').trigger('mbsc-enhance');
//}
//$(document).ready(function () {
//    $(window).keydown(function (event) {
//        if (event.keyCode == 13) {
//            event.preventDefault();
//            return false;
//        }
//    });
//});
$(document).on('click', '.scrollview-locationsearch', function (e) {
    //var index_row = $('#LocationWOTable tr').index($(this).closest('tr')) - 1;
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

    //$(document).find("#LocationWOModal").modal('hide');
    $('#partcheckoutLocationModalHide').trigger('click');
});
$(document).on('click', '#partcheckoutLocationModalHide', function () {
    //$(document).find('#LocationWOModal').modal("hide");
    LocListlength = 0;
    $('#partcheckoutLocationModal').removeClass('slide-active');
    $('#txtLocationSearch_Mobile').val('');
    //$('#DivLocationSearchScrollViewModal').html('');
});
//$(document).on('hidden.bs.modal', '#LocationWOModal', function () {
//    $('#txtLocationSearch_Mobile').val('');
//    $('#DivLocationSearchScrollViewModal').html('');
//});
$(document).on("keyup", '#txtLocationSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#LocListViewForSearch").html("");
        InitializeLocListView_Mobile();
    }
});
var LocListView,
    LocListlength = 0,
    PageLength = 100;
function InitializeLocListView_Mobile() {
    LocListView = $('#LocListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindLocDataForListView();
    $('#LocationWOModal').addClass('slide-active');
    //$('#LocationWOModal').trigger('mbsc-enhance');
}
$(document).on('click', '#btnLocLoadMore', function () {
    $(this).hide();
    LocListlength += PageLength;
    InitializeLocListView_Mobile();
});
function BindLocDataForListView() {
    var Search = $(document).find('#txtLocationSearch_Mobile').val();
    $.ajax({
        "url": "/Parts/GetLocationLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: LocListlength,
            Length: PageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            LocListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-locationsearch" data-chargetoname="' + item.Name + '" data-id="' + item.LocationId + '"; data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                LocListView.add(null, mobiscroll.$(lihtml));
            }
            if ((LocListlength + PageLength) < data.recordsTotal) {
                $('#btnLocLoadMore').show();
            }
            else {
                $('#btnLocLoadMore').hide();
            }
        },
        complete: function () {
            LocListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}