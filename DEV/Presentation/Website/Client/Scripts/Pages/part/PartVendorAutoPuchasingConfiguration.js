var dtVendorTable;
var VenClientLookupId = "";
var VenName = "";
$(document).on('click', '#addPartVendorforAutoPurchasingConfig', function () {
    var areaddescribedby = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId").attr('aria-describedby');
    $('#' + areaddescribedby).hide();
    generatePartVendorAutoPuchasingConfigurationDataTable();
});
function generatePartVendorAutoPuchasingConfigurationDataTable() {
 
    var rCount = 0;
    var visibility;
    if ($(document).find('#tblVendorModalPopup').hasClass('dataTable')) {
        dtVendorTable.destroy();
    }
    mcxDialog.loading({ src: "../content/Images" });
    dtVendorTable = $("#tblVendorModalPopup").DataTable({
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
            "url": "/Base/GetVendorLookupListchunksearch",
            data: function (d) {
                d.ClientLookupId = VenClientLookupId;
                d.Name = VenName;
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
                    return '<a class=link_PartVendorAutoPuchasingConfiguration_detail href="javascript:void(0)">' + data + '</a>'
                }
            },
            {
                "data": "Name", "autoWidth": true, "bSearchable": true, "bSortable": true,
            }
        ],
        initComplete: function () {
            $(document).find('#tblVendorModalPopupFooter').show();
            mcxDialog.closeLoading();
            SetPageLengthMenu();
            if (!$(document).find('#VendorTableModalPopup').hasClass('show')) {
                $(document).find('#VendorTableModalPopup').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            $('#tblVendorModalPopup tfoot th').each(function (i, v) {
                var colIndex = i;
                var title = $('#tblVendorModalPopup thead th').eq($(this).index()).text();
                $(this).html('<input type="text" style="width:100%" class="tfootsearchtxt" id="colindex_' + colIndex + '" /><i class="fa fa-search dropSearchIcon"></i>');
                if (VenClientLookupId) { $('#colindex_0').val(VenClientLookupId); }
                if (VenName) { $('#colindex_1').val(VenName); }

            });

            $('#tblVendorModalPopup tfoot th').find('.tfootsearchtxt').on("keyup", function (e) {
                if (e.keyCode == 13) {
                    var thisId = $(this).attr('id');
                    var colIdx = thisId.split('_')[1];
                    var searchText = LRTrim($(this).val());
                    VenClientLookupId = $('#colindex_0').val();
                    VenName = $('#colindex_1').val();
                    dtVendorTable.page('first').draw('page');
                }
            });
        }
    });
}

$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

});
$(document).on('click', '.link_PartVendorAutoPuchasingConfiguration_detail', function (e) {
    var row = $(this).parents('tr');
    var data = dtVendorTable.row(row).data();
    if (data.ClientLookupId != "") {
        CloseLoader();
        $.ajax({
            url: '/Parts/AddPartsVendorConfigureAutoPurchasing', 
            type: 'POST',
            data: { PartId: $(document).find("#partsConfigureAutoPurchasingModel_PartId").val(), VendorId: data.VendorID, VendorClientLoopId: data.ClientLookupId }, // Pass the PartId or other data if needed
            beforeSend: function () {
                ShowLoader();
            },
            success: function (response) {
                if (response.Result === "success") {
                    SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
                    $(document).find("#VendorTableModalPopup").modal('hide');
                    CloseLoader();
                    swal(SuccessAlertSetting, function () {
                        var $dropdown = $(document).find("#partsConfigureAutoPurchasingModel_PartVendorId");
                        $dropdown.empty(); // Clear existing items
                        $dropdown.append('<option value="">--Select--</option>'); // Optional default

                        $.each(response.VendorList, function (i, item) {
                            $dropdown.append($('<option>', {
                                value: item.Value,
                                text: item.Text
                            }));
                        });
                        ShowConfigureAutoPurchasing();
                    });

                   
                } else {
                    CloseLoader();
                    ShowErrorAlert(response);
                }
            },
            error: function () {
                CloseLoader();
                ShowErrorAlert(response);
                
            }
        });

      
        
    }
   
});
