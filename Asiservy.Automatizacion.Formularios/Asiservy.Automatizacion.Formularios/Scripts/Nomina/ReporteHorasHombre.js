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



    var table = $("#tblDataTable");

    $("#generarReporte").click(function () {
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();

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


                $("#tblDataTable tbody").empty();

                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Fecha' },
                    { data: 'Cedula' },
                    { data: 'Nombre' },
                    { data: 'CodCentroCosto' },
                    { data: 'CentroCosto' },
                    { data: 'Linea' },
                    { data: 'CodRecurso' },
                    { data: 'Recurso' },
                    { data: 'Cargo' },
                    { data: 'Turno' },
                    { data: 'HoraInicio' },
                    { data: 'HoraFin' },
                    { data: 'HorasReloj' },
                    { data: 'DescuentoAlmuerzo' },
                    { data: 'DescuentoCena' },
                    { data: 'HorasLaboradas' },
                    { data: 'NoFinAsistencia' },
                    { data: 'TipoRol' }

                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
                                             

                var _fechas = [];
                $.each(resultado, function (i, rowObj) {
                    if (jQuery.inArray(rowObj.Fecha, _fechas) === -1) {
                        _fechas.push(rowObj.Fecha);
                    }
                });

                var tuplesRows = [];
                $.each(_fechas, function (i, _rowFecha) {
                    tuplesRows.push({ "tuple": ["Fecha." + _rowFecha] });
                });

                var pivot_agente = new WebDataRocks({
                    container: "#wdr-component-horas",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "TipoRol",
                                    "filter": {
                                        "members": [
                                            "TipoRol.PLANTA"
                                        ]
                                    }
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "Fecha"
                                },
                                {
                                    "uniqueName": "CodCentroCosto"
                                },
                                {
                                    "uniqueName": "CentroCosto"
                                },
                                {
                                    "uniqueName": "CodRecurso"
                                },
                                {
                                    "uniqueName": "Turno"
                                },
                                {
                                    "uniqueName": "Cargo"
                                },
                                {
                                    "uniqueName": "Nombre"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "Measures"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Cedula",
                                    "aggregation": "distinctcount"
                                },
                                {
                                    "uniqueName": "HorasLaboradas",
                                    "aggregation": "sum"
                                }
                            ],
                            "expands": {
                                "rows": tuplesRows
                            },
                            "tableSizes": {
                                "columns": [
                                    {
                                        "idx": 0,
                                        "width": 508
                                    }
                                ]
                            }
                        }
                        
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

               
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