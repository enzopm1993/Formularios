$(document).ready(function () {

  
    $('#txtPlantilla').summernote({
        codeviewFilter: false,
        codeviewIframeFilter: true,
        dialogsInBody: false,
        lang: 'es-ES',
        height: 300,
        toolbar: [
             
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']]
        ]
    });
   
    //$('#txtPlantilla').summernote('pasteHTML', unescapeHtml( test));

    $(".asignaEtiqueta").click(function () {
        var tag = $(this).data('tag');
        $('#txtPlantilla').summernote('insertText', tag);
        return false;
    });

    $("#btnNuevo").click(function () {
        accionNuevo();
        return false;
    });

    $("#btnCancela").click(function () {
        accionCancelaGuarda("¿Está seguro de cancelar el proceso?");
        return false;
    });

    $("#btnGuardaCrea").click(function () {
        var desde = $(this).data("desde");
        var idCertificado = 0;
        if (desde == "I") {
            idCertificado = 0;
        } else {
            idCertificado = $("#idCertificado").val();
        }

        var descripcion = $.trim($("#txtDescripcionCert").val());
        var estado =false;
        var conPlantilla = false;
        if ($("#txtCheckPlantilla").is(":checked")) {
            conPlantilla = true;
        }
        if ($("#txtCheckEstado").is(":checked")) {
            estado = true;
        }
        var plantilla = $('#txtPlantilla').summernote('code');
        var username = $("#usernameLogin").val();
    
    
        if (confirm("¿Está seguro de guardar el certificado?")) {
            if (descripcion == "") {
                alert("La descripción es obligatoria")
            } else {
                if (conPlantilla == true && plantilla == "") {
                    alert("La plantilla es obligatoria si se ha marcado la casilla 'Con Plantilla', desactivar eesta casilla si no es necesaria")
                } else {
                    $(".certificadosLoadAjax").show();
                    $.ajax({
                       // dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        url: "../Nomina/ProcesaCertificado",
                        type: "POST",
                        data: JSON.stringify({
                            'Id': idCertificado,
                            'Descripcion': descripcion,
                            'Estado': estado,
                            'ConPlantilla': conPlantilla,
                            'Plantilla': plantilla,
                            'usuario': username,
                            'Opcion': desde
                        }),
                        success: function (resultado) {
                            $(".certificadosLoadAjax").hide();
                            console.log(resultado);
                            alert(resultado.Descripcion);
                            if (resultado.Codigo == "1") {                                
                                window.location.reload(false);
                            }
                        },
                        error: function (resultado) {
                            $(".certificadosLoadAjax").hide();
                            MensajeError(resultado.statusText, false);
                        }
                    });
                }
            }
        }
       
       
        return false;
    });

    $(".itemCertificado").click(function () {
        $(".itemCertificado").removeClass('active');
        $(this).addClass('active');
        $("#txtDescripcionCert").val('');
        $("#txtCheckPlantilla").prop('checked', false);
        $("#txtCheckEstado").prop('checked', true);
        $("#txtPlantilla").summernote("reset");
        $("#idCertificado").val("0");
        var idCertificado = $(this).data('id');
        $(".certificadosLoadAjax").show();
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../Nomina/ObtenerCertificado",
            type: "POST",
            data: JSON.stringify({
                'Id': idCertificado
            }),
            success: function (resultado) {
                $(".certificadosLoadAjax").hide();
                console.log(resultado);
                if (resultado.length > 0) {
                    accionNuevo();
                    $("#txtDescripcionCert").val(resultado[0].Descripcion);
                    $("#txtCheckPlantilla").prop('checked', resultado[0].ConPlantilla);
                    $("#txtCheckPlantilla").trigger("change");
                    $("#txtCheckEstado").prop('checked', resultado[0].Estado);
                    $("#txtPlantilla").summernote("code", resultado[0].Plantilla);
                    $("#idCertificado").val(resultado[0].Id);
                    $("#btnGuardaCrea").attr('data-desde', 'U');
                } 
               
            },
            error: function (resultado) {
                $(".certificadosLoadAjax").hide();
                MensajeError(resultado.statusText, false);
            }
        });
        return false;

    });

    $('#txtCheckPlantilla').change(function () {
        if ($(this).prop('checked')) {
            $("#bloquePlantilla").slideDown();            
        } else{
            $("#bloquePlantilla").slideUp();
        }
    })

    function accionNuevo() {
        $("#btnGuardaCrea").attr('data-desde', 'I');
        $("#btnNuevo").hide();
        $("#btnGuardaCrea").show();
        $("#btnCancela").show();
    }
    function accionCancelaGuarda(mensaje) {
        var resp = confirm(mensaje);
        if (resp) {
            $("#btnNuevo").show();
            $("#btnGuardaCrea").hide();
            $("#btnCancela").hide();
        }
        return resp
    }
   
});