$(function () {

    var fileNameExcel = "DatosAsistencia-CSV-file_";

    var optionsGenero = {
        chart: {
            type: 'bar'
        },
        series: [{name:'', data: []}],
        xaxis: {
            categories: ['MASCULINO','FEMENINO']
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
    
    var chartGenero = new ApexCharts(document.querySelector("#chartGenero"), optionsGenero);
    chartGenero.render();



    var optionsPermiso = {
        chart: {
            height: 350,
            type: 'bar',
        },
        plotOptions: {
            bar: {
                columnWidth: '50%',
                endingShape: 'rounded'
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            width: 2
        },
        series: [{
            name: 'Permisos',
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
            categories: ['PERMISO'],
        },
        yaxis: {
            title: {
                text: 'Permisos',
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
        },

    }

    var chartPermisos = new ApexCharts(document.querySelector("#chartPermisos"), optionsPermiso);
    chartPermisos.render();

    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
    var start = moment().subtract(1, 'days');
    var end = moment().subtract(1, 'days');
    var mesesLetras = {
        '01':"Enero",
        '02':"Febrero",
        '03':"Marzo",
        '04':"Abril",
        '05':"Mayo",
        '06':"Junio",
        '07':"Julio",
        '08':"Agosto",
        '09':"Septiembre",
        '10':"Octubre",
        '11':"Noviembre",
        '12':"Diciembre"
    }

    function cb(start, end) {

        var fechaMuestraDesde = mesesLetras[start.format('MM')] + ' ' + start.format('D') + ', ' + start.format('YYYY');
        var fechaMuestraHasta = mesesLetras[end.format('MM')] + ' ' + end.format('D') + ', ' + end.format('YYYY');
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));

        fileNameExcel = fileNameExcel + start.format('DDMMYYYY') + "-" + end.format('DDMMYYYY')

        $('#reportrange span').html(fechaMuestraDesde + ' - ' + fechaMuestraHasta);
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

    $("#exportExcel").click(function () {

        var hotInstance = $('#data-asistencia').handsontable('getInstance');

        var exportPlugin1 = hotInstance.getPlugin('exportFile');

        exportPlugin1.downloadFile('csv', {
            bom: false,
            columnDelimiter: ',',
            columnHeaders: true,
            exportHiddenColumns: true,
            exportHiddenRows: true,
            fileExtension: 'csv',
            filename: fileNameExcel,
            mimeType: 'text/csv',
            rowDelimiter: '\r\n',
            rowHeaders: false
        });
        return false;
    });

    $("#generarAsistencia").click(function () {
        var fechaDesde = $("#fechaDesde").val();
        var fechaHasta = $("#fechaHasta").val();
        var empresa = $("#select_empresa").val();
        var cedula = $.trim($("#txtCedulaUsuario").val());

        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);
        var diffDays = f2.diff(f1, 'days');
       
        if (diffDays == 0) {
            $("#detallePorDia").hide();
        }

        $("#generarAsistencia").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarAsistencia").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/GenerarAsistencial",
            type: "POST",
            data: JSON.stringify({
                'fechaIni': fechaDesde,
                'fechaFin': fechaHasta,
                'empresa': empresa,
                'cedula': cedula
            }),
            success: function (resultado) {
                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");


                $("#txtCantTotalPersonas").text(resultado.TotalPersonas);
                $("#txtCantAsistieron").text(resultado.TotalAsistentes);
                $("#txtCantAusentes").text(resultado.TotalAusentes);
                $("#txtCantAusentesPermiso").text(resultado.TotalConPermiso);
                $("#txtCantAusentesSinPermiso").text(resultado.TotalSinPermiso);

                if (cedula == "") {
                  
                    $("#detallePorGeneroPermisos").addClass('mostrarBloques');

                    var categorias = [];
                    var _series = [];
                    $("#bodyTblDatosGenero").empty();
                    $.each(resultado.TotalGeneros, function (i, it0) {
                        categorias[i] = it0.Genero;
                        var newRowContent = "<tr><td>" + it0.Genero + "</td><td>" + it0.Ausentes + "</td><td>" + it0.AusentesConPermiso + "</td><td>" + it0.AusentesSinPermiso +"</td></tr>";

                        $("#bodyTblDatosGenero").append(newRowContent);
                    });

                    var keysGenerosTotales = Object.keys(resultado.TotalGeneros[0]);
                    var columns_series = [];
                    $.each(keysGenerosTotales, function (i, it1) {
                        if (it1 == 'AusentesConPermiso' || it1 == 'Ausentes' || it1 == "AusentesSinPermiso") {
                            columns_series.push(it1);
                        }
                       
                    });

                    $.each(columns_series, function (i3, cols) {
                        var _data = [];
                        $.each(categorias, function (i, cat) {
                            $.each(resultado.TotalGeneros, function (i2, item) {
                                if (cat == item.Genero) {
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

                   
                    chartGenero.updateOptions({
                        xaxis: {
                            categories: categorias
                        }
                    })
                    chartGenero.updateSeries(_series)

                    var permisosTotalLabels = [];
                    var permisosTotalCantidades = [];
                    $.each(resultado.TotalPermisos, function (i, it) {
                        permisosTotalLabels[i] = it.Descripcion;
                        permisosTotalCantidades[i] = it.Total;
                    });                  
                    
                    chartPermisos.updateOptions({
                        xaxis: {
                            categories: permisosTotalLabels,
                        }
                    });
                    chartPermisos.updateSeries([{
                        name: 'Permisos',
                        data: permisosTotalCantidades
                    }]);

                    $("#detallePorPermiso").slideDown();
                    if (diffDays > 0) {

                        var cats_dias = [];
                        var _seriesTotal = [];

                        $.each(resultado.TotalDias, function (keyDia, itDia) {
                            cats_dias[keyDia] = itDia.Descripcion;
                            _seriesTotal[keyDia] = itDia.Total;
                        });
                        var optionsDias = {
                            chart: {
                                height: 350,
                                type: 'line',
                                shadow: {
                                    enabled: true,
                                    color: '#000',
                                    top: 18,
                                    left: 7,
                                    blur: 10,
                                    opacity: 1
                                },
                                toolbar: {
                                    show: false
                                }
                            },
                            colors: ['#77B6EA', '#545454'],
                            dataLabels: {
                                enabled: true,
                            },
                            stroke: {
                                curve: 'smooth'
                            },
                            series: [
                                {
                                    name: 'Ausentes',
                                    data: _seriesTotal
                                }
                            ],
                            grid: {
                                borderColor: '#e7e7e7',
                                row: {
                                    colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
                                    opacity: 0.5
                                },
                            },
                            markers: {

                                size: 6
                            },
                            xaxis: {
                                categories: cats_dias,
                                title: {
                                    text: 'Fecha'
                                }
                            },
                            yaxis: {
                                title: {
                                    text: 'Personas'
                                },
                                min: 0,
                                max: Math.max.apply(Math, _seriesTotal) + 10
                            },
                            legend: {
                                position: 'top',
                                horizontalAlign: 'right',
                                floating: true,
                                offsetY: -25,
                                offsetX: -5
                            }
                        }

                        document.getElementById('chartDiario').innerHTML = '';
                        var chartDiario = new ApexCharts(
                            document.querySelector("#chartDiario"),
                            optionsDias
                        );

                        chartDiario.render();



                        $("#detallePorDia").slideDown();
                    } 
                } else {
                    $("#detallePorGeneroPermisos").removeClass('mostrarBloques');
                    $("#detallePorDia").hide();
                }

                

                var keys = Object.keys(resultado.dataGeneral[0]);
                var columns_obj = [];
                $.each(keys, function (i, it) {
                    columns_obj[i] = it ;
                });

                var hotSettings = {
                    readOnly: true,
                    data: resultado.dataGeneral,
                    stretchH: 'all',
                    autoWrapRow: true,
                    height: 600,
                    filters: true,
                    rowHeaders: true,
                    dropdownMenu: true,
                    fixedColumnsLeft: 1,
                    colHeaders: columns_obj,
                    language: 'es-MX',
                    dropdownMenu: [
                        'filter_by_condition',
                        'filter_by_value',
                        'filter_action_bar'
                    ],
                    licenseKey: 'non-commercial-and-evaluation'
                };

                $('#data-asistencia').handsontable(hotSettings);

                var hotInstance = $('#data-asistencia').handsontable('getInstance');
                hotInstance.getPlugin('Filters').clearConditions();
                hotInstance.getPlugin('Filters').filter();
                hotInstance.render();

                $("#exportExcel").show();
              

                //$("#data-asistencia").slideDown();
            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarAsistencia").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarAsistencia").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

                $("#detallePorPermiso").hide();
                $("#exportExcel").hide();
            }
        });
       
        return false;

    });
    
});