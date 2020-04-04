var CarrierDashboard = {

    variables: {
        srcGetCarrierDashboardData: $('#hdnAppBaseURL').val() + 'Dashboard/GetCarrierDashboardData',
        srcUpdateBookingStatus: $('#hdnAppBaseURL').val() + 'Dashboard/UpdateBookingStatus',
        srcGetPendingBookings: $('#hdnAppBaseURL').val() + 'Dashboard/GetPendingBookingsDetails',
        srcGetCustomerRatings: $('#hdnAppBaseURL').val() + 'Dashboard/GetCustomerRatings',
        srcUpdateNotificationToRead: "/Dashboard/UpdateNotificationToRead",
        PanelArray: [],
    },
    controls: {
        tblCarrierPendingBookings: '#tblCarrierPendingBookings',
        tblCarrierBookingReports: '#tblCarrierBookingReports',
        hdnUserId: '#hdnUserId',
        hdnIsAdmin: '#hdnIsAdmin',
        hdnCarrierId: '#hdnCarrierId',
        lblCarrier_AwaitingApproval: '#lblCarrier_AwaitingApproval',
        lblCarrier_InTransit: '#lblCarrier_InTransit',
        lblCarrier_Completed: '#lblCarrier_Completed',
        lblCarrier_TotalMonthBookings: '#lblCarrier_TotalMonthBookings',
        lblCarrier_TotalMonthRevenue: '#lblCarrier_TotalMonthRevenue',
        lblCarrier_CurrentEscrowTotal: '#lblCarrier_CurrentEscrowTotal',
        lblCarrier_TotalReviewsCurrentMonth: '#lblCarrier_TotalReviewsCurrentMonth',
        lblCarrier_AverageProfitPerft: '#lblCarrier_AverageProfitPerft',
        ddlPanelHandling: '#ddlPanelHandling',
        divNotifications: '#divNotifications'
    },

    getDashBoardData: function () {

        $.ajax({
            type: "GET",
            url: CarrierDashboard.variables.srcGetCarrierDashboardData + "?CarrierId=" + $(CarrierDashboard.controls.hdnCarrierId).val(),
            cache: false,
            success: function (response, textStatus, xhr) {
                if (response.Success) {

                    CarrierDashboard.SetAccountSummaryDetails(response.data.AccountSummary);
                    CarrierDashboard.BindCarrierPendingBokkings(response.data.PendingBookingsModel);
                    CarrierDashboard.BindNotificationsPanel(response.data.Notifications);
                    CarrierDashboard.BindCarrierBookingsReport(response.data.BookingReports);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }
        });
    },

    //To Set the Account Summary Details
    SetAccountSummaryDetails: function (AccountSummary) {

        if (AccountSummary != null && AccountSummary != undefined && AccountSummary != '') {

            $(CarrierDashboard.controls.lblCarrier_AwaitingApproval).text(AccountSummary.AwaitingApproval);
            $(CarrierDashboard.controls.lblCarrier_InTransit).text(AccountSummary.InTransit);
            $(CarrierDashboard.controls.lblCarrier_Completed).text(AccountSummary.Completed);
            $(CarrierDashboard.controls.lblCarrier_TotalMonthBookings).text(AccountSummary.TotalMonthBookings);
            $(CarrierDashboard.controls.lblCarrier_TotalMonthRevenue).text(AccountSummary.TotalMonthRevenue);
            $(CarrierDashboard.controls.lblCarrier_CurrentEscrowTotal).text(AccountSummary.CurrentEscrowTotal);
            $(CarrierDashboard.controls.lblCarrier_TotalReviewsCurrentMonth).text(AccountSummary.TotalReviewsCurrentMonth);
            $(CarrierDashboard.controls.lblCarrier_AverageProfitPerft).text(AccountSummary.AverageProfitPerft);
        }
        else {
            $(CarrierDashboard.controls.lblCarrier_AwaitingApproval).text(0);
            $(CarrierDashboard.controls.lblCarrier_InTransit).text(0);
            $(CarrierDashboard.controls.lblCarrier_Completed).text(0);
            $(CarrierDashboard.controls.lblCarrier_TotalMonthBookings).text(0);
            $(CarrierDashboard.controls.lblCarrier_TotalMonthRevenue).text(0);
            $(CarrierDashboard.controls.lblCarrier_CurrentEscrowTotal).text(0);
            $(CarrierDashboard.controls.lblCarrier_TotalReviewsCurrentMonth).text(0);
            $(CarrierDashboard.controls.lblCarrier_AverageProfitPerft).text(0);
        }

    },

    //To Bind the Carrier Pending Booking
    BindCarrierPendingBokkings: function (ListOfPendingBookings) {

        $("#tblCarrierPendingBookings > tbody").html("");

        if (ListOfPendingBookings.length > 0) {

            //Fetching and Pushing the Rows from the Grid 
            $.each(ListOfPendingBookings, function (index, element) {

                var listingURL = $('#hdnAppBaseURL').val() + 'Listing/' + element.ListingID + '/View';
                var tablerow = "";
                tablerow += '<tr>';
                tablerow += '<td><a href="' + listingURL + '" target="_blank">' + element.Listing + '</a></td>'; // Listing
                tablerow += '<td>' + element.Origin + '</td>'; // Origin
                tablerow += '<td>' + element.Destination + '</td>'; // Destination
                tablerow += '<td>' + element.Amount + '</td>'; // Amount
                tablerow += '<td><a onclick="CarrierDashboard.ApproveBookingConfirm(\'' + element.BookingID + '\')"><span class="btn btn-success">Approve</span></a>&nbsp;<a onclick="CarrierDashboard.RejectBookingConfirm(\'' + element.BookingID + '\')"><span class="btn btn-danger">Reject</span></a></td>';
                tablerow += '</tr>';

                $('#tblCarrierPendingBookings tbody').append(tablerow);
            });

        }
        else {
            var tablerow = "";
            tablerow += '<tr>';
            tablerow += '<td align="center" colspan="5">No Pending Booking(s) Found.</td>';
            tablerow += '</tr>';

            $('#tblCarrierPendingBookings tbody').append(tablerow);
        }

    },

    //Asking the Carrier to Confirmation the booking - Approve
    ApproveBookingConfirm: function (ele, id) {
        bootbox.confirm("Are you sure you want to approve the booking", function (result) {
            if (result == true) {
                CarrierDashboard.UpdateBooking(ele, "Approve");
            }
        });
    },

    //Asking the Carrier to Confirmation the booking -- Reject
    RejectBookingConfirm: function (ele, id) {
        bootbox.confirm("Are you sure you want to reject the booking", function (result) {
            if (result == true) {
                CarrierDashboard.UpdateBooking(ele, "Reject");
            }
        });
    },

    UpdateBooking: function (BookingId, AcitonStatus) {
        $.ajax({
            type: "GET",
            url: CarrierDashboard.variables.srcUpdateBookingStatus + "?BookingId=" + BookingId + "&Status=" + AcitonStatus,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.Success == true) {
                    common.alertTortooga("Booking Status Updated Successfully", null);

                    //Fetch the Pending Booking Records and Bind Again                    
                    $.ajax({
                        url: CarrierDashboard.variables.srcGetCarrierDashboardData + "?CarrierId=" + $(CarrierDashboard.controls.hdnCarrierId).val(),
                        type: "get",
                        success: function (response) {
                            if (response.Success == true) {

                                CarrierDashboard.SetAccountSummaryDetails(response.data.AccountSummary);
                                CarrierDashboard.BindCarrierPendingBokkings(response.data.PendingBookingsModel);
                                CarrierDashboard.BindNotificationsPanel(response.data.Notifications);
                                CarrierDashboard.BindCarrierBookingsReport(response.data.BookingReports);
                            }
                            else {
                                common.alertTortooga("Pending Bookings Were Not Refreshed");
                            }
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            common.alertTortooga(errorThrown);
                        }
                    });
                }
                else {
                    common.alertTortooga(response.message);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }
        });
    },

    //Binding the Carrier Notifications Panel
    BindNotificationsPanel: function (Notifications) {       

        var divNotifications = $(CarrierDashboard.controls.divNotifications);

        if (Notifications.Orders != undefined && Notifications.Orders.length > 0) {
            var divstring = "";

            for (var i = 0 ; i < Notifications.Orders.length; i++) {

                divstring = "";

                switch (Notifications.Orders[i]) {

                    case "NewBooking":
                        var object = Notifications.NewBooking.filter(function (element) {
                            return element.OrderNo == parseInt(i + 1);
                        })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-ship"> New Booking</i>';
                        divstring += '<span class="pull-right text-muted small">';
                        divstring += '<em>' + object[0].NotificationTime + '</em>';
                        divstring += '</span>';
                        divstring += '</a>';
                        break;
                    case "RatingReceived":
                        var object = Notifications.RatingReceived.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-star-half-full"> Rating Received</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentArrival":
                        var object = Notifications.ShipmentArrivals.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Arrival</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentShipped":
                        var object = Notifications.ShipmentShipped.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Shipped</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentDelayed":
                        var object = Notifications.ShipmentDelayed.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Delayed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentCancelled":
                        var object = Notifications.ShipmentCancelled.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Cancelled</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "NewMessage":
                        var object = Notifications.ShipmentCancelled.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="CarrierDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Delayed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;


                }

                divNotifications.append(divstring);
            }
        }
        else { //No Notifications

            divNotifications.append('<i class="fa fa-comment"> No New Notifications</i>');
        }

    },

    //Changing the Notification Status to Read
    ChangeNotificationToRead: function (URL, NotificationId) {

        if (NotificationId != '' && NotificationId != undefined && NotificationId != null) {
            $.ajax({
                type: "GET",
                url: CarrierDashboard.variables.srcUpdateNotificationToRead + "?NotificationId=" + NotificationId,
                success: function (response, textStatus, xhr) {
                    if (response.Success) {
                        window.location.href = URL;
                    }
                }, error: function (xhr, textStatus, errorThrown) {
                    common.alertTortooga(errorThrown);
                }
            });


        }
    },

    //To Bind the Carrier Booking Reports
    BindCarrierBookingsReport: function (ListOfBookingReports) {

        $("#tblCarrierBookingReports > tbody").html("");

        if (ListOfBookingReports.length > 0) {
            //Fetching and Pushing the Rows from the Grid 
            $.each(ListOfBookingReports, function (index, element) {

                var listingURL = $('#hdnAppBaseURL').val() + 'Listing/' + element.ListingID + '/View';
                var tablerow = "";
                tablerow += '<tr>';
                tablerow += '<td>' + element.Date.split('T')[0] + '</td>'; // Date
                tablerow += '<td><a href="' + listingURL + '" target="_blank">' + element.Details + '</a></td>'; //Details
                tablerow += '<td>' + element.BookedBy + '</td>'; // Booked By
                tablerow += '<td>' + element.Origin + '</td>'; // Origin
                tablerow += '<td>' + element.Destination + '</td>'; //Destination
                tablerow += '<td>' + element.Size + 'ft</td>'; //Size
                tablerow += '<td>' + element.Status + '</td>'; //Status
                tablerow += '<td>$' + element.Amount + '</td>'; //Amount
                tablerow += '</tr>';

                $('#tblCarrierBookingReports tbody').append(tablerow);
            });
        }
        else {
            var tablerow = "";
            tablerow += '<tr>';
            tablerow += '<td align="center" colspan="8">No Booking Record(s) Found.</td>';
            tablerow += '</tr>';

            $('#tblCarrierBookingReports tbody').append(tablerow);
        }


    },

    //Draw the Chart
    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.
    drawChart: function () {

        ////Fetch the Data For the Customer Ratings                  
        $.ajax({
            url: CarrierDashboard.variables.srcGetCustomerRatings + "?CarrierId=" + $(CarrierDashboard.controls.hdnCarrierId).val(),
            type: "get",
            success: function (response) {
                if (response.Success == true) {

                    // Create the data table.
                    var ratingData = new google.visualization.DataTable();
                    ratingData.addColumn('string', 'Topping');
                    ratingData.addColumn('number', 'Slices');
                    ratingData.addRows([
                      ['Excellent', response.data.CountOfFive],
                      ['Very Good', response.data.CountOfFour],
                      ['Good', response.data.CountOfThree],
                      ['Bad', response.data.CountOfTwo],
                      ['Very Bad', response.data.CountOfOne]
                    ]);

                    // Set chart options
                    var ratingOptions = {
                        'title': 'Customer Rating',
                        'backgroundColor': 'transparent',
                        'pieHole': 0.3,
                        'chartArea': { 'width': '100%', 'height': '100%' },
                        'legend': { 'position': 'labeled' }
                    };

                    // Instantiate and draw RATING chart, passing in some options.
                    var ratingChart = new google.visualization.PieChart(document.getElementById('customer-rating-chart'));
                    ratingChart.draw(ratingData, ratingOptions);
                }
                else {
                    common.alertTortooga("Pending Bookings Were Not Refreshed");
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }
        });



        //var utilizationData = new google.visualization.DataTable();
        //utilizationData.addColumn('string', 'Topping');
        //utilizationData.addColumn('number', 'Slices');
        //utilizationData.addRows([
        //  ['', 3],
        //  ['', 1],
        //]);

        //// Set chart options
        //var utilizationOptions = {
        //    'legend': 'none',
        //    'backgroundColor': 'transparent',
        //    'pieHole': 0.3,
        //};

        //// Instantiate and draw UTILIZATION chart, passing in some options.
        //var utilizationChart = new google.visualization.PieChart(document.getElementById('utilization-rate-chart'));
        //utilizationChart.draw(utilizationData, utilizationOptions);



        //var data = google.visualization.arrayToDataTable([
        //  ['Task', 'Hours per Day'],
        //  ['Excellent', 11],
        //  ['Very Good', 2],
        //  ['Good', 2],
        //  ['Bad', 2],
        //  ['Very Bad', 7]
        //]);

        //var options = {          
        //    chartArea: { 'width': '100%', 'height': '100%' },
        //};

        //var chart = new google.visualization.PieChart(document.getElementById('customer-rating-chart'));

        //chart.draw(data, options);
    },

    FillPanelDropDown: function () {

        var select = $(CarrierDashboard.controls.ddlPanelHandling);
        select.empty().trigger("change");
        var myOptions = '';
        select.append($('<option/>', {
            value: "",
            selected: true,
            text: "Select To Display"
        }));

        $.each(CarrierDashboard.variables.PanelArray, function (index, itemData) {
            select.append($('<option/>', {
                value: itemData.value,
                text: itemData.text
            }));
        });
    }

}

$(document).ready(function () {

    ///Fetch the Carrier Dashboard Data
    CarrierDashboard.getDashBoardData();

    CarrierDashboard.FillPanelDropDown();

    $(".remove-btn").click(function (element) {

        switch ($(this).parents(".panel").attr('id')) {

            case "divPendingBookings":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Pending Bookings",
                    value: "divPendingBookings"
                });
                break;
            case "divAccountSummary":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Account Summary",
                    value: "divAccountSummary"
                });
                //$('#divPendingBookings').parents("div").removeClass('col-md-7');
                //$('#divPendingBookings').parents("div").addClass('col-md-12');
                break;
            case "divCustomerRatings":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Customer Ratings",
                    value: "divCustomerRatings"
                });
                break;
            case "divBookingReports":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Booking Reports",
                    value: "divBookingReports"
                });
                break;
            case "divNotificationPanel":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Notification Panel",
                    value: "divNotificationPanel"
                });
                break;
            case "divInbox":
                CarrierDashboard.variables.PanelArray.push({
                    text: "Inbox",
                    value: "divInbox"
                });
                break;
        }

        CarrierDashboard.FillPanelDropDown();
        $(this).parents(".panel").hide();
    })

    // Load the Visualization API and the corechart package.
    google.charts.load('current', { 'packages': ['corechart'] });

    // Set a callback to run when the Google Visualization API is loaded.
    google.charts.setOnLoadCallback(CarrierDashboard.drawChart);

    //Change Event for the DropDown of Panel
    $(CarrierDashboard.controls.ddlPanelHandling).change(function () {

        if ($(this).val() != "" && $(this).val() != null && $(this).val() != undefined) {
            var divVal = $(this).val();

            $('#' + divVal).show();

            //Remove from Array
            CarrierDashboard.variables.PanelArray = $.grep(CarrierDashboard.variables.PanelArray, function (e) {
                return e.value != divVal;
            });

            //Fill the DropDown Again
            CarrierDashboard.FillPanelDropDown();
        }

    });

});