$(document).ready(function () {
    $('chkIsBusiness').attr('checked', false);
    $(".tabsArea").hide();
    $("ul.vtabs li:first").addClass("active").show();
    $(".tabsArea:first").show();
    $("ul.vtabs li").click(function () {
        $("ul.vtabs li").removeClass("active");
        $(this).addClass("active");
        $(".tabsArea").hide();
        var activeTab = $(this).find("a").attr("href");
        $(activeTab).fadeIn();
        return false;
    });
    if ($(document).find('#AddVendorDynamicForm').length > 0) {
        $('input, form').blur(function () {
            $(this).valid();
        });
    }
    $(document).on('click', "#btnCancelAddVendor", function () {
        swal(CancelAlertSetting, function () {
            window.location.href = "../Vendor/Index?page=Inventory_Vendors";
        });
    });
});

function VendorAddOnSuccess(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
            swal(SuccessAlertSetting, function () {
                localStorage.setItem("VENDORSEARCHGRIDDISPLAYSTATUS", "1");
                localStorage.setItem("vendorstatustext", getResourceValue("ActiveVendorsAlert"));
                RedirectToVendorDetail(data.vendorid);
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}

 //#region V2-642
$(document).on('click', "#btnCancelAddVendorDynamic", function () {
    swal(CancelAlertSetting, function () {
        window.location.href = "../Vendor/Index?page=Inventory_Vendors";
    });
});
function VendorAddOnSuccessDynamic(data) {
    CloseLoader();
    if (data.Result == "success") {
        if (data.Command == "save") {
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
            swal(SuccessAlertSetting, function () {
                localStorage.setItem("VENDORSEARCHGRIDDISPLAYSTATUS", "1");
                localStorage.setItem("vendorstatustext", getResourceValue("ActiveVendorsAlert"));
                RedirectToVendorDetail(data.vendorid);
            });
        }
        else {
            ResetErrorDiv();
            SuccessAlertSetting.text = getResourceValue("VendorAddAlert");
            swal(SuccessAlertSetting, function () {
                $(document).find('form').trigger("reset");
                $(document).find('form').find("select").val("").trigger('change');
                $(document).find('form').find("select").removeClass("input-validation-error");
                $(document).find('form').find("input").removeClass("input-validation-error");
                $(document).find('form').find("textarea").removeClass("input-validation-error");
            });
        }
    }
    else {
        ShowGenericErrorOnAddUpdate(data);
    }
}
$(document).on('change', '#RemitUseBusiness', function () {
    if (this.checked) {
        $(document).find('#RemitAddress1').val($(document).find('#Address1').val());
        $(document).find('#RemitAddress2').val($(document).find('#Address2').val());
        $(document).find('#RemitAddress3').val($(document).find('#Address3').val());
        $(document).find('#RemitCity').val($(document).find('#AddressCity').val());
        $(document).find('#RemitState').val($(document).find('#AddressState').val());
        $(document).find('#RemitPostCode').val($(document).find('#AddressPostCode').val());
        $(document).find('#RemitCountry').val($(document).find('#AddressCountry').val());
        $(document).find('#RemitAddress1').attr('readonly', true);
        $(document).find('#RemitAddress2').attr('readonly', true);
        $(document).find('#RemitAddress3').attr('readonly', true);
        $(document).find('#RemitCity').attr('readonly', true);
        $(document).find('#RemitState').attr('readonly', true);
        $(document).find('#RemitPostCode').attr('readonly', true);
        $(document).find('#RemitCountry').attr('readonly', true);
    }
    else {
        $(document).find('#RemitAddress1').val('');
        $(document).find('#RemitAddress2').val('');
        $(document).find('#RemitAddress3').val('');
        $(document).find('#RemitCity').val('');
        $(document).find('#RemitState').val('');
        $(document).find('#RemitPostCode').val('');
        $(document).find('#RemitCountry').val('');
        $(document).find('#RemitAddress1').attr('readonly', false);
        $(document).find('#RemitAddress2').attr('readonly', false);
        $(document).find('#RemitAddress3').attr('readonly', false);
        $(document).find('#RemitCity').attr('readonly', false);
        $(document).find('#RemitState').attr('readonly', false);
        $(document).find('#RemitPostCode').attr('readonly', false);
        $(document).find('#RemitCountry').attr('readonly', false);
    }
});
//#endregion