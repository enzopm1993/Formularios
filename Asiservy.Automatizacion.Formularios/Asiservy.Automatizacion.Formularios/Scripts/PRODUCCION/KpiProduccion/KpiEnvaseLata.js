
var model = [];
var chart = [];
var Data = [];
var Fechas = [];
var DesperdicioSolido = [];
var DesperdicioLiquido = [];
var DesperdicioAceites = [];

var options = {
    series: [{
        name: 'Solido',
        data: DesperdicioSolido
    }, {
        name: 'Liquido',
        data: DesperdicioLiquido
    }, {
        name: 'Aceite',
        data: DesperdicioAceites
    }],
    chart: {
        type: 'bar',
        height: 350
    },
    plotOptions: {
        bar: {
            horizontal: false,
            columnWidth: '55%',
            endingShape: 'rounded'
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    xaxis: {
        categories: Data,
    },
    yaxis: {
        title: {
            text: '% (Desperdicio)'
        }
    },
    fill: {
        opacity: 1
    },
    tooltip: {
        y: {
            formatter: function (val) {
                return val + "%"
            }
        }
    }
};
chart = new ApexCharts(document.querySelector("#divChart"), options);
chart.render();


function ConsultaKpiPorFecha() {
    $("#spinnerCargando").prop("hidden", false);
    $('#MensajeRegistros').hide();

    $.ajax({
        url: "../KpiProduccion/ConsultaKpiEnvaseLatas",
        type: "GET",
        data: {
            FechaDesde: $("#fechaDesde").val(),
            FechaHasta: $("#fechaHasta").val(),
            Turno: $("#selectTurno").val(),
            Linea: $("#selectLinea").val()
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
                $("#MensajeRegistros").html("No existen registros.");
                $("#divChart").html("");
                //  console.log(chart);
                Kpi(null);  
                
            } else {
                Kpi(resultado);             
            }
            $("#spinnerCargando").prop("hidden", true);
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(Mensajes.Error + resultado.responseText, false);
        }
    });

}

function Kpi(model) {
    if (model != null) {
        //console.log(model);
        DesperdicioSolido = [];
        DesperdicioLiquido = [];
        DesperdicioAceites = [];
        Data = [];
        $.each(model, function (i, item) {
            Data[i] = item.OrdenFabricacion;
            Fechas[i] = moment(item.Fecha).format("DD-MM-YYYY");
            DesperdicioSolido[i] = item.Solido;
            DesperdicioLiquido[i] = item.Liquido;
            DesperdicioAceites[i] = item.Aceite;
        });
        Fechas = Fechas.unique();
        //console.log(Fechas.length);
        if (Fechas.length > 1) {
            Data = Fechas;
        }

        var categories = Data;

        var series = [{
            name: 'Solido',
            data: DesperdicioSolido
        }, {
            name: 'Liquido',
            data: DesperdicioLiquido
        }, {
            name: 'Aceite',
            data: DesperdicioAceites
        }];

        chart.updateSeries(series);
        chart.updateOptions({
            xaxis: {
                categories: categories
            }
        })
    } else {
        chart.updateSeries([]);
        chart.updateOptions({
            xaxis: {
                categories: []
            }
        })

    }
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
        ConsultaKpiPorFecha();
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
    ConsultaKpiPorFecha();
});





Array.prototype.unique = function (a) {
    return function () { return this.filter(a) }
}(function (a, b, c) {
    return c.indexOf(a, b + 1) < 0
});