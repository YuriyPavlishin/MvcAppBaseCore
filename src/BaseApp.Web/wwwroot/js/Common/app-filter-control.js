function appFilterControl(container) {
    var events = $(this);

    var uiElements = {
        container: $(container)
    };
    
    this.on = $.proxy(events.on, events);
    this.off = $.proxy(events.off, events);
    this.getFilteredColumns = getFilteredColumns;

    function init() {
        initUi();
        subscribeToEvents();

        refreshHighlights();
    }

    function subscribeToEvents() {
        getFilterControls().change(updateClearButtonStatus);
        $(":input[autocomplete]", uiElements.container).keyup(updateClearButtonStatus);

        uiElements.btnApply.click(function (e) {
            e.preventDefault();
            submitFilter();
        });
        uiElements.form.submit(function (e) {
            e.preventDefault();
            submitFilter();
        });
        uiElements.btnClear.click(function (e) {
            e.preventDefault();
            if (uiElements.btnClear.attr("disabled") === "disabled")
                return;
            getFilterControls().val(null);
            updateClearButtonStatus();
            submitFilter();
        });
    }

    function submitFilter() {
        var args = { filterData: uiElements.form.serialize(), filteredColumns: getFilteredColumns() };
        events.trigger("filter-control-changed", args);
        refreshHighlights();
    }

    function getFilteredColumns() {
        var columns = [];

        getNonEmptyFilterControls()
            .each(function (index, elem) {
                var column = $(elem).attr("data-highlight-column") || $(elem).attr("name");
                if (column) {
                    var splitted = column.split(",");
                    for (var i = 0; i < splitted.length; i++) {
                        columns.push(splitted[i]);
                    }
                }
            });
        
        return columns;
    }

    function updateClearButtonStatus() {
        var isNonEmpty = getNonEmptyFilterControls().length > 0;
        uiElements.btnClear.prop("disabled", !isNonEmpty);
    }

    function refreshHighlights() {
        updateClearButtonStatus();
        getFilterControls().each(function () {
            highlightFilterElement($(this));
        });
    }

    function highlightFilterElement($element) {
        $element.toggleClass("highlight", isNonEmptyFilterControl($element));
    }

    function initUi() {
        uiElements.btnApply = $("[data-apply]", uiElements.container);
        uiElements.btnClear = $("[data-clear]", uiElements.container);
        uiElements.form = uiElements.container;
    }

    function getFilterControls() {
        return $(":input", uiElements.container);
    }

    function getNonEmptyFilterControls() {
        return getFilterControls()
            .filter(function(i, elem) { return isNonEmptyFilterControl(elem); });
    }

    function isNonEmptyFilterControl(elem) {
        return $(elem).val() != "";
    }
    

    init();
}