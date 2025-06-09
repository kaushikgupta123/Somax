var dtPRVendorTable;
var VenClientLookupId = "";
var VenName = "";
var VenAddressCity = "";
var VenAddressState = "";
$(document).on('click', '#vopunchoutpengrid', function () {
    generatePunchOutVendorDataTable();
});
function generatePunchOutVendorDataTable() {
    var rCount = 0;
    var visibility;
    if ($(document).find('#PRVendorsTable').hasClass('dataTable')) {
        dtPRVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtPRVendorTable = $("#PRVendorsTable").DataTable({
        searching: true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bProcessing": true,
        "bDeferRender": true,
        "order": [[0, "asc"]],
        "pageLength": 10,
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?InnerGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/Base/GetPunchOutVendorLookupList",
            data: function (d) {
                d.ClientLookupId = VenClientLookupId;
                d.Name = VenName;
                /* V2-759*/
                d.addressCity = VenAddressCity;
                d.addressState = VenAddressState;
            },
            "type": "POST",
            "datatype": "json",
            "dataSrc": function (json) {
                rCount = json.data.length;
                return json.data;
            }
        },
        "columns":
        [
            {
                "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true,
                "mRender": function (data, type, row) {
                    return '<a class=link_prpunchoutvendor_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                /* V2-759*/
                {
                    "data": "AddressCity", "autoWidth": true, "bSearchable": true, "bSortable": true,
                },
                {
                    "data": "AddressState", "autoWidth": true, "bSearchable": true, "bSortable": true,
                }
        ],
        initComplete: function () {
            $(document).find('#tblvendorfooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#partModal').hasClass('show')) {
                $(document).find('#partModal').modal("show");
            }

            $('#PRVendorsTable tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#PRVendorsTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="vcolindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#vcolindex_0').val(VenClientLookupId); }
                if (VenName) { $('#vcolindex_1').val(VenName); }

                if (VenAddressCity) { $('#vcolindex_2').val(VenAddressCity); }
                if (VenAddressState) { $('#vcolindex_3').val(VenAddressState); }
            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
            $('#PRVendorsTable tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#vcolindex_0').val();
                    VenName = $('#vcolindex_1').val();
                    VenAddressCity = $('#vcolindex_2').val();
                    VenAddressState = $('#vcolindex_3').val();
                    dtPRVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).on('click', '.link_prpunchoutvendor_detail', function (e) {
    var index_row = $('#PRVendorsTable tr').index($(this).closest('tr')) - 1;
    var row = $(this).parents('tr');
    var td = $(this).parents('tr').find('td');
    var data = dtPRVendorTable.row(row).data();
    var VendorID = data.VendorID;
    $.ajax({
        url: "/Purchaserequest/AddPurchaseRequestPunchOut",
        type: "POST",
        dataType: "html",
        beforeSend: function () {
            ShowLoader();
        },
        data: { VendorID: VendorID },
        success: function (data) {
            var json_obj = $.parseJSON(data);
            if (json_obj.Result == "success") {
                var message;
                localStorage.setItem("PURCHASEREQUESTSTATUS", '10')
                localStorage.setItem("prstatustext", getResourceValue("MyOpenRequestsAlert"));
                SuccessAlertSetting.text = getResourceValue("PurchaseRequestAddedAlert");
                swal(SuccessAlertSetting, function () {
                    RedirectToPRequestDetail(json_obj.purchaserequestid, "overview");
                });
            }
            else {
                GenericSweetAlertMethod(json_obj);              
            }

        },
        complete: function () {
            CloseLoader();
            $(document).find("#partModal").modal('hide');
        },
        error: function () {
            CloseLoader();
        }
    });

});