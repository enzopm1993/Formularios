$(document).ready(function () {
    ConsultarControl();
    $("#txtCloro").mask("9?.99");
    $("#txtCloro2").mask("9?.99");
});

function ValidaEstadoReporte(Fecha) {
    $.ajax({
        url: "../CloroAguaAutoClave/ValidaEstadoReporte",
        type: "GET",
        data: {
            Fecha: Fecha,
            Turno: $("#selectTurno").val()
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
    if ($("#txtFecha").val() == '' || $("#selectTurno").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    ValidaEstadoReporte($("#txtFecha").val());
    $.ajax({
        url: "../CloroAguaAutoclave/CloroAguaAutoclavePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(), 
            Turno: $("#selectTurno").val()
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

    $("#txtFecha").css('borderColor', '#ced4da');
    $("#txtHora").css('borderColor', '#ced4da');
    $("#txtParada").css('borderColor', '#ced4da');
    $("#txtProducto").css('borderColor', '#ced4da');
    $("#txtTemperatura").css('borderColor', '#ced4da');
    $("#txtCloro").css('borderColor', '#ced4da');
    $("#selectAutoclave").css('borderColor', '#ced4da');
    $("#selectConserva").css('borderColor', '#ced4da');

}

function NuevoControlEdita() {
    $("#txtIdCloroAguaAutoclave").val('0');
    $("#txtHora2").val(moment().format("YYYY-MM-DDTHH:mm"));
    $("#txtObservacion2").val('');
    $("#txtParada2").val('');
    $("#txtProducto2").val('');
    $("#txtTemperatura2").val('');
    $("#txtCloro2").val('');
    $("#selectAutoclave2").prop("selectedIndex", 0);
    $("#selectConserva2").prop("selectedIndex", 0);

    $("#txtHora2").css('borderColor', '#ced4da');
    $("#txtParada2").css('borderColor', '#ced4da');
    $("#txtProducto2").css('borderColor', '#ced4da');
    $("#txtTemperatura2").css('borderColor', '#ced4da');
    $("#txtCloro2").css('borderColor', '#ced4da');
    $("#selectAutoclave2").css('borderColor', '#ced4da');
    $("#selectConserva2").css('borderColor', '#ced4da');
}

function Validar() {
    var valida = true;
  
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
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

    if ($("#txtParada").val() == "" || $("#txtParada").val() > 99999999 || $("#txtParada").val() < -99999999) {
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

    if ($("#txtTemperatura").val() == "" || $("#txtTemperatura").val() > 999999999 || $("#txtTemperatura").val() < -999999999) {
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
            Turno: $("#selectTurno").val(), 
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


function EditarControl(model)
{
    NuevoControlEdita();
    $("#modalEditar").modal("show");
    $("#txtIdCloroAguaAutoclave").val(model.IdCloroAguaAutoclave);
    $("#txtHora2").val(moment(model.Hora).format("YYYY-MM-DDTHH:mm"));
    $("#txtObservacion2").val(model.Observacion);
    $("#txtParada2").val(model.Parada);
    $("#txtProducto2").val(model.Producto);
    $("#txtTemperatura2").val(model.Temperatura);
    $("#txtCloro2").val(model.Cloro);
    $("#selectAutoclave2").val(model.Autoclave);
    $("#selectConserva2").val(model.TipoConserva);
    $("#btnEliminar2").prop("hidden", false);

}

function ValidarEdita() {
    var valida = true;

    if ($("#txtHora2").val() == "") {
        $("#txtHora2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora2").css('borderColor', '#ced4da');
    }

    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#txtParada2").val() == "") {
        $("#txtParada2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada2").css('borderColor', '#ced4da');
    }

    if ($("#txtParada2").val() == "" || $("#txtParada2").val() > 99999999 || $("#txtParada2").val() < -99999999) {
        $("#txtParada2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada2").css('borderColor', '#ced4da');
    }


    if ($("#selectAutoclave2").val() == "") {
        $("#selectAutoclave2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectAutoclave2").css('borderColor', '#ced4da');
    }

   
    if ($("#txtProducto2").val() == "") {
        $("#txtProducto2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtProducto2").css('borderColor', '#ced4da');
    }
    if ($("#txtTemperatura2").val() == "") {
        $("#txtTemperatura2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTemperatura2").css('borderColor', '#ced4da');
    }

    if ($("#txtTemperatura2").val() == "" || $("#txtTemperatura2").val() > 99999999 || $("#txtTemperatura2").val() < -99999999) {
        $("#txtTemperatura2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTemperatura2").css('borderColor', '#ced4da');
    }

    
    if ($("#txtCloro2").val() == "") {
        $("#txtCloro2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCloro2").css('borderColor', '#ced4da');
    }

    if ($("#selectConserva2").val() == "") {
        $("#selectConserva2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectConserva2").css('borderColor', '#ced4da');
    }
    return valida;
}


function GuardarControlEdita() {
    if (!ValidarEdita()) {
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
            Turno: $("#selectTurno").val(),
            Hora: $("#txtHora2").val(),
            Parada: $("#txtParada2").val(),
            Autoclave: $("#selectAutoclave2").val(),
            TipoConserva: $("#selectConserva2").val(),
            Producto: $("#txtProducto2").val(),
            Temperatura: $("#txtTemperatura2").val(),
            Cloro: $("#txtCloro2").val(),
            Observacion: $("#txtObservacion2").val()
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
                NuevoControlEdita();
                ConsultarControl();
            }
            $("#modalEditar").modal("hide");
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
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
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
                NuevoControlEdita();
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

function EliminarControl(model) {
    //  $("#txtEliminarDetalle").val($("#txtIdCloroAguaAutoclave").val());
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#txtIdCloroAguaAutoclave").val(model.IdCloroAguaAutoclave);
        $("#modalEliminarControlDetalle").modal('show');
   
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
