
$(document).ready(function () {
    //ValidaProyeccion();

    CargarControlHoraMaquina();
});

function NuevoControlHoraMaquina() {
    $("#txtIdControlHoraMaquina").val('');
    $("#SelectOrdenFabricacion").prop("selectedIndex", 0);
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
                        $("#txtCliente").val('Libre Utilización');
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
    $("#txtFechaOrden").val($("#txtFechaProduccion").val());    

    
    var fecha = moment($("#txtFechaProduccion").val()).format("YYYY-MM-DDTHH:mm");   
    $("#txtFechaInicioDetalle").val(fecha);    
    $("#txtFechaFinDetalle").val(fecha);   

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
    $("#btnGenerar").prop("disabled", true);
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
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == '0') {
                MensajeAdvertencia("Faltan Parametros");
            } else {
                CargarControlHoraMaquina();
                MensajeCorrecto(resultado);
                NuevoControlHoraMaquina();
            }           
            $("#btnGenerar").prop("disabled", false);
            $('#spinnerCargando').prop("hidden", true);
        },
        error: function (resultado) {
            CargarControlHoraMaquina();
            $("#btnGenerar").prop("disabled", false);
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
    
    $("#divDetalleControl").fadeOut();
    $("#divCabeceraControl").fadeIn();
    $("#btnInactivar").prop("hidden", true);
   

    $("#btnAtras").prop("hidden", true);
    $("#btnNuevo").prop("hidden", true);
    $("#btnGenerar").prop("hidden", false);
    $("#btnGuardar").prop("hidden", true);
    $("#txtIdControlHoraMaquina").val('');
    
    CargarControlHoraMaquina();
}

function seleccionarControlHoraMaquina(model) {
    LimpiarDetalle();
    $("#divDetalleControl").fadeIn("swing");
    $("#divCabeceraControl").fadeOut("swing");
    $("#btnInactivar").prop("hidden", false);
   
    $("#btnAtras").prop("hidden", false);
    $("#btnNuevo").prop("hidden", false);
    $("#btnGenerar").prop("hidden", true);
    $("#btnGuardar").prop("hidden", false);
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
            IdControl: $('#txtIdControlHoraMaquina').val(),
            Fecha: $("#txtFechaProduccion").val()
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


function CalculoTiempo() {

    var FechaInicio = "#txtFechaInicioDetalle" ;
    var FechaFin = "#txtFechaFinDetalle";
    var TotalHoras = "#txtTotalHoras";
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


//function check(id, autoclave) {
//    if ($("#"+id).prop("checked")) {
//        GuardarControlDetalle(id, autoclave);
//    } else {
//        EliminarControlDetalle(id, autoclave);
//    }
//}


function LimpiarControlDetalle() {    
    $("#txtIdControlHoraMaquinaDetalle").val('0');   
    $("#txtTotalHoras").val('');
    $("#txtTotalCoche").val('');
    $("#txtObservacion").val('');
    var fecha = moment($("#txtFechaProduccion").val()).format("YYYY-MM-DDTHH:mm");
    $("#txtFechaInicioDetalle").val(fecha);
    $("#txtFechaFinDetalle").val(fecha);   
    $("#SelectAutoclave").prop("selectedIndex",0);   
    $("#btnInactivarDetalle").prop("hidden", true);    


}


function ValidarDetalle() {
    var valida = true;
    var FechaInicio = "#txtFechaInicioDetalle";
    var FechaFin = "#txtFechaFinDetalle";
    var TotalHoras = "#txtTotalHoras";
    var Autoclave = "#SelectAutoclave";

    if ($(FechaInicio).val() == "") {
        $(FechaInicio).css('borderColor', '#FA8072');
        valida = false;
    } else {
        $(FechaInicio).css('borderColor', '#ced4da');
    }

    if ($(FechaFin).val() == "") {
        $(FechaFin).css('borderColor', '#FA8072');
        valida = false;
    } else {
        $(FechaFin).css('borderColor', '#ced4da');
    }

    if ($(TotalHoras).val() == "") {
        $(TotalHoras).css('borderColor', '#FA8072');
        valida = false;
    } else {
        $(TotalHoras).css('borderColor', '#ced4da');
    }
    if ($(Autoclave).val() == "") {
        $(Autoclave).css('borderColor', '#FA8072');
        valida = false;
    } else {
        $(Autoclave).css('borderColor', '#ced4da');
    }

    return valida;
}

function GuardarControlHoraMaquinaDetalle() {

    if (!ValidarDetalle())
        return;   
    var FechaInicio = "#txtFechaInicioDetalle";
    var FechaFin = "#txtFechaFinDetalle";
    var TotalHoras = "#txtTotalHoras";
    var TotalCoche = "#txtTotalCoche";
    var Observacion = "#txtObservacion";
    var Autoclave = "#SelectAutoclave";
    $("#btnGuardar").prop("disabled", true);
    $.ajax({
        url: "../ControlHoraMaquina/GuardarModificarControlHoraMaquinaDetalle",
        type: "POST",
        data: {
            IdControlHoraMaquina: $("#txtIdControlHoraMaquina").val(),
            IdControlHoraMaquinaDetalle: $("#txtIdControlHoraMaquinaDetalle").val(),
            Autoclave: $(Autoclave).val(),
            FechaInicio: $(FechaInicio).val(),
            FechaFin: $(FechaFin).val(),
            TotalHoras: $(TotalHoras).val(),
            TotalCoches: $(TotalCoche).val(),
            Observacion: $(Observacion).val()
           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            $("#btnGuardar").prop("disabled", false);

            if (resultado == "0") {
                MensajeAdvertencia("Autoclave ya está siendo usado en esas horas");
                $("#txtFechaInicioDetalle").css('borderColor', '#FA8072');
                $("#txtFechaFinDetalle").css('borderColor', '#FA8072');
                return;
            }
            MensajeCorrecto(resultado);
            LimpiarControlDetalle();
            CargarControlHoraMaquinaDetalle();

        },
        error: function (resultado) {           
            $("#btnGuardar").prop("disabled", false);

            MensajeError(resultado.responseText, false);
           
        }
    });
}


function EliminarControlDetalle() {  
    $.ajax({
        url: "../ControlHoraMaquina/EliminarControlHoraMaquinaDetalle",
        type: "Get",
        data: {            
            idControl: $("#txtIdControlHoraMaquinaDetalle").val()
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            MensajeCorrecto(resultado);
            LimpiarControlDetalle();
            CargarControlHoraMaquinaDetalle();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);           
        }
    });
}


function SeleccionarControlDetalle(model) {
    $("#btnInactivarDetalle").prop("hidden", false);    
    $("#txtIdControlHoraMaquinaDetalle").val(model.IdControlHoraMaquinaDetalle);
    $("#txtTotalHoras").val(model.TotalHoras);
    $("#txtTotalCoche").val(model.TotalCoches);
    $("#txtObservacion").val(model.Observacion);   
    $("#txtFechaInicioDetalle").val(model.FechaInicio);
    $("#txtFechaFinDetalle").val(model.FechaFin);   
    $("#SelectAutoclave").val(model.Autoclave);   
}



$("#btnInactivarDetalle").on("click", function () {
    $("#mi-modal-detalle").modal('show');
});

$("#modal-btn-si-detalle").on("click", function () {
    EliminarControlDetalle();
    $("#mi-modal-detalle").modal('hide');
});

$("#modal-btn-no-detalle").on("click", function () {
    $("#txtEliminar").val('');
    $("#mi-modal-detalle").modal('hide');
});