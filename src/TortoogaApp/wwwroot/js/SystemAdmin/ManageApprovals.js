var ManageApprovalList = {
    variables: {
        oTable: null,
        srcListApprovals: '/SystemAdmin/GetAllApprovalsList/',
        srcApproveRequest: '/SystemAdmin/ApproveRegistrationRequest/',
        srcRejectRequest: '/SystemAdmin/RejectRegistrationRequest/',
        oTableActionColumnIndx: 10
    },
    controls: {
        tblApprovalList: '#tblAppovalList',
    },
    initDatatable_Approvals: function () {
        ManageApprovalList.variables.oTable = $(ManageApprovalList.controls.tblApprovalList).dataTableTortooga({
            "sAjaxSource": ManageApprovalList.variables.srcListApprovals,
            "aaSorting": [[8, "desc"]],// default sorting
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
             },
             {
                 "aTargets": [7],
                 "visible": false,
             },
             {
                 "aTargets": [8],
             },
             {
                 "aTargets": [9],

             },
            {
                "aTargets": [10],

            },
            {
                "aTargets": [11],
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
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $('td:eq(' + ManageApprovalList.variables.oTableActionColumnIndx + ')', nRow).html(ManageApprovalList.getActionItems(aData[11], aData[7]));
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
    reload: function () {
        ManageApprovalList.variables.oTable.dataTable().fnClearTable(0);
        ManageApprovalList.variables.oTable.dataTable().fnStandingRedraw();
    },
    getActionItems: function (regId, status) {
        var result = "";
        if (status == 'Pending') {
            result = '<a onclick=\"ManageApprovalList.Approve(\'' + regId + '\')"><span class="btn btn-xs btn-success">Approve</span></a>&nbsp;';
            result += '<a onclick=\"ManageApprovalList.Reject(\'' + regId + '\')"><span class="btn btn-xs btn-danger" style="width:67px">Reject</span></a>';
            //result += "<input type=\"button\" class=\"btn btn-danger\" value=\"Reject\" title='Reject Request'  onclick=\"ManageApprovalList.Approve(\'" + regId + "\')\" />";
        } else {
            result = status;
        }
        return result;
    },
    Approve: function (regId) {
        bootbox.confirm("Are you sure you want to approve registration request", function (result) {
            if (result == true) {
                debugger;
                ManageApprovalList.ApproveRequest(regId);
            }
        });
    },
    Reject: function (regId) {
        bootbox.confirm("Are you sure you want to reject registration request", function (result) {
            if (result == true) {

                ManageApprovalList.RejectRequest(regId);
            }
        });
    },
    ApproveRequest: function (id) {
        $.ajax({
            type: "GET",
            url: ManageApprovalList.variables.srcApproveRequest + "?regId=" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.Success == true) {
                    debugger;
                    common.alertTortooga("Registration Approved", null);
                    ManageApprovalList.reload();
                }
            }
        });
    },
    RejectRequest: function (id) {
        $.ajax({
            type: "GET",
            url: ManageApprovalList.variables.srcRejectRequest + "?regId=" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.Success == true) {
                    debugger;
                    common.alertTortooga("Registration Rejected", null);
                    ManageApprovalList.reload();
                }
            }
        });
    },
}

$(document).ready(function () {

    //Initiating the Datatable
    ManageApprovalList.initDatatable_Approvals();
    
    $('#tblAppovalList_length').hide();

});
