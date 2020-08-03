//console.log("hola");
var IdCabecera = 0;
var IdDetalle = 0;
var IdSubDetalle = 0;
var Error = 0; 
var DatosDetalle;
var ListaLotes;
var paramSeleccionado;
var SubDxParam = 0;
var IdELiminarParamSubdetalle = 0;
var opciosGrid = {
    loadPanel: {
        enabled: true
    },
    groupPanel: { visible: true },
    grouping: {
        autoExpandAll: false
    },
    //dataSource: resultado,
    keyExpr: "IdTipoxParametro",
    selection: {
        mode: "single"
    },
    hoverStateEnabled: true,
    showColumnLines: true,
    showRowLines: true,
    rowAlternationEnabled: true,
    showBorders: true,
    allowColumnResizing: true,
    columnResizingMode: "nextColumn",
    columnMinWidth: 50,
    columnAutoWidth: true,
    //columnFixing: {
    //    enabled: true
    //},
    showBorders: true,
    showRowLines: true,
    filterRow: {
        visible: true,
        applyFilter: "auto"
    },
    headerFilter: {
        visible: true
    },
    paging: {
        enabled: true,
        pageSize: 5
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [5, 10, 0],
        showInfo: true,
        //visible: true,
        showNavigationButtons: true,
        infoText: "Página #{0}. Total: {1} ({2} filas)"
    },
    searchPanel: { visible: true },
    columns: [
        //{
          
        //    dataField: "IdTipoxParametro",
        //    area: "column",
        //    dataType: "number",
        //    hidingPriority: 0,
        //    sortOrder: "desc"
        //},
       {
            caption: "Tipo Producto",
            dataField: "TipoProducto",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Area",
            dataField: "Area",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Parámetro",
            dataField: "Parametro",
            area: "column",
            dataType: "string"
        }
        , {
            caption: "Cantidad",
            dataField: "Cantidad",
            area: "column",
            dataType: "number"
        }
        , 
        {
            caption: "Acciones", cellTemplate: function (container, options) {
                var btnEditar = "<button id='btnActualizar' class='btn btn-link' onclick='ModificarParamSubDetalleModal(" + JSON.stringify(options.data) + ")'> Editar</button>";
                var btnActivar = "<button id='btnEliminar' class='btn btn-link' onclick='InactivarParamSubdetalleConfirmar(" + JSON.stringify(options.data) + ")'> Eliminar</button>";
             
                $("<div>")
                    .append($(btnEditar))
                    .append($(btnActivar))
                    .appendTo(container);
            }
        },
       
    ]
    //,onSelectionChanged: function (selectedItems) {
    //    var data = selectedItems.selectedRowsData[0];
    //    if (data) {
    //        ModificarParamSubDetalleModal(data);
    //    }
    //}
    , export: {
        enabled: true,
        allowExportSelectedData: true
    },
    onExporting: function (e) {
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Reporte');

        DevExpress.excelExporter.exportDataGrid({
            component: e.component,
            worksheet: worksheet,
            autoFilterEnabled: true
        }).then(function () {
            workbook.xlsx.writeBuffer().then(function (buffer) {
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ListaReportesAnalisisQuimicoSe.xlsx');
            });
        });
        e.cancel = true;
    }

}
$(document).ready(function () {
    
    //IdArray.forEach(Objeto =>
    //    $('#' + Objeto.IdParametro).inputmask({
    //        alias: "decimal",
    //        clearMaskOnLostFocus: true,
    //        'digitsOptional': true,
    //        'digits': 2,
    //        max: Objeto.Mascara
    //    })
    //);
    $('#cmbTurno').prop('selectedIndex', 1);
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
function InactivarParamSubdetalleConfirmar(data) {
    $('#ModalEliminarParametroSubdetalle').modal('show');
    IdELiminarParamSubdetalle = data.IdTipoxParametro;
}
function EliminarParametroSubdetalle() {
    $('#cargac').show();
    Error = 0;
    const data = new FormData();
    data.append('IdTipoxParametro', IdELiminarParamSubdetalle);
    data.append('poFecha', $('#txtFechaProduccion').val());
    data.append('IdCabecera', IdCabecera);

    fetch("../AnalisisQuimicoProductoSemielaborado/EliminarParametroSubDetalle", {
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
            $('#ModalEliminarParametroSubdetalle').modal('hide');
            //if (resultado[0] == "002") {
            //    MensajeAdvertencia(resultado[1]);
            //} else {

            if (resultado[0] != '002') {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
                ConsultarSubDetalleControl();
                ConsultarRegModalParametros(IdDetalle);
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
function ValidarIngresoParametros() {
    var valida = true;

    if ($('#cmbMuestra').prop('selectedIndex') == 0) {
        $('#cmbMuestra').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbMuestra').css('borderColor', '#ced4da');
    }
    if ($('#cmbTipoProducto').prop('selectedIndex') == 0) {
        $('#cmbTipoProducto').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbTipoProducto').css('borderColor', '#ced4da');
    }
    if ($('#cmbArea').prop('selectedIndex') == 0) {
        $('#cmbArea').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbArea').css('borderColor', '#ced4da');
    }
    if ($('#cmbParametro').prop('selectedIndex') == 0) {
        $('#cmbParametro').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbParametro').css('borderColor', '#ced4da');
    }
    if ($('#txtCantidadParametro').val() == '') {
        $('#txtCantidadParametro').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#txtCantidadParametro').css('borderColor', '#ced4da');
    }
    return valida;
}
function LimpiarParamModal() {
    $('#cmbMuestra').prop('selectedIndex', 0);
    $('#cmbTipoProducto').prop('selectedIndex', 0);
    $('#cmbArea').prop('selectedIndex', 0);
    $('#cmbParametro').prop('selectedIndex', 0);
    SubDxParam = 0;
    $('#cmbMuestra').prop('disabled', false);
    $('#cmbTipoProducto').prop('disabled', false);
    $('#cmbArea').prop('disabled', false);
    $('#cmbParametro').prop('disabled', false);
    $('#txtCantidadParametro').val('');
    $('#cmbParametro').empty();
    $('#cmbParametro')
        .append($("<option></option>")
            .text('SELECCIONE..'));
}
function ModificarParamSubDetalleModal(data) {
    //console.log(data);
    $('#cmbMuestra').val(data.NMuestra);
    $('#cmbTipoProducto').val(data.CodTipoProducto);
    $('#cmbArea').val(data.CodArea);
    $('#txtCantidadParametro').val(data.Cantidad)
    ConsultarParamxArea(data.IdParametro);
    
    $('#cmbMuestra').prop('disabled',true);
    $('#cmbTipoProducto').prop('disabled', true);
    $('#cmbArea').prop('disabled', true);
    $('#cmbParametro').prop('disabled', true);
    SubDxParam = data.IdTipoxParametro;

    $('#txtCantidadParametro').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: data.Mascara
    });
    $('#spancantidad').text(data.Mascara);
}
function ConsultarRegModalParametros(IdDetalle) {

    $('#cargac').show();
    Error = 0;
    let params = {
        IdDetalle: IdDetalle
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/ConsultarSubDetalle_ParamxSubdetalle?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '0') {
                DevExpress.localization.locale(navigator.language);
                opciosGrid.dataSource = resultado;
                $("#gridContainer").dxDataGrid(opciosGrid).dxDataGrid("instance");
                
            } else {
              
            }
            $('#cargac').hide();
        })
        .catch(function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
function GuardarSubDetalle_ParamxSubdetalle() {
    if (!ValidarIngresoParametros()) {
        return;
    }
    var parametro =
    {
        ParametroLaboratorio: $('#cmbParametro').val(),
        Cantidad: $('#txtCantidadParametro').val(),
        TipoProducto: $('#cmbTipoProducto').val(),
        IdDetalle: IdDetalle,
        EstadoRegistro:'A'
    };
    var data = JSON.stringify({
        poParamxDetalle:parametro,
        poFecha: $('#txtFechaProduccion').val(),
        Idcabecera: IdCabecera

    });
    //console.log(data);
    fetch("../AnalisisQuimicoProductoSemielaborado/GuardarSubDetalle_ParamxSubdetalle", {
        method: 'POST',
        headers: {
            "Content-Type": "application/json"
        },
        body: data
    }).then(function (respuesta) {
        if (!respuesta.ok) {
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
            ConsultarRegModalParametros(IdDetalle);
            //LimpiarParamModal();
            
            ConsultarSubDetalleControl(1);
            if (resultado[0] == "002") {
                MensajeAdvertencia(resultado[1]);
            } else {
                MensajeCorrecto(resultado[1]);
            }
            if (resultado[0] == "000") {
                $('#txtCantidadParametro').val('');
            }
            if (resultado[0] == "001") {
                LimpiarParamModal();
            }
        }

    })
    .catch(function (resultado) {
        MensajeError("Error comuníquese con el departamento de Sistemas", false);

    })
}
function ConsultarMascara() {
    
    var Mascara = Enumerable.From(paramSeleccionado)
        .Where(function (x) { return x.IdParametro == $('#cmbParametro').val() })
        .Select(function (x) { return x.Mascara})
        .SingleOrDefault();
    //console.log(objeto);
    $('#txtCantidadParametro').inputmask({
        alias: "decimal",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: Mascara
    });
    $('#spancantidad').text(Mascara);
}
function ConsultarParamxArea(IdParametro) {
    if (IdParametro == null) {
        $('#cargac').show();
    }
    
    Error = 0;
    let params = {
        IdArea: $('#cmbArea').val()
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');

    let url = '../AnalisisQuimicoProductoSemielaborado/ConsultarParametroxArea?' + query;
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '0') {
                $('#cmbParametro').empty();
                $('#cmbParametro')
                    .append($("<option></option>")
                        .text('SELECCIONE..'));
                resultado.forEach(function (value) {
                    $('#cmbParametro')
                        .append($("<option></option>")
                            .attr("value", value.IdParametro)
                            .text(value.NombreParametro));
                });
                paramSeleccionado = resultado;
                if (IdParametro != null) {
                    $('#cmbParametro').val(IdParametro);
                }
            } else {
                $('#cmbParametro').empty();
                $('#cmbParametro')
                    .append($("<option></option>")
                        .text('SELECCIONE..'));
            }
            $('#cargac').hide();
        })
        .catch(function (resultado) {
            MensajeError(resultado.responseText, false);
            $('#cargac').hide();
        })
}
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
    $('.modal-dialog').draggable({
        handle: ".modal-header"
    });
    LimpiarControlesDetalle();

    $('#btnOrden').prop('disabled', false);
    $('#cmbLote').prop('disabled', false);
}
function AbrirModalSubDetalle() {
    $('#ModalSubDetalle').modal({ backdrop: 'static', keyboard: false, show: true });
    $('.modal-dialog').draggable({
        handle: ".modal-header"
    });
    IdSubDetalle = 0;
    LimpiarControlesSubDetalle();
    LimpiarParamModal();
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
            //console.log(ResultadoConsultar);
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
    data.append('Turno', $("#cmbTurno").val());
    var promesa = fetch("../AnalisisQuimicoProductoSemielaborado/ConsultarCabeceraControl", {
        method: 'POST',
        body: data
    })
    return promesa;
}
async function ConsultarCabControl(bandera) {

    try {


        if ($("#txtFechaProduccion").val() == '') {

            $('#msjErrorFechaProduccion').prop('hidden', false);
            return;
        } else {
            $('#msjErrorFechaProduccion').prop('hidden', true);
        }
        if ($('#cmbTurno').prop('selectedIndex') == 0) {

            $('#msjTurno').prop('hidden', false);
            return;
        } else {
            $('#msjTurno').prop('hidden', true);
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
            //console.log(ResultadoConsulta.IdAnalisisQuimicoProductoSe);
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
    data.append('Turno', $("#cmbTurno").val());
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
            if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == "444") {
                MensajeAdvertencia(resultado[1]);
             
                $('#Observacion').val(resultado[2].Observacion);
       
            } else {
                MensajeCorrecto(resultado[1]);
                //$('#brespacio').remove();
                $('#mensajeRegistros').text('');
                $('#DivDetalle').empty();
                $('#CardDetalle').prop('hidden', false);
                $('#brespacio').remove();
                //SlideCabecera();
                $('#CardDetalle').prop('hidden', false);
            }
            
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
    data.append('poFecha', $('#txtFechaProduccion').val());

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
        data.append('poFecha', $('#txtFechaProduccion').val());
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

                if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == '444') {
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
    ConsultarRegModalParametros(data.IdDetalleAnalisisQuimicoProductoSe);
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
    var ParametrosxTipo=[];
    IdArray.forEach(Objeto =>
        ParametrosxTipo.push({
            ParametroLaboratorio: Objeto.IdParametro, Cantidad: $('#' + Objeto.IdParametro).val()
        })
    );
  
    if (!ValidarSubDetallle()) {
        return;
    } else {
        $('#cargac').show();
        var data ={
            IdTipoAnalisisQuimicoProductoSe: IdSubDetalle,
            TipoProducto: $("#cmbTipoProducto").val(),
            IdDetalleAnalisisQuimicoProductoSe: IdDetalle,
            CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO: ParametrosxTipo
        };
        var data2 = JSON.stringify({
            poSubDetalleControl: data,
            CabeceraControl: IdCabecera,
            poFecha: $('#txtFechaProduccion').val()
        });
        console.log(data);
        fetch("../AnalisisQuimicoProductoSemielaborado/GuardarSubDetalleControl", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: data2
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

                if (resultado[0] == "002" || resultado[0] == "003" || resultado[0] == "444") {
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
                MensajeError('Error en el Sistema, comuníquese con el departamento de sistemas', false);
                $('#cargac').hide();
            })
    }
    
}
function ConsultarSubDetalleControl(bandera) {
    if (bandera != 1) {
        $('#cargac').show();
    }
    
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
    LimpiarControlesSubDetalle();
    $('#ModalSubDetalle').modal('show');
    $('#cmbTipoProducto').val(data.TipoProducto);
    //$('#txtSalProceso').val(data.SalProceso);
    //$('#txtHistaminaProceso').val(data.HistaminaProceso);
    //$('#txtHumedadProceso').val(data.HumedadProceso);
    //$('#txtSalEmpaque').val(data.SalEmpaque);
    //$('#txtHistaminaEmpaque').val(data.HistaminaEmpaque); 
    IdSubDetalle = data.IdTipoAnalisisQuimicoProductoSe;
    data.CC_ANALISIS_QUIMICO_PRODUCTO_SEMIELABORADO_PARAMETROXTIPO.forEach(function (objeto) {
        $('#' + objeto.ParametroLaboratorio).val(objeto.Cantidad);
    });
}
function LimpiarControlesSubDetalle() {
    $('#cmbTipoProducto').prop('selectedIndex',0);
    //$('#txtSalProceso').val('');
    //$('#txtHistaminaProceso').val('');
    //$('#txtHumedadProceso').val('');
    //$('#txtSalEmpaque').val('');
    //$('#txtHistaminaEmpaque').val('');
    //IdArray.forEach(function (objeto) {
    //    $('#' + objeto.IdParametro).val('');
    //});
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
    data.append('poFecha', $('#txtFechaProduccion').val());

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
    data.append('poFecha', $('#txtFechaProduccion').val());

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
function ValidaVacio(input) {
    if (input.value != '') {
        $(input).css('borderColor', '#ced4da');
    }
    else {
        $('#' + input.id).css('borderColor', '#FA8072');
    }
}