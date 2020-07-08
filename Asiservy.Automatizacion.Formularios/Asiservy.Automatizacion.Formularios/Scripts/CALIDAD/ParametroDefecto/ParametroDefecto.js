Error = 0;
var ObjetoDefecto;
IdCabecera = 0
IdMantDefecto = 0;
IdDetalle = 0;
$(document).ready(function () {
    $('#txtMaximoDet').inputmask({
        alias: "integer",
        clearMaskOnLostFocus: true,
        'digitsOptional': true,
        'digits': 2,
        max: 100
    });
    ConsultarCabeceras();
    CargarDefectos();

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
function GuardarCabceraControl() {
    if (!ValidaCabDefectos()) {
        return;
    }
    $('#cargac').show();

    fetch("../ParametroDefecto/GuardarCabecera", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            IdParametroDefecto:IdCabecera,
            Formulario: $('#cmbFormulario').val(),
            Tipo: $('#cmbProducto').val(),
            NivelLimpieza: $('#cmbNivelLimpieza').val(),
            ColorDentroDeRango: $('#cmbColorRangoDentro').val(),
            ColorFueraDeRango: $('#cmbColorRangoFuera').val(),
            EstadoRegistro: $('#EstadoRegistro').prop('checked')?'A':'I',
        })
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError(Mensajes.Error);
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
                $('#ModalConfirmarActualizar').modal('show');
                ObjetoDefecto = resultado[2];

            } else {
                MensajeCorrecto(resultado[1]);
                //$('#brespacio').remove();
                ////$('#mensajeRegistros').text('');
                //$('#DivDetalle').empty();
                //$('#CardDetalle').prop('hidden', false);
                $('#brespacio').remove();
                $('#ModalCabecera').modal('hide');
                ConsultarCabeceras();
       
            }
            //$('#CardDetalle').prop('hidden', false);
        }

        $('#btnGuardar').prop('hidden', false);
        $('#cargac').hide();

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(Mensajes.Error, false);
            $('#cargac').hide();
        })
}
function ConsultarCabeceras() {
    $('#cargac').show();
    Error = 0;

    let url = '../ParametroDefecto/PartialCabecerasParametroDefectos?';
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
                $('#CardCabeceras').prop('hidden', false);
                $('#DivCabeceras').empty();
                $('#DivCabeceras').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableCabeceras').DataTable(config.opcionesDT);

                //$('#brespacio').remove();
                //LimpiarDetalleControles();


            } else {
                $('#DivCabeceras').empty();
            }
            $('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(Mensajes.Error, false);
            ////$('#btnCargando').prop('hidden', true);
            ////$('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })
}

function MostrarDetalle(data) {
    $('#botonesdetalle').prop('hidden', false);
    $('#CardCabeceras').prop('hidden', true);
    $('#CardCab').prop('hidden', true);
    $('#CardDetalle').prop('hidden', false);
    $('#espacio').prop('hidden', false);
    IdCabecera = data.IdParametroDefecto;
    $('#cmbFormulario').val(data.Formulario);
    $('#cmbProducto').val(data.Tipo);

    $('#cmbNivelLimpieza').val(data.NivelLimpieza);

    $('#cmbColorRangoDentro').val(data.ColorDentroDeRango);

    $('#cmbColorRangoFuera').val(data.ColorFueraDeRango);
    if (data.EstadoRegistro == 'A') {
        $('#EstadoRegistro').prop('checked', true);
    } else {
        $('#EstadoRegistro').prop('checked', false);
    }
    $('#cmbFormulario').prop('disabled', true);
    $('#cmbProducto').prop('disabled', true);
    $('#cmbNivelLimpieza').prop('disabled', true);
    CargarDetalles(IdCabecera);

}
function Atras() {
    $('#botonesdetalle').prop('hidden', true);
    $('#CardCabeceras').prop('hidden', false);
    $('#CardCab').prop('hidden', false);
    $('#CardDetalle').prop('hidden', true);
    $('#espacio').prop('hidden', true);
    ConsultarCabeceras();
}
function AbrirModalCabecera() {
    $('#ModalCabecera').modal('show');
    LimpiarCabecera();
}
function EditarCabeceraModal() {
    $('#ModalCabecera').modal('show');
    $('#cmbFormulario').css('borderColor', '#ced4da');
    $('#cmbProducto').css('borderColor', '#ced4da');
    $('#cmbNivelLimpieza').css('borderColor', '#ced4da');
}
function LimpiarCabecera() {
    $('#cmbFormulario').prop('disabled', false);
    $('#cmbProducto').prop('disabled', false);
    $('#cmbNivelLimpieza').prop('disabled', false);
    $('#cmbFormulario').prop('selectedIndex', 0);
    $('#cmbProducto').prop('selectedIndex', 0);
    $('#cmbNivelLimpieza').prop('selectedIndex', 0);
    $('#cmbColorRangoDentro').prop('selectedIndex', 0);
    $('#cmbColorRangoFuera').prop('selectedIndex', 0);
    IdCabecera = 0;
    $('#LabelEstado').text('Activo');
    $('#EstadoRegistro').prop("checked", true);  
    $('#cmbFormulario').css('borderColor', '#ced4da');
    $('#cmbProducto').css('borderColor', '#ced4da');
    $('#cmbNivelLimpieza').css('borderColor', '#ced4da');
}
function CambioEstado(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstado').text('Activo');
    else
        $('#LabelEstado').text('Inactivo');

}
function CambioEstadoMant(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstadoMant').text('Activo');
    else
        $('#LabelEstadoMant').text('Inactivo');

}
function CambioEstadoDetalle(valor) {
    // console.log(valor);
    if (valor)
        $('#LabelEstadoDetalle').text('Activo');
    else
        $('#LabelEstadoDetalle').text('Inactivo');

}
function ActualizarCabecera() {
    fetch("../ParametroDefecto/GuardarCabecera", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ObjetoDefecto)
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError(Mensajes.Error);
            Error = 1;
        }
        return respuesta.json();
    }).then(function (resultado) {
        //console.log(respuesta);
        if (resultado == "101") {
            window.location.reload();
        }
        if (Error == 0) {
                MensajeCorrecto(resultado[1]);
                ////$('#brespacio').remove();
                //$('#mensajeRegistros').text('');
                ////$('#DivDetalle').empty();
                ////$('#CardDetalle').prop('hidden', false);
                $('#brespacio').remove();
                $('#ModalCabecera').modal('hide');
                $('#ModalConfirmarActualizar').modal('hide'); 
                ConsultarCabeceras();
        }

        $('#btnGuardar').prop('hidden', false);

    })
    .catch(function (resultado) {
        //console.log('error');
        //console.log(resultado);
        $('#ModalCabecera').modal('hide');
        $('#ModalConfirmarActualizar').modal('hide');  
        MensajeError(Mensajes.Error, false);

    })
}
function AbrirModalDetalle() {
    $('#ModalDetalle').modal('show');
    LimpiarDetalle();
}
function ValidaCabDefectos() {
    var valida = true;
    if ($('#cmbFormulario').prop('selectedIndex') == 0) {
        valida = false;
        $('#cmbFormulario').css('borderColor', '#FA8072');
    } else {
        $('#cmbFormulario').css('borderColor', '#ced4da');
    }
    if ($('#cmbProducto').prop('selectedIndex') == 0) {
        $('#cmbProducto').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbProducto').css('borderColor', '#ced4da');
    }
    if ($('#cmbNivelLimpieza').prop('selectedIndex') == 0) {
        $('#cmbNivelLimpieza').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#cmbNivelLimpieza').css('borderColor', '#ced4da');
    }
    return valida;
}
function ValidaDetDefectos() {
    var valida = true;
    if ($('#cmbDefecto').prop('selectedIndex') == 0) {
        valida = false;
        $('#cmbDefecto').css('borderColor', '#FA8072');
    } else {
        $('#cmbDefecto').css('borderColor', '#ced4da');
    }
    if ($('#txtMaximoDet').val()=='') {
        $('#txtMaximoDet').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#txtMaximoDet').css('borderColor', '#ced4da');
    }
    
    return valida;
}
//MANTENIMIENTO DE DEFECTO
function AbrirModalMantDefecto() {
    $('#ModalDefecto').modal('show');
    LimpiarMantDefecto();
}
function LimpiarMantDefecto() {
    IdMantDefecto = 0;
    $('#txtNombreDefecto').val('');
    $('#LabelEstadoMant').text('Activo');
    $('#EstadoRegistroDefecto').prop('checked', true);
}
function ValidaMantDefecto() {
    var valida = true;
    if ($('#txtNombreDefecto').val() == '') {
        $('#txtNombreDefecto').css('borderColor', '#FA8072');
        valida = false;
    } else {
        $('#txtNombreDefecto').css('borderColor', '#ced4da');
    }
    return valida;
}
function CargarDefectos() {
    let url = '../ParametroDefecto/PartialMantDefectos?';
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
            
                $('#DivDefectos').empty();
                $('#DivDefectos').html(resultado);
                config.opcionesDT.pageLength = 10;
                $('#TableMantDefectos').DataTable(config.opcionesDT);

                //$('#brespacio').remove();
                //LimpiarDetalleControles();


            } else {
                $('#DivDefectos').empty();
            }
            //$('#cargac').hide();
            //console.log(resultado);
        })
        .catch(function (resultado) {
            //console.log(resultado);
            MensajeError(Mensajes.Error, false);
            ////$('#btnCargando').prop('hidden', true);
            ////$('#btnConsultar').prop('hidden', false);
            //$('#cargac').hide();
        })
}
function EditarMantDefecto(data) {
    IdMantDefecto = data.IdDefecto;
    console.log(data);
    $('#txtNombreDefecto').val(data.Nombre);
    if (data.EstadoRegistro=='A') {
        $('#EstadoRegistroDefecto').prop('checked', true);
        $('#LabelEstadoMant').text('Activo');
    } else {
        $('#EstadoRegistroDefecto').prop('checked', false);
        $('#LabelEstadoMant').text('Inactivo');
    }
    
}
function GuardarMantDefecto() {
    if (!ValidaMantDefecto()) {
        return;
    }
    $('#cargac').show();
    Error = 0;
    fetch("../ParametroDefecto/GuardarMantDefecto", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            IdDefecto: IdMantDefecto,
            Nombre: $('#txtNombreDefecto').val(),
            EstadoRegistro: $('#EstadoRegistroDefecto').prop('checked') ? 'A' : 'I',
        })
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError(Mensajes.Error);
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
                LimpiarMantDefecto();
                CargarDefectos();
                LLenarComboDefectos();
            }
            //$('#CardDetalle').prop('hidden', false);
        }
        
        $('#cargac').hide();

    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(Mensajes.Error, false);
            $('#cargac').hide();

        })
}
function LLenarComboDefectos() {
    
    let url = '../ParametroDefecto/ConsultarDefectosMant?';
    fetch(url)
        //,body: data
        .then(function (respuesta) {
            return respuesta.json();
        })
        .then(function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado != '"0"') {
                $('#cmbDefecto').empty();
                $("#cmbDefecto").append(new Option("SELECCIONE..", ""));
                $.each(resultado, function (id, name) {
                    $('#cmbDefecto').append('<option value=' + name.IdDefecto + '>' + name.Nombre + '</option>');
                });
            } else {
                $('#cmbDefecto').empty();
                $("#cmbDefecto").append(new Option("SELECCIONE..", ""));
            }
       
        })
        .catch(function (resultado) {
           //console.log(resultado);
            MensajeError(Mensajes.Error, false);
     
        })
}
function GuardarDetalle() {
    if (!ValidaDetDefectos()) {
        return;
    }
    $('#cargac').show();
    fetch("../ParametroDefecto/GuardarDetalle", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            IdCabeceraParametro:IdCabecera,
            IdParametroDefectoDetalle: IdDetalle,
            Defecto: $('#cmbDefecto').val(),
            Maximo: $('#txtMaximoDet').val(),
            EstadoRegistro: $('#EstadoRegistroDetalle').prop('checked') ? 'A' : 'I'
        })
    }).then(function (respuesta) {
        if (!respuesta.ok) {
            //MensajeError(respuesta.statusText);
            MensajeError(Mensajes.Error);
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
                $('#ModalDetalle').modal('hide');
                CargarDetalles(IdCabecera);

            }
            //$('#CardDetalle').prop('hidden', false);
            $('#cargac').hide();
        }

    
    })
        .catch(function (resultado) {
            //console.log('error');
            //console.log(resultado);
            MensajeError(Mensajes.Error, false);
            $('#cargac').hide();

        })
   
}
function CargarDetalles(poIdCabecera) {
    $('#cargac').show();
    Error = 0;
    let params = {
        Cabecera: poIdCabecera
    }
    let query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&');
    let url = '../ParametroDefecto/PartialDetalleParametroDeDefectos?'+query;
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
                $('#TableDetParam').DataTable(config.opcionesDT);

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
            MensajeError(Mensajes.Error, false);
            ////$('#btnCargando').prop('hidden', true);
            ////$('#btnConsultar').prop('hidden', false);
            $('#cargac').hide();
        })
}
function LimpiarDetalle() {
    $('#cmbDefecto').prop('disabled', false);
    IdDetalle = 0;
    $('#cmbDefecto').prop('selectedIndex', 0);
    $('#txtMaximoDet').val('');
    $('#EstadoRegistroDetalle').prop("checked", true);
    $('#LabelEstadoDetalle').text('Activo');
    $('#txtMaximoDet').css('borderColor', '#ced4da');
    $('#cmbDefecto').css('borderColor', '#ced4da');
}
function EditarDetalle(data) {
    $('#txtMaximoDet').css('borderColor', '#ced4da');
    $('#cmbDefecto').css('borderColor', '#ced4da');
    IdDetalle = data.IdParametroDefectoDetalle;
    $('#cmbDefecto').prop('disabled', true);
    $('#cmbDefecto').val(data.Defecto);
    $('#txtMaximoDet').val(data.Maximo);
    if (data.EstadoRegistro == 'A') {
        $('#EstadoRegistroDetalle').prop("checked", true);
        $('#LabelEstadoDetalle').text('Activo');
    } else {
        $('#EstadoRegistroDetalle').prop("checked", false);
        $('#LabelEstadoDetalle').text('Inactivo');
    }
    $('#ModalDetalle').modal('show');
}
function ValidaVacio(input) {
    if (input.value != '') {
        $(input).css('borderColor', '#ced4da');
    }
}
function CambioColor(id, name) {
    //console.log('#' + name + 'option');
    //console.log($("#" + id).val());
    if ($("#" + id).prop('selectedIndex') != 0) {
        $("#" + id).css("color", $("#" + id).val());
        $('#'+name+' option').eq(0).css("color", 'black');
    } else {
        $("#" + id).css("color", 'black');
        $('#'+name+' option').eq(0).css("color", 'black');
    }
}