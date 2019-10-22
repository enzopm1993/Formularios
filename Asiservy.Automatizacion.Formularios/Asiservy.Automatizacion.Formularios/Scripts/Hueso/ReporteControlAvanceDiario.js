
function CargarReporteAvance() {
    var txtFecha = $('#txtFecha').val();
    var selectLinea = $('#selectLinea').val();
   // console.log(selectLinea);
    if (txtFecha == "") {
        MensajeAdvertencia("Igrese una Fecha");
        return;
    }
    if (selectLinea == "") {
        MensajeAdvertencia("Seleccione una Linea");
        return;
    }
    $('#btnConsultar').prop("disabled", true);
    $("#spinnerCargando").prop("hidden", false);
    var bitacora = $('#DivTableReporteControlAvance');
    bitacora.html('');
    $.ajax({
        url: "../Hueso/ReporteControlAvanceDiarioPartial",
        type: "GET",
        data: {
            ddFecha:txtFecha,
            dsLinea: selectLinea
        },
        success: function (resultado) {
            if (resultado == "1") { 

                MensajeAdvertencia("No existen registros para esa linea");
                $("#spinnerCargando").prop("hidden", true);

            } else {
                var bitacora = $('#DivTableReporteControlAvance');
                $("#spinnerCargando").prop("hidden", true);
                bitacora.html(resultado);
               
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