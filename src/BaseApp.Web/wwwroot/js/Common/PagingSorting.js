function PagingSorting(containerSelector, url, filterFormSelector) {
    var self = this;

    var uiElements = {
        container: null,
        getTable: null,
        getState: null
    };
    var filter = null;

    function init() {
        initUi();
        subscribeToEvents();
    }

    this.SendRequest = function (data, keepPaging) {
        var parametesData = getState();
        data = SerializeObject.GetObject(data);
        if (data) {
            $.extend(parametesData, data);
        }
        if (!keepPaging) {
            $.extend(parametesData.PagingSortingInfo, { Page: 1 });
        }

        return ajaxRequest(parametesData);
    }

    function subscribeToEvents() {
        subscribeToPaging();
        subscribeToSorting();
        if (filter) {
            filter.on("filter-control-changed", function(e, args) {
                ajaxRequest(args.filterData);
            });
        }
    }

    function subscribeToPaging() {
        uiElements.container.on("click", "a[data-Page]", function () {
            var $this = $(this);
            var page = $this.attr("data-Page");
            var data = { Page: page };
            pagingSortingChanged(data);
        });
    }

    function subscribeToSorting() {
        uiElements.container.on("click", ".paging-sorting th[data-SortMember]", function () {
            var $this = $(this);
            var sortMember = $this.attr("data-SortMember");
            var sortDescending = $this.attr("data-SortDescending");
            var data = {
                SortMember: sortMember,
                SortDescending: sortDescending
            };
            pagingSortingChanged(data);
        });
    }
    
    function pagingSortingChanged(changedData) {
        var parametesData = getState();
        var pagingSortingInfoData = parametesData["PagingSortingInfo"];
        $.extend(pagingSortingInfoData, changedData);
        parametesData["PagingSortingInfo"] = pagingSortingInfoData;

        ajaxRequest(parametesData);
    }

    function getState() {
        var stateControl = uiElements.getState();
        if (stateControl.length <= 0)
            return null;
        return JSON.parse(stateControl.attr("data-paging-sorting-state"));
    }

    function ajaxRequest(data) {
        var $self = $(self);
        
        var deff = $.ajax({
            url: url,
            data: data,
            cache: false,
            type: "POST",
            beforeSend: function () { $self.trigger("request-start"); }
        })
        .done(function (html) {
            uiElements.container.empty().append(html);
            $self.trigger("request-success");
        })
        .always(function () {
            $self.trigger("request-complete");
            refreshHighlights();
        });

        ajaxAreaLoading(uiElements.getTable().find("tbody"), deff);

        return deff.promise();
    }

    function initUi() {
        uiElements.container = $(".paging-sorting-update", containerSelector);
        if (uiElements.container.length === 0) {
            var errMsg = ".paging-sorting-update not found in container '" + containerSelector + "' or container not found";
            alert(errMsg);
            throw new Error(errMsg);
        }
        uiElements.getTable = function () { return $("table.paging-sorting", uiElements.container); };
        uiElements.getState = function () { return $("[data-paging-sorting-state]", uiElements.container); };

        initFilterControl();
    }

    function initFilterControl() {
        var filterContainer;
        if (filterFormSelector) {
            filterContainer = $(filterFormSelector);
        } else {
            filterContainer = $(".paging-sorting-filter-form", containerSelector);
        }
        if (filterContainer.length > 0) {
            filter = new appFilterControl(filterContainer);
            refreshHighlights();
        }
    }

    function refreshHighlights() {
        if (filter) {
            highlightTableColumns(filter.getFilteredColumns());
        }
    }

    function highlightTableColumns(columns) {
        $("[data-sortmember]", uiElements.container).each(function() {
            var $this = $(this);
            var currentColumn = $this.attr("data-sortmember");
            $this.toggleClass("highlight", columns.indexOf(currentColumn) >= 0);
        });
    }

    init();
}