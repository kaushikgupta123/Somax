$(document).on('click', '.OpenAssetModalPopupGridoverEquipmentModal', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateEquipmentDataTable_Mobile();
});
function generateEquipmentDataTable_Mobile() {
  
    AssetListlength = 0;
    $.ajax({
        "url": "/Equipment/EquipmentLookupListView_Mobile",
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
           
        },
        complete: function () {
            InitializeAssetListView_Mobile();
            $('#EquipmentWOModal').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.scrollview-equipmentsearch', function (e) {

    $(document).find('#' + TextField).val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#' + ValueField).val($(this).data('id'));//.removeClass('input-validation-error');

    $(document).find('#' + TextField).closest('form').valid();

    $(document).find('#' + ValueField).parent().find('div > button.ClearequipmentModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).parents('div').eq(0).css('display') == 'none') {
        $(document).find('#' + TextField).parents('div').eq(0).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).parents('div').eq(0).css('display') == 'block') {
        $(document).find('#' + ValueField).parents('div').eq(0).css('display', 'none');
    }

    $('#EquipmentWOModalHide').trigger('click');
});
$(document).on('click', '#EquipmentWOModalHide', function () {
 
    AssetListlength = 0;
    $('#EquipmentWOModal').removeClass('slide-active');
    $('#txtEquipmentSearch_Mobile').val('');
    $('#DivEquipmentSearchScrollViewModal').html('');
});

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
 
}
$(document).on('click', '#btnAssetLoadMore', function () {
    $(this).hide();
    AssetListlength += PageLength;
    InitializeAssetListView_Mobile();
});
function BindAssetDataForListView() {
    var Search = $(document).find('#txtEquipmentSearch_Mobile').val();
    $.ajax({
        "url": "/Equipment/GetEquipmentLookupListchunksearch_Mobile",// need to create a SP later on for mobile and change layers
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