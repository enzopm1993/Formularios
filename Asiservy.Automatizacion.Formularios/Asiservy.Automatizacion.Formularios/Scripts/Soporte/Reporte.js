$(function () {

    var fechaMuestraDesdeTitulo = '';
    var fechaMuestraHastaTitulo = '';
    var fechaMuestraDesdeTituloCerrados = '';
    var fechaMuestraHastaTituloCerrados = '';
    var optionsDepartamentos = {
        colors: ['#b84644'],
        chart: {
            height: 350,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                distributed: true,
                columnWidth: '50%',
                endingShape: 'rounded',
                colors: {
                    ranges: [{
                        from: 0,
                        to: 0,
                        color: undefined
                    }],
                    backgroundBarColors: [],
                    backgroundBarOpacity: 1,
                    backgroundBarRadius: 0,
                }
            }
        },
        dataLabels: {
            enabled: true
        },
        stroke: {
            width: 2
        },
        series: [{
            name: 'Tickets',
            data: [0]
        }],
        grid: {
            row: {
                colors: ['#fff', '#f2f2f2']
            }
        },
        xaxis: {
            labels: {
                rotate: -45
            },
            categories: ['DEPARTAMENTO'],
        },
        yaxis: {
            title: {
                text: 'Tickets',
            },
            labels: {
                formatter: function (val) {
                    return val.toFixed(0)
                }
            }
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
        }
    }


    var chartDepartamentos = new ApexCharts(document.querySelector("#chartPorDepartamento"), optionsDepartamentos);    
    chartDepartamentos.render();



    var optionsCumplidos = {
        series: [0, 0],
        chart: {
            type: 'donut',
        },
        fill: {
            colors: ['#FF0000','#00FF3E']
        },
        colors: ['#FF0000','#00FF3E'],
        labels: ['Tickets abiertos', 'Tickets cerrados'],
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chartCumplimiento = new ApexCharts(document.querySelector("#chartCumplimiento"), optionsCumplidos);
    chartCumplimiento.render();

    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
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

        fechaMuestraDesdeTitulo = fechaMuestraDesde;
        fechaMuestraHastaTitulo = fechaMuestraHasta;

   

       
        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
    }


    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        minDate: moment("01/06/2020", "DD/MM/YYYY"),
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
    

    var startCerrados = moment();
    var endCerrados = moment();

    function cbCerrados(start, end) {
        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesdeCerrados").val(start.format('YYYY-MM-DD'));
        $("#fechaHastaCerrados").val(end.format('YYYY-MM-DD'));
        fechaMuestraDesdeTituloCerrados = fechaMuestraDesde;
        fechaMuestraHastaTituloCerrados = fechaMuestraHasta;
        $('#reportrangeCerrados span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);       
    }


    $('#reportrangeCerrados').daterangepicker({
        startDate: startCerrados,
        endDate: endCerrados,
        minDate: moment("01/06/2020", "DD/MM/YYYY"),
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
    }, cbCerrados);

    cbCerrados(start, end);


    var table = $("#tblDataTable");

    $("#generarReporte").click(function () {
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();

        var fechaDesde = $("#fechaDesde").val();
        var fechaHasta = $("#fechaHasta").val();
        var metodo = "P";

        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);
        var diffDays = f2.diff(f1, 'days');


        $("#generarReporte").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarReporte").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Soporte/ObtenerSoportesReporte",
            type: "GET",
            data: {
                'fechaIni': fechaDesde,
                'fechaFin': fechaHasta
            },
            success: function (resultado) {


                $("#generarReporte").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarReporte").removeClass("btnWait");

                if (fechaMuestraDesdeTitulo == fechaMuestraHastaTitulo) {
                    $("#titleTabFecha").html(' - ' + fechaMuestraHastaTitulo);
                    $("#titleTabFecha2").html(' - ' + fechaMuestraHastaTitulo);
                } else {
                    $("#titleTabFecha").html(' - ' + fechaMuestraDesdeTitulo + ' - ' + fechaMuestraHastaTitulo);
                    $("#titleTabFecha2").html(' - ' + fechaMuestraDesdeTitulo + ' - ' + fechaMuestraHastaTitulo);
                }

                $("#tblDataTable tbody").empty();

                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'ID' },
                    { data: 'Departamento' },
                    { data: 'Usuario' },
                    { data: 'Ticket' },
                    { data: 'Asunto' },
                    { data: 'Estado' },
                    { data: 'AgenteAsignado' },
                    { data: 'FechaCreacion' },
                    { data: 'HoraCreacion' },
                    { data: 'FechaAsignacion' },
                    { data: 'HoraAsignacion' },
                    { data: 'FechaCierre' },
                    { data: 'HoraCierre' },
                    { data: 'FechaInicioSoporte' },
                    { data: 'HoraInicioSoporte' },
                    { data: 'FechaFinSoporte' },
                    { data: 'HoraFinSoporte' },
                    { data: 'TiempoTicket' },
                    { data: 'TiempoAsignacionCierre' },
                    { data: 'TiempoSoporte' },
                    { data: 'TiempoCreacionFinSoporte' },
                    { data: 'TiempoTicketLetras' }
                   
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado.DataPlana);
                table.DataTable().draw();


                var permisosTotalLabels = [];
                var permisosTotalCantidades = [];
                var coloresDeps = [];


                $.each(resultado.TotalTicketsPorDep, function (i, it) {
                    permisosTotalLabels[i] = it.Descripcion;
                    permisosTotalCantidades[i] = it.Total;

                    $.each(resultado.DepartamentoColores, function (i0, dep) {
                        if (dep.Codigo == it.Descripcion) {
                            coloresDeps[i] = dep.Descripcion
                        }
                    });  
                });  


                

                chartDepartamentos.updateOptions({
                    colors: coloresDeps,
                    xaxis: {
                        categories: permisosTotalLabels,
                    }
                });
                chartDepartamentos.updateSeries([{
                    name: 'Tickets',
                    data: permisosTotalCantidades
                }]);


                var tkAbiertos = 0;
                var tkCerrados = 0;
                $.each(resultado.DataPlana, function (i, it) {
                    if (it.Estado == 'Abierto') {
                        tkAbiertos = tkAbiertos + 1;
                    }
                    if (it.Estado == 'Cerrado') {
                        tkCerrados = tkCerrados + 1;
                    }
                });  
                chartCumplimiento.updateSeries([tkAbiertos,tkCerrados]);


                var pivot_agente = new WebDataRocks({
                    container: "#wdr-component-agente",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCreacion.Year"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Month"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Day"
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "AgenteAsignado"
                                },
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "Estado"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count"
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                var pivot_depa = new WebDataRocks({
                    container: "#wdr-component-depa",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCreacion.Year"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Month"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Day"
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "AgenteAsignado"
                                },                                
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "Estado"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count"
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },                   
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                var pivot_dias_agente = new WebDataRocks({
                    container: "#wdr-component-dias-agente",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCreacion.Year"
                                },
                                {
                                    "uniqueName": "Estado",
                                    "filter": {
                                        "members": [
                                            "Estado.Cerrado"
                                        ]
                                    }
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Month"
                                },
                                {
                                    "uniqueName": "FechaCreacion.Day"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count",
                                    "availableAggregations": [
                                        "count",
                                        "distinctcount"
                                    ]
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                $("#txtTkTotales").html(resultado.Totales.Totales);
                $("#txtTkAbiertos").html(resultado.Totales.Abiertos);
                $("#txtTkCerrados").html(resultado.Totales.Cerrados);
                $("#txtTkSinAsignar").html(resultado.Totales.SinAsignar);
            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarReporte").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarReporte").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

            }
        });

        return false;

    });

   


    var chartDepartamentos_cerrados= new ApexCharts(document.querySelector("#chartPorDepartamento-cerrados"), optionsDepartamentos);
    chartDepartamentos_cerrados.render();

    var tableCerrados = $("#tblDataTableCerrados");
    $("#generarReporteCerrados").click(function () {
        tableCerrados.DataTable().destroy();
        tableCerrados.DataTable().clear();
        tableCerrados.DataTable().draw();

        var fechaDesde = $("#fechaDesdeCerrados").val();
        var fechaHasta = $("#fechaHastaCerrados").val();
       
        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);
        var diffDays = f2.diff(f1, 'days');


        $("#generarReporteCerrados").attr('href', "javascript:void(0)");
        $("#iconSearchCerrados").removeClass(iconSearch);
        $("#iconSearchCerrados").addClass(iconLoader);
        $("#generarReporteCerrados").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Soporte/ObtenerSoportesReporteCerrados",
            type: "GET",
            data: {
                'fechaIni': fechaDesde,
                'fechaFin': fechaHasta
            },
            success: function (resultado) {


                $("#generarReporteCerrados").attr('href', "#");
                $("#iconSearchCerrados").removeClass(iconLoader);
                $("#iconSearchCerrados").addClass(iconSearch);
                $("#generarReporteCerrados").removeClass("btnWait");

                if (fechaMuestraDesdeTituloCerrados == fechaMuestraHastaTituloCerrados) {
                    $("#titleTabFechaCerrados").html(' - ' + fechaMuestraHastaTituloCerrados);
                    $("#titleTabFechaCerrados2").html(' - ' + fechaMuestraHastaTituloCerrados);
                } else {
                    $("#titleTabFechaCerrados").html(' - ' + fechaMuestraDesdeTituloCerrados + ' - ' + fechaMuestraHastaTituloCerrados);
                    $("#titleTabFechaCerrados2").html(' - ' + fechaMuestraDesdeTituloCerrados + ' - ' + fechaMuestraHastaTituloCerrados);
                }

                $("#tblDataTableCerrados tbody").empty();

                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'ID' },
                    { data: 'Departamento' },
                    { data: 'Usuario' },
                    { data: 'Ticket' },
                    { data: 'Asunto' },
                    { data: 'Estado' },
                    { data: 'AgenteAsignado' },
                    { data: 'FechaCreacion' },
                    { data: 'HoraCreacion' },
                    { data: 'FechaAsignacion' },
                    { data: 'HoraAsignacion' },
                    { data: 'FechaCierre' },
                    { data: 'HoraCierre' },
                    { data: 'FechaInicioSoporte' },
                    { data: 'HoraInicioSoporte' },
                    { data: 'FechaFinSoporte' },
                    { data: 'HoraFinSoporte' },
                    { data: 'TiempoTicket' },
                    { data: 'TiempoAsignacionCierre' },
                    { data: 'TiempoSoporte' },
                    { data: 'TiempoCreacionFinSoporte' },
                    { data: 'TiempoTicketLetras' }

                ];
                tableCerrados.DataTable().destroy();
                tableCerrados.DataTable(config.opcionesDT);
                tableCerrados.DataTable().clear();
                tableCerrados.DataTable().rows.add(resultado.DataPlana);
                tableCerrados.DataTable().draw();


                var permisosTotalLabels = [];
                var permisosTotalCantidades = [];
                var coloresDeps = [];


                $.each(resultado.TotalTicketsPorDep, function (i, it) {
                    permisosTotalLabels[i] = it.Descripcion;
                    permisosTotalCantidades[i] = it.Total;

                    $.each(resultado.DepartamentoColores, function (i0, dep) {
                        if (dep.Codigo == it.Descripcion) {
                            coloresDeps[i] = dep.Descripcion
                        }
                    });
                });




                chartDepartamentos_cerrados.updateOptions({
                    colors: coloresDeps,
                    xaxis: {
                        categories: permisosTotalLabels,
                    }
                });
                chartDepartamentos_cerrados.updateSeries([{
                    name: 'Tickets',
                    data: permisosTotalCantidades
                }]);


              

                var pivot_agente2 = new WebDataRocks({
                    container: "#wdr-component-agente-cerrados",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCierre.Year"
                                },
                                {
                                    "uniqueName": "FechaCierre.Month"
                                },
                                {
                                    "uniqueName": "FechaCierre.Day"
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "AgenteAsignado"
                                },
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count"
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                var pivot_depa2 = new WebDataRocks({
                    container: "#wdr-component-depa-cerrados",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCierre.Year"
                                },
                                {
                                    "uniqueName": "FechaCierre.Month"
                                },
                                {
                                    "uniqueName": "FechaCierre.Day"
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "AgenteAsignado"
                                },
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count"
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                var pivot_dias_agente2 = new WebDataRocks({
                    container: "#wdr-component-dias-agente-cerrados",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.DataPlana
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "FechaCierre.Year"
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "Departamento"
                                },
                                {
                                    "uniqueName": "Usuario"
                                },
                                {
                                    "uniqueName": "Asunto"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "FechaCierre.Month"
                                },
                                {
                                    "uniqueName": "FechaCierre.Day"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Ticket",
                                    "aggregation": "count",
                                    "availableAggregations": [
                                        "count",
                                        "distinctcount"
                                    ]
                                },
                                {
                                    "uniqueName": "ID",
                                    "aggregation": "count",
                                    "active": false
                                }
                            ]

                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                $("#txtTkTotales-cerrados").html(resultado.Totales.Totales);
                $("#txtTiempoSoporte").html(0);
                $("#txtTiempoEspera").html(0);


            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarReporteCerrados").attr('href', "#");
                $("#iconSearchCerrados").removeClass(iconLoader);
                $("#iconSearchCerrados").addClass(iconSearch);
                $("#generarReporteCerrados").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

            }
        });

        return false;

    });

    function customizeToolbar(toolbar) {
        var tabs = toolbar.getTabs(); // get all tabs from the toolbar
        toolbar.getTabs = function () {
            delete tabs[0]; // delete the first tab
            delete tabs[1];
            delete tabs[2];
            delete tabs[3].menu[1]; // borrar exportar html
            return tabs;
        }
    }

    
    $("#generarReporte").trigger('click');
    $("#generarReporteCerrados").trigger('click');
});