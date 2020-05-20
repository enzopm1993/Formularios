$(document).ready(function () {
    ConsultarReporteMaestros();
    $("#selectOpcion").select2();
    $("#selectOpcionModal").select2();
});

var ReporteModel = [];
var rotation = 0;


function ConsultarReporteMaestros() {

    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ReporteMaestro/ReporteMaestroPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error, comuniquese con sistemas. " + resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



function GenerarReporteMaestro() {
    if ($("#txtIdControl").val() == "") {
        $("#txtIdControl").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtIdControl").css('borderColor', '#ced4da');
    }
    if ($("#txtNombre").val() == "") {
        $("#txtNombre").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtNombre").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigo").val() == "") {
        $("#txtCodigo").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtCodigo").css('borderColor', '#ced4da');
    }
     if ($("#selectOpcion").val() == "") {
        //$("#SelectTextura").css('borderColor', '#FA8072');
        $("#selectOpcion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });
        valida = false;
    } else {
        $("#selectOpcion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
    }

    $.ajax({
        url: "../ReporteMaestro/ReporteMaestro",
        type: "POST",
        data: {
            IdReporteMaestro: $("#txtIdControl").val(),
            Nombre: $("#txtNombre").val(),
            Codigo: $("#txtCodigo").val(),
            IdOpcion: $("#selectOpcion").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }
      //      NuevaReporteMaestro();
            MensajeCorrecto("Registro Exitoso");
            ConsultarReporteMaestros();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError("Error, comuniquese con sistemas. " + result, false);
        }
    });
}

function NuevoControl() {
    $("#txtIdControl").val('0');
    $("#txtNombre").val('');
    $("#txtCodigo").val('');
    $("#txtVersion").val('');
    $("#selectOpcion").prop("selectedIndex",0).change()
    $("#selectOpcionModal").prop("selectedIndex", 0).change()
    $("#txtIdControlModal").val('0');
    $("#txtNombreModal").val('');
    $("#txtCodigoModal").val('');
    $("#txtVersionModal").val('');

}

function SeleccionarReporteMaestro(model) {
   // alert(model);
    $("#hMensaje").prop("hidden", false);
    $("#hMensaje").html(model.Nombre);
    $("#selectOpcion").val(model.IdOpcion).change()
    $("#txtIdControl").val(model.IdReporteMaestro);
    $("#txtNombre").val(model.Nombre);
    $("#txtCodigo").val(model.Codigo);
    $("#txtVersion").val(model.UltimaVersion);

    $("#selectOpcionModal").val(model.IdOpcion).change()
    $("#txtIdControlModal").val(model.IdReporteMaestro);
    $("#txtNombreModal").val(model.Nombre);
    $("#txtCodigoModal").val(model.Codigo);
    $("#txtVersionModal").val(model.UltimaVersion);

    $("#divCabecera").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);
    $("#divCabeceraPartial").prop("hidden", true);

    $("#btnEditar").prop("hidden", false);
    $("#btnEliminar").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#divDetalle").prop("hidden", false);

    ReporteModel = model;
    ConsultarReporteDetalle(ReporteModel.IdReporteMaestro);

}
function Atras() {

    $("#hMensaje").prop("hidden", true);
    $("#hMensaje").html('');

    $("#divCabecera").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    $("#divCabeceraPartial").prop("hidden", false);

    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);

    $("#btnAtras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    NuevoControl();
    ConsultarReporteMaestros();
}


function EditarReporte() {
    $("#ModalEditarControl").modal("show");
}

function EditarReporteMaestro() {
    if ($("#txtIdControlModal").val() == "") {
        $("#txtIdControlModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtIdControlModal").css('borderColor', '#ced4da');
    }
    if ($("#txtNombreModal").val() == "") {
        $("#txtNombreModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtNombreModal").css('borderColor', '#ced4da');
    }
    if ($("#txtCodigoModal").val() == "") {
        $("#txtCodigoModal").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtCodigoModal").css('borderColor', '#ced4da');
    }
    if ($("#selectOpcionModal").val() == "") {
        //$("#SelectTextura").css('borderColor', '#FA8072');
        $("#selectOpcionModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });
        valida = false;
    } else {
        $("#selectOpcionModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
    }
    //if ($("#txtVersionModal").val() == "") {
    //    $("#txtVersionModal").css('borderColor', '#FA8072');
    //    return;
    //} else {
    //    $("#txtVersionModal").css('borderColor', '#ced4da');
    //}
    $.ajax({
        url: "../ReporteMaestro/ReporteMaestro",
        type: "POST",
        data: {
            IdReporteMaestro: $("#txtIdControlModal").val(),
            Nombre: $("#txtNombreModal").val(),
            Codigo: $("#txtCodigoModal").val(),
            IdOpcion: $("#selectOpcionModal").val()
            //UltimaVersion: $("#txtVersionModal").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }
            //      NuevaReporteMaestro();
            $("#ModalEditarControl").modal("hide");
            MensajeCorrecto("Registro Exitoso");
            //ConsultarReporteMaestros();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError("Error, comuniquese con sistemas. " + result, false);
        }
    });
}

function InactivarControl() {
    $.ajax({
        url: "../ReporteMaestro/EliminarReporteMaestro",
        type: "POST",
        data: {
            IdReporteMaestro: ReporteModel.IdReporteMaestro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            Atras();
            //NuevoControl();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error, comuniquese con sistemas. " + resultado, false);
        }
    });
}


function EliminarControl() {
        $("#modalEliminarControl").modal('show');
}

$("#modal-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});



////////////////////////////////////////////////////////////DETALLE///////////////////////////////////////////////

function ConsultarReporteDetalle(Id) {

    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../ReporteMaestro/ReporteDetallePartial",
        type: "GET",
        data: {
            idControl: Id
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartDetalle").html("No existen registros");
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#chartDetalle").html(resultado);
                config.opcionesDT.pageLength = -1;
                config.opcionesDT.order = [];
                $('#tblDataTable2').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error, comuniquese con sistemas. " + resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function GenerarReporteDetalle() {
    NuevoDetalle();
   
}

function NuevoDetalle() {
    $("#txtVersionDetalleModal").val('');
    $("#file-upload").val('');
    $("#file-preview-zone").html('');
    $("#ModalControlDetalle").modal("show");
    $("#txtVersionDetalleModal").css('borderColor', '#ced4da');
    $("#divFileUpload").css('color', '#ced4da');
    $("#file-preview-zone").css('borderColor', '#ced4da');
    rotation = 0;

}

function ValidarDetalle() {
    //console.log($("#file-preview").val());
    var valida = true;
    if ($("#txtVersionDetalleModal").val() == "") {
        $("#txtVersionDetalleModal").css('borderColor', '#FA8072');
        valida=false;
    } else {
        $("#txtVersionDetalleModal").css('borderColor', '#ced4da');
    }
    if ($("#file-preview").val() == undefined) {
        $("#divFileUpload").css('color', '#FA8072');
        $("#file-preview-zone").css('borderColor', '#FA8072');
        
        valida=false;
    } else {
        $("#divFileUpload").css('color', '#ced4da');
        $("#file-preview-zone").css('borderColor', '#ced4da');

    }
    return valida;
}

function GuardarReporteDetalle() {

    if (!ValidarDetalle()) {
        return;
    }
   // console.log($("#file-upload").get(0).files[0]);


    //NuevoDetalle();
    var imagen = $('#file-upload')[0].files[0];
    var data = new FormData();
    data.append("dataImg", imagen);
    data.append("IdReporteMaestro", $("#txtIdControlModal").val());
    data.append("IdReporteDetalle", $("#txtIdDetalleModal").val());
    data.append("Version", $("#txtVersionDetalleModal").val());
    data.append("Rotacion", rotation);
    data.append("UltimaVersion", $("#CheckVersion").prop("checked"));
    //var files = $('#file-upload')[0].files[0]; 
    
    $.ajax({
        url: "../ReporteMaestro/ReporteDetalle",
        type: "POST",
        cache: false,
        data: data,
        contentType: false,
        processData: false,
        async:false,
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                MensajeAdvertencia("Faltan parametros");
                return;
            }
            //      NuevaReporteMaestro();
            $("#ModalEditarControl").modal("hide");
            ConsultarReporteDetalle(ReporteModel.IdReporteMaestro);
            $("#ModalControlDetalle").modal("hide");
             MensajeCorrecto("Registro Exitoso");
            //ConsultarReporteMaestros();
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError("Error, comuniquese con sistemas. " + result, false);
        }
    });
}

function EditarReporteDetalle(model) {
    NuevoDetalle();

    if (model.Version == $("#txtVersion").val()) {
        $("#CheckVersion").prop("checked", true);
    } else {
        $("#CheckVersion").prop("checked", false);
    }

    $("#txtIdDetalleModal").val(model.IdReporteDetalle);
    $("#txtVersionDetalleModal").val(model.Version);

    var filePreview = document.createElement('img');
    filePreview.id = 'file-preview';
    filePreview.src = "/Content/Img/" + model.Imagen;
    var previewZone = document.getElementById('file-preview-zone');
    previewZone.appendChild(filePreview);
 
    $("#file-preview").addClass("img");
    $('#file-preview').rotate(model.Rotacion);
    document.getElementById("file-preview").style.height = "0px";
    document.getElementById("file-preview").style.width = "0px";

    var img = new Image();
    img.onload = function () {
      //  alert(this.width + 'x' + this.height);
        var ancho = this.width;
        var alto = this.height;  
        if (ancho < alto) {
            document.getElementById("file-preview").style.height = "350px";
            document.getElementById("file-preview").style.width = "250px";
        } else {
            document.getElementById("file-preview").style.height = "250px";
            document.getElementById("file-preview").style.width = "350px";
        }
        $("#ModalControlDetalle").modal("show");

    }
    img.src = "/Content/Img/" + model.Imagen;

}

function InactivarControlDetalle(IdControlElimnar) {
    $.ajax({
        url: "../ReporteMaestro/EliminarReporteDetalle",
        type: "POST",
        data: {
            IdReporteDetalle: IdControlElimnar
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            ConsultarReporteDetalle(ReporteModel.IdReporteMaestro);
            //NuevoControl();
            $("#modalEliminarControlDetalle").modal("hide");
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError("Error, comuniquese con sistemas. " + resultado, false);
        }
    });
}

var IdControlElimnar = 0;
function EliminarControlDetalle(model) {
    IdControlElimnar = model.IdReporteDetalle;
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControlDetalle(IdControlElimnar);
    $("#modalEliminarControl").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});



function readFile(input) {


    if (input.files && input.files[0]) {
        var reader = new FileReader();
       
        reader.onload = function (e) {
           // console.log(this.width.toFixed(0));

            // alert('Imagen correcta :)')
            $("#file-preview-zone").html('');
            var filePreview = document.createElement('img');
            filePreview.id = 'file-preview';
           // filePreview.setAttribute("type", "hidden");

            //e.target.result contents the base64 data from the image uploaded
            filePreview.src = e.target.result;
            //console.log(e.target.result);
            var previewZone = document.getElementById('file-preview-zone');                
            previewZone.appendChild(filePreview);
            $("#file-preview").addClass("img");    
            document.getElementById("file-preview").style.height = "0px";
            document.getElementById("file-preview").style.width = "0px";
            //console.log(e.target.result);
            var image = new Image();
            image.src = e.target.result;
            image.onload = function () {
                if (this.width < this.height) {
                    document.getElementById("file-preview").style.height = "350px";
                    document.getElementById("file-preview").style.width = "250px";
                }
                else {
                    document.getElementById("file-preview").style.height = "250px";
                    document.getElementById("file-preview").style.width = "350px";
                }
               
            };      
        
        
         }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    readFile(e.srcElement);

}


$('#file-preview-zone').on("click", function (e) {
    rotation += 90;
    $('#file-preview').rotate(rotation);
    if (rotation == 360) {
        rotation = 0;
    }
});

