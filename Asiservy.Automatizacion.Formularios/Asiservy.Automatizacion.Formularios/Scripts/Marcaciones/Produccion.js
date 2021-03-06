﻿$(function () {

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
    var table = $("#tblDataTable");

    $("#generarMarcaciones").click(function () {
        table.DataTable().destroy();
        table.DataTable().clear();
        table.DataTable().draw();

        var fechaDesde = $("#fechaDesde").val();
        var fechaHasta = $("#fechaHasta").val();
        var metodo = "P";

        var f1 = moment(fechaDesde);
        var f2 = moment(fechaHasta);
        var diffDays = f2.diff(f1, 'days');


        $("#generarMarcaciones").attr('href', "javascript:void(0)");
        $("#iconSearch").removeClass(iconSearch);
        $("#iconSearch").addClass(iconLoader);
        $("#generarMarcaciones").addClass("btnWait");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Marcaciones/ObtenerMarcacionesEmpleados",
            type: "GET",
            data: {
                'fechaIni': fechaDesde,
                'fechaFin': fechaHasta,
                'desde': metodo
            },
            success: function (resultado) {
                $("#generarMarcaciones").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarMarcaciones").removeClass("btnWait");

                console.log(resultado);
                $("#tblDataTable tbody").empty();

                config.opcionesDT.order = [];
                config.opcionesDT.columns = [
                    { data: 'FECHA_MARCA' },
                    { data: 'DIA' },
                    { data: 'LINEA' },
                    { data: 'EMPLEADO' },
                    { data: 'INGRESO' },
                    { data: 'SALIDA' }
                ];
                table.DataTable().destroy();
                table.DataTable(config.opcionesDT);
                //table.DataTable().draw();
                table.DataTable().clear();
                table.DataTable().rows.add(resultado);
                table.DataTable().draw();
            },
            error: function (resultado) {
                console.log(resultado);
                $("#generarMarcaciones").attr('href', "#");
                $("#iconSearch").removeClass(iconLoader);
                $("#iconSearch").addClass(iconSearch);
                $("#generarMarcaciones").removeClass("btnWait");
                MensajeError(resultado.statusText, false);

            }
        });

        return false;

    });
    $("#generarMarcaciones").trigger('click');
});