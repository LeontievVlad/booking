

function AUTEValidation() {
    //alert("Begin");
    var UsersEmails = $('#UsersEmails').val();

    alert(UsersEmails);

    if (UsersEmails == "") {
        
        return false;

    }

    return true;
};


function AUTESuccess() {
    alert("AUTESuccess");
};

function AUTEFailure() {
    alert("AUTEFailure");
};

