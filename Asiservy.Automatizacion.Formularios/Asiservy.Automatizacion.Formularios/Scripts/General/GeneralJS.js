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

    $('#exampleModalCenter').modal();
}


function CerrarModalCargando() {
  
    $('#exampleModalCenter').modal("hide");
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
