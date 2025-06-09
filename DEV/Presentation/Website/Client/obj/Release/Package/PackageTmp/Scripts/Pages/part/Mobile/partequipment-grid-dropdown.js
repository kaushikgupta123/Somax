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
//function generateEquipmentDataTable_Mobile() {
//    //var Search = $(document).find('#txtEquipmentSearch_Mobile').val();
//    AssetListlength = 0;
//    $.ajax({
//        "url": "/Dashboard/EquipmentLookupListView_Mobile",
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
//            $('#DivEquipmentSearchScrollViewModal').html(data);
//            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
//        },
//        complete: function () {
//            //BindEquipmentScrollViewOfMobiScroll();
//            InitializeAssetListView_Mobile();
//            //if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
//            //    $(document).find('#EquipmentWOModal').modal("show");
//            //}
//            $('#EquipmentWOModal').addClass('slide-active');
//            CloseLoader();
//        },
//        error: function () {
//            CloseLoader();
//        }
//    });
//}
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
    if ($(document).find('#txtChargeToId').length > 0) {
        $(document).find('#txtChargeToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val($(this).data('id'));
        $(document).find('#txtChargeToId').closest('form').valid();
    }
    $('#partcheckoutEquipmentModalHide').trigger('click');
});
$(document).on('click', '#partcheckoutEquipmentModalHide', function () {
    AssetListlength = 0;
    $('#partcheckoutEquipmentModal').removeClass('slide-active');
    $('#txtEquipmentSearch_Mobile').val('');
});
$(document).on("keyup", '#txtEquipmentSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#AssetListViewForSearch").html("");
        InitializeAssetListView_Mobile();
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
        "url": "/Parts/GetEquipmentLookupListchunksearch_Mobile",
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

$(document).on('click', '#imgChargeToTree', function (e) {
    $(this).blur();
    generateEquipmentTree($(this).data('assignedid'));
});

function generateEquipmentTree(assignedid) {
    $.ajax({
        url: '/PlantLocationTree/EquipmentHierarchyTree',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
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
                //if ($(document).find('#hdnChargeTo').val() == '0' || $(document).find('#hdnChargeTo').val() == '') {

                //    if ($(this).data('equipmentid') === equipmentid) {
                //        $(this).attr('checked', true);
                //    }
                //}
                //else {
                var c_id = $(this).data('clientlookupid').split("(")[0];
                console.log($(document).find('#' + assignedid).val());
                if (c_id != null && c_id.trim() == $(document).find('#' + assignedid).val().trim()) {
                    $(this).attr('checked', true);
                }
                //}
            });
            $.each($(document).find('#tblTree > tbody > tr').find('img[src="data:image/png;base64,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"]'), function (i, elem) {
                var parentId = elem.parentNode.parentNode.getAttribute('data-tt-id');
                $(document).find('#tblTree > tbody > tr[data-tt-id=' + parentId + ']').trigger('click');
            });
            //-- collapse all element
            $('#commonWOTreeModal').addClass('slide-active');//.trigger('mbsc-enhance');

            $('#tblTree tr td').removeAttr('style');// code to remove the style applied from treetable.js -- white-space: nowrap;
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

$(document).on('change', '.radSelect', function () {
    var equipmentid = $(this).data('equipmentid');
    $(document).find('#hdnChargeToId').val(equipmentid);
    var clientlookupid = $(this).data('clientlookupid').split("(")[0];
    $('#commonWOTreeModal').removeClass('slide-active');
    //$(document).find('#inventoryCheckoutModel_ChargeToClientLookupId').val(equipmentid).trigger('change');
    $(document).find('#txtChargeToId').val(clientlookupid).trigger('mbsc-enhance');
    $(document).find('#txtChargeToId').closest('form').valid(); //.val(clientlookupid).removeClass('input-validation-error');
});
$(document).on('click', "#commonWOTreeModalHide", function () {
    $('#commonWOTreeModal').removeClass('slide-active');
});