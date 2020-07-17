var Model = [];

$(document).ready(function () {
    CargarControl();
});


function CargarControl() {
    MostrarModalCargando();
    $("#DivTableControl").html('');
    $.ajax({
        url: "../ConfiguracionMenu/FondoLoginPartial",
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#DivTableControl").html('');
                $("#DivTableControl").html("No existen registros");
            } else {
                $("#DivTableControl").html('');
                $("#DivTableControl").html(resultado);
               
            }
            CerrarModalCargando();

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            CerrarModalCargando();

        }
    });
}

function MostrarModal() {
    Model = [];
    //console.log(Model);

    $("#txtDescripcion").val('');
    $("#file-upload").val('');
    $("#ModalLoginFondo").modal("show");
    $("#lblfoto").html("Seleccione fondo");
    $("#file-preview-zone").html('');
    $("#txtDescripcion").css('borderColor', '#ced4da');
    $("#lblfoto").css('borderColor', '#ced4da');
}

function Validar() {
    var valida = true;
    if ($("#txtDescripcion").val() == "") {
        $("#txtDescripcion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtDescripcion").css('borderColor', '#ced4da');
    }

    //console.log(Model.IdFondoLogin);
    if (Model.IdFondoLogin == null) {
        if ($('#file-upload')[0].files[0] == null) {
            $("#lblfoto").css('borderColor', '#FA8072');
            valida = false;
        } else {
            $("#lblfoto").css('borderColor', '#ced4da');
        }
    }


    return valida;
}

function GuardarFondoLogin() {
    if (!Validar()) {
        return;
    }
    var imagen = $('#file-upload')[0].files[0];
    var data = new FormData();
    data.append("dataImg", imagen);
    data.append("Descripcion", $("#txtDescripcion").val());
    data.append("IdFondoLogin", Model.IdFondoLogin);
    data.append("Vigente", $("#chkVigente").prop("checked"));
    MostrarModalCargando()
    $.ajax({
        url: "../ConfiguracionMenu/FondoLogin",
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
            CargarControl();
            MensajeCorrecto(resultado);
            $("#ModalLoginFondo").modal("hide");

        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado.responseText, false);
            $("#spinnerCargandoDetalle2").prop("hidden", true);
            CerrarModalCargando();
        }
    });
}




function EditarControl(model) {
    Model = model;
    //onsole.log(model);
    $("#file-preview-zone").html('');
    $("#txtDescripcion").val(Model.Descripcion);
    if (Model.Imagen != null && Model.Imagen != '') {
        var filePreview = document.createElement('img');
        filePreview.id = 'file-preview';
        filePreview.src = "../ImagenSiaa/" + Model.Imagen;
        var previewZone = document.getElementById('file-preview-zone');
        previewZone.appendChild(filePreview);
        $("#file-preview").addClass("img");
      
        var img = new Image();
        img.onload = function () {
            document.getElementById("file-preview").style.height = "250px";
            document.getElementById("file-preview").style.width = "350px";
            $("#ModalLoginFondo").modal("show");
        }
        img.src = "../ImagenSiaa/" + Model.Imagen;

    } else {
        $("#ModalLoginFondo").modal("show");
    }

    //ModalGenerarControlDetalle();
}


function ActivarControl(m) {
    $.ajax({
        url: "../ConfiguracionMenu/ActivarFondoLogin",
        type: "POST",
        data: {
            IdFondoLogin: m.IdFondoLogin
            },
        success: function (resultado) {
            //  $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            }
            CargarControl();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(Mensajes.Error + resultado.responseText, false);
        }
    });
}



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
                //console.log(this.width);
                //console.log(this.height);
                if (this.width < 1200 || this.height < 800) {
                    MensajeAdvertencia('Las medidas deben ser: 1200 x 800');
                    $("#file-preview-zone").html('');
                    $("#file-upload").val("");
                    $("#lblfoto").html("Seleccione fondo");
                    
                    

                    return;
                }
                //if (this.width < this.height) {
                document.getElementById("file-preview").style.height = "250px";
                document.getElementById("file-preview").style.width = "350px";
                //}
                //else {
                //    document.getElementById("file-preview").style.height = "250px";
                //    document.getElementById("file-preview").style.width = "350px";
                //}

            };


        }
        reader.readAsDataURL(input.files[0]);
    }
}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
  //  console.log(validarImagen());
        readFile(e.srcElement);
}


var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);
}