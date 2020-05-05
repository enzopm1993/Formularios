var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});

//CARGAR BANDEJA
function CargarBandeja() {
    $("#spinnerCargando").prop("hidden", false);
    $('#divPartialControl').html('');
    $.ajax({
        url: "../OperatividadMetal/BandejaOperatividadMetalPartial",
        type: "GET",
        success: function (resultado) {
            $('#divPartialControl').html('');
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControl').html(resultado);
            }
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = 10;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    //var table = $("#tblDataTableAprobar");
    //table.DataTable().clear();

    console.log(model);
    $("#ModalApruebaCntrol").modal("show");
    //listaDatos = model;
    //$.ajax({
    //    url: "../OperatividadMetal/BandejaAprobarOperatividadMetal",
    //    type: "GET",
    //    data: {
    //        Fecha: model.Fecha
    //    },
    //    success: function (resultado) {
    //        console.log(resultado);
    //        if (resultado == "101") {
    //            window.location.reload();
    //        }
    //        if (resultado == "102") {
    //            MensajeAdvertencia("No existen datos para este model.");
    //        }
    //        if (resultado == "0") {
    //            MensajeAdvertencia("¡El REGISTRO no tiene detalle, por favor ingrese los datos en el CONTROL!");
    //        } else {
    //            $("#tblDataTableAprobar tbody").empty();
    //            config.opcionesDT.order = [];
    //            config.opcionesDT.columns = [
    //                { data: 'Fecha' },
    //                { data: 'Hora' },
    //                { data: 'Cedula' },
    //                { data: 'Nombre' },
    //                { data: 'Condicion' },
    //                { data: 'Observacion' }
    //            ];
    //            table.DataTable().destroy();
    //            table.DataTable(config.opcionesDT);
    //            resultado.forEach(function (row) {
    //                row.Fecha = moment(row.Fecha).format('YYYY-MM-DD');
    //                row.Hora = moment(row.Hora).format('HH:mm');
    //            });

    //            table.DataTable().rows.add(resultado);
    //            table.DataTable().draw();
    //            $("#ModalApruebaCntrolCloro").modal("show");
    //        }
    //    },
    //    error: function (resultado) {
    //        MensajeError(resultado.responseText, false);
    //        $("#spinnerCargandoMaterial").prop("hidden", true);
    //    }
    //});
}

function AprobarControlCloroDetalle(data) {
    //var estadoReporte = data;
    $.ajax({
        url: "../OperatividadMetal/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdOperatividadMetal: listaDatos.IdOperatividadMetal,
            Fecha: listaDatos.Fecha

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            CargarBandeja();
            $("#ModalApruebaCntrolCloro").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function FiltrarAprobadosFecha() {
    if ($("#selectEstadoRegistro").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        CargarBandeja();

    } else {
        $('#divPartialControl').html('');
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../OperatividadMetal/BandejaOperatividadMetalPartial",
            type: "GET",
            data: {
                FechaDesde: $("#fechaDesde").val(),
                FechaHasta: $("#fechaHasta").val(),
                Estado: $("#selectEstadoRegistro").val()
            },
            success: function (resultado) {
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                    $('#divPartialControl').html('');

                } else {
                    $('#MensajeRegistros').hide();
                    $('#divPartialControl').html(resultado);
                }
                $("#spinnerCargando").prop("hidden", true);
                $("#btnPendiente").prop("hidden", false);
                $("#btnAprobado").prop("hidden", true);
                $("#divDateRangePicker").prop('hidden', false);
            },
            error: function (resultado) {
                $("#spinnerCargando").prop("hidden", true);
                MensajeError(resultado.responseText, false);
            }
        });
    }
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