﻿
$(document).ready(function () {
    CargarProyecciones();
});


function validar() {
    var valida = true;

    if ($("#txtFechaDesde").val() == '') {
        $("#validaFechaDesde").prop("hidden", false);
        valida = false;
    } else {
        $("#validaFechaDesde").prop("hidden", true);
    }


    if ($("#txtFechaHasta").val() == '') {
        $("#ValidaFechaHasta").prop("hidden", false);
        valida = false;
    } else {
        $("#ValidaFechaHasta").prop("hidden", true);
    }

    return valida;

}

function check(idCheck, idProyeccion,Fecha) {
    var checkPreparacion = "#ckeck" + idProyeccion;

    if ($("#" + idCheck).prop("checked")) {
        var urlFinalizar = "../ProyeccionProgramacion/FinalizarIngresoProyeccionProgramacion";
        FinalizaHabilitaProyeccion(idCheck, idProyeccion, urlFinalizar, Fecha);
        $("#txtObservacion" + idProyeccion).prop("disabled", true);
        $(checkPreparacion).prop("checked", false);
    } else {
        var urlHabilita = "../ProyeccionProgramacion/HabilitarIngresoProyeccionProgramacion";
        FinalizaHabilitaProyeccion(idCheck, idProyeccion, urlHabilita, Fecha);
        $("#txtObservacion" + idProyeccion).prop("disabled", false);
        $(checkPreparacion).prop("checked", true);

    }

}

function FinalizaHabilitaProyeccion(idCheck, idProyeccion, url, Fecha) {
    $.ajax({
        url: url,
        type: "GET",
        data: {
            id: idProyeccion,
            proceso: 4,
            Observacion: $("#txtObservacion" + idProyeccion).val(),
            Fecha: Fecha
        },
        success: function (resultado) {         
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "800") {
                var checkPreparacion = "#ckeck" + idProyeccion;
                var checkFinaliza = "#proyeccion" + idProyeccion;
                MensajeAdvertencia(Mensajes.MensajePeriodo);
                $("#txtObservacion" + idProyeccion).prop("disabled", false);
                $(checkPreparacion).prop("checked", false);
                $(checkFinaliza).prop("checked", false);
            } 
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $("#txtObservacion" + idProyeccion).prop("disabled", false);
        }
    });
}


function CargarProyecciones() {
    FechaDesde = $("#txtFechaDesde").val();
    FechaHasta = $("#txtFechaHasta").val();
    if (!validar())
        return;
    $("#divTableProyeccion").html('');
    $("#divMensaje").html('');
    $('#spinnerCargando').prop("hidden", false);
    $.ajax({
        url: "../ProyeccionProgramacion/FinalizaProyeccionProgramacionPreparacionPartial",
        type: "GET",
        data: {
            fechaDesde: FechaDesde,
            fechaHasta: FechaHasta
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $('#spinnerCargando').prop("hidden", true);
            if (resultado == '0') {
                $("#divMensaje").html('<h3 class="text-warning"> No existén registros</h3>');
            } else {
                $("#divTableProyeccion").html(resultado);
                config.opcionesDT.pageLength = 10;
                //config.opcionesDT.order = [[1, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#spinnerCargando').prop("hidden", true);

        }
    });
}