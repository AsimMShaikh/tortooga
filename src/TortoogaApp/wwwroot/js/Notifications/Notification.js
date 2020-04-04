var Notifications = {

    variables: {
        oTable: null,
        srcListNotifications: '/Notifications/GetNotificationsList/',
        srcUpdateNotificationToRead: "/Dashboard/UpdateNotificationToRead",
    },
    controls: {
        tblNotifications: '#tblNotifications',
        txtNotificationSearch: '#txtNotificationSearch',
        ddlNotifications_PageSizeSelection: '#ddlNotifications_PageSizeSelection'
    },

    initDatatable: function () {
        Notifications.variables.oTable = $(Notifications.controls.tblNotifications).dataTableTortooga({
            "sAjaxSource": Notifications.variables.srcListNotifications,
            "aaSorting": [[1, "desc"]],// default sorting
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
                 "className": "text-center",
                 "mRender": function (data, type, full) {
                     if (full[3] == "True")
                         return '<i class="fa fa-check-circle"></i>'
                     else
                         return '';
                 },
             },
             {
                 "aTargets": [4],
             },
             {
                 "aTargets": [5],
                 "className": "text-center",
                 "mRender": function (data, type, full) {
                     return Notifications.renderClickable(data, type, full);
                 },
                 "bSortable": false
             }],

            "oLanguage": {
                "sEmptyTable": "No Notification(s) available",
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
                return common.pagingText(iStart, iEnd, iTotal, "Records", oSettings._iDisplayLength);
            }
        });
    },

    //Rendering the Action Buttons on the Grid Last Column
    renderClickable: function (data, type, full) {        
        var finallist = '';
        if (full[3] == "False")
            finallist = '<a title="Mark As Read" class="btnEdit btn btn-xs btn-primary" href="javascript:void(0);" onclick="Notifications.ChangeNotificationToRead(\'' + full[5] + '\',\'' + full[6] + '\')"><i class="fa fa-book"></i></a>';
        return finallist;
    },

    //Changing the Notification Status to Read
    ChangeNotificationToRead: function (URL, NotificationId) {     
        if (NotificationId != '' && NotificationId != undefined && NotificationId != null) {
            $.ajax({
                type: "GET",
                url: Notifications.variables.srcUpdateNotificationToRead + "?NotificationId=" + NotificationId,
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

    //Reloading the Datatable
    reload: function () {
        Notifications.variables.oTable.dataTable().fnClearTable(0);
        Notifications.variables.oTable.dataTable().fnStandingRedraw();
    },
    //Resetting the Grid When the TextBox is Empty
    Reset: function () {
        $(Notifications.controls.txtNotificationSearch).val('');
        $(Notifications.controls.ddlNotifications_PageSizeSelection).val('10')
        $("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(Notifications.variables.srchKey);
        Notifications.reload();
    },

}

$(document).ready(function () {

    //Initiating the Datatable
    Notifications.initDatatable();
    $('#tblNotifications_length').hide();

    //Page Size DropDown Change Event
    $(document).off('change', Notifications.controls.ddlNotifications_PageSizeSelection);
    $(document).on('change', Notifications.controls.ddlNotifications_PageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(Notifications.controls.ddlNotifications_PageSizeSelection).val())
        $("div").removeData(Notifications.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(Notifications.controls.txtNotificationSearch).val() == '' ? '' : $(Notifications.controls.txtNotificationSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
           ]
       );
        Notifications.reload();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    //Search Via the Search TextBox
    $(Notifications.controls.txtNotificationSearch).keypress(function (e) {
        if (e.keyCode == 13) {
            if ($(this).val().trim() != '') {
                $("div").removeData(Notifications.variables.srchKey);
                $("div").data("srchParams",
                   [{ name: 'srchTxt', value: encodeURIComponent($(Notifications.controls.txtNotificationSearch).val() == '' ? '' : $(Notifications.controls.txtNotificationSearch).val()) },
                    { name: 'srchBy', value: 'ALL' },
                    { name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
                   ]);
                Notifications.reload();
            }
            else {
                Notifications.Reset();
            }
        }
    });

});