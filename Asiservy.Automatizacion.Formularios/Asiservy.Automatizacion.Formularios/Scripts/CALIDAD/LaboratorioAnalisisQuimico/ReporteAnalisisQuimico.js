var itemSeleccionar = [];
$(document).ready(function () {
    CargarCabecera();
});

var turno = ['', "A", "B","C","D","E","F"];

function CargarCabecera() {
    Atras();
    $('#lblMostrarFecha').text('');
    $('#lblMostrarHora').text('');
    $('#lblMostrarObservacion').text('');
    $('#cargac').show();

    if ($("#fechaDesde").val() == '' || $("#fechaHasta").val() == '') {
        var date = new Date();
        var shortDate = moment(date).format('YYYY-MM-DD');
        shortDate += ' 23:59';
        $("#fechaDesde").val(moment(date).format('YYYY-MM-DD'));
        $("#fechaHasta").val(moment(shortDate).format('YYYY-MM-DDTHH:mm'));
    } else {
        var d = moment($("#fechaHasta").val()).format('YYYY-MM-DD');
        d += ' 23:59';
        $("#fechaHasta").val(moment(d).format('YYYY-MM-DDTHH:mm'));
    }

    $.ajax({
        url: "../LaboratorioAnalisisQuimico/ReporteAnalisisQuimicoPartial",
        data: {
            fechaDesde: $("#fechaDesde").val(),
            fechaHasta: $("#fechaHasta").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function SeleccionarCabecera(jdata) {
    if (jdata.Turno != null || jdata.Turno != '') {
        $('#lblMostrarHora').text(turno[jdata.Turno]);
        $('#lblMostrarHoraM').text(turno[jdata.Turno]);
    }
    itemSeleccionar = [];
    $('#cargac').show();
    var op = 2;
    itemSeleccionar = jdata;

    $.ajax({
        url: "../LaboratorioAnalisisQuimico/ReporteAnalisisQuimicoDetallePartial",//MUESTRO EL DETALLE DE LA FILA SELECCIONADA
        data: {
            fechaControl: jdata.Fecha,
            fechaAsignada: jdata.FechaAsignada,
            turno:jdata.Turno,
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divBotones').prop('hidden', true);
                $('#divTurno').prop('hidden', true);
                $("#divMostarTablaCabecera").prop('hidden', false);
                $("#divCardMostrarDetalle").prop('hidden', true);
                MensajeAdvertencia('No existen registro de DETALLE');
            } else {
                $("#divMostarTablaCabecera").prop('hidden', true);
                $("#divCardMostrarDetalle").prop('hidden', false);
                $('#divBotones').prop('hidden', false);
                $('#divTurno').prop('hidden', false);
                $("#divMostarTablaDetalle").html(resultado);
            }
            $('#lblMostrarFecha').text(moment(jdata.Fecha).format('DD-MM-YYYY'));
            $('#lblMostrarFechaM').text(moment(jdata.Fecha).format('DD-MM-YYYY'));
            if (jdata.ObservacionCtrl != null) {
                $('#lblMostrarObservacion').text('\u00a0' + jdata.ObservacionCtrl.toUpperCase());
            }
            $('#txtUsuarioCreacion').text('\u00a0' + jdata.UsuarioIngresoLog.toUpperCase());
            $('#txtFechaCreacion').text('\u00a0' + moment(jdata.FechaIngresoLog).format('DD-MM-YYYY'));
            if (jdata.AprobadoPor == null) {
                jdata.AprobadoPor = '';
            }

            if (jdata.FechaAprobado != null) {
                jdata.FechaAprobado = moment(jdata.FechaAprobado).format('DD-MM-YYYY HH:mm');
            } else if (jdata.FechaAprobado == null) {
                jdata.FechaAprobado = '';
            }
            $('#txtUsuarioAprobacion').text('\u00a0' + jdata.AprobadoPor);
            $('#txtFechaAprobacion').text('\u00a0' + jdata.FechaAprobado);
            if (jdata.ObservacionCtrl == null) {
                jdata.ObservacionCtrl = '';
            }
            $('#lblObservacion').text('\u00a0' + jdata.ObservacionCtrl);
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function Atras() {
    itemSeleccionar = [];
    $('#cargac').show();
    $('#divBotones').prop('hidden', true);
    $('#divTurno').prop('hidden', true);
    $("#divMostarTablaCabecera").prop('hidden', false);
    $("#divCardMostrarDetalle").prop('hidden', true);
    $("#divMostarTablaDetalle").html('');
    $('#cargac').hide();
}

function printDiv() {   
    window.print();
}

function validarImg(rotacion, id, imagen) {
    $('#' + id).rotate(parseInt(rotacion));
    var img = new Image();
    //img.onload = function () {
    document.getElementById(id).style.borderRadius = "20px";
    document.getElementById(id).style.height = "270px";
    document.getElementById(id).style.width = "270px";
    //}
    img.src = $('#btnPath').val() + imagen;
}

function exportTableToExcel(tableID, filename = '') {
    let file = new Blob([$('#tblImprimir').html()], { type: "application/vnd.ms-excel" });
    let url = URL.createObjectURL(file);
    let a = $("<a />", {
        href: url,
        download: "ReporteAnalisisQuimicoPrecoccion.xls"
    }).appendTo("body").get(0).click();
    e.preventDefault();

}

//FECHA DataRangePicker
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
            'Futuro': [moment(), moment().add('1', 'month')],
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