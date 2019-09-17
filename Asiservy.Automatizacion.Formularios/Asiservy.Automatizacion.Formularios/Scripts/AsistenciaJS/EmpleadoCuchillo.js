

$(document).ready(function () {
    CargarEmpleadoCuchillo();
     Nuevo();
});

function Nuevo() {
    $('#IdEmpleadoCuchillo').val("0");
    $("#SelectEmpleado").prop('selectedIndex', 0);
    $('#SelectCuchilloBlanco').prop('selectedIndex', 0);
    $('#SelectCuchilloRojo').prop('selectedIndex', 0);
    $('#SelectCuchilloNegro').prop('selectedIndex', 0);
}

function SeleccionEmpleadoCuchillo(id, cedula, blanco,rojo, negro, estado) {
    
    $('#IdEmpleadoCuchillo').val(id);
    $('#SelectEmpleado').val(cedula);
    $('#SelectCuchilloBlanco').val(blanco);
    $('#SelectCuchilloRojo').val(rojo);
    $('#SelectCuchilloNegro').val(negro);
   
    if (estado == 'A') {
        $('#CheckEstadoRegistro').prop('checked', true);
        // console.log($('#LabelEstado').val());
        $('#LabelEstado').text('Activo');
    }
    else {
        $('#CheckEstadoRegistro').prop('checked', false);
        $('#LabelEstado').text('Inactivo');
    }
}


function CargarEmpleadoCuchillo() {
    $.ajax({
        url: "../Asistencia/CuchilloEmpleadoPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableEmpleadoCuchillo');
            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}

//function CambioColorCuchillo(valor,numero) {
//    $("#SelectNumeroCuchillo").empty();
//    $("#SelectNumeroCuchillo").append("<option value='' >-- Seleccionar Opción--</option>");
  
//    $.ajax({
//        url: "../Asistencia/ConsultaNumeroCuchillo",
//        type: "Get",
//        data:
//        {
//            dsColor: valor
//        },
//        success: function (resultado) {
         
//            if (!$.isEmptyObject(resultado)) {
//                $.each(resultado, function (create, row) {
//                    $("#SelectNumeroCuchillo").append("<option value='" + row.NumeroCuchillo + "'>" + row.NumeroCuchillo + "</option>")
//                });
//                if (numero != "0") {
//                    $('#SelectNumeroCuchillo').val(numero);    
//                }
//            } else {
//                MensajeAdvertencia("Color no tiene numeros asigandos", false);
//            }
//        },
//        error: function (resultado) {
//            MensajeError(JSON.stringify(resultado), false);
//        }
//    });
//}




function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}
