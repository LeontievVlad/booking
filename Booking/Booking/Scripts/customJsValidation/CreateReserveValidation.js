

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

    //$.ajax({
    //    url: '../Rooms/Reserve.cshtml',
    //    type: 'Post',
    //    cache: false,
    //    data: { EventName, ReservedDate, ReservedTimeFrom, ReservedTimeTo },
    //    dataType: 'html',
    //    success: function (data) {
    //        alert('success');
    //    },
    //    error: function (data) {
    //        alert('error');
    //    }
    //});
};

function fnSuccess() {
    alert("fnSuccess");

    var url = "/Reserveds/Index";
    window.location.href = url;

};


function fnFailure() {
    alert("fnFailure");
   
    window.location.href = this;
};