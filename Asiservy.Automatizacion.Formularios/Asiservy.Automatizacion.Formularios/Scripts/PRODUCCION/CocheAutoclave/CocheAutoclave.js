var DatosOF = [];
var DatosCabecera = [];

$(document).ready(function () {
    CargarCocheAutoclave();
    CargarOrdenFabricacion();  
});

function CargarCocheAutoclave() {
    $("#chartCabecera2").html('');
    if ($("#txtFecha").val() == '' || $("#txtTurno").val() =='' ) {
        return;
    }
    if (moment($("#txtFecha").val()).format("YYYY-MM-DD") > moment().format("YYYY-MM-DD")) {
        $("#txtFecha").val("");
        MensajeAdvertencia("Fecha no permitida");
        return;
    }

    $("#spinnerCargando").prop("hidden", false);   
    $.ajax({
        url: "../CocheAutoclave/CocheAutoclavePartial",
        type: "GET",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#txtTurno").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#divCabecera2").prop("hidden", false);
            if (resultado == "0") {
                $("#chartCabecera2").html("No existen registros");
                $("#spinnerCargando").prop("hidden", true);
            } else {
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
    $("#txtOrdenFabricacion").val('');
    $("#txtObservacion").val('');
    $("#txtAutoclave").val('');
    $("#txtParada").val('');
    $("#txtCodProducto").val('');
    $("#txtLote").val('');
    $("#txtIdControl").val("0");   
    $("#txtFecha").css('borderColor', '#ced4da');
    $("#txtTurno").css('borderColor', '#ced4da');
    $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    $("#txtAutoclave").css('borderColor', '#ced4da');
    $("#txtParada").css('borderColor', '#ced4da');
    $("#txtLote").css('borderColor', '#ced4da');
    $("#txtCodProducto").css('borderColor', '#ced4da');
}

function Validar() {
    var valida = true;
    if ($("#txtFecha").val() == "") {
        $("#txtFecha").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtFecha").css('borderColor', '#ced4da');
    }

    if ($("#txtTurno").val() == "") {
        $("#txtTurno").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTurno").css('borderColor', '#ced4da');
    }

    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }

    if ($("#txtAutoclave").val() == "") {
        $("#txtAutoclave").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAutoclave").css('borderColor', '#ced4da');
    }

    if ($("#txtParada").val() == "") {
        $("#txtParada").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada").css('borderColor', '#ced4da');
    }

    if ($("#txtLote").val() == "") {
        $("#txtLote").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLote").css('borderColor', '#ced4da');
    }

    if ($("#txtCodProducto").val() == "") {
        $("#txtCodProducto").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodProducto").css('borderColor', '#ced4da');
    }

    return valida;

}

function GuardarCocheAutoclave() {
    if (!Validar()) {
        return;
    }

    $.ajax({
        url: "../CocheAutoclave/CocheAutoclave",
        type: "POST",
        data: {
            Fecha: $("#txtFecha").val(),
            Turno: $("#txtTurno").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Autoclave: $("#txtAutoclave").val(),
            Parada: $("#txtParada").val(),
            Producto: DatosOF.NOMBRE_PRODUCTO,
            CodigoProducto: $("#txtCodProducto").val(),
            Envase: DatosOF.ENVASE,
            Lote: $("#txtLote").val(),
            Observacion: $("#txtObservacion").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            CargarCocheAutoclave();
            NuevoControl();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
           
        }
    });
}
function CargarDatosOrdenFabricacion() {
    if ($("#txtOrdenFabricacion").val() == "") {      
        return;
    }
    DatosOF = [];
    $.ajax({
        url: "../General/ConsultarDatosOrdenFabricacion",
        type: "GET",
        data: {
            Orden: $("#txtOrdenFabricacion").val()          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (resultado == "1") {
                MensajeAdvertencia("No se pudo obtener información");
                return;
            }          
            $("#txtProducto").val(resultado.NOMBRE_PRODUCTO);
            DatosOF = resultado;          
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);         
        }
    });

}

$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});

$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").val() == '') {
        $('#validaOrden').prop("hidden", false);        
        return;
    }
    $("#txtOrdenFabricacion").val($("#SelectOrdenFabricacion").val());
    CargarDatosOrdenFabricacion();   
    //CargarLotes();
  //  CargarLotes($("#txtOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});

function CargarOrdenFabricacion() {
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    valor = $("#txtFechaOrden").val();
    if (valor == '' || valor == null)
        return;

    $.ajax({
        url: "../General/ConsultaOFNivel2",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("No existen ordenes para esa fecha");
                return;
            }
            // LimpiarDetalle();
            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                    $("#SelectOrdenFabricacion").append("<option value='" + row.ORDEN_FABRICACION + "'>" + row.ORDEN_FABRICACION + "</option>")
                });
                $('#validaFecha').prop("hidden", true);
            } else {
                $('#validaFecha').prop("hidden", false);
            }
            //CargarControlHueso();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}

function ModalEditarControl() {
    $("#ModalEditarControl").modal("show");
}

function ValidarEditar() {
    var valida = true;
  
    if ($("#txtAutoclave2").val() == "") {
        $("#txtAutoclave2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtAutoclave2").css('borderColor', '#ced4da');
    }

    if ($("#txtParada2").val() == "") {
        $("#txtParada2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtParada2").css('borderColor', '#ced4da');
    }

    if ($("#txtLote2").val() == "") {
        $("#txtLote2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtLote2").css('borderColor', '#ced4da');
    }

    if ($("#txtCodProducto2").val() == "") {
        $("#txtCodProducto2").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtCodProducto2").css('borderColor', '#ced4da');
    }

    return valida;
}

function EditarCocheAutoclave() {
    if (!ValidarEditar()) {
        return;
    }

    $.ajax({
        url: "../CocheAutoclave/CocheAutoclave",
        type: "POST",
        data: {
            IdCocheAutoclave: $("#txtIdCocheAutoclave").val(),
            OrdenFabricacion: DatosCabecera.OrdenFabricacion,          
            Autoclave: $("#txtAutoclave2").val(),
            Parada: $("#txtParada2").val(),
            Producto: DatosCabecera.Producto,
            CodigoProducto: $("#txtCodProducto2").val(),
            Envase: DatosCabecera.Envase,
            Lote: $("#txtLote2").val(),
            Observacion: $("#txtObservacion2").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            $("#ModalEditarControl").modal("hide");
            $("#txtDescripcionCabecera").html(DatosCabecera.Producto + ", " + DatosCabecera.OrdenFabricacion + ", " + DatosCabecera.Autoclave + "-" + DatosCabecera.Parada);
            //CargarCocheAutoclave();
            //NuevoControl();
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function InactivarControl() {
    $.ajax({
        url: "../CocheAutoclave/EliminarCocheAutoclave",
        type: "POST",
        data: {
            IdCocheAutoclave: $("#txtEliminarDetalle").val()
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
    $("#txtEliminarDetalle").val(DatosCabecera.IdCocheAutoclave);
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



function SeleccionarControl(model) {
    //console.log(model);
    $("#divCabecera").prop("hidden", true);
    $("#divCabecera2").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);
    $("#btnAtras").prop("hidden", false);
    $("#divDetalle").prop("hidden", false);
    $("#btnEditar").prop("hidden", false);
    $("#btnEliminar").prop("hidden", false);
    $("#txtIdCocheAutoclave").val(model.IdCocheAutoclave);
    $("#txtAutoclave2").val(model.Autoclave);
    $("#txtParada2").val(model.Parada);
    $("#txtCodProducto2").val(model.CodigoProducto);
    $("#txtLote2").val(model.Lote);
    $("#txtObservacion2").val(model.Observacion);
    $("#txtIdCocheAutoclave").val(model.IdCocheAutoclave);
    $("#txtDescripcionCabecera").html(model.Producto + ", " + model.OrdenFabricacion + ", " + model.Autoclave + "-" + model.Parada);
    DatosCabecera = model;
    CargarCocheAutoclaveDetalle();
}

function AtrasControlPrincipal() {
    $("#divCabecera").prop("hidden", false);
    $("#divCabecera2").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    $("#btnAtras").prop("hidden", true);
    $("#divDetalle").prop("hidden", true);
    $("#btnEditar").prop("hidden", true);
    $("#btnEliminar").prop("hidden", true);
    $("#txtIdCocheAutoclave").val('0');
    DatosCabecera = [];
    NuevoControl();
    CargarCocheAutoclave();
}


///********************************DETALLE************************************************
function NuevoDetalle() {
    $("#txtTarjeta").val('');
    $("#txtHoraInicio").val('');
    $("#txtCoche").val('');
    $("#txtLinea").val('');
    $("#txtIdCocheAutoclaveDetalle").val('0');
}

function ModalGenerarDetalle() {
    NuevoDetalle();
    $("#ModalCocheAutoclaveDetalle").modal("show");
}

function EditarCocheAutoclaveDetalle(model) {
    //console.log(model);
    $("#txtTarjeta").val(model.Tarjeta);
    $("#txtHoraInicio").val(model.HoraInicio);
    $("#txtCoche").val(model.Coche);
    $("#txtLinea").val(model.LineaProduccion);
    $("#txtIdCocheAutoclaveDetalle").val(model.IdCocheAutoclaveDetalle);
    $("#ModalCocheAutoclaveDetalle").modal("show");
}

function ValidarDetalle() {
    var valida = true;

    if ($("#txtTarjeta").val() == "") {
        $("#txtTarjeta").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtTarjeta").css('borderColor', '#ced4da');
    }

    if ($("#txtHoraInicio").val() == "") {
        $("#txtHoraInicio").css('borderColor', '#FA8072');
        valida = false;
    } else {
        $("#txtHoraInicio").css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarCocheAutoclaveDetalle() {
    if (!ValidarDetalle()) {
        return;
    }
    var fecha1 = moment(DatosCabecera.Fecha).add(1, 'days').format('YYYY-MM-DD');
    var fecha2 = moment($("#txtHoraInicio").val()).format('YYYY-MM-DD');
   // console.log(fecha1);
   //s console.log(fecha2);
    if (fecha2 > fecha1) {
        MensajeAdvertencia("No puede ingresar una fecha mayor a: " + fecha1);
        return;
    }


    $.ajax({
        url: "../CocheAutoclave/CocheAutoclaveDetalle",
        type: "POST",
        data: {
            IdCocheAutoclave: DatosCabecera.IdCocheAutoclave,
            IdCocheAutoclaveDetalle: $("#txtIdCocheAutoclaveDetalle").val(),
            Tarjeta: $("#txtTarjeta").val(),
            HoraInicio: $("#txtHoraInicio").val(),
            Coche: $("#txtCoche").val(),
            LineaProduccion: $("#txtLinea").val()
           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            $("#ModalCocheAutoclaveDetalle").modal("hide");
            CargarCocheAutoclaveDetalle();
            NuevoDetalle();

        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);
        }
    });
}


function CargarCocheAutoclaveDetalle() {
    $("#divTableDetalle").html('');   
    $("#spinnerCargandoDetalle").prop("hidden", false);
    $.ajax({
        url: "../CocheAutoclave/CocheAutoclaveDetallePartial",
        type: "GET",
        data: {
            IdControl: DatosCabecera.IdCocheAutoclave            
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }           
            $("#spinnerCargandoDetalle").prop("hidden", true);
            if (resultado == "0") {
                $("#divTableDetalle").html("No existen registros");
            } else {
                $("#divTableDetalle").html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            //  $('#btnConsultar').prop("disabled", true);
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);    
            $("#spinnerCargandoDetalle").prop("hidden", true);
        }
    });
}

function InactivarCocheAutoclaveDetalle() {
    $.ajax({
        url: "../CocheAutoclave/EliminarCocheAutoclaveDetalle",
        type: "POST",
        data: {
            IdCocheAutoclaveDetalle: $("#txtEliminarModalDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Faltan Parametros");
            }
            CargarCocheAutoclaveDetalle();
            $("#modalEliminarControlDetalle").modal("hide");
        },
        error: function (resultado) {
            MensajeError("Error: Comuníquese con sistemas", false);    
        }
    });
}

function EliminarCocheAutoclaveDetalle(model) {
    $("#txtEliminarModalDetalle").val(model.IdCocheAutoclaveDetalle);
    $("#txtDetallep").html("Parada -> " + model.Tarjeta);
    //$("#pModalDetalle").html("Hora: " + moment(model.HoraInicio).format('HH:mm') + ' - ' + moment(model.HoraFin).format('HH:mm'));
    $("#modalEliminarControlDetalle").modal('show');
}

$("#modal-detalle-si").on("click", function () {
    InactivarCocheAutoclaveDetalle();
    $("#modalEliminarControlDetalle").modal('hide');
});

$("#modal-detalle-no").on("click", function () {
    $("#modalEliminarControlDetalle").modal('hide');
});