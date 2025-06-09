var UDFLookupGrid;
$(function () {
    $(document).find(' #viewNameLookUp').trigger('change');
    $(document).find('.select2picker').select2({});
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
});

$(document).on('change', "#viewNameLookUp", function (e) {

    var ViewId = $(document).find("#viewNameLookUp").val();

    //localStorage.setItem("ViewId", ViewId);
    GetAvailableSelectedlist(ViewId);
});

function GetAvailableSelectedlist(ViewId) {

    $.ajax({
        url: '/UiConfiguration/GetAvailableandSelectedList',
        data: {
            ViewId: ViewId
        },
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#AvailableandselectedContent").html('');
            $("#AvailableandselectedContent").html(data);
        },
        complete: function () {
            UIConfigurationSortable();
            CloseLoader();
        }
    });
}

function UiConfigurationAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("uIConfigUpdatedSuccessfullyAlert");

        swal(SuccessAlertSetting, function () {
            //titleText = getResourceValue("AlertActive");
            $("#viewNameLookUp").val(data.UiViewId).trigger('change');

            $("#btnsubmit").attr('disabled', 'disabled');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

$(document).on('click', "#btnLookUpListcancel", function () {
    var ViewId = $(document).find("#viewNameLookUp").val();
    swal(CancelAlertSetting, function () {
        $("#viewNameLookUp").val(ViewId).trigger('change');
    });
    
});

//#region  Selected Column Card
var UIColumnconfigDataDictionaryId = 0;
var UIColumnconfigisRequired = false;
var UiConfigurationId = 0;
var UIColumnconfigDisplayonForm = false; //V2-944
function resetUIColumnconfigData() {
    UIColumnconfigDataDictionaryId = 0;
    UIColumnconfigisRequired = false;
    UIColumnconfigDisplayonForm = false; //V2-944
    $("#hdncolumnSettingRequired").val(UIColumnconfigDataDictionaryId);
    $("#spn_ColumnSettingDetails_ColumnName").html('');
    $("#spn_ColumnSettingDetails_Description").html('');
    $("#chk_ColumnSettingDetails_IsRequired").prop('checked', UIColumnconfigisRequired);
    $("#chk_ColumnSettingDetails_DisplayonForm").prop('checked', UIColumnconfigDisplayonForm);
}
$(document).on('click', '.UIColumnconfig', function () {
    UIColumnconfigDataDictionaryId = $(this).attr('data-DataDictId');
    UIColumnconfigisRequired = $(this).attr('data-isRequired') == "false" ? false : true;
    UiConfigurationId = $(this).attr('data-configId');
    UIColumnconfigDisplayonForm = $(this).attr('data-DisplayonForm') == "false" ? false : true;
    var ColumnLabel = $(this).attr('data-ColumnLabel');
    var ColumnName = $(this).attr('data-ColumnName');
    var UDF = $(this).attr('data-udf');
    var columnType = $(this).attr('data-columntype');
    var Listname = $(this).attr('data-LookupName');
    $.ajax({
        url: '/UiConfiguration/SetConfigurationDetails',
        data: {
            DataDictionaryId: UIColumnconfigDataDictionaryId,
            UiConfigurationId: UiConfigurationId,
            ColumnName: ColumnName,
            columnlabel: ColumnLabel,
            IsRequired: UIColumnconfigisRequired,
            IsUDF: UDF,
            ColumnType: columnType,
            Listname: Listname,
            DisplayonForm: UIColumnconfigDisplayonForm
        },
        datatype: "html",
        beforeSend: function () {
            //ShowLoader();
        },
        success: function (data) {
            $("#ColumnConfigurationSettings").html('');
            $("#ColumnConfigurationSettings").html(data);
            SetControls();
            if (UDF == 'True' && columnType == 'Select') {
                //generateUDFLookuplistGrid(Listname)
                generateUDFLookuplistGridNew(Listname)
               
            }
            $("#ColumnConfigurationModal").modal("show");
        },
        complete: function () {
            //UIConfigurationSortable();
           // CloseLoader();
        }
    });
   
});
function SetControls() {
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
}
function UpdateConfigurationOnSuccess(data) {
    CloseLoader();
    if (data.Result == 'success') {
        $("#ColumnConfigurationModal").modal("hide");
        SuccessAlertSetting.text = getResourceValue("configurationUpdatedSuccessfullyAlert");
       //var ViewId = $(document).find("#viewNameLookUp").val();
        swal(SuccessAlertSetting, function () {
            //GetAvailableSelectedlist(ViewId);
            if (data.Required == false) {
                $("#" + UiConfigurationId).find(".red-required").remove();
                $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-isRequired', 'false')
                $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-ColumnLabel', data.Columnlabel)
                $("#" + UiConfigurationId).find('.left-box').find('.Columntext').text(data.Columnlabel)
            }
            else {
               
                if ($("#" + UiConfigurationId).find(".red-required").length == 0) {
                    $("#" + UiConfigurationId).find('.left-box').append('<span class="red-required">Required</span>')
                    $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-isRequired', 'true')
                    $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-ColumnLabel', data.Columnlabel)
                    $("#" + UiConfigurationId).find('.left-box').find('.Columntext').text(data.Columnlabel)
                }
                else{
                    $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-ColumnLabel', data.Columnlabel)
                    $("#" + UiConfigurationId).find('.left-box').find('.Columntext').text(data.Columnlabel)
                }
            }

            if (data.DisplayonForm == false) {
                $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-DisplayonForm', 'false');
            } else if (data.DisplayonForm == true) {
                $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-DisplayonForm', 'true');
            }
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '.SavecolumnSetting1', function () {
    
    var DataDictionaryId = UIColumnconfigDataDictionaryId;
    var oldcolumnSettingRequired = UIColumnconfigisRequired;
    var Description = $(".txtColumnLabel").val();
    var NewcolumnSettingConfigModel_Required = $("#chk_ColumnSettingDetails_IsRequired").is(":checked");
    var NewcolumnSettingConfigModel_DisplayonForm = $("#chk_ColumnSettingDetails_DisplayonForm").is(":checked");
    //if (oldcolumnSettingRequired != NewcolumnSettingConfigModel_Required) {
        $.ajax({
            url: '/UiConfiguration/UpdateColumnSettingDetails',
            type: "POST",
            data: {
                DataDictionaryId: DataDictionaryId,
                isRequired: NewcolumnSettingConfigModel_Required,
                description: Description,
                displayonForm: NewcolumnSettingConfigModel_DisplayonForm
            },
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                $("#ColumnConfigurationModal").modal("hide");
                if (data.success == true) {
                    SuccessAlertSetting.text = getResourceValue("configurationUpdatedSuccessfullyAlert");
                    var ViewId = $(document).find("#viewNameLookUp").val();
                    swal(SuccessAlertSetting, function () {
                        //GetAvailableSelectedlist(ViewId);
                        if (NewcolumnSettingConfigModel_Required == false) {
                            $("#" + UiConfigurationId).find(".red-required").remove();
                            $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-isRequired', 'false')
                        }
                        else {
                            if ($("#" + UiConfigurationId).find(".red-required").length == 0) {
                                $("#" + UiConfigurationId).find('.left-box').append('<span class="red-required">Required</span>')
                                $("#" + UiConfigurationId).find('.UIColumnconfig').attr('data-isRequired', 'true')
                            }
                        }  
                    });
                }
            },
            complete: function () {
                CloseLoader();

            },
            error: function () {
                CloseLoader();
            }
        });
    //}
});
$(document).on('click', '.removeUIconfig', function () {
    var UIConfigurationId = $(this).attr('data-UIconfigId');
    CancelAlertSettingForCallback.text = getResourceValue("columnsCardNotSavedMayRemoveAlert");
    swal(CancelAlertSettingForCallback, function () {
        $.ajax({
            url: '/UiConfiguration/removeColumnCardfromSelectedCardUI',
            type: "POST",
            data: {
                UIConfigurationId: UIConfigurationId
            },
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.success == true) {
                    SuccessAlertSetting.text = getResourceValue("columnCardRemovedSuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        var ViewId = $(document).find("#viewNameLookUp").val();
                        GetAvailableSelectedlist(ViewId);
                    });
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
//#endregion

//#region  Add User Define Field
$(document).on('click', '#linkAddUserDefineField', function (e) {
    generateDDTable();

});

function generateDDTable() {

    if ($(document).find('#DDTable').hasClass('dataTable')) {
        SelectDDTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    SelectDDTable = $("#DDTable").DataTable({
        columnDefs: [{
            "data": "DataDictionaryId",
            orderable: false,
            className: 'select-checkbox dt-body-center',
            targets: 0,
            'render': function (data, type, full, meta) {
                return '<input type="checkbox" name="id[]" data-ddid="' + data + '" class="isSelect" value="'
                    + $('<div/>').text(data).html() + '">';
            }
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/UiConfiguration/GetAllDDList",
            "type": "GET",
            "datatype": "json",
            "data": { UiViewId:$(document).find("#viewNameLookUp").val()}
        },
        "columns":
            [
                {},
                { "data": "ColumnName", "autoWidth": true, "bSearchable": true, "bSortable": true, "className": "text-center" },

            ],
        initComplete: function () {
            $("#AddUserDefineFieldmodal").modal('show');
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if ($("#DDTable").dataTable().fnGetData().length < 1)
                $('#btnAddUDF').prop('disabled', true);
            else
                $('#btnAddUDF').prop('disabled', false);
            return;
        }
    });
}

$(document).on('click', '#btnAddUDF', function () {
    var DDIDs = SelectDDTable.column(0).nodes().to$().map(function () {
        if ($(this).find('.isSelect').is(':checked')) {
            return $(this).find('.isSelect').data('ddid');
        }
    }).get().join(',');
    if (DDIDs !== "") {
        $.ajax({
            url: '/UiConfiguration/AddSelectUDF',
            data: { ListofIds: DDIDs },
            type: "POST",
            datatype: "json",
            beforeSend: function () {
                ShowLoader();
            },
            success: function (data) {
                if (data.data == 'success') {
                    SuccessAlertSetting.text = getResourceValue("userDefinedFieldsAddsuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        SelectDDTable.state.clear();
                        $("#AddUserDefineFieldmodal").modal('hide');
                        $(document).find(' #viewNameLookUp').trigger('change');
                    });
                }
            },
            complete: function () {
                CloseLoader();
               
            }
        });
    }
    else {
        swal({
            title: getResourceValue("NoRowsSelectAlert"),
            text: "Please select a User Define Field to proceed.",//getResourceValue("SelectChildrenAlerts"),
            type: "warning",
            confirmButtonClass: "btn-sm btn-primary",
            confirmButtonText: getResourceValue("SaveAlertOk"),
        });
        return false;
    }
});
//#endregion

//#region Sortable
var selectedIdArr = [];
var availableIdArr = [];
var blockSort = true;
var isrequired = false;

function UIConfigurationSortable() {
    $("#sortable1").sortable({
        connectWith: [$('#sortable2')],
        handle: '.dragableicon',
    });
    $("#sortable2").sortable({
        connectWith: [$('#sortable1')],
        handle: '.dragableicon',
    });
    $("#sortable1, #sortable2").bind('sortreceive', function (i, k) {
        blockSort = false;
        var itemid = k.item.attr('id');
        if ($('#' + itemid).find('.red-required').length > 0) {
            blockSort = true;
            isrequired = true;
        }
    }).bind('sortstop', function (e, ui) {
        if (e.currentTarget.id == 'sortable2' && isrequired == false) {
            blockSort = false;
        }
        if (blockSort) {
            $("#sortable1 .icon-box").remove();
            e.preventDefault();
            isrequired = false;
        }
        else {
            $("#btnsubmit").removeAttr('disabled');
            selectedIdArr = [];
            availableIdArr = [];
            $("#sortable1 .icon-box").remove();
            $('#sortable1 > div').each(function (i, item) {
                availableIdArr.push(item.id);
            });
            $('#sortable2 > div').each(function (i, item) {
                //var conId = item.id;
                //if ($("#" + conId).find(".icon-box").length == 0 && $(this).attr("data-systemreq") == "False") {
                //    $("#" + conId).append($('<div class="icon-box">' +
                //        '<a class="click-btn" href="#"><i class="fa fa-window-close" data-configId="' + conId + '"  aria-hidden="true"></i></a>' +
                //        '<a class="click-btn" href="#"><i class="fa fa-cog" data-configId="' + conId + '" aria-hidden="true"></i></a>' +
                //        '</div>'))
                //}
                selectedIdArr.push({
                    ItemId: item.id,
                    OrderId: i
                });
            });
            $("#hiddenSelectedList").val(JSON.stringify(selectedIdArr));
            $("#hiddenAvailableList").val(availableIdArr);
        }
        blockSort = true;
        isrequired = false;
    });
}
//#endregion

//#region Add Section
$(document).on('click', '#linkAddSection', function () {
    $("#addSectiomModel_SectionName").val('');
    $('.errormessage').html('')
    $("#UIViewId").val($(document).find("#viewNameLookUp").val());
    $("#addSectionModal").modal("show");
   
});

function AddSectionOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        SuccessAlertSetting.text = getResourceValue("sectionAddSuccessAlert");

        swal(SuccessAlertSetting, function () {
            $("#addSectionModal").modal("hide");
            $("#viewNameLookUp").val(data.UiViewId).trigger('change');
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

//#endregion

//#region Populate UDF Lookuplist
function generateUDFLookuplistGridNew(lookuplistName) {
    if ($(document).find('#tblUdfLookUpListGridNew').hasClass('dataTable')) {
        UDFLookupGrid.destroy();
    }
    $.ajax({
        url: '/UiConfiguration/GetLookupListGrid',
        "data": { DescriptionLookUp: lookuplistName },
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#UdflookupTableContainner").html('');
            $("#UdflookupTableContainner").html(data);
             UDFLookupGrid =$('#tblUdfLookUpListGridNew').DataTable(
                    {
                        "order": [[0, "asc"]],
                        paging: false,
                     searching: false,
                     "bProcessing": true,
                        responsive: true,
                        scrollY: 300,
                     "scrollCollapse": true,
                     "bDeferRender": true,
                        sDom: 'Btlipr',
                        language: {
                            url: "/base/GetDataTableLanguageJson?nGrid=" + true
                        },
                        buttons: [],
                        "columnDefs": [
                            {
                                "orderable": false, targets: [2]
                               
                            }
                        ],

                        initComplete: function () {
                            $(document).find('.dataTables_scroll').addClass('tblchild-scroll');
                            CloseLoader();
                        }
                 });
            

        },
        complete: function () {
            //CloseLoader();
        }
    });

}

//function generateUDFLookuplistGrid(lookuplistName) {
//    if ($(document).find('#tblUdfLookUpListGrid').hasClass('dataTable')) {
//        UDFLookupGrid.destroy();
//    }
//    UDFLookupGrid = $("#tblUdfLookUpListGrid").DataTable({
//        colReorder: true,
//        rowGrouping: true,
//        searching: true,
//        serverSide: true,
//        "pagingType": "full_numbers",
//        "bProcessing": true,
//        "bDeferRender": true,
//        "order": [[0, "asc"]],
//        stateSave: false,
//        language: {
//            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
//        },
//        sDom: 'Btlipr',
//        buttons: [],
//        "orderMulti": true,
//        "ajax": {
//            "url": "/UiConfiguration/GetUDFLookUpListsGrid",
//            "type": "POST",
//            "datatype": "json",
//            "data": { DescriptionLookUp: lookuplistName},
//            //"dataSrc": function (response) {
//            //    rCount = response.data.length;
//            //    if (rCount > 0) {
//            //        $(document).find("#AddLaborbtn").hide();
//            //    }
//            //    else {
//            //        $(document).find("#AddLaborbtn").show();
//            //    }

//            //    laborSecurity = response.security.Access;
//            //    return response.data;
//            //}
//        },
//        columnDefs: [
//            {
//                "data": null,
//                /*visible: false,*/
//                "className": "text-center",
//                targets: [2], render: function (a, b, data, d) {
//                    return '<a class="btn btn-outline-success editUdfBtn gridinnerbutton AddEditLookup" title= "Edit" data-LookupId="' + data.LookupListId + '" data-Description="' + data.Description + '" data-ListName="' + data.ListName + '" data-ListValue="' + data.ListValue + '" data-updateIndex="' + data.UpdateIndex+'"> <i class="fa fa-pencil"></i></a>' +
//                        '<a class="btn btn-outline-danger delUdfBtn gridinnerbutton" title="Delete" data-LookupId="' + data.LookupListId + '"> <i class="fa fa-trash"></i></a>';
//                }
//            }
//        ],
//        "columns":
//            [
//                { "data": "ListValue", "autoWidth": true, "bSearchable": true, "bSortable": true },
//                { "data": "Description", "autoWidth": true, "bSearchable": true, "bSortable": true }
               
//            ],
//        initComplete: function () {
//            CloseLoader();
//            SetPageLengthMenu();
//        }
//    });
//}
//#endregion

//#region Add Udf Lookuplist
$(document).on('click', '.AddEditLookup', function () {
    var DescriptionLookUpText = $(this).attr('data-Description');
    var LookupListId = $(this).attr('data-LookupId');
    var ListName = $(this).attr('data-ListName');
    var ListValue = $(this).attr('data-ListValue');
    var UpdateIndex = $(this).attr('data-updateIndex');
    $.ajax({
        url: '/UiConfiguration/AddEditLookUpLists',
        data: {
            ListName: ListName,
            DescriptionLookUpText: DescriptionLookUpText,
            LookupListId: LookupListId,
            ListValue: ListValue,
            updateIndex: UpdateIndex
        },
        datatype: "html",
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $("#AddEditUDFMainContainer").html('');
            $("#AddEditUDFMainContainer").html(data);
            SetControls();
            $("#ColumnConfigurationModal").modal("hide");
            $('#addEditUDFLookupModal').modal('show');

        },
        complete: function () {
            //UIConfigurationSortable();
            CloseLoader();
        }
    });
});
$(document).on('click', '.cancelUdflook', function () {
$('#AddEditUDFMainContainer').html('');
    $('#addEditUDFLookupModal').modal('hide');
    $('.modal-backdrop').hide();
    $("#ColumnConfigurationModal").modal("show");
   
});

function AddEditLookupOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.mode == "add") {
            SuccessAlertSetting.text = getResourceValue("ItemAddSuccessAlert");
        }
        else {
            SuccessAlertSetting.text = getResourceValue("ItemUpdateSuccessAlert");
        }
        swal(SuccessAlertSetting, function () {
            $('#AddEditUDFMainContainer').html('');
            $('#addEditUDFLookupModal').modal('hide');
            $('.modal-backdrop').hide();
            $("#ColumnConfigurationModal").modal("show");
            
            //UDFLookupGrid.page('first').draw('page');
            generateUDFLookuplistGridNew(data.lookuplistName)
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};

$(document).on('click', '.delUdfBtn', function () {
    var LookupListId = $(this).attr('data-LookupId');
    var ListName = $(this).attr('data-ListName');
    swal(CancelAlertSetting, function () {
        $.ajax({
            url: '/UiConfiguration/DeleteLookUpLists',
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
                    SuccessAlertSetting.text = getResourceValue("ItemDeletedSuccessAlert");
                    swal(SuccessAlertSetting, function () {
                        generateUDFLookuplistGridNew(ListName)
                        //UDFLookupGrid.page('first').draw('page');
                        //UDFLookupGrid.state.clear();
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
//#endregion