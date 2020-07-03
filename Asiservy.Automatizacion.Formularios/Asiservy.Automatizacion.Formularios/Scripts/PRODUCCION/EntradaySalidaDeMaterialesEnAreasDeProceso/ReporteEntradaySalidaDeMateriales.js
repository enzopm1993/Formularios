function MostrarReporte(data) {
    console.log(data);
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabecera: data.IdControlEntradaSalidaMateriales
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EntradaySalidaDeMaterialesEnAreasDeProceso/PartialReporteControl?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (resultado != '"0"') {

                $('#DivBotonImpr').prop('hidden', false);
                $('#DivReporte').prop('hidden', false);
                $('#DivCabReportes').prop('hidden', true);
                $('#DivReporte').empty();
                $('#DivReporte').html(resultado);
                $('#lblUsuarioIngreso').text(data.UsuarioIngresoLog);
                $('#lblFechaIngreso').text(data.FechaIngresoLog.slice(0, -6));
                $('#lblAprobadoPor').text(data.AprobadoPor);
                $('#lblFechaAprobacion').text(data.FechaAprobacion);
                $('#lblFechap').text(data.Fecha);
                $('#lblObservacionp').text(data.Observacion);
                $('#lblturno').text(data.turno);
                $('#lblLinea').text(data.Linea);
                $('#mensajeRegistros').prop('hidden', true);
                //config.opcionesDT.pageLength = 30;
                //$('#tblReporte').DataTable(config.opcionesDT);
                //LimpiarDetalleControles();
            } else {
                $('#DivReporte').empty();
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })

        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })

}
function imprimirw() {
    window.print();
}

function Atras() {
    $('#DivBotonImpr').prop('hidden', true);
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
        FechaHasta: $('#fechaHasta').val(),
        CodLinea: $('#codLinea').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../EntradaySalidaDeMaterialesEnAreasDeProceso/ConsultarCabecerasReporte?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            console.log(resultado);
            if (resultado == '101') {
                window.location.reload();
            }
            if (resultado != '0') {
                console.log(resultado);
                $('#mensajeRegistros').prop('hidden', true);
                $("#tblDataTableReporte tbody").empty();
                $('#DivCabReportes').prop('hidden', false);
                config.opcionesDT.columns = [
                    { data: 'IdControlEntradaSalidaMateriales' },
                    { data: 'Fecha' },
                    { data: 'Linea' },
                    { data: 'turno' },
                    { data: 'Observacion' },
                    { data: 'FechaIngresoLog' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'TerminalIngresoLog' },
                    { data: 'FechaModificacionLog' },
                    { data: 'UsuarioModificacionLog' },
                    { data: 'TerminalModificacionLog' },
                    { data: 'EstadoControl' },
                    { data: 'AprobadoPor' },
                    { data: 'FechaAprobacion' }
                ];

                resultado.forEach(function (row) {
                    row.Fecha = moment(row.Fecha).format('DD-MM-YYYY');
                    if (row.FechaAprobacion != null) {
                        row.FechaAprobacion = moment(row.FechaAprobacion).format('DD-MM-YYYY HH:mm');
                    }
                    if (row.FechaIngresoLog != null) {
                        row.FechaIngresoLog = moment(row.FechaIngresoLog).format('DD-MM-YYYY HH:mm');
                    }
                    if (row.FechaModificacionLog != null) {
                        row.FechaModificacionLog = moment(row.FechaModificacionLog).format('DD-MM-YYYY HH:mm');
                    }
                    var estiloTrue = '<i class="fas fa-check-square" style="color:green"></i>'
                    var estiloClass = 'badge badge-danger';
                    var estado = 'PENDIENTE';
                    if (row.Observacion != null) {
                        row.Observacion = row.Observacion.toUpperCase();
                    }
                    if (row.EstadoControl == true) {
                        estiloClass = 'badge badge-success';
                        estado = 'APROBADO';
                    }

                    row.EstadoControl = '<span class="' + estiloClass + '">' + estado + '</span>';
                });
                config.opcionesDT.columnDefs = [
                    {
                        "targets": [0],
                        "visible": false,
                        "searchable": false
                    }
                ];

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
                $('#DivCabReportes').prop('hidden', true);
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