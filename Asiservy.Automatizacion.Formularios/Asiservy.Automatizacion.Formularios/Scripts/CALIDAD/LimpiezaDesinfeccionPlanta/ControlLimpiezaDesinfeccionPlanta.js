﻿var itemEditar = [];
var estadoReporte = [];
var rotation = 0;
var siActualizar = false;
var eliminarDetalle = [];
var accionCorrectiva = [];
$(document).ready(function () {
    //document.getElementById('selectTurno').value = 0;
    CargarCabecera();
    $('.js-example-basic-single').select2();
    $('#selectAreaAuditarFiltro').select2({
        width: '100%'
    });
    $('#selectTurno').select2({
        width: '100%'
    });
    $('#selectTurnoIngresar').select2({
        width: '100%'
    });
    var x = document.getElementById("selectAreaAuditarFiltro");
    var option = document.createElement("option");
    option.text = 'TODOS LOS RESGISTROS';
    option.value = 'galo';
    x.add(option);
});

function ConsultarEstadoRegistro() {
    var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/ConsultarCabeceraTurno",
        data: {
            fechaControl: moment(fechaFiltro).format('YYYY-MM-DD'),
            turno: document.getElementById('selectTurno').value
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
    DatePicker();
    if (document.getElementById('selectTurno') == null) {
        $('#divBotonCrear').prop('hidden', true);
        $('#divBotonCrearDetalle').prop('hidden', true);
        return;
    }
    $('#cargac').show();
    if ($('#txtFecha').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/ConsultarCabeceraTurno",
        data: {
            fechaControl: moment(fechaFiltro).format('YYYY-MM-DD'),
            turno: document.getElementById('selectTurno').value
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
                CambiarMensajeEstado(false);
                itemEditar = 0;
                LimpiarModalIngresoCabecera();
                $("#lblAprobadoPendiente").text("");
            } else {
                itemEditar = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);                
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtObservacionVer").val(resultado.ObservacionControl);
                document.getElementById('selectTurno').value = resultado.Turno;
                $('#txtInspectorVer').val(resultado.UsuarioIngresoLog);
                $('#selectAreaAuditarFiltro').val('galo').trigger('change');             
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
            var fechaIngreso = $('#datetimepicker2').datetimepicker('viewDate');
            $.ajax({
                url: "../LimpiezaDesinfeccionPlanta/GuardarModificarLimpiezaCabecera",
                type: "POST",
                data: {
                    IdLimpiezaDesinfeccionPlanta: itemEditar.IdLimpiezaDesinfeccionPlanta,
                    Fecha: moment(fechaIngreso).format('YYYY-MM-DD'),    
                    ObservacionControl: $("#txtIngresoObservacion").val(),
                    Turno: document.getElementById('selectTurnoIngresar').value,
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
                        var e = document.getElementById("selectTurnoIngresar");
                        var strUser = e.options[e.selectedIndex].text;
                        MensajeAdvertencia('Error el TURNO ya esta ingresado  : <span class="badge badge-danger">' + strUser + '</span>');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 5) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    }
                    document.getElementById('selectTurno').value = document.getElementById('selectTurnoIngresar').value;
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
                url: "../LimpiezaDesinfeccionPlanta/EliminarLimpiezaCabecera",
                type: "POST",
                data: {
                    IdLimpiezaDesinfeccionPlanta: itemEditar.IdLimpiezaDesinfeccionPlanta,
                    Fecha: moment(itemEditar.Fecha).format('YYYY-MM-DD')
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
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
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
    DatePicker2();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtIngresoFecha").val(moment(itemEditar.Fecha).format("DD-MM-YYYY"));
            $("#txtIngresoObservacion").val(itemEditar.ObservacionControl);        
            document.getElementById('selectTurnoIngresar').value = itemEditar.Turno;
            $("#txtIngresoInspector").val(itemEditar.UsuarioIngresoLog);
            $('#ModalIngresoCabecera').modal('show');
        }
    }, 200);
}

function ModalIngresoCabecera() {
    DatePicker2();
    LimpiarModalIngresoCabecera();
    $('#selectTurnoIngresar').val(document.getElementById('selectTurno').value).trigger('change');
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarModalIngresoCabecera() {
    var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
    $('#txtIngresoFecha').val(moment(fechaFiltro).format('DD-MM-YYYY'));
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
function CargarDetalle(op) {
    if ($('#selectAreaAuditarFiltro').val()=='galo') {
        op = 1;
    }

    $('#cargac').show();
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/ConsultarDetallePartial",
        data: {
            idLimpiezaDesinfeccionPlanta: itemEditar.IdLimpiezaDesinfeccionPlanta,
            op: op,
            idAuditoria: $('#selectAreaAuditarFiltro').val()
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
            $('#cargac').hide();
            //console.log(resultado.responseText);
            MensajeError(Mensajes.Error, false);
        }
    });
}

function ModalIngresoDetalleV() {
    ConsultarEstadoRegistro();
    $('#cargac').show();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            $('#cargac').hide();
            return;
        } else {
            ModalIngresoDetalle();            
          }
    },200);
}

function ModalIngresoDetalle() {
    ConsultarIntermidia();
    $('#selectAreaAuditar').prop('disabled', false);
    setTimeout(function () {
        var date = new Date();
        //document.getElementById('txtIngresoFechaDetalle').value = moment(date).format('YYYY-MM-DDTHH:mm');
        $('#ModalIngresoDetalle').modal('show');
        siActualizar = false;
        $('#cargac').hide();
    }, 300);
}

function ConsultarIntermidia() {
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/ConsultarIntermediaJoinObjetoPartial",
        type: "GET",
        data: {
            idAuditoria: $('#selectAreaAuditar').val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia('No hay registros',false);
            } else {
                $('#divMostarTablaDetalles').html(resultado);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
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
            var listaDetalle = [];
            var idAuditoria = 0;
            jdata.forEach(function (rowMantenimiento) {
                var detalle = {};
                detalle.HoraAuditoria = document.getElementById('txtIngresoFechaDetalle').value;
                detalle.Limpieza = document.getElementById('selectLimpieza-' + rowMantenimiento.IdObjeto).value;
                detalle.Desinfeccion = document.getElementById('selectDesinfeccion-' + rowMantenimiento.IdObjeto).value;
                detalle.ObservacionDetalle = document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdObjeto).value;
                //detalle.Turno = document.getElementById('selectTurno').value;
                detalle.IdLimpiezaDesinfeccionPlanta = itemEditar.IdLimpiezaDesinfeccionPlanta;
                detalle.IdMantenimiento = rowMantenimiento.IdMantenimiento;
                detalle.IdDetalle = document.getElementById('txtIdDetalle-' + rowMantenimiento.IdObjeto).value;
                idAuditoria = rowMantenimiento.IdAuditoria;
                listaDetalle.push(detalle);
            });

            $.ajax({
                url: "../LimpiezaDesinfeccionPlanta/GuardarModificarLimpiezaDetalle",
                type: "POST",
                data: {
                    listaDetalle: listaDetalle,
                    siActualizar: siActualizar,
                    idAuditoria: idAuditoria
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeCorrecto('Registro guardado correctamente');                        
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    } else if (resultado == 2) {
                        MensajeError('Error al guardar el detalle');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 3) {
                        MensajeAdvertencia('¡Error! La hora ingresada ya existe  : <span class="badge badge-danger">' + moment($("#txtIngresoFechaDetalle").val()).format('DD-MM-YYYY HH:mm') + '</span>');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 4) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    }
                    //$('#selectTurnoFiltro').val($('#selectTurno').val());
                    $('#selectAreaAuditarFiltro').val($('#selectAreaAuditar').val()).trigger('change');

                    CargarDetalle(2);
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

function ActualizarDetalle(jdata) {//LLAMADA DESDE EL PARTIAL LimpiezaDesinfeccionPlantaDetallePartial
    ConsultarEstadoRegistro();   
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#cargac').show();
            $('#divMostarTablaDetalles').html('');
            ModalIngresoDetalle();  
            setTimeout(function () {
                $('#selectAreaAuditar').val(jdata[0].IdAuditoria).trigger('change');
                $('#selectAreaAuditar').prop('disabled',true);
                //$('#selectTurno').val(jdata[0].Turno);
                document.getElementById('txtIngresoFechaDetalle').value = moment(jdata[0].HoraAuditoria).format('YYYY-MM-DDTHH:mm');
                setTimeout(function () {
                    jdata.forEach(function (rowMantenimiento) {
                        document.getElementById('selectLimpieza-' + rowMantenimiento.IdObjeto).value = rowMantenimiento.Limpieza;
                        document.getElementById('selectDesinfeccion-' + rowMantenimiento.IdObjeto).value = rowMantenimiento.Desinfeccion;
                        document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdObjeto).value = rowMantenimiento.ObservacionDetalle;
                        //document.getElementById('selectTurno').value = rowMantenimiento.Turno;
                        document.getElementById('txtIdDetalle-' + rowMantenimiento.IdObjeto).value = rowMantenimiento.IdDetalle;
                    });
                    siActualizar = true;
                }, 200);
                $('#cargac').hide();
            },500);            
        }
    }, 200);
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
            eliminarDetalle = jdata;
        }
    }, 200);
}

function EliminarDetalleSi() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
           
            $.ajax({
                url: "../LimpiezaDesinfeccionPlanta/EliminarLimpiezaDetalle",
                type: "POST",
                data: {
                    model: eliminarDetalle
                },
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == "0") {
                        MensajeAdvertencia("Falta Parametro eliminarDetalle");
                        $("#modalEliminarControlDetalle").modal("hide");
                        $('#cargac').hide();
                        return;
                    } else if (resultado == "1") {
                        $('#firmaDigital').prop('hidden', true);
                        $("#modalEliminarControlDetalle").modal("hide");
                        CargarDetalle(1);
                        $('#selectAreaAuditarFiltro').val('galo').trigger('change');
                        MensajeCorrecto("Registro eliminado con Éxito");
                        $('#cargac').hide();
                    } else if (resultado == '2') {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    }
                    $('#cargac').hide();
                },
                error: function (resultado) {
                    $('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function LimpiarDetalle() {

}

function EliminarDetalleNo() {
    $("#modalEliminarControlDetalle").modal("hide");
}

function AccionCorrectiva(jdata) {
    ConsultarEstadoRegistro(); 
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#ModalAccionCorrectiva').modal('show');
            accionCorrectiva = jdata;
            EditarAccionCorrectiva();
        }
    },200);
}

function GuardarAccionCorrectiva() {
    $('#cargac').show();
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            if (OnChangeTextBoxAccion()==1) {
                MensajeAdvertencia('Ingrese todos los datos requeridos');
                $('#cargac').hide();
                return;
            }           
            $('#cargac').show();
            var imagen = $('#file-upload')[0].files[0];
            var data = new FormData();
            data.append("dataImg", imagen);
            data.append("IdLimpiezaDesinfeccionPlanta", itemEditar.IdLimpiezaDesinfeccionPlanta);
            data.append("IdDetalle", accionCorrectiva.IdDetalle);
            data.append("HoraAccionCorrectiva", $("#txtHoraAccionCorrectiva").val());
            data.append("PersonaAccionCorrectiva", $("#txtAuditor").val());
            data.append("AccionCorrectiva", $("#txtAccionCorrectiva").val());
            data.append("Rotation", rotation);
            $.ajax({
                url: "../LimpiezaDesinfeccionPlanta/GuardarModificarAccionCorrectiva",
                type: "POST",
                cache: false,
                data: data,
                contentType: false,
                processData: false,
                async: false,
                data: data,
                success: function (resultado) {
                    if (resultado == "101") {
                        window.location.reload();
                    }
                    if (resultado == 0) {
                        MensajeAdvertencia('Error al guardar la Acción Correctiva');
                    } else if (resultado == 1) {
                        MensajeCorrecto('Registro actualizado correctamente');
                    } else if (resultado == 3) {
                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 4) {
                        MensajeAdvertencia('¡Solo se permiten imagenes!', 5);
                        $('#cargac').hide();
                        return;
                    } else if (resultado == 100) {
                        MensajeAdvertencia(Mensajes.MensajePeriodo);
                    } else {
                        var mb =parseFloat(resultado / (1024 * 1024)).toFixed(2);
                        MensajeAdvertencia('¡Exedio el limite de capacidad permitido!:  <span class="badge badge-success">5Mb</span>: Su imagen:<span class="badge badge-danger">' + mb+ 'Mb</span>');
                        $('#cargac').hide();
                        return;
                    }
                    $('#selectAreaAuditarFiltro').val(accionCorrectiva.IdAuditoria).trigger('change');
                    
                    CargarDetalle(2);
                    $('#ModalAccionCorrectiva').modal('hide');
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

function OnChangeTextBoxAccion() {
    var con = 0;
    if ($('#txtHoraAccionCorrectiva').val() == '') {
        $("#txtHoraAccionCorrectiva").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtHoraAccionCorrectiva").css('border', ''); }
    if ($('#txtAuditor').val() == '') {
        $("#txtAuditor").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtAuditor").css('border', ''); }
    if ($('#txtAccionCorrectiva').val() == '') {
        $("#txtAccionCorrectiva").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtAccionCorrectiva").css('border', ''); }
    if ($('#file-upload').val() == '') {
        $("#file-upload").css('border', '1px dashed red');
        //con = 1;
    } else { $("#file-upload").css('border', ''); }
    return con;
}

function LimpiarAccionCorrectiva() {
    var date=new Date();
    $('#txtHoraAccionCorrectiva').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    $("#txtHoraAccionCorrectiva").css('border', '');
    $('#txtIngresoObservacion').val('');
    $('#txtAuditor').val('');
    $("#txtAuditor").css('border', '');
    $('#txtAccionCorrectiva').val('');
    $("#txtAccionCorrectiva").css('border', '');
    $("#file-preview-zone").html('');
    $("#file-upload").val('');
    $("#file-upload").css('border', '');
    Rotacion = 0;
}

function EditarAccionCorrectiva() {
    LimpiarAccionCorrectiva();
    if (accionCorrectiva.HoraAccionCorrectiva != null) {
        $("#txtHoraAccionCorrectiva").val(moment(accionCorrectiva.HoraAccionCorrectiva).format('YYYY-MM-DDTHH:mm'));
    } 
    $("#txtAuditor").val(accionCorrectiva.PersonaAccionCorrectiva);
    $("#txtAccionCorrectiva").val(accionCorrectiva.AccionCorrectiva);
    if (accionCorrectiva.RutaFoto != null && accionCorrectiva.RutaFoto != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = $('#btnPath').val() + accionCorrectiva.RutaFoto;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);

        $("#file-preview").addClass("img");
        $('#file-preview').rotate(parseInt(accionCorrectiva.Rotation));

        var img = new Image();
        document.getElementById("file-preview").style.height = "250px";
        document.getElementById("file-preview").style.width = "250px";
        img.src = $('#btnPath').val() + accionCorrectiva.RutaFoto;
    }
}

function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#file-preview-zone").html('');
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
            filePreview.src = e.target.result;
            var previewZone = document.getElementById('file-preview-zone');
            previewZone.appendChild(filePreview);
            $("#file-preview").addClass("img");
            document.getElementById("file-preview").style.height = "0px";
            document.getElementById("file-preview").style.width = "0px";
            var image = new Image();
            image.src = e.target.result;
            image.onload = function () {
                if (this.width < this.height) {
                    document.getElementById("file-preview").style.height = "300px";
                    document.getElementById("file-preview").style.width = "300px";
                }
                else {
                    document.getElementById("file-preview").style.height = "300px";
                    document.getElementById("file-preview").style.width = "300px";
                }
            };
        }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    if ($('#file-upload').val() == '') {
        $("#file-upload").css('border', '1px dashed red');
        con = 1;
    } else { $("#file-upload").css('border', ''); }
    readFile(e.srcElement);
}

$('#file-preview-zone').on("click", function (e) {
    rotation += 90;
    $('#file-preview').rotate(rotation);
    if (rotation == 360) {
        rotation = 0;
    }
});

function DatePicker() {
    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar-alt',
            up: 'fas fa-caret-up',
            down: 'fas fa-caret-down',
            previous: 'fas fa-backward',
            next: 'fas fa-forward',
            today: 'fas fa-calendar-day',
            clear: 'fas fa-trash-alt',
            close: 'fas fa-window-close'
        }
    });
    $('#datetimepicker1').datetimepicker(
        {
            //date: moment().format("DD-MM-YYYY"),
            format: "DD-MM-YYYY",
            //minDate: model.Fecha,
            maxDate: moment(),
            ignoreReadonly: true
        });
}

function DatePicker2() {
    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar-alt',
            up: 'fas fa-caret-up',
            down: 'fas fa-caret-down',
            previous: 'fas fa-backward',
            next: 'fas fa-forward',
            today: 'fas fa-calendar-day',
            clear: 'fas fa-trash-alt',
            close: 'fas fa-window-close'
        }
    });
    $('#datetimepicker2').datetimepicker(
        {
            format: "DD-MM-YYYY",
            maxDate: moment(),
            ignoreReadonly: true
        });
}