$(document).on('click', '.OpenVendorModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateVendorDataTable_Mobile();
});
function generateVendorDataTable_Mobile() {
    AccountListlength = 0;
    $.ajax({
        "url": "/Dashboard/GetVendorLookupList_Mobile",
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
            $('#DivVendorSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializeVendorListView_Mobile();
            $('#VendorTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.scrollview-vendorsearch', function (e) {
    $(document).find('#' + TextField).val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#' + ValueField).val($(this).data('id'));//.removeClass('input-validation-error');

    $(document).find('#' + TextField).closest('form').valid();

    $(document).find('#' + ValueField).parent().find('div > button.ClearVendorModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).parents('div').eq(0).css('display') == 'none') {
        $(document).find('#' + TextField).parents('div').eq(0).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).parents('div').eq(0).css('display') == 'block') {
        $(document).find('#' + ValueField).parents('div').eq(0).css('display', 'none');
    }
    $('#VendorTableModalPopupHide').trigger('click');
});

$(document).on('click', '#VendorTableModalPopupHide', function () {
    VendorListlength = 0;
    $('#VendorTableModalPopup').removeClass('slide-active');
    TextField = "";
    ValueField = "";
    $('#txtVendorSearch_Mobile').val('');
    $('#DivVendorSearchScrollViewModal').html('');

});
$(document).on("keyup", '#txtVendorSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generateVendorDataTable_Mobile();
    }
});

var VendorListView,
    VendorListlength = 0,
    VendorPageLength = 100;
function InitializeVendorListView_Mobile() {
    VendorListView = $('#VendorListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindVendorDataForListView();
    $('#VendorTableModalPopup').addClass('slide-active');
}
$(document).on('click', '#btnVendorLoadMore', function () {
    $(this).hide();
    VendorListlength += VendorPageLength;
    InitializeVendorListView_Mobile();
});


function BindVendorDataForListView() {
    var Search = $(document).find('#txtVendorSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/GetVendorLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: VendorListlength,
            Length: VendorPageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            VendorListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-vendorsearch" data-id="' + item.VendorID + '" data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                VendorListView.add(null, mobiscroll.$(lihtml));
            }
            if ((VendorListlength + VendorPageLength) < data.recordsTotal) {
                $('#btnVendorLoadMore').show();
            }
            else {
                $('#btnVendorLoadMore').hide();
            }
        },
        complete: function () {
            VendorListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}