$(document).ready(function () {
    $("#selectCondicion").select2();
});

function ValidarEmpleado() {
    var valida = true;
  
    if ($("#Lineas").val() == "") {
        $("#Lineas").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#Lineas").css("border-color", "#d1d3e2");
    }

    return valida;
}
function CargarEmpleados(formulario) {

    if (!ValidarEmpleado()) {
        return;
    }
    MostrarModalCargando();
    $('#' + formulario).attr("disabled", true);
    $.ajax({
        url: "../General/EmpleadoBuscar",
        type: "Get",
        data:
        {
            dsLinea: $('#Lineas').val()
        },
        success: function (resultado) {
            $('#ModelCargarEmpleados').html(resultado);
            $("#ModalEmpleado").modal("show");
            $('#' + formulario).attr("disabled", false);
            CerrarModalCargando();
        },
        error: function (resultado) {
            MensajeError("Se ha generado un error, Comuniquese con sistemas.", false);
            $('#' + formulario).remove("disabled");
            CerrarModalCargando();

        }
    });

}


function NuevoControl() {
    $("#Lineas").css("border-color", "#d1d3e2");
    $("#Cedula").val('');
    $("#Nombre").val('');
    //$("#txtFecha").val('');
    $("#txtHora").val(moment().format("HH:mm"));
    $("#txtObservacion").val('');
    $("#selectCondicion").prop("selectedIndex", 0).change();
    $("#Lineas").prop("selectedIndex", 0);
}

function Validar() {
    var valida = true;
    if ($("#Cedula").val() == "") {
        $("#Nombre").css('borderColor', '#FA8072');
        valida=false;
    } else {
        $("#Nombre").css('borderColor', '#ced4da');
    }
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    if ($("#selectCondicion").val() == "") {
        //$("#SelectTextura").css('borderColor', '#FA8072');
        $("#selectCondicion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });
        valida = false;
    } else {
        $("#selectCondicion").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });
    }

    //if ($("#selectCondicion").val() == "") {
    //    $("#selectCondicion").css('borderColor', '#FA8072');
    //    valida = false;
    //} else {
    //    $("#selectCondicion").css('borderColor', '#ced4da');
    //}
    return valida;
}


function GuardarControl() {
    if (!Validar()) {
        return;
    }
    
    $.ajax({
        url: "../CondicionPersonal/CondicionPersonal",
        type: "POST",
        data: {
            IdCondicionPersonal: $("#txtIdControl").val(),
            Fecha: $("#txtFecha").val(),
            Hora: $("#txtHora").val(),
            Cedula: $("#Cedula").val(),
            CodCondicion: $("#selectCondicion").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            } else {
                NuevoControl();
                ConsultarReporte();
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

    //alert("generado");
}