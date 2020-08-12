


function Consultar() {
    $("#DivReporte").html("");

   
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }


    if ($("#selectTurno").val() == "") {
        $("#selectTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#selectTurno").css('borderColor', '#ced4da');
    }

    if ($("#Selectreporte").val() == "") {
        $("#Selectreporte").css('borderColor', '#FA8072');
        return;
    } else {
        $("#Selectreporte").css('borderColor', '#ced4da');
    }
       
    var Url = "";
    if ($("#Selectreporte").val() == "1") {
        Url = "../ControlEnfundado/ReportePorEnfundadora";
    } else {
        Url = "../ControlEnfundado/ReportePorHora";
    }
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: Url,
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()

        },
        success: function (resultado) {
           
            if (resultado.Codigo == 0) {
                $("#DivReporte").html("<h3 class='text-center'>"+resultado.Mensaje+"</h3>");
            } else {
                $("#DivReporte").html(resultado);
            }
            $("#spinnerCargando").prop("hidden", true);

            
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);  
            $("#spinnerCargando").prop("hidden", true);

        }
    });

}