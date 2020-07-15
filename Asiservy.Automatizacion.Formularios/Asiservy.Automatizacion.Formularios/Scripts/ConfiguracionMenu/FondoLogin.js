

$(document).ready(function () {
    CargarControl();
});


function CargarControl() {
    $("#spinnerCargando").prop("hidden", false);
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
                config.opcionesDT.pageLength = 5;
                config.opcionesDT.order = [[1, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#DivTableControl").html('');
            $("#spinnerCargando").prop("hidden", true);
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
                    $("file-upload").val("");
                    $("lblfoto").html("");

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

//var banderaTamano = false;
//function validarImagen() {
//    var o = document.getElementById('file-upload');
//    var foto = o.files[0];
//    var c = 0;
//    console.log(foto);

//    if (o.files.length == 0 || !(/\.(jpg|png)$/i).test(foto.name)) {
//        MensajeAdvertencia('Ingrese una imagen con alguno de los siguientes formatos: .jpeg/.jpg/.png.');
//        return false;
//    }

//    // Si el tamaño de la imagen fue validado
//    if (banderaTamano) {
//        return true;
//    }

//    var img = new Image();
//    img.onload = function dimension() {
//        if (this.width.toFixed(0) < 1200 || this.height.toFixed(0) < 800) {
//            MensajeAdvertencia('Las medidas deben ser: 1200 x 800');
//        } else {
//            //alert('Imagen correcta :)');
//            //// El tamaño de la imagen fue validado
//            banderaTamano = true;

//            //// Buscamos el formulario
//            //var form = document.getElementById('formulario');
//            //// Enviamos de nuevo el formulario con la bandera modificada.
//            //form.submit();

//            //return true;
//        }
//    };
//    img.src = URL.createObjectURL(foto);

//    // Devolvemos false porque falta validar el tamaño de la imagen
//    //return false;
//}

var fileUpload = document.getElementById('file-upload');
fileUpload.onchange = function (e) {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    readFile(e.srcElement);
}