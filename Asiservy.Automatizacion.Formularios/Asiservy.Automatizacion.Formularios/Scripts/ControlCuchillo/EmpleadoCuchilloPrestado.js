
$(document).ready(function () {
    CargarEmpleadoCuchilloPrestado();
    CargarEmpleadoPrestado();
    $("#txtCuchilloBlanco").select2();
    $("#txtCuchilloRojo").select2();

});

function CargarEmpleadoCuchillo() {
    CargarEmpleadoCuchilloPrestado();
    CargarEmpleadoPrestado();
    CargarCuchillosDisponibles();
}
function CargarEmpleadoCuchilloPrestado() {
    if ($("#txtFecha").val() == '') {
        return;
    }
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableCuchilloPrestado').html('');
    $.ajax({
        url: "../ControlCuchillo/EmpleadoCuchilloPrestadoPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                $('#DivTableCuchilloPrestado').html("<div class='text-center'><h4>No Existen Registros</h4></div>");

            } else {
                $('#DivTableCuchilloPrestado').html(resultado);
                config.opcionesDT.pageLength = 50;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }


        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function CargarEmpleadoPrestado() {
    $("#selectEmpleado").empty();
    $("#selectEmpleado").append("<option value='' >-- Seleccionar Opción--</option>");
    if ($("#txtFecha").val() == '') {
        return;
    }
    $.ajax({
        url: "../ControlCuchillo/ConsultaEmpleadosPrestado",
        type: "Get",
        data:
        {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectEmpleado").append("<option value='" + row.CEDULA + "'>" + row.NOMBRES + "</option>")
                });
            } else {
                MensajeAdvertencia("No Existen Empleados Prestados", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function CargarCuchillosDisponibles() {
    $("#txtCuchilloBlanco").empty();
    $("#txtCuchilloBlanco").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#txtCuchilloRojo").empty();
    $("#txtCuchilloRojo").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#txtCuchilloNegro").empty();
    $("#txtCuchilloNegro").append("<option value='' >-- Seleccionar Opción--</option>");
    if ($("#txtFecha").val() == '') {
        return;
    }
    $.ajax({
        url: "../ControlCuchillo/ConsultaCuchillosDisponibles",
        type: "Get",
        data:
        {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    if (row.Color == 'B') {
                        $("#txtCuchilloBlanco").append("<option value='" + row.Numero + "'>" + row.Numero + "</option>")
                    } else if (row.Color == 'R') {
                        $("#txtCuchilloRojo").append("<option value='" + row.Numero + "'>" + row.Numero + "</option>")
                    } else if (row.Color == 'N') {
                        $("#txtCuchilloNegro").append("<option value='" + row.Numero + "'>" + row.Numero + "</option>")
                    }
                });
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}

function Validar() {
    var valida = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectEmpleado").val() == "") {
        $("#selectEmpleado").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectEmpleado").css('borderColor', '#ced4da');
    }

    return valida;
}

function GuardarModificarCuchilloEmpleadoPrestado() {
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../ControlCuchillo/EmpleadoCuchilloPrestado",
        type: "POST",
        data:
        {
            Cedula: $("#selectEmpleado").val(),
            Fecha: $("#txtFecha").val(),
            CuchilloBlanco: $("#txtCuchilloBlanco").val(),
            CuchilloRojo: $("#txtCuchilloRojo").val(),
            CuchilloNegro: $("#txtCuchilloNegro").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '0') {
                MensajeAdvertencia("No se ha encontrado al empleado");
                return;
            }
            if (resultado == '1') {
                MensajeAdvertencia("Cuchillo ya ha sido asignado a otro empleado");
                return;
            }
            CargarEmpleadoCuchilloPrestado();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}

function seleccionarCuchilloEmpleadoPrestado(model) {
    $("#selectEmpleado").val(model.Cedula);
    $("#txtCuchilloBlanco").val(model.CuchilloBlanco).trigger('change');
    $("#txtCuchilloRojo").val(model.CuchilloRojo).trigger('change');
    $("#txtCuchilloNegro").val(model.CuchilloNegro).trigger('change');
}