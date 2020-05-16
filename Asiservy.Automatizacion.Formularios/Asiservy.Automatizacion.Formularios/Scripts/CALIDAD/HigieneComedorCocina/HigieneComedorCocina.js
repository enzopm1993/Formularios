var itemEditar = [];
//PARAMETROS SP op=0 FILTRO POR idControlHigiene
var estadoReporte = [];
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFecha').val()=='') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarHigieneControl",
        data: {
            fecha: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#firmaDigital').prop('hidden', true);
                $('#divMostrarCabecera').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
                $('#divBotonCrear').prop('hidden', false); divBotonCrearDetalle
                $('#divBotonCrearDetalle').prop('hidden', true); 
                $('#divMostarTablaDetallesVer').prop('hidden', true);
                $('#divMostarTablaDetallesVer').html(resultado);
                LimpiarCabecera();
            } else {                
                CambiarMensajeEstado(resultado[0].EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false); 
                CargarDetalle(resultado[0].IdControlHigiene, 0);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado[0].Fecha).format('YYYY-MM-DDTHH:mm'));
                $("#txtObservacionVer").val(resultado[0].Observacion);
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
            var idControlHigiene = 0;
            if (itemEditar.length != 0) {
                idControlHigiene = itemEditar[0].IdControlHigiene;
            }
            $.ajax({
                url: "../HigieneComedorCocina/GuardarModificarHigieneControl",
                type: "POST",
                data: {
                    IdControlHigiene: idControlHigiene,
                    Fecha: $("#txtIngresoFechaCabecera").val(),
                    Hora: $("#txtIngresoFechaCabecera").val(),
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

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte== true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    },200);
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
            var idControlHigiene = 0;
            if (itemEditar.length != 0) {
                idControlHigiene = itemEditar[0].IdControlHigiene;
            }
            $.ajax({
                url: "../HigieneComedorCocina/EliminarHigieneControl",
                type: "POST",
                data: {
                    IdControlHigiene: idControlHigiene
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
                    }else if (resultado == '2') {
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
            $("#txtIngresoFechaCabecera").val(moment(itemEditar[0].Fecha).format("YYYY-MM-DDTHH:hh"));
            $("#txtObservacion").val(itemEditar[0].Observacion);
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
    } else { $("#txtIngresoFechaCabecera").css('border', '');}
    return con;
}

//DETALLE
function CargarDetalle(idControlHigiene, op) {
    $('#cargac').show();
    $.ajax({
        url: "../HigieneComedorCocina/HigieneComedorCocinaDetallePartial",
        data: {
            IdControlHigiene: idControlHigiene,
            fechaDesde: $('#txtFecha').val(),
            fechaHasta: $('#txtFecha').val(),
            op: op
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
        url: "../HigieneComedorCocina/ConsultaHigieneMantActivosPartial",
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
                url: "../HigieneComedorCocina/GuardarModificarHigieneControlDetalle",
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

function ActualizarDetalle(jdata) {//LLAMADA DESDE EL PARTIAL HigieneComedorCocinaDetallePartial
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
        url: "../HigieneComedorCocina/ConsultarHigieneControl",
        data: {
            fecha: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            estadoReporte = resultado[0].EstadoReporte;
            CambiarMensajeEstado(resultado[0].EstadoReporte);
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
    } else if(estadoReporteParametro == false) {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada'){
        $("#lblAprobadoPendiente").text("");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").removeClass('badge badge-danger');
    }
}