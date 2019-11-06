
//modal
function ModalSuccess(url) {

    var dialog = bootbox.dialog({

        message: '<p class="text-center mb-0 text-success">Дані успішно збережено</p>',
        closeButton: false,
        buttons: {
            ok: {
                callback: function () {
                    window.location.href = url;
                }
            }
        }
    });


};
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
        'IsPrivate': $("#IsPrivate").is(":checked")
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

        $('#messageWait').html("Ви можете зберегти дані");

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
        $('#messageAlert').addClass('hidden');
        var dialog = bootbox.dialog({
            title: '<p class="text-center mb-0">Повідомлення</p>',
            message: '<p class="text-center mb-0 text-success">Зберігаю...</p>',
            closeButton: false
        });
        dialog.init(function () {
            setTimeout(function () {
                dialog.find('.bootbox-body').html('<p class="text-center mb-0 text-success">Зберігаю...</p>');
            }, 2000);
        });
        $.ajax({
            type: "POST",
            url: '/Reserveds/SaveEdit',
            data: JSON.stringify(dataToSave),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data, status) {

                $('#messageWait').html("Дані успішно збережено");
                $('#messageWait').show();

                $('#save').removeClass('disabled');
                $('#save').val('Підтвердити');

                var url = "/Reserveds/Details/" + data;

                dialog.modal('hide');
                ModalSuccess(url);




            },
            error: function () {
                dialog.modal('hide');
                $('#save').removeClass('disabled');
                $('#save').val('Підтвердити');
                $('#messageWarning').html("Помилка при збереженні");
                $('#messageWarning').show();
                $('#messageWait').hide();
                
                $('#save').hide();
                $('#submit').show();
            }
        });
    }

};

