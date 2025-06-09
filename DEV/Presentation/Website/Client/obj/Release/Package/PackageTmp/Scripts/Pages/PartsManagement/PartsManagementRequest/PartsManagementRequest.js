var DTPMRequestTable;
var run = false;
var filteritemcount = 0;
var PMRStatusDropValue;
var PMRRequesterValue;
var PMRRequestTypeValue;
var pageflag = 0;
var partmasterrequestId = "";
var requesttype = "";

var zoomConfig = {
    zoomType: "window",
    lensShape: "round",
    lensSize: 1000,
    zoomWindowFadeIn: 500,
    zoomWindowFadeOut: 500,
    lensFadeIn: 100,
    lensFadeOut: 100,
    easing: true,
    scrollZoom: true,
    zoomWindowWidth: 450,
    zoomWindowHeight: 450
};
var gridname = "PartMasterRequest_Search";
var order = '0';
var orderDir = 'asc';
var selectCount = 0;
function filterinfo(id, value) {
    this.key = id;
    this.value = value;
}
//#region Search
$(document).on('click', '#liPrint', function () {
    $(".buttons-print")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liCsv', function () {
    $(".buttons-csv")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', '#liPdf', function () {
    $(".buttons-pdf")[0].click();
    funcCloseExportbtn();
});
$(document).on('click', "#liExcel", function () {
    $(".buttons-excel")[0].click();
    funcCloseExportbtn();
});
function generatePMRequestDataTable() {
    if ($(document).find('#tblPMRequest').hasClass('dataTable')) {
        DTPMRequestTable.destroy();
    }
    DTPMRequestTable = $("#tblPMRequest").DataTable({
        colReorder: {
            fixedColumnsLeft: 1
        },
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: true,
        //"stateSaveCallback": function (settings, data) {
        //    if (run == true) {
        //        $.ajax({
        //            "url": "/Base/CreateUpdateState",
        //            "data": {
        //                GridName: "PartManagementRequest_Search",
        //                LayOutInfo: JSON.stringify(data)
        //            },
        //            "dataType": "json",
        //            "type": "POST",
        //            "success": function () { return; }
        //        });
        //    }
        //    run = false;
        //},
        //"stateLoadCallback": function (settings, callback) {
        //    $.ajax({
        //        "url": "/Base/GetState",
        //        "data": {
        //            GridName: "PartManagementRequest_Search",
        //        },
        //        "async": false,
        //        "dataType": "json",
        //        "success": function (json) {
        //            if (json) {
        //                callback(JSON.parse(json));
        //            }
        //            else {
        //                callback(json);
        //            }
        //        }
        //    });
        //},
        "stateSaveCallback": function (settings, data) {
            if (run == true) {
                if (data.order) {
                    data.order[0][0] = order;
                    data.order[0][1] = orderDir;
                }
                var filterinfoarray = getfilterinfoarraymr($("#txtColumnSearch"), $('#advsearchsidebar'));
                $.ajax({
                    "url": "/Base/CreateUpdateState",
                    "data": {
                        GridName: gridname,
                        LayOutInfo: JSON.stringify(data),
                        FilterInfo: JSON.stringify(filterinfoarray)
                    },
                    "dataType": "json",
                    "type": "POST",
                    "success": function () { return; }
                });
            }
            run = false;
        },
        "stateLoadCallback": function (settings, callback) {
            var o;
            $.ajax({
                "url": "/Base/GetLayout",
                "data": {
                    GridName: gridname
                },
                "async": false,
                "dataType": "json",
                "success": function (json) {
                    if (json.LayoutInfo) {
                        var LayoutInfo = JSON.parse(json.LayoutInfo);
                        order = LayoutInfo.order[0][0];
                        orderDir = LayoutInfo.order[0][1];
                        callback(JSON.parse(json.LayoutInfo));
                        if (json.FilterInfo) {
                            setsearchuimr(JSON.parse(json.FilterInfo), $("#txtColumnSearch"), $(".filteritemcount"), $("#advsearchfilteritems"));
                        }
                    }
                    else {
                        callback(json);
                    }

                }
            });
            //return o;
        },
        scrollX: true,
        fixedColumns: {
            leftColumns: 1
        },
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Parts Management Request'
            },
            {
                extend: 'print',
                title: 'Parts Management Request',
            },
            {
                text: 'Export CSV',
                extend: 'csv',
                filename: 'Parts Management Request',
                extension: '.csv'
            },
            {
                text: 'Print',
                extend: 'pdfHtml5',
                orientation: 'landscape',
                pageSize: 'A3',
                title: 'Parts Management Request'
            }
        ],
        "orderMulti": true,
        "ajax": {
            "url": "/PartsManagementRequest/GetPartmanagementRequestGridDataChunkList", //GetPartmanagementRequestGridData
            "type": "post",
            "datatype": "json",
            data: function (d) {
                d.SearchTextDropID = ptStatus;
                d.PartId = LRTrim($('#PMRPartId').val());
                //d.Requestor = $("#PMRRequester option:selected").val();//LRTrim($("#PMRRequester").val());
                d.Requestor = LRTrim($("#PMRRequester").val());
                d.Justification = LRTrim($("#PMRJustification").val());
                //d.RequestType = $("#PMRRequestType option:selected").val();//LRTrim($('#PMRRequestType').val());
                d.RequestType = LRTrim($('#PMRRequestType').val());
                //d.Status = $("#PMRStatusDrop option:selected").val();
                d.Status = LRTrim($('#PMRStatusDrop').val());
                d.Manufacturer = LRTrim($("#PMRManufacturer").val());
                d.ManufacturerPartNumber = LRTrim($("#PMRManufacturerPartNumber").val());
            },
            "dataSrc": function (result) {
                let colOrder = DTPMRequestTable.order();
                orderDir = colOrder[0][1];
                HidebtnLoader("btnsortmenu");
                HidebtnLoaderclass("LoaderDrop");
                //$("#PMRStatusDrop").empty();
                //$("#PMRStatusDrop").append("<option value=''>" + "--Select--" + "</option>");
                //$.each(result.StatusList, function (index, item) {
                //    $("#PMRStatusDrop").append("<option value='" + item + "'>" + getStatusValue(item) + "</option>");
                //});
                //if (PMRStatusDropValue && $("#PMRStatusDrop option[value='" + PMRStatusDropValue + "']").length > 0) {
                //    $("#PMRStatusDrop").val(PMRStatusDropValue).trigger("change.select2");
                //}
                //else  {
                //    $("#PMRStatusDrop").val('').trigger("change.select2");
                //}
                //$("#PMRRequester").empty();
                //$("#PMRRequester").append("<option value=''>" + "--Select--" + "</option>");
                //$.each(result.RequesterList, function (index, item) {
                //    $("#PMRRequester").append("<option value='" + item + "'>" + item + "</option>");
                //});
                //if (PMRRequesterValue && $("#PMRRequester option[value='" + PMRRequesterValue + "']").length > 0) {
                //    $("#PMRRequester").val(PMRRequesterValue).trigger("change.select2");
                //}
                //else {
                //    $("#PMRRequester").val('').trigger("change.select2");
                //}
                //$("#PMRRequestType").empty();
                //$("#PMRRequestType").append("<option value=''>" + "--Select--" + "</option>");
                //$.each(result.RequestTypeList, function (index, item) {
                //    $("#PMRRequestType").append("<option value='" + item + "'>" + item + "</option>");
                //});
                //if (PMRRequestTypeValue && $("#PMRRequestType option[value='" + PMRRequestTypeValue + "']").length > 0) {
                //    $("#PMRRequestType").val(PMRRequestTypeValue).trigger("change.select2");
                //}
                //else {
                //    $("#PMRRequestType").val('').trigger("change.select2");
                //}
                HidebtnLoaderclass("LoaderDrop");
                if (result.data.length == "0") {
                    $(document).find('.import-export').attr('disabled', 'disabled');
                }
                else {
                    $(document).find('.import-export').removeAttr('disabled');
                }

                return result.data;
            },
            global: true
        },
        "columns":
        [
            {
                "data": "PartMasterRequestId",
                "autoWidth": true,
                "bSearchable": true,
                "bSortable": true,
                "className": "text-left",
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<a class=lnk_pm_req href="javascript:void(0)">' + data + '</a>';
                }
            },
            { "data": "Requester", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "1" },
            { "data": "Justification", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "LocalizedRequestType", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            {
                "data": "Status_Display", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4",
                render: function (data, type, row, meta) {
                    return getStatusValue(data);
                }
            },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
        ],
        columnDefs: [
            {
                targets: [0],
                className: 'noVis'
            }
        ],
        initComplete: function () {
            SetPageLengthMenu();
            $("#PurchaseRequestGridAction :input").removeAttr("disabled");
            $("#PurchaseRequestGridAction :button").removeClass("disabled");
        }
    });
};
$(document).on('click', '.lnk_pm_req', function (e) {
    e.preventDefault();
    var index_row = $('#tblPMRequest tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = DTPMRequestTable.row(row).data();
    var pid = data.PartMasterRequestId;
    var reqtype = data.RequestType;
    $.ajax({
        url: "/PartsManagementRequest/GetPartMgmtDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PartMasterRequestId: pid, RequestType: reqtype },
        success: function (data) {
            $('#renderpartsreview').html(data);
        },
        complete: function () {
            SetPartMgmtDetailEnvironment();
            pageflag = 1;
            partmasterrequestId = pid;
            requesttype = reqtype;
        }
    });
});
function redirectDetails() {
    $.ajax({
        url: "/PartsManagementRequest/GetPartMgmtDetail",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { PartMasterRequestId: partmasterrequestId, RequestType: requesttype },
        success: function (data) {
            $('#renderpartsreview').html(data);
        },
        complete: function () {
            SetPartMgmtDetailEnvironment();
        }
    });
}
$("#SearchTextDropID").change(function () {
    run = true;
    var optionval = $('#SearchTextDropID option:selected').val();
    localStorage.setItem("PTMREQUESTSTATUS", optionval);
    ptStatus = optionval;
    if (optionval.length !== 0) {
        ShowbtnLoaderclass("LoaderDrop");
        DTPMRequestTable.page('first').draw('page');
    }
});
$(function () {
    $(document).find('.select2picker').select2({});
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $(document).on('click', '#dismiss, .overlay', function () {
        $('#sidebar').removeClass('active');
        $('.overlay').fadeOut();
    });
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('.overlay').fadeIn();
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });
    var parttrasstatus = localStorage.getItem("PTMREQUESTSTATUS");
    if (parttrasstatus) {
        ptStatus = parttrasstatus;
        generatePMRequestDataTable();
        $('#SearchTextDropID').val(parttrasstatus).trigger('change.select2');
    }
    else {
        ptStatus = 1;
        generatePMRequestDataTable();
        $('#SearchTextDropID').val(ptStatus).trigger('change.select2');
    }
    ShowbtnLoaderclass("LoaderDrop");
    $(".actionBar").fadeIn();
    $("#PurchaseRequestGridAction :input").attr("disabled", "disabled");
    jQuery.fn.DataTable.Api.register('buttons.exportData()', function (options) {
        if (this.context.length) {
            var currestsortedcolumn = $('#tblPMRequest').dataTable().fnSettings().aaSorting[0][0];
            var coldir = $('#tblPMRequest').dataTable().fnSettings().aaSorting[0][1];
            var colname = $('#tblPMRequest').dataTable().fnSettings().aoColumns[currestsortedcolumn].name;
            var jsonResult = $.ajax({
                "url": "/PartsManagementRequest/GetPartmanagementRequestPrintData",
                "type": "get",
                "datatype": "json",
                data: {
                    colname: colname,
                    coldir: coldir,
                    SearchTextDropID: ptStatus,
                    PartId: LRTrim($('#PMRPartId').val()),
                    Requestor: LRTrim($("#PMRRequester").val()),
                    Justification: LRTrim($("#PMRJustification").val()),
                    RequestType: LRTrim($('#PMRRequestType').val()),
                    Status: $("#PMRStatusDrop option:selected").val(),
                    Manufacturer: LRTrim($("#PMRManufacturer").val()),
                    ManufacturerPartNumber: LRTrim($("#PMRManufacturerPartNumber").val())
                },
                success: function (result) {
                },
                async: false
            });
            var thisdata = JSON.parse(jsonResult.responseText).data;
            var visiblecolumnsIndex = $("#tblPMRequest thead tr th").map(function (key) {
                return this.getAttribute('data-th-index');
            }).get();
            var d = [];
            $.each(thisdata, function (index, item) {
                if (item.PartMasterRequestId != null) {
                    item.PartMasterRequestId = item.PartMasterRequestId;
                }
                else {
                    item.PartMasterRequestId = "";
                }
                if (item.Requester != null) {
                    item.Requester = item.Requester;
                }
                else {
                    item.Requester = "";
                }
                if (item.Justification != null) {
                    item.Justification = item.Justification;
                }
                else {
                    item.Justification = "";
                }
                if (item.RequestType != null) {
                    item.RequestType = item.RequestType;
                }
                else {
                    item.RequestType = "";
                }
                if (item.Manufacturer != null) {
                    item.Manufacturer = item.Manufacturer;
                }
                else {
                    item.Manufacturer = "";
                }
                if (item.ManufacturerId != null) {
                    item.ManufacturerId = item.ManufacturerId;
                }
                else {
                    item.ManufacturerId = "";
                }
                var fData = [];
                $.each(visiblecolumnsIndex, function (index, inneritem) {
                    var key = Object.keys(item)[inneritem];
                    var value = item[key]
                    fData.push(value);
                });
                d.push(fData);
            })
            return {
                body: d,
                header: $("#tblPMRequest thead tr th div").map(function (key) {
                    return this.innerHTML;
                }).get()
            };
        }
    });
});
$(document).on('click', '#btnPMRDataAdvSrch', function () {
    PMRStatusDropValue = $(document).find('#PMRStatusDrop').val();
    PMRRequesterValue = $(document).find('#PMRRequester').val();
    PMRRequestTypeValue = $(document).find('#PMRRequestType').val();
    run = true;
    PMRequestAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    DTPMRequestTable.page('first').draw('page');
});
function PMRequestAdvSearch() {
    var searchitemhtml = "";
    selectCount = 0;
    $('#advsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    var optionVal = $(document).find("#equipDropdown").val();
    if (optionVal == "1") {
        InactiveFlag = true;
    }
    $("#advsearchfilteritems").html(searchitemhtml);
    $(".filteritemcount").text(selectCount);
}
$(document).on('click', '#liClearAdvSearchFilter', function () {
    run = true;
    ptStatus = 0;
    $("#SearchTextDropID").val('0').trigger('change.select2');
    clearPMRAdvanceSearch();
    DTPMRequestTable.page('first').draw('page');
});
$(document).on('click', '.btnCross', function () {
    run = true;
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('').trigger('change');
    $(this).parent().remove();
    filteritemcount--;
    if (searchtxtId == "PMRStatusDrop") {
        PMRStatusDropValue = null;
    }
    if (searchtxtId == "PMRRequester") {
        PMRRequesterValue = null;
    }
    if (searchtxtId == "PMRRequestType") {
        PMRRequestTypeValue = null;
    }
    selectCount--;
    PMRequestAdvSearch();
    DTPMRequestTable.page('first').draw('page');
});
function clearPMRAdvanceSearch() {
    filteritemcount = 0;
    $('#advsearchsidebar').find('input:text').val('');
    $('#advsearchsidebar').find('select').val('').trigger('change.select2');
    $('.filteritemcount').text(filteritemcount);
    $('#advsearchfilteritems').find('span').html('');
    $('#advsearchfilteritems').find('span').removeClass('tagTo');
    PMRStatusDropValue = $(document).find('#PMRStatusDrop').val();
    PMRRequesterValue = $(document).find('#PMRRequester').val();
    PMRRequestTypeValue = $(document).find('#PMRRequestType').val();
}
$(document).on('click', '#tblPMRequest_paginate .paginate_button', function () {
    run = true;
});
$(document).on('change', '#tblPMRequest_length .searchdt-menu', function () {
    run = true;
});

//#endregion
//#region ColumnVisibility
$(document).on('click', '#liCustomize', function () {
    funCustomizeBtnClick(DTPMRequestTable, true);
});
$(document).on('click', '.saveConfig', function () {
    var colOrder = [0];
    funCustozeSaveBtn(DTPMRequestTable, colOrder);
    run = true;
    DTPMRequestTable.state.save(run);
});
//#endregion
//#region Common
$(document).on('click', "ul.vtabs li", function () {
    $(document).find("ul.vtabs li").removeClass("active");
    $(document).find(this).addClass("active");
    $(document).find(".tabsArea").hide();
    var activeTab = $(this).find("a").attr("href");
    $(document).find(activeTab).fadeIn();
    return false;
});
function SetPartMgmtDetailEnvironment() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
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
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
    CloseLoader();
    ZoomImage($(document).find('#PartMasterRequestZoom'));
    Dropzone.autoDiscover = false;
    if ($(document).find('#dropzoneForm').length > 0) {
        var myDropzone = new Dropzone("div#dropzoneForm", {
            url: "../Base/SaveUploadedFile",
            addRemoveLinks: true,
            paramName: 'file',
            maxFilesize: 10, // MB
            maxFiles: 4,
            dictDefaultMessage: getResourceValue("FileUploadAlert"),
            acceptedFiles: ".jpeg,.jpg,.png",
            init: function () {
                this.on("removedfile", function (file) {
                    if (file.type != 'image/jpeg' && file.type != 'image/jpg' && file.type != 'image/png') {
                        ShowErrorAlert(getResourceValue("spnValidImage"));
                    }
                });
            },
            autoProcessQueue: true,
            dictRemoveFile: "X",
            uploadMultiple: true,
            dictRemoveFileConfirmation: getResourceValue("CancelAlertSure"),
            dictCancelUpload: "X",
            parallelUploads: 1,
            dictMaxFilesExceeded: "You can not upload any more files.",
            success: function (file, response) {
                var imgName = response;
                file.previewElement.classList.add("dz-success");
                var radImage = '<a class="lnkContainer setImage" data-toggle="tooltip" title="Upload a selective Image!" data-image="' + file.name + '" style="cursor:pointer;"><i class="fa fa-upload" style="cursor:pointer;"></i></a>';
                $(file.previewElement).append(radImage);
                console.log("Successfully uploaded :" + imgName);
            },
            error: function (file, response) {
                if (file.size > (1024 * 1024 * 10)) // not more than 10mb
                {
                    ShowImageSizeExceedAlert();
                }
                file.previewElement.classList.add("dz-error");
                var _this = this;
                _this.removeFile(file);

            }
        });
    }
    SetFixedHeadStyle();
}
function openCity(evt, cityName) {
    evt.preventDefault();
    switch (cityName) {
        case "PMRPhoto":
            //$('#Photo').show();
            break;
        case "PMRStatus":
            //$('#PMRStatus').show();
            break;
        case "PMRAttachment":
            generateMrAttachmentsGrid();
            // $('#PMRAttachment').show();
            break;
        case "PMRReviewLog":
            generateMrReviewLogGrid();
            //$('#PMRReviewLog').show();
            break;
    }
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    if (typeof evt.currentTarget !== "undefined") {
        evt.currentTarget.className += " active";
    }
    else {
        evt.target.className += " active";
    }
}
//#endregion
//#region AssignPartMaster
$(document).on('click', '#optAssignPartMaster', function () {
    $.ajax({
        "url": "/PartsManagementRequest/AssignPartMastertoIndusnetBakery",
        "dataType": "html",
        "type": "GET",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1) {
                $(document).find('#brdassnPartId').text(partmasterrequestId);
            }
            else {
                $(document).find('#brdassnPartId').text("");
            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#Assopengrid', function () {
  var RequestType = $(document).find('#assignPartMastertoIndusnetBakeryModel_RequestType').val();
  //var RequestType = $(document).find('#replacePartModal_RequestType').val();
    generatePMDataTable(RequestType);
});
function AssignPartMasterOnSuccess(data) {
    if (data.Result == "success") {
        ResetErrorDiv();
        $(document).find('#AssignPartMaster').modal('hide');
        SuccessAlertSetting.text = getResourceValue("PartAssignedAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSearchPage();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnPmCancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToSearchPage();
    });
});
//#endregion
//#region ReplacePart
$(document).on('click', '#optReplacePartMaster', function () {
    $.ajax({
        "url": "/PartsManagementRequest/ReplacePart",
        "dataType": "html",
        "type": "GET",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1) {
                $(document).find('#replbrdId').text(partmasterrequestId);
            }
            else {
                $(document).find('#replbrdId').text("");
            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#InPartopengrid', function () {
    var RequestType = $(document).find('#replacePartModal_RequestType').val();
    generatePMReplDataTable(RequestType);
});
$(document).on('click', '#Replopengrid', function () {
    var RequestType = $(document).find('#replacePartModal_RequestType').val();
    generatePMDataTable(RequestType);
});
function ReplacePartMasterOnSuccess(data) {
    if (data.Result == "success") {
        ResetErrorDiv();
        SuccessAlertSetting.text = getResourceValue("PartReplaceSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSearchPage();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnPmReplCancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToSearchPage();
    });
});
//#endregion
//#region InactivePart
$(document).on('click', '#optInactivePartMaster', function () {
    $.ajax({
        "url": "/PartsManagementRequest/InactivePart",
        "dataType": "html",
        "type": "GET",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1) {
                $(document).find('#brdinactId').text(partmasterrequestId);
            }
            else {
                $(document).find('#brdinactId').text("");
            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#InactPartopengrid', function () {
    var RequestType = $(document).find('#inactivePartModel_RequestType').val();
    generatePMReplDataTable(RequestType);
});
function InactivePartMasterOnSuccess(data) {
    if (data.Result == "success") {
        ResetErrorDiv();
        SuccessAlertSetting.text = getResourceValue("PartInactivateSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSearchPage();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnPmInactiveCancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToSearchPage();
    });
});
//#endregion
//#region ReplaceSXPart
$(document).on('click', '#optReplaceSXPartMaster', function () {
    $.ajax({
        "url": "/PartsManagementRequest/ReplaceSXPart",
        "dataType": "html",
        "type": "GET",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1) {
                $(document).find('#brdreplsxId').text(partmasterrequestId);
            }
            else {
                $(document).find('#brdreplsxId').text("");
            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#SXreplopengrid', function () {
    var RequestType = $(document).find('#replaceSXPartModel_RequestType').val();
    generatePMReplDataTable(RequestType);
});
$(document).on('click', '#SXassopengrid', function () {
    var RequestType = $(document).find('#replaceSXPartModel_RequestType').val();
    generatePMDataTable(RequestType);
});
function ReplaceSXPartMasterOnSuccess(data) {
    if (data.Result == "success") {
        ResetErrorDiv();
        SuccessAlertSetting.text = getResourceValue("SXPartReplaceSuccessAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSearchPage();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnPmSxCancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToSearchPage();
    });
});
//#endregion
//#region AddNewPartMaster
$(document).on('click', '#optAddNewPartMaster', function () {
    $.ajax({
        "url": "/PartsManagementRequest/AddNewPartManagement",
        "dataType": "html",
        "type": "GET",
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            SetPMRControls();
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1)
            {
                $(document).find('#brdId').text(partmasterrequestId);
            }
            else
            {
                $(document).find('#brdId').text("");
            }
            CloseLoader();
        }
    });
});
$(document).on('click', '#addPmMfgrid', function () {
    generatePMManufacDataTable();
});
function AddNewPartMasterOnSuccess(data) {
    if (data.Result == "success") {
        ResetErrorDiv();
        SuccessAlertSetting.text = getResourceValue("PartMasterAddAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToSearchPage();
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', '#btnAdPmCancel', function () {
    swal(CancelAlertSetting, function () {
        RedirectToSearchPage();
    });
});
//#endregion AddNewPartMaster
//#region ReplacePartWithNewPartMaster
$(document).on('click', '#optReplacePartWithNewPartMaster', function () {
    var ModeOfAction = "repalcewithnew";
    $.ajax({
        "url": "/PartsManagementRequest/AddNewPartManagement",
        "dataType": "html",
        "type": "GET",
        data: { ModeOfAction: ModeOfAction },
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            SetPMRControls();
            $.validator.unobtrusive.parse(document);
            if (pageflag == 1) {
                $(document).find('#brdId').text(partmasterrequestId);
            }
            else {
                $(document).find('#brdId').text("");
            }
            CloseLoader();
        }
    });
});
//#endregion ReplacePartWithNewPartMaster

//#region ReplacePartWithExistingPartMaster
$(document).on('click', '#optReplacePartWithExistingPartMaster', function () {
    var ModeOfAction = "repalcewithexisting";
    $.ajax({
        "url": "/PartsManagementRequest/AddNewPartManagement",
        "dataType": "html",
        "type": "GET",
        data: { ModeOfAction: ModeOfAction },
        beforeSend: function () {
            ShowLoader();
        },
        "success": function (data) {
            $('#renderpartsreview').html(data);
        },
        "complete": function () {
            if (pageflag == 1) {
                $(document).find('#brdId').text(partmasterrequestId);
            }
            else {
                $(document).find('#brdId').text("");
            }
            SetPMRControls();
            $.validator.unobtrusive.parse(document);
            CloseLoader();
        }
    });
});
$(document).on('click', '#replSomPidgrid', function () {
    var RequestType = $(document).find('#partsManagementRequestModel_RequestTypeForPopUp').val();
    generatePMReplDataTable(RequestType);
});
//#endregion ReplacePartWithExistingPartMaster
//#region Edit
$(document).on('click', "#btnEdit,#btnSiteEdit", function () {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    var RequestType = LRTrim($(document).find('#partsManagementRequestModel_RequestType').val());
    $.ajax({
        url: "/PartsManagementRequest/EditPartManagementRequest",
        type: "GET",
        dataType: 'html',
        data: { PartMasterRequestId: PartMasterRequestId, RequestType: RequestType, },
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            $('#renderpartsreview').html(data);
        },
        complete: function () {
            SetPMRControls();
        },
        error: function () {
            CloseLoader();
        }
    });
});
function EditPartMasterRequestSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        ResetErrorDiv();
        SuccessAlertSetting.text = getResourceValue("PartMasterEditAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToPartMasterRequestDetail(data.PartMasterRequestId, data.RequestType);
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('click', "#btnsPartMasterRequestcancel", function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestModel_PartMasterRequestId').val());
    var RequestType = $(document).find('#partsManagementRequestModel_RequestType').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
    });
});
$(document).on('click', "#btnsPartMasterRequestDetailcancel", function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#partsManagementRequestDetailModel_PartMasterRequestId').val());
    var RequestType = $(document).find('#partsManagementRequestDetailModel_RequestType').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
    });
});
$(document).on('click', '#PmMfgrid', function () {
    generatePMManufacDataTable();
});
$(document).on('click', "#pmrid", function () {
    var PartMasterRequestId = $(this).attr('data-val');
    var RequestType = $(this).attr('data-val-rtype');
    RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
});
//#endregion
function SetPMRControls() {
    CloseLoader();
    $.validator.setDefaults({ ignore: null });
    $.validator.unobtrusive.parse(document);
    $('input, form').blur(function () {
        $(this).valid();
    });
    $('.select2picker, form').change(function () {
        var areaddescribedby = $(this).attr('aria-describedby');
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
    });
    $(document).find('.select2picker').select2({});
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
}
function RedirectToSearchPage() {
    if (pageflag == 0) {
        window.location.href = "/PartsManagementRequest/Index?page=Inventory_Parts_Management_Parts_Management_Requests";
    }
    else {
        redirectDetails();
    }
}
$(document).on('click', '.clsbrd', function () {
    redirectDetails();
});

//#region V2-798
$(document).on('click', "#btnsPartMasterRequestDetailcancelForAssign", function (e) {
    var PartMasterRequestId = LRTrim($(document).find('#assignPartMastertoIndusnetBakeryModel_PartMasterRequestId').val());
    var RequestType = $(document).find('#assignPartMastertoIndusnetBakeryModel_RequestType').val();
    swal(CancelAlertSetting, function () {
        RedirectToPartMasterRequestDetail(PartMasterRequestId, RequestType);
    });
});
function getfilterinfoarraymr(txtsearchelement, advsearchcontainer) {
    var filterinfoarray = [];
    var f = new filterinfo('searchstring', LRTrim(txtsearchelement.val()));
    filterinfoarray.push(f);
    advsearchcontainer.find('.adv-item').each(function (index, item) {
        if ($(this).parent('div').is(":visible")) {
            f = new filterinfo($(this).attr('id'), $(this).val());
            filterinfoarray.push(f);
        }
    });
    return filterinfoarray;
}
function setsearchuimr(data, txtsearchelement, advcountercontainer, searchstringcontainer) {
    var searchitemhtml = '';
    $.each(data, function (index, item) {
        if (item.key == 'searchstring' && item.value) {
            var txtSearchval = item.value;
            if (item.value) {
                txtsearchelement.val(txtSearchval);
                searchitemhtml = "";
                searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + txtsearchelement.attr('id') + '">Search: ' + txtSearchval + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            }
            return false;
        }
        else {
            if ($('#' + item.key).parent('div').is(":visible")) {
                $('#' + item.key).val(item.value);
                if (item.value) {
                    selectCount++;
                    searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
                }
            }

            //if (item.key == 'advcreatedaterange' && item.value !== '') {
            //    $('#' + item.key).val(item.value);
            //    if (item.value) {
            //        searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            //    }
            //}
            //if (item.key == 'advprocessdedbydaterange' && item.value !== '') {
            //    $('#' + item.key).val(item.value);
            //    if (item.value) {
            //        searchitemhtml = searchitemhtml + '<span style="display:none;" class="label label-primary tagTo" id="sp_' + item.key + '">' + $("label[for='" + item.key + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCross" aria-hidden="true"></a></span>';
            //    }
            //}

            if (item.key == 'PMRRequester') {
                $("#PMRRequester").val(item.value).trigger('change.select2');
            }
            if (item.key == 'PMRRequestType') {
                $("#PMRRequestType").val(item.value).trigger('change.select2');
            }

            if (item.key == 'PMRStatusDrop') {
                $("#PMRStatusDrop").val(item.value).trigger('change.select2');
            }


            advcountercontainer.text(selectCount);
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';
    searchstringcontainer.html(searchitemhtml);

}
$('#tblPMRequest').find('th').click(function () {
    if ($(this).data('col') !== undefined && $(this).data('col') !== '') {
        run = true;
        order = $(this).data('col');
    }

});
//endregion