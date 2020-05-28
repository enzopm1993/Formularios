var itemEditar = [];
var json = [];
var estadoReporte = [];
$(document).ready(function () {
    CargarCabecera(1);   
});

function MascaraInputs() {    
    json = JSON.parse($('#inpTotalEstandar').val());//Lista de Estandares enviados mendiante ViewBag txt id="inpTotalEstandar" 
    json.forEach(function (row) {
        $('#Estandar_' + row.IdEstandar).val('');
        $('#Estandar_' + row.IdEstandar).css('border', '');
        $('#Estandar_' + row.IdEstandar).inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, 'max': '99.99' });
    });
}

function CargarCabecera(op) {
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
            FechaHasta: $("#fechaHasta").val(),
            op:op
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
    var detalleCalibracion = [];
    var idCalibracionFluor=0;
    json.forEach(function (row) {//json= Variable listada desde el mantenimiento(ESTANDAR) y recivida en ViewBag---txt id="inpTotalEstandar" 
        var d = {};
        if (itemEditar!=null) {
            itemEditar.forEach(function (subItem) {//Envio la lista filtrada por idCalibracionFluor en el boton Editar
                if (row.IdEstandar == subItem.IdEstandar) {
                    d.IdCalibracionFluorDetalle = subItem.IdCalibracionFluorDetalle;
                    d.IdCalibracionFluor = subItem.IdCalibracionFluor;
                    idCalibracionFluor = subItem.IdCalibracionFluor;
                }
            });   
        }            
        d.IdEstandar = row.IdEstandar;
        d.ValorEstandar = $('#Estandar_' + row.IdEstandar).val();
        d.FechaIngresoLog = "";
        d.EstaoRegistro = "";
        d.TerminalIngresoLog = "";
        d.UsuarioIngresoLog = "";
        d.FechaModificacionLog = "";
        d.TerminalModificacionLog = "";
        d.UsuarioModificacionLog = "";        
        detalleCalibracion.push(d);
    });
    $('#cargac').show();
    $.ajax({
        url: "../CalibracionFluorometro/GuardarModificarCalibracionFluor",
        type: "POST",
        data: {
            IdCalibracionFluor: idCalibracionFluor,
            FechaHora: $("#txtFechaCalibre").val(),
            CoeficienteDeterminacion: $("#txtCoeficiente").val(),
            siAprobar: siAprobar,
            detalle: detalleCalibracion
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
                MensajeAdvertencia('La fecha que intenta ingresar ya existe: <span class="badge badge-danger">' + moment($("#txtFechaCalibre").val()).format('DD-MM-YYYY') + '</span>');
                $('#cargac').hide();
                return;
            } else if (resultado == 5) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            }
            $('#ModalIngresoCabecera').modal('hide');
            $('#divMostarTablaDetalle').prop('hidden', false);
            itemEditar = 0;
            $('#cargac').hide();
            CargarCabecera(1);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ModalIngresoCabecera() {
    Limpiar();
    $('#txtFechaCalibre').css('border', '');
    $('#ModalIngresoCabecera').modal('show');
    MascaraInputs();
    itemEditar = [];
}

function Limpiar() {
    MascaraInputs();
    $('#txtCoeficiente').val('');
    var date = new Date();
    $('#txtFechaCalibre').val(moment(date).format('YYYY-MM-DDTHH:mm'));
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
    if ($('#txtFechaCalibre').val() == '') {
        $("#txtFechaCalibre").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtFechaCalibre").css('border', ''); }
   
    json.forEach(function (row) {
        if ($('#Estandar_' + row.IdEstandar).val() == '') {
            $('#Estandar_' + row.IdEstandar).css('border', '1px dashed red');
            con = 1;
        } else { $('#Estandar_' + row.IdEstandar).css('border', ''); }
    });
    return con;
}

function EliminarConfirmar(jdata) {
    ConsultarEstadoRegistro(jdata[0].IdCalibracionFluor);
    setTimeout(function () {
        if (estadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            itemEditar = jdata;
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    }, 200);
}

function EliminarCabeceraSi() {
    $('#cargac').show();
    var idCalibracionFluor = 0;
    itemEditar.forEach(function (subItem) {//OBTENHGO EL ID DE LA CABECERA DE LA LISTA JSON ENVIADA DESDE EL PARTIAL     
        idCalibracionFluor = subItem.IdCalibracionFluor;
    });   
    $.ajax({
        url: "../CalibracionFluorometro/EliminarCalibracionFluor",
        type: "POST",
        data: {
            IdCalibracionFluor: idCalibracionFluor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdCalibracionFluor");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera(1);
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

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera(jdata) {
    ConsultarEstadoRegistro(jdata[0].IdCalibracionFluor);
    setTimeout(function () {
        if (estadoReporte == true) {            
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            ModalIngresoCabecera();            
            jdata.forEach(function (row) {
                $('#txtFechaCalibre').val(row.FechaHora); 
                $('#txtCoeficiente').val(row.CoeficienteDeterminacion);
                $('#Estandar_' + row.IdEstandar).val(row.ValorEstandar);
            });
            itemEditar = jdata;
        }
    }, 200);
}

function ConsultarEstadoRegistro(idCalibracionFluor) {
    $.ajax({
        url: "../CalibracionFluorometro/ConsultarCalibracionFluorometroJson",
        data: {
            idCalibracionFluor: idCalibracionFluor
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado.EstadoReporte==true) {
                CargarCabecera(1);
            }
            estadoReporte = resultado.EstadoReporte;
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
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