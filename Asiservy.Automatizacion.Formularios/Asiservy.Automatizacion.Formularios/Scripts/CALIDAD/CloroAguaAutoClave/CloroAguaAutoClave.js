$(document).ready(function () {
    ConsultarControl();
    $("#txtCloro").mask("9?.99");
});

function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../CloroAguaAutoClave/ValidaEstadoReporte",
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
        url: "../CloroAguaAutoclave/CloroAguaAutoclavePartial",
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
    console.log(model);
    $("#txtFecha").prop("disabled", true);
    $("#txtIdCloroAguaAutoclave").val(model.IdCloroAguaAutoclave);
    $("#txtHora").val(moment(model.Hora).format("YYYY-MM-DDTHH:mm"));
    $("#txtObservacion").val(model.Observacion);
    $("#txtParada").val(model.Parada);
    $("#txtProducto").val(model.Producto);
    $("#txtTemperatura").val(model.Temperatura);
    $("#txtCloro").val(model.Cloro);
    $("#selectAutoclave").val(model.Autoclave);
    $("#selectConserva").val(model.TipoConserva);
    $("#btnEliminar").prop("hidden", false);
}

function NuevoControl() {
    $("#txtFecha").prop("disabled", false);
    $("#txtIdCloroAguaAutoclave").val('0');
    $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));
    $("#txtObservacion").val('');
    $("#txtParada").val('');
    $("#txtProducto").val('');
    $("#txtTemperatura").val('');
    $("#txtCloro").val('');
    $("#selectAutoclave").prop("selectedIndex", 0);
    $("#selectConserva").prop("selectedIndex", 0);
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

    if ($("#txtParada").val() == "") {
        $("#txtParada").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada").css('borderColor', '#ced4da');
    }

    if ($("#selectAutoclave").val() == "") {
        $("#selectAutoclave").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectAutoclave").css('borderColor', '#ced4da');
    }

    if ($("#txtProducto").val() == "") {
        $("#txtProducto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtProducto").css('borderColor', '#ced4da');
    }
    if ($("#txtTemperatura").val() == "") {
        $("#txtTemperatura").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTemperatura").css('borderColor', '#ced4da');
    }

    if ($("#txtCloro").val() == "") {
        $("#txtCloro").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCloro").css('borderColor', '#ced4da');
    }

    if ($("#selectConserva").val() == "") {
        $("#selectConserva").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectConserva").css('borderColor', '#ced4da');
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
        url: "../CloroAguaAutoclave/CloroAguaAutoclave",
        type: "POST",
        data: {
            IdCloroAguaAutoclave: $("#txtIdCloroAguaAutoclave").val(),
            Fecha: $("#txtFecha").val(),
            Hora: $("#txtHora").val(),
            Parada: $("#txtParada").val(),
            Autoclave: $("#selectAutoclave").val(),
            TipoConserva: $("#selectConserva").val(),
            Producto: $("#txtProducto").val(),
            Temperatura: $("#txtTemperatura").val(),
            Cloro: $("#txtCloro").val(),
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
        url: "../CloroAguaAutoclave/EliminarCloroAguaAutoclave",
        type: "POST",
        data: {
            IdCloroAguaAutoclave: $("#txtIdCloroAguaAutoclave").val(),
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
    //  $("#txtEliminarDetalle").val($("#txtIdCloroAguaAutoclave").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    if ($("#txtIdCloroAguaAutoclave").val() > 0) {
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
