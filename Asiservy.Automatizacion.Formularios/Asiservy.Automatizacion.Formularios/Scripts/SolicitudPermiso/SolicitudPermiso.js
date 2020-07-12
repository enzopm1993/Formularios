
$(document).ready(function () {
    $("#DivHora").hide(); 
    if ($("#txtControladorLinea").val()=="1")
        CargarAreas();
    //CargarAreas();
});


function CargarAreas() {
    $.ajax({
        url: "../SolicitudPermiso/ConsultaListadoAreas",
        type: "Get",
        data:
        {
            CodLinea: $('#selectLinea').val()
        },
        success: function (resultado) {
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#selectArea").append("<option value='" + row.Codigo + "'>" + row.Descripcion + "</option>")
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
function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraSalida");
    var HoraHasta = document.getElementById("timeHoraRegreso");
    var FechaSalidaRegreso = document.getElementById("dateSalidaRegreso");
    var FechaDesde = document.getElementById("dateSalida");
    var FechaHasta = document.getElementById("dateRegreso");
    var check = document.getElementById("switchHoraFecha").checked
   

    if (check) {
        $("#LabelFecha").text("Hora");
        $("#DivHora").slideUp(300).fadeIn(1000);
        $("#DivFecha").slideUp(300).fadeOut(1000);
        FechaDesde.value = null;
        FechaHasta.value = null;
    } else {
        $('#LabelFecha').text("Fecha");
        $("#DivHora").slideUp(300).fadeOut(1000);
        $("#DivFecha").slideUp(300).fadeIn(1000);    
        FechaSalidaRegreso.value = null;
        HoraDesde.value = null;
        HoraHasta.value = null;
    }
}
function GuardarSolicitud() {
    $("#GuardarSolicitudGeneral").prop("hidden", true);
    $("#formSolicitudPermiso").submit();
}

var modalConfirm = function (callback) {
    $("#GuardarSolicitudGeneral").on("click", function () {
        var check = document.getElementById("switchHoraFecha").checked
        if (check) {
            if (moment($("#dateSalidaRegreso").val()).format("YYYY-MM-DD") == moment().format("YYYY-MM-DD")) {
                GuardarSolicitud();
            } else {
                $("#myModalLabel").html("Está Generando una solicitud para el dia: " + $("#dateSalidaRegreso").val());
                $("#mi-modal").modal('show');
            }
          } else {            
            if (moment($("#dateSalida").val()).format("YYYY-MM-DD") == moment().format("YYYY-MM-DD")) {
                GuardarSolicitud();
            } else {
                $("#myModalLabel").html("Está Generando una solicitud para el dia: " + $("#dateSalida").val());
                $("#mi-modal").modal('show');
            }
        }
    });

    $("#modal-btn-si").on("click", function () {
        callback(true);
        $("#mi-modal").modal('hide');
    });

    $("#modal-btn-no").on("click", function () {
        //        callback(false);
        $("#mi-modal").modal('hide');
    });
};

modalConfirm(function (confirm) {
    if (confirm) {
        //Acciones si el usuario confirma
        GuardarSolicitud();
    }
});





