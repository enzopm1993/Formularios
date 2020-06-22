


function sololetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8 || tecla == 32 || tecla == 13) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function MostrarModalCargando() {
    $('#cargac').show();
    //$('#Cargac').modal();
}


function CerrarModalCargando() {
  
    $('#cargac').hide();

}


    window.onload = function () {
        if (typeof history.pushState === "function") {
        history.pushState("jibberish", null, null);
    window.onpopstate = function () {
        history.pushState('newjibberish', null, null);
    };
}
        else {
            var ignoreHashChange = true;
            window.onhashchange = function () {
                if (!ignoreHashChange) {
        ignoreHashChange = true;
    window.location.hash = Math.random();   
}
                else {
        ignoreHashChange = false;
    }
};
}
}




function CambioClave() {
    LimpiarModalCambioClave();
    $("#ModalCambioClave").modal("show");
}

function ValidarCambioClave() {
    var valida = true;
    if ($("#txtUsuario2").val() == '') {
        $("#txtUsuario2").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtUsuario2").css("border-color", "#ced4da");
    }
    if ($("#txtClaveActual").val() == '') {
        $("#txtClaveActual").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveActual").css("border-color", "#ced4da");
    }
    if ($("#txtClaveNueva").val() == '') {
        $("#txtClaveNueva").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveNueva").css("border-color", "#ced4da");
    }
    if ($("#txtClaveNuevaConfirmar").val() == '') {
        $("#txtClaveNuevaConfirmar").css("border-color", "#f71d06");
        valida = false;
    } else {
        $("#txtClaveNuevaConfirmar").css("border-color", "#ced4da");
    }
    //console.log(valida);
    return valida;
}

function LimpiarModalCambioClave() {
    $("#txtClaveActual").css("border-color", "#ced4da");
    $("#txtClaveNueva").css("border-color", "#ced4da");
    $("#txtClaveNuevaConfirmar").css("border-color", "#ced4da");
    $("#txtClaveActual").val('');
    $("#txtClaveNueva").val('');
    $("#txtClaveNuevaConfirmar").val('');
}

function CambiarClave(e) {
    if (e != null) {
        if (e.keyCode != 13) {          
            return;
        }
    }
    if (!ValidarCambioClave()) {
        return;
    }
    $("#btnCerrarModal").prop("hidden", true);
    $("#btnGuardarCambioClave").prop("hidden", true);
    $("#btnCargando").prop("hidden", false);
    $.ajax({
        type: "GET",
        url: "../Login/CambiarClave",
        data:
        {
            Usuario: $("#txtUsuario2").val().trim(),
            claveActual: $("#txtClaveActual").val(),
            clave1: $("#txtClaveNueva").val(),
            clave2: $("#txtClaveNuevaConfirmar").val()
        },
        success: function (result) {
            $("#btnCerrarModal").prop("hidden", false);
            $("#btnGuardarCambioClave").prop("hidden", false);
            $("#btnCargando").prop("hidden", true);
            if (result == "0") {
                MensajeAdvertencia("Faltan Parametros");
                return;
            }
            if (result == "1") {
                MensajeAdvertencia("Clave nueva no coincide");
                return;
            }
            if (result.Codigo == "0") {
                $("#ModalCambioClave").modal("hide");
                MensajeAdvertencia(result.Descripcion+", Clave Incorrecta");
                return;
            }
            if (result.Codigo == "1") {
                $("#ModalCambioClave").modal("hide");
                MensajeCorrecto(result.Descripcion)
            }

        },
        error: function (result) {
            $("#btnCerrarModal").prop("hidden", false);
            $("#btnGuardarCambioClave").prop("hidden", false);
            $("#btnCargando").prop("hidden", true);
            MensajeError(result);
            $("#ModalCambioClave").modal("hide");

        }
    });


}


function MostarModalColumns() {
    var columns = $('#tblDataTable').dataTable().dataTableSettings[0].aoColumns;
    var options = '';
    var contador = 0;
    var columnas = new Array();
    var contValidaTodos = 0;
   
    $.each(columns, function (i, v) {
        if (v.sTitle.trim() != '') {
            var col = parseInt(i);
            columnas.push(i);
            console.log(columnas);

            // Get the column API object
            var column = $('#tblDataTable').DataTable().column(col);
            // Toggle the visibility
            if (!column.visible()) {
                options += '<div class="col-6"><input onchange="OcultarColumnas(this.value)" type="checkbox" name="checkbox-' + i + '" id="checkbox-' + i + '" value="' + i + '" class="custom" />';
                options += '<label for="checkbox-' + i + '">' + v.sTitle + '</label></div>';
            } else {
                options += '<div class="col-6"><input checked onchange="OcultarColumnas(this.value)" type="checkbox" name="checkbox-' + i + '" id="checkbox-' + i + '" value="' + i + '" class="custom" />';
                options += '<label for="checkbox-' + i + '">' + v.sTitle + '</label></div>';
                contValidaTodos++;
            }
            contador++;
        }
    });
    var columnas = JSON.stringify(columnas);
    if (contValidaTodos > 0) {
        options += '<div class="col-6"><input checked onchange="TodosColumna(this.checked,' + columnas + ')" type="checkbox" name="checkbox-Todos" id="checkbox-Todos" class="custom" />';
        options += '<label  class="text-info"  for="checkbox-Todos">' + "Todos" + '</label></div>';
    } else {
        options += '<div class="col-6"><input  onchange="TodosColumna(this.checked,' + columnas + ')" type="checkbox" name="checkbox-Todos" id="checkbox-Todos" class="custom" />';
        options += '<label  class="text-info"  for="checkbox-Todos">' + "Todos" + '</label></div>';
    }
    $('#ModalBody').html('');
    $('#ModalBody').append(options);

    if (contador > 0) {       
        $("#ModalColumnas").modal("show");
    } else {
        MensajeAdvertencia("No aplica para este reporte.");
    }
}


function OcultarColumnas(col) {
    var col = parseInt(col);   
    var column = $('#tblDataTable').DataTable().column(col); 
    column.visible(!column.visible());
}

function TodosColumna(check, columnas) {
    console.log(check);
    console.log(columnas);
    if (check) {
        $.each(columnas, function (index,value) {
            var column = $('#tblDataTable').DataTable().column(value);
            column.visible(true);
            $("#checkbox-" + value).prop("checked", true);
        });
    }
    else {
        $.each(columnas, function (index, value) {
            var column = $('#tblDataTable').DataTable().column(value);
            column.visible(false);
            $("#checkbox-" + value).prop("checked", false);

        });
    }

}

