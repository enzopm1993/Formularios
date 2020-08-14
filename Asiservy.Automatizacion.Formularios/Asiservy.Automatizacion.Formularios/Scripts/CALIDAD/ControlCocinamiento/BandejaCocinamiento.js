DevExpress.localization.locale(navigator.language);
var listaDatos = [];
var itemDetalle = [];
var itemParadas = [];
$(document).ready(function () {
    CargarBandeja();
    $('#selectEstadoReporte').select2({
        width: '100%'
    });
});

//CARGAR BANDEJA
function CargarBandeja() {
    var dateAux = '';
    dateAux = $('#fechaHasta').val() + ' 23:59';
    $('#cargac').show();
    var estadoReporte = $('#selectEstadoReporte').val();
    $('#divCalendar').prop('hidden', true);
    if (estadoReporte == 'true') {
        $('#divCalendar').prop('hidden', false);
    }
    $.ajax({
        url: "../ControlCocinamiento/BandejaCocinamientoPartial",
        data: {
            fechaDesde: $('#fechaDesde').val(),
            fechaHasta: dateAux,
            estadoReporte: estadoReporte
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "102") {
                MensajeAdvertencia("No existen datos para este model.");
            }
            if (resultado == "0") {
                $("#divTablaAprobados").html("No existen registros: " + resultado);
            } else {
                $("#btnPendiente").prop("hidden", true);
                $("#btnAprobado").prop("hidden", false);
                $("#divTablaAprobados").show();
                $("#divTablaAprobados").html(resultado);
            }
            $('#cargac').hide();
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
    var op = 1;
    $.ajax({
        url: "../ControlCocinamiento/JsonBandejaCocinamientoAprobacion",
        type: "GET",
        data: {
            fechaProduccion: listaDatos.FechaProduccion,
            fechaAsignada: model.FechaAsignada,
            op: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia('¡No existe detalle para este registro!');
                $("#divTblAprobarPendiente").html("No existen registros");
            } else if (resultado == 1) {
                MensajeError(Mensajes.Error, false);
            } else {
                itemParadas = resultado;
                $("#ModalApruebaPendiente").modal("show");
                var tempProm = [];
                var Lote = [];
                var temperaturaCorte = [];
                resultado.forEach(function (row) {
                    //row.Lote = row.Lote + ' - ' + row.OrdenFabricacion;
                    row.Cocina = row.Cocina + ' / ' + row.Parada;
                    row.Especie = row.Especie + ' / ' + row.Talla;
                    Lote.push(row.Lote);
                    var acu = 0;
                    var con = 0;
                    row.listaTemperatura.forEach(function (rowTemp) {
                        acu = acu + rowTemp.Temperatura;
                        con++;
                    });
                    if (con == 0) { con = 1; }
                    tempProm.push(parseFloat(acu / con).toFixed(2));
                    temperaturaCorte.push(70);
                });               
                gridDetalle.dataSource = resultado;
                var dataGrid = $("#divAprobarPendienteJson").dxDataGrid(gridDetalle).dxDataGrid("instance");
                dataGrid.deselectAll();
                dataGrid.clearSelection();
                $('#divApexChartDetalle').prop('hidden', false);
                var _seriePromedio = [
                    {
                        name: "PROMEDIO DE COCCIÓN",
                        data: tempProm
                    },
                    {
                        name: "TEMPERATURA CORTE COCCION",
                        data: temperaturaCorte
                    },
                ];
                chartDetalle.updateSeries(_seriePromedio)
                chartDetalle.updateOptions({
                    xaxis: {
                        categories: Lote,
                        title: {
                            text: 'LOTE'
                        }
                    },
                    yaxis: {
                        title: {
                            text: 'TEMPERATURA PROMEDIO'
                        }
                    }
                });
               
                resultado.forEach(function (row) {
                    row.listaImagenes.forEach(function (rowImages) {
                        var divHeader = document.createElement("div");
                        var divDetalle = document.createElement("div");
                        divHeader.id = 'header_' + rowImages.IdImagen;
                        divHeader.classList = 'col-md-3 col-12 col-sm-12';
                        document.getElementById("divImages").appendChild(divHeader);
                        var img = document.createElement("img");
                        img.id = rowImages.IdImagen;
                        img.src = rowImages.RutaImagen;
                        img.style = 'border:1px solid #ddd';
                        img.classList = 'card-img-bottom img';                        
                        img.onclick = function () { window.open(this.src)};
                        document.getElementById('header_' + rowImages.IdImagen).appendChild(img);
                        divDetalle.id = 'divLbl_' + rowImages.IdImagen;
                        document.getElementById('header_' + rowImages.IdImagen).appendChild(divDetalle);
                        var lblObservacion = document.createElement("label");
                        lblObservacion.style = 'color:black;font-size:10px;white-space:normal;width:250px';
                        lblObservacion.innerText = rowImages.ObservacionImagen;
                        document.getElementById('divLbl_' + rowImages.IdImagen).appendChild(lblObservacion);
                        validarImg(rowImages.Rotation, rowImages.IdImagen);

                    });
                });          
            }
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
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
    var siAprobar = 1;
    $.ajax({
        url: "../ControlCocinamiento/GuardarModificarCocinamiento",
        type: "POST",
        data: {
            IdCocinamientoCtrl: listaDatos.IdCocinamientoCtrl,
            EstadoReporte: estadoReporte,
            FechaAprobado: $('#txtFechaAprobado').val(),
            FechaProduccion: listaDatos.FechaProduccion,
            siAprobar: siAprobar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 2 || resultado == 1) {
                MensajeCorrecto('¡Cambio de ESTADO realizado correctamente!');
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else {
                MensajeError('Error en: model.Fecha!=DateTime.MinValue - GuardarModificarControlCocinamiento');
                return;
            }
            $("#ModalApruebaPendiente").modal("hide");
            CargarBandeja();
            listaDatos = [];
            LimpiarFecha();
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
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
    $('#divImages').html('');
}

function validarImg(rotacion, id) {
    $('#' + id).rotate(parseInt(rotacion));
    document.getElementById(id).style.borderRadius = "20px";
    document.getElementById(id).style.height = "270px";
    document.getElementById(id).style.width = "270px";
}

async function ConsultarSubDetalleAC() {//USADO CUANDO SE DA CLICK EN EL APEXCHART
    try {
        const data = new FormData();
        data.append('IdCocinamientoCtrl', listaDatos.IdCocinamientoCtrl);
        data.append('IdCocinamientoDet', itemDetalle.IdCocinamientoDet);
        var promiseCall = fetch("../ControlCocinamiento/JsonSubDetalle", {
            method: 'POST',
            body: data
        });
        var objectPromise = await promiseCall;
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.json();
        if (jsonResult == "101") {
            window.location.reload();
        }

        gridSubDetalleAC.dataSource = jsonResult;
        $("#divSubDetalleAC").dxDataGrid(gridSubDetalleAC).dxDataGrid("instance");
        $('#cargac').hide();
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

//VARIABLES
var gridDetalle = {
    loadPanel: {
        enabled: true
    },
    keyExpr: "Lote",
    selection: {
        mode: "none"
    },
    hoverStateEnabled: true,
    showColumnLines: true,
    rowAlternationEnabled: true,
    allowColumnResizing: true,
    columnResizingMode: "nextColumn",
    columnMinWidth: 50,
    columnAutoWidth: true,
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
        pageSize: 0
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [0],
        showInfo: true,
        showNavigationButtons: true,
        infoText: "Página #{0}. Total: {1} ({2} filas)"
    },
    columns: [
        {
            caption: "LOTE",
            dataField: "Lote",
            area: "column",
            dataType: "string",
            width: 60,
            fixed: true,
        }
        , {
            caption: "BARCO ORIGEN",
            dataField: "Barco",
            area: "column",
            dataType: "string"
        },{
            caption: "BARCO ASIGNADO",
            dataField: "BarcoAsignado",
            area: "column",
            dataType: "string"
        }, {
            caption: "COCINA / PARADA",
            dataField: "Cocina",
            area: "column",
            dataType: "int"
        }, {
            caption: "HORA COCCIÓN",
            dataField: "COCINAPARADA",
            area: "column",
            dataType: "int"
        }, {
            caption: "TIEMPO. COCCIÓN",
            dataField: "COCINAPARADA",
            area: "column",
            dataType: "int"
        }, {
            caption: "ESPECIE / TALLA",
            dataField: "Especie",
            area: "column",
            dataType: "int"
        }, {
            caption: "TEMPERATURA CORTE",
            dataField: "TiempoCorte",
            area: "column",
            dataType: "int"
        }, {
            caption: "TEMPERATURA FINALIZACION COCCIÓN",
            cssClass: 'text-center',
            columns: [{
                caption: "COCHE [ARRIBA]",
                area: "row",
                calculateCellValue: function (data) {
                    var aba = [];
                    data.listaTemperatura.forEach(function (row) {
                        if (row.TomaMuestra == 1) {
                            aba.push(row.NumCoche + ' [' + row.Temperatura + ']');
                        }
                    });
                    return aba.join('-');
                }
            }, {
                caption: 'COCHE [MEDIO]',
                calculateCellValue: function (data) {
                    var aba = [];
                    data.listaTemperatura.forEach(function (row) {
                        if (row.TomaMuestra == 2) {
                            aba.push(row.NumCoche + ' [' + row.Temperatura + ']');
                        }
                    });
                    return aba.join('-');
                }
            }, {
                caption: 'COCHE [ABAJO]',
                calculateCellValue: function (data) {
                    var aba = [];
                    data.listaTemperatura.forEach(function (row) {
                        if (row.TomaMuestra == 3) {
                            aba.push(row.NumCoche + ' [' + row.Temperatura + ']');
                        }
                    });
                    return aba.join('-');
                }
            }]
        }
    ]
    
}

var options = {
    series: [
        {
            name: "High - 2013",
            type: 'line',
            data: [1.4, 2, 2.5, 1.5, 2.5, 2.8, 3.8, 4.6]
        },
        {
            name: "Low - 2013",
            type: 'column',
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 450,        
        type: 'line',
        stacked: false,
        dropShadow: {
            enabled: true,
            color: '#000',
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2
        },
        toolbar: {
            show: false
        },
        events: {
            markerClick: async function (event, chartContext, { seriesIndex, dataPointIndex, config }) {
                var lote = chartContext.grid.xaxisLabels[dataPointIndex];
                itemParadas.forEach(function (row) {
                    if (row.Lote == lote) {
                        itemDetalle = row;
                    }
                });
                var awaitSubDetalle = await ConsultarSubDetalleAC();
                document.getElementById('hLote').innerText = 'LOTE: ' + lote;
                $('#modalMostrarAC').modal('show');
                itemDetalle = [];
            }
        }
    },
    colors: ['#66C7F4', '#C5EDAC'],
    dataLabels: {
        enabled: false,
        formatter: function (val) {
            return val + "%";
        },
    },
    stroke: {
        curve: 'smooth'
    },
    plotOptions: {
        bar: {
            columnWidth: "20%"
        }
    },
    grid: {
        borderColor: '#e7e7e7',
        row: {
            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.5
        },
    },
    markers: {
        size: 1
    },
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
            text: 'Month'
        }
    },
    yaxis: {
        title: {
            text: 'Temperature'
        }
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: 6,
        offsetX: -5
    }
};

var chartDetalle = new ApexCharts(document.querySelector("#divApexChartDetalle"), options);

chartDetalle.render();

var gridSubDetalleAC = {
    loadPanel: {
        enabled: true
    },
    grouping: {
        autoExpandAll: false
    },
    keyExpr: "IdCocinamientoSubDet",
    selection: {
        mode: "single"
    },
    hoverStateEnabled: true,
    showColumnLines: true,
    rowAlternationEnabled: true,
    allowColumnResizing: true,
    columnResizingMode: "nextColumn",
    columnMinWidth: 50,
    columnAutoWidth: true,
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
        pageSize: 0
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [0],
        showInfo: true,
        showNavigationButtons: true,
        infoText: "Página #{0}. Total: {1} ({2} filas)"
    },
    columns: [
        {
            caption: "Nº COCHE",
            dataField: "NumCoche",
            area: "column",
            dataType: "int"
        },
        {
            caption: "MUESTRA",
            area: "column",
            dataType: "int",
            calculateCellValue: function (data) {
                if (data.TomaMuestra == 1) {
                    return 'ARRIBA';
                } else if (data.TomaMuestra == 2) {
                    return 'MEDIO';
                } else if (data.TomaMuestra == 3) {
                    return 'ABAJO';
                }
            }
        }
        , {
            caption: "TEMPERATURA",
            dataField: "Temperatura",
            area: "column",
            dataType: "int"
        }
    ]
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