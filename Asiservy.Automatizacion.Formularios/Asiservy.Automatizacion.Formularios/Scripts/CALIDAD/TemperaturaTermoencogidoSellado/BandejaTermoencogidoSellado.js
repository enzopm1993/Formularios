var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
    $('#tblDataTable tbody').on('click', 'tr', function () {
        var table = $('#tblDataTable').DataTable();
        var dataCabecera = table.row(this).data();
        SeleccionarBandeja(dataCabecera);
    });
    $('#selectEstadoReporte').select2({
        width: '100%'
    });
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();   
    var estadoReporte = $('#selectEstadoReporte').val();
    $('#divCalendar').prop('hidden', true);
    if (estadoReporte == 'true') {
        $('#divCalendar').prop('hidden', false);
    }
    var table = $("#tblDataTable");
    table.DataTable().clear();
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/ConsultarBandejaTermoencogidoSellado",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            estadoReporte: estadoReporte
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                //MensajeAdvertencia("No existen registros.");
                $("#divTablaAplrobados").html("No existen registros: " + resultado);
            } else {
                $("#btnPendiente").prop("hidden", true);
                $("#btnAprobado").prop("hidden", false);
                $("#divTablaAplrobados").show();
                $("#tblDataTable tbody").empty();
                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'Fecha' },
                    { data: 'Observacion' },
                    { data: 'UsuarioIngresoLog' },
                    { data: 'tr' },
                    { data: 'EstadoReporteControl' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                $('#cargac').hide();
                var conRow = 0;
                resultado.forEach(function (row) {
                    row.FechaComparar = row.Fecha;
                    row.Fecha = moment(row.Fecha).format('DD-MM-YYYY');                    
                                 
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
                //table.DataTable().destroy();
                var tr = document.getElementById("acoplar");
                var tds = tr.getElementsByTagName("td");

                for (var i = 1; i < tds.length; i++) {
                    tds[i].style.whiteSpace = 'normal';
                }

            }
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function SeleccionarBandeja(model) {
    if (model.EstadoReporte == true) {
        $('#divfechaap').prop('hidden', true);
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#divfechaap').prop('hidden', false);
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    $.fn.datetimepicker.Constructor.Default = $.extend({}, $.fn.datetimepicker.Constructor.Default, {
        icons: {
            time: 'far fa-clock',
            date: 'far fa-calendar-alt',
            up: 'fas fa-caret-up',
            down: 'fas fa-caret-down',
            previous: 'fas fa-backward',
            next: 'fas fa-forward',
            today: 'fas fa-calendar-day',
            clear: 'fas fa-trash-alt',
            close: 'fas fa-window-close'
        }
    });
    $('#datetimepicker1').datetimepicker(
        {
            date: moment().format("DD-MM-YYYY HH:mm"),
            format: "DD-MM-YYYY HH:mm",
            minDate: model.Fecha,
            maxDate: moment(),
            ignoreReadonly: true
        });
    listaDatos = model;
    var date = new Date();
    $('#txtFechaAprobado').val(moment(date).format('DD-MM-YYYY HH:mm'));
    $('#cargac').show();
   
    var op = 0;
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/BandejaTermoencogidoSelladoPartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            id: listaDatos.Id,
            opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTblAprobarPendiente").html("No existen registros");
            } else {
                $("#tblAprobarPendientePartial").html('');
                $("#ModalApruebaPendiente").modal("show");
                $("#divAprobarPendientePartial").html(resultado);                
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
    if ($("#selectEstadoReporte").val() == 'false') {
        var date = new Date();
        if (moment($('#txtFechaAprobado').val()).format('YYYY-DD-MM') < moment(listaDatos.FechaComparar).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.FechaComparar).format('DD-MM-YYYY')+ '</span>');
            return;
        }
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val(''); }
    $.ajax({
        url: "../TemperaturaTermoencogidoSellado/GuardarModificarTermoencogidoSellado",
        type: "POST",
        data: {
            Id: listaDatos.Id,
            FechaAprobado: $('#txtFechaAprobado').val(),
            Fecha: listaDatos.Fecha,
            siAprobar: true,
            EstadoReporte: estadoReporte
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1 || resultado==2) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else if(resultado == 0){
                MensajeError('El registro no debe guardarse- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion');
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }

            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
        }
    });
}

function validar() {
    if ($('#txtFechaAprobado').val() == '') {
        $("#txtFechaAprobado").css('border', '1px dashed red');
        MensajeAdvertencia('Fecha invalida');
        return;
    } else {
        $("#txtFechaAprobado").css('border', '');
    }
}

function LimpiarFecha() {
    $("#txtFechaAprobado").css('border', '');
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