var CarrierRatingsList = {
    variables: {
        oTable: null,
        srcListCarrierRatings: $('#hdnAppBaseURL').val() + 'Ratings/GetCarrierRatingsList/',
        srcSubmitRating: $('#hdnAppBaseURL').val() + 'Ratings/SubmitRatings'
    },
    controls: {

        tblCarrierRatingsList: '#tblCarrierRatingsList',
        ddlCarrierRatingsPageSizeSelection: '#ddlCarrierRatings_PageSizeSelection',
        txtCarrierRatingsSearch: '#txtCarrierRatingsSearch'

    },
    initDatatable: function () {
        CarrierRatingsList.variables.oTable = $(CarrierRatingsList.controls.tblCarrierRatingsList).dataTableTortooga({
            "sAjaxSource": CarrierRatingsList.variables.srcListCarrierRatings,
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
                     finallist += '<div class="col-sm-12 col-lg-4 col-md-6">';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Rated By</label><div class="col-md-8">';
                     finallist += full[12];   //Rated By
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Rated On</label><div class="col-md-8">';
                     finallist += full[13];   //Origin
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Origin</label><div class="col-md-8">';
                     finallist += full[0];   //Origin
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-4 control-label mb9">Destination</label><div class="col-md-8">';
                     finallist += full[1]; //Destination
                     finallist += '</div></div>';                                         
                     finallist += '</div>';
                     finallist += '<div class="col-sm-12 col-lg-3 col-md-6">';
                     finallist += '<div class="row"><label class="col-md-6 control-label mb9">Carrier Name</label><div class="col-md-6">';
                     finallist += full[2]; //Carrier Name
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-6 control-label">Listing Ref.</label><div class="col-md-6">';
                     finallist += full[3]; // Listing Ref.
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-6 control-label">Booking Ref.</label><div class="col-md-6">';
                     finallist += full[4]; //Booking Ref.
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-6 control-label">Departure Date</label><div class="col-md-6">';
                     finallist += full[5]; //Departure Date.
                     finallist += '</div></div>';
                     finallist += '<div class="row"><label class="col-md-6 control-label">Arrival Date</label><div class="col-md-6">';
                     finallist += full[6]; // Arrival Date
                     finallist += '</div></div>';
                     finallist += '</div>';
                     finallist += '<div class="col-sm-12 col-lg-5 col-md-12"><div class="row"><label class="col-md-5 control-label">Overall Experience</label>';
                     finallist += '<div class="col-md-7"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[7] + '"></div>'; //Overall Exprience
                     finallist += '</div><div class="grid-fav"></div></div>';
                     finallist += '</div>';
                     finallist += '<div class="row"><label class="col-md-5 control-label">Service</label>';
                     finallist += '<div class="col-md-7"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[8] + '"></div>'; //Service
                     finallist += '</div><div class="grid-fav"></div></div>';
                     finallist += '</div>';
                     finallist += '<div class="row"><label class="col-md-5 control-label">Communication</label>';
                     finallist += '<div class="col-md-7"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[9] + '"></div>'; //Communication
                     finallist += '</div> <div class="grid-fav"></div></div>';
                     finallist += '</div>';
                     finallist += '<div class="row"><label class="col-md-5 control-label">Price</label>';
                     finallist += '<div class="col-md-7"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[10] + '"></div>'; //Price;
                     finallist += '</div></div>';
                     finallist += '</div></div></div>';

                     return finallist;
                 },
                 "bSortable": false
             }],

            "oLanguage": {
                "sEmptyTable": "No Ratings(s) available",
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
    reload_CompletedRatings: function () {
        CarrierRatingsList.variables.oTable.dataTable().fnClearTable(0);
        CarrierRatingsList.variables.oTable.dataTable().fnStandingRedraw();
    },
    //Resetting the Grid When the TextBox is Empty
    Reset: function () {
        $(CarrierRatingsList.controls.txtCarrierRatingsSearch).val('');
        $(CarrierRatingsList.controls.ddlCarrierRatingsPageSizeSelection).val('10')
        $("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(CarrierRatingsList.variables.srchKey);
        CarrierRatingsList.reload_CompletedRatings();
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    CarrierRatingsList.initDatatable();
    $('#tblCarrierRatingsList_length').hide();

    //Page Size DropDown Change Event
    $(document).off('change', CarrierRatingsList.controls.ddlCarrierRatingsPageSizeSelection);
    $(document).on('change', CarrierRatingsList.controls.ddlCarrierRatingsPageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(CarrierRatingsList.controls.ddlCarrierRatingsPageSizeSelection).val())
        $("div").removeData(CarrierRatingsList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierRatingsList.controls.txtCarrierRatingsSearch).val() == '' ? '' : $(CarrierRatingsList.controls.txtCarrierRatingsSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
           ]
       );
        CarrierRatingsList.reload_CompletedRatings();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    //Search Via the Search TextBox
    $(CarrierRatingsList.controls.txtCarrierRatingsSearch).keypress(function (e) {
        if (e.keyCode == 13) {
            if ($(this).val().trim() != '') {
                $("div").removeData(CarrierRatingsList.variables.srchKey);
                $("div").data("srchParams",
                   [{ name: 'srchTxt', value: encodeURIComponent($(CarrierRatingsList.controls.txtCarrierRatingsSearch).val() == '' ? '' : $(CarrierRatingsList.controls.txtCarrierRatingsSearch).val()) },
                    { name: 'srchBy', value: 'ALL' },
                    { name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
                   ]);
                CarrierRatingsList.reload_CompletedRatings();
            }
            else {
                CarrierRatingsList.Reset();
            }
        }
    });

});