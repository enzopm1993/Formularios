var itemEditar = [];
//PARAMETROS SP op=0 FILTRO POR idControlHigiene
var estadoReporte = [];
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../CalibracionFluorometro/ConsultarCalibracionFluorometroJson",
        data: {
            fecha: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divMostrarCabecera').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
                $('#divBotonCrear').prop('hidden', false);
                $('#divBotonCrearDetalle').prop('hidden', true);
                $('#divMostarTablaDetallesVer').prop('hidden', true);
                $('#divMostarTablaDetallesVer').html(resultado);
                LimpiarCabecera();
            } else {
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                CargarDetalle(resultado.IdCalibracionFluorDetalle);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DDTHH:mm'));
                $("#txtObservacionVer").val(resultado.Observacion);
                itemEditar = resultado;
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera(siAprobar) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
           
            $.ajax({
                url: "../CalibracionFluorometro/GuardarModificarCalibracionFluor",
                type: "POST",
                data: {
                    IdCalibracionFluor: itemEditar.IdCalibracionFluor,
                    Fecha: $("#txtIngresoFechaCabecera").val(),                    
                    Observacion: $("#txtObservacion").val(),
                    siAprobar: siAprobar
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    }
                    $('#ModalIngresoCabecera').modal('hide');
                    $('#divBotonesCRUD').prop('hidden', false);
                    $('#divMostarTablaDetalle').prop('hidden', false);
                    $('#divBotonCrear').prop('hidden', true);
                    LimpiarCabecera();
                    itemEditar = 0;
                    $('#cargac').hide();
                    CargarCabecera();
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    }, 200);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {           
            $.ajax({
                url: "../CalibracionFluorometro/EliminarHigieneControl",
                type: "POST",
                data: {
                    IdCalibracionFluor: itemEditar.IdCalibracionFluor
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == "0") {
                        MensajeAdvertencia("Falta Parametro IdLavadoCisterna");
                        $("#modalEliminarControl").modal("hide");
                        $('#cargac').hide();
                        return;
                    } else if (resultado == "1") {
                        $('#firmaDigital').prop('hidden', true);
                        $("#modalEliminarControl").modal("hide");
                        CargarCabecera();
                        MensajeCorrecto("Registro eliminado con Éxito");
                        $('#cargac').hide();
                    } else if (resultado == '2') {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                        $('#cargac').hide();
                        return;
                    }
                    itemEditar = 0;
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            var hora = moment(itemEditar.Hora).format('HH:mm');
            $("#txtIngresoFechaCabecera").val(moment(itemEditar.Fecha +' '+hora).format("YYYY-MM-DDTHH:mm"));
            $("#txtObservacion").val(itemEditar.Observacion);
            $('#ModalIngresoCabecera').modal('show');
        }
    }, 200);
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $("#txtFechaCabecera").prop('disabled', false);
    $('#ModalIngresoCabecera').modal('show');
    $("#txtIngresoFechaCabecera").val(moment($("#txtFecha").val()).format("YYYY-MM-DDTHH:mm"));
    itemEditar = [];
}

function LimpiarCabecera() {
    $('#txtFechaCabeceraVer').val('');
    $('#txtObservacionVer').val('');
    CambiarMensajeEstado('nada');
}

function LimpiarModalIngresoCabecera() {
    $('#txtIngresoFechaCabecera').val(moment($('#txtFecha').val()).format('YYYY-MM-DDTHH:mm'));
    $('#txtObservacion').val('');
}

function ValidarDatosVacios(siAprobar) {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera(siAprobar);
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtIngresoFechaCabecera').val() == '') {
        $("#txtIngresoFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFechaCabecera").css('border', ''); }
    return con;
}

//DETALLE
function CargarDetalle(idCalibracionFluorDetalle) {
    $('#cargac').show();
    $.ajax({
        url: "../CalibracionFluorometro/CalibracionFluorometroPartial",
        data: {
            idCalibracionFluorDetalle: idCalibracionFluorDetalle            
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaDetallesVer").html("No existen registros");
                $('#firmaDigital').prop('hidden', true);
            } else {
                $('#firmaDigital').prop('hidden', false);
                $('#divBotonCrearDetalle').prop('hidden', true);
                $('#divMostarTablaDetallesVer').prop('hidden', false);
                $('#divMostarTablaDetallesVer').html(resultado);

            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ModalIngresoDetalle() {
    LimpiarDetalle();
    $('#ModalIngresoDetalle').modal('show');
    var estadoRegistro = 'A';
    //INICIO AJAX
    $.ajax({
        url: "../CalibracionFluorometro/ConsultaHigieneMantActivosPartial",
        type: "GET",
        data: {
            estadoRegistro: estadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaDetalles").html("No existen registros");
            } else {

                $("#divMostarTablaDetalles").html(resultado);

            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
    //FIN AJAX
}

function LimpiarDetalle() {

}

function GuardarDetalle(jdata) {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#cargac').show();
            var con = 0;
            var idControlHigiene = itemEditar[0].IdControlHigiene;
            jdata.forEach(function (rowMantenimiento) {
                var sel = document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value;
                var obs = document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value;
                var acc = document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value;
                var idControlMantenimiento = document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value;
                jdata[con].IdControlHigiene = idControlHigiene;
                jdata[con].LimpiezaEstado = sel;
                jdata[con].Observacion = obs;
                jdata[con].AccionCorrectiva = acc;
                jdata[con].IdControlDetalle = idControlMantenimiento;
                con++;
            });

            $.ajax({
                url: "../CalibracionFluorometro/GuardarModificarHigieneControlDetalle",
                type: "POST",
                data: {
                    listaControlDetalle: jdata
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    }
                    $('#ModalIngresoDetalle').modal('hide');
                    LimpiarCabecera();
                    itemEditar = 0;
                    $('#cargac').hide();
                    CargarCabecera(0);
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function ActualizarDetalle(jdata) {//LLAMADA DESDE EL PARTIAL CalibracionFluorometroDetallePartial
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ModalIngresoDetalle();
            setTimeout(function () {
                jdata.forEach(function (rowMantenimiento) {
                    document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.LimpiezaEstado;
                    document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.Observacion;
                    document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.AccionCorrectiva;
                    document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.IdControlDetalle;
                });
            }, 1000);
        }
    }, 200);
}

function ConsultarEstadoRegistro() {
    $.ajax({
        url: "../CalibracionFluorometro/ConsultarCalibracionFluorometroJson",
        data: {
            idCalibracionFluor: itemEditar.IdCalibracionFluor
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            estadoReporte = resultado.EstadoReporte;
            CambiarMensajeEstado(resultado.EstadoReporte);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#lblAprobadoPendiente").text("APROBADO");
        $("#lblAprobadoPendiente").removeClass('badge-danger');
        $("#lblAprobadoPendiente").addClass('badge badge-success');
    } else if (estadoReporteParametro == false) {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada') {
        $("#lblAprobadoPendiente").text("");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").removeClass('badge badge-danger');
    }
}