$(function () {

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
        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
    }


    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 31
        },
        minDate: moment("01/01/2020", "DD/MM/YYYY"),
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

        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);


        $("#generarReporte").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarReporte").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/GenerarReporteHorasHombre",
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
                

                var gridOptions = {
                    dataSource: resultado,
                    columns: [{ dataField: 'Fecha', dataType: "date" },
                    { dataField: 'Cedula', dataType: "string" },
                    { dataField: 'Nombre', dataType: "string" },
                    { dataField: 'CodCentroCosto', dataType: "string" },
                    { dataField: 'CentroCosto', dataType: "string" },
                    { dataField: 'Linea', dataType: "string" },
                    { dataField: 'CodRecurso', dataType: "string" },
                    { dataField: 'Recurso', dataType: "string" },
                    { dataField: 'Cargo', dataType: "string" },
                    { dataField: 'Turno', dataType: "string" },
                    { dataField: 'HoraInicio', dataType: "string" },
                    { dataField: 'HoraFin', dataType: "string" },
                    { dataField: 'HorasReloj', dataType: "number" },
                    { dataField: 'DescuentoAlmuerzo', dataType: "number" },
                    { dataField: 'DescuentoCena', dataType: "number" },
                    { dataField: 'HorasLaboradas', dataType: "number" },
                    { dataField: 'NoFinAsistencia', dataType: "boolean" },
                    { dataField: 'TipoRol', dataType: "string" }],
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
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Reporte_' + fechaDesde + '_' + fechaHasta + '.xlsx');
                            });
                        });
                        e.cancel = true;
                    },
                    summary: {
                        totalItems: [{
                            column: "HorasLaboradas",
                            summaryType: "sum",
                            valueFormat: "decimal"
                        }]
                    }
                };
                $("#grid").dxDataGrid(gridOptions).dxDataGrid("instance");

                var pivotGridDataSource = new DevExpress.data.PivotGridDataSource({
                    store: resultado,
                    fields: [
                        {
                            caption: "Fecha",
                            dataField: "Fecha",
                            area: "column",
                            width: 100
                        },
                        {
                            caption: "Código CC",
                            dataField: "CodCentroCosto",
                            area: "row"
                        },
                        {
                            caption: "Centro de Costo",
                            dataField: "CentroCosto",
                            area: "row"
                        },
                        {
                            caption: "Código Recurso",
                            dataField: "CodRecurso",
                            area: "row"
                        },
                        {
                            caption: "Turno",
                            dataField: "Turno",
                            area: "row"
                        },
                        {
                            caption: "Cargo",
                            dataField: "Cargo",
                            area: "row"
                        },
                        {
                            caption: "Nombre",
                            dataField: "Nombre",
                            area: "row"
                        },
                        {
                            caption: "Tipo de rol",
                            dataField: "TipoRol",
                            area: "filter",
                            filterType: 'include',
                            filterValues: ['PLANTA']
                        },
                        {
                            caption: "Personas",
                            dataField: "Cedula",
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
                        },
                        {
                            caption: "Horas",
                            dataField: "HorasLaboradas",
                            summaryType: "sum",
                            format: "decimal",
                            dataType: "number",
                            area: "data",
                            width: 50
                        }
                    ]
                });


                var pivotGridOptions = {
                    dataSource: pivotGridDataSource,
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
                    },
                    export: {
                        enabled: true,
                        ignoreExcelErrors: true,
                        fileName: "ResumenHorasDiarias"
                    },
                };
                $("#pivotgrid").dxPivotGrid(pivotGridOptions).dxPivotGrid("instance");
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




    function customizeToolbar(toolbar) {
        var tabs = toolbar.getTabs(); // get all tabs from the toolbar
        //toolbar.getTabs = function () {
        //    delete tabs[0]; // delete the first tab
        //    delete tabs[1];
        //    delete tabs[2];
        //    delete tabs[3].menu[1]; // borrar exportar html
        //    return tabs;
        //}
    }


    $("#generarReporte").trigger('click');
});