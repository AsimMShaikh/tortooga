var AddEditCarrierUser = {
    variables: {
        oTable: null,
        srcListStates: '/Account/GetAllStatesByCountryCode',
        srcDeleteUser: '/SystemAdmin/DeleteUser',
        oTableActionColumnIndx: 6,
        hdnuserId: '#hdnUserId',
        hdnstatus: '#hdnStatus'
    },
    controls: {
        ddlCountry: '#ddlCountry',
        ddlState: '#ddlState',
        btnDeleteUser: '#btnDeleteUser'
    },
    deleteUserConfirm: function (userId) {
        bootbox.confirm("Are you sure you want to delete this user", function (result) {
            if (result == true) {
                AddEditCarrierUser.DeleteUser(userId);
            }
        });
    },
    DeleteUser: function (id) {
        $.ajax({
            type: "POST",
            url: AddEditCarrierUser.variables.srcDeleteUser + "/" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.result == "true") {                                      

                    common.alertTortooga("User deleted successfully");
                    setTimeout(function () {
                        window.location = response.url;
                    }, 5000);
                }
                else {
                    common.alertTortooga(response.message);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });

    },
    Disable: function (id) {

        $.ajax({
            type: "POST",
            url: AddEditCarrierUser.variables.srcDisableUser + "/" + id,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (response, textStatus, xhr) {
                if (response.result == "true") {
                    common.alertTortooga(response.message, null);
                    $(AddEditCarrierUser.variables.hdnstatus).val(true);
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
                    $(AddEditCarrierUser.variables.hdnstatus).val(false);
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
            url: AddEditCarrierUser.variables.srcList,
            type: "get",
            success: function (res) {
                if (res == '') {
                }
                else {
                   
                    var markup = "";
                    markup = "<option value = '0' selected>" + "--Select--" + "</option>";
                    if (res.Data != undefined) {
                        for (var i = 0; i < res.Data.length; i++) {
                            markup += "<option value='" + res.Data[i].Value + "'>" + res.Data[i].Text + "</option>";
                        }
                    }

                    $(AddEditCarrierUser.controls.ddlCarriers).html(markup);
                }
            }
        });
    },
}

$(document).ready(function () {


    $(document).on('click', AddEditCarrierUser.controls.btnDeleteUser, function (event) {
        debugger;
        AddEditCarrierUser.deleteUserConfirm($(AddEditCarrierUser.variables.hdnuserId).val())
    });
    


    $(document).off('change', AddEditCarrierUser.controls.ddlCountry);
    $(document).on('change', AddEditCarrierUser.controls.ddlCountry, function (event) {
        debugger;
        $.ajax({
            url: AddEditCarrierUser.variables.srcListStates + '?code=' + $(AddEditCarrierUser.controls.ddlCountry).val(),
            type: "get",
            success: function (res) {
                if (res == '') {
                }
                else {
                    $(AddEditCarrierUser.controls.ddlState).html('')
                    var markup = "";
                    markup = "<option value = '0' selected>" + "--Select--" + "</option>";
                    if (res != undefined) {
                        for (var i = 0; i < res.length; i++) {
                            markup += "<option value='" + res[i].Value + "'>" + res[i].Text + "</option>";
                        }
                    }

                    $(AddEditCarrierUser.controls.ddlState).html(markup);
                }
            }
        });
    });


    $('#toggleStatus').change(function () {        

        if ($('#toggleStatus').val() == "on") {
            AddEditCarrierUser.Enable($(AddEditCarrierUser.controls.hdnuserId).val());
        } else {
            AddEditCarrierUser.Disable($(AddEditCarrierUser.controls.hdnuserId).val());
        }
        //$('#toggleStatus').html('Toggle: ' + $(this).prop('checked'))
    })

    var FormDropzone = function () {


        return {

            //main function to initiate the module
            init: function () {

                Dropzone.options.ProfilePic = {

                    //autoProcessQueue: false,
                    uploadMultiple: false,
                    parallelUploads: 1,
                    addRemoveLinks: true,
                    maxFiles: 1,
                    acceptedFiles: '.jpg,.jpeg,.JPEG,.JPG,.png,.PNG',
                    init: function () {
                        this.on("addedfile", function (file) {

                            if (file.size > (1024 * 1024 * 1)) // not more than 1mb
                            {
                                this.removeFile(file); // if you want to remove the file or you can add alert or presentation of a message
                                alert("File size exceeds limit!");
                            }
                            // Create the remove button
                            var removeButton = Dropzone.createElement("<button class='btn btn-sm btn-block'>Remove file</button>");

                            // Capture the Dropzone instance as closure.
                            var _this = this;

                            // Listen to the click event
                            removeButton.addEventListener("click", function (e) {
                                // Make sure the button click doesn't submit the form:
                                e.preventDefault();
                                e.stopPropagation();

                                // Remove the file preview.
                                _this.removeFile(file);
                                // If you want to the delete the file on the server as well,
                                // you can do the AJAX request here.
                            });

                            // Add the button to the file preview element.
                            file.previewElement.appendChild(removeButton);
                        });

                        this.on("maxfilesexceeded", function (file) {
                            this.removeFile(file);
                            alert("No more files please!");
                        });
                    }
                }
            }
        };
    }();
    FormDropzone.init();

});


