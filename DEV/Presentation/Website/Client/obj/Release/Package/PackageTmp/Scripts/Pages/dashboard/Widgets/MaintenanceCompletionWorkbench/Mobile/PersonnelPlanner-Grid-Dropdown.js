
$(document).on('click', '.OpenPersonnelPlannerModalPopupGrid', function () {
    TextField = $(this).data('textfield');
    ValueField = $(this).data('valuefield');
    generatePersonnelPlannerDataTable_Mobile();
});

function generatePersonnelPlannerDataTable_Mobile() {
    //var Search = $(document).find('#txtPersonnelPlannerSearch_Mobile').val();
    PersonnelPlannerListlength = 0;
    $.ajax({
        "url": "/Dashboard/GetPersonnelPlannerLookupList_Mobile",
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
            $('#DivPersonnelPlannerSearchScrollViewModal').html(data);
            //$('#AddOnDemandWOModalpopup').modal({ backdrop: 'static', keyboard: false, show: true });
        },
        complete: function () {
            InitializePersonnelPlannerListView_Mobile();
            //BindPersonnelPlannerScrollViewOfMobiScroll();
            //if (!$(document).find('#EquipmentWOModal').hasClass('show')) {
            //    $(document).find('#EquipmentWOModal').modal("show");
            //}
            $('#PersonnelPlannerTableModalPopup').addClass('slide-active');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
}

$(document).on('click', '.scrollview-PersonnelPlannersearch', function (e) {
    //var row = $(this).parents('tr');
    //var data = dtPersonnelPlannerTable.row(row).data();

    $(document).find('#' + TextField).val($(this).data('clientlookupid')).trigger('mbsc-enhance');//.removeClass('input-validation-error');
    $(document).find('#' + ValueField).val($(this).data('id'));//.removeClass('input-validation-error');

    $(document).find('#' + TextField).closest('form').valid();

    $(document).find('#' + ValueField).parent().find('div > button.ClearPersonnelPlannerModalPopupGridData').css('display', 'block');

    if ($(document).find('#' + TextField).parents('div').eq(0).css('display') == 'none') {
        $(document).find('#' + TextField).parents('div').eq(0).css('display', 'block');
    }
    if ($(document).find('#' + ValueField).parents('div').eq(0).css('display') == 'block') {
        $(document).find('#' + ValueField).parents('div').eq(0).css('display', 'none');
    }
    //$(document).find("#PersonnelPlannerTableModalPopupHide").trigger('click');
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
        "url": "/Dashboard/GetPersonnelPlannerLookupListchunksearch_Mobile",
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
                lihtml = lihtml + "" + item.ClientLookupId + ' (' + item.NameFirst + ' ' + item.NameLast +')</li>';
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
}

