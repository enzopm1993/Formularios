﻿
$(document).ready(function () {
    CargarReporte();
  
    $("#btnColumnas").prop("hidden", false);
});

function ValidarEstado() {
  //  console.log("Hola mundo");
    $("#Check1").prop("checked", false);
    $("#Check2").prop("checked", false);
    $("#Check3").prop("checked", false);
    $("#Check4").prop("checked", false);
    $("#Check5").prop("checked", false);
    $.ajax({
        url: "../ProyeccionProgramacion/ValidarProyeccionProgramacionEstado",
        type: "GET",
        data: { Fecha: $('#txtFecha').val(), Turno: $('#selectTurno').val()},
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }            
            if (resultado == 1) {
                $("#Check1").prop("checked", true);
            } else if (resultado == 2) {
                $("#Check2").prop("checked", true);
            } else if (resultado == 3) {
                $("#Check3").prop("checked", true);
            } else if (resultado == 4) {
                $("#Check4").prop("checked", true);
            } else if (resultado == 5) {
                $("#Check5").prop("checked", true);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function CargarReporte() {

    if ($('#txtFechaProduccion').val() == "" || $('#selectTurno').val() == "") {
        return;
    }

    $("#spinnerCargando").prop("hidden", false);
    $('#DivTableReporteProyeccion').html('');
    $.ajax({
        url: "../ProyeccionProgramacion/ReporteProyeccionProgramacionPartial",
        type: "GET",
        data: { Fecha: $('#txtFecha').val(), Turno: $('#selectTurno').val() },
        success: function (resultado) {     
            if (resultado == "101") {
                window.location.reload();
            }
            $('#DivTableReporteProyeccion').html(resultado);
            $("#spinnerCargando").prop("hidden", true);
            config.opcionesDT.pageLength = 50;
            $('#tblDataTable').DataTable(config.opcionesDT);           
            ValidarEstado();

        },
        error: function (resultado) {
            MensajeError(resultado.responseText);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}
