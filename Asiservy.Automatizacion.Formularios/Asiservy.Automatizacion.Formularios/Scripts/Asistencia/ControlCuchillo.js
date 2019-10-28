



function SelectEstado(valor) {
    if (valor < 1 && $("#EstadoControlCuchillo").val() !="undefined")
        valor = $("#EstadoControlCuchillo").val();

    CargarControlCuchillo(valor);
}

function CargarControlCuchillo(estado) {
    if (estado < 1)
        return;

    if ($("#txtFecha").val() == '' || $("#txtFecha").val() == null) {

        MensajeAdvertencia("Ingrese una Fecha");
        return;
    }
    $('#TablaControlCuchillo').html('');
    $("#spinnerCargando").prop("hidden",false);
  $.ajax({
        url: "../Asistencia/ControlCuchilloPartial",
        type: "GET",
      data: {
          dsEstado: estado,
          ddFecha: $("#txtFecha").val()
      },
        success: function (resultado) {
            var bitacora = $('#TablaControlCuchillo');
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                bitacora.html("<div class='text-center'><h4>No se ha generado la asistencia</h4></div>");

            } else {
                bitacora.html(resultado);
            }

        },
        error: function (resultado) {
           
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);


        }
    });
}




function check(id, color, cedula) {
   // alert(id);
    //console.log(id);
    //console.log(color);
    //console.log(cedula);
    //7b8a8b
    var label = "#labelCuchillo";
    var numero = 0;
    var estado = $('#EstadoControlCuchillo').val();
    if (color == 'B') {
        label = label + "Blanco_";
        label = label + id.substring(9, 6);
        numero = id.substring(9, 6);
        console.log(numero);

    } if (color == 'R') {
        label = label + "Rojo_";
        label = label + id.substring(8, 4);
        numero = id.substring(8, 4);

    } if (color == 'N') {
        label = label + "Negro_";
        label = label + id.substring(9, 5);
        numero = id.substring(9, 5);

    }
    //console.log(label);
   // console.log(cedula);
    id = "#" + id;
    $(id).prop('disabled', true);
    if($(id).prop('checked')) {
        $(label).css("background", "#28B463");
        GuardarControlCuchillo(cedula, color, numero, estado, true, id, label);
    } else {
        $(label).css("background", "#7b8a8b");
        GuardarControlCuchillo(cedula, color, numero, estado, false, id, label);
    }
}



function GuardarControlCuchillo(cedula, color, numero, estado, check,idCheck,idLabel) {
    $.ajax({
        url: "../Asistencia/GuardarControlCuchillo",
        type: "GET",
        data: {
            dsCedula: cedula,
            dsColor: color,
            dsNumero: numero,
            dsEstado: estado,
            dbCheck: check,
            ddFecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado.codigo == 1) {
                MensajeAdvertencia(resultado.descripcion)
                $(idCheck).prop('checked', false);
                $(idLabel).css("background", "#7b8a8b");
               
            }
            $(idCheck).prop('disabled', false);
            
        },
        error: function (resultado) {
            //console.log(resultado.responseJSON);
            //console.log(resultado);
            MensajeError(resultado.responseText + "", false);
           // MensajeError(resultado + "", false);
            $(idCheck).prop('checked', false);
            $(idLabel).css("background", "#7b8a8b");
            $(idCheck).prop('disabled', false);

        }
    });
}
