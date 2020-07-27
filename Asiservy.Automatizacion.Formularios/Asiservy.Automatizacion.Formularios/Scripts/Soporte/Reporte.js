$(function () {
    DevExpress.localization.locale(navigator.language);
    var fechaMuestraDesdeTitulo = '';
    var fechaMuestraHastaTitulo = '';
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
  


    $("#generarReporte").click(function () {


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
                    $("#titleTabFecha2").html(' - ' + fechaMuestraHastaTitulo);
                } else {
                    $("#titleTabFecha2").html(' - ' + fechaMuestraDesdeTitulo + ' - ' + fechaMuestraHastaTitulo);
                }

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


                var pivotGridChartAgente = $("#pivotgrid-chart-agente").dxChart({
                    commonSeriesSettings: {
                        type: "bar"
                    },
                    tooltip: {
                        enabled: true,
                        format: "decimal",
                        customizeTooltip: function (args) {
                            return {
                                html: args.seriesName + " | " + args.valueText + " Tickets"
                            };
                        }
                    },
                    size: {
                        height: 300
                    },
                    adaptiveLayout: {
                        width: 450
                    }
                }).dxChart("instance");
                var pivotGridDataSource1 = new DevExpress.data.PivotGridDataSource({
                    store: resultado.DataPlana,
                    fields: [
                        {
                            caption: "Estado",
                            dataField: "Estado",
                            area: "column",
                            width: 100
                        },
                        {
                            caption: "Agente TI",
                            dataField: "AgenteAsignado",
                            area: "row"
                        },
                        {
                            caption: "Departamento",
                            dataField: "Departamento",
                            area: "row"
                        },
                        {
                            caption: "Asunto",
                            dataField: "Asunto",
                            area: "row",
                            selector: function (data) {
                                return data.Asunto + " (" + data.Usuario + ")";
                            }

                        },
                        {
                            caption: "Fecha de creación",
                            dataField: "FechaCreacion",
                            dataType: "date",
                            area: "filter",
                            groupName: "Date"
                        },

                        { groupName: "Date", groupInterval: "year", groupIndex: 0 },
                        { groupName: "Date", groupInterval: "month", groupIndex: 1 },
                        { groupName: "Date", groupInterval: "day", groupIndex: 2 },
                        {
                            caption: "Tickets",
                            dataField: "Ticket",
                            summaryType: "count",
                            area: "data",
                            width: 50,
                            calculateCustomSummary: function (options) {
                                switch (options.summaryProcess) {
                                    case "start":
                                        options.arr = [];
                                        options.totalValue = 0;
                                        break;
                                    case "calculate":
                                        if ($.inArray(options.value, options.arr) == -1) {
                                            options.arr.push(options.value);
                                            options.totalValue += 1;
                                        }
                                        break;
                                    case "finalize":
                                        if (options.totalValue == '' || options.totalValue == null) {
                                            options.totalValue = 0;
                                        }
                                        options.arr = [];
                                        break;
                                }
                            }
                        }
                    ]
                });
                var pivotGridOptions1 = {
                    dataSource: pivotGridDataSource1,
                    allowSortingBySummary: true,
                    allowSorting: true,
                    allowFiltering: true,
                    showBorders: true,
                    showColumnGrandTotals: true,
                    showRowGrandTotals: true,
                    showRowTotals: true,
                    showColumnTotals: false,
                    fieldPanel: {
                        showColumnFields: true,
                        showDataFields: true,
                        showFilterFields: true,
                        showRowFields: true,
                        allowFieldDragging: true,
                        visible: true
                    },
                    fieldChooser: {
                        enable: true,
                        height: 500
                    }
                };
                var pivotGridAgente = $("#pivotgrid-component-agente").dxPivotGrid(pivotGridOptions1).dxPivotGrid("instance");
                pivotGridAgente.bindChart(pivotGridChartAgente, {
                    dataFieldsDisplayMode: "splitPanes",
                    alternateDataFields: false
                });


                var pivotGridChartDepa = $("#pivotgrid-chart-depa").dxChart({
                    commonSeriesSettings: {
                        type: "bar"
                    },
                    tooltip: {
                        enabled: true,
                        format: "decimal",
                        customizeTooltip: function (args) {
                            return {
                                html: args.seriesName + " | " + args.valueText + " Tickets"
                            };
                        }
                    },
                    size: {
                        height: 300
                    },
                    adaptiveLayout: {
                        width: 450
                    }
                }).dxChart("instance");
                var pivotGridDataSource2 = new DevExpress.data.PivotGridDataSource({
                    store: resultado.DataPlana,
                    fields: [
                        {
                            caption: "Estado",
                            dataField: "Estado",
                            area: "column",
                            width: 100
                        },
                        {
                            caption: "Departamento",
                            dataField: "Departamento",
                            area: "row"
                        },
                        {
                            caption: "Asunto",
                            dataField: "Asunto",
                            area: "row",
                            selector: function (data) {
                                return data.Asunto + " (" + data.Usuario + ")";
                            }

                        },
                        {
                            caption: "Fecha de creación",
                            dataField: "FechaCreacion", 
                            dataType: "date",
                            area: "filter",
                            groupName: "Date"
                        },
                        
                        { groupName: "Date", groupInterval: "year", groupIndex: 0 },
                        { groupName: "Date", groupInterval: "month", groupIndex: 1 },
                        { groupName: "Date", groupInterval: "day", groupIndex: 2 },
                        {
                            caption: "Tickets",
                            dataField: "Ticket",
                            summaryType: "count",
                            area: "data",
                            width: 50,
                            calculateCustomSummary: function (options) {
                                switch (options.summaryProcess) {
                                    case "start":
                                        options.arr = [];
                                        options.totalValue = 0;
                                        break;
                                    case "calculate":
                                        if ($.inArray(options.value, options.arr) == -1) {
                                            options.arr.push(options.value);
                                            options.totalValue += 1;
                                        }
                                        break;
                                    case "finalize":
                                        if (options.totalValue == '' || options.totalValue == null) {
                                            options.totalValue = 0;
                                        }
                                        options.arr = [];  
                                        break;
                                }
                            }
                        }
                    ]
                });
                var pivotGridOptions2 = {
                    dataSource: pivotGridDataSource2,
                    allowSortingBySummary: true,
                    allowSorting: true,
                    allowFiltering: true,
                    showBorders: true,
                    showColumnGrandTotals: true,
                    showRowGrandTotals: true,
                    showRowTotals: true,
                    showColumnTotals: false,
                    fieldPanel: {
                        showColumnFields: true,
                        showDataFields: true,
                        showFilterFields: true,
                        showRowFields: true,
                        allowFieldDragging: true,
                        visible: true
                    },
                    fieldChooser: {
                        enable: true,
                        height: 500
                    }
                };
                var pivotGridDepa = $("#pivotgrid-component-depa").dxPivotGrid(pivotGridOptions2).dxPivotGrid("instance");
                pivotGridDepa.bindChart(pivotGridChartDepa, {
                    dataFieldsDisplayMode: "splitPanes",
                    alternateDataFields: false
                });


                var pivotGridChartDiasAgente = $("#pivotgrid-chart-dias-agente").dxChart({
                    commonSeriesSettings: {
                        type: "bar"
                    },
                    tooltip: {
                        enabled: true,
                        format: "decimal",
                        customizeTooltip: function (args) {
                            return {
                                html: args.seriesName + " | " + args.valueText +  " Tickets"
                            };
                        }
                    },
                    size: {
                        height: 300
                    },
                    adaptiveLayout: {
                        width: 450
                    }
                }).dxChart("instance");
                var pivotGridDataSource3 = new DevExpress.data.PivotGridDataSource({
                    store: resultado.DataPlana,
                    fields: [
                        {
                            caption: "Fecha de creación",
                            dataField: "FechaCreacion",
                            dataType: "date",
                            area: "column",
                            groupName: "Fecha",
                            width: 100,
                            expanded: true
                        },
                        { caption: "Año", groupName: "Fecha", groupInterval: "year", groupIndex: 0 },
                        { caption: "Mes",  groupName: "Fecha", groupInterval: "month", groupIndex: 1 },
                        { caption: "Día",  groupName: "Fecha", groupInterval: "day", groupIndex: 2 },
                        {
                            caption: "Departamento",
                            dataField: "Departamento",
                            area: "row"
                        },
                        {
                            caption: "Usuario",
                            dataField: "Usuario",
                            area: "row"
                        },
                        {
                            caption: "Asunto",
                            dataField: "Asunto",
                            area: "row"
                        },
                        {
                            caption: "Estado",
                            dataField: "Estado",
                            area: "filter",
                            filterType: 'include',
                            filterValues: ['Cerrado']
                        },
                        {
                            caption: "Tickets",
                            dataField: "Ticket",
                            summaryType: "custom",
                            area: "data",
                            width: 50,
                            calculateCustomSummary: function (options) {
                                switch (options.summaryProcess) {
                                    case "start":
                                        options.arr = [];
                                        options.totalValue = 0;
                                        break;
                                    case "calculate":
                                        if ($.inArray(options.value, options.arr) == -1) {
                                            options.arr.push(options.value);
                                            options.totalValue += 1;
                                        }
                                        break;
                                    case "finalize":
                                        options.arr = [];
                                        break;
                                }
                            }
                        }
                    ]
                });
                var pivotGridOptions3 = {
                    dataSource: pivotGridDataSource3,
                    allowSortingBySummary: true,
                    allowSorting: true,
                    allowFiltering: true,
                    showBorders: true,
                    showColumnGrandTotals: false,
                    showRowGrandTotals: true,
                    showRowTotals: true,
                    showColumnTotals: false,
                    fieldPanel: {
                        showColumnFields: true,
                        showDataFields: true,
                        showFilterFields: true,
                        showRowFields: true,
                        allowFieldDragging: true,
                        visible: true
                    },
                    fieldChooser: {
                        enable: true,
                        height: 500
                    }
                };
                var pivotGridDiasAgente = $("#pivotgrid-dias-agente").dxPivotGrid(pivotGridOptions3).dxPivotGrid("instance");
                pivotGridDiasAgente.bindChart(pivotGridChartDiasAgente, {
                    dataFieldsDisplayMode: "splitPanes",
                    alternateDataFields: false
                });


                var gridOptions = {
                    dataSource: resultado.DataPlana,
                    columns: [{ dataField: 'ID', dataType: "number" },
                    { dataField: 'Departamento', dataType: "string" },
                    { dataField: 'Usuario', dataType: "string" },
                    { dataField: 'Ticket', dataType: "string" },
                    { dataField: 'Asunto', dataType: "string" },
                    { dataField: 'Estado', dataType: "string" },
                    { dataField: 'AgenteAsignado', dataType: "string" },
                    { dataField: 'FechaCreacion', dataType: "date" },
                    { dataField: 'HoraCreacion', dataType: "string" },
                    { dataField: 'FechaAsignacion', dataType: "date" },
                    { dataField: 'HoraAsignacion', dataType: "string" },
                    { dataField: 'FechaCierre', dataType: "date" },
                    { dataField: 'HoraCierre', dataType: "string" },
                    { dataField: 'FechaInicioSoporte', dataType: "date" },
                    { dataField: 'HoraInicioSoporte', dataType: "string" },
                    { dataField: 'FechaFinSoporte', dataType: "date" },
                    { dataField: 'HoraFinSoporte', dataType: "string" },
                    { dataField: 'TiempoTicket', dataType: "string" },
                    { dataField: 'TiempoAsignacionCierre', dataType: "string" },
                    { dataField: 'TiempoSoporte', dataType: "string" },
                    { dataField: 'SoporteHoras', dataType: "number" },
                    { dataField: 'TiempoCreacionFinSoporte', dataType: "string" },
                    { dataField: 'TiempoTicketLetras', dataType: "string" }
                    ],
                    paging: {
                        pageSize: 15
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true,
                        width: 240,
                        placeholder: "Buscar..."
                    },
                    groupPanel: { visible: true },
                    grouping: {
                        autoExpandAll: false
                    },
                    selection: {
                        mode: "single"
                    },
                    allowColumnResizing: true,
                    columnResizingMode: "nextColumn",
                    columnMinWidth: 50,
                    columnAutoWidth: true,
                    columnFixing: {
                        enabled: true
                    },
                    showBorders: true,
                    showRowLines: true,
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    headerFilter: {
                        visible: true
                    },
                    export: {
                        enabled: true,
                        allowExportSelectedData: true
                    },
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Datos');

                        DevExpress.excelExporter.exportDataGrid({
                            component: e.component,
                            worksheet: worksheet,
                            autoFilterEnabled: true
                        }).then(function () {
                            workbook.xlsx.writeBuffer().then(function (buffer) {
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Soportes.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                    summary: {
                        totalItems: [{
                            column: "SoporteHoras",
                            summaryType: "sum",
                            valueFormat: "decimal"
                        }]
                    }
                };
                $("#grid-data-plana").dxDataGrid(gridOptions).dxDataGrid("instance");
               

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

   



    
    $("#generarReporte").trigger('click');
});