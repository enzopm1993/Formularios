var itemEditar = [];
var listaAnterior = [];
$(document).ready(function () {
    CargarCabecera();
});

function CargarCabecera() {
    $('#cargac').show();
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/MantAreaAuditoriaPartial",
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
                $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function GuardarCabecera() {
    $('#ModalIngresoCabecera').modal('hide');
    $('#cargac').show();
    var listaIdObjetoAuditar = new Array();
    $("input[type=checkbox]:checked").each(function (resultado) {
        id = $(this).attr("id");
        id = id.replace('Objeto-', '');
        if (id != "Objeto" && id != "switchHoraFecha") {
            listaIdObjetoAuditar.push(id);
        }
    });

    if (listaIdObjetoAuditar.length == 0) {
        $('#ModalIngresoCabecera').modal('show');        
        $('#cargac').hide();
        MensajeAdvertencia('¡Debe seleccionar un OBJETO para crear una Area de Auditoria!');
        return;
    }
    if (listaAnterior!=0) {//VALIDO SI LA LISTA ANTERIOR ESTA VACIA.
        listaIdObjetoAuditar.forEach(function (rowNew) {
            var con = 0;
            listaAnterior.forEach(function (rowPrevious) {
                if (rowNew == rowPrevious.IdObjeto) {
                    delete listaAnterior[con];//ELIMINO LOS ID CHECK LOS RESTANTES LOS ELIMINO
                }
                con++;
            });
        });
    }    
    
    $.ajax({
        url: "../HigieneComedorCocina/GuardarModificarAreaAuditoria",
        type: "POST",
        data: {
            IdAuditoria: itemEditar.IdAuditoria,
            NombreAuditoria: $('#txtNombre').val(),
            DescripcionAuditoria: $("#txtDescripcion").val(),
            listaIdObjetoAuditar: listaIdObjetoAuditar,
            listaIdObjetoEliminar: listaAnterior
        },
        success: function (resultado) {
            $('#ModalIngresoCabecera').modal('hide');
            if (resultado == "101") {
                window.location.reload();
            }
            CargarCabecera();
            if (resultado == 0) {
                MensajeCorrecto('Registro guardado correctamente');
            } else if (resultado == 1) {
                MensajeCorrecto('Registro actualizado correctamente');
            } else if (resultado == 2) {
                MensajeAdvertencia('El registro no se pudo Actualizar ¡Por favor ACTIVE y vuelva a intentar!');
            } else if (resultado == 3) {
                MensajeAdvertencia('Error al guardar el registro: No se permite espacios en blanco ni vacío');
                $('#ModalIngresoCabecera').modal('show');
                $("#txtNombre").css('border', '1px dashed red');
                $('#cargac').hide();
                return;
            } else {
                $('#ModalIngresoCabecera').modal('show');
                MensajeAdvertencia('Debe seleccionar un OBJETO para crear una Area de Auditoria');
                $('#cargac').hide();
                return;
            }
            $("#txtDescripcion").val('');
            LimpiarCabecera();
            $('#cargac').hide();
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function ModalIngresoCabecera(idObjeto) {
    document.getElementById("btnActualizarObjeto").disabled = true;
    var objIdObjeto = idObjeto;    
    if (idObjeto == true) {
        objIdObjeto = [0];
        LimpiarCabecera();
    } else if (idObjeto==0) {
        objIdObjeto = [0];
    }
    var estadoRegistro = 'A';
    $('#ModalIngresoCabecera').modal('show');    
    $.ajax({
        url: "../HigieneComedorCocina/ConsultarObjetosActivos",
        type: "GET",
        data: {
            estadoRegistro:estadoRegistro
        },
        success: function (resultado) {            
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                $("#divMostarObjetos").html("No existen registros");
            } else {
                var theHTML = "<table class='table table-bordered' style='cursor: pointer' id='tblDataTableObjeto'>\
                               <thead style='font-size: 14px'><tr>\
                               <th><div class='custom-control custom-checkbox'>\
                               <input type='checkbox'  class='custom-control-input' id='Objeto' onclick='SeleccionarTodos(this.checked)'>\
                               <label class='custom-control-label' for='Objeto'>Todos</label>\
                               </div></th>\
                               <th>Nombre</th>\
                               <th>Descripcion</th>\
                               </tr></thead>\
                               <tbody style='font-size: 14px'>";                    
                resultado.forEach(function (row) {
                    var check = '';
                    theHTML += "<tr>";
                    theHTML += "<td class='text-center'>\
                                     <div class='custom-control custom-checkbox'>";
                    objIdObjeto.forEach(function (rowObjeto) {
                        if (rowObjeto.IdObjeto == row.IdObjeto) {
                            check = 'checked';
                        }
                    });
                    theHTML += "<input type='checkbox' " + check + " class='custom-control-input' id='Objeto-" + row.IdObjeto + "'>";
                    theHTML += "<label class='custom-control-label' for='Objeto-" + row.IdObjeto + "'></label></div></td>";
                    theHTML += "<td>" + row.NombreObjeto + "</td>";
                    theHTML += "<td>" + row.DescripcionObjeto + "</td>";
                    theHTML += "</tr>";
                });                
                theHTML += "</tbody></table>";
                //LO ENVIO AL DIV QUE CONTIENE LA TABLA
                document.getElementById("divMostarObjetos").innerHTML = theHTML;
                setTimeout(function () {
                    document.getElementById("btnActualizarObjeto").disabled = false;
                },5000);
            }
        },
        error: function (resultado) {
            MensajeError(resultado.responseText, false);
        }
    });
}

function ActualizarCabecera(jdata) {
    if (jdata.EstadoRegistro == 'A') {
        $("#txtNombre").val(jdata.NombreAuditoria);
        $("#txtDescripcion").val(jdata.DescripcionAuditoria);
        $('#ModalIngresoCabecera').modal('show');
        itemEditar = jdata;
        $.ajax({
            url: "../LimpiezaDesinfeccionPlanta/ConsultarIntermediaActivos",
            type: "GET",
            data: {
                idAuditoria: jdata.IdAuditoria
            },
            success: function (resultado) {
                if (resultado == "101") {
                    window.location.reload();
                }
                ModalIngresoCabecera(resultado);
                listaAnterior = resultado;
            },
            error: function (resultado) {
                MensajeError(resultado.responseText, false);
            }
        });
        
    } else {
        MensajeAdvertencia('¡Por favor ACTIVE el registro y vuelva a intentar!');
    }
    
}

function LimpiarCabecera() {
    $("#txtNombre").val('');
    $("#txtDescripcion").val('');
    $("input[type=checkbox]").each(function (resultado) {
        id = $(this).attr("id");
        if (id != "switchHoraFecha") {
            $("#" + this.id).prop('checked', false);
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

function EliminarCabeceraSi() {
    $('#cargac').show();
    $.ajax({
        url: "../LimpiezaDesinfeccionPlanta/EliminarAreaAuditoria",
        type: "POST",
        data: {
            IdAuditoria: itemEditar.IdAuditoria,
            EstadoRegistro: itemEditar.EstadoRegistro
        },
        success: function (resultado) {
            if (resultado == "101") {
                window.location.reload();
            }
            if (resultado == "0") {
                MensajeAdvertencia("Falta Parametro IdAuditoria");
                $("#modalEliminarControl").modal("hide");
                $('#cargac').hide();
                return;
            } else if (resultado == "1") {
                $("#modalEliminarControl").modal("hide");
                CargarCabecera();
                MensajeCorrecto("Registro Actualizado con Éxito");
                setTimeout(function () {
                    $('#cargac').hide();
                }, 200);
            }
            itemEditar = 0;
        },
        error: function (resultado) {
            $('#cargac').hide();
            MensajeError(resultado.responseText, false);
        }
    });
}

function EliminarCabeceraNo() {
    $("#modalEliminarControl").modal("hide");
}

function ValidarDatosVacios() {
    var vacio = OnChangeTextBox();
    if (vacio == 1) {
        MensajeAdvertencia('¡Ingrese todo los datos requeridos!');
        return;
    } else GuardarCabecera();
}

function OnChangeTextBox() {
    var con = 0;
    if ($('#txtNombre').val() == '') {
        $("#txtNombre").css('border', '1px dashed red');
        con = 1;
    } else $("#txtNombre").css('border', '');   
    return con;
}

function SeleccionarTodos(check) {
    $("input[type=checkbox]").each(function (resultado) {
        id = $(this).attr("id");
        if (id != "switchHoraFecha") {
            $("#" + this.id).prop('checked', check);
        }
    });
}

function NuevoRegistro() {   
    var url = $("#RedirectTo").val();
    var win = window.open(url, '_blank');
}