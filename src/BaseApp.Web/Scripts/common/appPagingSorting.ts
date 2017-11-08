class appPagingSorting {

    private uiElements = {
        container: null as JQuery,
        getTable: null as () => JQuery,
        getState: null as () => JQuery
    };
    private containerSelector: JQuery | string;
    private url: string;
    
    private filterFormSelector: JQuery | string;
    
    private filter: appFilterControl = null;


    constructor(containerSelector: JQuery|string, url: string, filterFormSelector: JQuery|string ) {
        this.containerSelector = containerSelector;
        this.url = url;
        this.filterFormSelector = filterFormSelector;
        this.uiElements.container = $(".paging-sorting-update", containerSelector);
        if (this.uiElements.container.length === 0) {
            var errMsg = `.paging-sorting-update not found in container '${containerSelector}' or container not found`;
            alert(errMsg);
            throw new Error(errMsg);
        }
        this.uiElements.getTable = () => $("table.paging-sorting", this.uiElements.container); 
        this.uiElements.getState = () => $("[data-paging-sorting-state]", this.uiElements.container);

        this.initFilterControl();
        this.subscribeToEvents();
    }

    private initFilterControl() {
        var filterContainer :JQuery;
        if (this.filterFormSelector) {
            filterContainer = $(this.filterFormSelector);
        } else {
            filterContainer = $(".paging-sorting-filter-form", this.containerSelector);
        }
        if (filterContainer.length > 0) {
            this.filter = new appFilterControl(filterContainer);
            this.refreshHighlights();
        }
    }

    private refreshHighlights() {
        if (this.filter) {
            this.highlightTableColumns(this.filter.getFilteredColumns());
        }
    }

    private highlightTableColumns(columns:string[]) {
        $("[data-sortmember]", this.uiElements.container).each((index, elem) => {
            var $this = $(elem);
            var currentColumn = $this.attr("data-sortmember");
            $this.toggleClass("highlight", columns.indexOf(currentColumn) >= 0);
        });
    }

    sendRequest(data: any, keepPaging:boolean) {
        var parametesData = this.getState();
        data = appSerializeObject(data);
        if (data) {
            $.extend(parametesData, data);
        }
        if (!keepPaging) {
            $.extend(parametesData.PagingSortingInfo, { Page: 1 });
        }

        return this.ajaxRequest(parametesData);
    }

    private subscribeToEvents() {
        this.subscribeToPaging();
        this.subscribeToSorting();
        if (this.filter) {
            this.filter.on("filter-control-changed", (e: any, args: any) => {
                this.ajaxRequest(args.filterData);
            });
        }
    }

    private subscribeToPaging() {
        this.uiElements.container.on("click", "a[data-Page]", (event) => {
            var $this = $(event.currentTarget);
            var page = $this.attr("data-Page");
            var data = { Page: page };
            this.pagingSortingChanged(data);
        });
    }

    private subscribeToSorting() {
        this.uiElements.container.on("click", ".paging-sorting th[data-SortMember]", (event) => {
            var $this = $(event.currentTarget);
            var sortMember = $this.attr("data-SortMember");
            var sortDescending = $this.attr("data-SortDescending");
            var data = {
                SortMember: sortMember,
                SortDescending: sortDescending
            };
            this.pagingSortingChanged(data);
        });
    }

    private pagingSortingChanged(changedData: any) {
        var parametesData = this.getState();
        var pagingSortingInfoData = parametesData["PagingSortingInfo"];
        $.extend(pagingSortingInfoData, changedData);
        parametesData["PagingSortingInfo"] = pagingSortingInfoData;

        this.ajaxRequest(parametesData);
    }

    private getState() {
        var stateControl = this.uiElements.getState();
        if (stateControl.length <= 0)
            return null;
        return JSON.parse(stateControl.attr("data-paging-sorting-state"));
    }

    private ajaxRequest(data:any) {
        var $self = $(this);

        let deff = $.ajax({
                url: this.url,
                data: data,
                cache: false,
                type: "POST",
                //beforeSend: function () { $self.trigger("request-start"); }
            })
            .done((html) => {
                this.uiElements.container.empty().append(html);
                //$self.trigger("request-success");
            })
            .always(() => {
                //$self.trigger("request-complete");
                this.refreshHighlights();
            });

        appAjaxAreaLoading(this.uiElements.getTable().find("tbody"), deff);

        return deff.promise();
    }
}