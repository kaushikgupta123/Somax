$(document).on('click', '.OpenPersonnelPlannerModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generatePersonnelPlannerDataTable_Mobile();
});

function generatePersonnelPlannerDataTable_Mobile() {
    PersonnelPlannerListlength = 0;
    $.ajax({
        url: "/Dashboard/GetPersonnelPlannerLookupList_Mobile",
        type: "POST",
        dataType: "html",
        beforeSend: ShowLoader,
        success: function (data) {
            $('#DivPersonnelPlannerSearchScrollViewModal').html(data);
        },
        complete: function () {
            InitializePersonnelPlannerListView_Mobile();
            $('#PersonnelPlannerTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: CloseLoader
    });
}

$(document).on('click', '.scrollview-PersonnelPlannersearch', function () {
    const $textField = $(document).find('#' + TextField);
    const $valueField = $(document).find('#' + ValueField);

    $textField.val($(this).data('clientlookupid')).trigger('mbsc-enhance');
    $valueField.val($(this).data('id'));

    $textField.closest('form').valid();
    $valueField.parent().find('div > button.ClearPersonnelPlannerModalPopupGridData').css('display', 'block');

    $textField.parents('div').eq(0).css('display', 'block');
    $valueField.parents('div').eq(0).css('display', 'none');

    $('#PersonnelPlannerTableModalPopupHide').trigger('click');
});

$(document).on('click', '#PersonnelPlannerTableModalPopupHide', function () {
    PersonnelPlannerListlength = 0;
    $('#PersonnelPlannerTableModalPopup').removeClass('slide-active');
    TextField = "";
    ValueField = "";
    $('#txtPersonnelPlannerSearch_Mobile').val('');
    $('#DivPersonnelPlannerSearchScrollViewModal').html('');
});

$(document).on("keyup", '#txtPersonnelPlannerSearch_Mobile', function (e) {
    if (e.keyCode === 13) {
        generatePersonnelPlannerDataTable_Mobile();
    }
});

let PersonnelPlannerListView,
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
    const Search = $('#txtPersonnelPlannerSearch_Mobile').val();
    $.ajax({
        url: "/Dashboard/GetPersonnelPlannerLookupListchunksearch_Mobile",
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
            data.data.forEach(item => {
                const lihtml = `<li class="scrollview-PersonnelPlannersearch" data-id="${item.PersonnelId}" data-clientlookupid="${item.ClientLookupId}">${item.ClientLookupId} (${item.NameFirst} ${item.NameLast})</li>`;
                PersonnelPlannerListView.add(null, mobiscroll.$(lihtml));
            });
            $('#btnPersonnelPlannerLoadMore').toggle((PersonnelPlannerListlength + PersonnelPlannerPageLength) < data.recordsTotal);
        },
        complete: function () {
            PersonnelPlannerListView.hideLoading();
            CloseLoader();
        },
        error: CloseLoader
    });
}
