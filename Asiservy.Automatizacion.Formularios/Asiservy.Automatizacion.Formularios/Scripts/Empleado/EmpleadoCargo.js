
function Validar() {
    var bool = true;
    $("#ValidaIdentificacion").prop("hidden", true);
    $("#ValidaLinea").prop("hidden", true);
    $("#ValidaArea").prop("hidden", true);
    $("#ValidaCargo").prop("hidden", true);

    if ($("#Identificacion").val() == "0") {
        bool = false;
        $("#ValidaIdentificacion").prop("hidden", false);

    }
    if ($("#selectLinea2").val() == "") {
        bool = false;
        $("#ValidaLinea").prop("hidden", false);

    }
    if ($("#selectArea2").val() == "") {
        bool = false;
        $("#ValidaArea").prop("hidden", false);

    }
    if ($("#selectCargo2").val() == "") {
        bool = false;
        $("#ValidaCargo").prop("hidden", false);

    }


    return bool;

}

function NuevoEmpleadoCargo() {
    $("#ValidaIdentificacion").prop("hidden", true);
    $("#ValidaLinea").prop("hidden", true);
    $("#ValidaArea").prop("hidden", true);
    $("#ValidaCargo").prop("hidden", true);

    $("#Identificacion").val('0'); 
    $("#NombreEmpleado").val('');
    $("#selectLinea").prop("selectedIndex",0);
    $("#selectArea").empty();
    $("#selectArea").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#selectCargo").empty();
    $("#selectCargo").append("<option value='' >-- Seleccionar Opción--</option>");

    $("#selectLinea2").prop("selectedIndex", 0)
    $("#selectArea2").empty();
    $("#selectArea2").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#selectCargo2").empty();
    $("#selectCargo2").append("<option value='' >-- Seleccionar Opción--</option>");
}


function GuardarCambioCargo() {
    if (!Validar()) {
        return;
    }
    $.ajax({
        url: "../Empleado/EmpleadoCargo",
        type: "POST",
        data:
        {
            Cedula: $("#Identificacion").val(),
            CodLinea: $("#selectLinea2").val(),
            CodArea: $("#selectArea2").val(),
            CodCargo: $("#selectCargo2").val()
        },
        success: function (resultado) {
            MensajeCorrecto(resultado);
            NuevoEmpleadoCargo();
        },
        error: function (resultado) {
            MensajeError(resultado, false);
        }
    });
}



function CambioLinea2(valor) {
    $("#selectArea2").empty();
    $("#selectArea2").append("<option value='' >-- Seleccionar Opción--</option>");
    $("#selectCargo2").empty();
    $("#selectCargo2").append("<option value='' >-- Seleccionar Opción--</option>");

    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: valor
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea2").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}


function CambioArea2(valor) {

    $("#selectCargo2").empty();
    $("#selectCargo2").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoCargos",
        type: "Get",
        data:
        {
            CodArea: valor
        },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                $.each(data, function (create, row) {
                    $("#selectCargo2").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
                });
            } else {
                MensajeAdvertencia("La linea seleccionado no tiene areas asignadas", false);
            }
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
