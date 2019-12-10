
$(document).ready(function () {
    $('#txtContenido').summernote({
        codeviewFilter: false,
        codeviewIframeFilter: true,
        dialogsInBody: false,
        lang: 'es-ES',
        height: 300
    });


    $('#boxFechaDesdeHasta').daterangepicker({
        "autoApply": true,

        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Apply",
            "cancelLabel": "Cancel",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, function (start, end, label) {
        $("#fechaDesde").val(start.format('YYYY-MM-DD'));
        $("#fechaHasta").val(end.format('YYYY-MM-DD'));
    });



    $("#nuevoComunicado").click(function () {
        vaciarCampos();
        $("#tituloFormComunicado").html("Nuevo comunicado");
        $("#tipoProceso").val("I");
        $("#idComunicado").val("0");
        $("#publicarComunicado").text("Publicar");
        $("#box_visualizar").hide();
        $("#box_editar").show();
        $("#ModalComunicado").modal({ backdrop: 'static', keyboard: false, show: true });
        return false;
    });

    $(".cargaDatos").click(function () {
        vaciarCampos();
        $("#box_visualizar").show();
        $("#box_editar").hide();

      

        var view_fechadesde = $(this).data("fechadesde");
        var view_fechahasta = $(this).data("fechahasta");
        var view_estadopublicacion = $(this).data("estadopublicacion");
        var view_estadoregistro = $(this).data("estadoregistro");
        var view_categorianombre = $(this).data("categorianombre");
        var view_categoriaid = $(this).data("categoriaid");
        var view_prioridad = $(this).data("prioridad");
        var view_titulo = $(this).data("titulo");
        var view_id = $(this).data("id"); 
        var comentario = $("#contenido_" + view_id).html();

        $("#tituloComunicado").html(view_titulo);
        $("#contenididoComunicado").html(comentario);

        $("#viewEstadoPublica").text(view_estadopublicacion);
        $("#viewDesde").text(view_fechadesde);
        $("#viewHasta").text(view_fechahasta);
        $("#viewCategoria").text(view_categorianombre);
        $("#viewPrioridad").text(view_prioridad);       

        if (view_estadoregistro == "True") {
            $("#viewEstadoRegistro").text("Activo");
            $("#txtCheckEstado").prop('checked', true);
        } else {
            $("#viewEstadoRegistro").text("Inactivo");
            $("#txtCheckEstado").prop('checked', false);
        }
      
        $("#tituloFormComunicado").html("Editar comunicado");
        $("#txtTituloComunicado").val(view_titulo);

        var date1 = moment(view_fechadesde, 'DD/MM/YYYY').format('YYYY-MM-DD');
        var date2 = moment(view_fechahasta, 'DD/MM/YYYY').format('YYYY-MM-DD');

       
        $('#boxFechaDesdeHasta').data('daterangepicker').setStartDate(moment(date1).format('DD/MM/YYYY'));
        $('#boxFechaDesdeHasta').data('daterangepicker').setEndDate(moment(date2).format('DD/MM/YYYY'));

        $("#fechaDesde").val(date1);
        $("#fechaHasta").val(date2);
        $("#txtCategoria").val(view_categoriaid);
        $("#txtPrioridad").val(view_prioridad);
        $("#idComunicado").val(view_id);

        $("#txtContenido").summernote("code", comentario);

        $("#ModalComunicado").modal({ backdrop: 'static', keyboard: false, show: true });

        return false;
    });

    $("#btnEditar").click(function () {

        $("#tipoProceso").val("U");  
        $("#publicarComunicado").text("Guardar");
        $("#box_visualizar").hide();
        $("#box_editar").slideDown();
        return false;
    });

    $("#addNewCat").click(function () {
        $("#addNewCat").hide();
        $("#txtCategoria").hide();
        $("#cancelNewCat").show();
        $("#txtCategoria").val("0");
        $("#nombre_cat_nueva").show();
        $("#es_nueva_cat").val("1");
        $("#nombre_cat_nueva").focus();
        return false;
    });

    $("#cancelNewCat").click(function () {
        $("#addNewCat").show();
        $("#txtCategoria").show();
        $("#cancelNewCat").hide();
        $("#txtCategoria").val("0");
        $("#nombre_cat_nueva").hide();
        $("#es_nueva_cat").val("0");
        return false;
    });

    $("#cierraModalForm").click(function () {
        var tipoProceso = $("#tipoProceso").val();
        if (tipoProceso == "I") {
            $("#ModalComunicado").modal('toggle');
        } else {
            $("#tipoProceso").val("I");
            $("#box_editar").hide();
            $("#box_visualizar").slideDown();
        }
        return false;
    });

    $("#publicarComunicado").click(function () {
        var estado = false;      
        if ($("#txtCheckEstado").is(":checked")) {
            estado = true;
        }
        var es_nueva_cat = false;
        if ($("#es_nueva_cat").val() == "1") {
            es_nueva_cat = true;
        }
        
        var tipoProceso = $("#tipoProceso").val();
        var idComunicado = $("#idComunicado").val();
        var titulo = $.trim($("#txtTituloComunicado").val());
        var fechaDesde = $("#fechaDesde").val();
        var fechaHasta = $("#fechaHasta").val();
        var idCategoria = $("#txtCategoria").val();
        var prioridad = $("#txtPrioridad").val();
        var contenido = $('#txtContenido').summernote('code');
        var nombre_nueva_cat = $.trim($("#nombre_cat_nueva").val()); 
        var usuario = $("#usernameLogin").val();

  
        if (validaCampos(titulo, fechaDesde, fechaHasta, idCategoria, es_nueva_cat, nombre_nueva_cat, tipoProceso, idComunicado)) {  
       
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: "../App/ProcesaComunicado",
                type: "POST",
                data: JSON.stringify({
                    'tipoProceso': tipoProceso,
                    'idComunicado': idComunicado,
                    'titulo': titulo,
                    'fechaDesde': fechaDesde,
                    'fechaHasta': fechaHasta,
                    'idCategoria': idCategoria,
                    'prioridad': prioridad,
                    'estado': estado,
                    'contenido': contenido,
                    'es_nueva_cat': es_nueva_cat,
                    'nombre_nueva_cat': nombre_nueva_cat,
                    'usuario': usuario
                }),
                success: function (resultado) {
                    if (resultado.Codigo == "1") {
                        if (tipoProceso == "I") {
                            alert("Registro creado con éxito");
                        } else {
                            alert("Registro actualizado con éxito");
                        }
                        
                        window.location.reload(false);
                    } else {
                        alert(resultado.Descripcion);
                    }
                },
                error: function (resultado) {
                    MensajeError(resultado.statusText, false);
                }
            });
        }
        return false;
    });

    function validaCampos(titulo, fechaDesde, fechaHasta, idCategoria, es_nueva_cat, nombre_nueva_cat, tipoProceso, idComunicado) {
        var resp = false;
        if (tipoProceso == "U" && idComunicado == "0") {
            alert("No se está enviando el código del comunicado");
        } else {
            if (titulo == "") {
                alert("El título del comunicado es obligatorio");
            } else {
                if (fechaDesde == "" || fechaHasta == "") {
                    alert("Seleccione un rango de fechas para la publicación");
                } else {
                    if (es_nueva_cat == "1") {
                        if (nombre_nueva_cat == "") {
                            alert("Ingrese un nombre para la nueva categoría");
                        } else {
                            resp = true;
                        }
                    } else {
                        if (idCategoria == "0") {
                            alert("Seleccione una categoría del comunicado");
                        } else {
                            resp = true;
                        }
                    }
                }
            }
        }
        
        return resp;
    }

    function vaciarCampos() {
        $("#tipoProceso").val("I");
        $("#idComunicado").val("0");
        $("#txtTituloComunicado").val("");
        $("#fechaDesde").val("");
        $("#fechaHasta").val("");
        $("#txtCategoria").val("0");
        $("#txtPrioridad").val("0");
        $('#txtContenido').summernote('reset');
        $("#nombre_cat_nueva").val("");
    }
    
});
