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

