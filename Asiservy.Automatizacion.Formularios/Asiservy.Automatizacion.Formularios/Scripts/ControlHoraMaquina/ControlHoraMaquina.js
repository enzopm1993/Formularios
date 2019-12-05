
$(document).ready(function () {
    //ValidaProyeccion();
   
    CargarControlHoraMaquina();
});

function NuevoControlHoraMaquina() {
    $("#SelectOrdenFabricacion").prop("selectedIndex", 0);  
    $('#txtOrdenFabricacion').val("");   
    LimpiarDetalle();
}

function CargarOrdenFabricacion() {
    valor = $("#txtFechaOrden").val();
    $('#txtOrdenFabricacion').val("");   

    if (valor == '' || valor == null)
        return;
    $("#SelectOrdenFabricacion").empty();
    $("#SelectOrdenFabricacion").append("<option value='' >-- Seleccionar Opción--</option>");
    $.ajax({
        url: "../ControlHoraMaquina/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            LimpiarDetalle();
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
            MensajeError(resultado.responseText, false);
        }
    });
}

function LimpiarDetalle(){
    $("#txtOrdenVenta").val('');
    $("#txtCodigoProducto").val('');
    $("#txtProducto").val('');
    $("#txtPesoNeto").val('');
    $("#txtLineaNegocio").val('');
    $("#txtCodigoCliente").val('');
    $("#txtCliente").val('');
}

function CargarOrdenFabricacionDetalle(orden) {
    LimpiarDetalle();
    valor = $("#txtFechaProduccion").val();
    if (valor == '' || valor == null)
        return;
    if ($("#txtOrdenFabricacion").val() == '')
        return;

    $.ajax({
        url: "../ControlHoraMaquina/ConsultarOrdenesFabricacion",
        type: "GET",
        data: {
            Fecha: valor,
            Orden: orden
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }

            if (!$.isEmptyObject(resultado)) {
                $.each(resultado, function (create, row) {
                   // $("#SelectOrdenFabricacion").append("<option value='" + row.ORDEN_FABRICACION + "'>" + row.ORDEN_FABRICACION + "</option>")

                    if (row.OrdenVenta == '0') {
                        $("#txtOrdenVenta").val('0');
                        $("#txtCodigoCliente").val('0');
                        $("#txtCliente").val('Libre de uso');
                    } else {
                        $("#txtOrdenVenta").val(row.OrdenVenta);
                        $("#txtCodigoCliente").val(row.CodigoCliente);
                        $("#txtCliente").val(row.NombreCliente);
                    }
                    
                    $("#txtCodigoProducto").val(row.ItemCode);
                    $("#txtProducto").val(row.ItemName);
                    $("#txtPesoNeto").val(parseInt(row.PesoNeto));
                    $("#txtLineaNegocio").val(row.LineaNegocio);

                });
                $('#validaFecha').prop("hidden", true);
            }             
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}




function CargarControlHoraMaquina() {  
    $("#DivTableControl").html('');
    $("#DivMensaje").html('');
    if ($("#txtFechaProduccion").val() == "") {
        $("#txtFechaProduccion").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtFechaProduccion").css('borderColor', '#ced4da');
    }  
    if ($("#txtTurno").val() == "") {
        $("#txtTurno").css('borderColor', '#FA8072');
        return;
    } else {
        $("#txtTurno").css('borderColor', '#ced4da');
    } 

    $("#spinnerCargando").prop("hidden", false);
   
    $.ajax({
        url: "../ControlHoraMaquina/ControlHoraMaquinaPartial",
        type: "GET",
        data:
        {
            Fecha: $('#txtFechaProduccion').val(),
            Turno: $('#txtTurno').val()
            
          
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarOrdenFabricacion();
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableControl").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}

function Validar() {
    var bool = true;

    
    if ($("#txtFechaProduccion").val() == "") {
        $("#txtFechaProduccion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtFechaProduccion").css('borderColor', '#ced4da');
    }
    if ($("#txtOrdenVenta").val() == "") {
        $("#txtOrdenVenta").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtOrdenVenta").css('borderColor', '#ced4da');
    }
    if ($("#txtOrdenFabricacion").val() == "") {
        $("#txtOrdenFabricacion").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtOrdenFabricacion").css('borderColor', '#ced4da');
    }
    if ($("#txtCliente").val() == "") {
        $("#txtCliente").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtCliente").css('borderColor', '#ced4da');
    }
    if ($("#txtLineaNegocio").val() == "") {
        $("#txtLineaNegocio").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtLineaNegocio").css('borderColor', '#ced4da');
    }
    if ($("#txtPesoNeto").val() == "") {
        $("#txtPesoNeto").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtPesoNeto").css('borderColor', '#ced4da');
    }
    if ($("#txtProducto").val() == "") {
        $("#txtProducto").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtProducto").css('borderColor', '#ced4da');
    }
    if ($("#txtTurno").val() == "") {
        $("#txtTurno").css('borderColor', '#FA8072');
        bool = false;
    } else {
        $("#txtTurno").css('borderColor', '#ced4da');    
    }
    return bool;
}
function GuardarControlHoraMaquina() {
    if (!Validar())
        return;
    $("#btnGuardar").prop("disabled", true);
    $('#spinnerCargando').prop("hidden", false);
    var DivControl = $('#DivTableControl');
    DivControl.html('');
    $.ajax({
        url: "../ControlHoraMaquina/ControlHoraMaquina",
        type: "POST",
        data: {
            IdControlHoraMaquina: $("#txtIdControlHoraMaquina").val(),
            OrdenVenta: $("#txtOrdenVenta").val(),
            OrdenFabricacion: $("#txtOrdenFabricacion").val(),
            Turno: $("#txtTurno").val(),
            Cliente: $("#txtCliente").val(),
            LineaNegocio: $("#txtLineaNegocio").val(),
            CodigoProducto: $("#txtCodigoProducto").val(),
            Producto: $("#txtProducto").val(),
            PesoNeto: $("#txtPesoNeto").val(),
            Fecha: $("#txtFechaProduccion").val()
        },
        success: function (resultado) {
            if (resultado == '0') {
                MensajeAdvertencia("Faltan Parametros");
            } else {
                CargarControlHoraMaquina();
                MensajeCorrecto(resultado);
                NuevoControlHoraMaquina();
            }           
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlHoraMaquina();
            $("#btnGuardar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
            MensajeError(resultado.responseText, false);
            NuevoControlHoraMaquina();

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
    CargarOrdenFabricacionDetalle($("#SelectOrdenFabricacion").val());
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);
});

$("#modal-orden-no").on("click", function () {
    $("#ModalOrdenes").modal('hide');
});

function InactivarControl() {
    $.ajax({
        url: "../ControlHoraMaquina/EliminarControlHoraMaquina",
        type: "Get",
        data: {
            idControl: $("#txtIdControlHoraMaquina").val()
        },
        success: function (resultado) {
           NuevoControlHoraMaquina();
            Atras();
            //CargarControlHoraMaquina();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            CargarControlHoraMaquina();

        }
    });
}

$("#btnInactivar").on("click", function () {  
    $("#mi-modal").modal('show');
});

$("#modal-btn-si").on("click", function () {   
      InactivarControl();   
    $("#mi-modal").modal('hide');
});

$("#modal-btn-no").on("click", function () {
    $("#txtEliminar").val('');
    $("#mi-modal").modal('hide');
});

function Atras() {
    $("#divCabeceraControl").fadeIn();
    $("#btnInactivar").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#btnNuevo").prop("hidden", false);
    $("#btnGenerar").prop("hidden", false);
    CargarControlHoraMaquina();
}

function seleccionarControlHoraMaquina(model) {
    LimpiarDetalle();

    $("#divCabeceraControl").fadeOut("swing");
    $("#btnInactivar").prop("hidden", false);
    $("#btnAtras").prop("hidden", false);
    $("#btnNuevo").prop("hidden", true);
    $("#btnGenerar").prop("hidden", true);

    $("#txtIdControlHoraMaquina").val(model.IdControlHoraMaquina);
    $("#txtOrdenFabricacion").val(model.OrdenFabricacion);
    $("#txtOrdenVenta").val(model.OrdenVenta);
    $("#txtLineaNegocio").val(model.LineaNegocio);
    $("#txtProducto").val(model.Producto);
    $("#txtPesoNeto").val(model.PesoNeto);
    $("#txtCliente").val(model.Cliente);
    $("#DivMensaje").prop("hidden", false);
    $("#DivMensaje").html('');
    $("#DivMensaje").html(model.Producto + ", " + model.PesoNeto+ ", " + model.Cliente);
   CargarControlHoraMaquinaDetalle();   
}



function CargarControlHoraMaquinaDetalle() {
    $("#DivTableControl").html('');
   
    $("#spinnerCargando").prop("hidden", false);
    $.ajax({
        url: "../ControlHoraMaquina/ControlHoraMaquinaDetallePartial",
        type: "GET",
        data:
        {
            IdControl: $('#txtIdControlHoraMaquina').val()    
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarOrdenFabricacion();
            if (resultado == 0) {
                $("#DivMensaje").html("<h3 class'text-center'> No existen registros </h3> ");
            } else {
                $("#DivTableControl").html(resultado);
                config.opcionesDT.pageLength = 50;
                config.opcionesDT.order = [[0, "asc"]];
                $('#tblDataTable').DataTable(config.opcionesDT);
            }
            $("#spinnerCargando").prop("hidden", true);

        },
        error: function (resultado) {
            MensajeError(JSON.stringify(resultado), false);
            $("#spinnerCargando").prop("hidden", true);

        }
    });
}


function CalculoTiempo(autoclave) {

    var FechaInicio = "#txtFechaInicio-" + autoclave;
    var FechaFin = "#txtFechaFin-" + autoclave;
    var TotalHoras = "#txtTotalHoras-" + autoclave;
    if ($(FechaFin).val() == '' || $(FechaInicio).val() == '') {
        return;
    }


    var minutosDif = moment($(FechaFin).val()).diff(moment($(FechaInicio).val()), 'minutes')
    if (minutosDif < 0) {
        $(FechaFin).val($(FechaInicio).val());
        minutosDif = 0;
    }  
    var HorasDif = moment($(FechaFin).val()).diff(moment($(FechaInicio).val()), 'hours')
    minutosDif=minutosDif % 60;
    if (minutosDif < 10) {
        minutosDif = "0" + minutosDif;
    }
    if (HorasDif < 10) {
        HorasDif = "0" + HorasDif;
    }
    $(TotalHoras).val(HorasDif + ":" + minutosDif);

}


function check(id, autoclave) {
    if ($("#"+id).prop("checked")) {
        GuardarControlDetalle(id, autoclave);
    } else {
        EliminarControlDetalle(id, autoclave);
    }
}

function ValidarDetalle(autoclave) {
    var valida = true;
    var FechaInicio = "#txtFechaInicio-" + autoclave;
    var FechaFin = "#txtFechaFin-" + autoclave;
    var TotalHoras = "#txtTotalHoras-" + autoclave;
    if ($(FechaInicio).val() == "") {
        $(FechaInicio).css('borderColor', '#FA8072');
        bool = false;
    } else {
        $(FechaInicio).css('borderColor', '#ced4da');
    }

    if ($(FechaFin).val() == "") {
        $(FechaFin).css('borderColor', '#FA8072');
        bool = false;
    } else {
        $(FechaFin).css('borderColor', '#ced4da');
    }

    if ($(TotalHoras).val() == "") {
        $(TotalHoras).css('borderColor', '#FA8072');
        bool = false;
    } else {
        $(TotalHoras).css('borderColor', '#ced4da');
    }
    return valida;
}

function GuardarControlDetalle(id, autoclave) {

    if (!ValidarDetalle(autoclave))
        return;   
    var FechaInicio = "#txtFechaInicio-" + autoclave;
    var FechaFin = "#txtFechaFin-" + autoclave;
    var TotalHoras = "#txtTotalHoras-" + autoclave;
    var TotalCoche = "#txtTotalCoche-" + autoclave;
    var Observacion = "#txtObservacion-" + autoclave;

    $.ajax({
        url: "../ControlHoraMaquina/GuardarModificarControlHoraMaquinaDetalle",
        type: "POST",
        data: {
            IdControlHoraMaquina: $("#txtIdControlHoraMaquina").val(),
            Autoclave: autoclave,
            FechaInicio: $(FechaInicio).val(),
            FechaFin: $(FechaFin).val(),
            TotalHoras: $(TotalHoras).val(),
            TotalCoches: $(TotalCoche).val(),
            Observacion: $(Observacion).val()
           
        },
        success: function (resultado) {
            $(FechaInicio).prop("disabled", true);
            $(FechaFin).prop("disabled", true);
            $(TotalCoche).prop("disabled", true);
            $(Observacion).prop("disabled", true);

        },
        error: function (resultado) {           
            MensajeError(resultado.responseText, false);
            $(FechaInicio).prop("disabled", false);
            $(FechaFin).prop("disabled", false);
            $(TotalCoche).prop("disabled", false);
            $(Observacion).prop("disabled", false);
            $("#" + id).prop("checked", false)
        }
    });
}


function EliminarControlDetalle(id, autoclave) {   
    var FechaInicio = "#txtFechaInicio-" + autoclave;
    var FechaFin = "#txtFechaFin-" + autoclave;
    var TotalHoras = "#txtTotalHoras-" + autoclave;
    var TotalCoche = "#txtTotalCoche-" + autoclave;
    var Observacion = "#txtObservacion-" + autoclave;
    $.ajax({
        url: "../ControlHoraMaquina/EliminarControlHoraMaquinaDetalle",
        type: "Get",
        data: {            
            idControl: $("#txtIdControlHoraMaquina").val(),
            Autoclave: autoclave       
        },
        success: function (resultado) {
            $(FechaInicio).prop("disabled", false);
            $(FechaFin).prop("disabled", false);
            $(TotalCoche).prop("disabled", false);
            $(Observacion).prop("disabled", false);

        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
            $(FechaInicio).prop("disabled", true);
            $(FechaFin).prop("disabled", true);
            $(TotalCoche).prop("disabled", true);
            $(Observacion).prop("disabled", true);
            $("#" + id).prop("checked", true)
        }
    });
}

