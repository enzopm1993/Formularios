$(document).keypress(function (e) {
    if (e.keyCode == 13) {
        $('#btnIngresar').click();
    }
});

function CambioClave() {
    LimpiarModalCambioClave();
    $("#ModalCambioClave").modal("show");
}

function Validar() {
    var valida = true;
    if ($("#txtClaveActual").val() == '') {
        $("#txtClaveActual").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveActual").css("border-color", "#ced4da");
    }
    if ($("#txtClaveNueva").val() == '') {
        $("#txtClaveNueva").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveNueva").css("border-color", "#ced4da");
    }
    if ($("#txtClaveNuevaConfirmar").val() == '') {
        $("#txtClaveNuevaConfirmar").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveNuevaConfirmar").css("border-color", "#ced4da");
    }
    return valida;
}

function LimpiarModalCambioClave() {
    $("#txtClaveActual").css("border-color", "#ced4da");
    $("#txtClaveNueva").css("border-color", "#ced4da");
    $("#txtClaveNuevaConfirmar").css("border-color", "#ced4da");
    $("#txtClaveActual").val('');
    $("#txtClaveNueva").val('');
    $("#txtClaveNuevaConfirmar").val('');
}

function CambiarClave() {
    if (!Validar()) {
        return;
    }
}


function Nuevo() {
    $("#txtUsuario").val('');
    $("#txtPassword").val('');
}

$('#logoutli').hide();
$("#btnIngresar").on("click", function () {
    //var URLdomain = window.location.host;
    //var URLprotocol = window.location.protocol;
    //console.log(URLdomain);
    //console.log(URLprotocol);
    if ($("#txtUsuario").val().trim() == "" || $("#txtPassword").val().trim() == "") {
        //$("#ModalErrorLogin2").modal("show");
        MensajeAdvertencia("  Error, Ingrese los campos requeridos");
        return;
    }
    $("#btnCargando").prop("hidden",false);
    $("#btnIngresar").prop("hidden", true);
    $("#btnNuevo").prop("hidden", true);
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
                $("#btnCargando").prop("hidden", true);
                $("#btnIngresar").prop("hidden", false);
                $("#btnNuevo").prop("hidden", false);

            } else {
                MensajeError(data, false);
                $("#btnCargando").prop("hidden", true);
                $("#btnIngresar").prop("hidden", false);
                $("#btnNuevo").prop("hidden", false);

            }

            
        }
    });
});