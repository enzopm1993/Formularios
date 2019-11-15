


function Consultar() {
    if ($("#txtFecha").val() == "") {
        $("#ValidaFecha").prop("hidden", false);
        return;
    } else {
        $("#ValidaFecha").prop("hidden", true);
    }

    if ($("#Selectreporte").val() == "0") {
        $("#ValidaReporte").prop("hidden", false);
        return;
    } else {
        $("#ValidaReporte").prop("hidden", true);
    }

    var Url = "";
    if ($("#Selectreporte").val() == "1") {
        Url = "../ControlEnfundado/ReportePorEnfundadora";
    } else {
        Url = "../ControlEnfundado/ReportePorHora";
    }
    $("#DivReporte").html("");
    $.ajax({
        url: Url,
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val() 

        },
        success: function (resultado) {
            if (resultado.Codigo == 0) {
                $("#DivReporte").html("<h3 class='text-center'>"+resultado.Mensaje+"</h3>");
            } else {
                $("#DivReporte").html(resultado);
            }
            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);           
        }
    });

}