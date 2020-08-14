DevExpress.localization.locale(navigator.language);
var itemCabecera = [];
var itemDetalle = [];
var itemSubDetalle = [];
var itemParadas = [];
var itemImagen = [];
var actulizarFoto = false;
$(document).ready(function () {
    CargarCabecera();
    $('#selectTomaMuestra').select2({
        width: '100%'
    });
    $('#selectPCC').select2({
        width: '100%'
    });   
    $('#txtNumCoche').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': 1000, 'min': 0 });
    $('#txtTemperatura').inputmask({ 'alias': 'decimal', 'groupSeparator': '', 'autoGroup': true, 'digits': 2, 'digitsOptional': true, 'max': 100, 'min': -50 });
});

var gridDetalle = {
    loadPanel: {
        enabled: true
    },
    groupPanel: { visible: true },
    grouping: {
        autoExpandAll: false
    },
    keyExpr: "Lote",
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
    searchPanel: { visible: true },
    
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
        }, {
            caption: "BARCO ASIGNADO",
            dataField: "BarcoAsignado",
            area: "column",
            dataType: "string"
        },{
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
                        if (row.TomaMuestra==1) {
                            aba.push(row.NumCoche+' ['+row.Temperatura+']');
                        }                        
                    });
                    //console.log(aba);
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
     ,onSelectionChanged: function (selectedItems) {
        var rowData = selectedItems.selectedRowsData[0];
         if (rowData) {
             ModalIngresoSubDetalle(rowData);
        }
    }
    , export: {
        enabled: true,
        allowExportSelectedData: true
    },
    onExporting: function (e) {
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Control');

        DevExpress.excelExporter.exportDataGrid({
            component: e.component,
            worksheet: worksheet,
            autoFilterEnabled: true
        }).then(function () {
            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ControlCocinamiento.xlsx');
            });
        });
        e.cancel = true;
    }
}

var gridSubDetalle = {
    loadPanel: {
        enabled: true
    },
    //groupPanel: { visible: true },
    grouping: {
        autoExpandAll: false
    },
    keyExpr: "IdCocinamientoSubDet",
    selection: {
        mode: "single"
    },
    columnFixing: {
        enabled: true
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
    //searchPanel: { visible: true },
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
        ,{
            caption: "TEMPERATURA",
            dataField: "Temperatura",
            area: "column",
            dataType: "int"
        }, {
            caption: "ACCIÓN", cellTemplate: function (container, options) {
                var btnEditar = "<button id='btnActualizar' class='btn btn-link' style='padding:0px' onclick='EditarSubDetalle(" + JSON.stringify(options.data) + ")'> Editar </button> ";
                var btnEliminar = " <button id='btnEliminar' class='btn btn-link' style='padding:0px' onclick='EliminarSubDetalle(" + JSON.stringify(options.data) + ")'> Eliminar</button>";

                $("<div>")
                    .append($(btnEditar))
                    .append($(btnEliminar))
                    .appendTo(container);
            }
        },
    ]
}

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

var options = {
    series: [
        {
            name: "High - 2013",
            type: 'line',
            data: [1.4, 2, 2.5, 1.5, 2.5, 2.8, 3.8, 4.6]
            //data: [28, 29, 33, 36, 32, 32, 33]
        },
        {
            name: "Low - 2013",
            type: 'column',
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 450,
        //width:1200,
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
                    if (row.Lote==lote) {
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
    colors: ['#66C7F4','#C5EDAC'],
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

async function ConsultarEstadoRegistro() {
    const data = new FormData();
    data.append('idCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
    var promesa = fetch("../ControlCocinamiento/ConsultarEstadoReporte", {
        method: 'POST',
        body: data
    });
    return promesa;
}

function CargarCabecera() {
    $('#cargac').show();
    if ($('#txtFechaProduccion').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }
    $.ajax({
        url: "../ControlCocinamiento/ConsultarCabecera",
        data: {
            fechaProduccion: $("#txtFechaProduccion").val()
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divMostrarCabecera').prop('hidden', true);
                $("#divMostarTablaCabecera").html("No existen registros");
                $('#divBotonCrear').prop('hidden', false);
                $('#divBotonCrearDetalle').prop('hidden', true);
                $('#divMostarTablaDetallesVer').prop('hidden', true);
                $('#txtFechaAsignada').prop('disabled', false);
                $('#divApexChartDetalle').prop('hidden', true);
                document.getElementById('txtFechaAsignada').value = document.getElementById('txtFechaProduccion').value;
                itemCabecera = [];
                LimpiarModalIngresoCabecera();
                CargarParadas();
            } else {
                itemCabecera = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                $("#txtFechaAsignada").val(moment(resultado.FechaAsignada).format('YYYY-MM-DD'));
                $('#txtFechaAsignada').prop('disabled', true);
                $("#txtObservacionVer").val(resultado.ObservacionC);
                var x = document.getElementById("selectPCC");
                for (i = 0; i < x.options.length; i++) {
                    if (x.options[i].value == resultado.PCC) {
                        document.getElementById('txtPCC').innerText = x.options[i].text;
                        break;
                    }                
                }                
                CargarParadas();
            }
            $('#cargac').hide();
        },
        error: function (result) {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

async function GuardarCabecera(siAprobar) {
    try {
        $('#cargac').show();
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            $('#cargac').hide();
            return;
        } else {
            const data = new FormData();
            data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
            data.append('FechaProduccion', $("#txtFechaProduccionIng").val());
            data.append('FechaAsignada', $("#txtFechaAsignada").val());
            data.append('ObservacionC', $("#txtIngresoObservacion").val());
            data.append('PCC', document.getElementById('selectPCC').value);
            data.append('siAprobar', siAprobar);

            var promiseCall = fetch('../ControlCocinamiento/GuardarModificarCocinamiento', {
                method: 'post',
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
            document.getElementById("txtFechaProduccion").value = moment($("#txtFechaProduccionIng").val()).format('YYYY-MM-DD');
            if (jsonResult == 0) {
                MensajeCorrecto('Registro guardado correctamente');
                //$('#txtFecha').prop('disabled', true);
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (jsonResult == 3) {
                MensajeAdvertencia('Error al ingresar la FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY') + '</span>');
                $('#cargac').hide();
                return;
            } else if (jsonResult == 4) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                return;
            } else if (jsonResult == 5) {               
                MensajeAdvertencia('Error, ya existe una FECHA  : <span class="badge badge-danger">' + moment($("#txtIngresoFecha").val()).format('DD-MM-YYYY')+ '</span>');
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            $('#ModalIngresoCabecera').modal('hide');
            $('#divBotonesCRUD').prop('hidden', false);
            $('#divMostarTablaDetalle').prop('hidden', false);
            $('#divBotonCrear').prop('hidden', true);
            itemCabecera = [];
            $('#cargac').hide();
            CargarCabecera();
        }
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function EliminarConfirmar() {
    try {
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        CambiarMensajeEstado(estadoReporte.EstadoReporte);
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarControl").modal("show");
            $("#myModalLabel").text("¿Desea Eliminar el registro?");
        }
    } catch (e) {
        MensajeError(Mensajes.Error, false);
    }
}

async function EliminarCabeceraSi() {
    try {
        $('#cargac').show();
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            const data = new FormData();
            data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
            data.append('FechaProduccion', moment(itemCabecera.FechaProduccion).format('YYYY-MM-DD'));
            var promisess = fetch('../ControlCocinamiento/EliminarCocinamiento', {
                method: 'post',
                body: data
            });
            var objectPromise = await promisess;
            if (!objectPromise.ok) {
                throw 'Error';
            }
            var jsonResult = await objectPromise.json();
            if (jsonResult == "0") {
                MensajeAdvertencia("Falta Parametro IdAnalisis");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (jsonResult == 1) {
                MensajeCorrecto("Registro eliminado con Éxito");
                $('#txtFecha').prop('disabled', false);
            } else if (jsonResult == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            }
            itemCabecera = [];
            CargarCabecera();
            $('#cargac').hide();
            $("#modalEliminarControl").modal("hide");
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }    
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

async function ActualizarCabecera() {
    try {
        var objectPromise = await ConsultarEstadoRegistro();
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.json();
        CambiarMensajeEstado(jsonResult.EstadoReporte);
        if (jsonResult.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);            
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtFechaProduccionIng").val(moment(itemCabecera.FechaProduccion).format("YYYY-MM-DD"));
            $("#txtIngresoObservacion").val(itemCabecera.ObservacionC);
            $('#selectPCC').val(itemCabecera.PCC).trigger('change');
            $('#ModalIngresoCabecera').modal('show');
        }
    } catch (e) {
        MensajeError(Mensajes.Error, false);
    }
}

function ModalIngresoCabecera() {
    LimpiarModalIngresoCabecera();
    $('#ModalIngresoCabecera').modal('show');
    //$('#selectTurnoInsertar').val(document.getElementById('selectTurno').value).trigger('change');
    itemCabecera = [];
}

function LimpiarModalIngresoCabecera() {
    $('#txtFechaProduccionIng').val(moment($('#txtFechaProduccion').val()).format('YYYY-MM-DD'));
    $('#txtIngresoObservacion').val('');
}

function ValidarDatosVacios(siAprobar) {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera(siAprobar);
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtIngresoFecha').val() == '') {
        $("#txtIngresoFecha").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtIngresoFecha").css('border', ''); }
    return con;
}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#lblAprobadoPendiente").text("APROBADO");
        $("#lblAprobadoPendiente").removeClass('badge-danger');
        $("#lblAprobadoPendiente").addClass('badge badge-success');
    } else if (estadoReporteParametro == false) {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada') {
        $("#lblAprobadoPendiente").text("");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").removeClass('badge badge-danger');
    }
}
//DETALLE
async function CargarParadas() {
    try {
        $('#cargac').show();
        let params = {
            fechaProduccion: $('#txtFechaProduccion').val(),
            fechaAsignada: document.getElementById('txtFechaAsignada').value,
            op:1
        }
        let query = Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
        let url = '../ControlCocinamiento/JsonControlCocinamiento?' + query;
        var promiseCall = fetch(url);
        var objectPromise = await promiseCall;
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.json();
        if (jsonResult == "101") {
            window.location.reload();
        }
        if (jsonResult == "0") {
            $("#divMostarTablaDetallesVer").html("No existen registros");
            $('#divBotonCrearDetalle').prop('hidden', false);
            $('#selectVerificacion').prop('disabled', false);
        } else {
            $('#divMostarTablaDetallesVer').prop('hidden', false);
            itemParadas = jsonResult;
            var tempProm = [];
            var Lote = [];
            var temperaturaCorte = [];
            jsonResult.forEach(function (row) {
                row.Cocina = row.Cocina + ' / ' + row.Parada ;
                row.Especie = row.Especie + ' / ' + row.Talla;
                Lote.push(row.Lote);
                var acu = 0;
                var con = 0;
                row.listaTemperatura.forEach(function (rowTemp) {
                    acu = acu + rowTemp.Temperatura;
                    con++;
                });
                if (con == 0) { con = 1;}
                tempProm.push(parseFloat(acu / con).toFixed(2));
                temperaturaCorte.push(70);
            });
            //DevExpress.localization.locale(navigator.language);
            gridDetalle.dataSource = jsonResult;
            var dataGrid=$("#divMostarTablaDetallesVer").dxDataGrid(gridDetalle).dxDataGrid("instance");
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
        }
        $('#cargac').hide();
    } catch (e) {
        console.log(e);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function CargarDetalle() {
    try {
        //$('#cargac').show();
        let params = {
            idCocinamientoCtrl: itemCabecera.IdCocinamientoCtrl,
            lote: document.getElementById('lblLote').value,
            ordenFabricacion: document.getElementById('lblOrdenFabricacion').value
        }
        let query = Object.keys(params)
            .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
            .join('&');
        let url = '../ControlCocinamiento/JsonCargarDetalle?' + query;
        var promiseCall = fetch(url);
        var objectPromise = await promiseCall;
        if (!objectPromise.ok) {
            throw 'Error';
        }
        var jsonResult = await objectPromise.json();
        if (jsonResult == "101") {
            window.location.reload();
        }
        if (jsonResult == "0") {
            MensajeAdvertencia('Detalle no encontrado');
            itemDetalle = null;
        } else {
            itemDetalle = jsonResult;
        }
        //$('#cargac').hide();
    } catch (e) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function ModalIngresoSubDetalle(jRowData) {
    LimpiarDatosImagen();
    LimpiarSubDetalle();
    try {
        if (itemCabecera.length != 0) {
            var fProduccion = document.getElementById('txtFechaProduccion').value;
            var fAsignada = document.getElementById('txtFechaAsignada').value;
            if (fProduccion != fAsignada) {
                $('#confirmarIngreso').html('Esta a punto de ingresar en un detalle con fecha diferente:' + '</br>Fecha Producción:<span class="badge badge-success"> ' + moment(fProduccion).format('DD-MM-YYYY') + '</span></br> Fecha  Asignada: <span class="badge badge-danger">' + moment(fAsignada).format('DD-MM-YYYY') +'</span>');
                $('#modalConfirmarEdicion').modal('show')
                $('#ModalIngresoSubDetalle').modal('hide');
            }
           
                var estadoReporteAwait = await ConsultarEstadoRegistro();
                if (!estadoReporteAwait.ok) {
                    throw 'Error';
                }
                var estadoReporte = await estadoReporteAwait.json();
                CambiarMensajeEstado(estadoReporte.EstadoReporte);
            if (estadoReporte.EstadoReporte == true) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else {
                $('#ModalIngresoSubDetalle').modal('show');
                itemDetalle = jRowData;
                document.getElementById('lblCocina').innerText = 'COCINA: ' + itemDetalle.Cocina;
                document.getElementById('lblLote').innerText = 'LOTE: ' + itemDetalle.Lote;
                document.getElementById('lblCocina').value = itemDetalle.Cocina;
                document.getElementById('lblLote').value = itemDetalle.Lote;
                document.getElementById('lblOrdenFabricacion').innerText = 'O.F: ' + itemDetalle.OrdenFabricacion;
                document.getElementById('lblOrdenFabricacion').value = itemDetalle.OrdenFabricacion;
                $('#tblImagenes').html('');
                ConsultarSubDetalle();
                CargarImagen();
                $('#cargac').hide();
            }           
        } else
            MensajeAdvertencia('No existe la cabecera del CONTROL');
    } catch (ex) {
        console.log(ex);
        MensajeError(Mensajes.Error, false);
    }
}

function LimpiarSubDetalle() {
    CargarParadas();
    document.getElementById('txtTemperatura').value = '';
    document.getElementById('txtNumCoche').value = '';
    $('#divElementos').html('');
}

async function GuardarSubDetalle() {
    try {
        if (OnChangeTextBoxSubDetalle() == 1) {
            MensajeAdvertencia('Por favor ingrese todos los datos requeridos');
            return;
        }
        $('#cargac').show();
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            var data = new FormData();
            data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
            data.append('IdCocinamientoDet', itemDetalle.IdCocinamientoDet);
            data.append('Lote', document.getElementById('lblLote').value);
            data.append('OrdenFabricacion', document.getElementById('lblOrdenFabricacion').value);            
            data.append('IdCocinamientoSubDet', itemSubDetalle.IdCocinamientoSubDet);
            data.append('NumCoche', document.getElementById('txtNumCoche').value);
            data.append('Temperatura', document.getElementById('txtTemperatura').value);
            data.append('TomaMuestra', document.getElementById('selectTomaMuestra').value);
            var promiseCall = fetch('../ControlCocinamiento/GuardarModificarSubDetalle', {
                method: 'post',
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
            if (jsonResult == 0) {               
                if (itemDetalle.IdCocinamientoDet == 0) {
                    var waitDetail = await CargarDetalle();
                   
                    //var jsonResult = await waitDetail.json();
                }
                MensajeCorrecto('Registro guardado correctamente');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
                NewSubDetail();
            } else if (jsonResult == 2) {
                MensajeAdvertencia('¡Error! No se a guardado  : <span class="badge badge-danger">' + 'SIN DATOS' + '</span>');
                $('#cargac').hide();
                $('#ModalIngresoSubDetalle').modal('hide');
                return;
            } else if (jsonResult == 3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $('#ModalIngresoSubDetalle').modal('hide');
                $('#cargac').hide();
                return;
            }
            document.getElementById('txtTemperatura').value = '';
            ConsultarSubDetalle();
            $('#cargac').hide();
        }
    } catch (ex) {
        console.log(ex);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function OnChangeTextBoxSubDetalle() {
    var con = 0;
    if ($('#txtNumCoche').val() == '') {
        $("#txtNumCoche").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtNumCoche").css('border', ''); }
    if ($('#txtTemperatura').val() == '') {
        $("#txtTemperatura").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtTemperatura").css('border', ''); }
    return con;
}

async function ConsultarSubDetalle() {//USADO CUANDO SE DA CLICK EN LA FILA DE LA TABLA
    try {
        const data = new FormData();
        data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
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
        gridSubDetalle.dataSource = jsonResult;
        $("#divSubDetalle").dxDataGrid(gridSubDetalle).dxDataGrid("instance");
        var dataGrid = $("#divSubDetalle").dxDataGrid("instance");
        dataGrid.deselectAll();
        dataGrid.clearSelection();
        $('#cargac').hide();
    } catch (ex) {
        console.log(ex);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function ConsultarSubDetalleAC() {//USADO CUANDO SE DA CLICK EN EL APEXCHART
    try {
        const data = new FormData();
        data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
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
        //console.log(ex);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function EditarSubDetalle(jdata) {
    $('div,html').animate({ scrollTop: 0 }, 500);
    itemSubDetalle = [];
    $('#selectTomaMuestra').val(jdata.TomaMuestra).trigger('change');
    document.getElementById('txtNumCoche').value = jdata.NumCoche;
    document.getElementById('txtTemperatura').value = jdata.Temperatura;
    QuitarAgregarStyleSD(1);
    itemSubDetalle = jdata;
}

function QuitarAgregarStyleSD(op) {    
    if (op == 1) {
        $("#txtNumCoche").css('border', '1px dashed green');
        $("#txtTemperatura").css('border', '1px dashed green');
        $("#txtNumCoche").css('background-color', 'lightgrey');
        $("#txtTemperatura").css('background-color', 'lightgrey');
    } else {        
        $("#txtNumCoche").css('border', '');
        $("#txtTemperatura").css('border', '');
        $("#txtNumCoche").css('background-color', '');
        $("#txtTemperatura").css('background-color', '');
    }
}

function NewSubDetail() {
    itemSubDetalle = [];
    QuitarAgregarStyleSD(0);
    document.getElementById('txtTemperatura').value = '';
    document.getElementById('txtNumCoche').value = '';   
}

async function EliminarSubDetalle(jdata) {
    try {
        NewSubDetail();
        var estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        CambiarMensajeEstado(estadoReporte.EstadoReporte);
        if (estadoReporte == "101") {
            window.location.reload();
        }
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            $("#modalEliminarSubDetalle").modal("show");
            $("#myModalLabelDetalle").text("¿Desea Eliminar la toma de temperatura?");
            itemSubDetalle = jdata;
        }
    } catch (e) {
        MensajeError(Mensajes.Error, false);
    }
}

function EliminarSubDetalleSi() {
    $.ajax({
        url: "../ControlCocinamiento/EliminarSubDetalle",
        type: "POST",
        data: {
            idCocinamientoCtrl: itemCabecera.IdCocinamientoCtrl,
            IdCocinamientoSubDet: itemSubDetalle.IdCocinamientoSubDet
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro itemSubDetalle");
                $("#modalEliminarSubDetalle").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarSubDetalle").modal("hide");
                ConsultarSubDetalle();
                MensajeCorrecto("Registro eliminado con Éxito");
                $('#cargac').hide();
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            } else if (resultado == '3') {
                MensajeAdvertencia('¡No se encontro ningun registro Cabecera en esta fecha!');
                $('#cargac').hide();
                return;
            } else if (resultado == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $('#modalEliminarSubDetalle').modal('hide');
                return;
            }
            itemSubDetalle = [];
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function EliminarSubDetalleNo() {
    itemSubDetalle = [];
    $("#modalEliminarSubDetalle").modal("hide");
}
//IMAGEN
async function GuardarImagen() {
    try {       
        $('#cargac').show();
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        if (estadoReporte.EstadoReporte == true) {
            $('#cargac').hide();
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            if (OnChangeTextBoxAccion() == 1) {
                MensajeAdvertencia('Ingrese todos los datos requeridos');
                $('#cargac').hide();
                return;
            }
            $('#cargac').show();
            //var idAnalisisDetalle = 0;
            //if (itemDetalle != null) {
            //    if (!actulizarFoto) {
            //        itemDetalle.forEach(function (row) {
            //            if (row.Cocinador == document.getElementById('lblCocinador').value && row.Parada == document.getElementById('lblParada').value) {
            //                idAnalisisDetalle = row.IdAnalisisDetalle;
            //            }
            //        });
            //    } else {
            //        idAnalisisDetalle = itemImagen.IdAnalisisDetalle;
            //    }
            //}
            var imagen = $('#file-upload')[0].files[0];
            var data = new FormData();
            data.append("dataImg", imagen);
            data.append('IdCocinamientoCtrl', itemCabecera.IdCocinamientoCtrl);
            data.append("IdCocinamientoDet", itemDetalle.IdCocinamientoDet);
            data.append('Lote', document.getElementById('lblLote').value);
            data.append('OrdenFabricacion', document.getElementById('lblOrdenFabricacion').value);
            data.append("IdImagen", itemImagen.IdImagen);
            data.append("ObservacionImagen", document.getElementById('txtObservacionImagen').value);
            data.append("Rotation", rotation);
            var promiseCall = fetch('../ControlCocinamiento/GuardarImagen', {
                method: 'post',
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
            if (jsonResult == 0) {
                if (itemDetalle.IdCocinamientoDet == 0) {
                    var waitDetail = await CargarDetalle();

                    //var jsonResult = await waitDetail.json();
                }
                MensajeCorrecto('Imagen guardada correctamente');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (jsonResult == 3) {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 4) {
                MensajeAdvertencia('¡Solo se permiten imagenes!', 5);
                $('#cargac').hide();
                return;
            } else if (jsonResult == 100) {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $('#ModalIngresoSubDetalle').modal('hide');
                $('#cargac').hide();
                return;
            } else {
                var mb = parseFloat(resultado / (1024 * 1024)).toFixed(2);
                MensajeAdvertencia('¡Exedio el limite de capacidad permitido!:  <span class="badge badge-success">5Mb</span>: Su imagen:<span class="badge badge-danger">' + mb + 'Mb</span>');
                $('#cargac').hide();
                return;
            }
            //var callCargarDetalle = await CargarDetalle();
            CargarImagen();
            LimpiarDatosImagen();
            $('#cargac').hide();
            itemImagen = [];
        }
    } catch (ex) {
        //console.log(ex);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function OnChangeTextBoxAccion() {
    var con = 0;
    if ($('#txtObservacionImagen').val() == '') {
        $("#txtObservacionImagen").css('border', '1px dashed red');
        con = 1;
    } else { $("#txtObservacionImagen").css('border', ''); }
    if (!actulizarFoto) {
        if ($('#file-upload').val() == '') {
            $("#file-upload").css('border', '1px dashed red');
            con = 1;
        } else { $("#file-upload").css('border', ''); }
        if ($('#lblImagen').val() == '') {
            $("#lblImagen").css('border', '1px dashed red');

        } else { $("#lblImagen").css('border', ''); }
    }
    return con;
}

function LimpiarDatosImagen() {
    $("#txtObservacionImagen").val("");
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $('#lblImagen').text('Seleccione archivo');
    $("#lblImagen").css('border', '');
    $("#file-upload").css('border', '');
    $("#txtObservacionImagen").css('border', '');
    rotation = 0;
    actulizarFoto = false;
}

function CargarImagen() {   
    $.ajax({
        url: "../ControlCocinamiento/VerCrearImagenPartial",
        data: {
            idCocinamientoDet: itemDetalle.IdCocinamientoDet
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == 0) {
                $('#divListarImagen').html('<span class="badge">SIN DATOS</span>');
            } else
                $('#divListarImagen').html(resultado);
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error, false);
        }
    });
}

function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#file-preview-zone").html('');
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
            filePreview.src = e.target.result;
            var previewZone = document.getElementById('file-preview-zone');
            previewZone.appendChild(filePreview);
            $("#file-preview").addClass("img");
            var image = new Image();
            image.src = e.target.result;
            image.onload = function () {
                document.getElementById("file-preview").style.height = "250px";
                document.getElementById("file-preview").style.width = "250px";
            };
        }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');

fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);
}

$('#file-preview-zone').on("click", function (e) {
    rotation += 90;
    $('#file-preview').rotate(rotation);
    if (rotation == 360) {
        rotation = 0;
    }
});

function validarImg(rotacion, id, imagen) {
    $('#' + id).rotate(parseInt(rotacion));
    var img = new Image();
    document.getElementById(id).style.borderRadius = "20px";
    document.getElementById(id).style.height = "250px";
    document.getElementById(id).style.width = "250px";
    img.src = $('#btnPath').val() + imagen;
}

function EditarImagen(jdata) {
    $('div,html').animate({ scrollTop: 0 }, 500);
    LimpiarDatosImagen();
    actulizarFoto = true;
    document.getElementById('txtObservacionImagen').value = jdata.ObservacionImagen;
    if (jdata.RutaImagen != null && jdata.RutaImagen != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = $('#btnPath').val() + jdata.RutaImagen;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);

        $("#file-preview").addClass("img");
        $('#file-preview').rotate(parseInt(jdata.Rotation));

        document.getElementById("file-preview").style.height = "250px";
        document.getElementById("file-preview").style.width = "250px";
        itemImagen = jdata;
    }
}

function EliminarImagenSi() {
    $.ajax({
        url: "../ControlCocinamiento/EliminarImagen",
        type: "POST",
        data: {
            IdImagen: itemImagen.IdImagen,
            idCocinamientoCtrl: itemCabecera.IdCocinamientoCtrl
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdFoto");
                $("#modalEliminarImagen").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                CargarImagen();
                $("#modalEliminarImagen").modal("hide");
                MensajeCorrecto("Registro eliminado con Éxito");
            } else if (resultado == '2') {
                MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
                $('#cargac').hide();
                return;
            }
            $('#cargac').hide();
        },
        error: function () {
            $('#cargac').hide();
            MensajeError(Mensajes.Error, false);
        }
    });
}

function EliminarImagenNo() {
    $("#modalEliminarImagen").modal("hide");
}

async function EliminarImagenConfirmar(jdata) {
    try {
        const estadoReporteAwait = await ConsultarEstadoRegistro();
        if (!estadoReporteAwait.ok) {
            throw 'Error';
        }
        var estadoReporte = await estadoReporteAwait.json();
        CambiarMensajeEstado(estadoReporte.EstadoReporte);
        if (estadoReporte.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!');
            return;
        } else {
            $("#modalEliminarImagen").modal("show");
            $("#mensajeEliminarImagen").text("¿Desea Eliminar la Imagen?");
            itemImagen = jdata;
        }
    } catch (ex) {
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

async function ConfirmarIngreso() {    
    $('#modalConfirmarEdicion').modal('hide')
    $('#ModalIngresoSubDetalle').modal('show');
}