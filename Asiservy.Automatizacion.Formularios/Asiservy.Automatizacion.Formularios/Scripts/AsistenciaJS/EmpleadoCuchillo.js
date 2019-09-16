

$(document).ready(function () {
    CargarEmpleadoCuchillo();
     Nuevo();
});

function Nuevo() {
    $('#IdEmpleadoCuchillo').val("0");
    $("#SelectEmpleado").prop('selectedIndex', 0);
    $("#SelectColor").prop('selectedIndex', 0);
    $("#SelectNumeroCuchillo").empty();
    $("#SelectNumeroCuchillo").append("<option value='' >-- Seleccionar Opción--</option>");
}

function SeleccionEmpleadoCuchillo(id, cedula, color, numero, estado) {
    
    $('#IdEmpleadoCuchillo').val(id);
    $('#SelectEmpleado').val(cedula);
    $('#SelectColor').val(color.charAt(0));
    CambioColorCuchillo(color.charAt(0), numero);
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

function CambioColorCuchillo(valor,numero) {
    $("#SelectNumeroCuchillo").empty();
    $("#SelectNumeroCuchillo").append("<option value='' >-- Seleccionar Opción--</option>");
  
    $.ajax({
        url: "../Asistencia/ConsultaNumeroCuchillo",
        type: "Get",
        data:
        {
            dsColor: valor
        },
        success: function (resultado) {
         
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectNumeroCuchillo").append("<option value='" + row.NumeroCuchillo + "'>" + row.NumeroCuchillo + "</option>")
                });
                if (numero != "0") {
                    $('#SelectNumeroCuchillo').val(numero);    
                }
            } else {
                MensajeAdvertencia("Color no tiene numeros asigandos", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}




function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}
