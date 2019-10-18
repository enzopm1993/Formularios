$(document).keypress(function (e) {
    if (e.keyCode == 13) {
        $('#btnIngresar').click();
    }
});

$('#logoutli').hide();
$("#btnIngresar").on("click", function () {

    if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
        //$("#ModalErrorLogin2").modal("show");
        MensajeAdvertencia("  Error, Ingrese los campos requeridos");
        return;
    }
    $("#btnCargando").prop("hidden",false);
    $("#btnIngresar").prop("hidden", true);
    $.ajax({
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: "../Login/LogIn",
        data: JSON.stringify({ usuario: $("#txtUsuario").val().trim(), password: $("#txtPassword").val() }),
        success: function (data) {          
            if (data == 1) {
                $('#menu').show();
                $('#logoutli').show();

                var returnUrl = "";

                if (returnUrl == "") {
                    window.location.href = "../Home/Home";

                }
                else {
                    window.location.href = returnUrl;
                }

            }
            else if (data == 0) {
                //                $("#ModalErrorLogin").modal("show");
                MensajeAdvertencia(" Error, usuario o contraseña incorrectos");
            } else {
                MensajeError(data, false);
            }

            $("#btnCargando").prop("hidden", true);
            $("#btnIngresar").prop("hidden", false);
        }
    });
});