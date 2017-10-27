class appFilterControl {
    private events = $(this);

    private uiElements = {
        container: null as JQuery,
        form: null as JQuery,
        btnApply: null as JQuery,
        btnClear: null as JQuery
    };

    constructor(container: JQuery) {
        this.uiElements.container = container;
        this.initUi();
        this.subscribeToEvents();
        this.refreshHighlights();
    }

    on = $.proxy(this.events.on, this.events);
    off = $.proxy(this.events.off, this.events);

    getFilteredColumns(): string[] {
        var columns: string[] = [];
        
        this.getNonEmptyFilterControls()
            .each((index, elem) => {
                var column = $(elem).attr("data-highlight-column") || $(elem).attr("name");
                if (column) {
                    var splitted = column.split(",");
                    for (let i = 0; i < splitted.length; i++) {
                        columns.push(splitted[i]);
                    }
                }
            });
        
        return columns;
    }


    private subscribeToEvents() {
        this.getFilterControls().change(() => { this.updateClearButtonStatus(); } );
        $(":input[autocomplete]", this.uiElements.container).keyup( () => { this.updateClearButtonStatus(); });

        this.uiElements.btnApply.click(e => {
            e.preventDefault();
            this.submitFilter();
        });

        this.uiElements.form.submit(e => {
            e.preventDefault();
            this.submitFilter();
        });
        this.uiElements.btnClear.click(e => {
            e.preventDefault();
            if (this.uiElements.btnClear.attr("disabled") === "disabled")
                return;
            this.getFilterControls().val(null);
            this.updateClearButtonStatus();
            this.submitFilter();
        });
    }

    private submitFilter() {
        var args = { filterData: this.uiElements.form.serialize(), filteredColumns: this.getFilteredColumns() };
        
        this.events.trigger("filter-control-changed", args);
        this.refreshHighlights();
    }

    private updateClearButtonStatus() {
        const isNonEmpty = this.getNonEmptyFilterControls().length > 0;
        this.uiElements.btnClear.prop("disabled", !isNonEmpty);
    }

    private refreshHighlights() {
        this.updateClearButtonStatus();
        this.getFilterControls().each((i, el) => this.highlightFilterElement($(el)));
    }

    private highlightFilterElement($element: JQuery) {
        $element.toggleClass("highlight", this.isNonEmptyFilterControl($element));
    }
    

    private getFilterControls():JQuery {
        return $(":input", this.uiElements.container);
    }

    private getNonEmptyFilterControls(): JQuery {
        return this.getFilterControls()
            .filter((i, elem) => this.isNonEmptyFilterControl(elem));
    }

    private isNonEmptyFilterControl(elem:Element|JQuery):boolean {
        return $(elem).val() !== "";
    }


    private initUi() {
        this.uiElements.btnApply = $("[data-apply]", this.uiElements.container);
        this.uiElements.btnClear = $("[data-clear]", this.uiElements.container);
        this.uiElements.form = this.uiElements.container;
    }
}