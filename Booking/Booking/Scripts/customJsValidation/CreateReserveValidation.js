

function ReserveValidation() {


    $('#messageShow').hide();

    var EventName = $("#EventName").val();

    var ReservedTimeFrom = $("#ReservedTimeFrom").val();
    var ReservedTimeTo = $("#ReservedTimeTo").val();

    var fail = "";

    if (EventName.length < 3) fail = "Ім'я не менше 3-х символів <br/>";
    if (ReservedTimeFrom >= ReservedTimeTo) fail += "Увага! Дата закінчення передує даті початку <br/>";



    if (fail != "") {
        $('#messageShow').html(fail);
        $('#messageShow').show();
        return false;
    } else {
        return true;
    }
};

function fnSuccess(data) {
    

};


function fnFailure(data) {
    alert(data);
   
    window.location.href = this;
};