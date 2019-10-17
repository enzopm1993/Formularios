$(document).keypress(function (e) {
    if (e.keyCode == 13) {
        $('#btnIngresar').click();
    }
});

$('#logoutli').hide();
$("#btnIngresar").on("click", function () {

    if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
        //$("#ModalErrorLogin2").modal("show");
        MensajeError("  Error, Ingrese los campos requeridos");
        return;
    }

    $.ajax({
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: "@Url.Action("LogIn", "Login")",
        data: JSON.stringify({ usuario: $("#txtUsuario").val().trim(), password: $("#txtPassword").val() }),
        success: function (data) {
            if (data == 1) {

                $('#menu').show();

                $('#logoutli').show();

                var returnUrl = "@Request.QueryString["ReturnUrl"]";

                if (returnUrl == "") {

                    window.location.href = "@Url.Action("Home", "Home")";

                }
                else {
                    window.location.href = returnUrl;
                }

            }
            else if (data == 0) {
                //                $("#ModalErrorLogin").modal("show");
                MensajeError(" Error, usuario o contraseña incorrectos");
            } else {
                MensajeError(data, false);
            }
        }
    });
});