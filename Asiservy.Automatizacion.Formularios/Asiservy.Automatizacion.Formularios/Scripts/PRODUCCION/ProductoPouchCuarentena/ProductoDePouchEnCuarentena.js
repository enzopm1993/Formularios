var ListaOrdenesFabricacion;
var IdCabecera = 0;
var Error = 0;
var IdDetalle = 0;
var IdSubdetalle = 0;
var IdDetalle_SUBDETALLE = 0;
var IdCocheAutoclave = 0;
var IdDetalleEliminar;

$(document).ready(function () {
    LLenarComboOrdenes();
});
$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});
function LLenarComboOrdenes(orden) {
    $('#txtProducto').val('');
    $('#txtTamanoFunda').val('');
    $('#txtCliente').val('');
    $('#txtPedidoVenta').val('');
    $('#CodProducto').val('');
    $('#SelectOrdenFabricacion').empty();
    $('#SelectOrdenFabricacion').append('<option>Seleccione...</option>');
    let params = {
        Fecha: $("#txtFechaOrden").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarOFNivel1?' + query;
    if ($('#txtFechaOrden').val() == '') {

    } else {
        fetch(url)
            //,body: data
            .then(function (respuesta) {
                return respuesta.json();
            })
            .then(function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                ListaOrdenesFabricacion = resultado;
                $.each(ListaOrdenesFabricacion, function (key, value) {
                    $('#SelectOrdenFabricacion').append('<option value=' + value.ORDEN_FABRICACION + '>' + value.ORDEN_FABRICACION + '</option>');
                });
                if (orden != null) {
                    $('#SelectOrdenFabricacion').val(orden);
                }
            })
            .catch(function (resultado) {
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#btnCargando').prop('hidden', true);
                $('#btnConsultar').prop('hidden', false);
            })
    }

}
function DatosOrdenFabricacion() {
    var Datos = Enumerable.From(ListaOrdenesFabricacion)
        .Where(function (x) { return x.ORDEN_FABRICACION == $('#cmbOrdeneFabricacion').val() })
        .Select(function (x) { return x })
        .FirstOrDefault();
    $('#txtProducto').val(Datos.ItemName);
    $('#txtTamanoFunda').val(Datos.Envase);
    $('#txtCliente').val(Datos.NombreCliente);
    $('#txtPedidoVenta').val(Datos.OrdenVenta);
    $('#CodProducto').val(Datos.ItemCode);
    
}
function GuardarCabceraControl() {
    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
    if ($('#cmbTurno').prop('selectedIndex') == 0) {
        $('#msjerrorturno').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorturno').prop('hidden', true);
    }
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    const data = new FormData();
    data.append('IdCabProdPouchCuarentena', IdCabecera);
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('FechaTerminado', $("#txtFechaTerminado").val());
    data.append('Turno', $("#cmbTurno").val());
    data.append('Codigo', $("#txtCodigo").val());
    data.append('PedidoVenta', $("#txtPedidoVenta").val());
    data.append('Producto', $("#txtProducto").val());
    data.append('TamanoFunda', $("#txtTamanoFunda").val());
    data.append('Cliente', $("#txtCliente").val());
    data.append('TotalCajas', $("#txtTotalCajas").val());
    data.append('CodigoProducto', $("#CodProducto").val());
    data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
    
    fetch("../ProductoPouchCuarentena/GuardarCabeceraConttrol", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
            //console.log(respuesta);
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {
                IdCabecera = resultado[2].IdCabProdPouchCuarentena;
                if (resultado[0] == "002") {
                    MensajeAdvertencia(resultado[1]);
                } else {
                    MensajeCorrecto(resultado[1]);
                    $('#CardDetalle').prop('hidden', false);
                }
                
            }
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#btnLimpiar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('hidden', false);
            $('#btnGuardar').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            
        })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            
        })
}
function ConsultarCabControl() {
    if ($('#txtFechaProduccion').val() == '') {
        $('#msjErrorFechaProduccion').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorFechaProduccion').prop('hidden', true);
    }
    if ($('#cmbTurno').prop('selectedIndex') == 0) {
        $('#msjerrorturno').prop('hidden', false);
        return false;
    } else {
        $('#msjerrorturno').prop('hidden', true);
    }
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    const data = new FormData();
    data.append('FechaProduccion', $("#txtFechaProduccion").val());
    data.append('Turno', $("#cmbTurno").val());
    fetch("../ProductoPouchCuarentena/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            //console.log(resultado);
            $('#txtFechaProduccion').prop('disabled', true);
            $('#cmbTurno').prop('disabled', true);
            //LLenarComboOrdenes(resultado.OrdenFabricacion);
            //$("#cmbTurno").val("2");
            if (resultado != "0") {
                $('#cmbOrdeneFabricacion').val(resultado.OrdenFabricacion);
                $('#mensajeRegistros').prop('hidden', true);
                IdCabecera = resultado.IdCabProdPouchCuarentena;
                $('#CodProducto').val(resultado.CodigoProducto);
                $('#txtProducto').val(resultado.Producto);
                $('#txtTamanoFunda').val(resultado.TamanoFunda);
                $('#txtCliente').val(resultado.Cliente);
                $('#txtPedidoVenta').val(resultado.PedidoVenta);
                $('#txtCodigo').val(resultado.Codigo);
                $('#txtFechaTerminado').val(moment(resultado.FechaTerminado).format("YYYY-MM-DD"));
                $('#txtTotalCajas').val(resultado.TotalCajas);
                $('#CardDetalle').prop('hidden', false);
                $('#btnEliminarCabeceraControl').prop('disabled', false);
                ConsultarDetalleControl();
            } else {
                $('#mensajeRegistros').prop('hidden', false);
            }
        }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
        

    }).catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarControles() {
    IdSubdetalle = 0;
    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    $('#cmbOrdeneFabricacion').val('');
    $('#txtFechaProduccion').val('');
    $('#cmbTurno').prop('selectedIndex', 0);
    $('#cmbOrdeneFabricacion').empty();
    //$('#cmbOrdeneFabricacion').append('<option>Seleccione...</option>');
    $('#txtProducto').val('');
    $('#txtTamanoFunda').val('');
    $('#txtCliente').val('');
    $('#txtPedidoVenta').val('');
    $('#txtCodigo').val('');
    $('#txtFechaTerminado').val('');
    $('#txtTotalCajas').val('');
    $('#txtFechaProduccion').prop('disabled', false);
    $('#cmbTurno').prop('disabled', false);
    $('#mensajeRegistros').prop('hidden', true);
    $('#CardDetalle').prop('hidden', true);
    $('#DivDetalles').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
}
function GuardarDetalleControl() {
    Error = 0;
    if ($('#txtPallet').val() == '') {
        $('#msjErrorPallet').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorPallet').prop('hidden', true);
    }
    if ($('#txtHoraInicio').val() == '') {
        $('#msjErrorHoraInicio').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorHoraInicio').prop('hidden', true);
    }
    if ($('#txtHoraFinal').val() == '') {
        $('#msjErrorHoraFin').prop('hidden', false);
        return false;
    } else {
        $('#msjErrorHoraFin').prop('hidden', true);
    }
    $('#btnGuardarDetalle').prop('hidden', true);
    $('#btnLimpiarDetalle').prop('hidden', true);
    $('#btnCargandoDetalle').prop('hidden', false)
    const data = new FormData();
    data.append('IdCabProdPouchCuarentena', IdCabecera);
    data.append('IdDetalleProdPouchCuarentena',IdDetalle);
    data.append('Pallet', $("#txtPallet").val());
    data.append('HoraInicio', $("#txtHoraInicio").val());
    data.append('HoraFin', $("#txtHoraFinal").val());
    

    fetch("../ProductoPouchCuarentena/GuardarDetalleConttrol", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            
            if (resultado[0] == "002") {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                $('#CardDetalle').prop('hidden', false);
                ConsultarDetalleControl();
            }

        }
        $('#btnGuardarDetalle').prop('hidden', false);
        $('#btnLimpiarDetalle').prop('hidden', false);
        $('#btnCargandoDetalle').prop('hidden', true);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function ConsultarDetalleControl() {
    let params = {
        IdCabecera: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../ProductoPouchCuarentena/PartialDetalleControl?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivDetalles').html(resultado);
                LimpiarDetalleControles();
            }
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
        })
}
function LimpiarDetalleControles() {
    $('#txtPallet').val('');
    $('#txtHoraInicio').val('');
    $('#txtHoraFinal').val('');
    $('#txtPallet').prop('disabled', false);
    $('#msjErrorPallet').prop('hidden', true);
    $('#msjErrorHoraInicio').prop('hidden', true);
    $('#msjErrorHoraFin').prop('hidden', true);
    IdDetalle = 0;
}
function ModificarDetalle(data) {
    //console.log(data);
    IdDetalle = data.IdDetalleProdPouchCuarentena;
    $('#txtPallet').val(data.Pallet);
    $('#txtHoraInicio').val(moment(data.HoraInicio).format("YYYY-MM-DDThh:mm"));
    $('#txtHoraFinal').val(moment(data.HoraFin).format("YYYY-MM-DDThh:mm"));
    $('#txtPallet').prop('disabled', true);
}
function AbrirModalSubDetalle(data) {
    $('#DivSubDetalle').empty();
    Error = 0;
    let params = {
        Fecha: $('#txtFechaProduccion').val(),
        Turno: $('#cmbTurno').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../ProductoPouchCuarentena/PartialCocheAutoclave?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == '"101"') {
                window.location.reload();
            }
            if (Error == 0) {
                $('#DivCochesAutoclave').html(resultado);
                IdDetalle_SUBDETALLE = data.IdDetalleProdPouchCuarentena;
                ConsultarSubDetalleControl();
            }
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
           
        })
    $('#modalSubdetalle').modal('show');
    
}
function ConsultarCarros(idcoche) {
    $('#cmbCarro').empty();
    Error = 0;
    let params = {
        idCocheAutoclave: idcoche
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../ProductoPouchCuarentena/ConsultarCarros?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            if (!respuesta.ok) {
                //MensajeError(respuesta.statusText);
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
                Error = 1;
            }
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (Error == 0) {
                
                $.each(resultado, function (i, value) {
                    $('#cmbCarro').append('<option value=' + value.Tarjeta + '>' + value.Tarjeta + '</option>');
                    //resultado[i].HoraInicio = moment(resultado[i].HoraInicio).format('YYYY-MM-DD, h:mm:ss')
                });
            }
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
        })
}
function AgregarCocheAControl(data) {
    ConsultarCarros(data.IdCocheAutoclave);
    $('#txtAutoclave').val(data.Autoclave);
    $('#txtEsterilizacion').val(data.Parada);
    IdCocheAutoclave = data.IdCocheAutoclave;
    IdSubdetalle = 0;
}
function GuardarSubDetalleControl() {
    Error = 0;
    if ($('#txtAutoclave').val() == '') {
        $('#msjerrorAutoclave').prop('hidden', false); 
        return false;
    } else {
        $('#msjerrorAutoclave').prop('hidden', true); 
    }
    if ($('#txtEsterilizacion').val() == '') {
        $('#msjerrorEsterilizacion').prop('hidden', false); 
        return false;
    } else {
        $('#msjerrorEsterilizacion').prop('hidden', true); 
    }
    if ($('#cmbCarro').val() == '') {
        $('#msjerrorCarro').prop('hidden', false); 
        return false;
    } else {
        $('#msjerrorCarro').prop('hidden', true); 
    }
    if ($('#txtFunda').val() == '') {
        $('#msjerrorFunda').prop('hidden', false); 
        return false;
    } else {
        $('#msjerrorFunda').prop('hidden', true); 
    }
    
    $('#btnEliminarSubDetalleControl').prop('hidden', true);
    $('#btnGuardarSubDetalle').prop('hidden', true);
    $('#btnLimpiarSubDetalle').prop('hidden', true);
    $('#btnCargandoSubDetalle').prop('hidden', false)
    const data = new FormData();
    data.append('IdSubDetalleProdPouchCuarentena', IdSubdetalle);
    data.append('IdDetalleProductoPouchCuarentena', IdDetalle_SUBDETALLE);
    data.append('IdCocheAutoclave', IdCocheAutoclave);
    data.append('Autoclave', $("#txtAutoclave").val());
    data.append('Esterilizacion', $("#txtEsterilizacion").val());
    data.append('NCarro', $("#cmbCarro").val());
    data.append('Funda', $("#txtFunda").val());

    fetch("../ProductoPouchCuarentena/GuardarSubDetalleConttrol", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {

            if (resultado[0] == "002") {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                //$('#CardDetalle').prop('hidden', false);
                //ConsultarDetalleControl();
                ConsultarSubDetalleControl();
            }
            //LimpiarSubDetalleControles();
        }
        $('#btnGuardarSubDetalle').prop('hidden', false);
        $('#btnLimpiarSubDetalle').prop('hidden', false);
        $('#btnCargandoSubDetalle').prop('hidden', true);
        $('#btnEliminarSubDetalleControl').prop('hidden', false);
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarSubDetalleControles() {
    IdSubdetalle = 0;
    $('#txtAutoclave').val('');
    $('#txtEsterilizacion').val('');
    $('#cmbCarro').empty();
    $('#txtFunda').val('');
    $('#cmbCarro').prop('disabled', false);
    $('#btnEliminarSubDetalleControl').prop('disabled', true);
}
function ConsultarSubDetalleControl() {
    let params = {
        IdDetalle: IdDetalle_SUBDETALLE
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../ProductoPouchCuarentena/PartialSubDetalleControl?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.text();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#DivSubDetalle').html(resultado);
                LimpiarSubDetalleControles();
            }
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
        })
}
function ModificarSubDetalle(data) {
    IdSubdetalle = data.IdSubDetalleProdPouchCuarentena;
    $('#cmbCarro').empty();
    $('#cmbCarro').append('<option value=' + data.NCarro + '>' + data.NCarro+'</option>');
    $('#cmbCarro').prop('disabled', true);
    $('#txtAutoclave').val(data.Autoclave);
    $('#txtEsterilizacion').val(data.Esterilizacion);
    $('#cmbCarro').val(data.NCarro);
    $('#txtFunda').val(data.Funda);
    $('#btnEliminarSubDetalleControl').prop('disabled', false);
}
function ConfirmarEliminarSubDetalle() {
    $('#ModalEliminarSubDetalle').modal('show');
}
function ConfirmarEliminarCab() {
    $('#ModalEliminarCabecera').modal('show');
}
function ConfirmarEliminarDetalle(data) {
    $('#ModalEliminarDetalle').modal('show');
    IdDetalleEliminar=data.IdDetalleProdPouchCuarentena;
}
function EliminarDetalle() {
    $('#btnsiDetalle').prop('disabled', true);
    $('#btnnoDetalle').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdDetalle', IdDetalleEliminar);


    fetch("../ProductoPouchCuarentena/EliminarDetalleConttrol", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarDetalle').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {
            MensajeCorrecto(resultado[1]);
            ConsultarDetalleControl();
            //LimpiarControles();
            //}

        }
        $('#btnsiDetalle').prop('disabled', false);
        $('#btnnoDetalle').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function EliminarSubDetalle() {
    $('#btnsiSubDetalle').prop('disabled', true);
    $('#btnnoSubDetalle').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdSubDetalle', IdSubdetalle);
    

    fetch("../ProductoPouchCuarentena/EliminarSubDetalleConttrol", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarSubDetalle').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {
             MensajeCorrecto(resultado[1]);
               
            ConsultarSubDetalleControl();
            //}
        
        }
        $('#btnsiSubDetalle').prop('disabled', false);
        $('#btnnoSubDetalle').prop('disabled', false);
   
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function EliminarCabecera() {
    $('#btnsicab').prop('disabled', true);
    $('#btnnocab').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdCabecera', IdCabecera);


    fetch("../ProductoPouchCuarentena/EliminarCabeceraControl", {
        method: 'POST',
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas');
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
            $('#ModalEliminarCabecera').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {
            MensajeCorrecto(resultado[1]);

            LimpiarControles();
            //}

        }
        $('#btnsicab').prop('disabled', false);
        $('#btnnocab').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}