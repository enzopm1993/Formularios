


function ConsultarBitacora() {
    $.ajax({
        url: "../Asistencia/BitacoraCambioPersonalPartial",
        type: "GET",
        data: {
            
            dsLinea: $('#selectLinea').val(),
            dsArea: $('#selectArea').val(),
            dsCArgo: $('#selectCargo').val(),
            dsCedula: $('#textCedula').val(),
            ddFechaDesde: $('#dateDesde').val(),
            ddFechaHasta: $('#dateHasta').val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));

            var bitacora = $('#TableBitacoraCambioPersonal');            
                bitacora.html(resultado);
          
        },
        error: function (result) {         
            MensajeError(result, false);
        }
    });
}

