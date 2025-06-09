var dtVendorTable;
var VenClientLookupId = "";
var VenName = "";
$(document).on('click', '#addPartVendorforAutoPurchasingConfig', function () {
    generatePartVendorAutoPuchasingConfigurationDataTable_Mobile();
});

function generatePartVendorAutoPuchasingConfigurationDataTable_Mobile() {
    VendorListlengthVendorAutoPuchasingConfiguration = 0;

    InitializeVendorAutoPuchasingConfigurationListView_Mobile();
    $('#VendorTableModalPopupPartsConfigureAutoPurchasing_Mobile').addClass('slide-active');
    CloseLoader();
}

$(document).on('click', '.scrollview-vendorsearchVendorConfigureAutoPurchasing', function (e) {

 
    if ($(this).data('clientlookupid') != "") {
        CloseLoader();
        $.ajax({
            url: '/Parts/AddPartsVendorConfigureAutoPurchasing',
            type: 'POST',
            data: { PartId: $(document).find("#partsConfigureAutoPurchasingModel_PartId").val(), VendorId: $(this).data('id'), VendorClientLoopId: $(this).data('clientlookupid') }, // Pass the PartId or other data if needed
            beforeSend: function () {
                ShowLoader();
            },
            success: function (response) {
                if (response.Result === "success") {
                    SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
                    $('#VendorTableModalPopupHidePartsConfigureAutoPurchasing_Mobile').trigger('click');
                    CloseLoader();
                    swal(SuccessAlertSetting, function () {
                        var $dropdown = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId");
                        $dropdown.empty(); // Clear existing items
                        $dropdown.append('<option value="">--Select--</option>'); // Optional default

                        $.each(response.VendorList, function (i, item) {
                            $dropdown.append($('<option>', {
                                value: item.Value,
                                text: item.Text
                            }));
                        });
                        ShowConfigureAutoPurchasing();
                    });


                } else {
                    CloseLoader();
                    ShowErrorAlert(response);
                }
            },
            error: function () {
                CloseLoader();
                ShowErrorAlert(response);

            }
        });



    }
   
});

$(document).on('click', '#VendorTableModalPopupHidePartsConfigureAutoPurchasing_Mobile', function () {
    VendorListlengthVendorAutoPuchasingConfiguration = 0;
    $('#VendorTableModalPopupPartsConfigureAutoPurchasing_Mobile').removeClass('slide-active');
    $('#txtVendorSearchPartsConfigureAutoPurchasing_Mobile').val('');
    $('#VendorListViewForSearchPopupPartsConfigureAutoPurchasing').html('');

});
$(document).on("keyup", '#txtVendorSearchPartsConfigureAutoPurchasing_Mobile', function (e) {
    if (e.keyCode == 13) {
        $('#VendorListViewForSearchPopupPartsConfigureAutoPurchasing').html('');
        generatePartVendorAutoPuchasingConfigurationDataTable_Mobile();
    }
});

var VendorListViewVendorAutoPuchasingConfiguration,
    VendorListlengthVendorAutoPuchasingConfiguration = 0,
    VendorPageLengthVendorAutoPuchasingConfiguration = 100;
function InitializeVendorAutoPuchasingConfigurationListView_Mobile() {
    VendorListViewVendorAutoPuchasingConfiguration = $('#VendorListViewForSearchPopupPartsConfigureAutoPurchasing').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindVendorDataVendorAutoPuchasingConfigurationForListView();
    $('#VendorTableModalPopupPartsConfigureAutoPurchasing_Mobile').addClass('slide-active');
}
$(document).on('click', '#btnVendorLoadMorePartsConfigureAutoPurchasing', function () {
    $(this).hide();
    VendorListlengthVendorAutoPuchasingConfiguration += VendorPageLengthVendorAutoPuchasingConfiguration;
    InitializeVendorAutoPuchasingConfigurationListView_Mobile();
});


function BindVendorDataVendorAutoPuchasingConfigurationForListView() {
    var Search = $(document).find('#txtVendorSearchPartsConfigureAutoPurchasing_Mobile').val();
    $.ajax({
        "url": "/Equipment/GetVendorLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: VendorListlengthVendorAutoPuchasingConfiguration,
            Length: VendorPageLengthVendorAutoPuchasingConfiguration
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            VendorListViewVendorAutoPuchasingConfiguration.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-vendorsearchVendorConfigureAutoPurchasing" data-id="' + item.VendorID + '" data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Name + ')</li>';
                VendorListViewVendorAutoPuchasingConfiguration.add(null, mobiscroll.$(lihtml));
            }
            if ((VendorListlengthVendorAutoPuchasingConfiguration + VendorPageLengthVendorAutoPuchasingConfiguration) < data.recordsTotal) {
                $('#btnVendorLoadMorePartsConfigureAutoPurchasing').show();
            }
            else {
                $('#btnVendorLoadMorePartsConfigureAutoPurchasing').hide();
            }
        },
        complete: function () {
            VendorListViewVendorAutoPuchasingConfiguration.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.clearTextBoxValuePartsConfigureAutoPurchasing', function () {
    $('#txtVendorSearchPartsConfigureAutoPurchasing_Mobile').val('');
    $('#VendorListViewForSearchPopupPartsConfigureAutoPurchasing').html('');
    generatePartVendorAutoPuchasingConfigurationDataTable_Mobile();
    
   
});



$(document).on('click', '.txtVendorSearchPartsConfigureAutoPurchasing_Mobile', function () {
    $('#VendorListViewForSearchPopupPartsConfigureAutoPurchasing').html('');
    generatePartVendorAutoPuchasingConfigurationDataTable_Mobile();
});