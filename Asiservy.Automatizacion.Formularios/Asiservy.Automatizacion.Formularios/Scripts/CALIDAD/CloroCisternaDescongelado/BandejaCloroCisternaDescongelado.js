﻿var listaDatos = [];
    $(document).ready(function () {
        CargarBandeja();
        $('#selectEstadoRegistro').select2({
            width: '100%'
        });
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
    $("#divDateRangePicker").prop('hidden', true);
    if (document.getElementById('selectEstadoRegistro').value=='true') {
        $("#divDateRangePicker").prop('hidden', false);
    }
    $.ajax({
        url: "../CloroCisternaDescongelado/BandejaCloroCisternaDescongeladoPartial",
        type: "GET",  
        data: {
            fechaDesde: document.getElementById('fechaDesde').value,
            fechaHasta: document.getElementById('fechaHasta').value,
            estadoReporte: document.getElementById('selectEstadoRegistro').value
        },
        success: function (resultado) {
            $('#divPartialControlCloro').empty();
            if (resultado == '0') {
                $('#MensajeRegistros').show();
            } else {
                $('#MensajeRegistros').hide();
                $('#divPartialControlCloro').html(resultado);
            } 
            $("#btnPendiente").prop("hidden", true);
            $("#btnAprobado").prop("hidden", false);
            //$("#spinnerCargando").prop("hidden", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function SeleccionarBandeja(model) {
    if (model.EstadoReporte == true) {
        $('#txtFechaAprobado').prop('hidden', true);
        $('#btnAprobado').prop('hidden', true);
        $('#btnPendiente').prop('hidden', false);
    } else {
        $('#txtFechaAprobado').prop('hidden', false);
        $('#btnPendiente').prop('hidden', true);
        $('#btnAprobado').prop('hidden', false);
    }
    var date = new Date();
    $('#txtFechaAprobado').val(moment(date).format('YYYY-MM-DDTHH:mm'));
    var table = $("#tblDataTableAprobar");        
    table.DataTable().clear();    
    
    $("#ModalAgregarDetalle").modal("show");
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
                MensajeAdvertencia("¡El REGISTRO no tiene detalle, por favor ingrese los datos en el CONTROL!");
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
                resultado.forEach(function (row) {
                    row.Hora = moment(row.Hora).format('DD-MM-YYYY HH:mm');
                    var estilo = document.getElementById('coloroFuera').value;
                    if (row.Ppm_Cloro >= document.getElementById('paramMin').value && row.Ppm_Cloro <= document.getElementById('paramMax').value) {
                        estilo = document.getElementById('colorRango').value;
                    }
                    row.Ppm_Cloro = '<span class="badge" style="color:white;background-color:'+estilo+'">'+row.Ppm_Cloro+'</span>';
                });
                table.DataTable().destroy();
                table.DataTable(configModal.opcionesDT);      
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
                table.DataTable().destroy();
                var tr = document.getElementById("acoplar");
                var tds = tr.getElementsByTagName("td");

                for (var i = 4; i < tds.length; i++) {
                    tds[i].style.whiteSpace = 'normal';
                }

                $("#ModalApruebaCntrolCloro").modal("show");
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
            //$("#spinnerCargandoMaterial").prop("hidden", true);
        }
    });
}

function AprobarControlCloroDetalle(data) {
    if ($("#selectEstadoRegistro").val() == 'false') {
        var date = new Date();
        if ($('#txtFechaAprobado').val() < moment(listaDatos.Fecha).format('YYYY-MM-DDTHH:mm')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser menor a la fecha de creacion del reporte: <span class="badge badge-danger">' + moment(listaDatos.Fecha).format('DD-MM-YYYY') + '</span>');
            return;
        }
        if (moment($('#txtFechaAprobado').val()).format('YYYY-MM-DD') > moment(date).format('YYYY-MM-DD')) {
            MensajeAdvertencia('La fecha de APROBACION no puede ser mayor a la fecha actual: <span class="badge badge-danger">' + moment(date).format('DD-MM-YYYY') + '</span>');
            return;
        }
    } else { $('#txtFechaAprobado').val('');}
    var estadoReporte = data;
    $.ajax({
        url: "../CloroCisternaDescongelado/AprobarBandejaControlCloro",
        type: "POST",
        data: {
            IdCloroCisterna: listaDatos.IdCloroCisterna,
            FechaAprobacion: $('#txtFechaAprobado').val(),
            EstadoReporte: estadoReporte,
            Fecha: moment(listaDatos.Fecha).format('YYYY-MM-DD')
        },
        success: function (resultado) {
            $("#ModalApruebaCntrolCloro").modal("hide");
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                return;
            }
            MensajeCorrecto(resultado);
            CargarBandeja();
            listaDatos = [];
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

//function FiltrarAprobadosFecha() {
//    LimpiarFecha();
//    if ($("#selectEstadoRegistro").val() == 'false') {
//        $("#divDateRangePicker").prop('hidden', true);
//        $('#txtFechaAprobado').prop('hidden', false);
//        $("#btnPendiente").prop("hidden", true);
//        $("#btnAprobado").prop("hidden", false);
//        CargarBandeja();
//    } else {
//        $("#divDateRangePicker").prop('hidden', false);
//        $('#txtFechaAprobado').prop('hidden', true);
//        $('#txtFechaAprobado').val('');
//        $("#btnPendiente").prop("hidden", false);
//        $("#btnAprobado").prop("hidden", true);
//        $.ajax({
//            url: "../CloroCisternaDescongelado/BandejaAprobadosCloroCisternaDescongeladoPartial",
//            type: "GET",
//            data: {
//                fechaInicio: $("#fechaDesde").val(),
//                fechaFin: $("#fechaHasta").val(),
//                estadoRegistro: $("#selectEstadoRegistro").val()
//            },
//            success: function (resultado) {
//                $('#divPartialControlCloro').empty();
//                if (resultado == '0') {
//                    $('#MensajeRegistros').show();
//                    $('#divPartialControlCloro').html('');

//                } else {
//                    $('#MensajeRegistros').hide();
//                    $('#divPartialControlCloro').html(resultado);
//                }
//                //$("#spinnerCargando").prop("hidden", true);
//                $("#btnPendiente").prop("hidden", false);
//                $("#btnAprobado").prop("hidden", true);
//                //$("#divDateRangePicker").prop('hidden', false);
//            },
//            error: function (resultado) {
//                //$("#spinnerCargando").prop("hidden", true);
//                MensajeError(resultado.responseText, false);
//            }
//        });
//    }
//}

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