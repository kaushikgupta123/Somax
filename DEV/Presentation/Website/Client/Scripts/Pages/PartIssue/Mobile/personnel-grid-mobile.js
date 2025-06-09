
$(document).on('click', '#OpenPersonnelModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generatePersonnelPlannerDataTable_Mobile();
});

function generatePersonnelPlannerDataTable_Mobile() {
    PersonnelPlannerListlength = 0;
    $.ajax({
        "url": "/Dashboard/GetPersonnelPlannerLookupList_Mobile",
        type: "POST",
        dataType: "html",
        data: {
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#DivPersonnelPlannerSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializePersonnelPlannerListView_Mobile();
            $('#PersonnelPlannerTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}
$(document).on('click', '#PersonnelPlannerTableModalPopupHide', function () {
    PersonnelPlannerListlength = 0;
    $('#PersonnelPlannerTableModalPopup').removeClass('slide-active');
    TextField = "";
    ValueField = "";
    $('#txtPersonnelPlannerSearch_Mobile').val('');
    $('#DivPersonnelPlannerSearchScrollViewModal').html('');

});
$(document).on("keyup", '#txtPersonnelPlannerSearch_Mobile', function (e) {
    if (e.keyCode == 13) {
        generatePersonnelPlannerDataTable_Mobile();
    }
});

var PersonnelPlannerListView,
    PersonnelPlannerListlength = 0,
    PersonnelPlannerPageLength = 100;
function InitializePersonnelPlannerListView_Mobile() {
    PersonnelPlannerListView = $('#PersonnelPlannerListViewForSearch').mobiscroll().listview({
        theme: 'ios',
        themeVariant: 'light',
        animateAddRemove: false,
        striped: true,
        swipe: false
    }).mobiscroll('getInst');
    BindPersonnelPlannerDataForListView();
    $('#PersonnelPlannerTableModalPopup').addClass('slide-active');
}
$(document).on('click', '#btnPersonnelPlannerLoadMore', function () {
    $(this).hide();
    PersonnelPlannerListlength += PersonnelPlannerPageLength;
    InitializePersonnelPlannerListView_Mobile();
});


function BindPersonnelPlannerDataForListView() {
    var Search = $(document).find('#txtPersonnelPlannerSearch_Mobile').val();
    $.ajax({
        "url": "/PartIssue/GetActiveAdminOrFullPersonnelLookupListGridData_Mobile",
        data: {
            Search: Search,
            Start: PersonnelPlannerListlength,
            Length: PersonnelPlannerPageLength
        },
        type: 'POST',
        dataType: 'JSON',
        beforeSend: function () {
            ShowLoader();
            PersonnelPlannerListView.showLoading();
        },
        success: function (data) {
            var i, item, lihtml;
            for (i = 0; i < data.data.length; ++i) {
                item = data.data[i];
                lihtml = '';
                lihtml = '<li class="scrollview-PersonnelPlannersearch" data-id="' + item.PersonnelId + '" data-clientlookupid="' + item.ClientLookupId + '">';
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.NameFirst + ' ' + item.NameLast + ')</li>';
                PersonnelPlannerListView.add(null, mobiscroll.$(lihtml));
            }
            if ((PersonnelPlannerListlength + PersonnelPlannerPageLength) < data.recordsTotal) {
                $('#btnPersonnelPlannerLoadMore').show();
            }
            else {
                $('#btnPersonnelPlannerLoadMore').hide();
            }
        },
        complete: function () {
            PersonnelPlannerListView.hideLoading();
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });

    $(document).on('click', '.clearTextBoxValue', function () {
        var id = $(this).data('txtboxid');
        $(document).find('#' + id).val('');
        BindPersonnelPlannerDataForListView();
    });

    $(document).on('click', '.scrollview-PersonnelPlannersearch', function (e) {
        if ($(document).find('#txtIssueToId').length > 0) {
            $(document).find('#txtIssueToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
            $(document).find('#hdnIssueToId').val($(this).data('id'));
            $(document).find('#txtIssueToId').closest('form').valid();
        }
        else if ($(document).find('#txtReturnToId').length > 0) {
            $(document).find('#txtReturnToId').val($(this).data('clientlookupid')).trigger('mbsc-enhance');
            $(document).find('#hdnReturnToId').val($(this).data('id'));
            $(document).find('#txtReturnToId').closest('form').valid();
        }
        $('#PersonnelPlannerTableModalPopupHide').trigger('click');

    });
}

