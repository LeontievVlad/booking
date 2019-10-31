
$(function () {
        $("#UsersEmails").chosen();
});

$(function () {

        $("#submit").click(function () {
            $('#messageShow').hide();
            var createReserveViewModel = {
                'EventName': $("#EventName").val(),
                'ReservedDate': $("#ReservedDate").val(),
                'ReservedTimeFrom': $("#ReservedTimeFrom").val(),
                'ReservedTimeTo': $("#ReservedTimeTo").val(),
                'UsersEmails': $("#UsersEmails").val(),
                'RoomId': $("#RoomId").val()
            };
            var fail = "";
            if (createReserveViewModel.EventName.length < 3) fail = "Ім'я не менше 3-х символів <br/>";
            if (createReserveViewModel.ReservedTimeFrom >= createReserveViewModel.ReservedTimeTo)
                fail += "Увага! Дата закінчення передує даті початку <br/>";

            if (fail != "") {
                $('#messageShow').html(fail);
                $('#messageShow').show();

            } else {

                //alert("обробка запиту");

                $.ajax({
                    type: "POST",
                    url: '@Url.Action("Reserve","Reserveds")',
                    data: JSON.stringify(createReserveViewModel),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data, status) {
                        $('#messageShow').hide();
                        if (data == "success") {
                            alert(data);
                            var url = "/Reserveds/Index";
                            window.location.href = url;
                        } else {
                            alert(data);
                        }

                    },
                    error: function () {
                        $('#messageShow').hide();
                        alert("Error while inserting data");
                    }
                });
            }
        });
});
    