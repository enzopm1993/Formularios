var ListaDatos = [];
var ListaDatosDetalle = [];
var horaActualizar = '';//0=No, 1=Yes
var date = new Date();
$(document).ready(function () {  
    //$("#txtFecha").val(moment(date).format('DD-MM-YYYY'));
    //DatePicker();
    CargarCabecera();
    $('#txtTemperatura').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': '100.00' });
    $('#selectTurno').select2({
        width: '100%'
    });
    $('#selectTurnoIngresar').select2({
        width: '100%'
    });
});

function ConsultarEstadoRegistro() {
    var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ConsultarTermoencogidoSellado",
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
        $('#btnModalGenerarRegistro').prop('hidden', true);
        $('#divCabecera1').prop('hidden', true);
        return;
    }
    $('#cargac').show();   
    if ($("#txtFecha").val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    } else {
        var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
        $.ajax({
            url: "../TemperaturaTermoencogidoSellado/ConsultarTermoencogidoSellado",
            type: "GET",
            data: {
                fechaControl: moment(fechaFiltro).format('YYYY-MM-DD'),                
                turno: document.getElementById('selectTurno').value
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
                    $("#lblAprobadoPendiente").text("");
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
                    CambiarMensajeEstado(resultado.EstadoReporte);
                    CargarDetalle(0);
                }
                    $('#cargac').hide();
                    return true;
            },
            error: function (resultado) {
                //MensajeError(resultado.responseText, false);
                                MensajeError(Mensajes.Error, false);
                $('#cargac').hide();
            }
        });
    }
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

function GuardarCabecera() {
    $('#cargac').show();
    var fechaIngreso = $('#datetimepicker2').datetimepicker('viewDate');
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/GuardarModificarTermoencogidoSellado",
        type: "POST",
        data: {
            id: ListaDatos.Id,
            Fecha: moment(fechaIngreso).format("YYYY-MM-DD"),
            Observacion: $("#txtIngresoObservacion").val(),
            Turno: document.getElementById('selectTurnoIngresar').value
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
            } else if (resultado == 5) {
                var e = document.getElementById("selectTurnoIngresar");
                var strUser = e.options[e.selectedIndex].text;
                MensajeAdvertencia('Error el TURNO ya esta ingresado  : <span class="badge badge-danger">' + strUser + '</span>');
                $('#cargac').hide();
                return;
            } else if (resultado == 4) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            CargarCabecera(0);
            $("#btnModalGenerar").prop("hidden", true);
            $("#btnModalEditar").prop("hidden", false);
            $("#btnModalEliminar").prop("hidden", false);
            $("#divCabecera1").prop("hidden", false);
            $("#divDetalleProceso").prop("hidden", false);
            $("#divDetalleControlCloro").prop("hidden", false);
            $('#ModalIngresarCabecera').modal('hide');
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarConfirmar() {
    ConsultarEstadoRegistro();
    setTimeout(function () {//ESTO NOS AYUDA A QUE SE EJECUTE PRIMERO EL METODO ANTERIOR PARA REALIZAR VALIDACIONES
        if (estadoReporte == true) {
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
            id: ListaDatos.Id,
            Fecha: moment(ListaDatos.Fecha).format('YYYY-MM-DD')
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
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } $('#cargac').hide();
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
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $('#ModalIngresarCabecera').modal('show');
            $('#selectTurnoIngresar').val(document.getElementById('selectTurno').value).trigger('change');
            $('#txtIngresoFecha').val($('#txtFecha').val());
            $('#txtIngresoObservacion').val($('#txtObservacion').val());
        }
    },500);
}

//DETALLE INGRESO DE LINEAS (SE LISTA EL CLASIFICADOR PARA PODER ARMAR LA CABECERA DE LA TABLA)
function ModalGenerarDetalle() {
    ConsultarEstadoRegistro();   
    setTimeout(function () {  
        if (estadoReporte == true) {
        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        return;
    } else {
        var date = new Date();
        $("#ModalGenerarDetalle").modal("show");        
        limpiarDetalle();
        }
    }, 200);
}

function limpiarDetalle() {
    $("#txtTemperatura").val('');
    $("#txtObservacionDetalle").val('');
    var date = new Date();
    document.getElementById('txtHora').value = moment(date).format('YYYY-MM-DDTHH:mm');       
    $("#checkSellado").prop('checked', false);
    $("#LabelEstado").text('NO');   
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
        var fechaHora = $("#txtHora").val();
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
                else if (resultado == 2) {
                    MensajeAdvertencia("¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!", 5);
                } else if (resultado == 100) {
                    MensajeAdvertencia(Mensajes.MensajePeriodo);
                }else {
                    MensajeError("!Error al Guardar/Actualizar los datos¡");
                    return;
                }
                CargarCabecera(0);
                CargarDetalle(0);
                limpiarDetalle();
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
    var fechaFiltro = $('#datetimepicker1').datetimepicker('viewDate');
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ControlTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            fechaDesde: moment(fechaFiltro).format('YYYY-MM-DD'),
            fechaHasta: moment(fechaFiltro).format('YYYY-MM-DD'),
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
            Id: ListaDatosDetalle.Id,
            IdCabecera: ListaDatos.Id
        },
        success: function (resultado) {
            $('#cargac').hide();
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros (Id)");
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == 50) {
                MensajeAdvertencia("Error al eliminar: Registro no encontrado");
            } else {
                $("#ModalEliminarDetalle").modal("hide");
                MensajeCorrecto("Registro Eliminado con Éxito");
                ListaDatosDetalle = [];
                CargarDetalle(0);
            }
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ActulizarDetalle(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#ModalGenerarDetalle").modal('show');            
            $("#txtTemperatura").val(jdata.Temperatura);
            $("#txtObservacionDetalle").val(jdata.Observacion);
            $("#txtHora").val(moment(jdata.HoraVerificacion).format("YYYY-MM-DDTHH:mm"));
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
    DatePicker2();
    $('#ModalIngresarCabecera').modal('show');
    $('#txtIngresoFecha').val($('#txtFecha').val());
    $('#txtIngresoObservacion').val('');
    $('#selectTurnoIngresar').val(document.getElementById('selectTurno').value).trigger('change');
}

function EliminarDetalleNo() {
    $("#ModalEliminarDetalle").modal("hide");
    ListaDatosDetalle = [];
}

function EliminarConfirmarDetalle(jdata) {
    ConsultarEstadoRegistro();
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ListaDatosDetalle = jdata;
            $("#ModalEliminarDetalle").modal("show");
        }
    },500);
}

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
            //date: moment().format("DD-MM-YYYY"),
            format: "DD-MM-YYYY",
            //minDate: model.Fecha,
            maxDate: moment(),
            ignoreReadonly: true
        });
}

//$("#datetimepicker1").on("change.datetimepicker", ({ date, oldDate }) => 
//    CargarCabecera();
//})