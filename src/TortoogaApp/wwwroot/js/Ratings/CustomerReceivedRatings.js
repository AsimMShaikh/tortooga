var CustomerReceivedRatings = {

    variables: {
        oTable: null,
        srcListCustomerPastRatingsList: $('#hdnAppBaseURL').val() + 'Ratings/GetCustomerReceivedRatingsList/',
        srcSubmitCustomerRating: $('#hdnAppBaseURL').val() + 'Ratings/SubmitCustomerRatings'
    },
    controls: {
        tblPastCustomerRatingList: '#tblPastCustomerRatingList',
        ddlPastCustomerRatings_PageSizeSelection: '#ddlPastCustomerRatings_PageSizeSelection',
        txtPastCustomerRating: '#txtPastCustomerRating',
        btnSubmitCustomerRating: '#btnSubmitCustomerRating',
        hiderow: '.hiderow',
        divCarrierPendingRatings: '#divCarrierPendingRatings',
    },
    initDatatable: function () {
        CustomerReceivedRatings.variables.oTable = $(CustomerReceivedRatings.controls.tblPastCustomerRatingList).dataTableTortooga({
            "sAjaxSource": CustomerReceivedRatings.variables.srcListCustomerPastRatingsList,
            "aaSorting": [[0, "asc"]],// default sorting
            "sDom": "frtlip",
            "autoWidth": false,
            "bLengthChange": true,
            "aoColumnDefs": [
             {
                 "aTargets": [0],
                 "mRender": function (data, type, full) {
                     var finallist = "";
                     finallist += ' <div class="row">';
                     finallist += '<div class="col-xs-12 col-sm-12 col-lg-2 col-md-2 text-center">';
                     if (full[10] != undefined && full[10] != '' && full[10] != null) {
                         finallist += '<img src="' + full[10] + '" style="width:100%;" />';
                         finallist += full[2];   //Company Name.
                     }
                     else {
                         finallist += '<img src="/images/companydefaultlogo_gird.png" />';
                         finallist += '<div class="clearfix"></div>';
                         finallist += full[2];   //Company Name.
                     }
                     finallist += '</div>';
                     finallist += '<div class="col-sm-12 col-lg-4 col-md-3">';
                     //finallist += '<div class="row"><label class="col-md-4 control-label mb9">Company Name</label><div class="col-md-8">';
                     //finallist += full[2];   //Company Name.
                     //finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Booking Ref.</label><div class="col-md-8">';
                     finallist += full[4];   //Booking Ref.
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Listing Ref.</label><div class="col-md-8">';
                     finallist += full[3];   //Listing Ref.
                     finallist += '</div></div>';
                     finallist += '</div>';
                     finallist += '<div class="col-sm-12 col-lg-4 col-md-3">';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Origin</label><div class="col-md-8">';
                     finallist += full[0];   //Origin
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Destination</label><div class="col-md-8">';
                     finallist += full[1]; //Destination
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Departed Date</label><div class="col-md-8">';
                     finallist += full[5]; //Departed
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Arrival Date</label><div class="col-md-8">';
                     finallist += full[6]; //Arrival
                     finallist += '</div></div>';
                     finallist += '</div>';
                     finallist += '<div class="col-sm-12 col-lg-2 col-md-12"><div class="row"><label class="col-md-12 control-label">Received Ratings</label>';
                     finallist += '<div class="col-md-12"><div class="star-rating-block"><div id="stars" class="gridstarrr" data-rating="' + full[9] + '"><span class="glyphicon .glyphicon-star-empty glyphicon-star"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span></div></div><div class="grid-fav"></div></div></div></div>';

                     return finallist;
                 },
                 "bSortable": false
             }],

            "oLanguage": {
                "sEmptyTable": "No Past Ratings(s) available",
                "sLengthMenu": "Page Size: _MENU_",
                "oPaginate": {
                    "sFirst": "First",
                    "sLast": "Last",
                    "sNext": "Next",
                    "sPrevious": "Previous"
                }
            },
            "fnServerParams": function (aoData) {

                var srchParams = $("div").data("srchParams");
                if (srchParams) {
                    for (var i = 0; i < srchParams.length; i++) {
                        aoData.push({ "name": "" + srchParams[i].name + "", "value": "" + srchParams[i].value + "" });
                    }
                }
            },
            "fnInfoCallback": function (oSettings, iStart, iEnd, iMax, iTotal, sPre) {
                $(".gridstarrr").starrr();
                return common.pagingText(iStart, iEnd, iTotal, "Records", oSettings._iDisplayLength);
            }
        });
    },
    //Reloading the Datatable
    reload: function () {
        CustomerReceivedRatings.variables.oTable.dataTable().fnClearTable(0);
        CustomerReceivedRatings.variables.oTable.dataTable().fnStandingRedraw();
    },
    //Resetting the Grid When the TextBox is Empty
    Reset: function () {
        $(CustomerReceivedRatings.controls.txtPastCustomerRating).val('');
        $(CustomerReceivedRatings.controls.ddlPastCustomerRatings_PageSizeSelection).val('10')
        $("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(CustomerReceivedRatings.variables.srchKey);
        CustomerReceivedRatings.reload();
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    CustomerReceivedRatings.initDatatable();
    $('#tblPastCustomerRatingList_length').hide();

    $(CustomerReceivedRatings.controls.hiderow).hide();

    //Page Size DropDown Change Event
    $(document).off('change', CustomerReceivedRatings.controls.ddlPastCustomerRatings_PageSizeSelection);
    $(document).on('change', CustomerReceivedRatings.controls.ddlPastCustomerRatings_PageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(CustomerReceivedRatings.controls.ddlCarrierRatingsPageSizeSelection).val())
        $("div").removeData(CustomerReceivedRatings.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(CustomerReceivedRatings.controls.txtPastCustomerRating).val() == '' ? '' : $(CustomerReceivedRatings.controls.txtPastCustomerRating).val()) },
            { name: 'srchBy', value: 'ALL' },
           ]
       );
        CustomerReceivedRatings.reload();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    //Search Via the Search TextBox
    $(CustomerReceivedRatings.controls.txtPastCustomerRating).keypress(function (e) {
        if (e.keyCode == 13) {
            if ($(this).val().trim() != '') {
                $("div").removeData(CustomerReceivedRatings.variables.srchKey);
                $("div").data("srchParams",
                   [{ name: 'srchTxt', value: encodeURIComponent($(CustomerReceivedRatings.controls.txtPastCustomerRating).val() == '' ? '' : $(CustomerReceivedRatings.controls.txtPastCustomerRating).val()) },
                    { name: 'srchBy', value: 'ALL' },
                    { name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
                   ]);
                CustomerReceivedRatings.reload();
            }
            else {
                CustomerReceivedRatings.Reset();
            }
        }
    });


});

