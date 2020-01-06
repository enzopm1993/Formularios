


$(document).ready(function () {
    CargarColorCuchillos();
    //  Nuevo();
});


function SeleccionCuchilloColor(numero, color, estado) {
    $('#NumeroCuchillo').val(numero);
    $('#ColorCuchillos').val(color.charAt(0));
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

function GuardarCuchillo() {

    $.ajax({
        url: "../ControlCuchillo/Cuchillo",
        type: "POST",
        data: {
            NumeroCuchillo: $("#NumeroCuchillo").val(),
            ColorCuchillo: $("#ColorCuchillos").val(),
            EstadoRegistro: $("#CheckEstadoRegistro").prop("checked")
        },
        success: function (resultado) {
            if (resultado == "1") {
                MensajeAdvertencia("Faltan Parametros");
            }
            else {
                MensajeCorrecto(resultado);
                CargarColorCuchillos();
            }
        },
        error: function (resultado) {
            MensajeError(resultado, false);

        }
    });
}

function CargarColorCuchillos() {
    $("#spinnerCargando").prop("hidden", false);
    $('#DivColorCuchillos').html('');
    $.ajax({
        url: "../ControlCuchillo/CuchilloPartial",
        type: "GET",
        data: {
            dsColor: $("#ColorCuchillos").val()
        },
        success: function (resultado) {
            var bitacora = $('#DivColorCuchillos');
            bitacora.html(resultado);

            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}



//Limpia el campo de busqueda creado sin DataTable.net
function LimpiarTexto() {
    $.each($("#TableCuchillos tbody tr"), function () {
        $(this).show();
    });
    document.getElementById("search").innerText = "";
    $("#search").val("");
}

//Busqueda Manual para tablas
$(document).ready(function () {
    $("#search").keyup(function () {
        _this = this;
        // Show only matching TR, hide rest of them
        $.each($("#TableCuchillos tbody tr"), function () {
            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1)
                $(this).hide();
            else
                $(this).show();
        });
    });
});


//Bloquear todos los cuchillos negros que no se usan por el momento
var i = 0;
$("tr").each(function () {
    var desCheck = "CheckCuchilloNegro";
    if (i > 1)
        desCheck += i;
    var x = document.getElementById(desCheck);
    if (x != null)
        x.disabled = true;
    i++;
});


//pintan celda de check de cuchillo
function Cuchillo(color, fila) {

    var desLabel = "labelCuchillo";
    var desCheck = "CheckCuchillo";

    if (color == 1) { desLabel += "Rojo"; desCheck += "Rojo"; }
    if (color == 2) { desLabel += "Negro"; desCheck += "Negro"; }
    if (color == 3) { desLabel += "Blanco"; desCheck += "Blanco"; }

    if (fila != 1) {
        desLabel += fila;
        desCheck += fila;
    }
    //console.log(desCheck);
    var label = document.getElementById(desLabel);
    var check = document.getElementById(desCheck).checked;
    //console.log(check);

    if (check) {
        label.style.background = "#27D5C3";
        document.getElementById(desCheck).checked = true;
    } else {
        label.style.background = "#ccc";
        document.getElementById(desCheck).checked = false;
    }

}

function Guardar() {
    var Estado = document.getElementById("SelectEstado");
    // console.log(Estado);
    //console.log(Estado.selectedIndex);
    if (Estado.selectedIndex == 0) {
        Mensaje("Seleccione un estado..");
        //   alert("Seleccione un Estado..");
    } else {
        Mensaje("Registro Guardado Exitosamente");
    }

}

function prueba() {

    var checkRojo = document.getElementById("CheckCuchilloRojo").checked;
    var checkBlanco = document.getElementById("CheckCuchilloBlanco").checked;
    var checkNegro = document.getElementById("CheckCuchilloNegro").checked;

    console.log(checkRojo, checkNegro, checkBlanco);

}


function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}