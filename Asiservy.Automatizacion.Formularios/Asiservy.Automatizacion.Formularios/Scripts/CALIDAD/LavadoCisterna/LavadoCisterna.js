var itemEditar = [];
var inputSelect = [];
var listaIdMantCisterna = [];
$(document).ready(function () {
    CargarCabecera(0);
    
});
function CargarCabecera(op) {
    MostrarModalCargando();
    $.ajax({
        url: "../LavadoCisterna/LavadoCisternaPartial",
        data: {
            fechaDesde:$("#txtFecha").val(),
            fechaHasta: $("#txtFecha").val(),
            op: op
        },
        type: "GET",
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarTablaCabecera").html("No existen registros");
            } else {
                $("#divMostarTablaCabecera").html(resultado);
            }
            itemEditar = 0;
            setTimeout(function () {
                CerrarModalCargando();
            }, 500);
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    MostrarModalCargando();
    $.ajax({
        url: "../LavadoCisterna/GuardarModificarLavadoCisterna",
        type: "POST",
        data: {
            IdLavadoCisterna: itemEditar.IdLavadoCisterna,
            Fecha: $("#txtFechaCabecera").val(),
            QuimUtilizados: $("#txtQUtilizados").val(),
            Observacion: $("#txtObservacion").val()           
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera(0);
            setTimeout(function () {
                LimpiarCabecera();
                $('#ModalIngresoCabecera').modal('hide');
                CerrarModalCargando();
            }, 500);
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function InactivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea inactivar el registro?");
    itemEditar.EstadoRegistro = 'I';
}

function ActivarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    $("#myModalLabel").text("¿Desea activar el registro?");
    itemEditar = jdata;
    itemEditar.EstadoRegistro = 'A';
}

function EliminarCabeceraSi(estado) {
    MostrarModalCargando();
    $.ajax({
        url: "../LavadoCisterna/EliminarLavadoCisterna",
        type: "POST",
        data: {
            IdCisterna: itemEditar.IdCisterna,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdDesinfeccionManos");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                setTimeout(function () {
                    CerrarModalCargando();
                }, 500);
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            CerrarModalCargando();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ActualizarCabecera(jdata) {
    $("#txtNDescripcion").val(jdata.NDescripcion);
    $("#txtUbicacion").val(jdata.Ubicacion);
    $("#txtAsignacion").val(jdata.Asignacion);
    $("#txtTipo").val(jdata.Tipo);
    $("#txtCapacidad").val(jdata.Capacidad);
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = jdata;
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = [];
}

function LimpiarCabecera() {
    $('#txtNCisterna').val('');
    $('#txtQUtilizados').val('');
    $('#txtObservacion').val('');    
    $("#txtFechaCabecera").css('border', '');
    $("#txtNCisterna").css('border', '');
    $("#txtQUtilizados").css('border', '');
    $("#txtObservacion").css('border', '');    
}

function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera();
}

function OnChangeTextBox() {
    var ddl = $('#txtNDescripcion').val();
    //var sel = document.getElementById('txtNDescripcion');
    //var selected = sel.options[sel.selectedIndex].text;
    if (ddl!=null) {
        $('#ddl').text(ddl);
        console.log($('#ddl').val(ddl));
    }
    var con = 0;
    if ($('#txtFechaCabecera').val() == '') {
        $("#txtFechaCabecera").css('border', '1px dashed red');
        con = 1;
    } else $("#txtFechaCabecera").css('border', '');
    if ($('#txtNCisterna').val() == '') {
        $("#txtNCisterna").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNCisterna").css('border', '');
    if ($('#txtQUtilizados').val() == '') {
        $("#txtQUtilizados").css('border', '1px dashed red');
        con = 1;
    } else $("#txtQUtilizados").css('border', '');
    if ($('#txtObservacion').val() == '') {
        $("#txtObservacion").css('border', '1px dashed red');
        con = 1;
    } else $("#txtObservacion").css('border', '');
    
    return con;
}

function MultiSelectCisterna(jdata) {
    inputSelect = jdata;
    $('#divFila').empty();
    jdata.forEach(function (entry) {
        $('#ModalIngresoCisterna').modal('show');    
        const div = document.createElement('div');
        div.className = 'col-md-12 col-6 col-sm-6';
        div.innerHTML = `
                        <div class="form-group">                            
                            <div class="custom-control custom-checkbox mb-3">
                                 <input type="checkbox" text="`+ entry.IdCisterna+`" id="CheckNCisterna`+ entry.NDescripcion + `" class="custom-control-input" name="check" value="` + entry.NDescripcion+`" />
                                <label class="custom-control-label" for="CheckNCisterna`+ entry.NDescripcion + `" id="lblNCisterna">CISTERNA: ` + entry.NDescripcion +`</label>
                            </div>
                        </div>
                        `;        
        document.getElementById('divFila').appendChild(div);
    });
    if ($('#txtNCisterna').val()!='') {
        var splitNDescripcion = $('#txtNCisterna').val().split(';');
        splitNDescripcion.forEach(function (objSplit) {
            if (objSplit!='') {
                jdata.forEach(function (objLista) {
                    if (objSplit == objLista.NDescripcion) {
                        var nomInput = '#CheckNCisterna' + objSplit;
                        $(nomInput).prop('checked', true);
                    }
                });
            }
        });
    }
}

function AgregarCisternas() {
    var cadena = "";
    $('#txtNCisterna').val('');
    inputSelect.forEach(function (entry) {
        var nomInput = '#CheckNCisterna' + entry.NDescripcion;
        if ($(nomInput).prop('checked') == true) {
            cadena += $(nomInput).val() + ";";
            listaIdMantCisterna += $(nomInput).text() + ";";
        }
    });
    $('#txtNCisterna').val(cadena);
    $('#ModalIngresoCisterna').modal('hide');
    $("#txtNCisterna").css('border', '');
}