var Listado= [];

$(document).ready(function () {
    ConsultarEntregaUniformes();
});
function ConsultarEntregaUniformes() {
    $("#spinnerCargando").prop("hidden", false);
    if ($("#txtFecha").val() == "") {
        return;
    }
    $('#divEntregaUniforme').html('');
    $.ajax({
        url: "../EntregaUniforme/BandejaEntregaUniformePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val()
        },
        success: function (resultado) {
            //console.log(JSON.stringify(resultado));
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                var divTable = $('#divEntregaUniforme');
                divTable.html('No existen registros');
                $("#spinnerCargando").prop("hidden", true);
                return;
            }
            var divTable = $('#divEntregaUniforme');
            $("#spinnerCargando").prop("hidden", true);            
            divTable.html(resultado);
            config.opcionesDT.pageLength = -1;
            config.opcionesDT.order = [4, "desc"];

            $('#tblDataTable').DataTable(config.opcionesDT);

           
        },
        error: function (result) {
           /* console.log(JSON.stringify(result.responseText))*/;
            MensajeError(resultado.Mensaje, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}
function EntregarUniforme(modal) {
    Listado = modal;
    $("#txtHora").val(moment().format("HH:mm"));
    $("#ModalEntregaUniforme").modal("show");
}


function CambiarEstadoEntregaUniforme() {

    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }

    $.ajax({
        url: "../EntregaUniforme/EntregarUniforme",
        type: "POST",
        data: {
            IdEntregaUniforme: Listado.IdEntregaUniforme,
            Fecha: $("#txtFecha").val(),
            Cedula: Listado.Cedula,
            EstadoEntrega: true,
            HoraEntregada:$("#txtHora").val()
        },
        success: function (resultado) {     
            $("#ModalEntregaUniforme").modal("hide");
            if (resultado == "101") {
                window.location.reload();
            } else if (resultado == "0") {
                var divTable = $('#divEntregaUniforme');
                divTable.html('Faltan Parametros');
                $("#spinnerCargando").prop("hidden", true);
                Listado = [];
                return;
            } else if (resultado.Respuesta == false) {
                MensajeAdvertencia(resultado.Mensaje);
            } else {
                MensajeCorrecto(resultado.Mensaje);     
            }
            ConsultarEntregaUniformes();
            Listado = [];
        },
        error: function (result) {          
            MensajeError(resultado.Mensaje, false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });

}