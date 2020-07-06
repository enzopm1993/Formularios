var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
    $('#tblDataTableDetalle tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableDetalle').DataTable();
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
    var op = 3;
    document.getElementById('divFiltroFecha').hidden = true;
    if ($('#selectEstadoReporte').val() == 'true') {
        op = 2;
        document.getElementById('divFiltroFecha').hidden = false;
    }
    var table = $("#tblDataTableDetalle");
    table.DataTable().clear();
    table.DataTable().destroy();
    table.DataTable().draw();
    $.ajax({
        url: "../LavadoDesinfeccionManos/BandejaLavadoDesinfeccionManosJson",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            op: op
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
                    { data: 'tr' },
                    { data: 'EstadoReporteControl' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                $('#cargac').hide();
                var conRow = 0;
                resultado.forEach(function (row) {
                    var estado = 'PENDIENTE';
                    row.FechaComparar = row.Fecha;
                    row.Fecha = moment(row.Fecha).format('DD-MM-YYYY');
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
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    $('#cargac').show();
    listaDatos = model;
    var date = new Date();
    $('#txtFechaAprobado').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    $('#cargac').show();
    if (model.EstadoReporte == true) {
        $('#txtFechaAprobado').prop('hidden', true);
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#txtFechaAprobado').prop('hidden', false);
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
                    MensajeAdvertencia('¡Este registro no contiene detalle¡');
                } else {
                    $("#divTblAprobarPendiente").html('');
                    $("#ModalApruebaPendiente").modal("show");
                    $("#divTblAprobarPendiente").html(resultado);
                    //ocultarBotones();
                }
                setTimeout(function () {
                    $('#cargac').hide();  
                },200);
                                            
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
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') < moment(listaDatos.FechaComparar).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.FechaComparar).format('DD-MM-YYYY') + '</span>');
            return;
        }
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val(''); }
    $.ajax({
        url: "../LavadoDesinfeccionManos/GuardarModificarControlLavadoDesinfeccionManos",
        type: "POST",
        data: {
            IdDesinfeccionManos: listaDatos.IdDesinfeccionManos,
            FechaAprobado: $('#txtFechaAprobado').val(),
            siAprobar: true,
            EstadoReporte: estadoReporte
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 1 || resultado==2) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else if (resultado == 0) { MensajeError('El registro no debe guardase- solo actualizarce- Controller: GuardarModificarControlCuchilloPreparacion'); }
           
            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];             
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
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