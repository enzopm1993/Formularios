$(document).ready(function () {
    ConsultarControl();

    $('#txtTemperatura').inputmask({
        'alias': 'integer',
        'groupSeparator': '',
        'autoGroup': true,
        'digitsOptional': true,
        'max': '100.00',
        'min': '-100.00'
    });

});

function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../TemperaturaInternaRefrigeradora/ValidaEstadoReporte",
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
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    ValidaEstadoReporte($("#txtFecha").val());
    $.ajax({
        url: "../TemperaturaInternaRefrigeradora/TemperaturaInternaRefrigeradoraPartial",
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
                $("#chartCabecera2").html('<div class="text-center"><h4 class="text-warning">' + Mensajes.SinRegistros + '</h4></div>');
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = false;
                config.opcionesDT.ordering = false;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            console.log(resultado);
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function SeleccionarControl(model) {
   // console.log(model);
    $("#txtFecha").prop("disabled", true);
    $("#txtIdTemperaturaInternaRefrigeradora").val(model.IdTemperaturaInternaRefrigeradora);
    $("#txtHora").val(model.Hora);
    $("#txtObservacion").val(model.Observacion);
    $("#txtTemperatura").val(model.Temperatura);
    $("#btnEliminar").prop("hidden", false);
}

function NuevoControl() {
    $("#txtFecha").prop("disabled", false);
    $("#txtIdTemperaturaInternaRefrigeradora").val('0');
    $("#txtHora").val(moment().format("HH:mm"));
    $("#txtObservacion").val('');
    $("#txtTemperatura").val('');
    $("#btnEliminar").prop("hidden", true);

}

function Validar() {
    var valida = true;

    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

   
    if ($("#txtTemperatura").val() == "") {
        $("#txtTemperatura").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTemperatura").css('borderColor', '#ced4da');
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
    MostrarModalCargando();
    $.ajax({
        url: "../TemperaturaInternaRefrigeradora/TemperaturaInternaRefrigeradora",
        type: "POST",
        data: {
            IdTemperaturaInternaRefrigeradora: $("#txtIdTemperaturaInternaRefrigeradora").val(),
            Fecha: $("#txtFecha").val(),
            Hora: $("#txtHora").val(),
            Temperatura: $("#txtTemperatura").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CerrarModalCargando();

            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else
            if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                NuevoControl();
                ConsultarControl();
                MensajeCorrecto(resultado);

            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            CerrarModalCargando();
        }
    });

    //alert("generado");
}



function InactivarControl() {
    $.ajax({
        url: "../TemperaturaInternaRefrigeradora/EliminarTemperaturaInternaRefrigeradora",
        type: "POST",
        data: {
            IdTemperaturaInternaRefrigeradora: $("#txtIdTemperaturaInternaRefrigeradora").val(),
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else
            if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                ConsultarControl();
                NuevoControl();
                MensajeCorrecto(resultado);
            }
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControl() {
    //  $("#txtEliminarDetalle").val($("#txtIdTemperaturaInternaRefrigeradora").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    if ($("#txtIdTemperaturaInternaRefrigeradora").val() > 0) {
        $("#modalEliminarControlDetalle").modal('show');
    } else {
        MensajeAdvertencia("Seleccione un control.");
    }
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
