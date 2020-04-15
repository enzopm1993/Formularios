$(document).ready(function () {
    ReporteControlCuchilloPreparacionPartial(0);
});
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
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        maxSpan: {
            "days": 60
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
});

function ReporteControlCuchilloPreparacionPartial(opcion) {  
    $('#cargac').show();
    var op = opcion;
   
    $.ajax({
        url: "../ControlCuchillosPreparacion/ReporteControlCuchilloPreparacionPivot",
        type: "GET",
        data: {
            filtroFechaDesde: $("#fechaDesde").val(),
            filtroFechaHasta: $("#fechaHasta").val(),
            opcion:op
        },
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
            }
            var _fechas = [];
            var _hora = [];
            $.each(resultado, function (i, rowObj) {
                if (jQuery.inArray(rowObj.Fecha, _fechas) === -1) {
                    _fechas.push(rowObj.Fecha);
                    _hora.push(rowObj.Hora);
                }
            });

            var tuplesColum = [];
            $.each(_fechas, function (i, _rowFecha) {
                tuplesColum.push({ "tuple": ["Fecha." + _rowFecha] });                
            });
            $.each(_hora, function (i, _rowHora) {
                tuplesColum.push({ "tuple": ["Hora" + _rowHora] });
            });

            console.log(tuplesColum);
            var pivot = new WebDataRocks({
                container: "#wdr-component",
                toolbar: true,
                beforetoolbarcreated: customizeToolbar,
                report: {
                    dataSource: {
                        data: resultado
                    },
                    "slice": {
                        "rows": [
                            {
                                "uniqueName": "Fecha"
                            },
                            {
                                "uniqueName": "Hora"
                            },
                            {
                                //"uniqueName": "CedulaEmpleado"
                            }
                        ],
                        "columns": [                           
                            {"uniqueName": "CodigoCuchillo"},
                            //{ "uniqueName": "Estado" },
                            { "uniqueName": "CedulaEmpleado"}
                        ],
                        "measures": [
                            {
                                "uniqueName": "Hora"
                                
                                //"aggregation": "count"
                            }
                            //, { "uniqueName": "Fecha" }
                        ],
                        "expands": {
                            "rows": [
                                {
                                    "tuple": [
                                        "CentroBarco.Limpieza de pescado"
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
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

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