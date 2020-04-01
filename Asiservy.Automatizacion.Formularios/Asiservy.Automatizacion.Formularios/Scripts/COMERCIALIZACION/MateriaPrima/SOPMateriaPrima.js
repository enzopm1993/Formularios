$(document).ready(function () {
    ComboAnio();
    $('#tblDataTable tbody').on('click', 'tr', function () {
        var table = $('#tblDataTable').DataTable();
        var data = table.row(this).data();
        SeleccionarCabecera(data);
    });
}); 
function CrearNuevoRegistro() {
    //$("#ModalIngresoCabecera").prop('hidden', false);
    $("#ModalIngresoCabecera").modal('show');
    $("#ModalIngresoCabecera").prop('hidden',false);
}

//FECHA DataRangePicker
function ComboAnio() {
    var n = (new Date()).getFullYear()
    var select = document.getElementById("selectAnio");
    for (var i = n; i >= 2015; i--) {
        select.options.add(new Option(i, i));
    }
}

function SeleccionarCabecera(model) {
    datosCabecera = model;
    var table = $("#tblDataTableCabecera");
    table.DataTable().clear();
    $('#ModalIngresoMeses').modal('show');
    $('#lblBarco').val('Barco: '+model[0]);
}

function CrearBarco() {
    $('#ModalIngresoBarco').modal('show');
    $('#ModalIngresoBarco').prop('hidden',false);
}

function ActualizarCabecera() {
    $('#ModalIngresoMeses').prop('hidden',true);
    $("#ModalIngresoMeses").modal('hide');
    $("#ModalIngresoCabecera").modal('show');
    $("#ModalIngresoCabecera").prop('hidden',false);
    //CerrarModalBarco();
}

function CerrarModalCabecera() {
    $('#ModalIngresoMeses').prop('hidden', true);
    $('#ModalIngresoMeses').modal('hide');
    $("#ModalIngresoBarco").prop('hidden', true);
    $("#ModalIngresoBarco").modal('hide');
    $("#ModalIngresoCabecera").prop('hidden', true);
    $("#ModalIngresoCabecera").modal('hide');
}

function CerrarModalBarco() {
    $("#ModalIngresoBarco").prop('hidden', true);
    $("#ModalIngresoBarco").modal('hide');
}