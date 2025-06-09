//#region EquipmentId
var AssetListView,
    AssetListlength = 0,
    PageLength = 100;
$(document).on('click', '.EquipmentTableModalPopup_Mobile', function () {
    generateEquipment_Mobile();
});
function generateEquipment_Mobile() {
    AssetListlength = 0;
    $.ajax({
        "url": "/Dashboard/EquipmentLookupListView_Mobile",
        type: "POST",
        dataType: "html",
        data: {
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivEquipmentSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializePAssetListView_Mobile();
            $('#EquipmentTableModalPopup_Mobile').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.scrollview-pequipmentsearch', function (e) {
    if ($(document).find('#txtChargeToId').length > 0) {
        $(document).find('#txtChargeToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
        $(document).find('#hdnChargeToId').val($(this).data('id'));
        $(document).find('#txtChargeToId').closest('form').valid();
    }
    else if ($(document).find('#txtReturnChargeToId').length > 0) {
        $(document).find('#txtReturnChargeToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
        $(document).find('#hdnReturnChargeToId').val($(this).data('id'));
        $(document).find('#txtReturnChargeToId').closest('form').valid();
    }
    $('#EquipmentTableModalPopupHide_Mobile').trigger('click');
});
$(document).on('click', '#EquipmentTableModalPopupHide_Mobile', function () {
    AssetListlength = 0;
    $('#EquipmentTableModalPopup_Mobile').removeClass('slide-active');
    $('#txtPEquipmentSearch_Mobile').val('');
});

$(document).on("keyup", '#txtPEquipmentSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        $(document).find("#AssetListViewForSearch").html("");
        InitializePAssetListView_Mobile();
    }
});

function InitializePAssetListView_Mobile() {
    AssetListView = $('#AssetListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPAssetDataForListView();
    $('#EquipmentTableModalPopup_Mobile').addClass('slide-active');
}
$(document).on('click', '#btnAssetLoadMore', function () {
    $(this).hide();
    AssetListlength += PageLength;
    InitializePAssetListView_Mobile();
});
function BindPAssetDataForListView() {
    var Search = $(document).find('#txtPEquipmentSearch_Mobile').val();
    $.ajax({
        "url": "/PartIssue/GetEquipmentLookupListchunksearch_Mobile",
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
                lihtml = '<li class="scrollview-pequipmentsearch" data-chargetoname="' + item.Name + '" data-id="' + item.EquipmentId + '"; data-clientlookupid="' + item.ClientLookupId + '">';
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
//#endregion