﻿

$(document).ready(function () {
    $("#submit").click(function () {

        //$('#messageShow').hide();

        var EventName = $("#EventName").val();
        
        var ReservedTimeFrom = $("#ReservedTimeFrom").val();
        var ReservedTimeTo = $("#ReservedTimeTo").val();

        var fail = "";

        if (EventName.length < 3) fail = "Ім'я не менше 3-х символів <br/>";
        if (ReservedTimeFrom > ReservedTimeTo) fail += "Час початку доцільно ввести більшим за час кінця <br/>";
       


        if (fail != "") {
            $('#messageShow').html(fail);
            $('#messageShow').show();
            return false;
        } else {
            alert("good");
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
    });
});