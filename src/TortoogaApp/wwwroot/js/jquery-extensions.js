(function ($) {
    $.fn.dataTableExt.oApi.fnStandingRedraw = function (oSettings) {
        //redraw to account for filtering and sorting
        // concept here is that (for client side) there is a row got inserted at the end (for an add)
        // or when a record was modified it could be in the middle of the table
        // that is probably not supposed to be there - due to filtering / sorting
        // so we need to re process filtering and sorting
        // BUT - if it is server side - then this should be handled by the server - so skip this step
        if (oSettings.oFeatures.bServerSide === false) {
            var before = oSettings._iDisplayStart;
            oSettings.oApi._fnReDraw(oSettings);
            //iDisplayStart has been reset to zero - so lets change it back
            oSettings._iDisplayStart = before;
            oSettings.oApi._fnCalculateEnd(oSettings);
        }

        oSettings._iDisplayStart = 0;
        oSettings._iDisplayLength = $("#hdnGeneralPageSize").val();
        //draw the 'current' page
        oSettings.oApi._fnDraw(oSettings);
    };

    $.fn.dataTableTortooga = function (params) {
        params = $.extend({ "sEmptyTable": "Your search did not return any results. Please try again." }, params);
        params = $.extend({
            "fnInfoCallback": function (oSettings, iStart, iEnd, iMax, iTotal, sPre) {
                return common.pagingText(iStart, iEnd, iTotal, "Records", oSettings._iDisplayLength);
            },
        }, params);

        this.each(function () {
            var $t = $(this);
            params.bProcessing = true;
            params.bServerSide = true;
            params.iDisplayLength = $("#hdnGeneralPageSize").val();
            params.aLengthMenu = [[10, 20, 50, 100], [10, 20, 50, 100]];
            params.bFilter = false;
            params.bDestroy = true;
            params.sPaginationType = "full_numbers";
            params.fnDrawCallback = function (oInstance) {
                var p = $(this).parents(".dataTables_wrapper");
                var colLength = $(this).dataTable().fnSettings().aoColumns.length;

                var colsTemp = $(this).dataTable().fnSettings();
                if (oInstance._iRecordsTotal == 0) {
                    try {
                        for (var k = 0; k < colLength; k++) {
                            var p1 = colsTemp.aoColumns[k].nTh;
                            if (colsTemp.aoColumns[k].bSortable == false)
                                $(p1).data("isDefaultSort", 0);
                            else
                                $(p1).data("isDefaultSort", 1);

                            colsTemp.aoColumns[k].bSortable = false;
                            colsTemp.aoColumns[k].bSort = false;
                            colsTemp.aoColumns[k].bSortClasses = false;
                        }
                        p.find("thead th").addClass("sorting_disabled");
                        p.find("thead th").removeClass("sorting");
                        p.find("thead th").removeClass("sorting_asc");
                        p.find("thead th").removeClass("sorting_desc");
                        p.find("thead td").removeClass("sorting_1");
                        p.find("thead th").removeAttr("title");
                        p.find(".area-info").hide();
                    }
                    catch (e) {
                    }
                }
                else {
                    try {
                        for (var k = 0; k < colLength; k++) {
                            var p1 = colsTemp.aoColumns[k].nTh;
                            if (colsTemp.aoColumns[k].bSortable == false) {
                                if ($(p1).data("isDefaultSort") == 0) {
                                    $(p1).addClass("sorting_disabled");
                                    $(p1).removeClass("sorting");
                                }
                                else if ($(p1).data("isDefaultSort") == 1) {
                                    $(p1).attr("title", "Click here to sort");
                                    $(p1).addClass("sorting");
                                    $(p1).removeClass("sorting_disabled");
                                }
                                else {
                                    $(p1).data("isDefaultSort", 0);
                                    $(p1).addClass("sorting_disabled");
                                    $(p1).removeClass("sorting");
                                }
                            }
                            else {
                                $(p1).data("isDefaultSort", 1);
                                $(p1).attr("title", "Click here to sort");
                                $(p1).addClass("sorting");
                                $(p1).removeClass("sorting_disabled");
                            }

                            if ($(p1).data("isDefaultSort") == 0) {
                                colsTemp.aoColumns[k].bSortable = false;
                                colsTemp.aoColumns[k].bSort = false;
                                colsTemp.aoColumns[k].bSortClasses = false;
                            }
                            else {
                                colsTemp.aoColumns[k].bSortable = true;
                                colsTemp.aoColumns[k].bSort = true;
                                colsTemp.aoColumns[k].bSortClasses = true;
                            }
                        }
                    }
                    catch (e) {
                    }
                }
            };
            $t.data("params", params);
            $t.dataTable(params);
        });
        return this;
    };

    //$('button[type=reset]').live('click', function () {
    //    //$(this).resetValidation();
    //    $(this).formReset();
    //});
    //$.validator.addMethod('stringexceptHTML', function (value, element) {
    //    return this.optional(element) || /^[\p{L}\d]*[^<>]*$/.test(value);
    //}, "This field cannot contain html tags.");
})(jQuery);