$(document).ready(function () {
    ConsultarControl();
    $("#txtNaCI1").mask("9?.99");
    $("#txtNaCI2").mask("9?.99");
    $("#txtNaCI3").mask("9?.99");
});


function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../VerificacionPotenciometro/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                $("#lblAprobadoPendiente").html("");

            } else if (resultado == 1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);

            } else {
                $("#lblAprobadoPendiente").removeClass("badge-info").addClass("badge-danger");
                $("#lblAprobadoPendiente").html(Mensajes.Pendiente);
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function ConsultarControl() {
    if ($("#txtFecha").val() == '') {
        return;
    }
    ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
    $("#h4Mensaje").html("" );
    $.ajax({
        url: "../VerificacionPotenciometro/VerificacionPotenciometroPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#txtModelo").prop("disabled", false);
                $("#txtSerie").prop("disabled", false);
                $("#txtNaCI1").prop("disabled", false);
                $("#txtNaCI2").prop("disabled", false);
                $("#txtNaCI3").prop("disabled", false);
                $("#txtObservacion").prop("disabled", false);
                $("#h4Mensaje").html(Mensajes.SinRegistros);
                $("#btnGenerar").prop("hidden", false);
                $("#btnEditar").prop("hidden", true);
                $("#btnEliminar").prop("hidden", true);
                $("#txtModelo").val("");
                $("#txtSerie").val("");
                $("#txtNaCI1").val("");
                $("#txtNaCI2").val("");
                $("#txtNaCI3").val("");
                $("#txtObservacion").val(resultado[0].Observacion);

            } else {
                $("#txtModelo").prop("disabled", true);
                $("#txtSerie").prop("disabled", true);
                $("#txtNaCI1").prop("disabled", true);
                $("#txtNaCI2").prop("disabled", true);
                $("#txtNaCI3").prop("disabled", true);
                $("#txtObservacion").prop("disabled", true);
                $("#btnGenerar").prop("hidden", true);
                $("#btnEditar").prop("hidden", false);
                $("#btnEliminar").prop("hidden", false);
                $("#txtModelo").val(resultado[0].Modelo);
                $("#txtSerie").val(resultado[0].Serie);
                $("#txtNaCI1").val(resultado[0].NaCI1);
                $("#txtNaCI2").val(resultado[0].NaCI2);
                $("#txtNaCI3").val(resultado[0].NaCI3);
                $("#txtObservacion").val(resultado[0].Observacion);

            }
            CerrarModalCargando();
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function EditarControl() {
    $("#txtModelo").prop("disabled", false);
    $("#txtSerie").prop("disabled", false);
    $("#txtNaCI1").prop("disabled", false);
    $("#txtNaCI2").prop("disabled", false);
    $("#txtNaCI3").prop("disabled", false);
    $("#txtObservacion").prop("disabled", false);
    $("#btnGenerar").prop("hidden", false);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", false);
}


function Validar() {
    var valida = true;

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#txtModelo").val() == "") {
        $("#txtModelo").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtModelo").css('borderColor', '#ced4da');
    }
    if ($("#txtSerie").val() == "") {
        $("#txtSerie").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtSerie").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI1").val() == "") {
        $("#txtNaCI1").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI1").css('borderColor', '#ced4da');
    }

    if ($("#txtNaCI2").val() == "") {
        $("#txtNaCI2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI2").css('borderColor', '#ced4da');
    }
    if ($("#txtNaCI3").val() == "") {
        $("#txtNaCI3").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNaCI3").css('borderColor', '#ced4da');
    }
    return valida;
}


function GuardarControl() {
    if (!Validar()) {
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }
    $.ajax({
        url: "../VerificacionPotenciometro/VerificacionPotenciometro",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Modelo: $("#txtModelo").val(),
            Serie: $("#txtSerie").val(),
            NaCI1: $("#txtNaCI1").val(),
            NaCI2: $("#txtNaCI2").val(),
            NaCI3: $("#txtNaCI3").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            }
            else {
                ConsultarControl();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });

    //alert("generado");
}



function InactivarControl() {
    $.ajax({
        url: "../VerificacionPotenciometro/EliminarVerificacionPotenciometro",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                ConsultarControl();
                NuevoControl();
            }
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarControl() {
    //  $("#txtEliminarDetalle").val($("#txtIdVerificacionPotenciometro").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');

}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
