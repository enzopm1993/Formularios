var listaDatos = [];
$(document).ready(function () { 
    CargarReporteControlCloro(0);    
});
function CargarReporteControlCloroCabecera(op) {
    listaDatos = null;  
   
        $.ajax({
            url: "../CloroCisternaDescongelado/ValidarCloroCisternaDescongelado",
            type: "GET",
            data: {
                fechaDesde: $('#txtFecha').val(),
                fechaHasta: $('#txtFecha').val(),
                idCloroCisterna: 0,
                op: op
            },
            success: function (resultado) {
                listaDatos = resultado;
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    
}

//function CargarReporteControlCloro(op) {
//    $('#cargac').show();
//    var table = $("#tblDataTable");
//    table.DataTable().destroy();
//    table.DataTable().clear();

//    $.ajax({
//        url: "../CloroCisternaDescongelado/ReporteCloroCisternaDescongeladoCabecera",
//        type: "GET",
//        data: {
//            fechaDesde: $('#fechaDesde').val(),
//            fechaHasta: $('#fechaHasta').val(),
//            idCloroCisterna: 0,
//            op: op
//        },
//        success: function (resultado) {
//            //config.opcionesDT.buttons = [];
//            config.opcionesDT.buttons=[];
//            if (resultado == "101") {
//                window.location.reload();
//            }
//            if (resultado == "0") {
//                //$('#MensajeRegistros').prop("hidden", false);
//            } else {
//                $("#tblDataTableReporte tbody").empty();
//                $("#DivReporteControlCloro").show();
//                //config.opcionesDT.order = [1,'asc'];

//                config.opcionesDT.columns = [
//                    { data: 'Fecha' },
//                    { data: 'HoraDet' },
//                    { data: 'Ppm_CloroDet' },
//                    { data: 'IdCisternaDet' },
//                    { data: 'ObservacionDet' },
//                    { data: 'AprobadoPor' },
//                    { data: 'EstadoReporte' }
//                ];

//                config.opcionesDT.aoColumnDefs = [{
//                    "aTargets": [2], // Columna a realizar la operacion
//                    "mRender": function (data, type, full) {
//                        var clscolor = "badge-danger";
//                        if (data >= 0.3 && data <= 1.5) {
//                            clscolor = "badge-success";
//                        }
//                        return '<span class="badge ' + clscolor + '">' + data + '</span>';
//                    }
//                }];
//                resultado.forEach(function (row) {
//                    row.Fecha = moment(row.Fecha).format('DD-MM-YYYY');
//                    row.HoraDet = moment(row.HoraDet).format("HH:mm");
//                    var estiloClass = 'badge badge-danger';
//                    var estado = 'PENDIENTE';
//                    if (row.ObservacionDet!=null) {
//                        row.ObservacionDet = row.ObservacionDet.toUpperCase();
//                    }
//                    if (row.EstadoReporte == true) {
//                        estiloClass = 'badge badge-success';
//                        estado = 'APROBADO';
//                    }
//                    row.EstadoReporte = '<span class="' + estiloClass + '">' + estado + '</span>';
//                });
//                table.DataTable().destroy();
//                table.DataTable(config.opcionesDT);
//                table.DataTable().clear();
//                table.DataTable().rows.add(resultado);
//                table.DataTable().draw();
//            }
//            $('#cargac').hide();
//        },
//        error: function (resultado) {
//            $('#cargac').hide();
//            MensajeError(resultado, false);
//        }
//    });
//}

function CargarReporteControlCloro(op) {
    $('#cargac').show();
   
    $.ajax({
        url: "../CloroCisternaDescongelado/ReporteCloroCisternaDescongeladoPartial",
        type: "GET",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: $('#fechaHasta').val(),
            idCloroCisterna: 0,
            op: op
        },
        success: function (resultado) {
            config.opcionesDT.buttons = [];
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#DivReporteControlCloro").html("No existen registros");
            } else {
                $('#DivReporteControlCloro').html(resultado);
            }
            setTimeout(function () {
                $('#cargac').hide();
            },200);            
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado, false);
        }
    });
}


function PrintReport(op) {
    var url = $("#RedirectTo").val() + '?' + 'fechaDesde=' + $("#fechaDesde").val() + '&fechaHasta=' + $("#fechaHasta").val() + '&id=' + 0 + '&op=' + op;
    window.open(url, '_blank');
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