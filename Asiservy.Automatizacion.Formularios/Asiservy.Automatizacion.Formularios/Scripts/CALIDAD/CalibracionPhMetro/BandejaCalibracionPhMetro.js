var Error = 0;
var IdControlAp;
$(document).ready(function () {
    CargarBandeja();

});
function ConfirmarAprobar(IdControl) {
    $('#ModalAprobar').modal('show');
    IdControlAp=IdControl
}
function ConfirmarReversar(IdControl) {
    $('#ModalReversar').modal('show');
    IdControlAp = IdControl
}
function AprobarControl() {
    Error = 0;
    
    const data = new FormData();
    data.append('IdControl', IdControlAp);
  
    fetch("../CalibracionPhMetro/AprobarControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalAprobar').modal('hide');
            MensajeCorrecto(resultado);
            
            CargarBandeja();
        }


    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function ReversarControl() {
    Error = 0;

    const data = new FormData();
    data.append('IdControl', IdControlAp);

    fetch("../CalibracionPhMetro/ReversarControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalReversar').modal('hide');
            MensajeCorrecto(resultado);

            CargarBandeja();
        }


    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function CargarBandeja() {
    $('#cargac').show();
    if ($("#cmbEstadoControl").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        $.ajax({
            url: "../CalibracionPhMetro/BandejaAprobadosCalibracionPhMetroPartial",
            type: "GET",
            data: {
                EstadoControl: false
            },
            success: function (resultado) {
                $('#DivCalibracionPhMetro').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivCalibracionPhMetro').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaPartial').DataTable(config.opcionesDT);
                }

                $('#cargac').hide();

                //$("#btnPendiente").prop("hidden", false);
                //$("#btnAprobado").prop("hidden", true);


            },
            error: function (resultado) {
                $('#cargac').hide();
                MensajeError(resultado.responseText, false);
            }
        });
    } else {
        $.ajax({
            url: "../CalibracionPhMetro/BandejaAprobadosCalibracionPhMetroPartial",
            type: "GET",
            data: {
                FechaInicio: $("#fechaDesde").val(),
                FechaFin: $("#fechaHasta").val(),
                EstadoControl: $("#cmbEstadoRegistro").val() == 'false' ? false : true
            },
            success: function (resultado) {
                $('#DivCalibracionPhMetro').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivCalibracionPhMetro').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaPartial').DataTable(config.opcionesDT);
                }
                $("#divDateRangePicker").prop('hidden', false);
                $('#cargac').hide();

                //$("#btnPendiente").prop("hidden", false);
                //$("#btnAprobado").prop("hidden", true);

                //$("#divDateRangePicker").prop('hidden', false);
            },
            error: function (resultado) {
                $('#cargac').hide();
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
