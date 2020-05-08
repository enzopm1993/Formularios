$(document).ready(function () {


    $('#txtPlantilla').summernote({
        codeviewFilter: false,
        codeviewIframeFilter: true,
        dialogsInBody: false,
        lang: 'es-ES',
        height: 800,
        toolbar: [

            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']]
        ]
    });

    $(".asignaEtiqueta").click(function () {
        var tag = $(this).data('tag');
        $("#modalTags").modal('hide');
        $('#txtPlantilla').summernote('insertText', tag);

        return false;
    });

    $("#btnGuardar").click(function () {
        var id = $("#idMemo").val();
        var titulo = $("#txtTituloMemo").val();
        var estado = $("#estado").prop('checked');
        var plantilla = $('#txtPlantilla').summernote('code');

        var respConfirm = false;
        if (id > 0) {
            respConfirm = confirm("¿Está seguro de actualizar este memorandum?");   
        } else {
            respConfirm = true;
        }
        if (respConfirm) {
            if (titulo == "") {
                alert("El título del formato es obligatorio para poderlo distinguir")
            } else {
                if (plantilla == "") {
                    alert("La plantilla es obligatoria")
                } else {
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        url: "../Memorandum/ProcesaMemorandum",
                        type: "POST",
                        data: JSON.stringify({
                            'Id': id,
                            'Titulo': titulo,
                            'Estado': estado,
                            'Plantilla': plantilla,
                        }),
                        success: function (resultado) {
                            alert(resultado.Descripcion);
                            if (parseInt(resultado.Codigo) > 0) {
                                if (id > 0) {
                                    window.location.reload(false);                                   
                                } else {
                                    window.location.href = "../Memorandum/Plantillas";
                                }
                               
                            } 
                        },
                        error: function (resultado) {
                            MensajeError(resultado.statusText, false);
                        }
                    });
                }
            }
           
        }
        return false;
    });
});