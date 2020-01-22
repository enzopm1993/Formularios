


$(document).ready(function () {
    $("#DivHora").hide();
    CargarEmpleados($("#txtCodLinea").val());
});

function SeleccionarTodos(check) {  
    $("input[type=checkbox]").each(function (resultado) {
        id = $(this).attr("id");
        if (id !="switchHoraFecha")
        {
            $("#" + this.id).prop('checked', check);
        }
    });
}


function CargarEmpleados(linea) {

    $("#spinnerCargando").prop("hidden", false);
    if (linea == "" || linea == null) {
        return;
    }

    $.ajax({
        url: "../Empleado/EmpleadosMasivoPartial",
        type: "Get",
        data: {
            Linea: linea
        },
        success: function (resultado) {
            if (resultado == 0) {
                MensajeAdvertencia("No Existen Empleados para esta Linea");
            } else {
                
                $("#spinnerCargando").prop("hidden", true);
                $("#DivTblEmpleados").html(resultado);
            }
        },
        error: function (resultado) {
            //console.log(resultado);
            $("#spinnerCargando").prop("hidden", true);
            $("#btnGuardar").prop("hidden", false);
            MensajeError(resultado.responseText, false);
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


function Guardar() {
   
    var result = new Array();
    var CodigoMotivo = $("#selectMotivo").val();
    var Observacion = $("#Observacion").val();
    var FechaSalidaEntrada = $("#dateSalidaRegreso").val();
    var HoraSalida = $("#timeHoraSalida").val();
    var HoraRegreso = $("#timeHoraRegreso").val();
    var FechaSalida = $("#dateSalida").val();
    var FechaRegreso = $("#dateRegreso").val();
    var CodLinea = $("#txtCodLinea").val();

    if ($("#selectMotivo").val() == "") {
        $("#ValidaMotivo").prop("hidden", false);
        return;
    } else {
        $("#ValidaMotivo").prop("hidden", true);
    } 

    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        id = id.replace('Empleado-', '');
       // console.log(id);
        if (id != "Empleado" && id != "switchHoraFecha") {            
            result.push(id);
        }
    });
       
    if (result.length == 0) {
        $("#ValidaEmpleado").prop("hidden", false);
        return;
    } else {
        $("#ValidaEmpleado").prop("hidden", true);
    }


   // console.log(result);
    $("#spinnerCargando").prop("hidden", false);
    $("#btnGuardar").prop("hidden", true);
    $.ajax({
        url: "../SolicitudPermiso/SolicitudPermisoMasivo",
        type: "POST",
        data: {
            CodigoMotivo: CodigoMotivo,
            Observacion: Observacion,
            FechaSalidaEntrada: FechaSalidaEntrada,
            HoraSalida: HoraSalida,
            HoraRegreso: HoraRegreso,
            FechaSalida: FechaSalida,
            FechaRegreso: FechaRegreso,
            CodigoLinea: CodLinea,
            Cedulas: result
        },
        success: function (resultado) {
            if (resultado.Codigo == 0) {
                MensajeAdvertencia(resultado.Mensaje);
            } else {
                MensajeCorrecto(resultado.Mensaje)               
                $("#h3Mensaje").html(resultado.Mensaje);
            }
            $("#spinnerCargando").prop("hidden", true);
          //  $("#spinnerCargando").prop("hidden", true);
        },
        error: function (resultado) {
            //console.log(resultado);
            $("#spinnerCargando").prop("hidden", true);
            $("#btnGuardar").prop("hidden", false);
            MensajeError(resultado.responseText, false);
        }
    });



}


var modalConfirm = function (callback) {   
    $("#btnGuardar").on("click", function () {
        var contador = 0;

        $("#tblDataTable").DataTable().destroy();
        $("input[type=checkbox]:checked").each(function (resultado) {
            id = $(this).attr("id");
            id = id.replace('Empleado-', '');           
            if (id != "Empleado" && id != "switchHoraFecha") {
               // result.push(id);
                contador++;
            }
        });
        $("#myModalLabel").html("Generar " + contador+" Solicitudes?")
        $("#mi-modal").modal('show');
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
        Guardar();
    } 
});