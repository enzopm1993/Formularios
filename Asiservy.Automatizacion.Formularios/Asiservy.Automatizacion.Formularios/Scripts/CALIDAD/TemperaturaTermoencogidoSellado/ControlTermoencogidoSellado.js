var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes

$(document).ready(function () {
    CargarCabecera(0);
});

function CargarCabecera(opcion) {
    $('#cargac').show();
    var op = opcion;
    if ($("#txtFecha").val() == '') {
        return;
    } else {
        $.ajax({
            url: "../TemperaturaTermoencogidoSellado/ConsultarTermoencogidoSellado",
            type: "GET",
            data: {
                fechaDesde: $("#txtFecha").val(),
                fechaHasta: $("#txtFecha").val(),
                opcion: op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                $("#txtObservacion").val('');
                $("#txtMostrarFecha").val('');
                if (resultado == "0") {
                    $("#txtObservacion").prop("disabled", false);
                    $("#divDetalleControlCloro").prop("hidden", true);
                    $("#btnModalEditar").prop("hidden", true);
                    $("#btnModalEliminar").prop("hidden", true);
                    $("#divCabecera1").prop("hidden", true);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalGenerarRegistro").prop("hidden", false);
                    ListaDatos = resultado;
                } else {
                    $("#divDetalleControlCloro").prop("hidden", false);
                    $("#btnModalGenerar").prop("hidden", false);
                    $("#btnModalEditar").prop("hidden", false);
                    $("#btnModalEliminar").prop("hidden", false);             
                    $("#btnModalGenerarRegistro").prop("hidden", true);
                    $("#txtObservacion").prop("disabled", true);
                    $("#txtObservacion").val(resultado.Observacion);
                    $("#txtMostrarFecha").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                    $("#divCabecera1").prop("hidden", false);                   
                    ListaDatos = resultado;
                    if (resultado.EstadoReporte == true) {
                        $("#lblAprobadoPendiente").text("APROBADO");
                        $("#lblAprobadoPendiente").removeClass('badge-danger');
                        $("#lblAprobadoPendiente").addClass('badge badge-success');
                    } else {
                        $("#lblAprobadoPendiente").text("PENDIENTE");
                        $("#lblAprobadoPendiente").removeClass('badge-success');
                        $("#lblAprobadoPendiente").addClass('badge badge-danger');
                    }
                    CargarDetalle(0);
                }
                setTimeout(function () {
                    $('#cargac').hide();
                    return true;
                }, 200);
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            }
        });
    }
}

function GuardarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/GuardarModificarTermoencogidoSellado",
        type: "POST",
        data: {
            id: ListaDatos.Id,
            Fecha: moment($("#txtIngresoFecha").val()).format("YYYY-MM-DD"),
            Observacion: $("#txtIngresoObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera(0);
            $("#btnModalGenerar").prop("hidden", true);
            $("#btnModalEditar").prop("hidden", false);
            $("#btnModalEliminar").prop("hidden", false);
            $("#divCabecera1").prop("hidden", false);
            $("#divDetalleProceso").prop("hidden", false);
            $("#divDetalleControlCloro").prop("hidden", false);
            $('#ModalIngresarCabecera').modal('hide');
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar() {
    CargarCabecera(0);
    setTimeout(function () {//ESTO NOS AYUDA A QUE SE EJECUTE PRIMERO EL METODO ANTERIOR PARA REALIZAR VALIDACIONES
        if (ListaDatos.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
        }
    },500);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
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
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#divTableEntregaProductoDetalle").prop("hidden", true);
                CargarCabecera(0);
                MensajeCorrecto("Registro Eliminado con Éxito");
                //$("#txtFecha").prop("disabled", false);
                $("#modalEliminarControl").modal("hide");
                $("#btnModalGenerar").prop("hidden", false);
                $("#btnModalEditar").prop("hidden", true);
                $("#btnModalEliminar").prop("hidden", true);
                $("#divCabecera1").prop("hidden", true);
                $("#divDetalleProceso").prop("hidden", true);
                $("#txtObservacion").prop("disabled", false);
                $("#txtObservacion").val('');
                $("#divDetalleControlCloro").prop("hidden", true);
                $("#divCabecera1").prop("hidden", true);
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);
            }
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabeceraActivarCotroles() {
    var r = CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#ModalIngresarCabecera').modal('show');
            $('#txtIngresoFecha').val(moment($('#txtFecha').val()).format('YYYY-MM-DD'));
            $('#txtIngresoObservacion').val($('#txtObservacion').val());
        }
    },500);
}

//DETALLE INGRESO DE LINEAS (SE LISTA EL CLASIFICADOR PARA PODER ARMAR LA CABECERA DE LA TABLA)
function ModalGenerarDetalle() {
    CargarCabecera(0);   
    setTimeout(function () {        
    if (ListaDatos.EstadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        $("#ModalGenerarDetalle").modal("show");
        limpiarDetalle();
        }
    }, 500);
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
    //$('#cargac').show();
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
                } else if (resultado == 1)
                { MensajeCorrecto("Actualización de datos correcta"); }
                else if (resultado == 2) { MensajeAdvertencia("¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!", 5);}
                else {
                    MensajeError("!Error al Guardar/Actualizar los datos¡");
                    return;
                }
                CargarCabecera(0);
                CargarDetalle(0);
                limpiarDetalle();
                //$('#cargac').hide();
            },
            error: function (resultado) {
                limpiarDetalle();
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
    
}

//Retorna PartialView
function CargarDetalle(opcion) {
    var op = opcion;
    var idCabecera = 0;
    if (ListaDatos == 0) {
        idCabecera = 0;
    } else {
        idCabecera = ListaDatos.Id;
    }    
    
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ControlTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            fechaDesde: $('#txtFecha').val(),
            fechaHasta: $('#txtFecha').val(),
            idCabecera: idCabecera,
            op: op
        },
        success: function (resultado) {
            ListaDatosDetalle = [];
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divTableEntregaProductoDetalle").html('');
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
            $('#cargac').hide();
        }
    });
}

function EliminarDetalle() {
    $('#cargac').show();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/EliminarTermoencogidoSelladoDetalle",
        type: "POST",
        data: {
            Id: ListaDatosDetalle.Id
        },
        success: function (resultado) {
            $('#cargac').hide();
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros (IdDesinfeccionManosDetalle)");
                return;
            }
            $("#ModalEliminarDetalle").modal("hide");
            MensajeCorrecto("Registro Eliminado con Éxito");
            ListaDatosDetalle = [];
            CargarDetalle(0);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ActulizarDetalle(jdata) {
    CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
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
    },500);
}

function CambioEstado(valor) {
    if (valor)
        $('#LabelEstado').text('SI');
    else
        $('#LabelEstado').text('NO');
}

function ModalIngresarCabecera() {
    $('#ModalIngresarCabecera').modal('show');
    $('#txtIngresoFecha').val(moment($('#txtFecha').val()).format('YYYY-MM-DD'));
    $('#txtIngresoObservacion').val('');
}

function EliminarDetalleNo() {
    $("#ModalEliminarDetalle").modal("hide");
    ListaDatosDetalle = [];
}

function EliminarConfirmarDetalle(jdata) {
    CargarCabecera(0);
    setTimeout(function () {
        if (ListaDatos.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ListaDatosDetalle = jdata;
            $("#ModalEliminarDetalle").modal("show");
        }
    },500);
}