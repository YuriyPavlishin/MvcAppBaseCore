function FilterHighlight(settings) {
    var uiElements = {
        container: null,
        filter: {
            container: null,
            btnClear: null
        },
        table: null
    };

    function initUi() {
        uiElements.filter.container = settings.filterContainer;
        uiElements.filter.btnClear = settings.filterBtnClear;
        uiElements.table = settings.table;
    }

    function updateClearButtonStatus() {
        var isValid;
        $(":input", uiElements.filter.container).each(function () {
            if ($(this).is(".multiselect")) {
                if ($(this).val() && $(this).val().length) {
                    isValid = true;
                }
            } else {
                if ($(this).val() || $(this).val() != "") {
                    isValid = true;
                }
            }
        });
        if (isValid) {
            uiElements.filter.btnClear.removeAttr("disabled");
        } else {
            uiElements.filter.btnClear.attr("disabled", "disabled");
        }
    }

    function highlightFilterElement($element) {
        if ($element.val() != "") {
            if ($element.closest("span.k-dropdown") != undefined) {
                $element.closest("span.k-dropdown").addClass("highlight");
            }
            $element.addClass("highlight");
        } else {
            if ($element.closest("span.k-dropdown") != undefined) {
                $element.closest("span.k-dropdown").removeClass("highlight");
            }
            $element.removeClass("highlight");
        }
    }

    function highlightTableColumn($element) {
        var elementHasCustomAttribute = $element.attr("data-highlight-column") != null;
        var sortColumnName;
        if (elementHasCustomAttribute) {
            sortColumnName = $element.attr("data-highlight-column");
        } else {
            sortColumnName = $element.attr("name");
        }

        var $sortLink = $("[data-sortmember=\"" + sortColumnName + "\"]", uiElements.table);

        var elementValue = $element.val();
        if (elementValue) {
            $sortLink.addClass("highlight");
            $sortLink.closest(".k-header").addClass("highlight");
        } else {
            if (!elementHasCustomAttribute
                || $("[data-highlight-column=\"" + sortColumnName + "\"]:input", uiElements.filter.container).filter(function () { return $(this).val() != ""; }).size() == 0) {
                $sortLink.removeClass("highlight");
                $sortLink.closest(".k-header").removeClass("highlight");
            }
        }
    }

    function highlightElement($element) {
        highlightFilterElement($element);
        highlightTableColumn($element);
    }

    this.RefreshHighlights = function () {
        updateClearButtonStatus();
        $(":input", uiElements.filter.form).each(function () {
            highlightElement($(this));
        });
    };

    function init() {
        initUi();

        $(":input", uiElements.filter.container).each(function () {
            $(this).change(function () {
                updateClearButtonStatus();
            });
        });
        $(":input[autocomplete]", uiElements.filter.container).each(function () {
            $(this).keyup(function () {
                updateClearButtonStatus();
            });
        });

        updateClearButtonStatus();
    }

    init();
};

function FilterHighlightSettigs() {
    this.filterContainer = null;
    this.filterBtnClear = null;
    this.table = null;
};