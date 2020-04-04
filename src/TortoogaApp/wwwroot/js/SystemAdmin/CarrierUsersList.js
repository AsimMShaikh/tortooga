var CarrierUsersList = {
    variables: {
        oTable: null,
        srcListApprovals: '/SystemAdmin/GetAllCarrierUsers/',
        srcEditCarrierUser: '/SystemAdmin/EditCarrierUser/',
        srcDeleteCarrierUser: '/SystemAdmin/DeleteCarrierUser/',
        srcListCarriers: '/SystemAdmin/ListCarriers',
        srcDisableUser: '/SystemAdmin/DisableUser',
        srcEnableUser: '/SystemAdmin/EnableUser',
        oTableActionColumnIndx: 6,
        showDisabled : "true"
    },
    controls: {
        tblCarrierList: '#tblcarrierUserslst',
        ddlCarriers: '#ddlCarriers',
        ddlCountry: '#ddlCountry',
        txtCarrierUsersSearch: '#txtCarrierUsersSearch',
        ddlCarrierUsersPageSizeSelection: '#ddlCarrierUsers_PageSizeSelection',
        chkDisabled: '#chkDisabled'       
    },
    initDatatable_Approvals: function () {
        CarrierUsersList.variables.oTable = $(CarrierUsersList.controls.tblCarrierList).dataTableTortooga({
            "sAjaxSource": CarrierUsersList.variables.srcListApprovals,
            "aaSorting": [[0, "desc"]],// default sorting
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
                  "visible": false,
              },
            {
                "aTargets": [7],
            }],
            "oLanguage": {
                "sEmptyTable": "No User(s) found",
                "sLengthMenu": "Page Size: _MENU_",
                "oPaginate": {
                    "sFirst": "First",
                    "sLast": "Last",
                    "sNext": "Next",
                    "sPrevious": "Previous"
                }
            },
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {                
                $('td:eq(' + CarrierUsersList.variables.oTableActionColumnIndx + ')', nRow).html(CarrierUsersList.getActionItems(aData[6], aData[7]));
                if (aData[6] == 'True') {
                    CarrierUsersList.StrikeThroghRow(nRow);
                }
                return nRow;
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
    //Reloading the Datatable
    reload_CarrierUsersTable: function () {
       
        CarrierUsersList.variables.oTable.dataTable().fnClearTable(0);
        CarrierUsersList.variables.oTable.dataTable().fnStandingRedraw();
    },
    Reset: function () {
     
        $(CarrierUsersList.controls.txtCarrierUsersSearch).val('');
        $(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val('10')
        //$("#hdnGeneralPageSize").val('10')
        //$(UserList.controls.ddlPageSizeSelection).select2();
        $("div").removeData(CarrierUsersList.variables.srchKey);
        CarrierUsersList.reload_CarrierUsersTable();
    },
    StrikeThroghRow: function (nRow) {       
        $(nRow).addClass('strikeout');        
    },
    getActionItems: function (disabled,userId) {      
        var result = "";
        if (disabled == "False") {
            result = '<a title="Edit User" class="btnEdit btn btn-xs btn-primary" href="/SystemAdmin/AddEditCarrierUser?guid=' + userId + '"><i class="fa fa-link"></i></a>'
            result += '<a title="Disable User" class="btn btn-xs btn-danger" onclick="CarrierUsersList.disableUserConfirm(\'' + userId + '\');"><i class="fa fa-remove"></i></a>';
        }
        else {
            result = '<a title="Edit User" class="btnEdit btn btn-xs btn-primary" href="/SystemAdmin/AddEditCarrierUser?guid=' + userId + '"><i class="fa fa-link"></i></a>'
            result += '<a title="Enable User" class="btn btn-xs btn-danger" onclick="CarrierUsersList.enableUserConfirm(\'' + userId + '\');"><i class="fa fa-check-circle"></i></a>';
        }
        //result += "<span class=\"btn btn-xs btn-danger\" title='Delete Document Entity Group'  onclick=\"CarrierUsersList.deleteUserConfirm(\'" + userId + "\')\"><i class=\"fa fa-trash-o\"></i></span>";
       
        return result;
    },
    disableUserConfirm: function (userId) {
        bootbox.confirm("Are you sure you want to disable this user.", function (result) {
            if (result == true) {
               
                CarrierUsersList.Disable(userId);
            }
        });
    },
    enableUserConfirm: function (userId) {
        bootbox.confirm("Are you sure you want to enable this user.", function (result) {
            if (result == true) {

                CarrierUsersList.Enable(userId);
            }
        });
    },    
    editUser: function (userId) {
       
        $.ajax({
            type: "GET",
            url: CarrierUsersList.variables.srcEditCarrierUser + "?userId=" + userId,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.Success == true) {
                  
                    common.alertTortooga("Registration Approved", null);
                   
                    CarrierUsersList.reload();
                }
            }
        });
         
    },
    Disable: function (id) {
       
        $.ajax({
            type: "POST",
            url: CarrierUsersList.variables.srcDisableUser + "/" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {                    
                if (response.result == "true") {
                    common.alertTortooga(response.message, null);
                    CarrierUsersList.reload_CarrierUsersTable();
                }
                else {
                    common.alertTortooga(response.message);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    
    },
    Enable: function (id) {
       
        $.ajax({
            type: "POST",
            url: CarrierUsersList.variables.srcEnableUser + "/" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.result == "true") {
                    common.alertTortooga(response.message, null);
                    CarrierUsersList.reload_CarrierUsersTable();
                }
                else {
                    common.alertTortooga(response.message, null);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });

    },
    FillCarriersDDL: function () {
        $.ajax({
            url: CarrierUsersList.variables.srcListCarriers,
            type: "get",
            success: function (res) {
                if (res == '') {
                }
                else {
                    $(CarrierUsersList.controls.ddlCarriers).html('')
                    var markup = "";
                    markup = "<option value = '0' selected>" + "--Select--" + "</option>";
                    if (res.Data != undefined) {
                        for (var i = 0; i < res.Data.length; i++) {
                            markup += "<option value='" + res.Data[i].Value + "'>" + res.Data[i].Text + "</option>";
                        }
                    }

                    $(CarrierUsersList.controls.ddlCarriers).html(markup);
                }
            }
        });
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    CarrierUsersList.initDatatable_Approvals();
    $('#tblcarrierUserslst_length').hide();
    
    //Page Size DropDown Change Event
    $(document).off('change', CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection);
    $(document).on('change', CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection, function (event) {
        $("#hdnGeneralPageSize").val($(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val())
        $("div").removeData(CarrierUsersList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierUsersList.controls.txtCarrierUsersSearch).val() == '' ? '' : $(CarrierUsersList.controls.txtCarrierUsersSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
            { name: 'CarrierGuid', value: encodeURIComponent($(CarrierUsersList.controls.ddlCarriers).val()) },
            { name: 'Country', value: encodeURIComponent($(CarrierUsersList.controls.ddlCountry).val()) },
            { name: 'showDisabled', value: encodeURIComponent($(CarrierUsersList.variables.showDisabled).val()) }
           ]
       );
        CarrierUsersList.reload_CarrierUsersTable();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    $(document).off('change', CarrierUsersList.controls.ddlCarriers);
    $(document).on('change', CarrierUsersList.controls.ddlCarriers, function (event) {
        $("#hdnGeneralPageSize").val($(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val())
        $("div").removeData(CarrierUsersList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierUsersList.controls.txtCarrierUsersSearch).val() == '' ? '' : $(CarrierUsersList.controls.txtCarrierUsersSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
            { name: 'CarrierGuid', value: encodeURIComponent($(CarrierUsersList.controls.ddlCarriers).val()) },
            { name: 'Country', value: encodeURIComponent($(CarrierUsersList.controls.ddlCountry).val()) },
            { name: 'showDisabled', value: encodeURIComponent($(CarrierUsersList.variables.showDisabled).val()) }
           ]
       );
       
        CarrierUsersList.reload_CarrierUsersTable();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });



    $(document).off('change', CarrierUsersList.controls.ddlCountry);
    $(document).on('change', CarrierUsersList.controls.ddlCountry, function (event) {
        $("#hdnGeneralPageSize").val($(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val())
        $("div").removeData(CarrierUsersList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierUsersList.controls.txtCarrierUsersSearch).val() == '' ? '' : $(CarrierUsersList.controls.txtCarrierUsersSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
            { name: 'CarrierGuid', value: encodeURIComponent($(CarrierUsersList.controls.ddlCarriers).val()) },
            { name: 'Country', value: encodeURIComponent($(CarrierUsersList.controls.ddlCountry).val()) },
            { name: 'showDisabled', value: encodeURIComponent($(CarrierUsersList.variables.showDisabled).val()) }
           ]
       );
        
        CarrierUsersList.reload_CarrierUsersTable();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });

    $(CarrierUsersList.controls.chkDisabled).click(function () {
        debugger;
        if (!$(this).is(':checked')) {
            CarrierUsersList.variables.showDisabled = "false";
        } else {
            CarrierUsersList.variables.showDisabled = "true";
        }
        $("#hdnGeneralPageSize").val($(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val())
        $("div").removeData(CarrierUsersList.variables.srchKey);
        $("div").data("srchParams",
           [{ name: 'iDisplayLength', value: $(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val() },
            { name: 'srchTxt', value: encodeURIComponent($(CarrierUsersList.controls.txtCarrierUsersSearch).val() == '' ? '' : $(CarrierUsersList.controls.txtCarrierUsersSearch).val()) },
            { name: 'srchBy', value: 'ALL' },
            { name: 'CarrierGuid', value: encodeURIComponent($(CarrierUsersList.controls.ddlCarriers).val()) },
            { name: 'Country', value: encodeURIComponent($(CarrierUsersList.controls.ddlCountry).val()) },
            { name: 'showDisabled', value: CarrierUsersList.variables.showDisabled }
           ]
       );

        CarrierUsersList.reload_CarrierUsersTable();
        $("div").data("srchParams", [{ name: 'iDisplayLength', value: $("#hdnGeneralPageSize").val() }]
      );
    });


    $(CarrierUsersList.controls.txtCarrierUsersSearch).keypress(function (e) {
        if (e.keyCode == 13) {
           
            if ($(this).val().trim() != '') {
                $("div").removeData(CarrierUsersList.variables.srchKey);
                $("div").data("srchParams",
                   [{ name: 'srchTxt', value: encodeURIComponent($(CarrierUsersList.controls.txtCarrierUsersSearch).val() == '' ? '' : $(CarrierUsersList.controls.txtCarrierUsersSearch).val()) },
                    { name: 'srchBy', value: 'ALL' },
                    { name: 'iDisplayLength', value: $(CarrierUsersList.controls.ddlCarrierUsersPageSizeSelection).val() },
                    { name: 'CarrierGuid', value: encodeURIComponent($(CarrierUsersList.controls.ddlCarriers).val()) },
                    { name: 'Country', value: encodeURIComponent($(CarrierUsersList.controls.ddlCountry).val()) },
                    { name: 'showDisabled', value: encodeURIComponent($(CarrierUsersList.variables.showDisabled).val()) }
                   ]);
                CarrierUsersList.reload_CarrierUsersTable();
            }
            else {
                CarrierUsersList.Reset();
            }
        }
    });

});
