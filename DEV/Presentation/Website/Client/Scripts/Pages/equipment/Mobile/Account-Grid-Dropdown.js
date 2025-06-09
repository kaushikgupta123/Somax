//var dtAccountTable;
//var gAccountClientLookupId = "";
//var gAccountName = "";
//var TextField = "";
//var ValueField = "";

$(document).on('click', '.OpenAccountModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateAccountDataTable_Mobile();
});
//$(document).on('click', '.ClearAccountModalPopupGridData', function () {
//    $(document).find('#' + $(this).data('textfield')).val('');
//    $(document).find('#' + $(this).data('valuefield')).val('');

//    if ($(document).find('#' + $(this).data('textfield')).css('display') == 'block') {
//        $(document).find('#' + $(this).data('textfield')).css('display', 'none');
//    }
//    if ($(document).find('#' + $(this).data('valuefield')).css('display') == 'none') {
//        $(document).find('#' + $(this).data('valuefield')).css('display', 'block');
//    }
//    $(this).css('display', 'none');
//});
//function generateAccountDataTable() {
//    if ($(document).find('#tblAccountModalPopup').hasClass('dataTable')) {
//        dtAccountTable.destroy();
//    }
//    mcxDialog.loading({ src: "../content/Images" });
//    dtAccountTable = $("#tblAccountModalPopup").DataTable({
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
//            "url": "/Base/GetAccountLookupList",
//            data: function (d) {
//                d.ClientLookupId = gAccountClientLookupId;
//                d.Name = gAccountName;
//            },
//            "type": "POST",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                //rCount = json.data.length;
//                return json.data;
//            }
//        },
//        "columns":
//            [
//                {
//                    "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                    "mRender": function (data, type, row) {
//                        return '<a class=link_Account_detail href="javascript:void(0)">' + data + '</a>'
//                    }
//                },
//                {
//                    "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
//                }
//            ],
//        initComplete: function () {
//            $(document).find('#tblAccountModalPopupFooter').show();
//            mcxDialog.closeLoading();
//            SetPageLengthMenu();
//            if (!$(document).find('#AccountTableModalPopup').hasClass('show')) {
//                $(document).find('#AccountTableModalPopup').modal("show");
//            }
//            $('#tblAccountModalPopup tfoot th').each(function (i, v) {
//                var colIndex = i;
//                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="account_colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
//                if (gAccountClientLookupId) { $('#account_colindex_0').val(gAccountClientLookupId); }
//                if (gAccountName) { $('#account_colindex_1').val(gAccountName); }
//            });
//            $(document).ready(function () {
//                $(window).keydown(function (event) {
//                    if (event.keyCode === 13) {
//                        event.preventDefault();
//                        return false;
//                    }
//                });
//                $('#tblAccountModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
//                    if (e.keyCode === 13) {
//                        gAccountClientLookupId = $('#account_colindex_0').val();
//                        gAccountName = $('#account_colindex_1').val();
//                        dtAccountTable.page('first').draw('page');
//                    }
//                });
//            });
//        }
//    });
//}

function generateAccountDataTable_Mobile() {
    //var Search = $(document).find('#txtAccountSearch_Mobile').val();
    AccountListlength = 0;
    $.ajax({
        "url": "/Equipment/GetAccountLookupList_Mobile",
        type: "POST",
        dataType: "html",
        data: {
            // Search: Search,
            //Name: Search
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivAccountSearchScrollViewModal').html(data);
            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            InitializeAccountListView_Mobile();
            //BindAccountScrollViewOfMobiScroll();
            //if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
            //    $(document).find('#EquipmentWOModal').modal("show");
            //}
            $('#AccountTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
//function BindAccountScrollViewOfMobiScroll() {
//    var $categoryNav,
//        $contentView

//    // init top tabs
//    $categoryNav = $('#Account-md-category').mobiscroll().nav({
//        type: 'tab',
//        onItemTap: function (event, inst) {
//            $contentView.mobiscroll('navigate', $('.' + $(event.target).data('page')));
//        }

//    });

//    $contentView = $('#Account-md-content').mobiscroll().scrollview({
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

//    $('.Account-md-list').mobiscroll().listview({
//        swipe: false,
//        striped: true,
//        enhance: true,
//    });
//    $('#AccountTableModalPopup').trigger('mbsc-enhance');
//}

//$(document).on('click', '.link_Account_detail', function (e) {
$(document).on('click', '.scrollview-accountsearch', function (e) {
    //var row = $(this).parents('tr');
    //var data = dtAccountTable.row(row).data();

    $(document).find('#' + TextField).val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#' + ValueField).val($(this).data('id'));//.removeClass('input-validation-error');

    $(document).find('#' + TextField).closest('form').valid();

    $(document).find('#' + ValueField).parent().find('div > button.ClearAccountModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).parents('div').eq(0).css('display') == 'none') {
        $(document).find('#' + TextField).parents('div').eq(0).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).parents('div').eq(0).css('display') == 'block') {
        $(document).find('#' + ValueField).parents('div').eq(0).css('display', 'none');
    }
    //$(document).find("#AccountTableModalPopupHide").trigger('click');
    $('#AccountTableModalPopupHide').trigger('click');
});

//$(document).on('hidden.bs.modal', '#AccountTableModalPopup', function () {
//    TextField = "";
//    ValueField = "";
//});

$(document).on('click', '#AccountTableModalPopupHide', function () {
    AccountListlength = 0;
    $('#AccountTableModalPopup').removeClass('slide-active');
    TextField = "";
    ValueField = "";
    $('#txtAccountSearch_Mobile').val('');
    $('#DivAccountSearchScrollViewModal').html('');

});
$(document).on("keyup", '#txtAccountSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generateAccountDataTable_Mobile();
    }
});

var AccountListView,
    AccountListlength = 0,
    AccountPageLength = 100;
function InitializeAccountListView_Mobile() {
    AccountListView = $('#AccountListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindAccountDataForListView();
    $('#AccountTableModalPopup').addClass('slide-active');
}
$(document).on('click', '#btnAccountLoadMore', function () {
    $(this).hide();
    AccountListlength += AccountPageLength;
    InitializeAccountListView_Mobile();
});


function BindAccountDataForListView() {
    var Search = $(document).find('#txtAccountSearch_Mobile').val();
    $.ajax({
        "url": "/Equipment/GetAccountLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: AccountListlength,
            Length: AccountPageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            AccountListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-accountsearch" data-id="' + item.AccountId + '" data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                AccountListView.add(null, mobiscroll.$(lihtml));
            }
            if ((AccountListlength + AccountPageLength) < data.recordsTotal) {
                $('#btnAccountLoadMore').show();
            }
            else {
                $('#btnAccountLoadMore').hide();
            }
        },
        complete: function () {
            AccountListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}