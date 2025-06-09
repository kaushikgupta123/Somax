$(document).on('click', '.OpenWOModalPopupGridWorkorderModal', function () {
    InitializeWOListView_Mobile();
});
$(document).on('click', '.scrollview-workordersearch', function (e) {
   
    if ($(document).find('#txtChargeTo').length > 0) {
        $(document).find('#txtChargeTo').val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
        $(document).find('#hdnChargeToId').val($(this).data('id'));
        //$('#workOrderModel_ChargeTo_Name').val($(this).data('chargetoname'));
        $(document).find('#txtChargeTo').closest('form').valid();
    }
    

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
    $('#partcheckoutWorkOrderModal').addClass('slide-active');
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
        "url": "/InventoryPartsIssue/GetWorkOrderLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: WOListlength,
            Length: PageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
           /* ShowLoader();*/
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