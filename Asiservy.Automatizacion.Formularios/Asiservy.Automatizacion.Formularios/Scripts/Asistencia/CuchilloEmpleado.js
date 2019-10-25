

$(document).ready(function () {
    console.log("prueba");

    CargarEmpleadoCuchillo();
    NuevoCuchilloEmpleado();
});

function Validar() {
    var cedula = $("#Identificacion").val();
    var CBlanco = $("#SelectCuchilloBlanco").val();
    var CRojo = $("#SelectCuchilloRojo").val();
    var CNegro = $("#SelectCuchilloNegro").val();


    if (cedula == "") {
        jQuery("#validaCedula").html("Seleccione un Empleado");
        return false;
    }
    if (CBlanco == '' && CRojo == '' && CNegro == '') {
        MensajeAdvertencia("Seleccione al menos un cuhillo.");
        return false;
    }
    return true;

}

function GrabarCuchilloEmpleado() {
    if (!Validar()) {
        return;
    }
    $("#btnNuevo").prop("disabled", false);
    $("#btnGuardar").prop("disabled", false);
    $.ajax({
        url: "../Asistencia/CuchilloEmpleado",
        type: "Post",
        data: {
            IdEmpleadoCuchillo: $("#IdEmpleadoCuchillo").val(),
            Cedula: $("#Identificacion").val(),
            CuchilloBlanco: $("#SelectCuchilloBlanco").val(),
            CuchilloRojo: $("#SelectCuchilloRojo").val(),
            CuchilloNegro: $("#SelectCuchilloNegro").val(),
            EstadoRegistro: $("#CheckEstadoRegistro").val()


        },
        success: function (resultado) {
            $("#btnNuevo").prop("disabled", true);
            $("#btnGuardar").prop("disabled", true);

            if (resultado == "1") {
                MensajeAdvertencia("Parametros Incompletos");
                return false;
            } else {
                MensajeCorrecto(resultado);
                NuevoCuchilloEmpleado();
                CargarEmpleadoCuchillo();
            }
          
        },
        error: function (resultado) {
            $("#btnNuevo").prop("disabled", true);
            $("#btnGuardar").prop("disabled", true);
            MensajeError(resultado, false);

        }
    });
}


function NuevoCuchilloEmpleado() {
    $('#IdEmpleadoCuchillo').val("0");
    $("#SelectEmpleado").prop('selectedIndex', 0);
    $('#SelectCuchilloBlanco').prop('selectedIndex', 0);
    $('#SelectCuchilloRojo').prop('selectedIndex', 0);
    $('#SelectCuchilloNegro').prop('selectedIndex', 0);
    $('#NombreEmpleado').val("");
    $('#Identificacion').val("");
   jQuery("#validaCedula").html("");

}

function SeleccionEmpleadoCuchillo(id, cedula, blanco,rojo, negro, estado, nombre) {
    
    $('#IdEmpleadoCuchillo').val(id);
    $('#SelectEmpleado').val(cedula);
    $('#SelectCuchilloBlanco').val(blanco);
    $('#SelectCuchilloRojo').val(rojo);
    $('#SelectCuchilloNegro').val(negro);
    $('#NombreEmpleado').val(nombre);
    $('#Identificacion').val(cedula);
   
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
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Asistencia/CuchilloEmpleadoPartial",
        type: "GET",
        success: function (resultado) {

            var bitacora = $('#DivTableEmpleadoCuchillo');
            $("#spinnerCargando").prop("hidden", true);

            bitacora.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);

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
