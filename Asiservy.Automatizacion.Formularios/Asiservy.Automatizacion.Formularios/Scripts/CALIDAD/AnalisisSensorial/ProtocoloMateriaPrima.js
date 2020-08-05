//model = [];
modelEliminar = [];
modelEditar = [];
$(document).ready(function () {
   // ValidaEstadoReporte($("#txtFecha").val());
    $("#txtFechaOrden").val($("#txtFecha").val());
    CargarOrdenFabricacion();
    ConsultarControl();
});


function ValidaEstadoReporte(ID) {
    $.ajax({
        url: "../AnalisisSensorial/ValidaEstadoReporte",
        type: "GET",
        data: {
            ID: ID
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                $("#lblAprobadoPendiente").html("");
                $("#h4Mensaje").html(Mensajes.SinRegistros);


            } else if (resultado == 1) {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);

            } else {
                $("#lblAprobadoPendiente").removeClass("badge-info").addClass("badge-danger");
                $("#lblAprobadoPendiente").html(Mensajes.Pendiente);
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function ConsultarControl() {
    if ($("#txtFecha").val() == '') {
        return;
    }
    //$("#txtFechaOrden").val($("#txtFecha").val());
    model = [];
   // ValidaEstadoReporte($("#txtFecha").val());
    MostrarModalCargando();
  
    $("#h4Mensaje").html("");
    $("#tblPartial").html("");
    $.ajax({
        url: "../AnalisisSensorial/ProtocoloMateriaPrimaPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#h4Mensaje").html(Mensajes.SinRegistros);
            } else {
                //console.log(resultado);
                CargarDevExpress(resultado);
                //$("#tblPartial").html(resultado);
            }
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function MostrarModal() {
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
        $("#modalControl").modal("show");
        nuevoControl();
    }
}

function nuevoControl() {
    $("#txtFechaDescarga").val('');
    $("#txtFechaEvaluación").val('');
    $("#txtOrdenFabricacion").val('');
    $("#selectLote").prop('selectedIndex', 0);
    $("#selectPcc").prop('selectedIndex', 0);
    $("#txtCodigoProtocolo").val('');
    $("#txtLoteDescarga").val('');
    $("#txtObservación").val('');
    $("#txtIdControl").val(0);
}

function Validar() {
    var valida = true;

    if ($("#txtFechaDescarga").val() == "") {
        $("#txtFechaDescarga").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFechaDescarga").css('borderColor', '#ced4da');
    }

    if ($("#txtFechaEvaluación").val() == "") {
        $("#txtFechaEvaluación").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFechaEvaluación").css('borderColor', '#ced4da');
    }

    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }

    if ($("#selectPcc").val() == "") {
        $("#selectPcc").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectPcc").css('borderColor', '#ced4da');
    }

    if ($("#selectLote").val() == "") {
        $("#selectLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectLote").css('borderColor', '#ced4da');
    }

   

    //if ($("#selectParametro").val() == "") {

    //    $("#selectParametro").each(function () {
    //        $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
    //    });

    //    valida = false;
    //} else {
    //    $("#selectParametro").each(function () {
    //        $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
    //    });

    //}

    


    return valida;
}

function GuardarControl() {
    if (!Validar()) {
        return;
    }

    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().add(7,'days').format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }

    var formdata = new FormData();
    formdata.append("Fecha", $("#txtFecha").val());

    $.ajax({
        url: "../AnalisisSensorial/ProtocoloMateriaPrima",
        type: "POST",
        data: {
            IdProtocoloMateriaPrima: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            FechaDescarga: $("#txtFechaDescarga").val(),
            FechaEvaluacion: $("#txtFechaEvaluación").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Lote: $("#selectLote").val(),
            Pcc: $("#selectPcc").val(),
            CodigoProtocolo: $("#txtCodigoProtocolo").val(),
            LoteDescarga: $("#txtLoteDescarga").val(),
            Observacion: $("#txtObservación").val()
           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                nuevoControl();
            } else {
                ConsultarControl();
            }
            $("#modalControl").modal("hide");

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });

    //alert("generado");
}

function EditarControl() {
    //console.log(modelEditar);

    $("#modalControl").modal('show');
    $("#txtFechaDescarga").val(moment(modelEditar.FechaDescarga).format('YYYY-MM-DD'));
    $("#txtFechaEvaluación").val(moment(modelEditar.FechaEvaluacion).format('YYYY-MM-DD'));
    $("#txtOrdenFabricacion").val(modelEditar.OrdenFabricacion);
    $("#selectLote").val(modelEditar.Lote);
    $("#txtCodigoProtocolo").val(modelEditar.CodigoProtocolo);
    $("#txtLoteDescarga").val(modelEditar.LoteDescarga);
    $("#selectPcc").val(modelEditar.Pcc);
    $("#txtObservación").val(modelEditar.Observacion);
    $("#txtIdControl").val(modelEditar.IdProtocoloMateriaPrima);

    $("#btnOrden").prop("disabled", true);
    
}


function InactivarControl() {
    // console.log(model);
   // console.log(moment(modelEditar.Fecha).format('YYYY-MM-DD'));

    $.ajax({
        url: "../AnalisisSensorial/EliminarProtocoloMateriaPrima",
        type: "POST",
        data: {
            IdProtocoloMateriaPrima: modelEditar.IdProtocoloMateriaPrima,
            Fecha: moment(modelEditar.Fecha).format('YYYY-MM-DD')
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
            } else {
                MensajeCorrecto(resultado);
                Atras();
            }
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarControl() {
    $("#modalEliminarControlDetalle").modal('show');
    modelEliminar = model;
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});


$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});

$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").val() == '') {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
    ConsultaLotes();
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});


function SeleccionarControl(model) {
    modelEditar = model;
    $("#divCabecera").prop("hidden", true);
    $("#divDetalle").prop("hidden", false);
    ConsultaLotesOf(model.OrdenFabricacion);
    ValidaEstadoReporte(model.IdProtocoloMateriaPrima);
    ConsultarDetalle();
    $("#hMensaje").html(model.Lote);
}

function Atras() {
    $("#divCabecera").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    modelEditar = [];
    ConsultarControl();
    var dataGrid = $("#divCuerpo").dxDataGrid("instance");
    dataGrid.deselectAll();
    dataGrid.clearSelection();
    $("#lblAprobadoPendiente").html("");
}



/////////////////////////////////////// DETALLE ///////////////////////////////////////////////////////////////////


function ConsultarDetalle() {
   
    $("#h4Mensaje").html("");
    $("#tblPartial").html("");
    $.ajax({
        url: "../AnalisisSensorial/ProtocoloMateriaPrimaDetallePartial",
        type: "GET",
        data: {
            Id: modelEditar.IdProtocoloMateriaPrima
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#tblPartial").prop("hidden", false);
            if (resultado == "0") {
                $("#h4Mensaje").html(Mensajes.SinRegistros);
            } else {
                //console.log(resultado);
                
                $("#tblPartial").html(resultado);
            }
          //  CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        //    CerrarModalCargando();
        }
    });
}

function MostrarModalDetalle() {
    $("#ModalDetalle").modal("show");
}




function CargarOrdenFabricacion() {
    valor = $("#txtFechaOrden").val();
    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../AnalisisSensorial/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#SelectOrdenFabricacion").empty();
                $("#SelectOrdenFabricacion").append("<option value='' >-- Error de servicio--</option>");
                return;
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.Orden + "'>" + row.Orden + "</option>")
                });
                $('#validaFecha').prop("hidden", true);

            } else {
                $('#validaFecha').prop("hidden", false);
            }
            //CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
function ConsultaLotes() {
    valor = $("#SelectOrdenFabricacion").val();
    if (valor == '' || valor == null)
        return;
    $("#selectLote").empty();
    $("#selectLote").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../AnalisisSensorial/ConsultaLotes",
        type: "GET",
        data: {
            Of: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#selectLote").empty();
                $("#selectLote").append("<option value='' >-- Error de servicio--</option>");
                return;
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectLote").append("<option value='" + row.Lote + "'>" + row.Lote + "</option>")
                });
            } 
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
function ConsultaLotesOf(Of) {
    if (Of == '' || Of == null)
        return;
    $("#selectLote").empty();
    $("#selectLote").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../AnalisisSensorial/ConsultaLotes",
        type: "GET",
        data: {
            Of: Of
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#selectLote").empty();
                $("#selectLote").append("<option value='' >-- Error de servicio--</option>");
                return;
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectLote").append("<option value='" + row.Lote + "'>" + row.Lote + "</option>")
                });
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}
function CargarDevExpress(data) {
    //console.log(data);
    DevExpress.localization.locale(navigator.language);
    var opciosGrid = {
        dataSource: data,
        loadPanel: {
            enabled: true
        },
        keyExpr: "IdProtocoloMateriaPrima",
        selection: {
            mode: "single"
        },
        hoverStateEnabled: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
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
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 50, 100],
            showInfo: true
        },
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true,
            width: 240,
            placeholder: "Buscar..."
        }, groupPanel: { visible: true },
        grouping: {
            autoExpandAll: true
        },

        columns: [
            { dataField: "IdProtocoloMateriaPrima", caption: "Id", sortOrder: "asc" },
            // { caption: "Parametro", dataField: "DescripcionParametroSensorial"  },
            {
                caption: "Fecha", dataField: "Fecha",
                cellTemplate: function (container, options) {
                    var fecha = moment(options.data.Fecha).format("DD-MM-YYYY");
                    //  console.log(fecha);
                    $("<div>")
                        .append($('<label>' + fecha + '</label>'))
                        .appendTo(container);
                } },
            {
                caption: "Fecha Evaluacion", dataField: "FechaEvaluacion",
                cellTemplate: function (container, options) {
                    var fecha = moment(options.data.FechaEvaluacion).format("DD-MM-YYYY");
                    //  console.log(fecha);
                    $("<div>")
                        .append($('<label>' + fecha + '</label>'))
                        .appendTo(container);
                } },
            {
                caption: "Fecha Descarga", dataField: "FechaDescarga",
                cellTemplate: function (container, options) {
                    var fecha = moment(options.data.FechaDescarga).format("DD-MM-YYYY");
                    //  console.log(fecha);
                    $("<div>")
                        .append($('<label>' + fecha + '</label>'))
                        .appendTo(container);
                } },
            {
                caption: "O.F.",
                dataField: "OrdenFabricacion",
            },
            { dataField: "Lote" },
            { dataField: "LoteDescarga" },
            { dataField: "CodigoProtocolo" },
            { dataField: "Pcc" },
            { dataField: "Observacion" }
        ],
        onSelectionChanged: function (selectedItems) {
            //console.log(selectedItems);
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                SeleccionarControl(data);
                

            }
        },
        //selection: {
        //    mode: "none" // or "multiple" | "none"
        //},
        export: {
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
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ManteniminetoCalificacionAS.xlsx');
                });
            });
            e.cancel = true;
        }
    }

    $("#divCuerpo").dxDataGrid(opciosGrid);
   // $("#divCuerpo").deselectAll();
   
}