$(document).ready(function () {
    LLenarComboOrdenes();
});
$("#btnOrden").on("click", function () {
    $("#ModalOrdenes").modal('show');
});
$("#modal-orden-si").on("click", function () {
    if ($("#SelectOrdenFabricacion").prop('selectedIndex') == 0) {
        $('#validaOrden').prop("hidden", false);
        return;
    }
    $("#cmbOrdeneFabricacion").val($("#SelectOrdenFabricacion").val());
  
    //DatosOrdenFabricacion();

    $("#ModalOrdenes").modal('hide');
    $('#validaOrden').prop("hidden", true);

    //ConsultarCabControl();

});
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
