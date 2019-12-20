$(document).ready(function () {
    var iconLoader = "fa-spinner fa-pulse";
    var iconJustifica = "fa-check-circle"

    $(".cargaDatos").click(function () {

       

        $(".cargaDatos").attr('href', "javascript:void(0)");
        var fecha = $(this).data("fecha");
        var marcacionFalta = $(this).data("marcacion");
        var cedula = $(this).data("cedula");
        var dataId = $(this).data("id");
        var nombrePersona = $(this).data("nombre");
        var fechaIni = moment(fecha, "DD/MM/YYYY").format("YYYY-MM-DD");
        var fechaFin = moment(fecha, "DD/MM/YYYY").format("YYYY-MM-DD");
        $("#cargaDatos_" + dataId).hide();
        $("#procesando_" + dataId).show();
        $("#tipoMarcacion").html(marcacionFalta);
        $("#listaTipoMarcacion").val(marcacionFalta);
        $("#cedulaUser").val(cedula);
        $("#idRegistro").val(dataId);
        $("#diaMarcacionFalta").html(fecha);
        $("#nombrePersonaJustifica").html(nombrePersona);
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: "../App/ObtieneLogMarcacion",
            type: "POST",
            data: JSON.stringify({
                'fechaIni': fechaIni,
                'fechaFin': fechaFin,
                'cedula':   cedula
            }),
            success: function (resultado) {

                $("#listLogMarcaciones").empty();
                if (resultado.length > 0) {
                    $.each(resultado, function (i, item) {                        
                        var newRowContent = '<li><label class="checkbox-inline"><input type="radio" name="optMarcacion" value="' + item.TIPO_MARCACION + '_' + item.HORA +'"> <span class="log_hora_marca">' + item.HORA + '</span> <i class="fas fa-arrow-alt-circle-right"></i> <span class="log_marcacion">' + item.TIPO_MARCACION +'</span></label> </li>';
                        $("#listLogMarcaciones").append(newRowContent);
                    });                   
                }  
                $("#listLogMarcaciones").append('<li><label class="checkbox-inline"><input type="radio" name="optMarcacion" value="NUEVA"> NUEVA</label></li>');
                $("#ModalLogMarcacion").modal({ backdrop: 'static', keyboard: false, show: true });
                $("#procesando_" + dataId).hide();
                $("#cargaDatos_" + dataId).show();
                $(".cargaDatos").attr('href', "#");
            },
            error: function (resultado) {
               
                MensajeError(resultado.statusText, false);
            }
        });
        
        return false;
    });

    $('#listLogMarcaciones').on("click", 'input[name="optMarcacion"]', function (e) {
      
        var valorSeleccionado = $(this).val();
        if (valorSeleccionado == "NUEVA") {
            $("#seccionNuevaMarcacion").addClass('muestraSeccion');
        } else {
            $("#seccionNuevaMarcacion").removeClass('muestraSeccion');           
        }
    });

    $("#justificarMarcacion").click(function () {
        $("#justificarIcon").removeClass(iconJustifica);
        $("#justificarIcon").addClass(iconLoader);
        var opcionSelccionada = $("input[name='optMarcacion']:checked").val();        
        if (opcionSelccionada == undefined) {
            alert("Seleccione una opción entre las marcaciones, o una nueva maración");
        } else {
            $("#justificarMarcacion").attr("href", "javascript:void(0)");
            var tipoMarcacion = $("#tipoMarcacion").html();
            var diaMarcacion = $("#diaMarcacionFalta").html();
            var cedula = $("#cedulaUser").val();            
            var idRegistro = $("#idRegistro").val();
            var horaMarcacionCorrecta = "";
            if (opcionSelccionada == "NUEVA") {
                horaMarcacionCorrecta = $("#hora_marcacion_nueva").val();
            } else {
                var arr = opcionSelccionada.split("_");
                horaMarcacionCorrecta = arr[1];
            }

            var objEnnvia = {
                'tipoMarcacion': tipoMarcacion,
                'diaMarcacion': diaMarcacion,
                'cedula': cedula,
                'idRegistro': idRegistro,
                'horaMarcacionCorrecta': horaMarcacionCorrecta,
                'usuarioActualiza': $("#usernameLogin").val()
            }
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: "../App/EnviarMarcacion",
                type: "POST",
                data: JSON.stringify(objEnnvia),
                success: function (resultado) {
                    alert(resultado.Descripcion);
                    if (resultado.Codigo == "1") {
                        window.location.reload(false);
                    } else {
                        $("#justificarIcon").removeClass(iconLoader);
                        $("#justificarIcon").addClass(iconJustifica);
                        $("#justificarMarcacion").attr("href", "#");
                    }
                },
                error: function (resultado) {
                    $("#justificarIcon").removeClass(iconLoader);
                    $("#justificarIcon").addClass(iconJustifica);
                    $("#justificarMarcacion").attr("href", "#");
                    MensajeError(resultado.statusText, false);
                }
            });

        }
        return false;
    });
});