$(document).on('click', '.OpenAccountModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generateAccountDataTable_Mobile();
});

function generateAccountDataTable_Mobile() {
    //var Search = $(document).find('#txtAccountSearch_Mobile').val();
    AccountListlength = 0;
    $.ajax({
        "url": "/Dashboard/GetAccountWOLookupList_Mobile",
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
        },
        complete: function () {
            InitializeAccountListView_Mobile();
            $('#AccountTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.scrollview-accountsearch', function (e) {

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
    $('#AccountTableModalPopupHide').trigger('click');
});

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
        "url": "/Dashboard/GetAccountLookupListchunksearch_Mobile",
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