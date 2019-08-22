function Guardar() {
    Mensaje("Registro Guardado Exitosamente");
}

function CambioDepartamento(valor) {
    var arrayValores = new Array(
        new Array(1, 1, "-	Calamidad domestica"),
        new Array(1, 2, "-	Comisión de servicios "),
        new Array(1, 3, "-	Asuntos particulares"),
        new Array(1, 4, "-	Vacaciones")
    );

    console.log(valor);
    if (valor == 0) {
        document.getElementById("selectMotivo").disabled = true;
    } else {
            document.getElementById("selectMotivo").options.length = 0;
            document.getElementById("selectMotivo").options[0] = new Option("Selecciona una opcion", "0");
            for (i = 0; i < arrayValores.length; i++) {
                    document.getElementById("selectMotivo").options[document.getElementById("selectMotivo").options.length] = new Option(arrayValores[i][2], arrayValores[i][1]);
            }
            document.getElementById("selectMotivo").disabled = false;
    }
    var selectArea = document.getElementById("selectArea");
    if (valor == 1) {
        selectArea.selectedIndex = 0;
        selectArea.disabled = false;
    } else {

        selectArea.selectedIndex = 0;
        selectArea.disabled = true;
    }
    CambioArea();
}

function CambioArea() {
    var e = document.getElementById("selectArea");
    var selectValor = e.options[e.selectedIndex].value;
    var labelLinea = document.getElementById("labelLinea");
    var SelectLinea = document.getElementById("selectLinea");
    if (selectValor == "1") {
        labelLinea.removeAttribute("hidden");
        SelectLinea.removeAttribute("hidden");
    }
    else {

        labelLinea.setAttribute("hidden", true);
        SelectLinea.setAttribute("hidden", true);
    }
}

function CambioHoraFecha() {
    var HoraDesde = document.getElementById("timeHoraDesde");
    var HoraHasta = document.getElementById("timeHoraHasta");
    var FechaDesde = document.getElementById("datetimeDesde");
    var FechaHasta = document.getElementById("datetimeHasta");
    var check = document.getElementById("switchHoraFecha").checked
    console.log(check);

    if (check) {
        HoraDesde.removeAttribute("readonly");
        HoraHasta.removeAttribute("readonly");
        FechaDesde.setAttribute("readonly",true);
        FechaHasta.setAttribute("readonly", true);
        console.log(FechaDesde);
        FechaDesde.value = "";
        FechaHasta.value = "";
    } else {

        HoraDesde.setAttribute("readonly", true);
        HoraHasta.setAttribute("readonly", true);
        FechaDesde.removeAttribute("readonly");
        FechaHasta.removeAttribute("readonly");
        HoraDesde.value = "00:00";
        HoraHasta.value = "00:00";
    }

    
}

//comboFind
$(function () {
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
                .addClass("custom-combobox")
                .insertAfter(this.element);

            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
                value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
                .appendTo(this.wrapper)
                .val(value)
                .attr("title", "")
                .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                .autocomplete({
                    delay: 0,
                    minLength: 0,
                    source: $.proxy(this, "_source")
                })
                .tooltip({
                    classes: {
                        "ui-tooltip": "ui-state-highlight"
                    }
                });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
                wasOpen = false;

            $("<a>")
                .attr("tabIndex", -1)
                .attr("title", "Show All Items")
                .tooltip()
                .appendTo(this.wrapper)
                .button({
                    icons: {
                        primary: "ui-icon-triangle-1-s"
                    },
                    text: false
                })
                .removeClass("ui-corner-all")
                .addClass("custom-combobox-toggle ui-corner-right")
                .on("mousedown", function () {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .on("click", function () {
                    input.trigger("focus");

                    // Close if already visible
                    if (wasOpen) {
                        return;
                    }

                    // Pass empty string as value to search for, displaying all results
                    input.autocomplete("search", "");
                });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
                .val("")
                .attr("title", value + " didn't match any item")
                .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });

    $("#combobox").combobox();
    $("#toggle").on("click", function () {
        $("#combobox").toggle();
    });
});



