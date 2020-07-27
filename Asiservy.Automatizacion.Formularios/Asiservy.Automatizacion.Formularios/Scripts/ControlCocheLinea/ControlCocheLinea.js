var Model = [];
var Proyeccion = [];
$(document).ready(function () {
    CargarControlCoche();
    $("#selectLote").select2({
        width: '100%'
    });
    $("#selectLinea").select2({
        width: '100%'
    });
    $("#selectTalla").select2({
        width: '100%'
    });
    $("#selectLoteModal").select2({
        width: '100%'
    });
    $("#selectLineaModal").select2({
        width: '100%'
    });
    $("#selectTallaModal").select2({
        width: '100%'
    });

});


function CargarControlCoche() {

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

    ConsultarLotes();
    $('#spinnerCargando').prop("hidden", false);
    var DivControl = $('#DivTableControlCoche');
    DivControl.html('');
    $.ajax({
        url: "../ControlCocheLinea/ControlCocheLineaPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            var DivControl = $('#DivTableControlCoche');
            $('#spinnerCargando').prop("hidden", true);  

            if (resultado.Codigo == 0) {
                $("#btnGuardar").prop("hidden", true);
                DivControl.html("<h2 class='text-center'>"+resultado.Mensaje+"<h2>");
                MensajeAdvertencia(resultado.Mensaje);
            } else {
                $("#btnGuardar").prop("hidden", false);
                DivControl.html(resultado);
                config.opcionesDT.pageLength = 5;
                config.opcionesDT.order = [[2, "desc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);

            }

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#spinnerCargando').prop("hidden", true);  
        }
    });

}

function ConsultarLotes() {
    $.ajax({
        url: "../ControlCocheLinea/ConsultaLotes",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#selectLote").empty();
            if (resultado == "0") {
                $("#selectLote").append('<option value="">No Existe Programación</option>');
            }
            $("#selectLote").append('<option value="">Seleccione</option>');
            $.each(resultado, function (key, registro) {
                $("#selectLote").append('<option value=' + registro.Lote + '>' + registro.Lote + '</option>');
              
            });
            Proyeccion = resultado;
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            
        }
    });
}


function CambiaLote() {
    if ($("#selectLote").val() != "") {
        var queryResult = Enumerable.From(Proyeccion)
            .Where(function (x) { return x.Lote == $("#selectLote").val() })
            .Select(function (x) { return x.Talla })
            .ToArray();
        console.log(queryResult);
        $("#selectTalla").empty();
        $.each(queryResult, function (key, registro) {
          $("#selectTalla").append('<option value=' + registro + '>' + registro + '</option>');
        });
    }

}

function Nuevo() {
    
    
    $("#txtHoraInicio").val("");
    $("#txtHoraFin").val("");
    $("#txtCoches").val("");
    $("#selectLineas").prop("selectedIndex",0);  
    $("#selectLote").prop("selectedIndex", 0).change();
    $("#selectTalla").empty();  
    $("#txtObservacion").val("");

    $("#txtHoraInicio").css('borderColor', '#ced4da');
    $("#txtHoraFin").css('borderColor', '#ced4da');
    $("#txtCoches").css('borderColor', '#ced4da');
    $("#selectLineas").css('borderColor', '#ced4da');
}



function validar() {
    var bool = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#txtHoraInicio").val() == "") {
        $("#txtHoraInicio").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtHoraInicio").css('borderColor', '#ced4da');
    }

    if ($("#txtHoraFin").val() == "") {
        $("#txtHoraFin").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtHoraFin").css('borderColor', '#ced4da');
    }

    if ($("#txtCoches").val() == "") {
        $("#txtCoches").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtCoches").css('borderColor', '#ced4da');
    }

    if ($("#selectLineas").val() == "") {
        $("#selectLineas").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectLineas").css('borderColor', '#ced4da');
    }

    if ($("#selectLote").val() == "") {
        $("#selectLote").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        bool = false;
    } else {
        $("#selectLote").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });

    }

    if ($("#selectTalla").val() == "") {
        $("#selectTalla").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        bool = false;
    } else {
        $("#selectTalla").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });

    }
       
    return bool;
}

function GuardarControl() {
    if (!validar())
        return;
    $("#btnGuardar").prop("disabled", true);
    $('#spinnerCargando').prop("hidden", false);
    var DivControl = $('#DivTableControlCoche');
    DivControl.html('');
    $.ajax({
        url: "../ControlCocheLinea/ControlCocheLinea",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            HoraInicio: $("#txtHoraInicio").val(),
            HoraFin: $("#txtHoraFin").val(),
            Coches: $("#txtCoches").val(),
            Linea: $("#selectLineas").val(),
            Lote: $("#selectLote").val(),
            Talla: $("#selectTalla").val(),
            Observacion: $("#txtObservacion").val(),
            Turno: $("#selectTurno").val()
        },
        success: function (resultado) {           
            CargarControlCoche();            
            MensajeCorrecto(resultado);
            Nuevo();          
            $("#btnGuardar").prop("disabled", false);
         
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
           // Nuevo();

        }
    });

}




function NuevoEditar() {
    $("#txtHoraInicioModal").val("");
    $("#txtHoraFinModal").val("");
    $("#txtCochesModal").val("");
    $("#selectLineasModal").prop("selectedIndex", 0);
    $("#selectLoteModal").prop("selectedIndex", 0).change();
    $("#selectTallaModal").prop("selectedIndex", 0).change();
    $("#txtObservacionModal").val("");

    $("#txtHoraInicioModal").css('borderColor', '#ced4da');
    $("#txtHoraFinModal").css('borderColor', '#ced4da');
    $("#txtCochesModal").css('borderColor', '#ced4da');
    $("#selectLineasModal").css('borderColor', '#ced4da');
}


function EditarControl(model) {
    Model = model;
    $("#ModalControl").modal("show");

    $("#txtFechaModal").val(moment(Model.Fecha).format("YYYY-MM-DD"));
    $("#txtHoraInicioModal").val(moment(Model.HoraInicio).format("YYYY-MM-DDThh:mm"));
    $("#txtHoraFinModal").val(moment(Model.HoraFin).format("YYYY-MM-DDThh:mm"));
    $("#txtCochesModal").val(Model.Coches);
    $("#selectLineasModal").val(Model.Linea);
    $("#txtLoteModal").val(Model.Lote);
    $("#txtTallaModal").val(Model.Talla);
    $("#txtObservacionModal").val(Model.Observacion);
    $("#txtTurnoModal").val($("#selectTurno option:selected").text());
} 




function ValidaEditar() {
    var bool = true;
    if ($("#txtHoraInicioModal").val() == "") {
        $("#txtHoraInicioModal").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtHoraInicioModal").css('borderColor', '#ced4da');
    }

    if ($("#txtHoraFinModal").val() == "") {
        $("#txtHoraFinModal").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtHoraFinModal").css('borderColor', '#ced4da');
    }

    if ($("#txtCochesModal").val() == "") {
        $("#txtCochesModal").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtCochesModal").css('borderColor', '#ced4da');
    }

    if ($("#selectLineasModal").val() == "") {
        $("#selectLineasModal").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#selectLineasModal").css('borderColor', '#ced4da');
    }

    if ($("#selectLoteModal").val() == "") {
        $("#selectLoteModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        bool = false;
    } else {
        $("#selectLoteModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });

    }

    if ($("#selectTallaModal").val() == "") {
        $("#selectTallaModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #FA8072');
        });

        bool = false;
    } else {
        $("#selectTallaModal").each(function () {
            $(this).siblings(".select2-container").css('border', '1px solid #ced4da');
        });

    }
    return bool;
}


function ModificarControl() {
    if (!ValidaEditar()) {
        return;
    }
  
    $.ajax({
        url: "../ControlCocheLinea/ControlCocheLinea",
        type: "POST",
        data: {
            IdControlCocheLinea: Model.IdControlCocheLinea,
            Fecha: $("#txtFecha").val(),
            HoraInicio: $("#txtHoraInicioModal").val(),
            HoraFin: $("#txtHoraFinModal").val(),
            Coches: $("#txtCochesModal").val(),
            Linea: $("#selectLineasModal").val(),
            Observacion: $("#txtObservacionModal").val(),
           
        },
        success: function (resultado) {
            CargarControlCoche();
            MensajeCorrecto(resultado);
            NuevoEditar();
            $("#ModalControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $("#ModalControl").modal("hide");
        }
    });

    //alert("generado");
}



function InactivarControl() {
    //console.log(model);
    $.ajax({
        url: "../ControlCocheLinea/EliminarControl",
        type: "POST",
        data: {
            IdControlCocheLinea: Model.IdControlCocheLinea,
            Fecha: Model.Fecha
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto("Registro eliminado con exito");
            CargarControlCoche();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarControl(model) {
    $("#modalEliminarControlDetalle").modal('show');
    Model = model;
}

$("#modal-detalle-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});
