function Consultar() {
    //  console.log($("#selectLinea").val());
    if ($("#selectLinea").val() == '') {
        $("#selectLinea").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectLinea").css('borderColor', '#ced4da');      
    }

    if ($("#txtFechaPrestado").val() == '') {
        $("#txtFechaPrestado").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaPrestado").css('borderColor', '#ced4da');
    }

    ConsultarEmpleadoTurno();
}

function ConsultarEmpleadoTurno() {
    var bitacora = $('#TableEmpleadoTurno');
    bitacora.html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Empleado/EmpleadoTurnoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFechaPrestado").val(),
            Linea: $("#selectLinea").val()

        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            var bitacora = $('#TableEmpleadoTurno');
            if (resultado == "0")
                bitacora.html('<div class="text-center"><h4>No Existen Registros</h4></div>');
            else
                bitacora.html(resultado);
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            MensajeError(resultado, false);
        }
    });
}


function GuardarEmpleadoTurno(value, linea) {
    var empleado = value.split("-");
    $.ajax({
        url: "../Empleado/EmpleadoTurnoPartial",
        type: "POST",
        data: {
            Cedula: empleado[0],
            Turno: empleado[1],
            CodLinea: linea
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            if (resultado == "0")
                mensajeAdvertencia("Faltan Parametros");  
        },
        error: function (resultado) {
            console.log(resultado);
            MensajeError(resultado, false);
        }
    });
}