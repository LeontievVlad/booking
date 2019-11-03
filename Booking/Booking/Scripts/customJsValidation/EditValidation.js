
//get from html
function GetAllValueForEdit() {
    var editReserveViewModel = {
        'ReservedId': $("#ReservedId").val(),
        'EventName': $("#EventName").val(),
        'Description': $("#Description").val(),
        'ReservedDate': $("#ReservedDate").val(),
        'ReservedTimeFrom': $("#ReservedTimeFrom").val(),
        'ReservedTimeTo': $("#ReservedTimeTo").val(),
        'RoomId': $("#RoomId").val(),
        'UsersEmails': $("#UsersEmails").val(),
        'IsPrivate': $("#IsPrivate").val()
    };
    return editReserveViewModel;
};

//check is valid on client
function CheckIsValid() {

    $('#messageWarning').hide();
    //alert("валідація для клієнта");


    var editReserveViewModel = GetAllValueForEdit();
    var fail = "";

    if (editReserveViewModel.EventName.length < 3) fail = "Ім'я не менше 3-х символів <br/>";
    if (editReserveViewModel.ReservedTimeFrom >= editReserveViewModel.ReservedTimeTo)
        fail += "Тривалість повинна бути більша за 0 <br/>";

    if (fail != "") {
        $('#messageWarning').html(fail);
        $('#messageWarning').show();
        return false;
    } else {
        //alert("валідація для клієнта успішно пройдена");

        ValidationServer(editReserveViewModel);

        return true;
    }

};

//check is valid on server
function ValidationServer(editReserveViewModel) {
    $('messageWarning').hide();

    $.ajax({
        type: "POST",
        url: '/Reserveds/Edit',
        data: JSON.stringify(editReserveViewModel),
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
    var dataToSave = GetAllValueForEdit();
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
            url: '/Reserveds/SaveEdit',
            data: JSON.stringify(dataToSave),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, status) {
                setTimeout(function () {
                $('#messageWait').html(data);
                $('#messageWait').show();
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
    $('#messageWait').hide();
    $('#save').hide();
    $('#submit').show();
};

