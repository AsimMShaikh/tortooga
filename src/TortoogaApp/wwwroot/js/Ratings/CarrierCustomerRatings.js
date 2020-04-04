var CarrierCustomerRatings = {
    variables: {
        oTable: null,
        srcListCustomerPastRatingsList: $('#hdnAppBaseURL').val() + 'Ratings/GetCustomerPastRatingsList/',
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
        CarrierCustomerRatings.variables.oTable = $(CarrierCustomerRatings.controls.tblPastCustomerRatingList).dataTableTortooga({
            "sAjaxSource": CarrierCustomerRatings.variables.srcListCustomerPastRatingsList,
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
                     if (full[8] != undefined && full[8] != '' && full[8] != null) {
                         finallist += '<img src="' + full[10] + '" style="width:100%;" />';
                     }
                     else {
                         finallist += '<img src="/images/defaultusergrid.jpg" />';
                     }
                     finallist += '<div class="clearfix"></div>';
                     finallist += '<label class="control-label">' + full[6] + '</label>';
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
                     finallist += '<div class="col-sm-12 col-lg-2 col-md-12"><div class="row"><label class="col-md-12 control-label">You Rated</label>';
                     finallist += '<div class="col-md-12"><div class="star-rating-block"><div id="stars" class="gridstarrr text-left" data-rating="'+full[7]+'"><span class="glyphicon .glyphicon-star-empty glyphicon-star"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span><span class="glyphicon .glyphicon-star-empty glyphicon-star-empty"></span></div></div><div class="grid-fav"></div></div></div></div>';

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
        CarrierCustomerRatings.variables.oTable.dataTable().fnClearTable(0);
        CarrierCustomerRatings.variables.oTable.dataTable().fnStandingRedraw();
    },
    //Resetting the Grid When the TextBox is Empty
    Reset: function () {
        $(CarrierCustomerRatings.controls.txtPastCustomerRating).val('');
        $(CarrierCustomerRatings.controls.ddlPastCustomerRatings_PageSizeSelection).val('10')
        $("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(CarrierCustomerRatings.variables.srchKey);
        CarrierCustomerRatings.reload();
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    CarrierCustomerRatings.initDatatable();
    $('#tblPastCustomerRatingList_length').hide();

    $(CarrierCustomerRatings.controls.hiderow).hide()

    //Submit Rating Button Click Event
    $(document).off('click', CarrierCustomerRatings.controls.btnSubmitCustomerRating);
    $(document).on('click', CarrierCustomerRatings.controls.btnSubmitCustomerRating, function (event) {
       
        var button = $(this);
        var row = button.closest('tr');
        var CarrierId = row.find('.row #IdsRow #lblCarrierId').text();
        var BookingId = row.find('.row #IdsRow #lblBookingId').text();
        var UserIdValue = row.find('.row #IdsRow #lblUserId').text();
        var RatingValue = row.find('.row #star-CustomerRatings .glyphicon-star').length;

        //Make an Ajax Post call to Submit the Ratings       
        var postData = {
            BookingID: BookingId,
            CarrierID: CarrierId,
            Rating: RatingValue,
            UserId: UserIdValue
        };

        $.ajax({
            type: "POST",
            url: CarrierCustomerRatings.variables.srcSubmitCustomerRating,
            data: postData,
            success: function (res) {
                if (res.result.isSucceed) {
                    //Setting the Html for the Pending Ratings
                    $(CarrierCustomerRatings.controls.divCarrierPendingRatings).empty();
                    $(CarrierCustomerRatings.controls.divCarrierPendingRatings).html(res.stringCustomerRatingModel);

                    $(CarrierCustomerRatings.controls.hiderow).hide();
                    $(".starrr").starrr();

                    //Reloading the JqueryDatatable
                    CarrierCustomerRatings.reload();

                    //Show the Alert Message for Completion
                    common.alertTortooga("Ratings Submitted Sucessfully");

                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }

        });

        ////$(FeedbackRatingsList.controls.divPendingRatings).empty();
        ////$(FeedbackRatingsList.controls.divPendingRatings).html('asdfasdfasdfsaf');  


    });

    //Page Size DropDown Change Event
    $(document).off('change', CarrierCustomerRatings.controls.ddlPastCustomerRatings_PageSizeSelection);
    $(document).on('change', CarrierCustomerRatings.controls.ddlPastCustomerRatings_PageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(CarrierCustomerRatings.controls.ddlCarrierRatingsPageSizeSelection).val())
        $("div").removeData(CarrierCustomerRatings.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierCustomerRatings.controls.txtPastCustomerRating).val() == '' ? '' : $(CarrierCustomerRatings.controls.txtPastCustomerRating).val()) },
            { name: 'srchBy', value: 'ALL' },
           ]
       );
        CarrierCustomerRatings.reload();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    //Search Via the Search TextBox
    $(CarrierCustomerRatings.controls.txtPastCustomerRating).keypress(function (e) {
        if (e.keyCode == 13) {
            if ($(this).val().trim() != '') {
                $("div").removeData(CarrierCustomerRatings.variables.srchKey);
                $("div").data("srchParams",
                   [{ name: 'srchTxt', value: encodeURIComponent($(CarrierCustomerRatings.controls.txtPastCustomerRating).val() == '' ? '' : $(CarrierCustomerRatings.controls.txtPastCustomerRating).val()) },
                    { name: 'srchBy', value: 'ALL' },
                    { name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
                   ]);
                CarrierCustomerRatings.reload();
            }
            else {
                CarrierCustomerRatings.Reset();
            }
        }
    });


});