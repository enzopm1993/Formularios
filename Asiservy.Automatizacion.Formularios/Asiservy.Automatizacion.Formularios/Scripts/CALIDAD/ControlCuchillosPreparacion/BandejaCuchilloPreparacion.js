var listaDatos = [];
$(document).ready(function () {
    CargarBandeja();
    $('#selectEstadoReporte').select2({
        width: '100%'
    });
});

//CARGAR BANDEJA
function CargarBandeja() {
    $('#cargac').show();
    var op = 2;
    document.getElementById('divFiltroFecha').hidden = true;
    if ($('#selectEstadoReporte').val() == 'true') {
        op = 0;
        document.getElementById('divFiltroFecha').hidden = false;
    } 
    $.ajax({
        url: "../ControlCuchillosPreparacion/BandejaCuchilloPreparacionPartial",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            estado: $('#selectEstadoReporte').val(),
            op:op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
            }
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            
            $('#divTablaAplrobados').empty();
            $('#divTablaAplrobados').html(resultado);
            $('#cargac').hide();
            
           
            //$('#tblDataTable').DataTable({               
            //    ordering: true,
            //    columnDefs: [{
            //        orderData: [[0, 'asc'], [1, 'asc']],
            //        targets: [1]
            //    }]
            //});
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
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
    $.ajax({
        url: "../ControlCuchillosPreparacion/ReporteControlCuchilloPreparacionPartial",//MUESTRO EL DETALLE DE LA FILA SELECCIONADA
        data: {
            idControlCuchillo: model.IdControlCuchillo,
            op: 0
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divBotones').prop('hidden', true);
                $("#divMostarTablaCabecera").prop('hidden', false);
                $("#tblAprobarPendientePartial").prop('hidden', true);
                MensajeAdvertencia('No existen registro de DETALLE');
            }  else {
                $("#divMostarTablaCabecera").prop('hidden', true);
                $("#ModalApruebaPendiente").modal('show');
                $('#divBotones').prop('hidden', false);
                $("#tblAprobarPendientePartial").html(resultado);
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function AprobarPendiente(estadoReporte) {
    if ($("#selectEstadoReporte").val() == 'false') {
        var date = new Date();
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') < moment(listaDatos.Fecha).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.Fecha).format('DD-MM-YYYY') + '</span>');
            return;
        }
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val(''); }
        $.ajax({
            url: "../ControlCuchillosPreparacion/AprobarPendiente",
            type: "POST",
            data: {
                IdControlCuchillo: listaDatos.IdControlCuchillo,
                FechaAprobado: $('#txtFechaAprobado').val(),
                siAprobar: true,
                EstadoReporte: estadoReporte,
                Fecha: moment(listaDatos.Fecha).format('DD-MM-YYYY')
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                if (resultado == 1) {
                    MensajeCorrecto('¡Registro aprobado correctamente!');
                } else if (resultado == 2) { MensajeCorrecto('Estado actualizado correctamente'); }
                else if (resultado == 100) {
                    MensajeAdvertencia(Mensajes.MensajePeriodo);
                }else { MensajeError('El registro no debe guardase - solo actualizarce - Controller: GuardarModificarControlCuchilloPreparacion'); }
                
                $("#ModalApruebaPendiente").modal("hide");
                CargarBandeja();                
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

var configDetalle = {
    wsUrl: 'http://192.168.0.31:8870',
    baseUrl: '@Url.Content("~/")',
    opcionesDT: {
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            },
            "buttons": {
                "pageLength": "<img style='width:100%' src='../../Content/icons/show24.png' />"
            }
        },
        "pageLength": 0,
        "lengthMenu": [],
        "pagingType": "full_numbers",
        "dom": 'Bfrtip',
        //"scrollX": "auto",
        //"scrollY": "auto",
        "order": [[0, "desc"], [1, "desc"]],
        "buttons": []
    }
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