/**
 *  jQuery.SelectListActions
 *  https://github.com/esausilva/jquery.selectlistactions.js
 *
 *  (c) http://esausilva.com
 */

(function ($) {
    //Moves selected item(s) from sourceList to destinationList
    $.fn.moveToList = function (sourceList, destinationList, selectedOption) {
        var flag = true;
        var opts = $(sourceList).find(selectedOption);
        //if (opts.length == 0) {
        //    alert("Nothing to move");
        //}
        //else {
        //var sourceArray = [];
        //$.each(opts, function (i, v) {
        //    sourceArray.push($(v).val());
        //});
        //var destArray = [];
        ////$(destinationList).find('option').each(function () {
        ////    destArray.push($(this).val());
        ////});
        //$(destinationList).find('li').each(function () {
        //    destArray.push($(this).data('val').toString());
        //});
        //sourceArray = sourceArray.filter(v => destArray.indexOf(v) == -1);
        ////$.each(sourceArray,function(i,v){
        ////$(destinationList).append($(v).clone());
        ////$.each(opts, function (j, vv) {
        ////    if ($(vv).val() == sourceArray[0])
        ////        $(destinationList).append($(vv).clone());
        ////});
        //$.each(opts, function (j, vv) {
        //    if ($(vv).val() == sourceArray[0])
        //        $(destinationList).append(GetHtml($(vv).val(), $(vv).text()));
        //});
        //});
        var sourceArray = [];
        sourceArray.push($(opts).val());
        var destArray = [];
        $(destinationList).find('li').each(function () {
            destArray.push($(this).data('val').toString());
        });
        sourceArray = $.grep(sourceArray, function (el) { return $.inArray(el, destArray) == -1 });
        //sourceArray = sourceArray.filter(v => destArray.indexOf(v) === -1);
        if (sourceArray.length > 0)
        $(destinationList).append(GetHtml($(opts).val(), $(opts).text(), $(opts).attr('disabled') ? "disabled" : "", $(opts).data('order'), $(opts).data('required') !== 'undefined' ? !$(opts).data('required') : true));
        //}

    };

    function GetHtml(dval, dtext, dDisabled, order, removable) {
        return "<li class='" + dDisabled + "' data-val='" + dval + "' data-removable='" + removable + "'><span>" + dtext + "</span></li>";
    }

    //Moves all items from sourceList to destinationList
    $.fn.moveAllToList = function (sourceList, destinationList) {
        var opts = $(sourceList + ' option');
        if (opts.length == 0) {
            alert("Nothing to move");
        }

        $(destinationList).append($(opts).clone());
    };

    //Moves selected item(s) from sourceList to destinationList and deleting the
    // selected item(s) from the source list
    $.fn.moveToListAndDelete = function (sourceList, destinationList) {
        var opts = $(sourceList + ' option:selected');
        if (opts.length == 0) {
            alert("Nothing to move");
        }

        $(opts).remove();
        $(destinationList).append($(opts).clone());
    };

    //Moves all items from sourceList to destinationList and deleting
    // all items from the source list
    $.fn.moveAllToListAndDelete = function (sourceList, destinationList) {
        var opts = $(sourceList + ' option');
        if (opts.length == 0) {
            alert("Nothing to move");
        }

        $(opts).remove();
        $(destinationList).append($(opts).clone());
    };

    //Removes selected item(s) from list
    $.fn.removeSelected = function (list) {
        //var opts = $(list + ' option:selected');
        //if (opts.length == 0) {
        //    alert("Nothing to remove");
        //}
        var opts = $(list + ' li.activeCol:not(.disabled)');
        if (opts.length == 0) {
            alert("Nothing to remove");
        }
        $(opts).remove();
    };

    //Moves selected item(s) up or down in a list
    $.fn.moveUpDown = function (list, btnUp, btnDown) {
        var opts = $(list + ' li.activeCol:not(.disabled)');
        //if (opts.length == 0) {
        //    alert("Nothing to move");
        //}

        if (btnUp) {
            opts.first().prev().before(opts);
        } else if (btnDown) {
            opts.last().next().after(opts);
        }
    };
})(jQuery);
