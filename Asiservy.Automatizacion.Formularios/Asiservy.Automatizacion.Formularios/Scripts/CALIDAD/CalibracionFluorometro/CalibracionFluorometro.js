var itemEditar = [];
var json = [];
$(document).ready(function () {
    CargarCabecera();   
});

function MascaraInputs() {    
    json = JSON.parse($('#inpTotalEstandar').val());
    json.forEach(function (row) {
        $('#Estandar_' + row.IdEstandar).val('');
        $('#Estandar_' + row.IdEstandar).inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'max': '99.99' });
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
        url: "../CalibracionFluorometro/CalibracionFluorometroPartial",
        data: {
            fechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
           
                $('#divMostrarCabecera').html(resultado);
            
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera(siAprobar) {
    //HACER UN FOREACH Y COMPARAR LOS DATOS ID DE LOS TEXT CON EL ID GUARDADO EN EL JSON
    $('#cargac').show();
    $.ajax({
        url: "../CalibracionFluorometro/GuardarModificarCalibracionFluor",
        type: "POST",
        data: {
            IdCalibracionFluor: itemEditar.IdCalibracionFluor,
            FechaHora: $("#txtFechaCalibre").val(),
            CoeficienteDeterminacion: $("#txtCoeficiente").val(),
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
                MensajeAdvertencia('Error de Fecha/Hora vacía');
                return;
            } else if (resultado == 4) {
                MensajeAdvertencia('La fecha que intenta ingresar ya existe: <span class="badge badge-danger">' + moment($("#txtFechaCalibre").val()).format('DD-MM-YYYY')+'</span>');
            } else if (resultado == 5) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
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

function ModalIngresoCabecera() {
    Limpiar();
    $("#txtFechaCabecera").prop('disabled', false);
    $('#ModalIngresoCabecera').modal('show');
    var date = new Date();
    $("#txtIngresoFechaCabecera").val(moment(date).format("YYYY-MM-DDTHH:mm"));
    MascaraInputs();
    itemEditar = [];
}

function Limpiar() {
    MascaraInputs();
    $('#txtCoeficiente').val('');
    var date = new Date();
    $('#txtFechaCalibre').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    CambiarMensajeEstado('nada');
}

function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } 
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtFechaCalibre').val() == '') {
        $("#txtFechaCalibre").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtFechaCalibre").css('border', ''); }
    return con;
}

//function EliminarConfirmar() {
//    ConsultarEstadoRegistro();
//    setTimeout(function () {
//        if (estadoReporte == true) {
//            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
//            return;
//        } else {
//            $("#modalEliminarControl").modal("show");
//            $("#myModalLabel").text("¿Desea Eliminar el registro?");
//        }
//    }, 200);
//}

//function EliminarCabeceraSi() {
//    $('#cargac').show();
//    ConsultarEstadoRegistro();
//    setTimeout(function () {
//        if (estadoReporte == true) {
//            $('#cargac').hide();
//            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
//            return;
//        } else {           
//            $.ajax({
//                url: "../CalibracionFluorometro/EliminarHigieneControl",
//                type: "POST",
//                data: {
//                    IdCalibracionFluor: itemEditar.IdCalibracionFluor
//                },
//                success: function (resultado) {
//                    if (resultado == "101") {
//                        window.location.reload();
//                    }
//                    if (resultado == "0") {
//                        MensajeAdvertencia("Falta Parametro IdLavadoCisterna");
//                        $("#modalEliminarControl").modal("hide");
//                        $('#cargac').hide();
//                        return;
//                    } else if (resultado == "1") {
//                        $('#firmaDigital').prop('hidden', true);
//                        $("#modalEliminarControl").modal("hide");
//                        CargarCabecera();
//                        MensajeCorrecto("Registro eliminado con Éxito");
//                        $('#cargac').hide();
//                    } else if (resultado == '2') {
//                        MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
//                        $('#cargac').hide();
//                        return;
//                    }
//                    itemEditar = 0;
//                },
//                error: function (resultado) {
//                    $('#cargac').hide();
//                    MensajeError(resultado.responseText, false);
//                }
//            });
//        }
//    }, 200);
//}

//function EliminarCabeceraNo() {
//    $("#modalEliminarControl").modal("hide");
//}

//function ActualizarCabecera() {
//    ConsultarEstadoRegistro();
//    setTimeout(function () {
//        if (estadoReporte == true) {
//            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
//            return;
//        } else {
//            LimpiarModalIngresoCabecera();
//            var hora = moment(itemEditar.Hora).format('HH:mm');
//            $("#txtIngresoFechaCabecera").val(moment(itemEditar.Fecha +' '+hora).format("YYYY-MM-DD"));
//            $("#txtObservacion").val(itemEditar.Observacion);
//            $('#ModalIngresoCabecera').modal('show');
//        }
//    }, 200);
//}





//function LimpiarModalIngresoCabecera() {
//    $('#txtIngresoFechaCabecera').val(moment($('#txtFecha').val()).format('YYYY-MM-DDTHH:mm'));
//    $('#txtObservacion').val('');
//}





////DETALLE
//function CargarDetalle(idCalibracionFluorDetalle) {
//    $('#cargac').show();
//    $.ajax({
//        url: "../CalibracionFluorometro/CalibracionFluorometroPartial",
//        data: {
//            idCalibracionFluorDetalle: idCalibracionFluorDetalle            
//        },
//        type: "GET",
//        success: function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado == "0") {
//                $("#divMostarTablaDetallesVer").html("No existen registros");
//                $('#firmaDigital').prop('hidden', true);
//            } else {
//                $('#firmaDigital').prop('hidden', false);
//                //$('#divBotonCrearDetalle').prop('hidden', true);
//                $('#divMostarTablaDetallesVer').prop('hidden', false);
//                $('#divMostarTablaDetallesVer').html(resultado);

//            }
//            $('#cargac').hide();
//        },
//        error: function (resultado) {
//            $('#cargac').hide();
//            MensajeError(resultado.responseText, false);
//        }
//    });
//}

//function ModalIngresoDetalle() {
//    LimpiarDetalle();
//    $('#ModalIngresoDetalle').modal('show');
//    var estadoRegistro = 'A';
//    //INICIO AJAX
//    $.ajax({
//        url: "../CalibracionFluorometro/ConsultaHigieneMantActivosPartial",
//        type: "GET",
//        data: {
//            estadoRegistro: estadoRegistro
//        },
//        success: function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado == "0") {
//                $("#divMostarTablaDetalles").html("No existen registros");
//            } else {

//                $("#divMostarTablaDetalles").html(resultado);

//            }
//        },
//        error: function (resultado) {
//            MensajeError(resultado.responseText, false);
//        }
//    });
//    //FIN AJAX
//}

//function LimpiarDetalle() {

//}

//function GuardarDetalle(jdata) {
//    $('#cargac').show();
//    ConsultarEstadoRegistro();
//    setTimeout(function () {
//        if (estadoReporte == true) {
//            $('#cargac').hide();
//            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
//            return;
//        } else {
//            $('#cargac').show();
//            var con = 0;
//            var idControlHigiene = itemEditar[0].IdControlHigiene;
//            jdata.forEach(function (rowMantenimiento) {
//                var sel = document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value;
//                var obs = document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value;
//                var acc = document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value;
//                var idControlMantenimiento = document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value;
//                jdata[con].IdControlHigiene = idControlHigiene;
//                jdata[con].LimpiezaEstado = sel;
//                jdata[con].Observacion = obs;
//                jdata[con].AccionCorrectiva = acc;
//                jdata[con].IdControlDetalle = idControlMantenimiento;
//                con++;
//            });

//            $.ajax({
//                url: "../CalibracionFluorometro/GuardarModificarHigieneControlDetalle",
//                type: "POST",
//                data: {
//                    listaControlDetalle: jdata
//                },
//                success: function (resultado) {
//                    if (resultado == "101") {
//                        window.location.reload();
//                    }
//                    if (resultado == 0) {
//                        MensajeCorrecto('Registro guardado correctamente');
//                    } else if (resultado == 1) {
//                        MensajeCorrecto('Registro actualizado correctamente');
//                    }
//                    $('#ModalIngresoDetalle').modal('hide');
//                    LimpiarCabecera();
//                    itemEditar = 0;
//                    $('#cargac').hide();
//                    CargarCabecera(0);
//                },
//                error: function (resultado) {
//                    $('#cargac').hide();
//                    MensajeError(resultado.responseText, false);
//                }
//            });
//        }
//    }, 200);
//}

//function ActualizarDetalle(jdata) {//LLAMADA DESDE EL PARTIAL CalibracionFluorometroDetallePartial
//    ConsultarEstadoRegistro();
//    setTimeout(function () {
//        if (estadoReporte == true) {
//            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
//            return;
//        } else {
//            ModalIngresoDetalle();
//            setTimeout(function () {
//                jdata.forEach(function (rowMantenimiento) {
//                    document.getElementById('selectLimpiezaEstado-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.LimpiezaEstado;
//                    document.getElementById('txtObservacionDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.Observacion;
//                    document.getElementById('txtACorrectivaDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.AccionCorrectiva;
//                    document.getElementById('txtIdControlDetalle-' + rowMantenimiento.IdMantenimiento).value = rowMantenimiento.IdControlDetalle;
//                });
//            }, 1000);
//        }
//    }, 200);
//}

//function ConsultarEstadoRegistro() {
//    $.ajax({
//        url: "../CalibracionFluorometro/ConsultarCalibracionFluorometroJson",
//        data: {
//            idCalibracionFluor: itemEditar.IdCalibracionFluor
//        },
//        type: "GET",
//        success: function (resultado) {
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            estadoReporte = resultado.EstadoReporte;
//            CambiarMensajeEstado(resultado.EstadoReporte);
//        },
//        error: function (resultado) {
//            MensajeError(resultado.responseText, false);
//        }
//    });
//}

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

$(function () {
    var start = moment();
    var end = moment();
    var mesesLetras = {
        '01': "Enero",
        '02': "Febrero",
        '03': "Marzo",
        '04': "Abril",
        '05': "Mayo",
        '06': "Junio",
        '07': "Julio",
        '08': "Agosto",
        '09': "Septiembre",
        '10': "Octubre",
        '11': "Noviembre",
        '12': "Diciembre"
    }

    function cb(start, end) {
        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));

        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 60
        },
        minDate: moment("01/10/2019", "DD/MM/YYYY"),
        maxDate: moment(),
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Mes actual (hasta hoy)': [moment().startOf('month'), moment()],
            'Último mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "fromLabel": "De",
            "toLabel": "a",
            "customRangeLabel": "Personalizada",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, cb);
    cb(start, end);
});