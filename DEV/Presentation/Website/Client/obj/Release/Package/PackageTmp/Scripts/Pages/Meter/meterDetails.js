//#region Readings
var DTReadingsTable;
var meterToQR = [];
var MeterToClientLookupIdQRcode = [];
function GenerateReadingsContainerGrid() {
    var rCount = 0;
    var meterId = $('#Meters_MeterId').val();
    if ($(document).find('#readingsTable').hasClass('dataTable')) {
        DTReadingsTable.destroy();
    }
    DTReadingsTable = $("#readingsTable").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        stateSave: false,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Meter/PopulateReadings",           
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.meterId = meterId;
                d.Reading = LRTrim($("#reading").val());
                d.ReadByClientLookupId = LRTrim($("#readByClientLookupId").val());
                d.DateRead = ValidateDate($("#dateread").val());
                d.Reset = LRTrim($("#reset").val());
            },

            "dataSrc": function (response) {
                rCount = response.data.length;
                return response.data;
            }
        },
        "columns":
        [
            { "data": "StringReading", "autoWidth": true, "bSearchable": true, "bSortable": true },
            { "data": "ReadByClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true },
            {
                "data": "DateRead", "autoWidth": true, "bSearchable": true, "bSortable": true, "type": "date",
                render: function (data, type, row, meta) {
                    if (data == null) {
                        return '';
                    } else {
                        return data;
                    }
                }
            },
            {
                "data": "Reset", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    if (data == true) {
                        return 'Yes';
                    }
                    else {
                        return 'No';
                    }

                }
            },

        ],
        initComplete: function () {
            SetPageLengthMenu();
        }
    });
}

$(document).on('click', '#readingMeter', function () {
    ResetErrorDiv($(document).find('#frmrecordmeterreading'));
    $('#readingMeterModal').find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy').datepicker("setDate", new Date()).removeClass('input-validation-error');
    $(document).find('#Readings_Reading').val('0').removeClass('input-validation-error');
    $.validator.unobtrusive.parse(document);
    $('#readingMeterModal').modal('show');
});

$(document).on('click', "#dismiss, .overlay", function () {
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
});
function RecordMeterReadingAddOnSuccess(data) {   
    if (data.Result == "success") {
        var message;
        $('#readingMeterModal').modal('hide');
        if (data.Command == "save") {
            ResetErrorDiv();
            if (data.woLookupids == "") {
                SuccessAlertSetting.text = getResourceValue("RecordAddAlert");
            }
            else {
                SuccessAlertSetting.text = getResourceValue("WorkOrderGeneratedAlert").replace("{0}", data.woLookupids);
            }
            swal(SuccessAlertSetting, function () {
                RedirectToDetail(data.MeterId, "Reading");
            });
        }
       
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
       
    }
}

$(document).on('click', "#brdreading", function () {
    var materid = $(this).attr('data-val');
    RedirectToDetail(materid, "Reading");
});
$(document).on('click', "#btnreadingcancel", function () {
    var meterId = $('#Meters_MeterId').val();
    swal(CancelAlertSetting, function () {
        RedirectToDetail(meterId, "Reading");
    });
});

$(document).on('click', "#readingsidebarCollapse", function () {
    $('#readingadvsearchcontainer').find('#sidebar').addClass('active');
    $('.overlay').fadeIn();
    $('.collapse.in').toggleClass('in');
    $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    $(document).find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy');
    $(document).find('.select2picker').select2({});
});
$(document).on('click', "#btnReadingAdvSrch", function () {
    ReadingAdvSearch();
    $('#sidebar').removeClass('active');
    $('.overlay').fadeOut();
    DTReadingsTable.page('first').draw('page');
});
function ReadingAdvSearch() {     
    var searchitemhtml = "";
    selectCount = 0;
    $('#advreadingsearchsidebar').find('.adv-item').each(function (index, item) {
        if ($(this).val()) {
            selectCount++;
            searchitemhtml = searchitemhtml + '<span class="label label-primary tagTo" id="sp_' + $(this).attr('id') + '">' + $("label[for='" + this.id + "']").text().replace(':', '') + '&nbsp;<a href="javascript:void(0);" class="fa fa-times btnCrossreading" aria-hidden="true"></a></span>';
        }
    });
    searchitemhtml = searchitemhtml + '<div style="clear:both;"></div>';  
    $("#readingadvsearchfilteritems").html(searchitemhtml);
    $(".recordfilteritemcount").text(selectCount);
}
function clearReadingAdvanceSearch() {
    $('#advreadingsearchsidebar').find('input:text').val('');
    selectCount = 0;
    $("#reset").val("").trigger('change');
    $(".recordfilteritemcount").text(selectCount);
    $('#readingadvsearchfilteritems').find('span').html('');
    $('#readingadvsearchfilteritems').find('span').removeClass('tagTo');
}
$(document).on('click', '#recordClearAdvSearchFilter', function () {
    run = true;
    clearReadingAdvanceSearch();
    DTReadingsTable.page('first').draw('page');
});
$(document).on('click', '.btnCrossreading', function () {
    var btnCrossedId = $(this).parent().attr('id');
    var searchtxtId = btnCrossedId.split('_')[1];
    $('#' + searchtxtId).val('');
    $(this).parent().remove();
    selectCount--;
    ReadingAdvSearch();
    DTReadingsTable.page('first').draw('page');
});
//#endregion
//#region ActivateInActivate
$(document).on('click', '#actinctivatemeter', function () {
    var meterid = $(document).find('#Meters_MeterId').val();
    var activationstatus = $(document).find('#hiddenmeterstatus').val();
    $.ajax({
        url: "/Meter/MakeActiveInactive",
        type: "POST",
        dataType: "json",
        data: { InActiveFlag: activationstatus, MeterId: meterid},
        beforeSend: function () {
            ShowLoader();
        },
        success: function (data) {
            if (data == true) {
                ShowSuccessAlert(getResourceValue("MeterInactivateAlert"));
            }
            else {
                ShowSuccessAlert(getResourceValue("MeterActivateAlert"));
            }
            RedirectToDetail(meterid, "Reading");
        },
        complete: function () {
            CloseLoader();
           
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion
//#region ResetMeter
$(document).on('click', '#resetmeter', function () {
    ResetErrorDiv($(document).find('#frmresetmeterreading'));
    $('#resetMeterModal').find('.dtpicker').datepicker({
        changeMonth: true,
        changeYear: true,
        beforeShow: function (i) { if ($(i).attr('readonly')) { return false; } },
        "dateFormat": "mm/dd/yy",
        autoclose: true
    }).inputmask('mm/dd/yyyy').datepicker("setDate", new Date()).removeClass('input-validation-error');
    $(document).find('#metersResetModel_Reading').val('1').removeClass('input-validation-error');
    $.validator.unobtrusive.parse(document);
    $('#resetMeterModal').modal('show');
});
function ResetMeterOnSuccess(data) {
    if (data.Result == "success") {
        $('#resetMeterModal').modal('hide');
        SuccessAlertSetting.text = getResourceValue("MeterResetAlert");
        swal(SuccessAlertSetting, function () {
            RedirectToDetail(data.MeterId, "Reading");
        });
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
};
//#endregion

//#region QR Code

function MeterQROnSuccess(data) {
    CloseLoader();
    if (data.success === 0) {
       var smallLabel = true;
        window.open('/Meter/QRCodeGenerationUsingRotativa?SmallLabel=' + encodeURI(smallLabel), '_blank');

        $('#printDetailsMeterQrCodeModal').modal('hide');
        meterToQR = [];
        MeterToClientLookupIdQRcode = [];
        //-- when called from grid         
        if ($(document).find('#meterSearch').find('.chksearch').length > 0) {
            $('#printQrcode').prop("disabled", "disabled");
            $('.itemQRcount').text(0);
            $('.itemcount').text(0);
            $(document).find('.DTFC_LeftBodyLiner').find('.chksearch').prop('checked', false);
            $(document).find(".updateArea").hide();
            $(document).find(".actionBar").fadeIn();
        }
        //--
    }
}
$(document).on('click', '#printDetailsMeterQrcode', function () {
    var ClientLookupId = $(document).find('#Meters_MeterClientLookUpId').val(); 
    var Name = $(document).find('#Meters_MeterName').val();
   
    MeterToClientLookupIdQRcode = [];
   
    MeterToClientLookupIdQRcode.push(ClientLookupId + '][' + Name );
    var meterClientLookups = MeterToClientLookupIdQRcode;
    $.ajax({
        url: "/Meter/MeterDetailsQRcode",
        data: {
            MeterClientLookups: meterClientLookups
        },
        type: "POST",
        beforeSend: function () {
            ShowLoader();
        },
        datatype: "html",
        success: function (data) {
            $('#printMeterDetailsqrcodemodalcontainer').html(data);
        },
        complete: function () {
            $('#printDetailsMeterQrCodeModal').modal('show');
            CloseLoader();
        },
        error: function () {
            CloseLoader();
        }
    });
});
//#endregion