
function CargarReporteAvance() {
    //var txtFecha = $('#txtFecha').val();
    var txtFechaDesde = $('#txtFechaDesde').val();
    var txtFechaHasta = $('#txtFechaHasta').val();
    var selectLinea = $('#selectLinea').val();
    var bitacora = $('#DivTableReporteControlAvance');
    bitacora.html('');

    // console.log(selectLinea);

   
    if (txtFechaDesde == "" || txtFechaHasta == "") {
        MensajeAdvertencia("Igrese un rango de fechas");
        return;
    }
    if (txtFechaDesde > txtFechaHasta) {
        MensajeAdvertencia("Fecha hasta no puede ser menor a fecha desde");
        return;
    }
    if (moment(txtFechaDesde).format("MMMM") != moment(txtFechaHasta).format("MMMM")) {
        MensajeAdvertencia("Solo puede consultar un mes");
        return;
    }
   // console.log(moment(txtFechaDesde).format("MMMM"));

    if (selectLinea == "") {
        MensajeAdvertencia("Seleccione una Linea");
        return;
    }
    if ($("#selectTurno").val() == "") {
        MensajeAdvertencia("Seleccione un turno");
        return;
    }
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../Hueso/ReporteControlAvanceDiarioPartial",
        type: "GET",
        data: {
            ddFechaDesde: txtFechaDesde,
            ddFechaHasta: txtFechaHasta,
            Turno: $("#selectTurno").val(),
            dsLinea: selectLinea
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "1") { 

                MensajeAdvertencia("No existen registros para esa linea");
                $("#spinnerCargando").prop("hidden", true);

            } else {
                var bitacora = $('#DivTableReporteControlAvance');
                $("#spinnerCargando").prop("hidden", true);
                bitacora.html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $('#btnConsultar').prop("disabled", true);
           
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);


        }
    });

}