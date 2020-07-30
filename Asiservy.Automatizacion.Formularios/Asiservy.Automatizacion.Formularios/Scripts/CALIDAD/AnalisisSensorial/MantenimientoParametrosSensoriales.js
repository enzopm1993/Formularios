var itemEditar = [];
$(document).ready(function () {
    ConsultarReporte();
});

function ConsultarReporte() {
    $("#chartCabecera2").html('');
 
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoIntermediaPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
 
            } else {
                console.log(resultado);
                $("#spinnerCargando").prop("hidden", true);
               // $("#chartCabecera2").html(resultado);

            }
        },
        error: function(resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
 
        }
    });
}

function MostarModal() {
   // bandera = 3;
    $("#ModalControlIntermedia").modal("show");
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

function ActualizarCabecera(model) {
    $("#txtIdControl").val(model.IdApariencia);
    $("#txtDescripcion").val(model.Descripcion);
    $("#txtAbreviatura").val(model.Abreviatura);
    $("#ModalControl").modal("show");

}

function InactivarConfirmar(jdata) {
    console.log(jdata);
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

function EliminarCabecera() {
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
                itemEditar = 0;
            }
        },
        error: function (resultado) {
            CerrarModalCargando();
        }
    });
}

function EliminarCabeceraSi() {
    if (bandera == 1) {
        EliminarCalificacion();
    } else if (bandera == 2) {
        EliminarParametroSensorial();
    }


    
}


////////////////////////////////////////////////// MANTENIMIENTO CALIFICACION //////////////////////////////////
var bandera = 0;


function NuevoMantenimiento() {
    $("#txtIdControl").val('');
    $("#txtDescripcion").val('');
    $("#txtAbreviatura").val('');
}

function MostrarModalCalificacion() {
    bandera = 1;
    $("#tituloModal").html("Mantenimiento Calificación");
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
                
                CargarDevExpressCalificacion(null);

            } else {
                CargarDevExpressCalificacion(resultado);
               // $("#divMantenimientoCalificacion").html(resultado);

            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EditarManteniminetoCalificacion(IdCalificacion, Descripcion, Abreviatura) {
    //console.log(IdCondicion);
    bandera = 1;
    $("#ModalControl").modal("show");
    $("#txtIdControl").val(IdCalificacion);
    $("#txtDescripcion").val(Descripcion);
    $("#txtAbreviatura").val(Abreviatura);
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
            IdCalificacion: $("#txtIdControl").val(),
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

function EliminarCalificacion() {
    console.log(itemEditar);
    $.ajax({
        url: "../AnalisisSensorial/EliminarCalificacion",
        type: "POST",
        data: {
            IdCalificacion: itemEditar.IdCalificacion,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                ConsultarMantenimientoCalificacion();
                MensajeCorrecto("Registro Actualizado con Éxito");
                itemEditar = 0;
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
            $("#modalEliminarControl").modal("hide");
        }
    });
}

function GuardarModificar() {
    if (bandera == 1) {
        GuardarModificarCalificacion();

    } else {
        GuardarModificarParametroSensorial();

    }
}



/////////////////////////////////////// PARAMETRO SENSORIAL //////////////////////////////////////////////

function MostrarModalParametro() {
    bandera = 2;
    $("#tituloModal").html("Parametro Sensorial");
    ConsultarParametroSensorial();
    $("#ModalCalificacion").modal("show");
}

function ConsultarParametroSensorial() {
    //  $("#divMantenimientoCalificacion").html('');
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoParametroSensorialPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                CargarDevExpressParametroSensorial(null);
            } else {
                CargarDevExpressParametroSensorial(resultado);
                // $("#divMantenimientoCalificacion").html(resultado);

            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function MostarModalGuardar() {
    NuevoMantenimiento();
    $("#ModalControl").modal("show");
}

function GuardarModificarParametroSensorial() {
    if (!Validar()) { return; }

  //  MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoParametroSensorial",
        type: "POST",
        data: {
            IdParametroSensorial: $("#txtIdControl").val(),
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
                ConsultarParametroSensorial();
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

function EditarParametroSensorial(IdCalificacion, Descripcion, Abreviatura) {
    //console.log(IdCondicion);
    bandera = 2;
    $("#ModalControl").modal("show");
    $("#txtIdControl").val(IdCalificacion);
    $("#txtDescripcion").val(Descripcion);
    $("#txtAbreviatura").val(Abreviatura);
}

function EliminarParametroSensorial() {
    //console.log(itemEditar);
    $.ajax({
        url: "../AnalisisSensorial/EliminarParametroSensorial",
        type: "POST",
        data: {
            IdParametroSensorial: itemEditar.IdParametroSensorial,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                ConsultarParametroSensorial();
                MensajeCorrecto("Registro Actualizado con Éxito");
                itemEditar = 0;
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
            $("#modalEliminarControl").modal("hide");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////

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

        columns: [
            { dataField: "IdCalificacion", caption: "Id", sortOrder: "ascss" },
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
                         estiloClass = 'badge badge-danger';
                        estado = "Inactivo";
                    } 
                    $("<div>")
                        .append($('<span class="' + estiloClass + '">' + estado+'</span>'))
                        .appendTo(container);

                }
            },
            {
                caption: "Acciones", cellTemplate: function (container, options) {
                    var btnEditar = '<button id="btnActualizar" class="btn btn-link" onclick="EditarManteniminetoCalificacion(' + options.data.IdCalificacion + ',' + "'" + options.data.Descripcion + "'" + ',' + "'" + options.data.Abreviatura + "'"+')"> Editar</button>';
                    var btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='InactivarConfirmar(" + JSON.stringify(options.data) + ")'> Inactivar</button>";
                    if (options.data.EstadoRegistro == "I") {
                        btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='ActivarConfirmar(" + JSON.stringify(options.data) + ")'> Activar</button>";
                    }
                    $("<div>")
                        .append($(btnEditar))
                        .append($(btnActivar))
                        .appendTo(container);
                }
            },
            {
                caption: "Fecha Ingreso" ,
                cellTemplate: function (container, options) {
                    var fecha = moment(options.data.FechaIngresoLog).format("DD-MM-YYYY");
                  //  console.log(fecha);
                    $("<div>")
                        .append($('<label>'+fecha+'</label>'))
                        .appendTo(container);
                }
            }
            , {
                caption: "Usuario Ingreso",
                dataField: "UsuarioIngresoLog",
                area: "column",
                dataType: "string"
            }
            
            , {
                caption: "Usuario modificación",
                dataField: "UsuarioModificacionLog",
                area: "column",
                dataType: "string"
            }
            , {
                caption: "Fecha Modificación",
                cellTemplate: function (container, options) {
                    if (options.data.FechaModificacionLog != null) {
                        var fecha = moment(options.data.FechaModificacionLog).format("DD-MM-YYYY");
                        //console.log(fecha);
                        $("<div>")
                            .append($('<label>' + fecha + '</label>'))
                            .appendTo(container);
                    }
                }
            }
           

        ],
      
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

    $("#divMantenimientoCalificacion").dxDataGrid(opciosGrid);
}
function CargarDevExpressParametroSensorial(data) {
    //console.log(data);
    DevExpress.localization.locale(navigator.language);
    var opciosGrid = {
        dataSource: data,
        loadPanel: {
            enabled: true
        },
        keyExpr: "IdParametroSensorial",
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
            { dataField: "IdParametroSensorial", caption: "Id", sortOrder: "ascss" },
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
                    } else {
                        estiloClass = 'badge badge-danger';
                        estado = "Inactivo";
                    }
                    $("<div>")
                        .append($('<span class="' + estiloClass + '">' + estado + '</span>'))
                        .appendTo(container);

                }
            },
            {
                caption: "Acciones", cellTemplate: function (container, options) {
                    var btnEditar = '<button id="btnActualizar" class="btn btn-link" onclick="EditarParametroSensorial(' + options.data.IdParametroSensorial + ',' + "'" + options.data.Descripcion + "'" + ',' + "'" + options.data.Abreviatura + "'" + ')"> Editar</button>';
                    var btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='InactivarConfirmar(" + JSON.stringify(options.data) + ")'> Inactivar</button>";
                    if (options.data.EstadoRegistro == "I") {
                        btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='ActivarConfirmar(" + JSON.stringify(options.data)+")'> Activar</button>";
                    }
                    $("<div>")
                        .append($(btnEditar))
                        .append($(btnActivar))
                        .appendTo(container);
                }
            },
            {
                caption: "Fecha Ingreso",
                cellTemplate: function (container, options) {
                    var fecha = moment(options.data.FechaIngresoLog).format("DD-MM-YYYY");
                    //console.log(fecha);
                    $("<div>")
                        .append($('<label>' + fecha + '</label>'))
                        .appendTo(container);
                }
            }
            , {
                caption: "Usuario Ingreso",
                dataField: "UsuarioIngresoLog",
                area: "column",
                dataType: "string"
            }

            , {
                caption: "Usuario modificación",
                dataField: "UsuarioModificacionLog",
                area: "column",
                dataType: "string"
            }
            , {
                caption: "Fecha Modificación",
                cellTemplate: function (container, options) {
                    if (options.data.FechaModificacionLog != null) {
                        var fecha = moment(options.data.FechaModificacionLog).format("DD-MM-YYYY");
                        console.log(fecha);
                        $("<div>")
                            .append($('<label>' + fecha + '</label>'))
                            .appendTo(container);
                    }
                }
            }


        ],

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