var message = [];

$(function () {
    $("#btnSaveClient").off("click");
    $("#btnSaveClient").on("click", function () {
        message = [];
        var savModel = getModal();
        if (savModel.StoreName.trim().length == 0) {
            message.push("Please enter valid Store Name.");
        }
        if (savModel.StoreURL.trim().length == 0) {
            message.push("Please enter valid Store StoreURL.");
        }
        if (savModel.ContactName.trim().length == 0) {
            message.push("Please enter valid Contact person name.");
        }
        if (savModel.PirmaryContact.trim().length == 0) {
            message.push("Please enter valid Pirmary Contact.");
        }
        if(savModel.EmailId.trim().length == 0) {
            message.push("Please enter valid Email.");
        }
        else if (!validateEmail(savModel.EmailId.trim())) {
            message.push("Please enter valid Email.");
        }
        if (savModel.Password.trim().length == 0) {
            message.push("Please enter valid Password.");
        }
        if (savModel.ConfirmPassword.trim().length == 0) {
            message.push("Please enter valid Confirm Password.");
        }
        if (savModel.Password.trim().length != 0 && savModel.ConfirmPassword.trim().length != 0 && savModel.Password.trim() != savModel.ConfirmPassword.trim()) {
            message.push("Password and Confirm Password dosen't match.");
        }
        if (savModel.PostalCode.trim().length !=6) {
            message.push("Please enter valid postal code.");
        }
        if (message.length == 0) {
            validateModal();
        }
        else {
            raiseError();
        }
    });

    $("#btnReset").off("click");
    $("#btnReset").on("click", function () { });

    $("#btnNavigate").off("click");
    $("#btnNavigate").on("click", function () {
        $('#successModal').modal("hide");
    });

    $('#successModal').on('hidden.bs.modal', function () {
        window.location.href =  'https://alldealz.ca'
    });
});

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

function getModal() {

    var model = {
        Industry: $("#ddnInsudtry").val(),
        StoreName: $("#txtStoreName").val(),
        StoreURL: $("#txtStoreURL").val(),
        ContactName: $("#txtConatctName").val(),
        Positon: $("#txtPosition").val(),
        PirmaryContact: $("#txtPrimaryContact").val(),
        EmailId: $("#txtEmail").val(),
        Password: $("#txtPassword").val(),
        ConfirmPassword: $("#txtConfirmPassword").val(),
        AddressLine1: $("#txtAddressLine1").val(),
        AddressLine2: $("#txtAddressLine2").val(),
        City: $("#txtCity").val(),
        TargetedCities: $("#txtTragetedCity").val(),
        TargetedCommunities: $("#txtTargetedCommunities").val(),
        WelcomeMessage: $("#txtWelcomeMsg").val(),
        TrolleryCount: $("#txtTrollyCount").val(),
        BasketCount: $("#txtBasketCount").val(),
        PostalCode: $("#txtPincode").val(),
        ClientFBUrl: $("#txtFbLink").val(),
        ClientTwitterUrl: $("#txtTwitterLink").val(),
        ClientInstaUrl: $("#txtInstaLink").val()
        
    };

    return model;
}

function getSelectedCatList() {
    //chkCat
    var catList = [];
    $(".chkCat:checked").each(function () {
        catList.push($(this).val());
    });
    return catList;
}

function validateModal() {
    $.ajax({
        url: '/Client/ValidateAddStore',
        data: getModal(),
        contentType: 'application/json',
        success: function (data) {
            message = [];
            if (data.userExists == true) {
                message.push('Email Already exists');
            }
            if (data.storeExists == true) {
                message.push('Store URL exists.');
            }
            if (message.length > 0) {
                raiseError();
            }
            else {
                saveClient();
            }
        },
        error: function (error) {
            alert('Error occured. Please contact Administrator')
        }
    });
    
}

function saveClient() {
    var model = {};
    model.SelectedList = getSelectedCatList();
    model.Model = getModal();
    $("#btnSaveClient").attr("disabled", true);
    $.ajax({
        url: '/Client/AddStore',
        type:'POST',
        data: JSON.stringify(model),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {

            $("#hdnUserId").val(data);
            if (data > 0 || data == false) {
                $("#successModal").modal("show");
            }
            else {
                alert('Error occured. Please contact Administrator');
            }
        },
        error: function (error) {
            alert('Error occured. Please contact Administrator');
        }
    });

}

function uploadImage() {
    message = [];
    if ($("#fileClientBanner")[0].files.length == 0) {
        message.push('Please select valid Banner');
    }
    if ($("#fileClientLogo")[0].files.length == 0) {
        message.push('Please select valid Logo');
    }
    if (message.length == 0) {
        $("#formImage").submit();
    }
    else
        raiseError()
};


function raiseError() {
    
    $("#ulErrorMsg").html("");
    for (var i = 0; i < message.length; i++) {
        $("#ulErrorMsg").append("<li>" + message[i] + "</li>");
    }
    $("#errorModal").modal("show");
    
}