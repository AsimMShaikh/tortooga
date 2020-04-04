var FeedbackRatingsList = {
    variables: {
        oTable: null,
        srcListFeedBackRatings: $('#hdnAppBaseURL').val() + 'Ratings/GetCompletedRatingsList/',
        srcSubmitRating: $('#hdnAppBaseURL').val() + 'Ratings/SubmitRatings'
    },
    controls: {
        tblYourRatingsList: '#tblYourRatingsList',
        btnSubmitRating: '#btnSubmitRating',
        IdsRow: '#IdsRow',
        divPendingRatings: '#divPendingRatings',
        hiderow: '.hiderow'
    },
    initDatatable_PendingRatings: function () {
        FeedbackRatingsList.variables.oTable = $(FeedbackRatingsList.controls.tblYourRatingsList).dataTableTortooga({
            "sAjaxSource": FeedbackRatingsList.variables.srcListFeedBackRatings,
            "aaSorting": [[0, "asc"]],// default sorting
            "sDom": "frtlip",
            "autoWidth": false,
            "bLengthChange": true,
            "aoColumnDefs": [
             {
                 "aTargets": [0],
             },
             {
                 "aTargets": [1],
             },
             {
                 "aTargets": [2],
             },
             {
                 "aTargets": [3],
             },
             {
                 "aTargets": [4],
             },
             {
                 "aTargets": [5],
             },
             {
                 "aTargets": [6],
                 "mRender": function (data, type, full) {
                     var finallist = "";
                     finallist += '<div class="star-rating-block">';
                     finallist += '<div class="starrr" data-rating="' + full[6] + '"></div>';
                     finallist += '<div class="grid-fav"></div></div>';
                     return finallist;
                 },
                 "bSortable": false
             },
             {
                 "aTargets": [7],
                 "mRender": function (data, type, full) {
                     var finallist = "";
                     finallist += '<div class="star-rating-block">';
                     finallist += '<div class="starrr" data-rating="' + full[7] + '"></div>';
                     finallist += '<div class="grid-fav"></div></div>';
                     return finallist;
                 },
                 "bSortable": false
             },
             {
                 "aTargets": [8],
                 "mRender": function (data, type, full) {
                     var finallist = "";
                     finallist += '<div class="star-rating-block">';
                     finallist += '<div class="starrr" data-rating="' + full[8] + '"></div>';
                     finallist += '<div class="grid-fav"></div></div>';
                     return finallist;
                 },
                 "bSortable": false
             },
             {
                 "aTargets": [9],
                 "mRender": function (data, type, full) {
                     var finallist = "";
                     finallist += '<div class="star-rating-block">';
                     finallist += '<div class="starrr" data-rating="' + full[9] + '"></div>';
                     finallist += '<div class="grid-fav"></div></div>';
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
                $(".starrr").starrr();
                return common.pagingText(iStart, iEnd, iTotal, "Records", oSettings._iDisplayLength);
            }
        });
    },
    //Reloading the Datatable
    reload_CompletedRatings: function () {
        FeedbackRatingsList.variables.oTable.dataTable().fnClearTable(0);
        FeedbackRatingsList.variables.oTable.dataTable().fnStandingRedraw();
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    FeedbackRatingsList.initDatatable_PendingRatings();
    $('#tblYourRatingsList_length').hide();

    $(FeedbackRatingsList.controls.hiderow).hide()

    //Submit Rating Button Click Event
    $(document).off('click', FeedbackRatingsList.controls.btnSubmitRating);
    $(document).on('click', FeedbackRatingsList.controls.btnSubmitRating, function (event) {
        var button = $(this);
        var row = button.closest('tr');
        var CarrierId = row.find('.row #IdsRow #lblCarrierId').text();
        var BookingId = row.find('.row #IdsRow #lblBookingId').text();
        var OverallExpRating = row.find('.row #star-Overall .glyphicon-star').length;
        var ServiceRating = row.find('.row #star-Service .glyphicon-star').length;
        var CommunicationRating = row.find('.row #star-Communication .glyphicon-star').length;
        var PriceRating = row.find('.row #star-Price .glyphicon-star').length;


        //Make an Ajax Post call to Submit the Ratings       
        var postData = {
            BookingID: BookingId,
            CarrierID: CarrierId,
            OverallExperience: OverallExpRating,
            Service: ServiceRating,
            Communication: CommunicationRating,
            Price: PriceRating,
        };

        $.ajax({
            type: "POST",
            url: FeedbackRatingsList.variables.srcSubmitRating,
            data: postData,
            success: function (res) {
                if (res.result.isSucceed) {
                    //Setting the Html for the Pending Ratings
                    $(FeedbackRatingsList.controls.divPendingRatings).empty();
                    $(FeedbackRatingsList.controls.divPendingRatings).html(res.stringFeedBackRatingsModel);

                    $(FeedbackRatingsList.controls.hiderow).hide();
                    $(".starrr").starrr();

                    //Reloading the JqueryDatatable
                    FeedbackRatingsList.reload_CompletedRatings();

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

});
