var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
    $("#divCabecera2").prop("hidden", false);
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoIntermediaPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);

            }
        },
        error: function(resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function MostarModal() {
    $("#ModalControl").modal("show");
    NuevoControl();
}


function Validar() {
    var bool = true;


    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    if ($("#txtAbreviatura").val() == "") {
        $("#txtAbreviatura").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtAbreviatura").css('borderColor', '#ced4da');
    }

    if ($("#selectColor1").val() == "") {
        $("#selectColor1").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectColor1").css('borderColor', '#ced4da');
    }

    return bool;
}

function GuardarModificarControl() {
    if (!Validar()) { return; }

    var estado = 'A';
    MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoApariencia",
        type: "POST",
        data: {
            IdApariencia: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val(),
            EstadoRegistro: estado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CerrarModalCargando();
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoControl();
                ConsultarReporte();
            }
            $("#ModalControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
            CerrarModalCargando();

        }
    });

    //alert("generado");
}

function CambioEstado(valor) {
    //  console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}



function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdApariencia);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtAbreviatura").val(model.Abreviatura);
    $("#ModalControl").modal("show");

}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}


function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}




function EliminarCabeceraSi() {
    MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/EliminarMantenimientoApariencia",
        type: "POST",
        data: {
            IdApariencia: itemEditar.IdApariencia,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                ConsultarReporte();
                MensajeCorrecto("Registro Actualizado con Éxito");
                CerrarModalCargando();
                itemEditar = 0;
            }
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}


////////////////////////////////////////////////// MANTENIMIENTO CALIFICACION //////////////////////////////////
var bandera = 0;


function NuevoMantenimiento() {
    $("#txtIdControl").val('');
    $("#txtDescripcion").val('');
    $("#txtAbreviatura").val('');
}

function MostrarModalCalificacion() {
    ConsultarMantenimientoCalificacion();
    $("#ModalCalificacion").modal("show");
}



function ConsultarMantenimientoCalificacion() {
  //  $("#divMantenimientoCalificacion").html('');
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoCalificacionPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMantenimientoCalificacion").html("No existen registros");
            } else {
                CargarDevExpressCalificacion(resultado)
               // $("#divMantenimientoCalificacion").html(resultado);

            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function MostarModalGuardarParametro(b) {
    bandera = b;
    $("#ModalControl").modal("show");
}

function EditarManteniminetoCalificacion(model) {
    console.log(model);
    $("#ModalControl").modal("show");
    $("#txtIdControl").val(model.IdCondicion);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtAbreviatura").val(model.Abreviatura);
}

function GuardarModificar() {
    if (bandera == 1) {
        GuardarModificarCalificacion();

    } else {
        GuardarModificarParametroSensorial();

    }
}






function Validar() {
    var bool = true;


    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    if ($("#txtAbreviatura").val() == "") {
        $("#txtAbreviatura").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtAbreviatura").css('borderColor', '#ced4da');
    }

     return bool;
}

function GuardarModificarCalificacion() {
    if (!Validar()) { return; }

  //  MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoCalificacion",
        type: "POST",
        data: {
            IdApariencia: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
        //    CerrarModalCargando();
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoMantenimiento();
                ConsultarMantenimientoCalificacion();
            }
            $("#ModalControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
      //      CerrarModalCargando();

        }
    });

    //alert("generado");
}



function GuardarModificarParametroSensorial() {
    if (!Validar()) { return; }

  //  MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoApariencia",
        type: "POST",
        data: {
            IdApariencia: $("#txtIdControl").val(),
            Descripcion: $("#txtDescripcion").val(),
            Abreviatura: $("#txtAbreviatura").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
      //      CerrarModalCargando();
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoMantenimiento();
                ConsultarReporte();
            }
            $("#ModalControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
    //        CerrarModalCargando();

        }
    });

    //alert("generado");
}



function CargarDevExpressCalificacion(data) {
    //console.log(data);
    DevExpress.localization.locale(navigator.language);
    var opciosGrid = {
        dataSource: data,
        loadPanel: {
            enabled: true
        },
        keyExpr: "IdCalificacion",
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
        groupPanel: { visible: true },
        grouping: {
            autoExpandAll: false
        },

        columns: [
            { dataField: "IdCalificacion", caption: "Id", sortOrder: "desc" },
            { caption: "Descripción", dataField: "Descripcion", area: "column" },
            { dataField: "Abreviatura", width: 180 },
            {
                caption: "Estado",
                dataField: "EstadoRegistro", dataType: "string",
                cellTemplate: function (container, options) {
                    var estiloClass = 'badge';
                    var estado = "Activo";
                    if (options.data.EstadoRegistro == "A") {
                         estiloClass = 'badge badge-success';
                    }else{
                         estiloClass = 'badge badge-warning';
                        estado = "Inactivo";
                    } 
                    $("<div>")
                        .append($('<span class="' + estiloClass + '">' + estado+'</span>'))
                        .appendTo(container);

                }
            },
            {
                caption: "", cellTemplate: function (container, options) {
                    $("<divxº>")
                        //.append($('<a class="badge badge-danger btnReversa" onclick="ReversarSolicitud(' + options.data.IdSolicitudPermiso + ')"> Reversar</span>'))
                        .append($('<button id="btnActualizar" class="btn btn-link" onclick="EditarManteniminetoCalificacion(' + "'" + options.data + "'" + ')"/button>'))
                        .appendTo(container);

                }
            },
            {
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
            

        ],
      
        selection: {
            mode: 'multiple'
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
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ManteniminetoCalificacionAS.xlsx');
                });
            });
            e.cancel = true;
        }
    }

    $("#divMantenimientoCalificacion").dxDataGrid(opciosGrid);
}



var modal_lv = 0;
$('.modal').on('shown.bs.modal', function (e) {
    $('.modal-backdrop:last').css('zIndex', 1051 + modal_lv);
    $(e.currentTarget).css('zIndex', 1052 + modal_lv);
    modal_lv++
});
$('.modal').on('hidden.bs.modal', function (e) {
    modal_lv--
});