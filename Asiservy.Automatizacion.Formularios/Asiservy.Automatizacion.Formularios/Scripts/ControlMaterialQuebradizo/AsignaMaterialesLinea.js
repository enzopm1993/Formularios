


$(document).ready(function () {
    CargarAsignaciones();
});


function CargarAsignaciones() {
    if ($("#selectLineas").val() == "") {        
        return;
    }  

    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableAsignaciones').html('');
    $('#Mensaje').html('');

    $.ajax({
        url: "../ControlMaterialQuebradizo/AsignaMaterialesLineaPartial",
        type: "GET",
        data: {
            Linea: $("#selectLineas").val()
        },
        success: function (resultado) {
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                $('#Mensaje').html('<h3 class="text-warning">No existen registros.</h3>');
                return;
            }
            var div = $('#DivTableAsignaciones');
          
            div.html(resultado);
            config.opcionesDT.pageLength = 10;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);
        },
        error: function (resultado) {
            MensajeError(resultado, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}


function Validar() {
    var valida = true;
    if ($("#selectLineas").val() == "") {
        $("#selectLineas").css("border-color", "#DC143C");
        valida = false;
    } else { 
    $("#selectLineas").css("border-color", "#d1d3e2");
    }
    if ($("#selectMateriales").val() == "") {
        $("#selectMateriales").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#selectMateriales").css("border-color", "#d1d3e2");
    }
    return valida;
}



function GuargarAsignacion() {   
    if (!Validar()) {
        return;
    }
    var Estado = $("#CheckEstadoRegistro").prop('checked');
    if (Estado == true)
        Estado = "A";
    else
        Estado = "I";

    $.ajax({
        url: "../ControlMaterialQuebradizo/AsignaMaterialesLinea",
        type: "POST",
        data: {
            Linea: $("#selectLineas").val(),          
            Codigo: $("#selectMateriales").val(),
            EstadoRegistro: Estado
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
           // Limpiar();
            CargarAsignaciones();
        },
        error: function (resultado) {
            MensajeError(resultado, false);

        }
    });
}


function SeleccionarAsignacionesMaterial(model) {
   // console.log(model);
    $("#selectMateriales").val(model.CodMaterial);

    if (model.EstadoRegistro == "A") {
        CambioEstado(true);
        $("#CheckEstadoRegistro").prop('checked', true);
    } else {
        $("#CheckEstadoRegistro").prop('checked', false);
        CambioEstado(false);
    }
}


function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');
}
