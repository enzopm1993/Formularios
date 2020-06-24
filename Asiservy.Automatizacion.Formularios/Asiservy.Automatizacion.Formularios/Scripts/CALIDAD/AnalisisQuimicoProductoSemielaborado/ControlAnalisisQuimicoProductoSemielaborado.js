var IdCabecera = 0;
var IdDetalle = 0;
var IdSubDetalle = 0;
var Error = 0;
var DatosDetalle;
var ListaLotes;
$(document).ready(function () {
    
    $('#txtSalProceso').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 99.99,
        rightAlign: false
    });
    $('#txtSalEmpaque').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 99.99,
        rightAlign: false
    });
    $('#txtHistaminaEmpaque').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 999.99,
        rightAlign: false
    });
    $('#txtHistaminaProceso').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 999.99,
        rightAlign: false
    });
    $('#txtHumedadProceso').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 99.99,
        rightAlign: false
    });
   
    //$('#txtSalEmpaque').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00','max': '99.99' });
    //$('#txtHistaminaEmpaque').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    //$('#txtHistaminaProceso').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '999.99' });
    //$('#txtHumedadProceso').inputmask({ 'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'digits': 2, 'digitsOptional': false, /*'placeholder': '0.00',*/'max': '99.99' });


    ConsultarCabControl();
    LLenarComboOrdenes();
});

var modal_lv = 0;
$('.modal').on('shown.bs.modal', function (e) {
    $('.modal-backdrop:last').css('zIndex', 1051 + modal_lv);
    $(e.currentTarget).css('zIndex', 1052 + modal_lv);
    modal_lv++
});

$('.modal').on('hidden.bs.modal', function (e) {
    modal_lv--
});

$("#btnOrden").on("click", function (e) {
    $('#ModalOrdenes').modal('show');
});
$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").prop('selectedIndex') == 0) {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#cmbOrdeneFabricacion").val($("#SelectOrdenFabricacion").val());
    LlenarComboLotes($("#SelectOrdenFabricacion").val());
    $('#txtProveedor').val('');
    $('#txtEspecie').val('');
    $('#txtTalla').val('');
    $('#txtCliente').val('');
    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);

    //ConsultarCabControl();

});
function DatosOrdenFabricacion() {
    Error = 0;
    $('#cargac').show();
    $('#txtCliente').val('');
    //console.log(ListaOrdenesFabricacion);
    //var Datos = Enumerable.From(ListaOrdenesFabricacion)
    //    .Where(function (x) { return x.ORDEN_FABRICACION == $('#cmbOrdeneFabricacion').val() })
    //    .Select(function (x) { return x })
    //    .FirstOrDefault();
    let params = {
        Orden: $("#cmbOrdeneFabricacion").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarDatosOrdenFabricacion?' + query;

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
                $('#txtCliente').val(resultado.CLIENTE);
                //console.log(resultado);
                //$.each(resultado, function (key, value) {
                //    $('#cmbLote').append('<option value=' + value.Codigo + '>' + value.Descripcion + '</option>');
                //});
                //console.log(resultado);

            }
            $('#cargac').hide();
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })


    //$('#txtCliente').val(Datos.NombreCliente);
    //LlenarComboLotes($("#cmbOrdeneFabricacion").val());
}
function DatosLote() {
    DatosOrdenFabricacion();
    var Datos = Enumerable.From(ListaLotes)
        .Where(function (x) { return x.descripcion == $('#cmbLote').val() })
        .Select(function (x) { return x })
        .FirstOrDefault();
    //console.log(Datos);
    ////$('#txtBuque').val(Datos.Barco);
    $('#txtProveedor').val(Datos.Barco);
    $('#txtEspecie').val(Datos.Especie);
    $('#txtTalla').val(Datos.Talla);
    $('#txtCliente').val(Datos.Cliente);
}
function LlenarComboLotes(orden,bandera) {
    Error = 0;
    $('#cmbLote').empty();
    $('#cmbLote').append('<option>Seleccione...</option>');

    let params = {
        Orden: orden /*$("#cmbOrdeneFabricacion").val()*/
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultarLotesPorOf?' + query;

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
                //console.log('combolotes');
                //console.log(resultado);
                ListaLotes = resultado;
                $.each(resultado, function (key, value) {
                    $('#cmbLote').append('<option value=' + value.descripcion + '>' + value.descripcion + '</option>');
                });
                if (bandera) {
                    $('#cmbLote').val(DatosDetalle.Lote);
                    DatosLote();
                }
            }
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })

}
function AbrirModalDetalle() {
    //$('#ModalSubDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    IdDetalle = 0;
    $('#ModalDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    LimpiarControlesDetalle();

    $('#btnOrden').prop('disabled', false);
    $('#cmbLote').prop('disabled', false);
}
function AbrirModalSubDetalle() {
    $('#ModalSubDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    IdSubDetalle = 0;
    LimpiarControlesSubDetalle();
}
async function LlenarComboOrdenesAjax() {
    let params = {
        Fecha: $("#txtFechaOrden").val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../General/ConsultaSoloOFNivel3?' + query;
    var promesa = fetch(url);
    return promesa;
}
async function LLenarComboOrdenes(/*orden*/) {

    try {
        $('#txtCliente').val('');
        //if ($('#txtFechaProduccion').val() == '') {
        //    $('#msjErrorFechaProduccion').prop('hidden', false);
        //    return;
        //} else {
        //    $('#msjErrorFechaProduccion').prop('hidden', true);
        //}
        $('#SelectOrdenFabricacion').empty();
        $('#SelectOrdenFabricacion').append('<option> Seleccione</option>');
        if (!$('#txtFechaOrden').val() == '') {
            var PromesaConsultar = await LlenarComboOrdenesAjax();
            if (!PromesaConsultar.ok) {
                throw "Error";
            }
            var ResultadoConsultar = await PromesaConsultar.json();
            console.log(ResultadoConsultar);
            if (ResultadoConsultar == "101") {
                window.location.reload();
            }
            $.each(ResultadoConsultar, function (key, value) {
                $('#SelectOrdenFabricacion').append('<option value=' + value.Orden + '>' + value.Orden + '</option>');
            });

        }
    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }


}
async function ConsultarCabControlAjax() {
    const data = new FormData();
    data.append('Fecha', $("#txtFechaProduccion").val());
    var promesa = fetch("../AnalisisQuimicoProductoSemielaborado/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    })
    return promesa;
}
async function ConsultarCabControl(bandera) {

    try {


        if ($('#txtFechaProduccion').val() == '') {
            $('#msjErrorFechaProduccion').prop('hidden', false);
            return;
        }
        else {

            $('#msjErrorFechaProduccion').prop('hidden', true);
        }
       
        //if (bandera != 'of')//bandera para que solo se ejecute si se llama desde onchange de fecha, y no por onchange de orden de fabricacion
        //{
        //    await LLenarComboOrdenes();
        //}
        $('#btnCargando').prop('hidden', false);
        $('#btnConsultar').prop('hidden', true);
        $('#btnLimpiar').prop('hidden', true);
        $('#btnEliminarCabeceraControl').prop('hidden', true);
        $('#btnGuardar').prop('hidden', true);
        var PromesaConsultar = await ConsultarCabControlAjax();
        if (!PromesaConsultar.ok) {
            throw "Error";
        }
        var ResultadoConsulta = await PromesaConsultar.json();
        if (ResultadoConsulta != "0") {

            $('#mensajeRegistros').prop('hidden', true);
            console.log(ResultadoConsulta.IdAnalisisQuimicoProductoSe);
            IdCabecera = ResultadoConsulta.IdAnalisisQuimicoProductoSe;
          
            if (ResultadoConsulta.EstadoControl == true) {
                $("#estadocontrol").removeClass("badge-danger").addClass("badge-success");
                $('#estadocontrol').text(Mensajes.Aprobado);
            }
            else {
                $("#estadocontrol").removeClass("badge-success").addClass("badge-danger");
                $('#estadocontrol').text(Mensajes.Pendiente);
            }
           
            $('#Observacion').val(ResultadoConsulta.Observacion);
            $('#CardDetalle').prop('hidden', false);
            $('#btnEliminarCabeceraControl').prop('disabled', false);
            $('#brespacio').remove();
            //ValidarParametro();
            //LlenarComboLotes(ResultadoConsulta.OrdenFabricacion);
            ConsultarDetalleControl();
     

            //SlideCabecera();
        } else {
            $('#brespacio').remove();
            $('#DivCabecera').after('<br id="brespacio">');
            $('#mensajeRegistros').prop('hidden', false);
            $('#mensajeRegistros').text(Mensajes.SinRegistros);
          
            LimpiarControles();

            $('#CardDetalle').prop('hidden', true);

        }
        //DatosOrdenFabricacion();

    } catch (ex) {
        MensajeError('Error comuníquese con el departamento de Sistemas, ' + ex.message, false);
    }
    $('#btnCargando').prop('hidden', true);
    $('#btnConsultar').prop('hidden', false);
    $('#btnLimpiar').prop('hidden', false);
    $('#btnEliminarCabeceraControl').prop('hidden', false);
    $('#btnGuardar').prop('hidden', false);

}
function GuardarCabceraControl() {
 
    $('#btnCargando').prop('hidden', false);
    $('#btnConsultar').prop('hidden', true);
    $('#btnLimpiar').prop('hidden', true);
    $('#btnEliminarCabeceraControl').prop('hidden', true);
    $('#btnGuardar').prop('hidden', true);
    const data = new FormData();
    data.append('IdAnalisisQuimicoProductoSe', IdCabecera);
    data.append('Fecha', $("#txtFechaProduccion").val());
    data.append('Observacion', $('#Observacion').val());
    fetch("../AnalisisQuimicoProductoSemielaborado/GuardarCabeceraControl", {
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
            IdCabecera = resultado[2].IdAnalisisQuimicoProductoSe;
            if (resultado[0] == "002" || resultado[0] == "003") {
                MensajeAdvertencia(resultado[1]);
             
                $('#Observacion').val(resultado[2].Observacion);
       
            } else {
                MensajeCorrecto(resultado[1]);
                //$('#brespacio').remove();
                $('#mensajeRegistros').text('');
                $('#CardDetalle').prop('hidden', false);
                //SlideCabecera();
            }
            $('#CardDetalle').prop('hidden', false);
        }
        $('#btnCargando').prop('hidden', true);
        $('#btnConsultar').prop('hidden', false);
        $('#btnLimpiar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('hidden', false);
        $('#btnGuardar').prop('hidden', false);
        $('#btnEliminarCabeceraControl').prop('disabled', false);
        //LlenarComboLotes($('#cmbOrdeneFabricacion').val());
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);

        })
}
function LimpiarControles() {
    //console.log($("#cmbOrdeneFabricacion").val());
    IdCabecera = 0;
    Error = 0;
    IdDetalle = 0;
    $('#estadocontrol').text('');
    $('#Observacion').val('');
    $('#DivDetalles').empty();
    $('#btnEliminarCabeceraControl').prop('disabled', true);
}
function ConfirmarEliminarCab() {
    $('#ModalEliminarControl').modal('show');
}
function EliminarCabecera() {
    $('#btnsicab').prop('disabled', true);
    $('#btnnocab').prop('disabled', true);
    Error = 0;
    const data = new FormData();
    data.append('IdCabecera', IdCabecera);


    fetch("../AnalisisQuimicoProductoSemielaborado/EliminarCabeceraControl", {
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
            $('#ModalEliminarControl').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);

                LimpiarControles();
                $('#DivDetalle').empty();
                ConsultarCabControl();
            }
            //}

        }
        $('#btnsicab').prop('disabled', false);
        $('#btnnocab').prop('disabled', false);

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnsicab').prop('disabled', false);
            $('#btnnocab').prop('disabled', false);
        })
}
function GuardarDetalleControl() {
    Error = 0;
    if (!ValidarDetalle()) {
        return;
    } else {
        $('#cargac').show();
        const data = new FormData();
        data.append('IdDetalleAnalisisQuimicoProductoSe', IdDetalle);
        data.append('OrdenFabricacion', $("#cmbOrdeneFabricacion").val());
        data.append('Lote', $("#cmbLote").val());
        data.append('Proveedor', $("#txtProveedor").val());
        data.append('Especie', $("#txtEspecie").val());
        data.append('Talla', $("#txtTalla").val());
        data.append('Cliente', $("#txtCliente").val());
        //console.log(IdCabecera);
        data.append('IdCabeceraAnalisisQuimicoProductoSe', IdCabecera);

        fetch("../AnalisisQuimicoProductoSemielaborado/GuardarDetalleControl", {
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

                if (resultado[0] == "002" || resultado[0] == "003") {
                    MensajeAdvertencia(resultado[1]);
                }
                if (resultado[0] == "000" || resultado[0] == "001") {
                    MensajeCorrecto(resultado[1]);

                    ConsultarDetalleControl();
                }
                $('#ModalDetalle').modal('hide');
                //LimpiarDetalleControles();
            }
            $('#cargac').hide();
        })
            .catch(function (resultado) {
                //console.log('error');
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            })
    }
    
}
function ConsultarDetalleControl() {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdCabeceraControl: IdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/PartialDetalleControl?' + query;
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
                $('#DivDetalle').empty();
                $('#DivDetalle').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableDetalle').DataTable(config.opcionesDT);
                //$('#brespacio').remove();
                //LimpiarDetalleControles();
                

            } else {
                $('#DivDetalle').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#btnCargando').prop('hidden', true);
            $('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })

}
function LimpiarControlesDetalle() {
    $('#cmbLote').empty();
    $('#cmbLote').append('<option>Seleccione...</option>');
    $('#cmbOrdeneFabricacion').val('');
    $('#txtProveedor').val('');
    $('#txtEspecie').val('');
    $('#txtTalla').val('');
    $('#txtCliente').val('');

    $('#msjerrorordenfb').prop('hidden', true);
    $('#msjerrorLote').prop('hidden', true);

    LLenarComboOrdenes();

}
function VerSubDetalle(data) {
    //console.log(data);
    $('#CardDetalle').prop('hidden', true);
    $('#CardSubDetalle').prop('hidden', false);
    $('#DivCabecera').prop('hidden', true);
    $('#botonessubdetalle').prop('hidden', false);
    DatosDetalle = data;
    IdDetalle = data.IdDetalleAnalisisQuimicoProductoSe;
    ConsultarSubDetalleControl();
}
function Atras() {
    $('#CardDetalle').prop('hidden', false);
    $('#CardSubDetalle').prop('hidden', true);
    $('#DivCabecera').prop('hidden', false);
    $('#botonessubdetalle').prop('hidden', true);
    IdDetalle = 0;
}
function GuardarSubDetalleControl() {
    Error = 0;
    if (!ValidarSubDetallle()) {
        return;
    } else {
        $('#cargac').show();
        const data = new FormData();
        data.append('IdTipoAnalisisQuimicoProductoSe', IdSubDetalle);
        data.append('TipoProducto', $("#cmbTipoProducto").val());
        data.append('SalProceso', $("#txtSalProceso").val());
        data.append('HistaminaProceso', $("#txtHistaminaProceso").val());
        data.append('HumedadProceso', $("#txtHumedadProceso").val());
        data.append('SalEmpaque', $("#txtSalEmpaque").val());
        data.append('HistaminaEmpaque', $("#txtHistaminaEmpaque").val());
        data.append('CabeceraControl', IdCabecera);
        data.append('IdDetalleAnalisisQuimicoProductoSe', IdDetalle);

        fetch("../AnalisisQuimicoProductoSemielaborado/GuardarSubDetalleControl", {
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

                if (resultado[0] == "002" || resultado[0] == "003") {
                    MensajeAdvertencia(resultado[1]);
                }
                if (resultado[0] == "000" || resultado[0] == "001") {
                    MensajeCorrecto(resultado[1]);

                    ConsultarSubDetalleControl();
                }
                $('#ModalSubDetalle').modal('hide');
                //LimpiarDetalleControles();
            }
            $('#cargac').hide();
        })
            .catch(function (resultado) {
                //console.log('error');
                //console.log(resultado);
                MensajeError(resultado.responseText, false);
                $('#cargac').hide();
            })
    }
    
}
function ConsultarSubDetalleControl() {
    $('#cargac').show();
    Error = 0;
    let params = {
        IdDetalleControl: IdDetalle
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/PartialSubDetalleControl?' + query;
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
                $('#DivSubDetalle').empty();
                $('#DivSubDetalle').html(resultado);
                //config.opcionesDT.pageLength = 10;
                //$('#TableSubDetalle').DataTable(config.opcionesDT);

                //$('#brespacio').remove();
                //LimpiarDetalleControles();


            } else {
                $('#DivSubDetalle').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            //$('#btnCargando').prop('hidden', true);
            //$('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })

}
function EditarDetalle() {
    $('#ModalDetalle').modal('show');
    $('#cmbOrdeneFabricacion').val(DatosDetalle.OrdenFabricacion);
    LlenarComboLotes(DatosDetalle.OrdenFabricacion, true);
    $('#btnOrden').prop('disabled', true);
    $('#cmbLote').prop('disabled', true);
}
function EditarSubDetalle(data) {
    $('#ModalSubDetalle').modal('show');
    $('#cmbTipoProducto').val(data.TipoProducto);
    $('#txtSalProceso').val(data.SalProceso);
    $('#txtHistaminaProceso').val(data.HistaminaProceso);
    $('#txtHumedadProceso').val(data.HumedadProceso);
    $('#txtSalEmpaque').val(data.SalEmpaque);
    $('#txtHistaminaEmpaque').val(data.HistaminaEmpaque); 
    IdSubDetalle = data.IdTipoAnalisisQuimicoProductoSe;
}
function LimpiarControlesSubDetalle() {
    $('#cmbTipoProducto').prop('selectedIndex',0);
    $('#txtSalProceso').val('');
    $('#txtHistaminaProceso').val('');
    $('#txtHumedadProceso').val('');
    $('#txtSalEmpaque').val('');
    $('#txtHistaminaEmpaque').val('');
    $('#msjerrorTipoProducto').prop('hidden', true);
    $('#msjerrorsalproceso').prop('hidden', true);
    $('#msjerrorhumedad').prop('hidden', true);
}
function ValidarDetalle() {
    var valida = true;
    if ($('#cmbOrdeneFabricacion').val() == '') {
        valida = false;
        $('#msjerrorordenfb').prop('hidden', false);

    } else {
        $('#msjerrorordenfb').prop('hidden', true);
    }
    if ($('#cmbLote').prop('selectedIndex') == 0) {
        valida = false;
        $('#msjerrorLote').prop('hidden', false);
    } else {
        $('#msjerrorLote').prop('hidden', true);
    }
    return valida;

}
function ValidarSubDetallle() {
    var valida = true;
    if ($('#cmbTipoProducto').prop('selectedIndex') == 0){
        valida = false;
        $('#msjerrorTipoProducto').prop('hidden', false);

    } else {
        $('#msjerrorTipoProducto').prop('hidden', true);
    }
    if ($('#txtSalProceso').val() == '') {
        $('#areproc').addClass('active');
        $('#areaemp').removeClass('active');
        $('#Proceso').addClass('active');
        $('#Empaque').removeClass('active fade');
        valida = false;
        $('#msjerrorsalproceso').prop('hidden', false);
    } else {
        $('#msjerrorsalproceso').prop('hidden', true);
    }
    if ($('#txtHumedadProceso').val() == '') {
        $('#areproc').addClass('active');
        $('#areaemp').removeClass('active');
        $('#Proceso').addClass('active');
        $('#Empaque').removeClass('active fade');
        valida = false;
        $('#msjerrorhumedad').prop('hidden', false);
    } else {
        $('#msjerrorhumedad').prop('hidden', true);
    }
    
    return valida;
}
function ConfirmarEliminarSubDetalle(data) {
    $('#ModalEliminarSubDetalle').modal('show');
    IdSubDetalle = data.IdTipoAnalisisQuimicoProductoSe;
 
}
function EliminarSubDetalle() {
    $('#cargac').show();
    Error = 0;
    const data = new FormData();
    data.append('IdSubdetalle', IdSubDetalle);
    data.append('IdCabecera', IdCabecera);


    fetch("../AnalisisQuimicoProductoSemielaborado/EliminarSubDetalleControl", {
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

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);

                LimpiarControlesSubDetalle();
                ConsultarSubDetalleControl();
            }
            //}

        }

        $('#cargac').hide();
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
function ConfirmarEliminarDetalle() {
    $('#ModalEliminarDetalle').modal('show');  
    $('#ordenflbl').text(DatosDetalle.OrdenFabricacion);
    $('#lotelbl').text(DatosDetalle.Lote);   
}
function EliminarDetalle() {
    $('#cargac').show();
    Error = 0;
    const data = new FormData();
    data.append('IdDetalle', IdDetalle);
    data.append('IdCabecera', IdCabecera);


    fetch("../AnalisisQuimicoProductoSemielaborado/EliminarDetalleControl", {
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

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                $('#CardDetalle').prop('hidden', false);
                $('#CardSubDetalle').prop('hidden', true);
                $('#DivCabecera').prop('hidden', false);
                $('#botonessubdetalle').prop('hidden', true);
                LimpiarControlesDetalle();
                ConsultarDetalleControl();

            }
            //}

        }

        $('#cargac').hide();
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
