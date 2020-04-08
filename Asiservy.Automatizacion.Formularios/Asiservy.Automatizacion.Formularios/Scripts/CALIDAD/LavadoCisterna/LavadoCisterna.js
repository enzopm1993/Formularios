var itemEditar = [];
var inputSelect = [];
var listaIdIntermedia = [];
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
            IdLavadoCisterna: itemEditar[0],
            Fecha: $("#txtFechaCabecera").val(),
            QuimUtilizados: $("#txtQUtilizados").val(),
            Observacion: $("#txtObservacion").val(),
            idMantCisterna: itemEditar[5],
            idIntermedia: listaIdIntermedia
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

function EliminarConfirmar(jdata) {
    $("#modalEliminarControl").modal("show");
    itemEditar = jdata;
    $("#myModalLabel").text("¿Desea Eliminar el registro?");
    
}

function EliminarCabeceraSi() {
    MostrarModalCargando();
    $.ajax({
        url: "../LavadoCisterna/EliminarLavadoCisterna",
        type: "POST",
        data: {
            IdLavadoCisterna: itemEditar[0],
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdLavadoCisterna");
                $("#modalEliminarControl").modal("hide");
                CerrarModalCargando();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera(0);
                MensajeCorrecto("Registro eliminado con Éxito");
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

function ActualizarCabecera(jdata, jIdIntermedia) {
    $("#txtFechaCabecera").val(moment(jdata[1]).format("YYYY-DD-MM"));
    $("#txtNCisterna").val(jdata[2]);
    $("#txtQUtilizados").val(jdata[3]);
    $("#txtObservacion").val(jdata[4]);
    $('#ModalIngresoCabecera').modal('show');
    itemEditar = jdata;
    listaIdIntermedia = jIdIntermedia;//Lista de IDIntermedia a eliminar
}

function ModalIngresoCabecera() {
    LimpiarCabecera();
    $('#ModalIngresoCabecera').modal('show');
    $("#txtFechaCabecera").val(moment($("#txtFecha").val()).format("YYYY-MM-DD"));
    itemEditar = [];
    listaIdIntermedia = [];
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
                                 <input type="checkbox" id="CheckNCisterna_`+ entry.NDescripcion + `" class="custom-control-input" name="check" value="` + entry.NDescripcion+`" />
                                <label class="custom-control-label" for="CheckNCisterna_`+ entry.NDescripcion + `" id="lblNCisterna">CISTERNA: ` + entry.NDescripcion +`</label>
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
                        var nomInput = '#CheckNCisterna_' + objSplit;
                        $(nomInput).prop('checked', true);
                    }
                });
            }
        });
    }
}

function AgregarCisternas() {
    var cadena = "";
    var cadenaIdIntermedia = "";
    $('#txtNCisterna').val('');
    inputSelect.forEach(function (entry) {
        var nomInput = '#CheckNCisterna_' + entry.NDescripcion;
        if ($(nomInput).prop('checked') == true) {
            cadena += $(nomInput).val() + ";";
            cadenaIdIntermedia += entry.IdCisterna + ";";//Guardo una cadena de IdMantCistterna para guardar en la tabla intermedia
        }
    });
    itemEditar[5] = cadenaIdIntermedia;
    $('#txtNCisterna').val(cadena);
    $('#ModalIngresoCisterna').modal('hide');
    $("#txtNCisterna").css('border', '');
}