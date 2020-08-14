
//modelEliminar = [];
modelEditar = [];
$(document).ready(function () {
    $("#txtFechaOrden").val($("#txtFecha").val());
    $('#txtTemperatura').inputmask({
        'alias': 'decimal',
        'groupSeparator': '',
        'digits': 2,
        'autoGroup': true,
        'digitsOptional': true,
        'max': '1000.00',
        'min': '0'
    });
    CargarOrdenFabricacion();
    ConsultarControl();   
});

function ValidaEstadoReporte(ID) {
    $.ajax({
        url: "../TiempoExposicion/ValidaEstadoReporte",
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

    MostrarModalCargando();
    $("#h4Mensaje").html("");
    //$("#tblPartial").html("");
    $.ajax({
        url: "../TiempoExposicion/TiempoExposicionPartial",
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
                //$("#h4Mensaje").html(Mensajes.SinRegistros);
                CargarDevExpress(null);

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

    $("#selectPcc").prop('selectedIndex', 0);
    $("#txtObservación").val('');
    $("#txtIdControl").val(0);
}

function Validar() {
    var valida = true;
    if ($("#selectPcc").val() == "") {
        $("#selectPcc").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectPcc").css('borderColor', '#ced4da');
    }  
    return valida;
}

function GuardarControl() {
    if (!Validar()) {
        return;
    }

    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().add(7, 'days').format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }

    $.ajax({
        url: "../TiempoExposicion/TiempoExposicion",
        type: "POST",
        data: {
            IdTiempoExposicion: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Pcc: $("#selectPcc").val(),
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
                //console.log($("#selectLote").val());
                $("#hMensaje").html($("#selectLote").val());
                $("#hMensajeOf").html($("#txtOrdenFabricacion").val());
                $("#hMensajePCC").html($("#selectPcc").val());
                modelEditar.Lote = $("#selectLote").val();
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
    $("#selectPcc").val(modelEditar.Pcc);
    $("#txtObservación").val(modelEditar.Observacion);
    $("#txtIdControl").val(modelEditar.IdTiempoExposicion);


}

function InactivarControl() {
    // console.log(model);
    // console.log(moment(modelEditar.Fecha).format('YYYY-MM-DD'));

    $.ajax({
        url: "../TiempoExposicion/EliminarTiempoExposicion",
        type: "POST",
        data: {
            IdTiempoExposicion: modelEditar.IdTiempoExposicion,
            Fecha: modelEditar.Fecha
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
    // modelEliminar = modelEditar;
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
    //ConsultaLotesOf(model.OrdenFabricacion);
    ValidaEstadoReporte(model.IdTiempoExposicion);
   ConsultarDetalle();
}

function Atras() {
    ConsultarControl();

    $("#divCabecera").prop("hidden", false);
    $("#divDetalle").prop("hidden", true);
    modelEditar = [];
    var dataGrid = $("#divCuerpo").dxDataGrid("instance");
    dataGrid.deselectAll();
    dataGrid.clearSelection();
    $("#lblAprobadoPendiente").html("");

}


/////////////////////////////////////// DETALLE ///////////////////////////////////////////////////////////////////
var ModalDetalle = [];

function ConsultarDetalle() {
    $("#h4Mensaje").html("");
    $("#tblPartial").html("");
    MostrarModalCargando();
    $.ajax({
        url: "../TiempoExposicion/TiempoExposicionDetallePartial",
        type: "GET",
        data: {
            Id: modelEditar.IdTiempoExposicion
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
                config.opcionesDT.pageLength = 20;
                config.opcionesDT.order = [[1, 'asc']];
                $('#tblDataTable').DataTable(config.opcionesDT);



            }
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function MostrarModalDetalle() {
    $("#ModalDetalle").modal("show");
    NuevoDetalle();
}

function NuevoDetalle() {
    $("#txtIdControlDetalle").val(0);
    $("#txtOrdenFabricacion").val('');
    $("#txtTemperatura").val('');
    $("#selectLote").empty();
    //$("#selectLote").append("<option value='" + model.Lote + "'>" + model.Lote + "</option>")
    $("#btnOrden").prop("readonly", false);
    $("#selectLote").prop("readonly", false);

}

function ValidarDetalle() {
    var valida = true;
   
    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }  


    if ($("#selectLote").val() == "") {
        $("#selectLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectLote").css('borderColor', '#ced4da');
    }  


    if ($("#txtTemperatura").val() == "") {
        $("#txtTemperatura").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTemperatura").css('borderColor', '#ced4da');
    }  
    return valida;
}

function GuardarDetalle() {
    if (!ValidarDetalle()) {
        return;
    }
    console.log(modelEditar.Fecha);
    //console.log(moment(modelEditar.Fecha).format('YYYY-MM-DD'));
    MostrarModalCargando();
    $.ajax({
        url: "../TiempoExposicion/GuardarTiempoExposicionDetalle",
        type: "POST",
        data: {
            IdTiempoExposicion: modelEditar.IdTiempoExposicion,
            IdTiempoExposicionDetalle: $("#txtIdControlDetalle").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Lote: $("#selectLote").val(),
            TemperaturaSala: $("#txtTemperatura").val(),
            Fecha: modelEditar.Fecha

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
                CerrarModalCargando();

            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                // nuevoControl();
                CerrarModalCargando();
            } else {
                MensajeCorrecto(resultado);
                ConsultarDetalle();
            }
            $("#ModalDetalle").modal("hide");
            //   CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();

        }
    });

    //alert("generado");
}

function EditarControlDetalle(model) {
    // console.log(model);
    NuevoDetalle();
    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtTemperatura").val(model.TemperaturaSala);
    $("#selectLote").empty();
    $("#selectLote").append("<option value='" + model.Lote + "'>" + model.Lote + "</option>")
    $("#btnOrden").prop("readonly", true);
    $("#selectLote").prop("readonly", true);
    $("#ModalDetalle").modal("show");
    $("#txtIdControlDetalle").val(model.IdTiempoExposicionDetalle);
   }



function InactivarDetalle(detalles) {
    MostrarModalCargando();
    $.ajax({
        url: "../TiempoExposicion/EliminarTiempoExposicionDetalle",
        type: "POST",
        data: {
            IdTiempoExposicionDetalle: modelEditar.IdTiempoExposicionDetalle,
            Fecha: moment(modelEditar.Fecha).format('YYYY-MM-DD')
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                CerrarModalCargando();

            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                CerrarModalCargando();

            } else if (resultado == "1") {
                $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                MensajeAdvertencia(Mensajes.ControlAprobado);
                CerrarModalCargando();

            } else {
                MensajeCorrecto(resultado);
                ConsultarDetalle();
            }
            $("#modalEliminarDetalle").modal("hide");

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            CerrarModalCargando();
        }
    });
}

function EliminarControlDetalle(model) {
    $("#modalEliminarDetalle").modal('show');
    ModalDetalle = model;
    // modelEliminar = modelEditar;
}

$("#modal-si").on("click", function () {
    var result = [ModalDetalle.IdTiempoExposicionDetalle];
    InactivarDetalle(result);
    $("#modalEliminarDetalle").modal('hide');
});

$("#modal-no").on("click", function () {
    $("#modalEliminarDetalle").modal('hide');
});


function CargarOrdenFabricacion() {
    valor = $("#txtFechaOrden").val();
    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../TiempoExposicion/ConsultarOrdenesFabricacion",
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
        url: "../TiempoExposicion/ConsultaLotes",
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
        url: "../TiempoExposicion/ConsultaLotes",
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
    if (data != null) {
        data.forEach(function (x, y) {
            x.Fecha = moment(x.Fecha).format("YYYY-MM-DD");
        });

    }

    DevExpress.localization.locale(navigator.language);
    var opciosGrid = {
        dataSource: data,
        loadPanel: {
            enabled: true
        },
        keyExpr: "IdTiempoExposicion",
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
        },
       
        columns: [
            { dataField: "IdTiempoExposicion", caption: "Id", sortOrder: "asc" },
            // { caption: "Parametro", dataField: "DescripcionParametroSensorial"  },
            {
                caption: "Fecha", dataField: "Fecha"
            },
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
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'TiempoExposicion.xlsx');
                });
            });
            e.cancel = true;
        }
    }

    $("#divCuerpo").dxDataGrid(opciosGrid);
    // $("#divCuerpo").deselectAll();

}