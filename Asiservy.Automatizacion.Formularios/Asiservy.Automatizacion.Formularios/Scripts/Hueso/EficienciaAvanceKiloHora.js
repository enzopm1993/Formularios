$(document).ready(function () {  
    CargarEficienciaAvanceKilosHora();
});

function CargarEficienciaAvanceKilosHora() {   
    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableEficiencia').html('');
    $.ajax({
        url: "../Hueso/EficienciaAvanceKiloHoraPartial",
        type: "GET",     
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargando").prop("hidden", true);
            if (resultado == "0") {
                $('#DivTableEficiencia').html("<div class='text-center'><h4>No Existen Registros</h4></div>");
            } else {
                $('#DivTableEficiencia').html(resultado);
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


function seleccionarEficienciaAvance(model) {
    $("#txtIdEficiencia").val(model.IdAvanceKilosHora);
    $("#selectTalla").val(model.Talla);
    $("#txtSencilla").val(model.Sencilla);
    $("#txtIntermedia").val(model.Intermedia);
    $("#txtDoble").val(model.Doble);
}

function NuevaEficienciaAvance() {
    $("#txtIdEficiencia").val(0);
    $("#selectTalla").prop("selectedIndex",0);
    $("#txtSencilla").val(0);
    $("#txtIntermedia").val(0);
    $("#txtDoble").val(0);
}


function GuardarModificarEficiencia() {
   
    $.ajax({
        url: "../Hueso/EficienciaAvanceKiloHora",
        type: "POST",
        data:
        {
            IdAvanceKilosHora: $("#txtIdEficiencia").val(),
            Talla: $("#selectTalla").val(),
            Sencilla: $("#txtSencilla").val(),
            Intermedia: $("#txtIntermedia").val(),
            Doble: $("#txtDoble").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            NuevaEficienciaAvance();
            CargarEficienciaAvanceKilosHora();
            MensajeCorrecto(resultado);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
        }
    });
}
