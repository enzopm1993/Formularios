var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    CargarCabecera(0);
});

function CargarCabecera(opcion) {
    MostrarModalCargando();
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        return;
    } else {
        $.ajax({
            url: "../TemperaturaTermoencogidoSellado/ConsultarTermoencogidoSellado",
            type: "GET",
            data: {
                fecha: $("#txtFecha").val(),
                opcion: op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                if (resultado == "0") {
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true);
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalGenerarRegistro").prop("hidden", false);
                    ListaDatos = resultado;
                } else {
                    $("#divDetalleControlCloro").prop("hidden", false);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalEditar").prop("hidden", false);
                    $("#btnModalEliminar").prop("hidden", false);
                    $("#txtObservacion").prop("disabled", true);
                    $("#btnModalGenerarRegistro").prop("hidden", true);
                    $("#txtObservacion").val(resultado.Observacion);
                    ListaDatos = resultado;
                    CargarDetalle(0);
                }
                setTimeout(function () {
                    CerrarModalCargando();
                }, 500);
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
                CerrarModalCargando();
            }
        });
    }
}

function GuardarCabecera() {
    MostrarModalCargando();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/GuardarModificarTermoencogidoSellado",
        type: "POST",
        data: {
            id: ListaDatos.Id,
            Fecha: moment($("#txtFecha").val()).format("YYYY-MM-DD"),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera(0);
            $("#btnModalGenerar").prop("hidden", true);
            $("#btnModalEditar").prop("hidden", false);
            $("#btnModalEliminar").prop("hidden", false);
            $("#divDetalleProceso").prop("hidden", false);
            $("#divDetalleControlCloro").prop("hidden", false);
            setTimeout(function () {
                CerrarModalCargando();
            }, 500);
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar() {
    $("#modalEliminarControl").modal("show");
}

function EliminarCabeceraSi() {
    MostrarModalCargando();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/EliminarTermoencogidoSellado",
        type: "POST",
        data: {
            id: ListaDatos.Id
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesinfeccionManos");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#divTableEntregaProductoDetalle").prop("hidden", true);
                CargarCabecera(0);
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
                setTimeout(function () {
                    CerrarModalCargando();
                }, 500);
            }
        },
        error: function (resultado) {
            CerrarModalCargando();
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
    $("#btnModalEditar").prop("hidden", true);
}

//DETALLE INGRESO DE LINEAS (SE LISTA EL CLASIFICADOR PARA PODER ARMAR LA CABECERA DE LA TABLA)
function ModalGenerarDetalle() {
    $("#ModalGenerarDetalle").modal("show");
    limpiarDetalle();
}

function limpiarDetalle() {
    $("#txtTemperatura").val('');
    $("#txtObservacionDetalle").val('');
    var date = new Date();
    $("#txtHora").val(moment(date).format("HH:mm"));
    $("#checkSellado").prop('checked', false);
    $("#LabelEstado").text('Estado Registro');   
    $("#txtObservacionDetalle").css('border', '');
    $("#txtHora").css('border', '');
}

function ValidarAntesGuardar() {
    if ($("#txtHora").val() == '') {
        $("#txtHora").css('border', '1px dashed red');
        MensajeAdvertencia('¡Error al validar la HORA!');
        return;
    }else if ($("#checkSellado").prop('checked') == false && $("#txtObservacionDetalle").val() == '') {
        $("#txtObservacionDetalle").css('border', '1px dashed red');
        MensajeAdvertencia("¡Debe ingresar una OBSERVACION!");
        return;
    } else { GuardarDetalle();}
}

function GuardarDetalle() {
     
        MostrarModalCargando();
        var fechaHora = $("#txtFecha").val() + " " + $("#txtHora").val();
        $.ajax({
            url: "../TemperaturaTermoencogidoSellado/GuardarModificarTermoencogidoSelladoDetalle",
            type: "POST",
            data: {
                Id: ListaDatosDetalle.Id,
                IdCabecera: ListaDatos.Id,
                HoraVerificacion: fechaHora,
                Temperatura: $("#txtTemperatura").val(),
                CorrectoSellado: $("#checkSellado").prop('checked'),
                Observacion: $("#txtObservacionDetalle").val()
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#ModalGenerarDetalle").modal("hide");
                if (resultado == 0) {
                    MensajeCorrecto("Datos guardados correctamente");
                } else if (resultado == 1) { MensajeCorrecto("Actualización de datos correcta"); }
                else {
                    MensajeError("!Error al Guardar/Actuaizar los datos¡");
                    return;
                }
                CargarCabecera(0);
                CargarDetalle(0);
                limpiarDetalle();
                CerrarModalCargando();
            },
            error: function (resultado) {
                limpiarDetalle();
                CerrarModalCargando();
                MensajeError(resultado.responseText, false);
            }
        });
    
}

//Retorna PartialView
function CargarDetalle(opcion) {
    var op = opcion;
    if (ListaDatos.length==0) {
        ListaDatos.IdCabecera = 0;
    }
    if (ListaDatosDetalle.length==0) {
        ListaDatosDetalle.Id = 0;
    }
    $("#divTableEntregaProductoDetalle").html('');
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ControlTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            id: ListaDatosDetalle.Id,
            idCabecera: ListaDatos.Id,
            op: op
        },
        success: function (resultado) {
            ListaDatosDetalle = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableEntregaProductoDetalle").html("No existen registros");
            } else {
                $("#divTableEntregaProductoDetalle").prop("hidden", false);
                $("#divTableEntregaProductoDetalle").html(resultado);
                $("#divDetalleControlCloro").prop("hidden", false);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            CerrarModalCargando();
        }
    });
}

function EliminarDetalle(jdato) {
    MostrarModalCargando();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/EliminarTermoencogidoSelladoDetalle",
        type: "POST",
        data: {
            Id: jdato.Id
        },
        success: function (resultado) {
            CerrarModalCargando();
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros (IdDesinfeccionManosDetalle)");
                return;
            }
            MensajeCorrecto("Registro Eliminado con Éxito");
            CargarDetalle(0);
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);

        }
    });
}

function ActulizarDetalle(jdata) {
    $("#ModalGenerarDetalle").modal('show');
    $("#txtTemperatura").val(jdata.Temperatura);
    $("#txtObservacionDetalle").val(jdata.Observacion);    
    $("#txtHora").val(moment(jdata.HoraVerificacion).format("HH:mm"));
    $("#checkSellado").prop('checked', jdata.CorrectoSellado);
    $("#txtObservacionDetalle").css('border', '');
    $("#txtHora").css('border', '');
    if (jdata.CorrectoSellado == true) {
        $("#LabelEstado").text('SI');
    } else {
        $("#LabelEstado").text('NO');
    }
    ListaDatosDetalle = [];
    ListaDatosDetalle = jdata;
}

function CambioEstado(valor) {
    if (valor)
        $('#LabelEstado').text('SI');
    else
        $('#LabelEstado').text('NO');
}