var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
});

//CARGAR BANDEJA
function CargarBandeja() {
    $("#spinnerCargando").prop("hidden", false);
    $("#txtFechaAprobacion").prop("hidden", true);
    $('#divPartialControl').html('');
    $.ajax({
        url: "../KardexReactivo/BandejaKardexReactivoPartial",
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
            $("#btnReversar").prop("hidden", true);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = 10;
            $('#tblDataTable').DataTable(config.opcionesDT);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    // console.log(model);
    //  CargarControlDetalle(model.IdKardexReactivo);
    if ($("#selectEstadoRegistro").val() == "false") {
        $("#txtFechaAprobacion").prop("hidden", false);
    } else {
        $("#txtFechaAprobacion").prop("hidden", true);
    }
   
    listaDatos = model;

    ConsultarControl();

}


function ConsultarControl() {
    if ($("#txtFecha").val() == '') {
        return;
    }
    MostrarModalCargando();
    $("#h4Mensaje").html("");
    $.ajax({
        url: "../KardexReactivo/KardexReactivoPartial",
        type: "GET",
        data: {
            Fecha: listaDatos.Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                mantenimientos.forEach(function (x) {
                    $("#txtReactivo-" + x).val("");
                    $("#txtReactivo-" + x).prop("disabled", false);
                });
                MensajeAdvertencia("Control no tiene registros");
            } else {
                mantenimientos.forEach(function (x) {
                    $("#txtReactivo-" + x).val("-");
                    $("#txtReactivo-" + x).prop("disabled", true);
                    resultado.forEach(function (y) {
                        if (x = y.IdReactivo) {
                            $("#txtReactivo-" + x).val(y.Valor);
                        }
                    });
                });

                $("#ModalApruebaCntrol").modal("show");
            }
            CerrarModalCargando();
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function AprobarControl() {
    //var estadoReporte = data;
    //console.log(listaDatos);
    //console.log(moment($("#txtFechaAprobacion").val()) < moment(listaDatos.FechaIngresoLog));
    if (moment($("#txtFechaAprobacion").val()).format('YYYY-MM-DD') < moment(listaDatos.Fecha).format('YYYY-MM-DD')) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser menor a la Fecha de Creación.");
        return;
    }
    if (moment($("#txtFechaAprobacion").val()).format('YYYY-MM-DD') > moment().format('YYYY-MM-DD')) {
        MensajeAdvertencia("Fecha de Aprobación no puede ser mayor a la fecha actual.");
        return;
    }

    $.ajax({
        url: "../KardexReactivo/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdKardexReactivoControl: listaDatos.IdKardexReactivoControl,
            Fecha: listaDatos.Fecha,
            FechaAprobacion: $("#txtFechaAprobacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else {
                MensajeCorrecto(resultado);
                CargarBandeja();
                $("#ModalApruebaCntrol").modal("hide");
            }
        },
        error: function (resultado) {
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
        }
    });
}


function ReversarControl() {
    //var estadoReporte = data;
    $.ajax({
        url: "../KardexReactivo/ReversarBandejaControl",
        type: "POST",
        data: {
            IdKardexReactivoControl: listaDatos.IdKardexReactivoControl,
            Fecha: listaDatos.Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else {
                MensajeCorrecto(resultado);
                FiltrarAprobadosFecha();
                $("#ModalApruebaCntrol").modal("hide");
            }
        },
        error: function (resultado) {
            MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
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
            url: "../KardexReactivo/BandejaKardexReactivoPartial",
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
                $("#btnReversar").prop("hidden", false);
                $("#btnAprobado").prop("hidden", true);
                $("#divDateRangePicker").prop('hidden', false);
            },
            error: function (resultado) {
                $("#spinnerCargando").prop("hidden", true);
                MensajeError('Error, Comuniquese con sistemas. ' + resultado.responseText, false);
            }
        });
    }
}


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