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
    $('#cargac').show();
    $("#divCargarCuchillosDetalle").prop("hidden", true);
    var op = opcion;
    var pFecha = moment($("#txtFechaFiltro").val()).format("YYYY-MM-DD");
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
                MensajeAdvertencia("¡No hay datos en la consulta!", false);
            }
            $('#MensajeRegistros').prop("hidden", true);
            for (var item in resultado) {
                resultado[item].Fecha = moment(resultado[item].Fecha).format("DD-MM-YYYY");
                resultado[item].Hora = moment(resultado[item].Hora).format("HH:mm");
            }
            $("#tblDataTableCargar tbody").empty();            
            config.opcionesDT.order = [1,'desc'];
            config.opcionesDT.columns = [
                { data: 'Fecha' },
                { data: 'Hora' },
                { data: 'Observacion' }
            ];           
            table.DataTable().destroy();
            table.DataTable(config.opcionesDT);
            table.DataTable().clear();
            table.DataTable().rows.add(resultado);
            table.DataTable().draw();
            setTimeout(function () {
                $('#cargac').hide();
            }, 500);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarModificarControlCuchilloPreparacion() {
    $('#ModalIngresoRegistroCabecera').modal('hide');
    var pFecha = moment($("#txtFechaIngresoCabecera").val()).format("YYYY-MM-DD");
    var pHora = moment($("#txtFechaIngresoCabecera").val()).format("YYYY-MM-DDTHH:mm");
    $("#txtObservacion").val($("#txtObservacionCabecera").val());
    $("#txtFecha").val($("#txtFechaIngresoCabecera").val());
    if ($("#txtFechaIngresoCabecera").val() != '') {
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloPreparacion",
            type: "POST",
            data: {
                IdControlCuchillo: datosCabecera.IdControlCuchillo,
                Fecha: pFecha,
                Hora: pHora,
                Observacion: $("#txtObservacionCabecera").val()
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
                LimpiarCabecera();
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

function LimpiarCabecera() {
    $("#txtObservacionCabecera").val('');
    var date = new Date();
    $("#txtFechaIngresoCabecera").val(moment(date).format('YYYY-MM-DDTHH:mm'));
}

function SeleccionarCabecera(model) {
    $('#divBotonNuevo').hide();
    $('#divCabecera').prop('hidden', false);
    datosCabecera = model;
    var table = $("#tblDataTableAprobar");
    table.DataTable().clear();
    var data = model;
    $("#divCargarCuchillosDetalle").prop("hidden", false);
    $("#divIngresarDetalleHora").prop("hidden",false);
    $("#divCargarCuchillos").prop("hidden", true);
    $('#txtFechaFiltro').prop('hidden', true);
    $('#btnBuscarFecha').prop('hidden', true);
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
    $("#divBotonNuevo").show();  
    $('#divCabecera').prop('hidden', true);
    $('#txtFechaFiltro').prop('hidden', false);
    $('#btnBuscarFecha').prop('hidden', false);
    datosCabecera = [];
    ListarControlCuchillos(0);
}

function OcultaControles() {
    $("#btnModalGenerarRegistro").prop("hidden", true);   
    $("#btnModalActualizarRegistro").prop("hidden", true);   
    $("#divCargarCuchillosDetalle").prop("hidden",true);
    $("#divIngresarDetalleHora").prop("hidden", true);
    $("#divCargarCuchillos").prop("hidden", false);
    $('#modalEliminarControl').modal('hide');
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
            $('#divCabecera').prop('hidden', true);
            $('#divBotonNuevo').show();
            $('#txtFechaFiltro').prop('hidden', false);
            $('#btnBuscarFecha').prop('hidden', false);
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
    $("#txtObservacionCabecera").val($("#txtObservacion").val());
    $("#txtFechaIngresoCabecera").val($("#txtFecha").val());
    $("#divCargarCuchillos").prop("hidden", true);   
    $('#ModalIngresoRegistroCabecera').modal('show');
}

function NuevoRegistroCabecera() {
    $('#ModalIngresoRegistroCabecera').modal('show');
    $("#txtFechaIngresoCabecera").val($("#txtFechaFiltro").val());
}

//GUARDAR DETALLE
function GuardarControlDetalle(op) {
    if (ValidarCuchilloExiste(op) == true || ValidarEmpleadoExiste(op)==true) {
        return;
    }
    else {
        var estado = $("#CheckEstadoRegistroOp").prop('checked');
        var idCuchillopreparacion = $('#txtCodigoCuchillo').val();
        var cedulaEmpleado = $('#txtEmpleado').val();
        var p = $('#txtCodigoCuchilloDetalle').val();
        if ($('#txtActualizar').val() == '1') {
            if ($('#txtCodigoCuchilloDetalle').val() != '') {
                idCuchillopreparacion = $('#txtCodigoCuchilloDetalle').val();
                estado = $("#CheckEstadoRegistroOpD").prop('checked');
            }
            if ($('#txtEmpleadoDetalle').val()!='') {
                cedulaEmpleado = $('#txtEmpleadoDetalle').val();
            }
        } else {
            if ($('#txtCodigoCuchillo').val() == "" || $('#txtEmpleado').val() == "") {
                MensajeAdvertencia('Seleccione el CUCHILLO o EMPLEADO ');
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
                CedulaEmpleado: cedulaEmpleado,
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
                datosDetalle = [];
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

function ConsultarControlCuchilloDetalle(idCuchilloPreparacion, idControlCuchillo, idControlCuchilloDetalle, opcion) {
    $('#cargac').show();
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
            opcion: op
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
                { data: 'CodigoCuchillo' },
                { data: 'Estado' },
                { data: 'CedulaEmpleado' },
                { data: 'UsuarioIngresoLog' }
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
                    return '<center><span class="badge ' + clscolor + '"><input type="checkbox" ' + checked + ' disabled id="vehicle2" name="Estado" value="Estado"></span></center>';
                }
            }];
            table.DataTable().destroy();
            table.DataTable(configDetalle.opcionesDT);
            table.DataTable().clear();
            var conRow=0;
            resultado.forEach(function (row) {                
                var colummCedula = "";
                colummCedula = row.CedulaEmpleado;
                colummCedula = colummCedula.split('-')
                resultado[conRow].Cedula = colummCedula[0];                
                row.CedulaEmpleado = colummCedula[1];
                conRow++;
            });
            table.DataTable().rows.add(resultado);            
            table.DataTable().draw();
            setTimeout(function () {
                $('#cargac').hide();
            }, 500);
        },
        error: function (resultado) {
            setTimeout(function () {
                $('#cargac').hide();
            }, 500);
            MensajeError(resultado.responseText, false);
        }
    });
}

function LimpiarDetalle() {
    $('#LabelEstado').text('Estado Registro');
    $('#CheckEstadoRegistroOp').prop('checked', false);
    $('#txtCodigoCuchillo').prop('selectedIndex', 0);
    $('#txtEmpleado').prop('selectedIndex', 0);
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
    $("#txtEmpleadoDetalle").val(model.Cedula);      
    $("#EstadoRegistroDetalle").val(datosDetalle.Estado);
    CambioEstadoDetalle(datosDetalle.Estado);
    $("#CheckEstadoRegistroOpD").prop('checked', datosDetalle.Estado);    
    $("#ModalEliminarActualizarDetalle").modal('show');
}

function ValidarCuchilloExiste(op) {    
    var sel = '';
    if (op == 0) {
        sel = document.getElementById('txtCodigoCuchillo');
    } else {
        sel = document.getElementById('txtCodigoCuchilloDetalle');
    }
    
    var selected = sel.options[sel.selectedIndex].text;
    if (datosDetalle.CodigoCuchillo == selected) {        
        return false;
    } else {
        var table = $("#tblDataTableCargarDetalle").DataTable();
        var form_data = table.rows().data();
        for (var i in form_data) {
            if (form_data[i].CodigoCuchillo == selected) {
                MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe un cuchillo con ese código registrado:    " + form_data[i].CodigoCuchillo +"</span>", 10);
                return true;                
            }
            //console.log(form_data[i].CodigoCuchillo);
        }
        //GuardarControlDetalle();
    }
    return false;
}

function ValidarEmpleadoExiste(op) {
    var selected = '';
    if (op == 0) {
        selected = $('#txtEmpleado').val();
    } else {
        selected = $('#txtEmpleadoDetalle').val();
    }
    if (datosDetalle.CedulaEmpleado == selected) {
        return false;
    } else {
        var table = $("#tblDataTableCargarDetalle").DataTable();
        var form_data = table.rows().data();
        for (var i in form_data) {
            if (form_data[i].CedulaEmpleado == selected) {
                MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe un EMPLEADO asignado:    " + form_data[i].CedulaEmpleado + "</span>", 10);
                return true;
            }
        }
    }
    return false;
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
                return;
            }
            MensajeCorrecto("¡Registro eliminado correctamente!");
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
