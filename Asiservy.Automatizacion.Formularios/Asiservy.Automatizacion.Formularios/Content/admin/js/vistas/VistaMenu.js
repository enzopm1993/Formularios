(function ($) {
    "use strict"; // Start of use strict

    var iconoControl = ' <i class="fas fa-cog"></i>';
    function cargarModulos() {
        $("#modulosLoadAjax").show();
        $.ajax({
            url: config.baseUrl + config.ajaxurl + "cargarModulos",
            dataType: "json",
            data: {},
            success: function (response) {
                $("#resultsModulos").empty();
                $.each(response, function (index, value) {
                    var link = $("<a>");

                    link.attr("href", "#");
                    link.attr("id", 'modulo_'+value.Codigo);
                    link.attr("data-id", value.Codigo);
                    link.attr("data-controlador", value.Controlador);
                    link.attr("data-font", value.Icono);
                    link.text(value.Descripcion);
                    link.addClass("list-group-item"); 
                    link.addClass("list-group-item-action"); 
                    link.addClass("buscarOpciones"); 

                    var div = $("<div>");
                    div.addClass("list-group"); 
                    div.addClass("listaRelative"); 
                    
                    div.html(link);
                    //
                 

                    $("#resultsModulos").append(div);
                });
                $("#modulosLoadAjax").hide();
            },
            error: function (error) {
                $("#modulosLoadAjax").hide();
            }
        });
    }


    $("#resultsModulos").on('click', 'a.buscarOpciones', function (e) {
        $("#opcionesLoadAjax").show();
        $(".buscarOpciones").removeClass('active');
        $(this).addClass('active');
        var modiloId = $(this).attr('data-id');
        $(".btn-editar-modulo").removeClass('disabled');
        $.ajax({
            url: config.baseUrl + config.ajaxurl + "cargarOpciones",
            dataType: "json",
            data: { id: modiloId},
            success: function (response) {

                $("#resultsOpciones").empty();
                $.each(response, function (index, value) {
                    var link = $("<a>");
                    link.attr("href", "#");
                    link.attr("data-id", value.Codigo);
                    link.attr("data-controlador", value.Controlador);
                    link.attr("data-font", value.Icono);
                    link.text(value.Descripcion);
                    link.addClass("list-group-item");
                    link.addClass("list-group-item-action");

                    var div = $("<div>");
                    div.addClass("list-group");
                    div.addClass("listaRelative"); 

                    div.html(link);

                    var linkOpen = $("<a>");
                    linkOpen.attr("href", "#");
                    linkOpen.attr("data-toggle", "modal");
                    linkOpen.attr("data-target", "#modalOpcion");
                    linkOpen.addClass("controles");
                    linkOpen.html(iconoControl);

                    div.append(linkOpen);

                    $("#resultsOpciones").append(div);
                });
                $("#opcionesLoadAjax").hide();
            },
            error: function (error) {
                console.log(error);
                $("#opcionesLoadAjax").hide();
            }
        });
        return false;
    });


    $(".btn-editar-modulo").click(function () {
        $("#tituloModulo").text("Editar módulo");
        $(".btnGuardar").attr("data-desde", "EDITAR");
        $('#modalmodulos').modal('show');
    });
    
    $(".btn-nuevo-modulo").click(function () {
        $("#tituloModulo").text("Agregar módulo");
        $(".btnGuardar").attr("data-desde","NUEVO");
        $('#modalmodulos').modal('show');
    });

    function vaciarCamposModulo() {

    }

    cargarModulos();

})(jQuery); // End of use strict

