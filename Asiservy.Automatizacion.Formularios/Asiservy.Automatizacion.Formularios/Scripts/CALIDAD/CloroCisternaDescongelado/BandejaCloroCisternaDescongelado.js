var listaDatos = [];
    $(document).ready(function () {
        FiltrarAprobadosFecha();
});
var configModal = {
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
        "pageLength": -1,
        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "Todos"]],
        "pagingType": "full_numbers",
        "dom": 'Bfrtip',
        "order": [[0, "desc"], [1, "desc"]],
        "buttons": []
    }
}
//CARGAR BANDEJA
function CargarBandeja() {
    var table = $("#tblDataTable");
    table.DataTable().clear();
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../CloroCisternaDescongelado/BandejaCloroCisternaDescongeladoPartial",
        type: "GET",       
        success: function (resultado) {
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
            } 
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            $("#spinnerCargando").prop("hidden", true);
            $('#divPartialControlCloro').empty();
            $('#divPartialControlCloro').html(resultado);
        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    var table = $("#tblDataTableAprobar");        
        table.DataTable().clear();    
        listaDatos = model;
    $.ajax({
        url: "../CloroCisternaDescongelado/BandejaAprobarCloroCisternaDescongelado",
        type: "GET",
        data: {
            Fecha:model.Fecha,
            IdCloroCisterna: model.IdCloroCisterna
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros.");
            } else {
                $("#tblDataTableAprobar tbody").empty();
                configModal.opcionesDT.order = [];
                configModal.opcionesDT.columns = [
                    { data: 'Hora' },
                    { data: 'Ppm_Cloro' },
                    { data: 'Cisterna' },
                    { data: 'UsuarioIngresoLog'},
                    { data: 'Observaciones' }
                ];
                table.DataTable().destroy();
                table.DataTable(configModal.opcionesDT);                
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
                $("#ModalApruebaCntrolCloro").modal("show");
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
}

function AprobarControlCloroDetalle(data) {
    var estadoReporte = data;
    $.ajax({
        url: "../CloroCisternaDescongelado/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdCloroCisterna: listaDatos.IdCloroCisterna,
            Fecha: listaDatos.Fecha,
            Observaciones: listaDatos.Observaciones,
            EstadoReporte:estadoReporte
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            FiltrarAprobadosFecha();
            $("#ModalApruebaCntrolCloro").modal("hide");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function FiltrarAprobadosFecha() {       
    if ($("#selectEstadoRegistro").val() == 'false') {
        CargarBandeja();
    } else {    
        $("#spinnerCargando").prop("hidden", false);
        $.ajax({
            url: "../CloroCisternaDescongelado/BandejaAprobadosCloroCisternaDescongeladoPartial",
            type: "GET",
            data: {
                fechaInicio: $("#fechaDesde").val(),
                fechaFin: $("#fechaHasta").val(),
                estadoRegistro: $("#selectEstadoRegistro").val()
            },
            success: function (resultado) {
                $("#spinnerCargando").prop("hidden", true);
                $('#divPartialControlCloro').html(resultado);               
                $("#btnPendiente").prop("hidden", false);
                $("#btnAprobado").prop("hidden", true);
                if (resultado == '0') {
                    $('#MensajeRegistros').show();
                } else {
                    $('#MensajeRegistros').hide();
                }
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

    var iconLoader = "fa-spinner fa-pulse";
    var iconSearch = "fa-search"
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