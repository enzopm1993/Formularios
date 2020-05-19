var ListaDatos = [];
var ListaDatosDetalle = [];
ListaDatos.IdCloroCisterna = 0;
ListaDatosDetalle.IdCloroCisternaDetalle = 0;
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    if ($("#txtFecha").val() == '') {
        return;
    } else {
        $.ajax({
            url: "../CloroCisternaDescongelado/ValidarCloroCisternaDescongelado",
            type: "GET",
            data: {
                Fecha: $("#txtFecha").val()
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == "0") {
                    $("#txtEstado").html("");
                    $("#txtFecha").prop("disabled", false);
                    $("#txtObservacion").val('');
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true);
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalGenerarRegistro").prop("hidden", false);
                } else {       
                    if (resultado.EstadoReporte == true) {
                        $("#txtEstado").html("APROBADO");
                        $("#txtEstado").removeClass('badge badge-danger');
                        $("#txtEstado").addClass('badge badge-success');
                        $("#btnModalEditar").prop("hidden", true);
                        $("#btnModalEliminar").prop("hidden", true);                         
                        
                        $("#btnModalGenerar").prop("hidden", true);                        
                    } else {                        
                        $("#txtEstado").html("PENDIENTE");
                        $("#txtEstado").addClass('badge badge-danger');
                        $("#divDetalleControlCloro").prop("hidden", false);                                               
                        $("#btnModalGenerar").prop("hidden", false);
                        $("#btnModalEditar").prop("hidden", false);
                        $("#btnModalEliminar").prop("hidden", false);
                    }
                    $("#txtObservacion").prop("disabled", true);
                    $("#btnModalGenerarRegistro").prop("hidden", true); 
                    $("#txtObservacion").val(resultado.Observaciones);
                    ListaDatos = resultado;
                    CargarControlCloroCisternaDetalle();
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

function GuardarCabecera() {
    $.ajax({
        url: "../CloroCisternaDescongelado/ControlCloroCisternaDescongelado",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),            
            Observaciones: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
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
    $("#modalEliminarControl").modal("show");
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
    $("#txtObservacion").prop("disabled", false)
    $("#btnModalEliminar").prop("hidden", true);
    $("#btnModalGenerarRegistro").prop("hidden", false);
    $("#btnModalEditar").prop("hidden",true);   
}

//DETALLE INGRESO DE HORA CONTROL CLORO CISTERNA
function ModalGenerarHoraControlCloroCisterna() {
    $("#ModalGenerarHoraControlCloroCisterna").modal("show");
    CargarHora();
}

function CargarHora() {
    var fechaactual = new Date();
    var horaactual = moment(fechaactual).format('HH:mm');
    var fecha = $("#txtFecha").val();
    var fechahora = moment(fecha + " " + horaactual).format('YYYY-MM-DDTHH:mm');
    $("#txtHora").val(fechahora); 
}

function limpiarDetalle() {
    $("#txtHora").val('');
    $("#txtPpm").val('');
    $("#txtCisterna").val('');
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
                Ppm_Cloro: $("#txtPpm").val(),
                Cisterna: $("#txtCisterna").val(),
                Observaciones: $("#txtObservacionDetalle").val()
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#ModalGenerarHoraControlCloroCisterna").modal("hide");                
                CargarCabecera();
                CargarControlCloroCisternaDetalle();
                limpiarDetalle();
                ListaDatos = [];
                ListaDatosDetalle = [];
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
            IdCloroCisterna: ListaDatos.IdCloroCisterna,
            Estado: ListaDatos.EstadoReporte
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                $("#divCabecera2").prop("hidden", true);
                
            } else {               
                $("#spinnerCargandoConsumoInsumoDetalle").prop("hidden", true);
                $("#divTableEntregaProductoDetalle").prop("hidden", false);   
                $("#divTableEntregaProductoDetalle").html(resultado);       
                $("#divDetalleControlCloro").prop("hidden", false);        
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
            IdCloroCisternaDetalle: dato.IdCloroCisternaDetalle
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }            
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
    if ($("#txtCisterna").val()>10) {
        MensajeAdvertencia('El número de la CISTERNA no puede ser mayor a 10');
        return false;
    }
    if ($("#txtPpm").val() > 5) {
        MensajeAdvertencia('El PPM CLORO no puede ser mayor a 5');
        return false;
    }
}

function ActulizarControlCloroCisternaDetalle(jdata) {
    var data = jdata;
    ListaDatosDetalle = jdata;
    $("#ModalGenerarHoraControlCloroCisterna").modal('show');
    //var date = moment($("#txtFecha").val() + " " + data.Hora).format("DD-MM-YYYYTHH:mm");
    $("#txtHora").val(moment($("#txtFecha").val() + " " + data.Hora).format("YYYY-MM-DDTHH:mm"));
    $("#txtPpm").val(data.Ppm_Cloro);
    $("#txtCisterna").val(data.Cisterna);
    $("#txtObservacionDetalle").val(data.Observaciones);
}