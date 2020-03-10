$(document).ready(function () {
    obtieneInfoPendiente();
});

var direccion = $("#txtDireccion");
var barrio = $("#txtBarrio");
var telefono = $("#txtTelf");
var celular = $("#txtCelular");
var correoPersonal = $("#txtCorreo");

$("#btnCancelar").hide();
$("#btnEnviar").hide();
$("#btnEditar").show();

function editarForm() {
    direccion.prop("disabled", false);
    barrio.prop("disabled", false);
    telefono.prop("disabled", false);
    celular.prop("disabled", false);
    correoPersonal.prop("disabled", false);
    $("#btnCancelar").show();
    $("#btnEnviar").show();
    $("#btnEditar").hide();
    return false;
}

function cancelar() {
   
    var r = confirm("¿Está seguro de cancelar la edición del formulario?");
    if (r == true) {

        direccion.prop("disabled", true);
        barrio.prop("disabled", true);
        telefono.prop("disabled", true);
        celular.prop("disabled", true);
        correoPersonal.prop("disabled", true);


        direccion.val(direccion.data("direccion"));
        barrio.val(barrio.data("barrio"));
        telefono.val(telefono.data("telefono"));
        celular.val(celular.data("celular"));
        correoPersonal.val(correoPersonal.data("correo"));


        $("#btnCancelar").hide();
        $("#btnEnviar").hide();
        $("#btnEditar").show();
    }
   
    return false;
}

function enviarDatos() {
   

    var r = confirm("Se enviarán sus datos a una revisión y aprobación, se actualizará la información una vez sea aprobada. ¿Está seguro de enviar a actualizar la información?");
    if (r == true) {

        var dataEnvia = {
            'username': $("#usernameLogin").val(),
            'cedula': $("#txtCedula").val(),
            'direccion': direccion.val(),
            'barrio': barrio.val(),
            'telefono': telefono.val(),
            'celular': celular.val(),
            'correoPersonal': correoPersonal.val()
        };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/EnviaSolicitudCambioDeDatos",
            type: "POST",
            data: JSON.stringify(dataEnvia),
            success: function (resultado) {
                alert(resultado.Descripcion);
                if (resultado.Codigo == "1") {
                    window.location.reload(false);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.statusText, false);
            }
        });
    }

}
function anular(idSolicitud) {
    console.log(idSolicitud);
    var r = confirm("¿Está seguro de anular esta solicitud?");
    if (r == true) {
       
        $.ajax({
            dataType: "json",
            url: "../App/ActualizaEstadoSolicitud",
            type: "POST",
            data: {
                id: idSolicitud,
                estado: "I",
                observacion: "Anulada por el usuario",
                username: $("#usernameLogin").val(),
                tipo: "datos"
            },
            success: function (resultado) {
                if (resultado.Codigo == "1") {
                    alert("Solicitud anulada con éxito");
                    window.location.reload(false);
                } else {
                    alert(resultado.Descripcion);
                }
            },
            error: function (resultado) {
                MensajeError(resultado.statusText, false);
            }
        });
    }
      

    
}

function obtieneInfoPendiente() {
    var idSolicitudPendiente = $("#solicitudPendiente").val();
    if (idSolicitudPendiente > 0) {
        $.ajax({
            dataType: "json",
            url: "../App/InfoSolicitudDatos/" + idSolicitudPendiente,
            type: "GET",
            success: function (resultado) {

                $("#txtCodigoSolicitud").val(resultado.id);

          

                $(".inputsEdita").removeClass('datoCambia');
                $(".inputsEdita").attr('title','');

                if (resultado.cambia_direccion) {
                    direccion.addClass('datoCambia');
                    direccion.attr('title', 'Se cambiará por "' + resultado.direccion + '"');
                } 
                if (resultado.cambia_barrio) {
                    barrio.addClass('datoCambia');
                    barrio.attr('title', 'Se cambiará por "' + resultado.barrio + '"');
                } 
                if (resultado.cambia_telefono) {
                    telefono.addClass('datoCambia');
                    telefono.attr('title', 'Se cambiará por "' + resultado.telefono + '"');
                } 
                if (resultado.cambia_celular) {
                    celular.addClass('datoCambia');
                    celular.attr('title', 'Se cambiará por "' + resultado.celular + '"');
                } 
                if (resultado.cambia_correo) {
                    correoPersonal.addClass('datoCambia');
                    correoPersonal.attr('title', 'Se cambiará por "' + resultado.correo + '"');
                } 
            },
            error: function (resultado) {
                MensajeError(resultado.statusText, false);
            }
        });
    }
  
}

