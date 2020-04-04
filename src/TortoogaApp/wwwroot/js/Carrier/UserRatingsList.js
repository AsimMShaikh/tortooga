var UserRatingsList = {
    variables: {
        oTable: null,
        srcListUserRatings: $('#hdnAppBaseURL').val() + 'Carrier/GetUserRatingsList?id=' + $('#hdnCarrierGuid').val(),
        srchKey:''
    },
    controls: {

        tblUserRatingsList: '#tblUserRatingsList',
        ddlCarrierRatingsPageSizeSelection: '#ddlCarrierRatings_PageSizeSelection'
        //txtCarrierRatingsSearch: '#txtUserRatingsSearch'

    },
    initDatatable: function () {
        debugger;
        UserRatingsList.variables.oTable = $(UserRatingsList.controls.tblUserRatingsList).dataTableTortooga({
            "sAjaxSource": UserRatingsList.variables.srcListUserRatings,
            "aaSorting": [[0, "asc"]],// default sorting
            "sDom": "frtlip",
            "autoWidth": false,
            "bLengthChange": true,
            "aoColumnDefs": [
             {
                 "aTargets": [0],
                 "mRender": function (data, type, full) {                   
                     var finallist = "";
                     finallist += '<li class="clearfix row">';
                     finallist +='<div class="img-block col-md-2 text-center">';
                     finallist += '<img src="' + full[8] + '" alt="" class="img-responsive"  />';
                    finallist +='</div>';
                    finallist += '<div class="content-block col-md-10">';
                    finallist += '<div class="clearfix">';
                    finallist +='<div class="top-part row">';
                    finallist += '<div class="col-md-12 mr10"><h4>' + full[6] + ' <span class="ml20">' + full[7] + '</span></h4> </div></div>';
                    finallist += '<div class="row">';                    
                     finallist += '<div class="col-md-2"><label>OverAll Experience</label></div><div class="col-md-2"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[1] + '"></div>';
                     finallist += '</div> <div class="grid-fav"></div></div></div>';                     
                     finallist += '</div>';
                     finallist += '<div class="row">';
                     finallist += '<div class="col-md-2"><label>Service</label></div><div class="col-md-2"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[2] + '"></div>';
                     finallist += '</div> <div class="grid-fav"></div></div>';
                     finallist += '</div>';
                     finallist += '<div class="row">';
                     finallist += '<div class="col-md-2"><label>Communication</label></div><div class="col-md-2"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[3] + '"></div>';
                     finallist += '</div> <div class="grid-fav"></div></div>';
                     finallist += '</div>';
                     finallist += '<div class="row">';
                     finallist += '<div class="col-md-2"><label>Price</label></div><div class="col-md-2"><div class="star-rating-block">';
                     finallist += '<div id="stars" class="gridstarrr" data-rating="' + full[4] + '"></div>';
                     finallist += '</div> <div class="grid-fav"></div></div></div>';
                     finallist += '</div>';
                    finallist += '</div></li>';
                          

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
        UserRatingsList.variables.oTable.dataTable().fnClearTable(0);
        UserRatingsList.variables.oTable.dataTable().fnStandingRedraw();
    },
    //Resetting the Grid When the TextBox is Empty
    Reset: function () {
        //$(UserRatingsList.controls.txtCarrierRatingsSearch).val('');
        $(UserRatingsList.controls.ddlCarrierRatingsPageSizeSelection).val('10')
        $("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(UserRatingsList.variables.srchKey);
        UserRatingsList.reload_CompletedRatings();
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    UserRatingsList.initDatatable();
    $('#tblUserRatingsList_length').hide();

    //Page Size DropDown Change Event
    $(document).off('change', UserRatingsList.controls.ddlCarrierRatingsPageSizeSelection);
    $(document).on('change', UserRatingsList.controls.ddlCarrierRatingsPageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(UserRatingsList.controls.ddlCarrierRatingsPageSizeSelection).val())
        $("div").removeData(UserRatingsList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(UserRatingsList.controls.txtCarrierRatingsSearch).val() == '' ? '' : $(UserRatingsList.controls.txtCarrierRatingsSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
           ]
       );
        UserRatingsList.reload_CompletedRatings();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    //Search Via the Search TextBox
    //$(UserRatingsList.controls.txtCarrierRatingsSearch).keypress(function (e) {
    //    if (e.keyCode == 13) {
    //        if ($(this).val().trim() != '') {
    //            $("div").removeData(UserRatingsList.variables.srchKey);
    //            $("div").data("srchParams",
    //               [{ name: 'srchTxt', value: encodeURIComponent($(UserRatingsList.controls.txtCarrierRatingsSearch).val() == '' ? '' : $(UserRatingsList.controls.txtCarrierRatingsSearch).val()) },
    //                { name: 'srchBy', value: 'ALL' },
    //                { name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
    //               ]);
    //            UserRatingsList.reload_CompletedRatings();
    //        }
    //        else {
    //            UserRatingsList.Reset();
    //        }
    //    }
    //});

});