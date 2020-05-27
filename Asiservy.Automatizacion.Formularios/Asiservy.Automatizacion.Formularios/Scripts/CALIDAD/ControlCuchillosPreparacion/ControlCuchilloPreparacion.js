﻿var datosCabecera = [];
var datosHora = [];
var datosDetalle = [];
var estadoR = [];
$(document).ready(function () {
    CargarCabecera();
    datosCabecera = 0;
    CambioEstado(false);  
});

function ConsultarEstadoReporte() {
    var pFecha = moment($("#txtFechaFiltro").val()).format("YYYY-MM-DD");
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarCabecera",
        type: "GET",
        data: {
            fecha: pFecha,
            idControlCuchillo: datosCabecera.IdControlCuchillo
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                MensajeAdvertencia('Error al realizar la consulta del estado de Reporte: ' + resultado);
            }
            CambiarMensajeEstado(resultado.EstadoReporte);
            if (resultado.EstadoReporte) {
                estadoR= true;
            } else {
                estadoR= false;
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function CargarCabecera() {
    datosCabecera = [];
    datosHora = [];
    datosDetalle = [];
    $('#cargac').show();
    if ($('#txtFechaFiltro').val() == '') {
        MensajeAdvertencia('Fecha invalida');
        $('#cargac').hide();
        return;
    }    
    var pFecha = moment($("#txtFechaFiltro").val()).format("YYYY-MM-DD");
    
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarCabecera",
        type: "GET",
        data: {
            fecha: pFecha           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divBotonNuevo').hide();
                $('#divCabecera').prop('hidden', true);
                $("#divIngresarDetalleHora").prop("hidden", true);
                $("#divCargarCuchillosDetalle").prop("hidden", true);
                $('#divBotonNuevo').show();
                $('#divHora').prop('hidden', true); 
                $('#divIngresoHora').prop('hidden', true); 
                CambiarMensajeEstado('nada');
            } else {
                $('#MensajeRegistros').prop("hidden", true);
                $('#divBotonNuevo').hide();
                $('#divCabecera').prop('hidden', false);
                $("#txtFecha").val(moment(resultado.Fecha).format('YYYY-MM-DD'));
                $("#txtObservacion").val(resultado.Observacion);
                $("#txtFecha").prop("disabled", true);
                $("#txtObservacion").prop("disabled", true);
                $('#divCargarCuchillosDetalle').prop('hidden', true);
                $('#divIngresarDetalleHora').prop('hidden', true);
                $('#btnAtras').prop('hidden', true);
                
                datosCabecera = resultado;
                ConsultarHora();                
            }
            CambiarMensajeEstado(resultado.EstadoReporte);
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ConsultarHora() {
    $('#cargac').show();   
    $.ajax({
        url: "../ControlCuchillosPreparacion/ConsultarHoraPartial",
        type: "GET",
        data: {
            idControlCuchillo: datosCabecera.IdControlCuchillo
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $('#divHora').html('No esxisten registros');
                $('#divIngresoHora').prop('hidden', true);
            } else {
                $('#divHora').html(resultado);
                $('#divIngresoHora').prop('hidden', false);
            }
            $('#divHora').prop('hidden', false); 
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function NuevoRegistroHora() {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            LimpiarCabecera();
            $('#modalIngresoHora').modal('show');
            $("#txtHora").val();
        }
    },200);
}

function GuardarHora() {
    var hora = $("#txtFecha").val() + ' ' + $("#txtHora").val();
    $('#cargac').show();
    if (ValidarHoraExiste() == true) {
        $('#cargac').hide();
        return;
    } else {
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarHora",
            type: "POST",
            data: {
                IdHora: datosHora.IdHora,
                IdControlCuchillo: datosCabecera.IdControlCuchillo,
                Hora: moment(hora).format('YYYY-MM-DDTHH:mm'),
                Descripcion: $("#txtDescripcion").val()
            },
            success: function (resultado) {
                if (resultado == 0) {
                    MensajeCorrecto("Registro guardado Exitosamente");
                } else if (resultado == 1) {
                    MensajeCorrecto("Actualización Exitoso");
                } else {
                    MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
                }
                $('#cargac').hide();
                ConsultarHora();
                datosHora = [];
                limpiarHora();
                $('#modalIngresoHora').modal('hide');
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

function ActualizarHora(jdata) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            NuevoRegistroHora();
            datosHora.IdHora = jdata.IdHora
            datosHora.Hora = moment(jdata.Hora).format('HH:mm');
            $("#txtHora").val(moment(jdata.Hora).format('HH:mm'));
            $("#txtDescripcion").val(jdata.Descripcion);
        }
    }, 200);
}

function EliminarHoraConfirmar(jdata) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            datosHora = [];
            datosHora = jdata;
            $("#modalHoraEliminar").modal("show");
        }
    }, 200);
}

function EliminarHoraSi() {
    $.ajax({
        url: "../ControlCuchillosPreparacion/EliminarrHora",
        type: "POST",
        data: {
            IdHora: datosHora.IdHora,
            idControlCuchillo: datosCabecera.IdControlCuchillo
        },
        success: function (resultado) {
            $("#modalHoraEliminar").modal("hide");
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Parametro IdControlCuchillo es incorrecto: REGISTRO NO ELIMINADO");
                return;
            }
            if (resultado == 2) {
                CargarCabecera();
                MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');
                return;
            }
            CargarCabecera();
            MensajeCorrecto("Registro eliminado exitosamente");
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarHoraNo() {
    $("#modalHoraEliminar").modal("hide");
}

function GuardarCabecera() {    
    var pFecha = moment($("#txtFechaFiltro").val()).format("YYYY-MM-DD");
    if ($("#txtFechaIngresoCabecera").val() != '') {
        $('#ModalIngresoRegistroCabecera').modal('hide');
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloPreparacion",
            type: "POST",
            data: {
                IdControlCuchillo: datosCabecera.IdControlCuchillo,
                Fecha: pFecha,
                Observacion: $("#txtObservacionCabecera").val()
            },
            success: function (resultado) {
                if (resultado == 0) {
                    limpiar();
                    MensajeCorrecto("Registro Exitoso");
                } else if (resultado == 1)  {                    
                    MensajeCorrecto("Actualización Exitoso");                    
                    $("#txtObservacion").val($("#txtObservacionCabecera").val());
                    $("#txtFecha").val($("#txtFechaIngresoCabecera").val());
                } else if (resultado == 3) {
                    MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');                   
                }
                CargarCabecera();
                $('#divBotonNuevo').hide();
                $('#divCabecera').prop('hidden', false);
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

function limpiarHora() {
    $("#txtDescripcion").val('');
    var date = new Date();    
    $("#txtHora").val(moment(date).format('HH:mm'));
}

function LimpiarCabecera() {
    $("#txtObservacionCabecera").val('');
    var date = new Date();
    $("#txtFechaIngresoCabecera").val(moment(date).format('YYYY-MM-DD'));
}

function AtrasControlPrincipal() {
    $('#divIngresoHora').prop('hidden', false);
    $('#divHora').prop('hidden', false); 
    $('#divCargarCuchillosDetalle').prop('hidden', true);
    $('#divIngresarDetalleHora').prop('hidden', true);
    $('#btnAtras').prop('hidden', true);
    datosHora = [];
}

function FechaNow() {
    var date = new Date();
    $("#txtFecha").val(moment(date).format('YYYY-MM-DD'));    
}

function ValidarHora() {
    if ($('#txtHora').val() == '') {
        $('#txtHora').css('border', '2px dashed red');
        MensajeAdvertencia('Hora incorrecta');
        return true;
    } else {
        $('#txtHora').css('border', '');
        return false;
    }
}

function ValidarHoraExiste() {     
    var table = $("#tblCargarHora").DataTable();
        var form_data = table.rows().data();
    for (var i in form_data) {
        if ($('#txtHora').val() == datosHora.Hora) {
            
            return false;
        }
            if (form_data[i][0] == $('#txtHora').val()) {
                MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe una HORA registrada:    " + form_data[i][0] + "</span>", 10);
                return true;
            }
           
        }
    return false;
}

function EliminarConfirmar() {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            $("#modalEliminarControl").modal("show");
        }
    }, 200);
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
            if (resultado == 2) {
                CargarCabecera();
                MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');
                $("#modalEliminarControl").modal('hide');                
                return;
            } if (resultado == 1) {
                MensajeCorrecto('Eliminación correcta');
            }
            $("#modalEliminarControl").modal("hide");
            limpiar(); 
            $('#divCabecera').prop('hidden', true);
            $('#divBotonNuevo').show();
            $('#txtFechaFiltro').prop('hidden', false);
            $('#btnBuscarFecha').prop('hidden', false);
            CargarCabecera();
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
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            $("#txtObservacionCabecera").val($("#txtObservacion").val());
            $("#txtFechaIngresoCabecera").val($("#txtFecha").val());
            $("#divCargarCuchillos").prop("hidden", true);
            $('#ModalIngresoRegistroCabecera').modal('show');
        }
    }, 200);
}

function NuevoRegistroCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoRegistroCabecera').modal('show');
    $("#txtFechaIngresoCabecera").val($("#txtFechaFiltro").val());
}

//GUARDAR DETALLE
function GuardarControlDetalle() {
    if (ValidarCuchilloExiste() == true || ValidarEmpleadoExiste() == true) {
        return;
    }
    else {
        //var estado = $("#CheckEstadoRegistroOp").prop('checked');
        //var idCuchillopreparacion = $('#txtCodigoCuchillo').val();
        //var cedulaEmpleado = $('#txtEmpleado').val();
        //if ($('#txtActualizar').val() == '1') {
        //    if ($('#txtCodigoCuchilloDetalle').val() != '') {
        //        idCuchillopreparacion = $('#txtCodigoCuchilloDetalle').val();
        //        estado = $("#CheckEstadoRegistroOpD").prop('checked');
        //    }
        //    if ($('#txtEmpleadoDetalle').val() != '') {
        //        cedulaEmpleado = $('#txtEmpleadoDetalle').val();
        //    }
        //} else {
            if ($('#txtCodigoCuchillo').val() == "" || $('#txtEmpleado').val() == "") {
                MensajeAdvertencia('Seleccione el CUCHILLO o EMPLEADO ');
                return;
            }
        //}
        if ($("#CheckEstadoRegistroOp").prop('checked') == false && $('#txtObservacionDetalle').val() == '') {
            $("#txtObservacionDetalle").css('border', '1px dashed red');
                MensajeAdvertencia('La observación es obligatoria');
                return;                       
        }
        $.ajax({
            url: "../ControlCuchillosPreparacion/GuardarModificarControlCuchilloDetalle",
            type: "POST",
            data: {
                IdHora: datosHora.IdHora,
                IdControlCuchilloDetalle: datosDetalle.IdControlCuchilloDetalle,
                IdCuchillopreparacion: $('#txtCodigoCuchillo').val(),
                CedulaEmpleado: $('#txtEmpleado').val(),
                idControlCuchillo: datosCabecera.IdControlCuchillo,
                Estado: $("#CheckEstadoRegistroOp").prop('checked'),
                Observacion: $('#txtObservacionDetalle').val()
            },
            success: function (resultado) {
                if (resultado == 0) {
                    LimpiarDetalle();
                    $("#ModalIngresoDetalle").modal('hide');
                    $("#ModalEliminarActualizarDetalle").modal('hide');
                    MensajeCorrecto("Registro Exitoso");
                } else if (resultado == 1) {
                    $("#ModalIngresoDetalle").modal('hide');
                    $("#ModalEliminarActualizarDetalle").modal('hide');
                    MensajeCorrecto("Actualización Exitoso");
                } else if (resultado == 2) {
                    MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');
                    $("#ModalIngresoDetalle").modal('hide');
                    CargarCabecera();
                    return;
                }
                if (datosCabecera.IdControlCuchillo == null) {
                    MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');
                    CargarCabecera();
                    return;
                }
                ConsultarDetalle(datosHora);
                datosDetalle = [];
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
    }
}

function ConsultarDetalle(jdata) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            datosHora = jdata;
            //$('#cargac').show();
            var table = $("#tblDataTableCargarDetalle");
            table.DataTable().destroy();
            table.DataTable().clear();
            //table.DataTable().draw();
            $.ajax({
                url: "../ControlCuchillosPreparacion/ConsultarControlCuchilloDetalle",
                type: "GET",
                data: {
                    idHora: jdata.IdHora,
                    op: 0
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
                        { data: 'UsuarioIngresoLog' },
                        { data: 'Acciones' }
                    ];
                    table.DataTable().destroy();
                    table.DataTable(configDetalle.opcionesDT);
                    table.DataTable().clear();
                    var conRow = 0;
                    resultado.forEach(function (row) {
                        var clscolor = "badge-danger"; //Aplico estilo a la columna Estado 
                        var checked = '';
                        if (row.Estado == true) {
                            clscolor = "badge-success";
                            checked = 'checked';
                        }
                        row.EstadoParametro = row.Estado;
                        row.Estado = '<center><span class="badge ' + clscolor + '"><input type="checkbox" ' + checked + ' disabled id="vehicle2" name="Estado" value="Estado"></span></center>';
                        var colummCedula = "";//Separo la cedula del monbre ej: 040495857-GALO BAQUE GONZALEZ
                        colummCedula = row.CedulaEmpleado;
                        var guion = colummCedula.includes("-");
                        if (guion == true) {
                            colummCedula = colummCedula.split('-')
                            resultado[conRow].Cedula = colummCedula[0];//Añado la propiedad Cedula al JSon para poder usarlo cuando edito el registro.
                            row.CedulaEmpleado = colummCedula[1];
                        } else { row.CedulaEmpleado = "<center><span class='badge badge-danger'>NO ASIGNADO</span></center>"; }
                        row.Acciones = '<button type = "button" class="btn btn-link" data-dismiss="modal" onclick="SeleccionarDetalle(' + row.IdCuchilloPreparacion + ',' + row.Cedula + ',' + row.EstadoParametro + ',' + row.IdControlCuchilloDetalle + ')">Editar</button>\
                    <button id = "btnEliminar" class="btn btn-link" onclick="EliminarDetalleConfirmar('+ row.IdControlCuchilloDetalle + ',' + row.IdHora + ')"> Eliminar</button>';
                        conRow++;
                    });
                    table.DataTable().rows.add(resultado);
                    table.DataTable().draw();
                    $('#divHora').prop('hidden', true);
                    $('#divIngresoHora').prop('hidden', true);
                    $('#divCargarCuchillosDetalle').prop('hidden', false);
                    $('#divIngresarDetalleHora').prop('hidden', false);
                    $('#btnAtras').prop('hidden', false);
                    //$('#cargac').hide();
                },
                error: function (resultado) {
                    //$('#cargac').hide();
                    MensajeError(resultado.responseText, false);
                }
            });
        }
    }, 200);
}

function LimpiarDetalle() {
    $('#txtObservacionDetalle').css('border', '');
    var date = new Date();
    $('#txtHora').val(moment(date).format('HH:mm'));
    $('#txtHora').css('border', '');
    $('#LabelEstado').text('Estado Registro');
    $('#CheckEstadoRegistroOp').prop('checked', false);
    $('#txtCodigoCuchillo').prop('selectedIndex', 0);
    $('#txtEmpleado').prop('selectedIndex', 0);
    $('#txtObservacionDetalle').val('');
}

function CambioEstado(valor) {
    if (valor) {
        $('#LabelEstado').text('Activo');
        $('#txtObservacionDetalle').css('border', '');
    }
    else {
        $('#LabelEstado').text('Inactivo');
        $("#txtObservacionDetalle").css('border', '1px dashed red');
    }
}

function ModalGenerarDetalle() {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            LimpiarDetalle();
            $("#ModalIngresoDetalle").modal('show');
            $('#txtActualizar').val('');
            datosDetalle = [];
        }
    }, 400);
}

function SeleccionarDetalle(idCuchilloP, ced, est, idDetalle) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            LimpiarDetalle();
            $("#ModalIngresoDetalle").modal('show');
            $('#txtActualizar').val('');
            datosDetalle = [];
            $("#txtCodigoCuchillo").val(idCuchilloP);
            $("#txtEmpleado").val(ced);
            $("#EstadoRegistro").val(est);
            CambioEstado(est);
            $("#CheckEstadoRegistroOp").prop('checked', est);
            datosDetalle.IdCuchilloPreparacion = idCuchilloP;
            datosDetalle.Cedula = ced;
            datosDetalle.IdControlCuchilloDetalle = idDetalle;
        }
    }, 200);
}

function ValidarCuchilloExiste() {    
    var sel = '';
        sel = document.getElementById('txtCodigoCuchillo');
    var selected = sel.options[sel.selectedIndex].text;
    var selectedValue = sel.options[sel.selectedIndex].value;
    if (datosDetalle.CodigoCuchillo == selected) {        
        return false;
    } else {
        var table = $("#tblDataTableCargarDetalle").DataTable();
        var form_data = table.rows().data();
        for (var i in form_data) {
            if (selectedValue == datosDetalle.IdCuchilloPreparacion) {
                return false;
            }
            if (form_data[i].CodigoCuchillo == selected) {
                MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe un cuchillo con ese código registrado:    " + form_data[i].CodigoCuchillo +"</span>", 10);
                return true;                
            }
        }
    }
    return false;
}

function ValidarEmpleadoExiste() {
    var selected = '';
        selected = $('#txtEmpleado').val();
    
    if (datosDetalle.CedulaEmpleado == selected) {
        return false;
    } else {
        var table = $("#tblDataTableCargarDetalle").DataTable();
        var form_data = table.rows().data();
        for (var i in form_data) {  
            if (form_data[i].Cedula == datosDetalle.Cedula) {            
                return false;
            }
            if (form_data[i].Cedula == selected) {
                MensajeAdvertencia("<span class='badge badge-danger'>!Ya existe un EMPLEADO asignado:    " + form_data[i].CedulaEmpleado + "</span>", 10);
                return true;
            }
        }
    }
    return false;
}

function EliminarDetalleSi() {
    $.ajax({
        url: "../ControlCuchillosPreparacion/EliminarControlCuchilloDetalle",
        type: "POST",
        data: {
            IdControlCuchilloDetalle: datosDetalle.IdControlCuchilloDetalle,
            idControlCuchillo: datosHora.IdControlCuchillo
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == 0) {
                MensajeAdvertencia("Parametro idControlCuchilloDetalle/idControlCuchilloEliminar es incorrecto: REGISTRO NO ELIMINADO");
                return;
            } else if (resultado == 2) {
                CargarCabecera();
                MensajeAdvertencia('El registro se encuentra APROBADO, por favor REVERSE el registro e intente nuevamente');
                $("#ModalEliminarActualizarDetalle").modal('hide');
                AtrasControlPrincipal();
                return;
            } 
            MensajeCorrecto("¡Registro eliminado correctamente!");
            $("#modalEliminarDetalle").modal("hide");
            LimpiarDetalle();
            $("#ModalEliminarActualizarDetalle").modal('hide');
            ConsultarDetalle(datosHora);
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarDetalleConfirmar(iddetalle, idHora) {
    ConsultarEstadoReporte();
    setTimeout(function () {
        if (estadoR == true) {
            MensajeAdvertencia('¡El registro se encuentra APROBADO, para poder editar dirigase a la Bandeja y REVERSE el registro!', 5);
        } else {
            datosDetalle.IdControlCuchilloDetalle = iddetalle;
            datosDetalle.IdHora = idHora;
            $("#modalEliminarDetalle").modal("show");
        }
    }, 200);
}

function EliminarDetalleCabeceraNo() {
    datosDetalle = [];
    $("#modalEliminarDetalle").modal("hide");
}

function CambiarMensajeEstado(estadoReporteParametro) {
    if (estadoReporteParametro == true) {
        $("#lblAprobadoPendiente").text("APROBADO");
        $("#lblAprobadoPendiente").removeClass('badge-danger');
        $("#lblAprobadoPendiente").addClass('badge badge-success');
    } else if (estadoReporteParametro == false) {
        $("#lblAprobadoPendiente").text("PENDIENTE");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").addClass('badge badge-danger');
    } else if (estadoReporteParametro == 'nada') {
        $("#lblAprobadoPendiente").text("");
        $("#lblAprobadoPendiente").removeClass('badge-success');
        $("#lblAprobadoPendiente").removeClass('badge badge-danger');
    }
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
        "pageLength": 0,
        "lengthMenu": [],
        "pagingType": "full_numbers",
        "dom": 'Bfrtip',
        //"scrollX": "auto",
        //"scrollY": "auto",
        "order": [[0, "desc"], [1, "desc"]],
        "buttons": []
    }
}
