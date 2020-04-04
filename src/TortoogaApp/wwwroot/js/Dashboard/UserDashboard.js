var UserDashboard = {

    variables: {
        srcGetUserDashboardData: $('#hdnAppBaseURL').val() + 'Dashboard/GetUserDashboardData',
        PanelArray: [],
        srcUpdateNotificationToRead: "/Dashboard/UpdateNotificationToRead",
    },

    controls: {
        hdnUserId: '#hdnUserId',
        ddlPanelHandling: '#ddlPanelHandling',
        divNotifications_User: '#divNotifications_User'
    },

    getDashBoardData: function () {

        $.ajax({
            type: "GET",
            cache: false,
            url: UserDashboard.variables.srcGetUserDashboardData + "?UserId=" + $(UserDashboard.controls.hdnUserId).val(),
            success: function (response, textStatus, xhr) {
                if (response.Success) {
                    UserDashboard.BindShipmentStatus(response.data.ShipmentStatus);
                    UserDashboard.BindNotificationsPanel(response.data.Notifications);
                    UserDashboard.BindBookingStatus(response.data.BookingStatus);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                common.alertTortooga(errorThrown);
            }
        });
    },

    ///For Binding the Booking Status
    BindBookingStatus: function (ListBookingStatus) {
        $("#tblBookingStatus > tbody").html("");

        if (ListBookingStatus.length > 0) {

            //Fetching and Pushing the Rows from the Grid 
            $.each(ListBookingStatus, function (index, element) {

                var listingURL = $('#hdnAppBaseURL').val() + 'Listing/' + element.ListingID + '/View';
                var tablerow = "";
                tablerow += '<tr>';
                tablerow += '<td><a href="' + listingURL + '" target="_blank">' + element.BookingReferencenNo + '</a></td>'; // Listing
                tablerow += '<td>' + element.Origin + '</td>'; // Origin
                tablerow += '<td>' + element.Destination + '</td>'; // Destination 
                tablerow += '<td>' + element.Status + '</td>'; // Status               
                tablerow += '</tr>';

                $('#tblBookingStatus tbody').append(tablerow);
            });

        }
        else {
            var tablerow = "";
            tablerow += '<tr>';
            tablerow += '<td align="center" colspan="4">No Booking(s) Found.</td>';
            tablerow += '</tr>';

            $('#tblBookingStatus tbody').append(tablerow);
        }
    },

    ///For Rendering the Shipment Status Grid
    BindShipmentStatus: function (ShipmentStatus) {

        //By accessing and/or using this code snippet, you agree to AccuWeather’s terms and conditions (in English) which can be found at http://www.accuweather.com/en/free-weather-widgets/terms and AccuWeather’s Privacy Statement (in English) which can be found at http://www.accuweather.com/en/privacy.
        $("#tblShipmentStatus > tbody").html("");

        if (ShipmentStatus.length > 0) {

            var countdiv = 1;

            //Fetching and Pushing the Rows from the Grid 
            $.each(ShipmentStatus, function (index, element) {
                var tablerow = "";
                tablerow += '<tr>';
                tablerow += '<td>';
                //tablerow += '<div class="row" style="border:black 1px solid">';
                //tablerow += '<div class="row" style="background-color:#94d191">';
                tablerow += '<div class="row">';
                tablerow += '<div class="col-md-5 font-16">';
                tablerow += '<div class="clearfix mb5">';
                tablerow += '<div class="clearfix mb5">Booking Ref : <strong>' + element.BookingReferenceNo + '</strong></div>';
                tablerow += '</div>';
                tablerow += '<div class="clearfix mb5"><i class="fa fa-fw fa-plane"></i>' + element.Origin + ' <i class="fa fa-fw fa-plane fa-rotate-90"></i>' + element.Destination + '</div>'
                //tablerow += '<div class="clearfix mb5"><i class="fa fa-fw fa-plane fa-rotate-90"></i>' + element.Destination +' </div>';                
                //tablerow += '</div>';
                //tablerow += '<div class="row">';
                //tablerow += '<div class="col-md-5">';
                tablerow += '<div class="clearfix mb5">Current Loc : <strong>' + element.CurrentLocation + '</strong></div>';
                tablerow += '<div class="clearfix mb5">Lat: <strong>' + element.Latitude + '</strong></div>';
                tablerow += '<div class="clearfix mb5">Long: <strong>' + element.Longitude + '</strong></div>';
                tablerow += '<div class="clearfix mb5">Estd. Arrival Dt : <strong>' + element.EstimatedArrivalDate.split('T')[0] + ' </strong></div>';
                if (element.Status == "delayed")
                    tablerow += '<div class="clearfix mb5"><span class="label label-danger">' + element.Status + '</span></div>';
                else
                    tablerow += '<div class="clearfix mb5"><span class="label label-success">' + element.Status + '</span></div>';
                tablerow += '</div>';
                tablerow += '<div class="col-md-7">';                
                tablerow += '<div id="showmap_' + countdiv + '" style="width:400px;height:200px"></div>';
                //tablerow += '<iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5171.444368107048!2d144.92840672027782!3d-37.84573537154353!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x903b3b9ca41d99b4!2sSpirit+Station+Pier!5e0!3m2!1sen!2sau!4v1479905065094" width="400" height="200" frameborder="0" style="border:0" allowfullscreen></iframe>';
                tablerow += '</div>';
                //tablerow += '<div class="col-md-3">';
                //tablerow += '<a href="http://www.accuweather.com/en/au/melbourne/26216/weather-forecast/26216" class="aw-widget-legal">';
                //tablerow += '</a><div id="awcc1479907568386" class="aw-widget-current" data-locationkey="26216" data-unit="f" data-language="en-us" data-useip="false" data-uid="awcc1479907568386"></div>';
                //tablerow += '<script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>';
                //tablerow += '</div>';
                //tablerow += '</div>';
                //tablerow += '</div>';
                tablerow += '</td>';
                tablerow += '</tr>';

                $('#tblShipmentStatus tbody').append(tablerow);
                UserDashboard.ShowLocationOnMap(element.LatDecimal, element.LongDecimal, countdiv);
                countdiv = countdiv + 1;
            });

        }
        else {
            var tablerow = "";
            tablerow += '<tr>';
            tablerow += '<td align="center">No Shipment Status Available.</td>';
            tablerow += '</tr>';

            $('#tblShipmentStatus tbody').append(tablerow);
        }
    },

    //Binding the Carrier Notifications Panel
    BindNotificationsPanel: function (Notifications) {

        var divNotifications = $(UserDashboard.controls.divNotifications_User);

        if (Notifications.Orders != undefined && Notifications.Orders.length > 0) {
            var divstring = "";
            for (var i = 0 ; i < Notifications.Orders.length; i++) {

                divstring = "";

                switch (Notifications.Orders[i]) {

                    case "RatingReceived":
                        var object = Notifications.RatingReceived.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-star-half-full"> Rating Received</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentArrival":
                        var object = Notifications.ShipmentArrivals.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Arrival</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentShipped":
                        var object = Notifications.ShipmentShipped.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Shipped</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentDelayed":
                        var object = Notifications.ShipmentDelayed.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Delayed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "ShipmentCancelled":
                        var object = Notifications.ShipmentCancelled.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Delayed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "NewMessage":
                        var object = Notifications.ShipmentCancelled.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Shipment Delayed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "BookingConfirmed":
                        var object = Notifications.BookingConfirmed.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Booking Confirmed</i>';
                        divstring += '<span class="pull-right text-muted small"><em>' + object[0].NotificationTime + '</em></span>';
                        divstring += '</a>';
                        break;

                    case "BookingRejected":
                        var object = Notifications.BookingRejected.filter(function (element) { return element.OrderNo == parseInt(i + 1); })
                        if (!object[0].IsRead)
                            divstring += '<a href="javascript:void(0);" style="cursor:pointer" onclick="UserDashboard.ChangeNotificationToRead(\'' + object[0].URL + '\',\'' + object[0].NotificationId + '\')" class="list-group-item unread-notification">';
                        else
                            divstring += '<a href="' + object[0].URL + '" class="list-group-item">';
                        divstring += '<i class="fa fa-anchor"> Booking Rejected</i>';
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
                url: UserDashboard.variables.srcUpdateNotificationToRead + "?NotificationId=" + NotificationId,
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

    FillPanelDropDown: function () {

        var select = $(UserDashboard.controls.ddlPanelHandling);
        select.empty().trigger("change");
        var myOptions = '';
        select.append($('<option/>', {
            value: "",
            selected: true,
            text: "Select To Display"
        }));

        $.each(UserDashboard.variables.PanelArray, function (index, itemData) {
            select.append($('<option/>', {
                value: itemData.value,
                text: itemData.text
            }));
        });
    },

    ShowLocationOnMap: function (latitude, longitude, countdiv) {        
        // Google has tweaked their interface somewhat - this tells the api to use that new UI
        google.maps.visualRefresh = true;
        var LatLng = new google.maps.LatLng(latitude, longitude);
        var mapOptions = {
            center: LatLng,
            zoom: 13,
            mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
        };
        var map = new google.maps.Map(document.getElementById("showmap_" + countdiv + ""), mapOptions);
        var marker = new google.maps.Marker({
            position: LatLng,
            map: map,
            title: "<div style = 'height:200px;width:200px'><b>Your location:</b><br />Latitude: " + latitude + "<br />Longitude: " + longitude
        });

        google.maps.event.addListener(marker, "click", function (e) {
            var infoWindow = new google.maps.InfoWindow();
            infoWindow.setContent(marker.title);
            infoWindow.open(map, marker);
        });
    }
}

$(document).ready(function () {

    ///Fetch the User Dashboard Data
    UserDashboard.getDashBoardData();

    UserDashboard.FillPanelDropDown();

    $(".remove-btn").click(function (element) {

        switch ($(this).parents(".panel").attr('id')) {

            case "divBookingStatus":
                UserDashboard.variables.PanelArray.push({
                    text: "Booking Status",
                    value: "divBookingStatus"
                });
                break;
            case "divShipmentStatus":
                UserDashboard.variables.PanelArray.push({
                    text: "Shipment Status",
                    value: "divShipmentStatus"
                });
                break;
            case "divNotificationPanel":
                UserDashboard.variables.PanelArray.push({
                    text: "Notification Panel",
                    value: "divNotificationPanel"
                });
                break;
            case "divInbox":
                UserDashboard.variables.PanelArray.push({
                    text: "Inbox",
                    value: "divInbox"
                });
                break;
        }

        UserDashboard.FillPanelDropDown();
        $(this).parents(".panel").hide();
    })

    //Change Event for the DropDown of Panel
    $(UserDashboard.controls.ddlPanelHandling).change(function () {

        if ($(this).val() != "" && $(this).val() != null && $(this).val() != undefined) {
            var divVal = $(this).val();

            $('#' + divVal).show();

            //Remove from Array
            UserDashboard.variables.PanelArray = $.grep(UserDashboard.variables.PanelArray, function (e) {
                return e.value != divVal;
            });

            //Fill the DropDown Again
            UserDashboard.FillPanelDropDown();
        }

    });

    //var map;
    //map = new google.maps.Map(document.getElementById('map'), {
    //    center: { lat: -34.397, lng: 150.644 },
    //    zoom: 8
    //});


});