$(document).ready(function () {
    CargarCabecera();
    $('#selectTurno').select2({
        width: '100%'
    });
    $('#selectTurnoInsertar').select2({
        width: '100%'
    });
    $('#selectIngresarLote').select2({
        width: '100%'
    });
    $('#selectParametros').select2({
        width: '100%'
    });
});

var opciosGrid = {
    loadPanel: {
        enabled: true
    },
    groupPanel: { visible: true },
    grouping: {
        autoExpandAll: false
    },
    keyExpr: "ORDEN",
    selection: {
        mode: "single"
    },
    hoverStateEnabled: true,
    showColumnLines: true,
    //showRowLines: true,
    rowAlternationEnabled: true,
    //showBorders: true,
    allowColumnResizing: true,
    columnResizingMode: "nextColumn",
    columnMinWidth: 50,
    columnAutoWidth: true,
    showBorders: true,
    showRowLines: true,
    //allowColumnReordering: true,
    //allowColumnResizing: true,
    //columnChooser: {
    //    enabled: true
    //},
    //columnFixing: {
    //    enabled: true
    //},
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
            dataField: "LOTE",
            area: "column",
            dataType: "string",
            width: 60,
            fixed: true,
        }
        , {
            caption: "BARCO",
            dataField: "BARCO",
            area: "column",
            dataType: "string"
        }, {
            caption: "COCINA/PARADA",
            dataField: "CocinaParada",
            area: "column",
            dataType: "int"
        }, {
            caption: "H. COCCIÓN",
            dataField: "COCINAPARADA",
            area: "column",
            dataType: "int"
        }, {
            caption: "TPO. COCCIÓN",
            dataField: "COCINAPARADA",
            area: "column",
            dataType: "int"
        }, {
            caption: "ESP/TALLA",
            dataField: "EspTalla",
            area: "column",
            dataType: "int"
        }, {
            caption: "TEMP CORTE",
            dataField: "TiempoCorte",
            area: "column",
            dataType: "int"
        }, {
            caption: "TEMPERATURA FINALIZACION COCCIÓN",
            cssClass: 'text-center',
            columns: [{
                caption: "ARRIBA",
                area: "row",
                calculateCellValue: function (data) {
                    var aba = [];
                    data.Temperatura.forEach(function (row) {
                        aba.push(row.Arriba);
                    });
                    return aba.join('-');
                }
            }, {
                caption: 'MEDIO',
                calculateCellValue: function (data) {
                    var aba = [];
                    data.Temperatura.forEach(function (row) {
                        aba.push(row.Medio);
                    });
                    return aba.join(' ');
                }
            }, {
                caption: 'ABAJO',
                calculateCellValue: function (data) {
                    var aba = [];
                    data.Temperatura.forEach(function (row) {
                        aba.push(row.Abajo);
                    });
                    return aba.join(' ');
                }
            }]
        }
    ]
    , export: {
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
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ControlCocinamiento.xlsx');
            });
        });
        e.cancel = true;
    }
}

var options = {
    series: [
        {
            name: "High - 2013",
            type: 'line',
            data: [28, 29, 33, 36, 32, 32, 33]
        },
        {
            name: "Low - 2013",
            type: 'bar',
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 350,
        type: 'line',
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
        }
    },
    colors: ['#005FFF', '#B548FF',],
    dataLabels: {
        enabled: false,
        formatter: function (val) {
            return val + "%";
        },
    },
    stroke: {
        curve: 'smooth'
    },
    //title: {
    //    text: 'LOTE',
    //    align: 'left'
    //},
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
        offsetY: -25,
        offsetX: -5
    }
};

var chartDetalle = new ApexCharts(document.querySelector("#divApexChartDetalle"), options);
chartDetalle.render();

async function ConsultarEstadoRegistro() {
    const data = new FormData();
    data.append('idAnalisis', itemCabecera.IdAnalisis);
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
                $('#divMostarTablaDetallesVer').html(resultado);
                $('#txtFechaAsignada').prop('disabled', false);
                document.getElementById('txtFechaAsignada').value = document.getElementById('txtFechaProduccion').value;
                itemCabecera = [];
                LimpiarModalIngresoCabecera();
            } else {
                itemCabecera = resultado;
                CambiarMensajeEstado(resultado.EstadoReporte);
                $('#divBotonCrearDetalle').prop('hidden', false);
                $('#divMostrarCabecera').prop('hidden', false);
                $('#divMostarTablaDetalle').html(resultado);
                $('#divBotonCrear').prop('hidden', true);
                //$("#txtFechaCabeceraVer").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtFechaAsignada").val(moment(resultado.FechaAsignada).format('YYYY-MM-DD'));
                $('#txtFechaAsignada').prop('disabled', true);
                $("#txtObservacionVer").val(resultado.ObservacionC);
                //CargarDetalle();
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
                $('#txtFecha').prop('disabled', true);
                //document.getElementById("txtFechaProduccion").value = moment($("#txtFechaProduccionIng").val()).format('YYYY-MM-DD');
                //$('#selectTurno').val(document.getElementById('selectTurnoInsertar').value).trigger('change');
            } else if (jsonResult == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
                //document.getElementById("txtFechaProduccion").value = moment($("#txtIngresoFecha").val()).format('YYYY-MM-DD');
                //$('#selectTurno').val(document.getElementById('selectTurnoInsertar').value).trigger('change');
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
        var jsonResult = objectPromise.json();
        if (jsonResult.EstadoReporte == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
            return;
        } else {
            LimpiarModalIngresoCabecera();
            $("#txtFechaProduccionIng").val(moment(itemCabecera.FechaProduccion).format("YYYY-MM-DD"));
            $("#txtIngresoObservacion").val(itemCabecera.ObservacionC);
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
    //document.getElementById("selectTurnoInsertar").options[0].selected = true;
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
            fechaAsignada: document.getElementById('txtFechaAsignada').value
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
            var con = 1;
            var RealLomo = [];
            var Lote = [];
            jsonResult.forEach(function (row) {
                row.CocinaParada = row.COCINA + ' / ' + row.PARADA;
                row.EspTalla = row.ESPECIE + ' / ' + row.TALLA;
                row.TiempoCorte = '35º';
                var temperaturaA = Array();                
                for (var i = 0; i < 3; i++) {
                    var temperatura = {};
                    temperatura.Arriba = con;
                    con++;
                    temperatura.Medio = con;
                    con++;
                    temperatura.Abajo = con;
                    temperaturaA.push(temperatura);
                    con++;
                }
                RealLomo.push(temperaturaA[0].Arriba);
                row.Temperatura = temperaturaA;  
                Lote.push(row.LOTE);
            });
            //console.log(jsonResult);
            DevExpress.localization.locale(navigator.language);
            opciosGrid.dataSource = jsonResult;
           var wait= await $("#divMostarTablaDetallesVer").dxDataGrid(opciosGrid).dxDataGrid("instance");
            ApexChartDetalle();
            //$('#divMostarTablaDetallesVer').html(jsonResult);
            //ConsultarElemento();
            var _serieLomo = [{
                name: "PROMEDIO",
                data: RealLomo
            }, {
                    name: "TEMPERATURA COCCION",
                    data: RealLomo
                }
           ];
            chartDetalle.updateSeries(_serieLomo)
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
            })
        }
        $('#cargac').hide();

    } catch (e) {
        console.log(e);
        $('#cargac').hide();
        MensajeError(Mensajes.Error, false);
    }
}

function ApexChartDetalle() {
    //var _serieLomo = [{
    //    name: "Real",
    //    data: RealLomo
    //},
    //{
    //    name: "Estandar",
    //    data: EstandarLomo
    //},
    //{
    //    name: "Diferencia",
    //    type: "column",
    //    data: DiferenciaLomo
    //}];
    //chartDetalle.updateSeries(_serieLomo)
    //chartDetalle.updateOptions({
    //    xaxis: {
    //        categories:Lote,
    //        title: {
    //            text: 'Lote'
    //        }
    //    },
    //    yaxis: {
    //        title: {
    //            text: 'Rendimiento %'
    //        }
    //    }
    //})
}

var options = {
    series: [{
        name: 'Rendimiento',
        data: [21, 22, 1]
    }],
    chart: {
        height: 350,
        type: 'bar',
        //events: {
        //    click: function (chart, w, e) {
        //        // console.log(chart, w, e)
        //    }
        //}
    },
    colors: ['#005FFF', '#B548FF', '#70F1D7'],
    plotOptions: {
        bar: {
            dataLabels: {
                position: 'top', // top, center, bottom
            },
            columnWidth: '45%',
            distributed: true
        }
    },
    dataLabels: {
        enabled: true,
        formatter: function (val) {
            return val + "%";
        },
        offsetY: -20,
        style: {
            fontSize: '12px',
            colors: ["#304758"]
        }
    },
    legend: {
        show: false
    },
    xaxis: {
        categories: [
            ['REAL'],
            ['ESTANDAR'],
            ['DIFERENCIA'],

        ],
        labels: {
            style: {
                colors: ['#005FFF', '#70F7D7'],
                fontSize: '12px'
            }
        }
    }
};