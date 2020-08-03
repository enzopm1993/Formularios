var itemEditar = [];
//MostrarModalCargando();
$(document).ready(function () {
    ConsultarReporte();
    // ConsultarComboCalificacion();
  //  CerrarModalCargando();
    ConsultarComboParametroSensorial();
    $("#btnMantenimientoCalificacion").prop("disabled", false);
    $("#btnMantenimientoParametro").prop("disabled", false);
});

function ConsultarReporte() {
    //$("#chartCabecera2").html('');
 
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
              //  console.log(resultado);
                $("#spinnerCargando").prop("hidden", true);
               // $("#chartCabecera2").html(resultado);
                CargarDevExpressIntermedia(resultado);
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
   // ConsultarComboParametroSensorial();
    NuevoControl();
}

function NuevoControl() {
    $("#txtDescripcionIntermedia").val("");
    $("#selectCalificacion").empty();
    $("#selectCalificacion").append('<option value="">Seleccione</option>');
    ConsultarComboParametroSensorial();
}

function ValidarControl() {
    var bool = true;


    if ($("#txtDescripcionIntermedia").val() == "") {
        $("#txtDescripcionIntermedia").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtDescripcionIntermedia").css('borderColor', '#ced4da');
    }

    if ($("#selectCalificacion").val() == "") {
        $("#selectCalificacion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectCalificacion").css('borderColor', '#ced4da');
    }

    if ($("#selectParametro").val() == "") {
        $("#selectParametro").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectParametro").css('borderColor', '#ced4da');
    }

    return bool;
}

function GuardarModificarControl() {
    if (!ValidarControl()) { return; }

    var estado = 'A';
    MostrarModalCargando();
    $.ajax({
        url: "../AnalisisSensorial/MantenimientoIntermedia",
        type: "POST",
        data: {
       //     IdIntermedia: $("#txtIdControl").val(),
            IdParametroSensorial: $("#selectParametro").val(),
            IdCalificacion: $("#selectCalificacion").val(),
            Descripcion: $("#txtDescripcionIntermedia").val(),
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
            $("#ModalControlIntermedia").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
            CerrarModalCargando();

        }
    });

    //alert("generado");
}

function EditarIntermedia(model) {
    //  $("#txtIdControl").val(model.IdApariencia);
    //console.log(model);
    $("#selectParametro").empty();
    $("#selectParametro").append('<option value="' + model.IdParametroSensorial + '">' + model.DescripcionParametroSensorial + '</option>');
    $("#selectCalificacion").empty();
    $("#selectCalificacion").append('<option value="' + model.IdCalificacion + '">' + model.DescripcionCalificacion + '</option>');
    $("#txtDescripcionIntermedia").val(model.Descripcion);
    $("#ModalControlIntermedia").modal("show");

}

function InactivarConfirmar(jdata,b) {
    console.log(b);
    bandera = b;
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata,b) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
    bandera = b;
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function EliminarIntermedia() {
    $.ajax({
        url: "../AnalisisSensorial/EliminarIntermedia",
        type: "POST",
        data: {
            IdParametroSensorial: itemEditar.IdParametroSensorial,
            IdCalificacion: itemEditar.IdCalificacion,
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
    } else if (bandera == 3) {
        EliminarIntermedia();
    } else {
        MensajeAdvertencia("No se pudo eliminar");
    }


    
}


function ConsultarComboCalificacion(idParametro) {
    //  $("#divMantenimientoCalificacion").html('');
    $.ajax({
        url: "../AnalisisSensorial/ConsultaComboMantenimientoCalificacion",
        type: "GET",
        data: { IdParametro: idParametro },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {

                CargarDevExpressCalificacion(null);

            } else {
                $("#selectCalificacion").empty();
                $("#selectCalificacion").append('<option value="">Seleccione</option>');
                $.each(resultado, function (key, registro) {
                    //console.log(resultado);
                    $("#selectCalificacion").append('<option value=' + registro.IdCalificacion + '>' + registro.Descripcion + '</option>');
                });   

            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ConsultarComboParametroSensorial() {
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
                $("#selectParametro").empty();
                $("#selectParametro").append('<option value="">Seleccione</option>');
                $.each(resultado, function (key, registro) {
                    //console.log(resultado);
                    $("#selectParametro").append('<option value=' + registro.IdParametroSensorial + '>' + registro.Descripcion + '</option>');
                });  
            }
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function CambiaParmetro() {
    if ($("#selectParametro").val() != '') {
        ConsultarComboCalificacion($("#selectParametro").val());
    } else {
        $("#selectCalificacion").empty();
        $("#selectCalificacion").append('<option value="">Seleccione</option>');
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
                ConsultarComboCalificacion();
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
   // console.log(itemEditar);
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

    } else if (bandera == 2) {
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
                ConsultarComboParametroSensorial();
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

function CargarDevExpressIntermedia(data) {
    //console.log(data);
    DevExpress.localization.locale(navigator.language);
    var opciosGrid = {
        dataSource: data,
        loadPanel: {
            enabled: true
        },
        keyExpr: "IdIntermedia",
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
            { dataField: "IdIntermedia", caption: "Id", sortOrder: "ascss" },
           // { caption: "Parametro", dataField: "DescripcionParametroSensorial"  },
            { caption: "Calificación", dataField: "DescripcionCalificacion", area: "row"},
            { dataField: "Descripcion" },
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
                  //  var btnEditar = '<button id="" class="btn btn-link" onclick="EditarIntermedia(' + options.data.IdCalificacion + ',' + "'" + options.data.Descripcion + "'" + ',' + "'" + options.data.Abreviatura + "'"+')"> Editar</button>';
                    var btnEditar = "<button id='' class='btn btn-link' onclick='EditarIntermedia(" + JSON.stringify(options.data) + ")'> Editar</button>";
                    var btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='InactivarConfirmar(" + JSON.stringify(options.data) + ",3)'> Inactivar</button>";
                    if (options.data.EstadoRegistro == "I") {
                        btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='ActivarConfirmar(" + JSON.stringify(options.data) + ",3)'> Activar</button>";
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
            },{
                caption: "Parametro",
                dataField: "DescripcionParametroSensorial",
                groupIndex: 0
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

    $("#chartCabecera2").dxDataGrid(opciosGrid);
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
                    var btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='InactivarConfirmar(" + JSON.stringify(options.data) + ",1)'> Inactivar</button>";
                    if (options.data.EstadoRegistro == "I") {
                        btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='ActivarConfirmar(" + JSON.stringify(options.data) + ",1)'> Activar</button>";
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
                    var btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='InactivarConfirmar(" + JSON.stringify(options.data) + ",2)'> Inactivar</button>";
                    if (options.data.EstadoRegistro == "I") {
                        btnActivar = "<button id='btnActualizar' class='btn btn-link' onclick='ActivarConfirmar(" + JSON.stringify(options.data)+",2)'> Activar</button>";
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