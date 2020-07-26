
var model = [];
var chart = [];
var Data = [];
var Fechas = [];
var DesperdicioSolido = [];
var DesperdicioLiquido = [];
var DesperdicioAceites = [];

var options = {
    series: [{
        name: '%',
        data: []
    }],
    
    chart: {
        height: 350,
        type: 'bar',
    },
    plotOptions: {
        bar: {
            dataLabels: {
                position: 'top', // top, center, bottom
            },
            columnWidth: '50%',
            endingShape: 'rounded'
        }
    },
    dataLabels: {
        enabled: true,
        formatter: function (val) {
            return val + "%";
        },
        offsetY: -20,
        style: {
            fontSize: '12px',
            colors: ["#304758"]
        }
    },
    stroke: {
        width: 2
    },

    grid: {
        row: {
            colors: ['#fff', '#f2f2f2']
        }
    },
    xaxis: {
        labels: {
            rotate: -45
        },
        categories: [
        ],
        tickPlacement: 'on'
    },
    yaxis: {
        title: {
            text: '%',
        },
    },
    fill: {
        type: 'gradient',
        gradient: {
            shade: 'light',
            type: "horizontal",
            shadeIntensity: 0.25,
            gradientToColors: undefined,
            inverseColors: true,
            opacityFrom: 0.85,
            opacityTo: 0.85,
            stops: [50, 0, 100]
        },
         //, '#E91E63', '#9C27B0'
    },
    title: {
        text: undefined,
        align: 'center',
        margin: 10,
        offsetX: 0,
        offsetY: 0,
        floating: false,
        style: {
            fontSize: '16px',
            fontWeight: 'bold',
            fontFamily: undefined,
            color: '#263238'
        },
    }

};

divChartSolido = new ApexCharts(document.querySelector("#divChartSolido"), options);
divChartSolido.render();

divChartLiquido = new ApexCharts(document.querySelector("#divChartLiquido"), options);
divChartLiquido.render();

divChartAceite = new ApexCharts(document.querySelector("#divChartAceite"), options);
divChartAceite.render();


function ConsultaKpiPorFecha() {
   // $("#spinnerCargando").prop("hidden", false);
    MostrarModalCargando();
    $('#MensajeRegistros').hide();
    var table = $('#tblTable');
    table.DataTable().clear();    
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
                $("#btnDetalle").prop("disabled", true);
                //  console.log(chart);
                Kpi(null);  
            } else {
                $("#btnDetalle").prop("disabled", false);
                $("#tblTable tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Fecha' },
                    { data: 'OrdenFabricacion' },
                    { data: 'OrdenVenta' },
                    { data: 'Producto' },
                    { data: 'Solido' },
                    { data: 'Liquido' },
                    { data: 'Aceite' },
                    { data: 'Empleados' }
                ];
                resultado.forEach(function (row, i) {
                    row.Fecha = moment(row.Fecha).format('YYYY-MM-DD');
                   
                });
                config.opcionesDT.pageLength = 10;
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
                Kpi(resultado);                             
               
            }
            CerrarModalCargando();
        },
        error: function (resultado) {
          
            MensajeError(Mensajes.Error + resultado.responseText, false);
            CerrarModalCargando();
        }
    });

}

function Kpi(m) {
    if (m != null) {
       // console.log(m);
        DesperdicioSolido = [];
        DesperdicioLiquido = [];
        DesperdicioAceites = [];
        Data = [];
        Fechas = [];

        if ($("#fechaDesde").val() == $("#fechaHasta").val()) {
            $.each(m, function (i, item) {
                Data[i] = item.OrdenFabricacion;
                Fechas[i] = moment(item.Fecha).format("DD-MM-YYYY");
                DesperdicioSolido[i] = item.Solido;
                DesperdicioLiquido[i] = item.Liquido;
                DesperdicioAceites[i] = item.Aceite;
            });
        } else {
            var result = Enumerable.From(m).GroupBy("$.Fecha", null,
                function (key, g) {
                    var result = {
                        Fecha: moment(g.Fecha).format("DD-MM-YYYY"),
                        Solido: (g.Sum("$.Solido") / g.Count()).toFixed(2),
                        Liquido: (g.Sum("$.Liquido") / g.Count()).toFixed(2),
                        Aceite: (g.Sum("$.Aceite") / g.Count()).toFixed(2)


                    }
                    return result;
                }).ToArray();

            $.each(result, function (i, item) {
                Fechas[i] = item.Fecha;
                DesperdicioSolido[i] = item.Solido;
                DesperdicioLiquido[i] = item.Liquido;
                DesperdicioAceites[i] = item.Aceite;
            });
            Data = Fechas;
        }
      

        var categories = Data;
        var series_Solido = [{
            name: 'Solido',
            data: DesperdicioSolido
        }];
        var Serie_Liquido = [
            {
                name: 'Liquido',
                data: DesperdicioLiquido
            }];


        var Serie_Aceite = [
            {
                name: 'Aceite',
                data: DesperdicioAceites
            }];
     


       // divChartSolido.updateTitle(title_solido);
        divChartSolido.updateSeries(series_Solido);
        divChartSolido.updateOptions({
            xaxis: {
                categories: categories
            },
            title: {
                text: 'SOLIDO'
            }
        })

        divChartLiquido.updateSeries(Serie_Liquido);
        divChartLiquido.updateOptions({
            xaxis: {
                categories: categories
            },
            title: {
                text: 'LIQUIDO'
            },
            fill: {
                colors: ['#48FFC1']
            }
        })

        divChartAceite.updateSeries(Serie_Aceite);
        divChartAceite.updateOptions({
            xaxis: {
                categories: categories
            },
            title: {
                text: 'ACEITE'
            },
            fill: {
                colors: ['#FFE348']
            }
        })
    } else {
        divChartSolido.updateSeries([]);
        divChartSolido.updateOptions({
            xaxis: {
                categories: []
            }
        })

        divChartLiquido.updateSeries([]);
        divChartLiquido.updateOptions({
            xaxis: {
                categories: []
            }
        })


        divChartAceite.updateSeries([]);
        divChartAceite.updateOptions({
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
    //ConsultaKpiPorFecha();
});





Array.prototype.unique = function (a) {
    return function () { return this.filter(a) }
}(function (a, b, c) {
    return c.indexOf(a, b + 1) < 0
});