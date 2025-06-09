var dtVendorTable;
var VenClientLookupId = "";
var VenName = "";
$(document).on('click', '#openvendorgrid', function () {
    generateVendorDataTable_Mobile();
});

function generateVendorDataTable_Mobile() {
    //var Search = $(document).find('#txtVendorSearch_Mobile').val();
    VendorListlength = 0;
    $.ajax({
        "url": "/Equipment/GetVendorLookupList_Mobile",
        type: "POST",
        dataType: "html",
        data: {

        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivVendorSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializeVendorListView_Mobile();
            $('#VendorTableModalPopup_Mobile').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.scrollview-vendorsearch', function (e) {

    $(document).find('#partsVendorModel_VendorClientLookupId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
    $(document).find('#vendorSessionData_Part').val($(this).data('id'));
    $(document).find('#partsVendorModel_VendorClientLookupId').closest('form').valid();

    $('#VendorTableModalPopupHide_Mobile').trigger('click');
});

$(document).on('click', '#VendorTableModalPopupHide_Mobile', function () {
    VendorListlength = 0;
    $('#VendorTableModalPopup_Mobile').removeClass('slide-active');
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
    $('#VendorTableModalPopup_Mobile').addClass('slide-active');
}
$(document).on('click', '#btnVendorLoadMore', function () {
    $(this).hide();
    VendorListlength += VendorPageLength;
    InitializeVendorListView_Mobile();
});


function BindVendorDataForListView() {
    var Search = $(document).find('#txtVendorSearch_Mobile').val();
    $.ajax({
        "url": "/Equipment/GetVendorLookupListchunksearch_Mobile",
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
$(document).on('click', '.clearTextBoxValue', function () {
    var id = $(this).data('txtboxid');
    $(document).find('#' + id).val('');
    if (id == 'txtVendorSearch_Mobile') {
        generateVendorDataTable_Mobile();
    }
    else if (id == 'txtPEquipmentSearch_Mobile') {
        generateEquipment_Mobile();
    }
});