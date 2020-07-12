$(function () {
    var start = moment().subtract(1, 'days');
    var end = moment().subtract(1, 'days');
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
    var optionsLineasGraficoAsistencia = {
        chart: {
            type: 'bar'
        },
        series: [{ name: '', data: [] }],
        xaxis: {
            categories: []
        },
        yaxis: {
            title: {
                text: '# (personas)'
            }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val + " personas"
                }
            }
        }
    }
    var chartLineasAsistentes = new ApexCharts(document.querySelector("#grafico-lineas-asistentes"), optionsLineasGraficoAsistencia);
    chartLineasAsistentes.render();

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
    var fAwesomeSpinner = '<i id="iconCargandoBox" class="fas fa-spinner fa-pulse fa-3x"></i>';
    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
       
    $("#generarAsistencia").click(function () {
        var fechaIni = $("#fechaDesde").val();
        var fechaFin = $("#fechaHasta").val();
        $("#wdr-component").empty();
        $("#wdr-component-biometrico").empty();
        $("#wdr-component-bio-novedades").empty();

        $("#wdr-component").html(fAwesomeSpinner);
        $("#wdr-component-biometrico").html(fAwesomeSpinner);
        $("#wdr-component-bio-novedades").html(fAwesomeSpinner);

        $("#generarAsistencia").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarAsistencia").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/GenerarAsistenciaInicialVsActual",
            type: "GET",
            data:{
                'fechaIni': fechaIni,
                'fechaFin': fechaFin
            },
            success: function (resultado) {

                $("#wdr-component").empty();
                $("#wdr-component-biometrico").empty();
                $("#wdr-component-bio-novedades").empty();

                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");

              
                var _fechas = [];
                $.each(resultado.modeloVistaPersonalPresentes, function (i, rowObj) {
                    if (jQuery.inArray(rowObj.Fecha, _fechas) === -1) {
                        _fechas.push(rowObj.Fecha);
                    }
                });

                var tuplesColum = [];
                $.each(_fechas, function (i, _rowFecha) {                    
                    tuplesColum.push({ "tuple" : [ "Fecha."+_rowFecha ] });
                });


                
                var pivot = new WebDataRocks({
                    container: "#wdr-component",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: resultado.modeloVistaPersonalPresentes
                        },
                        "slice": {
                            "rows": [
                               
                                {
                                    "uniqueName": "Linea"
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
                                },
                                {
                                    "uniqueName": "Fecha"
                                },
                                {
                                    "uniqueName": "Asistencia",
                                    "sort": "desc"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "Cedula",
                                    "aggregation": "count"
                                }
                            ],
                            "expands": {
                                "rows": [
                                    {
                                        "tuple": [
                                            "CentroCosto.Limpieza de pescado"
                                        ]
                                    }
                                ],
                                "columns": tuplesColum
                            }
                        },
                        "options": {
                            "grid": {
                                "showTotals": "rows",
                                "showGrandTotals": "columns"
                            }
                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });


                var rptBiometrico = resultado.modeloVistaPersonalPresentesBiometrico;
                var _fechas_bio = [];
                $.each(rptBiometrico, function (i, rowObj) {
                    if (jQuery.inArray(rowObj.FECHA, _fechas_bio) === -1) {
                        _fechas_bio.push(rowObj.FECHA);
                    }
                });

                var tuplesColum_bio = [];
                $.each(_fechas_bio, function (i, _rowFecha) {
                    tuplesColum_bio.push({ "tuple": ["FECHA." + _rowFecha] });
                });
                var pivot_biometrico = new WebDataRocks({
                    container: "#wdr-component-biometrico",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: rptBiometrico
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "TIPO_PROCESO",
                                    "filter": {
                                        "members": [
                                            "TIPO_PROCESO.PRODUCCIÓN"
                                        ]
                                    }
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "LINEA"
                                },
                                {
                                    "uniqueName": "NOMBRES"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "FECHA"
                                },
                                {
                                    "uniqueName": "Measures"
                                },
                                {
                                    "uniqueName": "ESTADO_ASISTENCIA"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "CEDULA",
                                    "aggregation": "count"
                                }
                            ],
                            "expands": {
                                "columns": tuplesColum_bio
                            }
                        },
                        "options": {
                            "grid": {
                                "showTotals": "rows",
                                "showGrandTotals": "columns"
                            }
                        }
                        
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });

                var pivot_biometrico_nove = new WebDataRocks({
                    container: "#wdr-component-bio-novedades",
                    toolbar: true,
                    beforetoolbarcreated: customizeToolbar,
                    report: {
                        dataSource: {
                            data: rptBiometrico
                        },
                        "slice": {
                            "reportFilters": [
                                {
                                    "uniqueName": "TIPO_PROCESO",
                                    "filter": {
                                        "members": [
                                            "TIPO_PROCESO.PRODUCCIÓN"
                                        ]
                                    }
                                },
                                {
                                    "uniqueName": "ESTADO_ASISTENCIA",
                                    "filter": {
                                        "members": [
                                            "ESTADO_ASISTENCIA.AUSENTE"
                                        ]
                                    }
                                }
                            ],
                            "rows": [
                                {
                                    "uniqueName": "NOVEDAD"
                                },
                                {
                                    "uniqueName": "LINEA"
                                },
                                {
                                    "uniqueName": "NOMBRES"
                                }
                            ],
                            "columns": [
                                {
                                    "uniqueName": "FECHA"
                                },
                                {
                                    "uniqueName": "Measures"
                                }
                            ],
                            "measures": [
                                {
                                    "uniqueName": "CEDULA",
                                    "aggregation": "count"
                                }
                            ]
                        },
                        "options": {
                            "grid": {
                                "showTotals": "rows",
                                "showGrandTotals": "columns"
                            }
                        }
                    },
                    global: {
                        // replace this path with the path to your own translated file
                        localization: config.baseUrl + "Content/webdatarocks/es.json"
                    }
                });



               // $("#detallePorGeneroPermisos").addClass('mostrarBloques');
                var LineasAsistentesTotales = resultado.LineasAsistentesTotales;
                var categorias = [];
                var _series = [];               

                $.each(LineasAsistentesTotales, function (i, it0) {  
                    categorias[i] = it0.Linea;
                });
                var columns_series = ['Ausentes', 'Presentes'];
                $.each(columns_series, function (i3, cols) {
                    var _data = [];
                    $.each(categorias, function (i, cat) {
                        $.each(LineasAsistentesTotales, function (i2, item) {
                            if (cat == item.Linea) {
                                _data.push(item[cols]);
                            }
                        });
                    });
                    var itemSerie = {
                        name: cols,
                        data: _data
                    }
                    _series.push(itemSerie);
                });
                chartLineasAsistentes.updateOptions({
                    xaxis: {
                        categories: categorias
                    }
                });
                chartLineasAsistentes.updateSeries(_series);
            },
            error: function (resultado) {
                console.log(resultado);
                $("#wdr-component").empty();
                $("#wdr-component-biometrico").empty();
                $("#wdr-component-bio-novedades").empty();
                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");
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


});