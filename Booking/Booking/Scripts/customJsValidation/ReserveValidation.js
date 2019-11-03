﻿
//get from html
function GetAllValueReserve() {
    var createReserveViewModel = {
        'EventName': $("#EventName").val(),
        'Description': $("#Description").val(),
        'ReservedDate': $("#ReservedDate").val(),
        'ReservedTimeFrom': $("#ReservedTimeFrom").val(),
        'ReservedTimeTo': $("#ReservedTimeTo").val(),
        'RoomId': $("#RoomId").val(),
        'UsersEmails': $("#UsersEmails").val(),
        'IsPrivate': $("#IsPrivate").val()
    };
    return createReserveViewModel;
};

//check is valid on client
function CheckIsValid() {

    $('#messageWarning').hide();
    //alert("валідація для клієнта");


    var createReserveViewModel = GetAllValueReserve();
    var fail = "";

    if (createReserveViewModel.EventName.length < 3) fail = "Ім'я не менше 3-х символів <br/>";
    if (createReserveViewModel.ReservedTimeFrom >= createReserveViewModel.ReservedTimeTo)
        fail += "Тривалість повинна бути більша за 0 <br/>";

    if (fail != "") {
        $('#messageWarning').html(fail);
        $('#messageWarning').show();
        return false;
    } else {
        //alert("валідація для клієнта успішно пройдена");
       
        ValidationServer(createReserveViewModel);

        return true;
    }

};

//check is valid on server
function ValidationServer(createReserveViewModel) {
    $('messageWarning').hide();

    $.ajax({
        type: "POST",
        url: '../Reserveds/Reserve',
        data: JSON.stringify(createReserveViewModel),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data, status) {


            $('#messageAlert').html(data);
            $('#messageAlert').show();

            //$('messageWarning').show();
            //$('#submit').hide();
            $('#save').show();
            //alert(data + " (server)");


        },
        error: function () {
            $('#messageWarning').html("Помилка з сервера");
            $('#messageWarning').show();
            $('#save').hide();
            $('#submit').show();

            //alert("Помилка з сервера");

        }
    });

};

//try to save
function SaveToDb() {
    //alert("спроба зберегти");
    var dataToSave = GetAllValueReserve();
    if (CheckIsValid()) {
        //save to db
        //alert("Зберігаю...");
        $('#save').addClass('disabled');
        $('#save').val('Зберігаю...');
        $('#messageWait').show();
        $('#messageWait').html("Зберігаю...");
        $('#messageWait').show();
        $.ajax({
            type: "POST",
            url: '../Reserveds/SaveReserve',
            data: JSON.stringify(dataToSave),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, status) {
                $('#messageWait').html(data);
                $('#messageWait').show();
                setTimeout(function () {
                    $('#save').removeClass('disabled');
                }, 4000);
                $('#save').val('Підтвердити');
                
                
                
                
                //alert(data);
                
                var url = "/Reserveds/MyEvents";
                window.location.href = url;

            },
            error: function () {
                $('#save').removeClass('disabled');
                $('#save').val('Підтвердити');
                $('#messageWarning').html("помилка при збереженні");
                $('#messageWarning').show();
                $('#save').hide();
                $('#submit').show();
            }
        });
    }
    //alert("щось не так");
    //$('#messageWait').hide();
    //$('#save').hide();
    //$('#submit').show();
};

