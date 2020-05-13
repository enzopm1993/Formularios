
var DatosCabecera = [];

$(document).ready(function () {
    CargarResidualCloro();
  
});

function ConsultarPeliduvios() {
    $("#selectPeliduvio").empty();
    $("#selectPeliduvio").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../ResidualCloro/ConsultarPeliduvios",
        type: "GET",
        data: {          
            Area: $("#selectArea").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            console.log(resultado);
            if (resultado == "0") {
                MensajeAdvertencia("No se encontraron peliduvios asigandos a esta Area.")                
            } else {     
                if (!$.isEmptyObject(resultado)) {
                    $.each(resultado, function (create, row) {
                        $("#selectPeliduvio").append("<option value='" + row.IdMantenimientoPediluvio + "'>" + row.Descripcion + "</option>")
                    });
                }             
                
            }
           
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
           
        }
    });
}

function CargarResidualCloro() {
    $("#chartCabecera2").html('');
    $("#btnGenerar").prop("disabled", false);

    if ($("#txtFecha").val() == '' || $("#selectArea").val() == '') {
        $("#divCabecera2").prop("hidden", true);
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }
    $("#divCabecera2").prop("hidden", false);
    $("#spinnerCargando").prop("hidden", false);
    $("#lblAprobadoPendiente").html("");
    ConsultarPeliduvios();
    $.ajax({
        url: "../ResidualCloro/ResidualCloroPartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Area: $("#selectArea").val()
            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else if (resultado.Fecha!=null)
            {
                $("#lblAprobadoPendiente").html(Mensajes.Aprobado);
                $("#btnGenerar").prop("disabled", true);
            }
            else {
                $("#lblAprobadoPendiente").html(Mensajes.Pendiente);

                $("#spinnerCargando").prop("hidden", true);
                $("#chartCabecera2").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function NuevoControl() {    
    $("#txtObservacion").val('');   
    $("#txtHora").val('');   
    $("#selectArea").css('borderColor', '#ced4da');
}

function Validar() {
    var valida = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }
    
    if ($("#txtHora").val() == "") {
        $("#txtHora").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora").css('borderColor', '#ced4da');
    }
    if ($("#selectArea").val() == "") {
        $("#selectArea").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectArea").css('borderColor', '#ced4da');
    }

   
    return valida;

}

function GuardarControl() {
    if (!Validar()) {
        return;
    }
    var fecha1 = moment($("#txtFecha").val()).add(1, 'days').format('YYYY-MM-DD');
    var fecha2 = moment($("#txtHora").val()).format('YYYY-MM-DD');
    if (fecha2 > fecha1) {
        MensajeAdvertencia("No puede ingresar una fecha mayor a: " + fecha1);
        return;
    }
    $.ajax({
        url: "../ResidualCloro/ResidualCloro",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Hora: $("#txtHora").val(),
            CodArea: $("#selectArea").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            CargarResidualCloro();
            NuevoControl();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function SeleccionarControl(model) {
    //console.log(model);
    $("#divCabecera").prop("hidden", true);
    $("#divCabecera2").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);
    $("#divDetalle").prop("hidden", false);
    $("#btnEditar").prop("hidden", false);
    $("#btnEliminar").prop("hidden", false);

    $("#txtHora2").val(model.Hora);
    $("#txtObservacion2").val(model.Observacion);
    $("#txtIdResidualCloro").val(model.IdResidualCloro);

    $("#txtDescripcionCabecera").html(moment(model.Hora).format("YYYY/MM/DD HH:mm"));
    DatosCabecera = model;
    CargarResidualCloroDetalle();
}


function AtrasControlPrincipal() {
    $("#divCabecera").prop("hidden", false);
    $("#divCabecera2").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    $("#btnAtras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);
    $("#txtIdResidualCloro").val('0');
    DatosCabecera = [];
    NuevoControl();
    CargarResidualCloro();
}



function InactivarControl() {
    $.ajax({
        url: "../ResidualCloro/EliminarResidualCloro",
        type: "POST",
        data: {
            IdResidualCloro: $("#txtEliminarDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            AtrasControlPrincipal();
            $("#modalEliminarControl").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $('#btnConsultar').prop("disabled", false);
            $("#spinnerCargando").prop("hidden", true);
        }
    });
}

function EliminarControl() {
    $("#txtEliminarDetalle").val(DatosCabecera.IdResidualCloro);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControl").modal('show');
}

$("#modal-si").on("click", function () {
    InactivarControl();
    $("#modalEliminarControl").modal('hide');
});

$("#modal-no").on("click", function () {
    $("#modalEliminarControl").modal('hide');
});



function ModalEditarControl() {
    $("#ModalEditarControl").modal("show");
}

function ValidarEditar() {
    var valida = true;

    if ($("#txtHora2").val() == "") {
        $("#txtHora2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHora2").css('borderColor', '#ced4da');
    }      
    return valida;
}

function EditarResidualCloro() {
    if (!ValidarEditar()) {
        return;
    }

    $.ajax({
        url: "../ResidualCloro/ResidualCloro",
        type: "POST",
        data: {
            IdResidualCloro: $("#txtIdResidualCloro").val(),
            Hora: $("#txtHora2").val(),     
            Observacion: $("#txtObservacion2").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            $("#ModalEditarControl").modal("hide");
            $("#txtDescripcionCabecera").html(DatosCabecera.Hora);         
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

///////////////////////////////////////////DETALLE ///////////////////////////////////

function CargarResidualCloroDetalle() {
    $("#divTableDetalle").html('');
    $("#spinnerCargandoDetalle").prop("hidden", false);

    $.ajax({
        url: "../ResidualCloro/ResidualCloroDetallePartial",
        type: "GET",
        data: {
            IdControl: DatosCabecera.IdResidualCloro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
            } 
            else {
                $("#divTableDetalle").html(resultado);    

            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function NuevoDetalle() {
    $("#txtCantidad").val('0');   
    $("#selectPeliduvio").prop('selectedIndex', 0);   
    $("#selectPeliduvio").css('borderColor', '#ced4da');
}

function ModalGenerarDetalle() {
    NuevoDetalle();
    $("#ModalResidualCloroDetalle").modal("show");
}

function EditarResidualCloroDetalle(model) {
    //console.log(model);
    $("#txtCantidad").val(model.Cantidad);
    $("#selectPeliduvio").val(model.CodPeliduvio);
    $("#txtIdResidualCloroDetalle").val(model.IdResidualCloroDetalle);
    $("#ModalResidualCloroDetalle").modal("show");
}

function ValidarDetalle() {
    var valida = true;

    if ($("#txtCantidad").val() == "") {
        $("#txtCantidad").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCantidad").css('borderColor', '#ced4da');
    }

    if ($("#selectPeliduvio").val() == "") {
        $("#selectPeliduvio").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#selectPeliduvio").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarResidualCloroDetalle() {
    if (!ValidarDetalle()) {
        return;
    }
    //var fecha1 = moment(DatosCabecera.Fecha).add(1, 'days').format('YYYY-MM-DD');
    //var fecha2 = moment($("#txtHoraInicio").val()).format('YYYY-MM-DD');
    //// console.log(fecha1);
    ////s console.log(fecha2);
    //if (fecha2 > fecha1) {
    //    MensajeAdvertencia("No puede ingresar una fecha mayor a: " + fecha1);
    //    return;
    //}


    $.ajax({
        url: "../ResidualCloro/ResidualCloroDetalle",
        type: "POST",
        data: {
            IdResidualCloro: DatosCabecera.IdResidualCloro,
            IdResidualCloroDetalle: $("#txtIdResidualCloroDetalle").val(),
            CodPeliduvio: $("#selectPeliduvio").val(),
            Cantidad: $("#txtCantidad").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            $("#ModalResidualCloroDetalle").modal("hide");
            CargarResidualCloroDetalle();
            NuevoDetalle();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}



function InactivarResidualCloroDetalle() {
    $.ajax({
        url: "../ResidualCloro/EliminarResidualCloroDetalle",
        type: "POST",
        data: {
            IdResidualCloroDetalle: $("#txtEliminarModalDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarResidualCloroDetalle();
            $("#modalEliminarControlDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function EliminarResidualCloroDetalle(model) {
    $("#txtEliminarModalDetalle").val(model.IdResidualCloroDetalle);
    $("#txtDetallep").html("Pediluvio -> " + model.Pediluvio);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarResidualCloroDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});