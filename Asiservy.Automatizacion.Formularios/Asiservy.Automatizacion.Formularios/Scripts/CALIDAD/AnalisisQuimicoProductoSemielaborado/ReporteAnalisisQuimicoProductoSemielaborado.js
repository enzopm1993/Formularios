DevExpress.localization.locale(navigator.language);
var opciosGrid = {
    //loadPanel: {
    //    enabled: true
    //},
    //dataSource: resultado,
    keyExpr: "IdAnalisisQuimicoProductoSe",
    selection: {
        mode: "single"
    },
    hoverStateEnabled: true,
    showColumnLines: true,
    showRowLines: true,
    rowAlternationEnabled: true,
    showBorders: true,
    allowColumnResizing: true,
    columnResizingMode: "nextColumn",
    columnMinWidth: 50,
    columnAutoWidth: true,
    columnFixing: {
        enabled: true
    },
    showBorders: true,
    showRowLines: true,
    filterRow: {
        visible: true,
        applyFilter: "auto"
    },
    headerFilter: {
        visible: true
    },
    paging: {
        enabled: true,
        pageSize: 5
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [5, 10,0],
        showInfo: true,
        //visible: true,
        showNavigationButtons: true,
        infoText: "Página #{0}. Total: {1} ({2} filas)"
    },
    searchPanel: { visible: true },
    columns: [
        {
            caption: "Fecha",
            dataField: "Fecha",
            area: "column",
            dataType: "date",
        },
        "turno",
        {
            dataField: "Observacion",
            width: 180
        }
        , {
            caption: "Fecha ingreso log",
            dataField: "FechaIngresoLog",
            area: "column",
            dataType: "datetime"
        }
        , {
            caption: "Usuario ingreso log",
            dataField: "UsuarioIngresoLog",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Terminal ingreso log",
            dataField: "TerminalIngresoLog",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Usuario modificación log",
            dataField: "UsuarioModificacionLog",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Fecha modificación log",
            dataField: "FechaModificacionLog",
            area: "column",
            dataType: "datetime"
        }
        , {
            caption: "Terminal modificación log",
            dataField: "TerminalModificacionLog",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Estado control",
            dataField: "EstadoControl",
            area: "column",
            dataType: "boolean",
            cellTemplate: function (container, options) {
                var estiloClass = 'badge badge-danger';
                var estado = 'PENDIENTE';
                if (options.value) {
                    var estiloClass = 'badge badge-success';
                    var estado = 'APROBADO';
                }
                $("<div>")
                    .append($('<span class="' + estiloClass + '">' + estado + '</span>'))
                    .appendTo(container);
            }
        }
        , {
            caption: "Aprobado por",
            dataField: "AprobadoPor",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Fecha aprobación",
            dataField: "FechaAprobacion",
            area: "column",
            dataType: "datetime"
        }
    ],
    onSelectionChanged: function (selectedItems) {
        var data = selectedItems.selectedRowsData[0];
        if (data) {
            MostrarReporte(data);
        }
    }, export: {
        enabled: true,
        allowExportSelectedData: true
    },
    onExporting: function (e) {
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Reporte');

        DevExpress.excelExporter.exportDataGrid({
            component: e.component,
            worksheet: worksheet,
            autoFilterEnabled: true
        }).then(function () {
            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ListaReportesAnalisisQuimicoSe.xlsx');
            });
        });
        e.cancel = true;
    }

}
$(document).ready(function () {
    $('#fechaDesde').val(moment().format("YYYY-MM-DD"));
    $('#fechaHasta').val(moment().format("YYYY-MM-DD"));
    CargarCabReportes();
});
function MostrarReporte(data) {
    //console.log(data);
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabecera: data.IdAnalisisQuimicoProductoSe
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/PartialReporteControl?' + query;
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
               
                $('#DivBotonImpr').prop('hidden', false);
                $('#DivReporte').prop('hidden', false);
                $('#DivCabReportes').prop('hidden', true);
                $('#DivReporte').empty();
                $('#DivReporte').html(resultado);
                $('#lblUsuarioIngreso').text(data.UsuarioIngresoLog);
                $('#lblFechaIngreso').text(moment(data.FechaIngresoLog).format("DD-MM-YYYY"));
                $('#lblAprobadoPor').text(data.AprobadoPor);
                $('#lblFechaAprobacion').text(data.FechaAprobacion);
                $('#lblFechap').text(data.Fecha);
                $('#lblObservacionp').text(data.Observacion);
                $('#lblturno').text(data.turno);
                $('#mensajeRegistros').prop('hidden', true);
                //config.opcionesDT.pageLength = 30;
                //$('#tblReporte').DataTable(config.opcionesDT);
                //LimpiarDetalleControles();
            } else {
                $('#DivReporte').empty();
                $('#mensajeRegistros').prop('hidden', false);
            }
            $('#cargac').hide();
            //console.log(resultado);
        })

        .catch(function (resultado) {
            console.log(resultado);
            MensajeError(resultado, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
        })

}
function imprimirw() {
    window.print();
}

function Atras() {
    $('#DivBotonImpr').prop('hidden', true);
    $('#DivReporte').prop('hidden', true);
    $('#DivCabReportes').prop('hidden', false);
}
function CargarCabReportes() {
    Atras();
    $('#cargac').show();
    //var table = $("#tblDataTableReporte");
    //    table.DataTable().destroy();
    //    table.DataTable().clear();
    let params = {
        FechaDesde: $('#fechaDesde').val(),
        FechaHasta: $('#fechaHasta').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/ConsultarCabecerasReporte?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == '101') {
                window.location.reload();
            }
            if (resultado != '0') {
                opciosGrid.dataSource = resultado;
                var dataGrid = $("#gridContainer").dxDataGrid(opciosGrid).dxDataGrid("instance");
                dataGrid.beginCustomLoading();

                dataGrid.endCustomLoading();
                $('#mensajeRegistros').prop('hidden', true);
                $("#tblDataTableReporte tbody").empty();
                $('#DivCabReportes').prop('hidden', false);
                $('#cargac').hide();
            } else {

                $('#DivReporte').empty();
                $('#DivCabReportes').prop('hidden', true);
                $('#mensajeRegistros').text(Mensajes.SinRegistrosRangoFecha);
                $('#mensajeRegistros').prop('hidden', false);
                $('#cargac').hide();
            }
  
        })
        .catch(function (resultado) {
            $('#cargac').hide();
            //console.log(resultado);
            MensajeError(resultado, false);
        
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