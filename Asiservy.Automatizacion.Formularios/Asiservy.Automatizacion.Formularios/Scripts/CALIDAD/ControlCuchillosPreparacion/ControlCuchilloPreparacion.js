var datosCabecera = [];
var datosDetalle = [];
$(document).ready(function () {
    ListarControlCuchillos(0); 
    datosCabecera.IdControlCuchillo = 0;
    datosDetalle.IdControlCuchillo = 0;
    
    CambioEstado(false);
    $('#tblDataTableCargar tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableCargar').DataTable();
        var data = table.row(this).data();
        SeleccionarCabecera(data);
    });

    $('#tblDataTableCargarDetalle tbody').on('click', 'tr', function () {
        var table = $('#tblDataTableCargarDetalle').DataTable();
        var dataDetalle = table.row(this).data();
        SeleccionarDetalle(dataDetalle);
    });
});

function ListarControlCuchillos(opcion) {
    $("#divCargarCuchillosDetalle").prop("hidden", true);
    var op = opcion;
    var pFecha = moment($("#txtFecha").val()).format("YYYY-MM-DD");
    var table = $("#tblDataTableCargar");
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();  
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarControlCuchilloPreparacion",
        type: "GET",
        data: {
            fecha: pFecha,
            IdControlCuchillo: 0,
	        opcion: op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#txtObservacion").prop("disabled", true);
                $("#btnModalEliminar").show();
                $("#btnModalEditar").show();                
                $("#btnModalGenerarRegistro").hide();
                MensajeAdvertencia("¡No hay datos en la consulta!", false);
            }
            $('#MensajeRegistros').prop("hidden", true);
            for (var item in resultado) {
                resultado[item].Fecha = moment(resultado[item].Fecha).format("DD-MM-YYYY");
                resultado[item].Hora = moment(resultado[item].Hora).format("HH:mm");
            }
            $("#tblDataTableCargar tbody").empty();            
            config.opcionesDT.order = [];
            config.opcionesDT.columns = [
                { data: 'IdControlCuchillo' },
                { data: 'Fecha' },
                { data: 'Hora' },
                { data: 'Observacion' }
            ];           
            table.DataTable().destroy();
            table.DataTable(config.opcionesDT);
            table.DataTable().clear();
            table.DataTable().rows.add(resultado);
            table.DataTable().draw();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarModificarControlCuchilloPreparacion() {
    var pFecha = moment($("#txtFecha").val()).format("YYYY-MM-DD");
    var pHora = moment($("#txtFecha").val()).format("YYYY-MM-DDTHH:mm");
    if ($("#txtFecha").val() != '') {
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloPreparacion",
            type: "POST",
            data: {
                IdControlCuchillo: datosCabecera.IdControlCuchillo,
                Fecha: pFecha,
                Hora: pHora,
                Observacion: $("#txtObservacion").val()
            },
            success: function (resultado) {
                if (resultado == 0) {
                    limpiar();
                    ListarControlCuchillos(0);
                    MensajeCorrecto("Registro Exitoso");
                } else {
                    ListarControlCuchillos(0);
                    $("#divCargarCuchillosDetalle").prop('hidden',false);
                    MensajeCorrecto("Actualización Exitoso");
                }
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    } else {
        MensajeAdvertencia("!La FECHA  esta incorrecta¡");
    }       
}

function limpiar() {  
    $("#txtObservacion").val('');
    $("#txtObservacion").prop('disabled', false);
    $("#txtFecha").prop("disabled", false);
}

function SeleccionarCabecera(model) {
    datosCabecera = model;
    var table = $("#tblDataTableAprobar");
    table.DataTable().clear();
    var data = model;
    $("#divCargarCuchillosDetalle").prop("hidden", false);
    $("#divIngresarDetalleHora").prop("hidden",false);
    $("#btnModalGenerarRegistro").prop("hidden", true);
    $("#btnModalActualizarRegistro").prop("hidden", true);
    $("#btnModalEditar").prop("hidden", false);
    $("#btnModalEliminar").prop("hidden", false);
    $("#btnAtras").prop("hidden",false);
    $("#divCargarCuchillos").prop("hidden", true);
    $("#txtObservacion").val(data.Observacion);
    $("#txtObservacion").prop("disabled", true);
    var fecha = data.Fecha;
    var fechasplit = fecha.split('-');
    var fechaMMDDYYYY = fechasplit[1] + "-" + fechasplit[0] + "-" + fechasplit[2] + " " + data.Hora;    
    var convertDate = new Date(fechaMMDDYYYY);
    $("#txtFecha").val(moment(convertDate).format('YYYY-MM-DDTHH:mm'));
    $("#txtFecha").prop("disabled", true);  
    ConsultarControlCuchilloDetalle(0, datosCabecera.IdControlCuchillo, 0, 1);
}

function AtrasControlPrincipal() {
    limpiar();
    OcultaControles();
    $("#btnModalGenerarRegistro").prop("hidden", false);  
    datosCabecera = [];
    FechaNow();
}

function OcultaControles() {
    $("#btnModalGenerarRegistro").prop("hidden", true);   
    $("#btnModalActualizarRegistro").prop("hidden", true);   
    $("#divCargarCuchillosDetalle").prop("hidden",true);    
    $("#btnModalEditar").prop("hidden", true);
    $("#btnModalEliminar").prop("hidden", true);
    $("#btnAtras").prop("hidden", true);
    $("#divIngresarDetalleHora").prop("hidden", true);
    $("#divCargarCuchillos").prop("hidden", false);    
    $("#modalEliminarControl").modal("hide");    
}

function FechaNow() {
    var date = new Date();
    $("#txtFecha").val(moment(date).format('YYYY-MM-DDTHH:mm'));    
}

function EliminarConfirmar() {
    $("#modalEliminarControl").modal("show");
}

function EliminarCabeceraSi() {
    $.ajax({
        url: "../ControlCuchillosPreparacion/EliminarControlCuchilloPreparacion",
        type: "POST",
        data: {
            IdControlCuchillo: datosCabecera.IdControlCuchillo
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Parametro IdControlCuchillo es incorrecto: REGISTRO NO ELIMINADO");
                $("#modalEliminarControl").modal("hide");
                return;
            }
            limpiar();            
            OcultaControles();
            ListarControlCuchillos(0);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);            
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera() {
    OcultaControles();
    $("#txtObservacion").prop('disabled', false);
    $("#divCargarCuchillos").prop("hidden", true); 
    $("#btnModalActualizarRegistro").prop("hidden", false);
}

function ActualizarControlSi() {
    GuardarModificarControlCuchilloPreparacion();
    datosCabecera.Observacion = $("#txtObservacion").val();
    SeleccionarCabecera(datosCabecera);
    datosCabecera.Observacion = $("#txtObservacion").val();
    $("#btnModalActualizarRegistro").prop("hidden", true);
    $("#txtObservacion").prop('disabled', true);
    $("#txtFecha").prop('disabled', true);
}

//GUARDAR DETALLE
function GuardarControlDetalle() {
    var estado = $("#CheckEstadoRegistroOp").prop('checked');
    var idCuchillopreparacion = $('#txtCodigoCuchillo').val();
    var p=$('#txtCodigoCuchilloDetalle').val();
    if ($('#txtActualizar').val() == '1') {
        if ($('#txtCodigoCuchilloDetalle').val() != '') {
            idCuchillopreparacion = $('#txtCodigoCuchilloDetalle').val();
            estado = $("#CheckEstadoRegistroOpD").prop('checked');
        }
    } else {
        if ($('#txtCodigoCuchillo').val() == "") {
            MensajeAdvertencia('Seleccione el CUCHILLO ');
            return;
        }
    }    
    $.ajax({
        url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloDetalle",
        type: "POST",
        data: {
            IdControlCuchillo: datosCabecera.IdControlCuchillo,
            IdControlCuchilloDetalle: datosDetalle.IdControlCuchilloDetalle,
            IdCuchillopreparacion: idCuchillopreparacion,
            Estado: estado
        },
        success: function (resultado) {
            if (resultado == 0) {
                LimpiarDetalle();
                $("#ModalIngresoDetalle").modal('hide');
                $("#ModalEliminarActualizarDetalle").modal('hide');
               MensajeCorrecto("Registro Exitoso");
            } else {
                $("#ModalIngresoDetalle").modal('hide');
                $("#ModalEliminarActualizarDetalle").modal('hide');
                MensajeCorrecto("Actualización Exitoso");
            }
            ConsultarControlCuchilloDetalle(0, datosCabecera.IdControlCuchillo, 0, 1);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function ConsultarControlCuchilloDetalle(idCuchilloPreparacion, idControlCuchillo, idControlCuchilloDetalle, opcion) {
    var op = opcion;
    var idCuchillo = idCuchilloPreparacion;
    var idDetalle = idControlCuchilloDetalle;
    var idControl = idControlCuchillo;
    $('#txtActualizar').val('1');
    var table = $("#tblDataTableCargarDetalle");
    table.DataTable().destroy();
    table.DataTable().clear();
    table.DataTable().draw();
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarControlCuchilloDetalle",
        type: "GET",
        data: {
            IdCuchilloPreparacion: idCuchillo,
            IdControlCuchilloDetalle: idDetalle,
            IdControlCuchillo: idControl,
	        opcion:op
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
              
                MensajeAdvertencia("¡No hay datos en la consulta!", false);
            }
            $('#MensajeRegistros').prop("hidden", true);
           
            $("#tblDataTableCargarDetalle tbody").empty();
            configDetalle.opcionesDT.order = [];
            configDetalle.opcionesDT.columns = [
                { data: 'IdCuchilloPreparacion' },
                { data: 'Estado' },
                { data: 'UsuarioIngresoLog' },
                { data: 'UsuarioModificacionLog' }
                //{ data: 'UsuarioModificacionLog' }
            ];
            configDetalle.opcionesDT.aoColumnDefs = [{
                "aTargets": [1], // Columna a la que se quiere aplicar el css
                "mRender": function (data, type, full) {                    
                    var clscolor = "badge-danger";
                    var checked = '';
                    if (data == true) {
                        clscolor = "badge-success";    
                        checked = 'checked';
                    }
                    return '<center><span class="badge ' + clscolor + '"><input type="checkbox" ' + checked+' disabled id="vehicle2" name="Estado" value="Estado"></span></center>';
                       
                }
            }];
            //configDetalle.opcionesDT.aoColumn[
            //    {
            //        "aTargets": [5],
            //    "mData": "UsuarioModificacionLog",
            //        "mRender": function (data, type, full) {
            //            return '<button href="#"' + 'id="' + data + '">Edit</button>';
            //        }
            //    }
            //];
            table.DataTable().destroy();
            table.DataTable(configDetalle.opcionesDT);
            table.DataTable().clear();
            table.DataTable().rows.add(resultado);
            table.DataTable().draw();
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function LimpiarDetalle() {
    $('#LabelEstado').text('Estado Registro');
    $('#CheckEstadoRegistroOp').prop('checked', false);
    $('#txtCodigoCuchillo').prop('selectedIndex', 0);
}

function CambioEstado(valor) {
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');
}

function ModalGenerarDetalle() {
    LimpiarDetalle();
    $("#ModalIngresoDetalle").modal('show');
    $('#txtActualizar').val('');
    datosDetalle = [];
}

function SeleccionarDetalle(model) {
    datosDetalle = model;
    
    $("#txtCodigoCuchilloDetalle").val(datosDetalle.IdCuchilloPreparacion);
    $("#EstadoRegistroDetalle").val(datosDetalle.Estado);
    CambioEstadoDetalle(datosDetalle.Estado);
    $("#CheckEstadoRegistroOpD").prop('checked', datosDetalle.Estado);    
    $("#ModalEliminarActualizarDetalle").modal('show');
    //ConsultarControlCuchilloDetalle(0, datosCabecera.IdControlCuchillo, 0, 1);
}

function CambioEstadoDetalle(valor) {
    if (valor)
        $('#LabelEstadoDetalle').text('Activo');
    else
        $('#LabelEstadoDetalle').text('Inactivo');
}

function EliminarDetalleSi() {
    $.ajax({
        url: "../ControlCuchillosPreparacion/EliminarControlCuchilloDetalle",
        type: "POST",
        data: {
            IdControlCuchilloDetalle: datosDetalle.IdControlCuchilloDetalle
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Parametro idControlCuchilloDetalle es incorrecto: REGISTRO NO ELIMINADO");
                //$("#modalEliminarControl").modal("hide");
                return;
            }
            LimpiarDetalle();
            $("#ModalEliminarActualizarDetalle").modal('hide');
            ConsultarControlCuchilloDetalle(0, datosCabecera.IdControlCuchillo, 0, 1);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

var configDetalle = {
    wsUrl: 'http://192.168.0.31:8870',
    baseUrl: '@Url.Content("~/")',
    opcionesDT: {
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            },
            "buttons": {
                "pageLength": "<img style='width:100%' src='../../Content/icons/show24.png' />"
            }
        },
        "pageLength": 15,
        "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "Todos"]],
        "pagingType": "full_numbers",
        "dom": 'Bfrtip',
        //"scrollX": "auto",
        //"scrollY": "auto",
        "order": [[0, "desc"], [1, "desc"]],
        "buttons": [
            {
                extend: 'pageLength',
                text: ' <img style="width:100%" src="../../Content/icons/show24.png" />',
                titleAttr: 'Mostrar'
            }, {
                extend: 'copyHtml5',
                text: ' <img style="width:100%" src="../../Content/icons/copy24.png" />',
                titleAttr: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                text: '<img style="width:100%" src="../../Content/icons/excel24.png" />',
                titleAttr: 'Excel'
            },
            {
                extend: 'print',
                text: '<img style="width:100%" src="../../Content/icons/print24.png" />',
                titleAttr: 'Imprimir'
            },
            {
                text: '<img id="imgBtnColumnas"  style="width:100%" src="../../Content/icons/justificar24.png" />',
                titleAttr: 'Ajustar Columnas',
                action: MostarModalColumns
            }
        ]
    }
}
