var Model = [];
var editar = false;
$(document).ready(function () {
    ConsultarControl();
    $("#txtFerroso").mask("9?.99");
    $("#txtNoFerroso").mask("9?.99");
    $("#txtAceroInoxidable").mask("9?.99");
});

var rotation = 0;
    

function ConsultarControl() {
    MostrarModalCargando();
    $("#divMensaje").html('');
    $("#divDetalle").prop("hidden", true);
    $("#divDetalle2").prop("hidden", true);
    $("#btnGenerar").prop("disabled", false);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);
    $("#lblAprobadoPendiente").html('');
    $("#divCabeceras").html('');
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            //if (resultado == "1") {
            //    $("#txtPcc").val('');
            //    $("#txtIdControl").val('0');
            //    $("#chkLomo").prop("checked", false);
            //    $("#chkLata").prop("checked", false);
            //    $("#txtFerroso").val('');
            //    $("#txtNoFerroso").val('');
            //    $("#txtAceroInoxidable").val('');
            //    $("#txtCodDetectorMetal").val('');
            //    $("#txtObservacion").val('');
            //    $("#btnGenerar").prop("disabled", true);
            //    $("#lblAprobadoPendiente").removeClass("badge-danger").addClass("badge-info");
            //    $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
            //    //$("#divMensaje").html("<h3 class='text-info'>CONTROL SE ENCUENTRA APROBADO</h3>");
            //} else
            if (resultado == "0") {               
                $("#txtPcc").val('');
                $("#txtIdControl").val('0');
                $("#chkLomo").prop("checked", false);
                $("#chkLata").prop("checked", false);
                $("#txtFerroso").val('');
                $("#txtNoFerroso").val('');
                $("#txtAceroInoxidable").val('');
                $("#txtCodDetectorMetal").val('');
                $("#txtObservacion").val('');
                $("#divMensaje").html('<div class="text-center"><h4 class="text-warning">' + Mensajes.SinRegistros + '</h4></div>');
            } else {

                $("#divCabeceras").html(resultado);
            }
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
            CerrarModalCargando();
        }
    });
}

function SeleccionarControl(resultado) {
  
    if (resultado.EstadoReporte) {
        MensajeAdvertencia("Control ha sido aprobado no puede modificarse");
    } else {
        $("#btnAtras").prop("hidden", false);
        $("#btnConsultar").prop("hidden", true);
        Model = resultado;
        $("#divCabeceras").prop("hidden", true);
        $("#divDetalle").prop("hidden", false);
        $("#divDetalle2").prop("hidden", false);
        $("#btnEditar").prop("hidden", false);
        $("#btnEliminar").prop("hidden", false);
        $("#btnGenerar").prop("disabled", true);
        $("#lblAprobadoPendiente").removeClass("badge-info").addClass("badge-danger");
        $("#lblAprobadoPendiente").html(Mensajes.Pendiente);

        CargarControlDetalle();
        CargarControlDetalle2();
    }  

}

function Atras() {
    $("#btnAtras").prop("hidden", true);
    $("#btnConsultar").prop("hidden", false);
    Model = [];
    $("#divCabeceras").prop("hidden", false);
    ConsultarControl();
}

function AbrirModal() {
    editar = false;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
        $("#ModalCabecera").modal("show");
    }
}

function EditarModal() {
    editar = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
          $("#txtPcc").prop("disabled", false);
        $("#txtCodDetectorMetal").prop("disabled", false);
        $("#chkLomo").prop("disabled", false);
        $("#chkLata").prop("disabled", false);
        $("#txtPcc").val(Model.Pcc);
        $("#chkLomo").prop("checked", Model.Lomos);
        $("#chkLata").prop("checked", Model.Latas);
        $("#txtFerroso").val(Model.Ferroso);
        $("#txtNoFerroso").val(Model.NoFerroso);
        $("#txtAceroInoxidable").val(Model.AceroInoxidable);
        $("#txtCodDetectorMetal").val(Model.DetectorMetal);
        $("#txtObservacion").val(Model.Observacion);
        $("#ModalCabecera").modal("show");
    }
}


function Validar() {
    var valida = true;
    if ($("#txtPcc").val() == "") {
        $("#txtPcc").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtPcc").css('borderColor', '#ced4da');
    }
   
    if ($("#txtFerroso").val() == "") {
        $("#txtFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtNoFerroso").val() == "") {
        $("#txtNoFerroso").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNoFerroso").css('borderColor', '#ced4da');
    }

    if ($("#txtAceroInoxidable").val() == "") {
        $("#txtAceroInoxidable").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAceroInoxidable").css('borderColor', '#ced4da');
    }


    if ($("#txtCodDetectorMetal").val() == "") {
        $("#txtCodDetectorMetal").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodDetectorMetal").css('borderColor', '#ced4da');
    }
    return valida;
}
function GenerarControl() {
    if (!Validar()) {
        return;
    }
    //alert("ok");
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetal",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            IdOperatividadMetal: Model.IdOperatividadMetal,
            Pcc: $("#txtPcc").val(),
            Lomos: $("#chkLomo").prop("checked"),
            Latas: $("#chkLata").prop("checked"),
            Ferroso: $("#txtFerroso").val(),
            NoFerroso: $("#txtNoFerroso").val(),
            AceroInoxidable: $("#txtAceroInoxidable").val(),
            DetectorMetal: $("#txtCodDetectorMetal").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "1") {
                $("#ModalCabecera").modal("hide");
                MensajeCorrecto("Control ya se encuentra Aprobado.");
                return;
            } else {
                $("#ModalCabecera").modal("hide");
                MensajeCorrecto("Registro Exitoso.");
                if (!editar) { ConsultarControl(); }
            }

        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado, false);
        }
    });
}

function InactivarControl() {
    $.ajax({
        url: "../OperatividadMetal/EliminarOperatividadMetal",
        type: "POST",
        data: {
            IdOperatividadMetal: Model.IdOperatividadMetal
        },
        success: function (resultado) {
          //  alert(resultado);
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            Atras();
            MensajeCorrecto("Control Eliminado con Exito");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error+resultado.responseText, false);
        }
    });
}

function EliminarControl() {
   // $("#txtEliminarDetalle").val(model.IdControlConsumoInsumoDetalle);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControl").modal('show');
}

$("#modal-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});


////////////////////////////////////////////// --DETALLE-- ////////////////////////////////////////////7
function CargarControlDetalle() {
    $("#divTableDetalle").html('');
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalDetallePartial",
        type: "GET",
        data: {
            IdControl: Model.IdOperatividadMetal
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle").html(Mensajes.SinRegistros);
                $("#spinnerCargandoDetalle").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle").prop("hidden", true);
                $("#divTableDetalle").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error+resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function ModalGenerarControlDetalle() {
    $("#txtIdControlDetalle").val(0);
    $("#txtHora").val(moment().format("YYYY-MM-DDTHH:mm"));
    //$("#txtHoraFinDetalle").val("");
    $("#ModalGenerarControlDetalle").modal("show");

}

function ValidarGenerarControlDetalle() {
    var valida = true;
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    if (!$("#chkFerroso").prop("checked") || !$("#chkNoFerroso").prop("checked") || !$("#chkAceroInoxidable").prop("checked") ) {
        if ($("#txtObservacionDetalle").val() == "") {
            $("#txtObservacionDetalle").css('borderColor', '#FA8072');
            valida = false;
        } else {
            $("#txtObservacionDetalle").css('borderColor', '#ced4da');
        }
    } else {
        $("#txtObservacionDetalle").css('borderColor', '#ced4da');
    }


    return valida;
}
function GenerarControlDetalle() {
    if (!ValidarGenerarControlDetalle()) {
        return;
    }

    if (moment($("#txtHora").val()) < moment(Model.Fecha)) {
        MensajeAdvertencia("Hora no puede ser menor a la fecha del control");
        return;
    }
    if (moment($("#txtHora").val()) > moment(Model.Fecha).add(1, 'days')) {
        MensajeAdvertencia("Hora no puede ser mayor a la fecha del control");
        return;
    }


 //   $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalDetalle",
        type: "POST",
        data: {
            IdOperatividadMetalDetalle: $("#txtIdControlDetalle").val(),
            IdOperatividadMetal: Model.IdOperatividadMetal,
            Hora: $("#txtHora").val(),
            Ferroso: $("#chkFerroso").prop("checked"),
            NoFerroso: $("#chkNoFerroso").prop("checked"),
            AceroInoxidable: $("#chkAceroInoxidable").prop("checked"),
            Observacion: $("#txtObservacionDetalle").val()
        },
        success: function (resultado) {
          //  $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            CargarControlDetalle();
            MensajeCorrecto(resultado);
            $("#ModalGenerarControlDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error+ resultado.responseText, false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}


function EditarConsumoInsumoDetalle(model) {
    // console.log(model);
    $("#txtIdControlDetalle").val(model.IdOperatividadMetalDetalle);    
    $("#txtHora").val(model.Hora);
    $("#chkFerroso").prop("checked", model.Ferroso);
    $("#chkNoFerroso").prop("checked", model.NoFerroso);
    $("#chkAceroInoxidable").prop("checked", model.AceroInoxidable);
    $("#txtObservacionDetalle").val(model.Observacion);


    $("#ModalGenerarControlDetalle").modal("show");
    //ModalGenerarControlDetalle();
}

function InactivarControlDetalle() {
    $.ajax({
        url: "../OperatividadMetal/EliminarOperatividadMetalDetalle",
        type: "POST",
        data: {
            IdOperatividadMetalDetalle: $("#txtEliminarDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarControlDetalle();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error +resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControlDetalle(model) {
    $("#txtEliminarDetalle").val(model.IdOperatividadMetalDetalle);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#labelMensaje").html("HORA: "+moment(model.Hora).format("HH:mm"));
  $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarControlDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});





///////////////////////////////////////// DETECCION DE MENTALES /////////////////////////////////////////////////////
function CargarControlDetalle2() {
    $("#divTableDetalle2").html('');
    $("#spinnerCargandoDetalle2").prop("hidden", false);
    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalDetectorPartial",
        type: "GET",
        data: {
            IdControl: Model.IdOperatividadMetal
            //  Tipo: $("#txtLineaNegocio").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divTableDetalle2").html(Mensajes.SinRegistros);
                $("#spinnerCargandoDetalle2").prop("hidden", true);
            } else {
                $("#spinnerCargandoDetalle2").prop("hidden", true);
                $("#divTableDetalle2").html(resultado);
                //config.opcionesDT.pageLength = 10;
                //      config.opcionesDT.order = [[0, "asc"]];
                //    $('#tblDataTable').DataTable(config.opcionesDT);
            }
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error +resultado.responseText, false);
            $("#spinnerCargandoDetalle2").prop("hidden", true);
        }
    });
}

function NuevoControlDetalle2() {
    $("#txtIdControlDetalle2").val("");
    $("#txtNovedad").val("");
    $("#file-upload").val(''); 
    $("#lblfoto").html(''); 
    $("#file-preview-zone").html('');
    rotation = 0;
}

function ModalGenerarControlDetalle2() {
    NuevoControlDetalle2();
    $("#ModalGenerarControlDetalle2").modal("show");

}

function ValidarGenerarControlDetalle2() {
    var valida = true;
    if ($("#txtNovedad").val() == "") {
        $("#txtNovedad").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtNovedad").css('borderColor', '#ced4da');
    }



    return valida;
}
function GenerarControlDetalle2() {

    if (!ValidarGenerarControlDetalle2()) {
        return;
    }
    //   $("#spinnerCargandoDetalle").prop("hidden", false);
    var imagen = $('#file-upload')[0].files[0];
    var data = new FormData();
    data.append("dataImg", imagen);
    data.append("IdOperatividadDetectorMetal", $("#txtIdControlDetalle2").val());
    data.append("IdOperatividadMetal", Model.IdOperatividadMetal);
    data.append("Novedad", $("#txtNovedad").val());
    data.append("Rotacion", rotation);

    $.ajax({
        url: "../OperatividadMetal/OperatividadMetalDetector",
        type: "POST",
        cache: false,
        data: data,
        contentType: false,
        processData: false,
        async: false,
        data: data,
        success: function (resultado) {
            //  $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            CargarControlDetalle2();
            MensajeCorrecto(resultado);
            $("#ModalGenerarControlDetalle2").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error +resultado.responseText, false);
            $("#spinnerCargandoDetalle2").prop("hidden", true);
        }
    });
}


function EditarConsumoInsumoDetalle2(id, novedad, imagen, Rotacion) {
    //console.log(Rotacion);
    //console.log(imagen);
    NuevoControlDetalle2();
    $("#txtIdControlDetalle2").val(id);
    $("#txtNovedad").val(novedad);
    if (imagen != null && imagen != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = "../ImagenSiaa/" + imagen;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);

        $("#file-preview").addClass("img");
        $('#file-preview').rotate(parseInt(Rotacion));
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
            $("#ModalGenerarControlDetalle2").modal("show");

        }
        img.src = "../ImagenSiaa/" + imagen;

    } else {
        $("#ModalGenerarControlDetalle2").modal("show");
    }
   
    //ModalGenerarControlDetalle();
}

function InactivarControlDetalle2() {
    $.ajax({
        url: "../OperatividadMetal/EliminarOperatividadMetalDetector",
        type: "POST",
        data: {
            IdOperatividadDetectorMetal: $("#txtEliminarDetalle2").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarControlDetalle2();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado.responseText, false);
         //   $('#btnConsultar').prop("disabled", false);
            $("#modalEliminarControl").modal("hide");
           // $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControlDetalle2(id,novedad) {
    $("#txtEliminarDetalle2").val(id);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#labelMensaje2").html("NOVEDAD: " + novedad);
    $("#modalEliminarControlDetalle2").modal('show');
}

$("#modal-detalle2-si").on("click", function () {
    InactivarControlDetalle2();
    $("#modalEliminarControlDetalle2").modal('hide');
});

$("#modal-detalle2-no").on("click", function () {
    $("#modalEliminarControlDetalle2").modal('hide');
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

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);



}