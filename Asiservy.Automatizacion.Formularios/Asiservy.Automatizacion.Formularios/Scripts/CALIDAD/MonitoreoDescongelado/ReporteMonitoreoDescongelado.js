//$(document).ready(function () {

//});

var model = [];

function FiltrarAprobadosFecha() {
    $('#divPartialControl').html('');
    $("#spinnerCargando").prop("hidden", false);
    $("#MensajeRegistros").html('');

    $.ajax({
        url: "../MonitoreoDescongelado/ReporteMonitoreoDescongeladoControlPartial",
        type: "GET",
        data: {
            FechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val()
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
                $("#MensajeRegistros").html(Mensajes.SinRegistros);
                $('#divPartialControl').html('');

            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControl').html(resultado);
            }
            $("#spinnerCargando").prop("hidden", true);
            $("#divDateRangePicker").prop('hidden', false);
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(Mensajes.Error + resultado.responseText, false);
        }
    });

}

function SeleccionarBandeja(Control) {
    //console.log(Control);
    model = Control;

    //if (!model.EstadoReporte) {
    //    MensajeAdvertencia("Reporte se encuentra pendiente.");
    //    return;
    //}
    $("#btnImprimir").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#btnConsultar").prop("hidden", true);
    $("#divMensaje").html('');
    $("#divCabeceras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    $("#lblLomos").html('');
    $("#divDetalle").prop("hidden", false);

    // console.log(model);

    $("#divMensaje").html('');
    if (model.Lomos) {
        $("#lblLomos").html("<i class='fas fa-check-circle' style='color:#1cc88a'></i>");
    }
    if (model.Latas) {
        $("#lblLatas").html("<i class='fas fa-check-circle' style='color:#1cc88a'></i>");
    }

    if (model.EstadoReporte) {
        $("#txtUsuarioAprobacion").html(model.AprobadoPor);
        $("#txtFechaAprobacion").html(moment(model.FechaAprobacion).format("YYYY-MM-DD HH:mm"));
    } else {
        $("#txtUsuarioAprobacion").html('');
        $("#txtFechaAprobacion").html('');
    }
    $("#txtUsuarioCreacion").html(model.UsuarioIngresoLog);
    $("#txtFechaCreacion").html(moment(model.FechaIngresoLog).format("YYYY-MM-DD HH:mm"));
  


    CargarControlDetalle();
}

function Atras() {
    $("#btnAtras").prop("hidden", true);
    $("#btnImprimir").prop("hidden", false);
    $("#divCabeceras").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    $("#btnConsultar").prop("hidden", false);

}


function CargarControlDetalle() {
    $("#divTableDetalle").html('');
    $("#spinnerCargandoDetalle").prop("hidden", false);
    MostrarModalCargando();
    $.ajax({
        url: "../MonitoreoDescongelado/ReporteMonitoreoDescongeladoPartial",
        type: "GET",
        data: {
            Fecha: model.Fecha,
            Turno: model.Turno
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html(Mensajes.SinRegistros);
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
            
            }
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
            CerrarModalCargando();

        }
    });
}

function validarImg(rotacion, id, imagen) {

    $('#' + id).rotate(rotacion);
    //document.getElementById(id).style.height = "0px";
    //document.getElementById(id).style.width = "0px";

    var img = new Image();
    img.onload = function () {
        //  alert(this.width + 'x' + this.height);
        var ancho = this.width;
        var alto = this.height;
        if (ancho < alto) {
            document.getElementById(id).style.height = "250px";
            document.getElementById(id).style.width = "150px";
        } else {
            document.getElementById(id).style.height = "150px";
            document.getElementById(id).style.width = "250px";
        }

    }
    img.src = "/Content/Img/" + imagen;

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
        FiltrarAprobadosFecha();
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 61
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
    FiltrarAprobadosFecha();
});



function printDiv() {
    window.print();
}