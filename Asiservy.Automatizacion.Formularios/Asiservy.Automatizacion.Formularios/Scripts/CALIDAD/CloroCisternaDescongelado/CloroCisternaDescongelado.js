var ListaDatos = [];
var ListaDatosDetalle = [];
var estadoReporte = [];
//ListaDatos.IdCloroCisterna = 0;
//ListaDatosDetalle.IdCloroCisternaDetalle = 0;
$(document).ready(function () {
    CargarCabecera();
    $('#txtPpm').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '9999.99' });
    $('#txtCisterna').inputmask({ 'alias': 'numeric', 'groupSeparator': ',', 'autoGroup': true, 'digits': 0, 'digitsOptional': true, 'max': '999' });
});

function ConsultarEstadoReporte() {
    $.ajax({
        url: "../CloroCisternaDescongelado/ConsultarEstadoReporte",
        type: "GET",
        data: {
            idControlCloro: ListaDatos.IdCloroCisterna
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia('Error al consultar estado del reporte: ConsultarEstadoReporte');
            } else {
                if (resultado.EstadoReporte == true) {
                    estadoReporte = true;
                } else {
                    estadoReporte = false;
                }
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarCabecera() {
     ListaDatos = [];
     ListaDatosDetalle = [];
     estadoReporte = [];
    if ($("#txtFecha").val() == '') {
        return;
    } else {
        $.ajax({
            url: "../CloroCisternaDescongelado/ValidarCloroCisternaDescongelado",
            type: "GET",
            data: {
                fecha: $("#txtFecha").val(),
                turno: document.getElementById('selectTurno').value
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == "0") {
                    $("#txtFecha").prop("disabled", false);
                    $("#txtObservacion").val('');
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);                    
                    $("#btnModalGenerarRegistro").prop("hidden", false); 
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true); 
                    $("#divCabecera2").prop("hidden", false); 
                    CambiarMensajeEstado('nada');
                    ListaDatos = [];
                } else { 
                    $("#divCabecera2").prop("hidden", false); 
                    $("#btnModalEditar").prop("hidden", false);
                    $("#btnModalEliminar").prop("hidden", false);  
                    $("#btnModalGenerarRegistro").prop("hidden", true); 
                    $("#txtObservacion").prop("disabled", true);                    
                    $("#divDetalleControlCloro").prop("hidden", false);
                    $("#btnModalGenerar").prop("hidden", false);   
                    $("#txtObservacion").val(resultado.Observaciones);
                    CambiarMensajeEstado(resultado.EstadoReporte);
                    ListaDatos = resultado;
                    CargarControlCloroCisternaDetalle();
                }
            },
            error: function (resultado) {
                MensajeError(Mensajes.Error, false);
            }
        });
    }
}

function GuardarCabecera() {
    $.ajax({
        url: "../CloroCisternaDescongelado/ControlCloroCisternaDescongelado",
        type: "POST",
        data: {
            IdCloroCisterna: ListaDatos.IdCloroCisterna,
            Fecha: $("#txtFecha").val(),
            Observaciones: $("#txtObservacion").val(),
            Turno: document.getElementById('selectTurno').value
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == 0) {
                MensajeCorrecto('Registro GUARDADO correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Registro ACTUALIZADO correctamente');
            } else if (resultado == 2) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                return;
            } 
            CargarCabecera();
                $("#txtObservacion").prop("disabled", true);
                $("#btnModalGenerar").prop("hidden", true);
                $("#btnModalEditar").prop("hidden", false);
                $("#btnModalEliminar").prop("hidden",false);
            $("#divDetalleProceso").prop("hidden", false);   
            $("#divDetalleControlCloro").prop("hidden", false);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);            
        }
    });
}

function EliminarConfirmar() {   
    ConsultarEstadoReporte();
    setTimeout(function () { 
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            $("#modalEliminarControl").modal("show");
        }
    }, 200);
}

function EliminarCabeceraSi() {
     $.ajax({
        url: "../CloroCisternaDescongelado/EliminarCloroCisternaDescongelado",
        type: "POST",
        data: {
            IdCloroCisterna: ListaDatos.IdCloroCisterna
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdCloroCisterna");
                return;
            } else if (resultado == 2) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                return;
            }
            $("#divTableEntregaProductoDetalle").prop("hidden", true);   
            CargarCabecera();
            MensajeCorrecto("Registro Eliminado con Éxito");            
            $("#txtFecha").prop("disabled", false);
            $("#modalEliminarControl").modal("hide");
            $("#btnModalGenerar").prop("hidden", false);
            $("#btnModalEditar").prop("hidden", true);
            $("#btnModalEliminar").prop("hidden", true);
            $("#divDetalleProceso").prop("hidden", true);
            $("#txtObservacion").prop("disabled", false);
            $("#txtObservacion").val('');
            $("#divDetalleControlCloro").prop("hidden", true);    
            $("#divCabecera2").prop("hidden", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabeceraActivarCotroles() {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            $("#txtObservacion").prop("disabled", false)
            //$("#btnModalEliminar").prop("hidden", true);
            $("#btnModalGenerarRegistro").prop("hidden", false);
            //$("#btnModalEditar").prop("hidden",true);   
        }
    }, 200);
}

//DETALLE INGRESO DE HORA CONTROL CLORO CISTERNA
function ModalGenerarHoraControlCloroCisterna() {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            $("#ModalGenerarHoraControlCloroCisterna").modal("show");
            CargarHora();
            ListaDatosDetalle = [];
        }
    }, 200);
}

function CargarHora() {
    var fechaactual = new Date();
    var horaactual = moment(fechaactual).format('YYYY-MM-DDTHH:mm');
    $("#txtHora").val(horaactual); 
}

function limpiarDetalle() {
    $("#txtHora").val('');
    $("#txtPpm").val('');
    $("#txtCisterna").val(1);
    $("#txtObservacionDetalle").val('');
}

function GuardarControlCloroDetalle() {
    if ($('#txtCisterna').val()==0) {
        MensajeAdvertencia('El número de Cisterna debe ser mayor a 0');
        return;
    }
    if (ControlMayorA()!=false) {
        $.ajax({
            url: "../CloroCisternaDescongelado/ControlCloroCisternaDescongeladoDetalle",
            type: "POST",
            data: {
                IdCloroCisternaCabecera: ListaDatos.IdCloroCisterna,
                IdCloroCisternaDetalle: ListaDatosDetalle.IdCloroCisternaDetalle,
                Hora: $("#txtHora").val(),
                Ppm_Cloro: parseFloat($("#txtPpm").val()),
                Cisterna: $("#txtCisterna").val(),
                Observaciones: $("#txtObservacionDetalle").val()
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                } else if (resultado == 2) {
                    MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                    return;
                }
                $("#ModalGenerarHoraControlCloroCisterna").modal("hide");
                CargarControlCloroCisternaDetalle();
                limpiarDetalle();
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }   
}

function CargarControlCloroCisternaDetalle() {
    $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", false);
    $("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../CloroCisternaDescongelado/ValidarCloroCisternaDescongeladoDetallePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            IdCloroCisterna: ListaDatos.IdCloroCisterna
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                
            } else {               
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                $("#divTableEntregaProductoDetalle").prop("hidden", false);   
                $("#divTableEntregaProductoDetalle").html(resultado);                   
            }            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
        }
    });
}

function EliminarControlCloroCisternaDetalle(dato) {
    $.ajax({
        url: "../CloroCisternaDescongelado/EliminarCloroCisternaDescongeladoDetalle",
        type: "POST",
        data: {
            IdCloroCisternaDetalle: ListaDatosDetalle.IdCloroCisternaDetalle,
            IdCloroCisternaCabecera: ListaDatosDetalle.IdCloroCisternaCabecera
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == 2) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                return;
            }
            $("#modalEliminarDetalle").modal("hide");
            MensajeCorrecto("Registro Eliminado con Éxito");
            CargarControlCloroCisternaDetalle();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function controlDiaMayorMenor() {
    var fecha = moment($("#txtHora").val()).format('YYYY-MM-DD');
    var fechasum = moment($("#txtFecha").val()).add(1, 'day').format('YYYY-MM-DD');
    if (fecha > fechasum) {
        MensajeAdvertencia('Solo se permite una dia mayor a la fecha de CONTROL ' + moment($("#txtFecha").val()).format('DD-MM-YYYY'));
        CargarHora();
    }
    if ($("#txtHora").val() < $("#txtFecha").val()) {
        MensajeAdvertencia('No se puede ingresar una fecha menor a la fecha del CONTROL ' + moment($("#txtFecha").val()).format('DD-MM-YYYY'));
        CargarHora();
    }
}

function ControlMayorA() {
    //if ($("#txtCisterna").val()>10) {
    //    MensajeAdvertencia('El número de la CISTERNA no puede ser mayor a 10');
    //    return false;
    //}
    //if ($("#txtPpm").val() > 5) {
    //    MensajeAdvertencia('El PPM CLORO no puede ser mayor a 5');
    //    return false;
    //}
}

function ActulizarControlCloroCisternaDetalle(jdata) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            var data = jdata;
            ListaDatosDetalle = jdata;
            $("#ModalGenerarHoraControlCloroCisterna").modal('show');
            $("#txtHora").val(moment(data.Hora).format("YYYY-MM-DDTHH:mm"));
            $("#txtPpm").val(data.Ppm_Cloro);
            $("#txtCisterna").val(data.Cisterna);
            $("#txtObservacionDetalle").val(data.Observaciones);
        }
    }, 200);
}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#txtEstado").text("APROBADO");
        $("#txtEstado").removeClass('badge-danger');
        $("#txtEstado").addClass('badge badge-success');
    } else if (estadoReporteParametro == false) {
        $("#txtEstado").text("PENDIENTE");
        $("#txtEstado").removeClass('badge-success');
        $("#txtEstado").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada') {
        $("#txtEstado").text("");
        $("#txtEstado").removeClass('badge-success');
        $("#txtEstado").removeClass('badge badge-danger');
    }
}

function EliminarDetalleConfirmar(jdata) {
    ListaDatosDetalle = [];
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            ListaDatosDetalle = jdata;
            $("#modalEliminarDetalle").modal("show");
        }
    }, 200);
}

function EliminarDetalleNo() {
    $("#modalEliminarDetalle").modal("hide");
}