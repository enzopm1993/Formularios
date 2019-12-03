
$(document).ready(function () {
    ConsultarAuditoriaChange();

    function changeColor() {
        var dt = new Date();
        var hora = dt.getHours();
        var minutos = dt.getMinutes();
        if (hora < 10) {
            hora = "0" + hora;
        }
        if (minutos < 10) {
            minutos = "0" + minutos;
        }
        var time = hora + ":" + minutos;//+ ":" + dt.getSeconds();   
        $("#HoraAuditoria").val(time);
    }
    setInterval(changeColor, 300000);
});



function LimpiarBoton() {
    $('#Cedula').val("");
    $('#Porcentaje').val("");
    $('#Nombre').val("");
    $('#txtObservacion').val("");
    $('#IdAuditoriaSangre').val("");
    $("#btnEliminarDetalle").prop("hidden", true);
    $("#TipoAuditoria").prop("selectedIndex", 0);
    
    
}
function ConsultarAuditoriaChange() {
    if ($('#FechaAuditoria').val() == "") {
        return;
    }
    $('#ControlAuditoriaSangre').html('');
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
        type: "GET",
        data:
        {
            Fecha: $('#FechaAuditoria').val()         
        },
        success: function (resultado) {
            LimpiarBoton();          
            $('#ControlAuditoriaSangre').html(resultado);
            config.opcionesDT.pageLength = 20;
            config.opcionesDT.order = [[1, "asc"]];
            $('#tblDataTable').DataTable(config.opcionesDT);
            $("#spinnerCargando").prop("hidden", true);
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function ValidarEmpleado() {
    var valida = true;
    if ($("#FechaAuditoria").val() == "") {
        $("#FechaAuditoria").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#FechaAuditoria").css('border-color', '#d1d3e2');
    }
    if ($("#HoraAuditoria").val() == "") {
        $("#HoraAuditoria").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#HoraAuditoria").css("border-color", "#d1d3e2");
    }
    if ($("#Lineas").val() == "") {
        $("#Lineas").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#Lineas").css("border-color", "#d1d3e2");
    }

    return valida;
}

function CargarEmpleados(formulario) {     

    if (!ValidarEmpleado()) {
        return;
    }
        $('#' + formulario).attr("disabled", true);
        $.ajax({
            url: "../AuditoriaSangre/EmpleadoBuscar",
            type: "Get",
            data:
            {
                Fecha: $("#FechaAuditoria").val(),
                Hora: $("#HoraAuditoria").val(),
                dsLinea: $('#Lineas').val()              
            },
            success: function (resultado) {
                $('#ModelCargarEmpleados').html(resultado);
                $("#ModalEmpleado").modal("show");
                $('#' + formulario).attr("disabled", false);
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
                $('#' + formulario).remove("disabled");
            }
        });
  
}

function Validar() {
    var valida = true;
    if ($("#FechaAuditoria").val() == "") {
        $("#FechaAuditoria").css("border-color", "#DC143C");
        valida=false;
    } else {
        $("#FechaAuditoria").css('border-color', '#d1d3e2');
    }
    if ($("#HoraAuditoria").val() == "") {
        $("#HoraAuditoria").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#HoraAuditoria").css("border-color", "#d1d3e2");
    }
    if ($("#Cedula").val() == "") {
        $("#Nombre").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#Nombre").css("border-color", "#d1d3e2");
    }
    if ($("#Porcentaje").val() == "") {
        $("#Porcentaje").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#Porcentaje").css("border-color", "#d1d3e2");
    }

    if ($("#TipoAuditoria").val() == "") {
        $("#TipoAuditoria").css("border-color", "#DC143C");
        valida = false;
    } else {
        $("#TipoAuditoria").css("border-color", "#d1d3e2");
    }

    return valida;
}

function IngresarAuditoriaSangre() {

    if (!Validar()) {
        return;
    }
    
    $("#Agregar").prop("disabled", true);
    
        $.ajax({
            url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
            type: "POST",
            data:
            {
                IdAuditoria: $('#IdAuditoriaSangre').val(),
                Cedula: $('#Cedula').val(),
                Porcentaje: $('#Porcentaje').val(),
                Fecha: $('#FechaAuditoria').val(),
                Hora: $('#HoraAuditoria').val(),
                TipoAuditoria: $("#TipoAuditoria").val(),
                Estado: 'A',
                Observacion:$("#txtObservacion").val()
            },
            success: function (resultado) {
                LimpiarBoton();             
                $("#btnEliminarDetalle").prop("hidden", true);        
                MensajeCorrecto("Registro ingresado con éxito", false);
    
                $("#Agregar").prop("disabled", false);
                ConsultarAuditoriaChange();
            },
            error: function (resultado) {
                MensajeError(JSON.stringify(resultado), false);
          
                $("#Agregar").prop("disabled", false);

            }
        });
    //}   
}

function InactivarDetalle() {
    if ($("#txtObservaccionEliminacion").val() =='') {
        $("#txtObservaccionEliminacion").css("border-color", "#DC143C");
        return;
    }
    $('#ModalObservacion').modal("hide");
    $.ajax({
        url: "../AuditoriaSangre/ControlAuditoriaSangrePartial",
        type: "POST",
        data:
        {
            IdAuditoria: $('#IdAuditoriaSangre').val(),
            Cedula: $('#Cedula').val(),
            Porcentaje: $('#Porcentaje').val(),
            Fecha: $('#FechaAuditoria').val(),
            Hora: $('#HoraAuditoria').val(),
            Estado: "I"
        },
        success: function (resultado) {
            LimpiarBoton();
           // $('#ControlAuditoriaSangre').html(resultado);
            MensajeCorrecto("Registro ingresado con éxito", false);
            ConsultarAuditoriaChange();
        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);

        }
    });

}


function CargarAuditoria(model) { 
    $('#IdAuditoriaSangre').val(model.IdControlAuditoriaSangre);
    $('#Nombre').val(model.Nombres);   
    $('#Cedula').val(model.Cedula);
    $('#Porcentaje').val(model.Porcentaje);    
    $('#TipoAuditoria').val(model.Tipo);    
    $("#btnEliminarDetalle").prop("hidden", false);
    $("#txtObservacion").val(model.Observacion);
}


function Observacion() {
    // console.log(valor);
    $("#txtObservaccionEliminacion").val(""); 
    $("#txtObservaccionEliminacion").css("border-color", "#d1d3e2");
    $('#ModalObservacion').modal("show");
}
