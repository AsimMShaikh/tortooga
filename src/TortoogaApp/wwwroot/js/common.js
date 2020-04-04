var common = {

    variables: {
        srcGetBadgeCounts: "/Base/GetBadgeCounts",
    },
    controls: {
        li_carrierAdmin_ratingsReceived: '#li_carrierAdmin_ratingsReceived',
        li_user_ratingsReceived: '#li_user_ratingsReceived',
        li_CarrierAdmin_Pending: '#li_CarrierAdmin_Pending',
        li_CarrierAdmin_Processing: '#li_CarrierAdmin_Processing',
        li_CarrierAdmin_Shipped: '#li_CarrierAdmin_Shipped',
        divManageBookingsCount: '#divManageBookingsCount'
    },

    pagingText: function (iStart, iEnd, iTotal, entity, pageSize) {
        iStart = iTotal == 0 ? 0 : iStart;
        var iEnd2 = (parseInt(iStart) + parseInt(pageSize) - 1);
        if (iEnd2 > iEnd) {
            iEnd2 = iEnd
        }
        if (iTotal == 0) {
            return "";
        }
        return "Showing " + iStart + " to " + iEnd2 + " of " + iTotal + " " + entity;
    },

    alertTortooga: function (message, fun) {
        bootbox.alert(message, fun);
    },

    //To show the Values for the Badge Count
    CalculateBadgeCount: function () {
        $.ajax({
            type: "GET",
            cache: false,
            url: common.variables.srcGetBadgeCounts,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.Success) {
                    if ($(common.controls.li_carrierAdmin_ratingsReceived).text() == "") {

                        if (response.data.RatingsReceived != 0) {
                            $(common.controls.li_user_ratingsReceived).find('a').append(' <span class="badge badge-warning pull-right">' + response.data.RatingsReceived + '</span>');
                        }
                    }
                    else if ($(common.controls.li_user_ratingsReceived).text() == "") {

                        if (response.data.RatingsReceived != 0)
                            $(common.controls.li_carrierAdmin_ratingsReceived).find('a').append(' <span class="badge badge-warning pull-right">' + response.data.RatingsReceived + '</span>');
                        if (response.data.PendingBookings != 0)
                            $(common.controls.li_CarrierAdmin_Pending).find('a').append(' <span class="badge badge-warning pull-right">' + response.data.PendingBookings + '</span>');
                        if (response.data.ProcessingBookings != 0)
                            $(common.controls.li_CarrierAdmin_Processing).find('a').append(' <span class="badge badge-warning pull-right">' + response.data.ProcessingBookings + '</span>');
                        if (response.data.ShippingBookings != 0)
                            $(common.controls.li_CarrierAdmin_Shipped).find('a').append(' <span class="badge badge-warning pull-right">' + response.data.ShippingBookings + '</span>');

                        var total = parseInt(response.data.PendingBookings) + parseInt(response.data.ProcessingBookings) + parseInt(response.data.ShippingBookings);

                        if (total != undefined && total != 0)
                            $(common.controls.divManageBookingsCount).append(' <span class="badge badge-success pull-right">' + total + '</span>');
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }
        });


    }
}

$(document).ready(function () {

    //To show the Badge Count
    common.CalculateBadgeCount();

});