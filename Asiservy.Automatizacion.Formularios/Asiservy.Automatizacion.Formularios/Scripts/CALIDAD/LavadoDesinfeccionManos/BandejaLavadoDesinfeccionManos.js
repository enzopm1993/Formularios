var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
    $('#tblDataTableDetalle tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableDetalle').DataTable();
        var dataCabecera = table.row(this).data();
        SeleccionarBandeja(dataCabecera);
    });
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var op = 3;
    if ($('#selectEstadoReporte').val() == 'true') {
        op = 2;
    }
    var table = $("#tblDataTableDetalle");
    table.DataTable().clear();
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();
    $.ajax({
        url: "../LavadoDesinfeccionManos/ConsultarControlLavadoDesinfeccionManos",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            opcion: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                //MensajeAdvertencia("No existen registros.");
                $("#divTablaAplrobados").html("No existen registros: "+resultado);
            } else {
                $("#btnPendiente").prop("hidden", true);
                $("#btnAprobado").prop("hidden", false);
                $("#divTablaAplrobados").show();
                $("#tblDataTableDetalle tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Fecha' },
                    { data: 'Observacion' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'EstadoReporteControl' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                $('#cargac').hide();
                var conRow = 0;
                resultado.forEach(function (row) {
                    var estado = 'PENDIENTE';
                    var css = 'badge-danger';
                    if (row.EstadoReporte == true) {
                        estado = 'APROBADO';
                        css = 'badge-success';
                    }
                    resultado[conRow].EstadoReporteControl = "<center><span class='badge " + css + "' >" + estado + "</span></center>";//Aplico estrilos al estadoReporte
                    conRow++;
                });
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
            }
            setTimeout(function () {
                $('#cargac').hide();
            }, 200);
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    $('#cargac').show();
    listaDatos = model;
    if (model.EstadoReporte == true) {
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }    
        var op = 0;
        $.ajax({
            url: "../LavadoDesinfeccionManos/BandejaLavadoDesinfeccionManosAprobarPartial",
            type: "GET",
            data: {
                IdDesinfeccionManos: listaDatos.IdDesinfeccionManos,
                opcion: op
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == "0") {
                    $("#divTblAprobarPendiente").html("No existen registros");
                } else {
                    $("#divTblAprobarPendiente").html('');
                    $("#ModalApruebaPendiente").modal("show");
                    $("#divTblAprobarPendiente").html(resultado);
                    //ocultarBotones();
                }                
                 $('#cargac').hide();                               
            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
}

function AprobarPendiente(estadoReporte) {
    $.ajax({
        url: "../LavadoDesinfeccionManos/GuardarModificarControlLavadoDesinfeccionManos",
        type: "POST",
        data: {
            IdDesinfeccionManos: listaDatos.IdDesinfeccionManos,
            Fecha: listaDatos.Fecha,
            Observacion: listaDatos.Observacion,
            EstadoReporte: estadoReporte
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else { MensajeError('El registro no debe guardase- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion'); }

            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];             
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

//DATE RANGE PICKER
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