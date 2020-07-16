var ListadoGeneral = [];

var options = {
    series: [
        {
            name: "High - 2013",
            type: 'line',
            data: [28, 29, 33, 36, 32, 32, 33]
        },
        {
            name: "Low - 2013",
            type: 'line',
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 350,
        type: 'line',
        dropShadow: {
            enabled: true,
            color: '#000',
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2
        },
        toolbar: {
            show: false
        }
    },
    colors: ['#005FFF', '#B548FF','#70F1D7'],
    dataLabels: {
        enabled: false,
    },
    stroke: {
        curve: 'smooth'
    },
    title: {
        text: 'LOMOS',
        align: 'left'
    },
    grid: {
        borderColor: '#e7e7e7',
        row: {
            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.5
        },
    },
    markers: {
        size: 1
    },
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
            text: 'Month'
        }
    },
    yaxis: {
        title: {
            text: 'Temperature'
        }
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: -25,
        offsetX: -5
    }
};


var chartRendimientoLomo = new ApexCharts(document.querySelector("#chartRendimientoLomo"), options);
chartRendimientoLomo.render();


var chartRendimientoMiga = new ApexCharts(document.querySelector("#chartRendimientoMiga"), options);
chartRendimientoMiga.render();


var options = {
    series: [{
        name:'Rendimiento',
        data: [21, 22,1]
    }],
    chart: {
        height: 350,
        type: 'bar',
        //events: {
        //    click: function (chart, w, e) {
        //        // console.log(chart, w, e)
        //    }
        //}
    },
    colors: ['#005FFF', '#B548FF', '#70F1D7'],
    plotOptions: {
        bar: {
            columnWidth: '45%',
            distributed: true
        }
    },
    dataLabels: {
        enabled: false
    },
    legend: {
        show: false
    },
    xaxis: {
        categories: [
            ['REAL'],
            ['ESTANDAR'],
            ['DIFERENCIA'],
           
        ],
        labels: {
            style: {
                colors: ['#005FFF', '#70F7D7'],
                fontSize: '12px'
            }
        }
    }
};

var chartRendimientoGeneralLomo = new ApexCharts(document.querySelector("#chartRendimientoGeneralLomo"), options);
chartRendimientoGeneralLomo.render();

var chartRendimientoGeneralMiga = new ApexCharts(document.querySelector("#chartRendimientoGeneralMiga"), options);
chartRendimientoGeneralMiga.render();

var chartRendimientoGeneralTotal = new ApexCharts(document.querySelector("#chartRendimientoGeneralTotal"), options);
chartRendimientoGeneralTotal.render();


$(document).ready(function () {
    CargarReporteAvance();
});


function CargarReporteAvance() {
    var txtFecha = $('#txtFecha').val();
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }
    $('#btnConsultar').prop("disabled", true);
    $('#DivTableReporteControlAvance').html("");
    $('#divMensaje').html("");
    
    MostrarModalCargando();
    $.ajax({
        url: "../Avance/ReporteRendimientoLotePartial",
        type: "GET",
        data: {
            ddFecha: txtFecha,
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#btnConsultar').prop("disabled", false);
            if (resultado == "1") {
                $('#divMensaje').html("No existen Registros");
                $("#spinnerCargando").prop("hidden", true);
                $("#divChart").prop("hidden", true);
                CerrarModalCargando();


            } else {
                $("#spinnerCargando").prop("hidden", true);
                $('#DivTableReporteControlAvance').html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
                $("#divChart").prop("hidden", false);

                var count = 0;
                var TotalToneladas=0;
                var RealLomo = [];
                var EstandarLomo = [];
                var DiferenciaLomo = [];

                var RealMiga = [];
                var EstandarMiga = [];
                var DiferenciaMiga = [];

                var RealLomoGeneral =0;
                var EstandarLomoGeneral = 0;

                var RealMigaGeneral = 0;
                var EstandarMigaGeneral = 0;

                var RealTotalGeneral = 0;
                var EstandarTotalGeneral = 0;

                var Lote = [];
               // console.log(ListadoGeneral);
                ListadoGeneral.forEach(function (row, i) {
                    RealLomo[i] = row.KiloRealLomoPorcentaje.toFixed(2);
                    EstandarLomo[i] = row.KiloStdLomoPorcentaje.toFixed(2);
                    DiferenciaLomo[i] = (row.KiloRealLomoPorcentaje - row.KiloStdLomoPorcentaje).toFixed(2);

                    RealMiga[i] = row.KiloRealMigaPorcentaje.toFixed(2);
                    EstandarMiga[i] = row.KiloStdMigaPorcentaje.toFixed(2);
                    DiferenciaMiga[i] = (row.KiloRealMigaPorcentaje - row.KiloStdMigaPorcentaje).toFixed(2);

                    RealLomoGeneral = RealLomoGeneral+row.KiloRealLomoPorcentaje;
                    EstandarLomoGeneral = EstandarLomoGeneral+ row.KiloStdLomoPorcentaje;

                    RealMigaGeneral = RealMigaGeneral+row.KiloRealMigaPorcentaje;
                    EstandarMigaGeneral = EstandarMigaGeneral+ row.KiloStdMigaPorcentaje;

                    RealTotalGeneral = RealTotalGeneral +(row.KiloRealLomoPorcentaje + row.KiloRealMigaPorcentaje);
                    EstandarTotalGeneral = EstandarTotalGeneral+ (row.KiloStdLomoPorcentaje + row.KiloStdMigaPorcentaje);

                    Lote[i] = row.Lote;
                    TotalToneladas = TotalToneladas + row.PesoLote
                    count = count + 1;

                });
                //console.log(RealLomo);
                //console.log(RealMiga);

                $("#txtTotalToneladas").html(TotalToneladas+" Kl");

                var _serieLomo = [{
                        name: "Real",
                    data: RealLomo
                },
                    {
                        name: "Estandar",
                        data: EstandarLomo
                    },
                    {
                        name: "Diferencia",
                        type:"column",
                        data: DiferenciaLomo
                    }];


                var _serieMiga = [{
                    name: "Real",
                    data: RealMiga
                },
                    {
                        name: "Estandar",
                        data: EstandarMiga
                    },
                    {
                        name: "Diferencia",
                        type: "column",
                        data: DiferenciaMiga
                    }];

                chartRendimientoLomo.updateSeries(_serieLomo)
                chartRendimientoLomo.updateOptions({
                     xaxis: {
                    categories: Lote,
                    title: {
                        text: 'Lote'
                    }
                    },
                    yaxis: {
                        title: {
                            text: 'Rendimiento %'
                        }
                    }
                    })
                chartRendimientoMiga.updateSeries(_serieMiga)
                chartRendimientoMiga.updateOptions({
                    title: {
                        text: 'MIGAS',
                        align: 'left'
                    },
                    xaxis: {
                        categories: Lote,
                        title: {
                            text: 'Lote'
                        }
                    },
                    yaxis: {
                        title: {
                            text: 'Rendimiento %'
                        }
                    }
                })

                //console.log(RealLomoGeneral);

                var diferenciaGeneral = ((RealLomoGeneral / count) - (EstandarLomoGeneral / count)).toFixed(2);
                RealLomoGeneral = (RealLomoGeneral / count).toFixed(2);
                EstandarLomoGeneral = (EstandarLomoGeneral / count ).toFixed(2);
                var _serieRendimiento = [{
                    name: 'Rendimiento',
                    data: [RealLomoGeneral, EstandarLomoGeneral, diferenciaGeneral]
                }];
                chartRendimientoGeneralLomo.updateSeries(_serieRendimiento)
                chartRendimientoGeneralLomo.updateOptions({
                    title: {
                        text: 'Rendimiento General Lomos',
                        align: 'left'
                    },
                })

                diferenciaGeneral = ((RealMigaGeneral / count) - (EstandarMigaGeneral / count)).toFixed(2);
                RealMigaGeneral = (RealMigaGeneral / count).toFixed(2);
                EstandarMigaGeneral = (EstandarMigaGeneral / count).toFixed(2);
                _serieRendimiento = [{
                    name: 'Rendimiento',
                    data: [RealMigaGeneral, EstandarMigaGeneral, diferenciaGeneral]
                }];
                chartRendimientoGeneralMiga.updateSeries(_serieRendimiento)
                chartRendimientoGeneralMiga.updateOptions({
                    title: {
                        text: 'Rendimiento General Migas',
                        align: 'left'
                    },
                })

                diferenciaGeneral = ((RealTotalGeneral / count) - (EstandarTotalGeneral / count)).toFixed(2);
                RealTotalGeneral = (RealTotalGeneral / count).toFixed(2);
                EstandarTotalGeneral = (EstandarTotalGeneral / count).toFixed(2);
               _serieRendimiento = [{
                    name: 'Rendimiento',
                    data: [RealTotalGeneral, EstandarTotalGeneral, diferenciaGeneral]
                }];
                chartRendimientoGeneralTotal.updateSeries(_serieRendimiento)
                chartRendimientoGeneralTotal.updateOptions({
                    title: {
                        text: 'Rendimiento General Total',
                        align: 'left'
                    },
                })

                CerrarModalCargando();
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
          CerrarModalCargando();
        }
    });
}
