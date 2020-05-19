var Error = 0;
function MostrarReporte(data) {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdControl: data.IdEvaluacionProductoEnfundado
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionProductoEnfundado/PartialReporteEvaluacionProductoEnfundado?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            //console.log(resultado);
            if (resultado != '"0"') {
                $('#DivReporte').prop('hidden', false);
                $('#DivCabReportes').prop('hidden', true);
                $('#DivReporte').empty();
                
                
                $('#DivReporte').html(resultado);
                
                $('#mensajeRegistros').prop('hidden', true);
                
                //config.opcionesDT.pageLength = 30;
                //$('#tblReporte').DataTable(config.opcionesDT);
                //LimpiarDetalleControles();
            } else {
                $('#DivReporte').empty();
                $('#DivCabReportes').empty();
            
                $('#mensajeRegistros').text(Mensajes.SinRegistrosRangoFecha);
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado.responseText, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })
}
function imprimirw() {
    window.print();
}
function Atras() {
    $('#DivReporte').prop('hidden', true);
    $('#DivCabReportes').prop('hidden', false);
}
function CargarCabReportes() {
    Atras();
    $('#cargac').show();
    var table = $("#tblDataTableReporte");
    //    table.DataTable().destroy();
    //    table.DataTable().clear();
    let params = {
        FechaDesde: $('#fechaDesde').val(),
        FechaHasta: $('#fechaHasta').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EvaluacionProductoEnfundado/ConsultarCabecerasReporte?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == '101') {
                window.location.reload();
            }
            if (resultado != '0') {
                console.log(resultado);
                $('#mensajeRegistros').prop('hidden', true);
                $("#tblDataTableReporte tbody").empty();
                $('#DivCabReportes').prop('hidden', false);
                config.opcionesDT.columns = [
                    { data:'IdEvaluacionProductoEnfundado'},
                    { data: 'FechaProduccion' },
                    { data: 'OrdenFabricacion' },
                    { data: 'Cliente' },
                    { data: 'Marca' },
                    { data: 'Destino' },
                    { data: 'Proveedor' },
                    { data: 'Lote' },
                    { data: 'Batch' },  
                    { data: 'Lomo' },
                    { data: 'Trozo' },
                    { data: 'Miga' },
                    { data: 'NivelLimpiezaDescripcion' },
                    { data: 'Observacion' },
                    { data: 'EstadoControl' },
                    { data: 'AprobadoPor' },
                    { data: 'FechaAprobacion' },
                    { data: 'FechaIngresoLog' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'FechaModificacionLog' },
                    { data: 'UsuarioModificacionLog' },
                ];

                resultado.forEach(function (row) {
                    row.FechaProduccion = moment(row.FechaProduccion).format('DD-MM-YYYY');
                    if (row.FechaAprobacion != null) {
                        row.FechaAprobacion = moment(row.FechaAprobacion).format('DD-MM-YYYY HH:mm:ss');
                    }
                    if (row.FechaIngresoLog != null) {
                        row.FechaIngresoLog = moment(row.FechaIngresoLog).format('DD-MM-YYYY');
                    }
                    if (row.FechaModificacionLog != null) {
                        row.FechaModificacionLog = moment(row.FechaModificacionLog).format('DD-MM-YYYY');
                    }
                    var estiloTrue = '<i class="fas fa-check-square" style="color:green"></i>'
                    var estiloClass = 'badge badge-danger';
                    var estado = 'PENDIENTE';
                    if (row.ObservacionDet != null) {
                        row.Observacion = row.Observacion.toUpperCase();
                    }
                    if (row.EstadoControl == true) {
                        estiloClass = 'badge badge-success';
                        estado = 'APROBADO';
                    }
                    if (row.Lomo == true) {
                        row.Lomo = estiloTrue;
                    } else {
                        row.Lomo = '';
                    }
                    if (row.Miga == true) {
                        row.Miga = estiloTrue;
                    } else {
                        row.Miga = '';
                    }
                    if (row.Trozo == true) {
                        row.Trozo = estiloTrue;
                    } else {
                        row.Trozo = '';
                    }
                    //if (row.Empaque == true) {
                    //    row.Empaque = estiloTrue;
                    //} else {
                    //    row.Empaque = '';
                    //}
                    //if (row.Enlatado == true) {
                    //    row.Enlatado = estiloTrue;
                    //} else {
                    //    row.Enlatado = '';
                    //}
                    //if (row.Pouch == true) {
                    //    row.Pouch = estiloTrue;
                    //} else {
                    //    row.Pouch = '';
                    //}
                    row.EstadoControl = '<span class="' + estiloClass + '">' + estado + '</span>';
                });
                config.opcionesDT.columnDefs = [
                    {
                        "targets": [0],
                        "visible": false,
                        "searchable": false
                    }
                ];
                //config.opcionesDT.scrollX = false;
                table.DataTable().destroy();

                table.DataTable(config.opcionesDT);
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();

                $('#tblDataTableReporte tbody').on('click', 'tr', function () {
                    var table = $('#tblDataTableReporte').DataTable();
                    var dataDetalle = table.row(this).data();

                    MostrarReporte(dataDetalle);
                });
            } else {

                $('#DivReporte').empty();
            
                $('#DivCabReportes').prop('hidden',true);
                $('#mensajeRegistros').text(Mensajes.SinRegistrosRangoFecha);
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            $('#cargac').hide();
            console.log(resultado);
            MensajeError(resultado, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })
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
});