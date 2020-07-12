
var Error = 0;
var IdControlAp;
$(document).ready(function () {
    CargarBandeja();

});

function CargarBandeja() {
    $('#cargac').show();
    if ($("#cmbEstadoControl").val() == 'false') {
        $("#divDateRangePicker").prop('hidden', true);
        $.ajax({
            url: "../EntradaySalidaDeMaterialesEnAreasDeProceso/BandejaAprobadosEntradaySalidaDeMaterialesPartial",
            type: "GET",
            data: {
                EstadoControl: false
            },
            success: function (resultado) {
                $('#DivAnalisisQuimicoProductoSemiElaborado').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivAnalisisQuimicoProductoSemiElaborado').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaAnalisis').DataTable(config.opcionesDT);
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
            url: "../EntradaySalidaDeMaterialesEnAreasDeProceso/BandejaAprobadosEntradaySalidaDeMaterialesPartial",
            type: "GET",
            data: {
                FechaInicio: $("#fechaDesde").val(),
                FechaFin: $("#fechaHasta").val(),
                EstadoControl: $("#cmbEstadoRegistro").val() == 'false' ? false : true
            },
            success: function (resultado) {
                $('#DivAnalisisQuimicoProductoSemiElaborado').empty();
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();

                    $('#DivAnalisisQuimicoProductoSemiElaborado').html(resultado);
                    config.opcionesDT.pageLength = 30;
                    $('#tblBandejaAnalisisEvaluacion').DataTable(config.opcionesDT);
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
function AbrirModalDetalle(IdCabecera,fecha,estadocontrol) {
    CerrarConfirmacionAprobar();
    //TipoLimpieza = NivelLimpieza;
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabecera: IdCabecera
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
                $('#DivDetalle').empty();
                $('#DivDetalle').html(resultado);
                IdControlAp = IdCabecera;
                $('#txtfechaaprob').prop('readonly', true);


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
                        date: moment().format("YYYY-MM-DD HH:mm"),
                        format: "DD-MM-YYYY HH:mm",
                        minDate: fecha,
                        maxDate: moment(),
                        ignoreReadonly: true
                    });


                //config.opcionesDT.pageLength = 10;
                //$('#tblDetalleBandeja').DataTable(config.opcionesDT);
                if (estadocontrol == 'True') {
                    $('#btnAprobar').prop('hidden', true);
                    $('#btnReversar').prop('hidden', false);
                    $('#divfechaap').prop('hidden', true);
                } else {
                    $('#btnAprobar').prop('hidden', false);
                    $('#btnReversar').prop('hidden', true);
                    $('#divfechaap').prop('hidden', false);
                }
                //ConsultarFirma(IdCabecera);
                $('#ModalDetalle').modal('show');
            } else {
                $('#DivDetalles').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })

}

function ConfirmarAprobar() {
    if ($('#txtFechaAprob').val() == '') {
        $('#msjerrorfechaaprobacion').prop('hidden', false);
        return;
    } else {
        $('#msjerrorfechaaprobacion').prop('hidden', true);
        MensajeConfirmacion('divconfirm', 'ModalDetalle', 'AprobarControl()', '¿Está seguro que desea aprobar el control?');
    }
}
function ConfirmarReversar() {
    MensajeConfirmacion('divconfirm', 'ModalDetalle', 'ReversarControl()', '¿Está seguro que desea reversar el control?');
}
function AprobarControl() {

    Error = 0;
    $('#BtnSi').prop('hidden', true);
    $('#BtnNo').prop('hidden', true);
    $('#btnCargando').prop('hidden', false);

    $('#btnAprobar').prop('disabled', true);
    $('#btnclose').prop('disabled', true);
    $('#btncerrar').prop('disabled', true);

    //var canvas = document.getElementById("firmacanvas");
    //var image = canvas.toDataURL('image/png').replace('data:image/png;base64,', '');

    var fechaingresada = $('#datetimepicker1').datetimepicker('viewDate');
    //console.log(fechaingresada);
    const data = new FormData();
    data.append('IdCabecera', IdControlAp);
    data.append('Fecha', moment(fechaingresada._d).format('YYYY-MM-DD HH:mm'));
    //data.append('imagen', image);
    fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/AprobarControl", {
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
            MensajeCorrecto(resultado);
            $('#ModalDetalle').modal('hide');
            CargarBandeja();
        }
        $('#BtnSi').prop('hidden', false);
        $('#BtnNo').prop('hidden', false);
        $('#btnCargando').prop('hidden', true);

        $('#btnAprobar').prop('disabled', false);
        $('#btnclose').prop('disabled', false);
        $('#btncerrar').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado, false);
            $('#btnAprobar').prop('disabled', false);

            $('#btnclose').prop('disabled', false);
            $('#btncerrar').prop('disabled', false);

            $('#BtnSi').prop('hidden', false);
            $('#BtnNo').prop('hidden', false);
            $('#btnCargando').prop('hidden', true);


        })


}
function ReversarControl() {
    Error = 0;
    $('#BtnSi').prop('hidden', true);
    $('#BtnNo').prop('hidden', true);
    $('#btnCargando').prop('hidden', false);

    $('#btnReversar').prop('disabled', true);
    $('#btnclose').prop('disabled', true);
    $('#btncerrar').prop('disabled', true);



    const data = new FormData();
    data.append('IdCabecera', IdControlAp);

    fetch("../EntradaySalidaDeMaterialesEnAreasDeProceso/ReversarControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {

            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {

        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            MensajeCorrecto(resultado);
            $('#ModalDetalle').modal('hide');
            CargarBandeja();
        }
        $('#BtnSi').prop('hidden', false);
        $('#BtnNo').prop('hidden', false);
        $('#btnReversar').prop('disabled', false);
        $('#btnCargando').prop('hidden', true);
        $('#btnclose').prop('disabled', false);
        $('#btncerrar').prop('disabled', false);
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado, false);
            $('#btnReversar').prop('disabled', true);
            $('#btnclose').prop('disabled', false);
            $('#btncerrar').prop('disabled', false);

            $('#BtnSi').prop('hidden', false);
            $('#BtnNo').prop('hidden', false);
            $('#btnCargando').prop('hidden', true);
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

