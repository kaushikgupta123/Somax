//var equipTable;
//var eqClientLookupId = "";
//var NameEq = "";
//var Model = "";
//var Type = "";
//var SerialNumber = "";
//var AssetGroup1ClientLookupId = "";
//var AssetGroup2ClientLookupId = "";
//var AssetGroup3ClientLookupId = "";

//function generateEquipmentDataTable() {
//    var rCount = 0;
//    var visibility;
//    if ($(document).find('#EquipmentWOTable').hasClass('dataTable')) {
//        equipTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    equipTable = $("#EquipmentWOTable").DataTable({
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
//            "url": "/Base/GetEquipmentLookupListchunksearch",
//            data: function (d) {
//                d.ClientLookupId = eqClientLookupId;
//                d.Name = NameEq;
//                d.Model = Model;
//                d.Type = Type;
//                d.AssetGroup1ClientLookupId = AssetGroup1ClientLookupId;
//                d.AssetGroup2ClientLookupId = AssetGroup2ClientLookupId;
//                d.AssetGroup3ClientLookupId = AssetGroup3ClientLookupId;
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

//                { "data": "AssetGroup1ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "AssetGroup2ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
//                { "data": "AssetGroup3ClientLookupId", "autoWidth": true, "bSearchable": true, "orderable": true, "bSortable": true },
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
//            $(document).find('#tblEquipmentWOfooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
//                $(document).find('#EquipmentWOModal').modal("show");
//            }
//            $('#EquipmentWOTable tfoot th').each(function (i, v) {
//                var colIndex = i;
//                var title = $('#EquipmentWOTable thead th').eq($(this).index()).text();
//                $(this).html('<input type="text" style="width:100%" class="woequipment tfootsearchtxt" id="woequipmentcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (eqClientLookupId) { $('#woequipmentcolindex_0').val(eqClientLookupId); }
//                if (NameEq) { $('#woequipmentcolindex_1').val(NameEq); }
//                if (Model) { $('#woequipmentcolindex_2').val(Model); }
//                if (Type) { $('#woequipmentcolindex_3').val(Type); }
//                if (AssetGroup1ClientLookupId) { $('#woequipmentcolindex_4').val(AssetGroup1ClientLookupId); }
//                if (AssetGroup2ClientLookupId) { $('#woequipmentcolindex_5').val(AssetGroup2ClientLookupId); }
//                if (AssetGroup3ClientLookupId) { $('#woequipmentcolindex_6').val(AssetGroup3ClientLookupId); }
//                if (SerialNumber) { $('#woequipmentcolindex_7').val(SerialNumber); }
//            });

//            $('#EquipmentWOTable tfoot th').find('.woequipment').on("keyup", function (e) {
//                if (e.keyCode == 13) {
//                    var thisId = $(this).attr('id');
//                    var colIdx = thisId.split('_')[1];
//                    var searchText = LRTrim($(this).val());
//                    eqClientLookupId = $('#woequipmentcolindex_0').val();
//                    NameEq = $('#woequipmentcolindex_1').val();
//                    Model = $('#woequipmentcolindex_2').val();
//                    Type = $('#woequipmentcolindex_3').val();
//                    AssetGroup1ClientLookupId = $('#woequipmentcolindex_4').val();
//                    AssetGroup2ClientLookupId = $('#woequipmentcolindex_5').val();
//                    AssetGroup3ClientLookupId = $('#woequipmentcolindex_6').val();
//                    SerialNumber = $('#woequipmentcolindex_7').val();
//                    equipTable.page('first').draw('page');
//                }
//            });
//        }
//    });
//}
function generateEquipmentDataTable_Mobile() {
    //var Search = $(document).find('#txtEquipmentSearch_Mobile').val();
    AssetListlength = 0;
    $.ajax({
        "url": "/Dashboard/EquipmentLookupListView_Mobile",
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
            $('#DivEquipmentSearchScrollViewModal').html(data);
            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            //BindEquipmentScrollViewOfMobiScroll();
            InitializeAssetListView_Mobile();
            //if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
            //    $(document).find('#EquipmentWOModal').modal("show");
            //}
            $('#EquipmentWOModal').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//function BindEquipmentScrollViewOfMobiScroll() {
//    var $categoryNav,
//        $contentView

//    // init top tabs
//    $categoryNav = $('#Equipment-md-category').mobiscroll().nav({
//        type: 'tab',
//        onItemTap: function (event, inst) {
//            $contentView.mobiscroll('navigate', $('.' + $(event.target).data('page')));
//        }

//    });

//    $contentView = $('#Equipment-md-content').mobiscroll().scrollview({
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

//    $('.Equipment-md-list').mobiscroll().listview({
//        swipe: false,
//        striped: true,
//        enhance: true,
//    });
//    $('#EquipmentWOModal').trigger('mbsc-enhance');
//}
//$(document).ready(function () {
//    $(window).keydown(function (event) {
//        if (event.keyCode == 13) {
//            event.preventDefault();
//            return false;
//        }
//    });
//});
$(document).on('click', '.scrollview-equipmentsearch', function (e) {
    //var index_row = $('#EquipmentWOTable tr').index($(this).closest('tr')) - 1;
    //var row = $(this).parents('tr');
    //var td = $(this).parents('tr').find('td');
    //var data = equipTable.row(row).data();
    if ($(document).find('#txtChargeTo').length > 0) {
        $(document).find('#txtChargeTo').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');     
        $(document).find('#hdnChargeTo').val($(this).data('id'));
        $('#workOrderModel_ChargeTo_Name').val($(this).data('chargetoname'));
        $(document).find('#txtChargeTo').closest('form').valid();
    }
    else if ($(document).find('#ChargeToClientLookupId').length > 0) {
        // work request dynamic and completion wizard
        $(document).find('#ChargeToClientLookupId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
        //.removeClass('input-validation-error');// work request dynamic        
        $(document).find('#ChargeToClientLookupId').parents('div').eq(0).css('display', 'block');
        $(document).find('#ChargeToId').val($(this).data('id'));
        $(document).find('#ChargeToId').parents('div').eq(0).css('display', 'none');
        $(document).find('#ChargeToClientLookupId').closest('form').valid();
        // work request dynamic and completion wizard
    }

    //$(document).find("#EquipmentWOModal").modal('hide');
    $('#EquipmentWOModalHide').trigger('click');
    //V2-948
    var SourceAssetAccount = $(document).find("#SourceAssetAccount").val();
    if (SourceAssetAccount != undefined && SourceAssetAccount == "True") {
        getlaboraccount_mobile($(this).data('id'));

    }
    //V2-948
});
$(document).on('click', '#EquipmentWOModalHide', function () {
    //$(document).find('#EquipmentWOModal').modal("hide");
    AssetListlength = 0;
    $('#EquipmentWOModal').removeClass('slide-active');
    $('#txtEquipmentSearch_Mobile').val('');
    $('#DivEquipmentSearchScrollViewModal').html('');
});
//$(document).on('hidden.bs.modal', '#EquipmentWOModal', function () {
//    $('#txtEquipmentSearch_Mobile').val('');
//    $('#DivEquipmentSearchScrollViewModal').html('');
//});
$(document).on("keyup", '#txtEquipmentSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generateEquipmentDataTable_Mobile();
    }
});
var AssetListView,
    AssetListlength = 0,
    PageLength = 100;
function InitializeAssetListView_Mobile() {
    AssetListView = $('#AssetListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindAssetDataForListView();
    $('#EquipmentWOModal').addClass('slide-active');
    //$('#EquipmentWOModal').trigger('mbsc-enhance');
}
$(document).on('click', '#btnAssetLoadMore', function () {
    $(this).hide();
    AssetListlength += PageLength;
    InitializeAssetListView_Mobile();
});
function BindAssetDataForListView() {
    var Search = $(document).find('#txtEquipmentSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/GetEquipmentLookupListchunksearch_Mobile",// need to create a SP later on for mobile and change layers
        data: {
            Search: Search,
            Start: AssetListlength,
            Length: PageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            AssetListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-equipmentsearch" data-chargetoname="' + item.Name + '" data-id="' + item.EquipmentId + '"; data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                AssetListView.add(null, mobiscroll.$(lihtml));
            }
            if ((AssetListlength + PageLength) < data.recordsTotal) {
                $('#btnAssetLoadMore').show();
            }
            else {
                $('#btnAssetLoadMore').hide();
            }
        },
        complete: function () {
            AssetListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

//#region V2-948
function getlaboraccount_mobile(EquipmentId) {
    $.ajax({
        url: "../base/GetAccountByEquipmentId",
        type: 'GET',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: { EquipmentId: EquipmentId },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            CloseLoader();
            $(document).find("#Labor_AccountId").parents('div').eq(0).css("display", "none");
            $(document).find("#Labor_AccountId").val(data.data.Labor_AccountId);
            $(document).find("#AccountClientLookupId").parents('div').eq(0).css("display", "block");
            
            $(document).find("#AccountClientLookupId").val(data.data.LaborAccountClientLookupId).trigger('mbsc-enhance');
        },
        error: function () {
            CloseLoader();
        }
    });
}
//#endregion