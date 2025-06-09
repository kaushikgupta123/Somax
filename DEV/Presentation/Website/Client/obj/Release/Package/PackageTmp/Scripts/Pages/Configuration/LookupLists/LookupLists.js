//#region LookUpLists Search
var LookUpListsdt;
$(function () {
    $(document).find(' #DescriptionLookUp').trigger('change');
    $(document).find('.select2picker').select2({});
    $(document).find("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
     var DescriptionLookUp;
    var localStorageDesc = localStorage.getItem("DESCRIPTIONLOOKUP");
    if (localStorageDesc) {
        DescriptionLookUp = localStorageDesc;
        $(document).find("#DescriptionLookUp").val(DescriptionLookUp);
        localStorage.removeItem("DESCRIPTIONLOOKUP");
    }
    else {
        DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    }
});
$(document).on('click', '#LUListsidebarCollapse', function () {
    $('#LookUpListadvsearchcontainer').find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
});
$(document).on('change', "#DescriptionLookUp", function (e) {    
    if (LookUpListsdt != null && LookUpListsdt != undefined)
    {
        LookUpListsdt.state.clear();
    }    
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    SetCurrentDescription(DescriptionLookUp);
    localStorage.setItem("DESCRIPTIONLOOKUP", DescriptionLookUp);
    GetLookUpsList($(document).find("#DescriptionLookUp").val());
});
function GetLookUpsList(DescriptionLookUp) {
    var RecordCount = 0;
    var searchText = LRTrim($('#txtLookUpListssearchbox').val());
    var Value = $(document).find("#LUListValue").val();
    var Description = $(document).find("#LUListDescription").val();

    if ($(document).find('#tblLookUpListGrid').hasClass('dataTable')) {
        LookUpListsdt.destroy();
    }
    LookUpListsdt = $(document).find("#tblLookUpListGrid").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        serverSide: true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons:[],
        "orderMulti": true,
        "ajax": {
            url: "/LookUpLists/GetLookUpListsGrid",
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.DescriptionLookUp = DescriptionLookUp;
                d.Description = Description;
                d.Value = Value;
                d.SearchText = searchText;
            },
            "dataSrc": function (result) {
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }
                RecordCount = result.InitialtotalRecords;
                return result.data;
            },
            global: true
        },
        columnDefs: [
            {
                targets: [3], render: function (a, b, data, d) {
                    if (data.IsReadOnly)
                    {
                        return '<a class="btn btn-outline-success editlookuplist gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>';

                    }
                    else
                    {
                        return '<a class="btn btn-outline-primary addlookuplist gridinnerbutton" title= "Add"> <i class="fa fa-plus"></i></a>' +
                            '<a class="btn btn-outline-success editlookuplist gridinnerbutton" title= "Edit"> <i class="fa fa-pencil"></i></a>' +
                            '<a class="btn btn-outline-danger dellookuplist gridinnerbutton" title="Delete"> <i class="fa fa-trash"></i></a>';
                    }
                 
                }
            }
        ],
        "columns":
        [
            { "data": "ListValue", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "0" },
            {
                "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1",
                "mRender": function (data, type, row) {
                    return "<div class='text-wrap width-400'>" + data + "</div>";
                }
            },
            {
                "data": "InactiveFlag", "autoWidth": true, "bSearchable": true, "bSortable": true, className: 'text-center', "name": "2",
                "mRender": function (data, type, row) {
                    if (data == true) {
                        return '<label class="m-checkbox m-checkbox--air m-checkbox--solid m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" checked="checked" class="status"><span></span></label>';
                    }
                    else {

                        return '<label class="m-checkbox m-checkbox--air m-checkbox--state-brand somax-checkbox lbl-innergrid-checkbox">' +
                            '<input type="checkbox" class="status"><span></span></label>';        
                    }

                }
            }
            ,
            {
                "data": "ObjectId", defaultContent: "", "bSearchable": false, "bSortable": false, "className": "text-center"
            }
        ],
        initComplete: function () {
            $(document).on('click', '.status', function (e) {
                e.preventDefault();
            });
            if (RecordCount > 0 || ($(document).find("#DescriptionLookUp option:selected").text()=='')) {
                $(document).find('#btnAddLookUpListContainer').hide();
            }
            else {
                $(document).find('#btnAddLookUpListContainer').show();
            }
            SetPageLengthMenu();
        }
    });
}
$('#btnLUListsearch').on('click', function () {
    clearAdvanceSearch();
    LookUpListsdt.state.clear();
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    GetLookUpsList(DescriptionLookUp);
});
function clearAdvanceSearch() {
    $("#LUListValue").val("");
    $("#LUListDescription").val("");
}
function SetCurrentDescription(description) {
    $.ajax({
        url: '/LookUpLists/SetDescription',
        data: {
            value: description
        },
        datatype: "json",
        success: function () {
        }
    });
}
//#endregion
//#region Advance Search
$(document).on('click', "#btnLUListDataAdvSrch", function (e) {
    LookUpListsdt.state.clear();
    var searchitemhtml = "";
    hGridfilteritemcount = 0
    $('#txtLookUpListssearchbox').val('');
    $('#advsearchsidebarLUList').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            hGridfilteritemcount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossLUList" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    $('#advsearchfilteritems').html(searchitemhtml);
    $('#sidebar').removeClass('active');
    $('#LookUpListadvsearchcontainer').find('#sidebar').removeClass('active');
    $(document).find('.overlay').fadeOut();
    GridAdvanceSearch();
});
function GridAdvanceSearch() {
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    GetLookUpsList(DescriptionLookUp);
    $('.filteritemcount').text(hGridfilteritemcount);
}
$(document).on('click', '.btnCrossLUList', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    hGridfilteritemcount--;
    GridAdvanceSearch();
});
$(document).on('click', '#liClearAdvSearchFilterLUList', function () {
    $("#txtLookUpListssearchbox").val("");
    clearAdvanceSearch();
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    GetLookUpsList(DescriptionLookUp);
});
function clearAdvanceSearch() {
    var filteritemcount = 0;
    $('#advsearchsidebarLUList').find('input:text').val('');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
}
//#endregion
//#region look up add-edit-delete
$(document).on('click', '.dellookuplist', function () {
    var data = LookUpListsdt.row($(this).parents('tr')).data();
    var LookupListId = data.LookupListId;
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/LookUpLists/DeleteLookUpLists',
            data: {
                LookupListId: LookupListId
            },
            beforeSend: function () {
                ShowLoader();
            },
            type: "post",
            datatype: "json",
            success: function (data) {
                if (data.Result == "success") {
                    swal(SuccessAlertSetting, function () {
                            LookUpListsdt.state.clear();
                            var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();                        
                            GetLookUpsList(DescriptionLookUp);
                        });
                }
                else {
                    CloseLoader();
                    ShowDeleteAlert(getResourceValue("RecordNotDeletedAlert"));
                }
            },
            complete: function () {
                CloseLoader();
            },
            error: function () {
                CloseLoader();
            }
        });
    });
});
$(document).on('click', "#btnAddLookUpList,.addlookuplist", function () {   
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    var DescriptionLookUpText = $(document).find("#DescriptionLookUp option:selected").text();
    $.ajax({
        url: "/LookUpLists/AddEditLookUpLists",
        type: "GET",
        dataType: 'html',
        data:
        {
            DescriptionLookUp: DescriptionLookUp,
            DescriptionLookUpText: DescriptionLookUpText
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderLookUpList').html(data);
        },
        complete: function () {
            SetLUListControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
$(document).on('click', '.editlookuplist', function () {   
    var data = LookUpListsdt.row($(this).parents('tr')).data();
    EditLookuplist(data);
});
function EditLookuplist(data) {
    var DescriptionLookUpText = $(document).find("#DescriptionLookUp option:selected").text();
    var DescriptionLookUp = $(document).find("#DescriptionLookUp").val();
    $.ajax({
        url: "/LookUpLists/AddEditLookUpLists",
        type: "GET",
        dataType: 'html',
        data:
        {
            DescriptionLookUp: DescriptionLookUp,
            DescriptionLookUpText: DescriptionLookUpText,
            LookupListId: data.LookupListId,
            IsReadOnly: data.IsReadOnly
        },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderLookUpList').html(data);
        },
        complete: function () {
            SetLUListControls();
        },
        error: function (jqXHR, exception) {
            CloseLoader();
        }
    });
}
function LookUpListsAddEditOnSuccess(data) {
    CloseLoader();
    var LookupListId = data.LookupListId;
    if (data.LookupListId > 0) {
        var message;
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("LookUpListAddedAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("LookUpListUpdatedAlert");
        }
        swal(SuccessAlertSetting, function () {
            LookUpListsdt.state.clear();
            SetLocalisationDescription();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
//#endregion
//#region commom
function SetLUListControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $(document).find('.select2picker').select2({});
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
        if (areaddescribedby) {
            if ($(this).valid()) {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).hide();
                }
            }
            else {
                if (typeof areaddescribedby != 'undefined') {
                    $('#' + areaddescribedby).show();
                }
            }
        }
    });
    }
$(document).on('click', "#btnLookUpListcancel", function () {
    RedirectToSureOncancel();
});
function RedirectToSureOncancel() {
    swal(CancelAlertSetting, function () {
        SetLocalisationDescription();
    });
}
function SetLocalisationDescription() {   
    window.location.href = "/LookUpLists/index?page=Lookup_Lists";
}

//#endregion





