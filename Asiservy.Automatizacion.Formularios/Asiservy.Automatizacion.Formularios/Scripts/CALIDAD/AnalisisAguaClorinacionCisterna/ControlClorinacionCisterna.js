var itemEditar = [];
var estadoReporte = [];
var itemDetalle = [];
var siActualizar = [];
$(document).ready(function () {
    CargarCabecera();
    $('#selectCisterna').select2();
    MascaraInputs();
});

function MascaraInputs() {   
    $('#txtStd').val('');
    $('#txtStd').css('border', '');
    $('#txtStd').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '999.99' , 'min':'0'});
    $('#txtDt').val('');
    $('#txtDt').css('border', '');
    $('#txtDt').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '999.99', 'min': '0' });
    $('#txtCl').val('');
    $('#txtCl').css('border', '');
    $('#txtCl').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '999.99', 'min': '0' });
}

function ConsultarEstadoRegistro() {
    $.ajax({
        url: "../AnalisisAguaClorinacionCisterna/ConsultarEstadoReporte",
        data: {
            fechaControl: $("#txtFecha").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            estadoReporte = resultado.EstadoReporte;
            CambiarMensajeEstado(resultado.EstadoReporte);
        },
        error: function () {
            MensajeError(Mensajes.Error, false);
        }
    });
}

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../AnalisisAguaClorinacionCisterna/ConsultarEstadoReporte",
        data: {
            fechaControl: $("#txtFecha").val()
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
                itemEditar = 0;
                LimpiarModalIngresoCabecera();
            } else {
                itemEditar = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtObservacionVer").val(resultado.Observaciones);
               
                CargarDetalle(1);
            }
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
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
                url: "../AnalisisAguaClorinacionCisterna/GuardarModificarClorinacionCisterna",
                type: "POST",
                data: {
                    IdAnalisisAguaControl: itemEditar.IdAnalisisAguaControl,
                    Fecha: $("#txtIngresoFecha").val(),
                    Observaciones: $("#txtIngresoObservacion").val(),
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
                    } else if (resultado == 3) {
                        MensajeAdvertencia('Error al ingresar la FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY') + '</span>');
                        return;
                    } else if (resultado == 4) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        return;
                    }
                    $('#ModalIngresoCabecera').modal('hide');
                    $('#divBotonesCRUD').prop('hidden', false);
                    $('#divMostarTablaDetalle').prop('hidden', false);
                    $('#divBotonCrear').prop('hidden', true);
                    itemEditar = 0;
                    $('#cargac').hide();
                    CargarCabecera();
                },
                error: function () {
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
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
                url: "../AnalisisAguaClorinacionCisterna/EliminarClorinacionCisterna",
                type: "POST",
                data: {
                    IdAnalisisAguaControl: itemEditar.IdAnalisisAguaControl
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
                    MensajeError(Mensajes.Error, false);
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
            $("#txtIngresoFecha").val(moment(itemEditar.Fecha).format("YYYY-MM-DD"));
            $("#txtIngresoObservacion").val(itemEditar.Observaciones);
            $('#ModalIngresoCabecera').modal('show');
        }
    }, 200);
}

function ModalIngresoCabecera() {
    LimpiarModalIngresoCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarModalIngresoCabecera() {
    $('#txtIngresoFecha').val(moment($('#txtFecha').val()).format('YYYY-MM-DD'));
    $('#txtIngresoObservacion').val('');
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
    if ($('#txtIngresoFecha').val() == '') {
        $("#txtIngresoFecha").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFecha").css('border', ''); }
    return con;
}

//DETALLE
function CargarDetalle() {
    $('#cargac').show();
    var op = 0;
    $.ajax({
        url: "../AnalisisAguaClorinacionCisterna/ControlClorinacionCisternaPartial",
        data: {           
            idAnalisisAguaControl: itemEditar.IdAnalisisAguaControl,
            op:op
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
                $('#divMostarTablaDetallesVer').prop('hidden', false);
                $('#divMostarTablaDetallesVer').html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            console.log(resultado.responseText, false)
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function ModalIngresoDetalle() {
    siActualizar = false;
    LimpiarDetalle();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#ModalIngresoDetalle').modal('show');            
            $('#cargac').hide();
        }
    }, 200);
}

function GuardarDetalle() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#cargac').show();           

            $.ajax({
                url: "../AnalisisAguaClorinacionCisterna/GuardarModificarClorinacionDetalle",
                type: "POST",
                data: {
                    IdAnalisisAguaDetalle: itemDetalle.IdAnalisisAguaDetalle,
                    IdAnalisisAguaControl: itemEditar.IdAnalisisAguaControl,
                    IdCisterna: $('#selectCisterna').val(),
                    Hora: $('#txtIngresoFechaDetalle').val(),
                    STD: $('#txtStd').val(),
                    DT: $('#txtDt').val(),
                    CL: $('#txtCl').val(),
                    siActualizar: siActualizar
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    }  else if (resultado == 2) {
                        MensajeAdvertencia('¡Error! La hora ingresada ya existe  : <span class="badge badge-danger">' + $("#txtIngresoFechaDetalle").val() + '</span>');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 3) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    }
                    $('#selectTurnoFiltro').val($('#selectTurno').val());
                    $('#selectAreaAuditarFiltro').val($('#selectAreaAuditar').val()).trigger('change');
                    LimpiarDetalle();
                    CargarDetalle();
                    $('#ModalIngresoDetalle').modal('hide');
                    $('#cargac').hide();
                },
                error: function () {
                    $('#cargac').hide();
                    MensajeError(Mensajes.Error, false);
                }
            });
        }
    }, 200);
}

function ActualizarDetalle(jdata) {
    ModalIngresoDetalle();
    document.getElementById('txtIngresoFechaDetalle').value = moment(jdata.Hora).format('HH:mm');
    $('#cargac').show();     
    if (jdata.STD!=null)
        document.getElementById('txtStd').value = jdata.STD;
    if (jdata.DT!=null)
        document.getElementById('txtDt').value = jdata.DT;
    if (jdata.CL!=null)
        document.getElementById('txtCl').value = jdata.CL;

    $('#selectCisterna').val(jdata.IdCisterna).trigger('change');
    itemDetalle = jdata;
    siActualizar = true;
    $('#cargac').hide();
    
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

function OnChangeTextBoxDetalle() {
    var con = 0;
    if ($('#txtIngresoFechaDetalle').val() == '') {
        $("#txtIngresoFechaDetalle").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFechaDetalle").css('border', ''); }
    return con;
}

function EliminarConfirmarDetalle(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControlDetalle").modal("show");
            $("#myModalLabelDetalle").text("¿Desea Eliminar el detalle?");
            itemDetalle = jdata;
        }
    }, 200);
}

function EliminarDetalleSi() {
    $.ajax({
        url: "../AnalisisAguaClorinacionCisterna/EliminarClorinacionDetalle",
        type: "POST",
        data: {
            IdAnalisisAguaControl: itemDetalle.IdAnalisisAguaControl,
            IdAnalisisAguaDetalle: itemDetalle.IdAnalisisAguaDetalle
        },
        success: function (resultado) {
            itemDetalle = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdAnalisisAguaDetalle");
                $("#modalEliminarControlDetalle").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControlDetalle").modal("hide");
                CargarDetalle();
                MensajeCorrecto("Registro eliminado con Éxito");
                $('#cargac').hide();
            } else if (resultado == '3') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            }
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function LimpiarDetalle() {
    MascaraInputs();
    var date = new Date();
    $('#txtIngresoFechaDetalle').val(moment(date).format('HH:mm'));
    //$('#txtStd').val('');
    //$('#txtDt').val('');
    //$('#txtCl').val('');
    itemDetalle = [];
}

function EliminarDetalleNo() {
    $("#modalEliminarControlDetalle").modal("hide");
}