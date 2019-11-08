
//modal
function ModalSuccess(url) {


    var dialog = bootbox.dialog({
        
        message: '<p class="text-center mb-0 text-success">Дані успішно збережено</p>',
        closeButton: false,
        size: "small",
        buttons: {
            ok: {
                callback: function () {
                    window.location.href = url;
                }
            }
        }
    });

    //dialog.init(function () {
    //    setTimeout(function () {
    //        dialog.find('.bootbox-body').html('');
    //        window.location.href = url;
    //    }, 3000);
    //});

    

};

//get from html
function GetAllValueReserve() {
    var createReserveViewModel = {
        'EventName': $("#EventName").val(),
        'EventName': $("#EventName").val(),
        'Description': $("#Description").val(),
        'ReservedDate': $("#ReservedDate").val(),
        'ReservedTimeFrom': $("#ReservedTimeFrom").val(),
        'ReservedTimeTo': $("#ReservedTimeTo").val(),
        'RoomId': $("#RoomId").val(),
        'UsersEmails': $("#UsersEmails").val(),
        'IsPrivate': $("#IsPrivate").is(":checked")
    };
    //$("#checkbox").is(":checked");

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
        $('#save').hide();
        return false;
    } else {
        //alert("валідація для клієнта успішно пройдена");
        $('#messageWait').html("Ви можете зберегти дані");
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
        $('#submit').addClass('disabled');
        $('#save').val('Збереження...');
        $('#messageWait').show();
        $('#messageWait').html("Збереження...");
        $('#messageWait').show();

        $('#messageAlert').addClass('hidden');
        var dialog = bootbox.dialog({
            //title: '<p class="text-center mb-0">Повідомлення</p>',
            message: '<p class="text-center mb-0 text-success">Збереження...</p>',
            closeButton: false,
            size: "small"
        });
        //dialog.init(function () {
        //    setTimeout(function () {
        //        dialog.find('.bootbox-body').html('<p class="text-center mb-0 text-success">Зберігаю...</p>');
        //    }, 2000);
        //});
        $.ajax({
            type: "POST",
            url: '../Reserveds/SaveReserve',
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
                $('#submit').removeClass('disabled');
                $('#submit').show();
            }
        });
    }
    
};

