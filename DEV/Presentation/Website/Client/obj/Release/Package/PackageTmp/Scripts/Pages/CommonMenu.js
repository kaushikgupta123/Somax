//#region MenuItemCount
function GetMenuItemsCount() {
    $.ajax({
        url: '/Base/GetMenuItemsCount',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        type: "GET",
        success: function (obj) {
            var countelement = obj.length;
            if (countelement > 0) {
                var spnopenevent = $('#spnopenevent');
                var spnopeneventcontainer = $('#spnopeneventcontainer');
                var spnopenwocontainer = $('#spnopenwocontainer');
                $.each(obj, function (index, item) {
                    if (item.ModuleName == "event") {
                        if (parseInt(item.ItemCount) > 0) {
                            spnopenevent.text(item.ItemCount);
                            spnopeneventcontainer.show();
                        }
                        else {
                            spnopeneventcontainer.hide();
                        }
                    }
                    else if (item.ModuleName == "workorder") {
                        if (parseInt(item.ItemCount) > 0) {
                            $('#spnopenwo').text(item.ItemCount);
                            spnopenwocontainer.show();
                        }
                        else {
                            spnopenwocontainer.hide();
                        }
                    }
                    else {
                        spnopeneventcontainer.hide();
                        spnopenwocontainer.hide();
                    }
                });
            }
            else {
                spnopeneventcontainer.hide();
                spnopenwocontainer.hide();
            }
        },
        complete: function () {
        },
        error: function (xhr) {
        }
    });
}
//#endregion

//#region Menu-State
function SetMenuBarOpenCloseState(state) {
    $.ajax({
        url: '/Base/SetMenuOpenState',
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: {
            state: state
        },
        beforeSend: function () {
            $('#procnotificationbody').html('');
            $('#notificationloader').show();
        },
        type: "GET",
        success: function (data) { },
        complete: function () {
        },
        error: function (xhr) {
        }
    });
}
//#endregion