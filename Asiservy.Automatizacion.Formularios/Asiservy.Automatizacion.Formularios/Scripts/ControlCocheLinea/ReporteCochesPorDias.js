$(function () {
    var datajsonClasificador = JSON.parse($("#lineas").val());
    var objetoCodigosLineas = [];
    var objetoLineas = [];
    $.each(datajsonClasificador, function (i, item0) {
        objetoCodigosLineas.push(item0.Codigo);
        objetoLineas[item0.Codigo] = item0.Descripcion;
    });
 
    var optionsConLinea = {
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
        colors: ['#77B6EA', '#545454', '#FF5733', '#D7FF33','#FF333C'],
        dataLabels: {
            enabled: true,
        },
        stroke: {
            curve: 'smooth'
        },
        series: [],
        title: {
            text: 'Total de coches por línea',
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

            size: 6
        },
        xaxis: {
            categories: ['0'],
            title: {
                text: 'Fechas'
            }
        },
        yaxis: {
            title: {
                text: 'Coches'
            },
            min: 5,
            max: 40
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val + " coches"
                }
            }
        },
        legend: {
            position: 'top',
            horizontalAlign: 'right',
            floating: true,
            offsetY: -25,
            offsetX: -5
        }

    }

    var chartDiario = new ApexCharts(document.querySelector("#chartDiario"), optionsConLinea);
    chartDiario.render();

    var chartDiarioTotal = new ApexCharts(document.querySelector("#chartDiarioTotalizado"), optionsConLinea);
    chartDiarioTotal.render();


    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
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


    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 61
        },
        minDate: moment("01/10/2019", "DD/MM/YYYY"),
        maxDate: moment().subtract(1, 'days'),
        ranges: {
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment().subtract(1, 'days')],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment().subtract(1, 'days')],
            'Mes actual (hasta hoy)': [moment().startOf('month'), moment().subtract(1, 'days')],
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


    $("#generarGrafico").click(function () {
        var fechaDesde = $("#fechaDesde").val();
        var fechaHasta = $("#fechaHasta").val();
        
        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);


        $("#generarGrafico").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarGrafico").addClass("btnWait");

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../ControlCocheLinea/ObtenerCochesPorLineaDiario",
            type: "GET",
            data: {
                'fechaIni': fechaDesde,
                'fechaFin': fechaHasta
            },
            success: function (resultado) {

                $("#generarGrafico").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarGrafico").removeClass("btnWait");

                var fechasCategories = [];
                if (resultado.length > 0) {
                    $.each(resultado, function (i, item) {
                        var _fecha = moment(parseInt(item.Fecha.substr(6))).format('DD/MM/YYYY');
                        if (jQuery.inArray(_fecha, fechasCategories) == -1) {
                            fechasCategories.push(_fecha);
                        }
                    });
                }



                var maxSerie = 0;
                var minSerie = 0;
                if (fechasCategories.length > 0) {
                    var _series = [];
                    $.each(objetoCodigosLineas, function (i, itemCodLinea) {
                        var dataSerie = [];
                        $.each(fechasCategories, function (i, itemFecha) {
                            var valorCoche = 0;                            
                            $.each(resultado, function (i, itemData) {
                                var _fecha = moment(parseInt(itemData.Fecha.substr(6))).format('DD/MM/YYYY');
                                if (_fecha == itemFecha && itemCodLinea == itemData.Linea) {
                                    valorCoche = itemData.Coches;
                                }
                            });
                            if (maxSerie < valorCoche) {
                                maxSerie = valorCoche
                            }
                            if (minSerie < valorCoche) {
                                minSerie = valorCoche
                            }
                            dataSerie.push(valorCoche);
                        });
                        var itemSerie = {
                            name: objetoLineas[itemCodLinea] ,
                            data: dataSerie
                        }
                        _series.push(itemSerie);
                    });
                  
                }

                var serieDataSum = [];

                $.each(fechasCategories, function (iCat0, _itemCat0) {
                    serieDataSum[iCat0] = 0;
                });

                $.each(_series, function (i, _itemSerie) {
                    $.each(fechasCategories, function (iCat, _itemCat) {
                        serieDataSum[iCat] = serieDataSum[iCat]  + _itemSerie.data[iCat] ;
                    });
                });

                var serieTotal = [{
                    name: 'Total día',
                    data: serieDataSum
                }];

            
                chartDiario.updateOptions({
                    xaxis: {
                        categories: fechasCategories,
                    },
                    yaxis: {    
                        title: {
                            text: 'Coches'
                        },
                        min: 0,
                        max: maxSerie + 10
                    }
                });
                chartDiario.updateSeries(_series);

                var minTotals = Math.min.apply(Math, serieDataSum);
                console.log(minTotals);
             
                if (minTotals >= 100) {
                    minTotals = 100;
                } else {
                    minTotals = 0;
                }

                
                
                console.log(minTotals);
                chartDiarioTotal.updateOptions({
                    title: {
                        text: 'Total de coches por día',
                        align: 'left'
                    },
                    xaxis: {
                        categories: fechasCategories,
                    },
                    yaxis: {
                        title: {
                            text: 'Coches'
                        },
                        min: minTotals,
                        max: Math.max.apply(Math, serieDataSum) + 100
                    }
                });
                chartDiarioTotal.updateSeries(serieTotal);

                $("#detallePorDia").slideDown();
            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarGrafico").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarGrafico").removeClass("btnWait");
                MensajeError(resultado.statusText, false);
            }
        });

        return false;

    });

});