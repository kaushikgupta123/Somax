var dtPartReviewTable;
$(function () {
    GetPartsReviewGrid();
    //#region V2-1215 Zoom Image
    function initZoom() {
        $('.zoomable').each(function () {
            // Remove previous zoom if exists
            if ($(this).data('elevateZoom')) {
                $(this).removeData('elevateZoom');
                $('.zoomContainer').remove(); // remove old zoom container
                $(this).off(); // unbind old events
            }

            // Apply elevateZoom
            $(this).elevateZoom({
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
            });
        });
    }

    // Initial run
    initZoom();

    // Re-apply zoom after each table redraw (e.g. paging, sorting)
    dtPartReviewTable.on('draw.dt', function () {
        initZoom();
    });
    //#endregion
});
function GetPartsReviewGrid() {
    if ($(document).find('#tblpartReview').hasClass('dataTable')) {
        dtPartReviewTable.destroy();
    }
    dtPartReviewTable = $("#tblpartReview").DataTable({
        colReorder: true,
        rowGrouping: true,
        searching: true,
        "bProcessing": true,
        serverSide: true,
        "pagingType": "full_numbers",
        "bDeferRender": true,
        "order": [[1, "asc"]],
        stateSave: true,
        language: {
            url: "/base/GetDataTableLanguageJson?nGrid=" + true,
        },
        sDom: 'Btlipr',
        buttons: [],
        "orderMulti": true,
        "ajax": {
            "url": "/PartsReview/GetPartsReviewMainGrid",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                d.SearchText = LRTrim($(document).find('#ptrsearch').val());
            },
            "dataSrc": function (json) {
                HidebtnLoader("btnpartreviewsearch");
                return json.data;
            }
        },

        "columns":
        [
            {
                "data": "ImageURL",
                "bVisible": true,
                "bSortable": false,
                "autoWidth": false,
                "bSearchable": false,
                "name": "0",
                "mRender": function (data, type, row) {
                    return '<img src="' + data + '" data-zoom-image="' + data + '" class="zoomable" style="max-width: 100px;"/>';
                }
            },
            {
                "data": "PartId",
                "bVisible": true,
                "bSortable": false,
                "autoWidth": false,
                "bSearchable": false,
                "name": "1",
                "mRender": function (data, type, row) {
                    /*if (row.ChildCount > 0) {*/
                        return '<img id="' + data + '" class="clsinnergrid" src="../../Images/details_open.png" alt="expand/collapse" rel="' + data + '" style="cursor: pointer;"/>';
                    //}
                    //else {
                    //    return '';
                    //}
                }
            },
            { "data": "ClientLookupId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "2" },
            { "data": "LongDescription", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "3" },
            { "data": "Manufacturer", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "4" },
            { "data": "ManufacturerId", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "5" },
            { "data": "Category", "autoWidth": true, "bSearchable": true, "bSortable": true, "name": "6" },
            { "data": "CategoryMasterDescription", "bSearchable": true, "bSortable": true, "name": "7" }
        ],
        initComplete: function () {
            $(document).find('#tblpartReview').on('click', '.clsinnergrid', function (e) {
                var tr = $(this).closest('tr');
                var row = dtPartReviewTable.row(tr);
                if (this.src.match('details_close')) {
                    this.src = "../../Images/details_open.png";
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    this.src = "../../Images/details_close.png";
                    var PartMasterId = $(this).attr("rel");
                    $.get("/PartsReview/PopulateReviewSite/?PartMasterId=" + PartMasterId, function (InnerGridLineItemModel) {
                        row.child(InnerGridLineItemModel).show();
                        dtinnerGrid = row.child().find('.PartsReviewInnerTable').DataTable(
                            {
                                "order": [[0, "asc"]],
                                paging: false,
                                searching: false,
                                "bProcessing": true,
                                responsive: true,
                                scrollY: 300,
                                "scrollCollapse": true,
                                sDom: 'Btlpr',
                                language: {
                                    url: "/base/GetDataTableLanguageJson?nGrid=" + true,
                                },
                                buttons: [],
                                "columnDefs": [
                                    {
                                        "targets": 2,
                                        "data": "download_link",
                                        "mRender": function (data, type, row) {
                                            return "<div class='text-wrap width-100'>" + data + "</div>";
                                        }
                                    }

                                ],
                                select: {
                                    style: 'os',
                                    selector: 'td:nth-child(6)'
                                },
                                initComplete: function () {
                                    //SetPageLengthMenu();
                                }
                            });

                        tr.addClass('shown');
                    });
                }
            });

            SetPageLengthMenu();
        }
    });
};
$(document).on('click', '#btnpartreviewsearch', function () {
    ShowbtnLoader("btnpartreviewsearch");
    dtPartReviewTable.page('first').draw('page');
});
$('#ptrsearch').keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        dtPartReviewTable.page('first').draw('page');
    }
});