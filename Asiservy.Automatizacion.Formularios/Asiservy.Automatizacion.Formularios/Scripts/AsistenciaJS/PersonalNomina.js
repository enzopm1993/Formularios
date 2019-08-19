
var d = new Date();
var dia = d.getDate();
var mes = ("0" + (d.getMonth() + 1));
var anio = d.getFullYear();
var fechatotal = anio + "-" + mes + "-" + dia

var dateControl = document.querySelector('input[type="date"]');
dateControl.value = fechatotal;


