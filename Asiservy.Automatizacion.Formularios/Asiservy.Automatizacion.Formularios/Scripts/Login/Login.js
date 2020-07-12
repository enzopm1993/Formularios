var BD = [];
$(document).ready(function (){
    ConsultaDB();
});

$(document).keypress(function (e) {
    if (e.keyCode == 13) {
        $('#btnIngresar').click();
    }
});


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


function ConsultaDB()
{
    //$("#selectBD").empty();
    //$("#selectBD").append("<option value='' >-- Cambiar de Ambiente--</option>");
    $.ajax({
         type: "GET",
        url: "../Login/ConsultarBD",        
        success: function (resultado) {
            BD = resultado;
            //console.log(resultado);
            if (!$.isEmptyObject(resultado)) {
                $("#EnlaceBase").prop("href",resultado[0].Descripcion);
                $("#EnlaceBase").text("Ir ambiente de "+resultado[0].Mensaje);

                //$.each(resultado, function (create, row) {
                //    $("#selectBD").append("<option value='" + row.Descripcion + "'>" + row.Mensaje + "</option>")
                //});

            }
        }
    });
}

function CambiarAmbienteDB(value) {
    if (value != "") {
        window.location.replace(value);
    }
}