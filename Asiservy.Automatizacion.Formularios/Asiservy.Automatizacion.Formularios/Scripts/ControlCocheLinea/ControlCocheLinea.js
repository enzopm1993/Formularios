
$(document).ready(function () {
    CargarControlCoche();
});


function SelectControlCoche(id, fecha, horaInicio, horaFin, Linea, talla,observacion, coche) {
    

    $("#txtIdControlCoche").val(id);
    $("#txtFecha").val(fecha);
    $("#txtHoraInicio").val(horaInicio);
    $("#txtHoraFin").val(horaFin);
    $("#txtCoches").val(coche);
    $("#selectLineas").val(Linea);
    $("#selectTalla").val(talla);

    $("#txtObservacion").val(observacion);
} 
function CargarControlCoche() {
    $.ajax({
        url: "../ControlCocheLinea/ControlCocheLineaPartial",
        type: "GET",
        success: function (resultado) {
            var DivControl = $('#DivTableControlCoche');
            DivControl.html(resultado);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });

}

function Nuevo() {
    $("#txtIdControlCoche").val("0");

    var fecha = new Date();
    // console.log(fecha);
    var dia = fecha.getDate();
    var mes = fecha.getMonth() + 1;
    if (dia < 10)
        dia = "0" + dia;
    if (mes < 10)
        mes = "0" + mes;
    var fechaFinal = fecha.getFullYear() + "-" + mes + "-" + dia;    
    $("#txtFecha").val(fechaFinal);
    $("#txtHoraInicio").val("00:00");
    $("#txtHoraFin").val("00:00");
    $("#txtCoches").val("0");
    $("#selectLineas").prop("selectedIndex",0);  
    $("#txtObservacion").val("");

    $("#txtValidaFecha").prop("hidden", true);
    $("#txtValidaHoraInicio").prop("hidden", true);
    $("#txtValidaHoraFin").prop("hidden", true);
    $("#txtValidaCoche").prop("hidden", true);
    $("#txtValidaLinea").prop("hidden", true);
}



function validar() {
  
   var fecha =  $("#txtFecha").val();
   var horaInicio= $("#txtHoraInicio").val();
   var horaFin= $("#txtHoraFin").val();
   var coches= $("#txtCoches").val();
   var linea= $("#selectLineas").val();
    var talla = $("#selectTalla").val();
    var bool= true;

    if (fecha == "") {
        $("#txtValidaFecha").prop("hidden", false);
        bool= false;
    } else {
        $("#txtValidaFecha").prop("hidden", true);
    }
    if (horaInicio == "") {
        $("#txtValidaHoraInicio").prop("hidden", false);
        bool = false;
    } else {
        $("#txtValidaHoraInicio").prop("hidden", true);
    }
    if (horaFin == "") {
        $("#txtValidaHoraFin").prop("hidden", false);
        bool = false;
    } else {
        $("#txtValidaHoraFin").prop("hidden", true);
        if (horaInicio >= horaFin) {
            MensajeAdvertencia("Hora fin tiene que ser mayor a la de inicio");
            bool = false;
        }
    }    
    if (coches == "" || coches<=0) {
        $("#txtValidaCoche").prop("hidden", false);
        bool = false;
    } else {
        $("#txtValidaCoche").prop("hidden", true);
    }
    if (linea == "") {
        $("#txtValidaLinea").prop("hidden", false);
        bool = false;
    } else {
        $("#txtValidaLinea").prop("hidden", true);
    }
    if (talla == "") {
        $("#txtValidaTalla").prop("hidden", false);
        bool = false;
    } else {
        $("#txtValidaTalla").prop("hidden", true);
    }

    return bool;
}

function GuardarControl() {
    if (!validar())
        return;
    $("#btnGuardar").prop("disabled", true);
    $('#btnGuardarCargando').prop("hidden", false);
    $('#btnGuardar').prop("hidden", true);
    $.ajax({
        url: "../ControlCocheLinea/ControlCocheLinea",
        type: "POST",
        data: {
            IdControlCocheLinea: $("#txtIdControlCoche").val(),
            Fecha: $("#txtFecha").val(),
            HoraInicio: $("#txtHoraInicio").val(),
            HoraFin: $("#txtHoraFin").val(),
            Coches: $("#txtCoches").val(),
            Linea: $("#selectLineas").val(),
            Talla: $("#selectTalla").val(),
            Observacion: $("#txtObservacion").val(),
        },
        success: function (resultado) {
           
            CargarControlCoche();
            
            MensajeCorrecto(resultado);
            Nuevo();          
            $("#btnGuardar").prop("disabled", false);
            $('#btnGuardarCargando').prop("hidden", true);
            $('#btnGuardar').prop("hidden", false);
        },
        error: function (resultado) {
          
            CargarControlCoche();
            $("#btnGuardar").prop("disabled", false);
            $('#btnGuardarCargando').prop("hidden", true);
            $('#btnGuardar').prop("hidden", false);
            MensajeError(resultado.responseText, false);
            Nuevo();

        }
    });

}


