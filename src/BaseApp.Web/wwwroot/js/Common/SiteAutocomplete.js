var SiteAutocomplete = new function() {
    this.init = function() {
        $("input[data-autocomplete-url]:not(.processed)").each(function() {

            var $this = $(this);
            $this.addClass("processed");

            $this.autocomplete(
                {
                    source: function(request, response) {
                        $.ajax({
                            type: "POST",
                            url: $this.attr("data-autocomplete-url"),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: $.toJSON({ prefixText: request.term, count: ($this.attr("data-itemscount") || '20'), contextKey: ($this.attr("data-contextKey") || "") }),
                            success: function(data) {
                                response($.map(data, function(item) {
                                    return {
                                        label: item.Label,
                                        value: item.Value
                                    };
                                }));
                            } /*, --COMMENTED because all ajax errors handled by default in $(document).ajaxError method 
                                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                alert("Autocomplete internal error occured. See log for details.");
                                            }*/
                        });
                    },
                    minLength: 0,
                    delay: 100,
                    select: function(event, ui) {
                        if (ui.item.value == '[No matches found]')
                            ui.item.value = '';

                        //fire asp.net validators for current control
                        //if (typeof Page_ClientValidate != "undefined") Page_ClientValidate(this);
                    },
                    open: function() {
                        //"ui-corner-top" css class used for getting open/closed status of autocomplete also
                        $(this).removeClass("ui-corner-all").addClass("ui-corner-top").addClass("autocompleteOpen");
                    },
                    close: function(event, ui) {
                        $(this).removeClass("ui-corner-top").addClass("ui-corner-all").removeClass("autocompleteOpen");

                        if ($this.attr("data-onautocompleteclose")) {
                            eval($this.attr("data-onautocompleteclose") + "(event, ui)");
                        }
                    }
                }
            );
            /* SAMPLE CUSTOM LIST ITEM RENDERING
            .data("autocomplete")._renderItem = function(ul, item) {
            return $("<li></li>")
            .data("item.autocomplete", item)
            .append("<a>" + item.value + "<br><b>" + item.label + "</b></a>")
            .appendTo(ul);
            };*/

            $this.dblclick(function() {
                $this.autocomplete("search");
            });
        });
    };
};