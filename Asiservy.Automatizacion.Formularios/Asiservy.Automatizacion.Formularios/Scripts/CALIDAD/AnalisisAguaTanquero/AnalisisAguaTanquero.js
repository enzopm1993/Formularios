$(document).ready(function () {
    ConsultarControl();
    $("#txtPh").mask("9?.99");
});

function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../AnalisisAguaTanquero/ValidaEstadoReporte",
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
        url: "../AnalisisAguaTanquero/AnalisisAguaTanqueroPartial",
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
    $("#txtIdAnalisisAguaTanquero").val(model.IdAnalisisAguaTanquero);
    $("#txtHora").val(model.Hora);
    $("#txtObservacion").val(model.Observacion);
    $("#txtPlaca").val(model.Placa);
    $("#txtStd").val(model.Std);
    $("#txtDureza").val(model.Dureza);
    $("#txtPh").val(model.Ph);
    $("#txtDestino").val(model.Destino);
    $("#chkOlor").prop("checked", model.Olor);
    $("#chkColor").prop("checked", model.Color);
    $("#chkSabor").prop("checked", model.Sabor);
    $("#btnEliminar").prop("hidden", false);
}

function NuevoControl() {
    $("#txtFecha").prop("disabled", false);
    $("#txtIdAnalisisAguaTanquero").val('0');
    $("#txtHora").val(moment().format("HH:mm"));
    $("#txtObservacion").val('');
    $("#txtPlaca").val('');
    $("#txtStd").val('');
    $("#txtDureza").val('');
    $("#txtPh").val('');
    $("#txtDestino").val('');
    $("#chkOlor").prop("checked", false);
    $("#chkColor").prop("checked", false);
    $("#chkSabor").prop("checked", false);

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


    if ($("#txtPlaca").val() == "") {
        $("#txtPlaca").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPlaca").css('borderColor', '#ced4da');
    }

    if ($("#txtStd").val() == "") {
        $("#txtStd").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtStd").css('borderColor', '#ced4da');
    }

    if ($("#txtDureza").val() == "") {
        $("#txtDureza").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtDureza").css('borderColor', '#ced4da');
    }

    if ($("#txtPh").val() == "") {
        $("#txtPh").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPh").css('borderColor', '#ced4da');
    }

    if ($("#txtDestino").val() == "") {
        $("#txtDestino").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtDestino").css('borderColor', '#ced4da');
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
        url: "../AnalisisAguaTanquero/AnalisisAguaTanquero",
        type: "POST",
        data: {
            IdAnalisisAguaTanquero: $("#txtIdAnalisisAguaTanquero").val(),
            Fecha: $("#txtFecha").val(),
            Hora: $("#txtHora").val(),
            Placa: $("#txtPlaca").val(),
            Std: $("#txtStd").val(),
            Dureza: $("#txtDureza").val(),
            Ph: $("#txtPh").val(),
            Olor: $("#chkOlor").prop("checked"), 
            Color: $("#chkColor").prop("checked"), 
            Sabor: $("#chkSabor").prop("checked"), 
            Destino: $("#txtDestino").val(), 
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
            }
            if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                NuevoControl();
                ConsultarControl();
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
        url: "../AnalisisAguaTanquero/EliminarAnalisisAguaTanquero",
        type: "POST",
        data: {
            IdAnalisisAguaTanquero: $("#txtIdAnalisisAguaTanquero").val(),
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            if (resultado == "1") {
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
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControl() {
    //  $("#txtEliminarDetalle").val($("#txtIdAnalisisAguaTanquero").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    if ($("#txtIdAnalisisAguaTanquero").val() > 0) {
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
