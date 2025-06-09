$(document).on('click', '.OpenPartCategoryMasterModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generatePartCategoryMasterDataTable_Mobile();
});
function generatePartCategoryMasterDataTable_Mobile() {
    PartCategoryMasterListlength = 0;
    $.ajax({
        "url": "/Dashboard/GetPartCategoryMasterLookupList_Mobile",
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
            $('#DivPartCategoryMasterSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializePartCategoryMasterListView_Mobile();
            $('#PartCategoryMasterTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '.scrollview-PCategoryMsearch', function (e) {
    $(document).find('#' + TextField).val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#' + ValueField).val($(this).data('id'));//.removeClass('input-validation-error');

    $(document).find('#' + TextField).closest('form').valid();

    $(document).find('#' + ValueField).parent().find('div > button.ClearPartCategoryMasterModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).parents('div').eq(0).css('display') == 'none') {
        $(document).find('#' + TextField).parents('div').eq(0).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).parents('div').eq(0).css('display') == 'block') {
        $(document).find('#' + ValueField).parents('div').eq(0).css('display', 'none');
    }
    $('#PartCategoryMasterTableModalPopupHide').trigger('click');
});

$(document).on('click', '#PartCategoryMasterTableModalPopupHide', function () {
    PartCategoryMasterListlength = 0;
    $('#PartCategoryMasterTableModalPopup').removeClass('slide-active');
    TextField = "";
    ValueField = "";
    $('#txtPartCategoryMasterSearch_Mobile').val('');
    $('#DivPartCategoryMasterSearchScrollViewModal').html('');

});
$(document).on("keyup", '#txtPartCategoryMasterSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePartCategoryMasterDataTable_Mobile();
    }
});

var PartCategoryMasterListView,
    PartCategoryMasterListlength = 0,
    PartCategoryMasterPageLength = 100;
function InitializePartCategoryMasterListView_Mobile() {
    PartCategoryMasterListView = $('#PartCategoryMasterListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPartCategoryMasterDataForListView();
    $('#PartCategoryMasterTableModalPopup').addClass('slide-active');
}
$(document).on('click', '#btnPartCategoryMasterLoadMore', function () {
    $(this).hide();
    PartCategoryMasterListlength += PartCategoryMasterPageLength;
    InitializePartCategoryMasterListView_Mobile();
});


function BindPartCategoryMasterDataForListView() {
    var Search = $(document).find('#txtPartCategoryMasterSearch_Mobile').val();
    $.ajax({
        "url": "/Dashboard/GetPartCategoryMasterLookupListchunksearch_Mobile",
        data: {
            Search: Search,
            Start: PartCategoryMasterListlength,
            Length: PartCategoryMasterPageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            PartCategoryMasterListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-PCategoryMsearch" data-id="' + item.PartCategoryMasterId + '" data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.Description + ')</li>';
                PartCategoryMasterListView.add(null, mobiscroll.$(lihtml));
            }
            if ((PartCategoryMasterListlength + PartCategoryMasterPageLength) < data.recordsTotal) {
                $('#btnPartCategoryMasterLoadMore').show();
            }
            else {
                $('#btnPartCategoryMasterLoadMore').hide();
            }
        },
        complete: function () {
            PartCategoryMasterListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}