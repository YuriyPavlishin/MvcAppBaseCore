function PagingSorting(containerSelector, urlArg, filterFormSelector) {
    var self = this;
    var url;
    var fadeDuration = 250;
    var fadeColor = "white";

    var ui = {
        filter: {
            container: null,
            btnApply: null,
            btnClear: null,
        },
        updateTarget: {
            container: null,
            getTable: null,
            getPaging: null,
            getState: null
        }
    };

    var filterHighlight = null;

    function getState() {
        var stateControl = ui.updateTarget.getState();
        if (stateControl.length <= 0)
            return null;
        return JSON.parse(stateControl.attr("data-paging-sorting-state"));
    }

    function endableTableLoading(complete) {
        var table = ui.updateTarget.getTable();
        if (table.length) {
            var tableTbody = table.find("tbody");
            var tbodyPos = tableTbody.position();
            var divOverlay = $("<div data-loading-overlay=\"" + ui.updateTarget.container.selector + "\"></div>");

            divOverlay.css({
                position: "absolute",
                "background-color": fadeColor,
                display: "none",
                top: tbodyPos.top,
                left: tbodyPos.left,
                width: tableTbody.width() - 1,
                height: tableTbody.height()
            });

            tableTbody.before(divOverlay);
            divOverlay.fadeIn({ duration: fadeDuration, complete: complete });
        } else {
            if (complete) {
                complete();
            }
        }
    }

    function disableTableLoading(complete) {
        var divOverlay = $("[data-loading-overlay=\"" + ui.updateTarget.container.selector + "\"]");
        if (divOverlay.is(":visible")) {
            divOverlay.fadeOut({
                duration: fadeDuration,
                complete: function () {
                    if (complete) {
                        complete();
                    }
                    divOverlay.remove();
                }
            });
        } else {
            if (complete) {
                complete();
            }
        }
    }

    function ajaxRequestInternal(changedData) {
        var parametesData = getState();
        var pagingSortingInfoData = parametesData["PagingSortingInfo"];
        $.extend(pagingSortingInfoData, changedData);
        parametesData["PagingSortingInfo"] = pagingSortingInfoData;

        ajaxRequest(parametesData);
    };

    function initPaging() {
        var aPagingElements = $("a[data-Page]", ui.updateTarget.getPaging());
        aPagingElements.unbind("click");
        aPagingElements.click(function () {
            var $this = $(this);
            var page = $this.attr("data-Page");
            var data = { Page: page };
            ajaxRequestInternal(data);
        });
    }

    function ajaxRequest(data, callback) {
        var $self = $(self);

        if (self.ModifyRequestData != null) {
            var modifyData = self.ModifyRequestData();
            $.extend(data, modifyData);
        }
        
        endableTableLoading(function () {
            $.ajax({
                url: url,
                type: "POST",
                data: data,
                dataType: "text",
                cache: false,
                success: function (html) {
                    ui.updateTarget.container.empty().append(html);
                    initPaging();
                    disableTableLoading(function () {

                        var event = jQuery.Event("request-success");
                        event.html = html;
                        $self.trigger(event);

                        if (callback) {
                            callback();
                        }
                    });
                },
                beforeSend: function () {
                    $self.trigger("request-start");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(jqXHR.responseText);
                },
                complete: function () {
                    $self.trigger("request-complete");
                    if (filterHighlight) {
                        filterHighlight.RefreshHighlights();
                    }
                }
            });
        });
    }

    this.SendRequest = function (data, keepPaging, callback) {
        var parametesData = getState();
        data = SerializeObject.GetObject(data);
        if (data) { $.extend(parametesData, data); }
        if (!keepPaging) { $.extend(parametesData.PagingSortingInfo, { Page: 1 }); }
        ajaxRequest(parametesData, callback);
    };

    this.ModifyRequestData = null;

    function initSorting() {
        ui.updateTarget.container.on("click", ".paging-sorting th[data-SortMember]", function () {
            var $this = $(this);
            var sortMember = $this.attr("data-SortMember");
            var sortDescending = $this.attr("data-SortDescending");
            var data = {
                SortMember: sortMember,
                SortDescending: sortDescending
            };
            ajaxRequestInternal(data);
        });
    }

    function initFilterHighlight() {
        var filterHighlightSettings = new FilterHighlightSettigs();
        filterHighlightSettings.filterContainer = ui.filter.container;
        filterHighlightSettings.filterBtnClear = ui.filter.btnClear;
        filterHighlightSettings.table = ui.updateTarget.container;
        filterHighlight = new FilterHighlight(filterHighlightSettings);
        filterHighlight.RefreshHighlights();
    }

    function initUi() {
        ui.updateTarget.container = $(".paging-sorting-update", containerSelector);
        if (ui.updateTarget.container.length === 0) {
            var errMsg = ".paging-sorting-update not found in container '" + containerSelector + "' or container not found";
            alert(errMsg);
            throw new Error(errMsg);
        }


        ui.updateTarget.getTable = function () { return $("table.paging-sorting", ui.updateTarget.container); };
        ui.updateTarget.getPaging = function () { return $("[data-id='paging']", ui.updateTarget.container); };
        ui.updateTarget.getState = function () { return $("[data-paging-sorting-state]", ui.updateTarget.container); };

        if (filterFormSelector) {
            ui.filter.container = $(filterFormSelector);
        } else {
            ui.filter.container = $(".paging-sorting-filter-form", containerSelector);
        }
        
        ui.filter.btnApply = $("[data-apply]", ui.filter.container);
        ui.filter.btnClear = $("[data-clear]", ui.filter.container);
        ui.filter.btnSave = $("[data-save-filter-type]", ui.filter.container);

        if (ui.filter.btnApply.length) {
            ui.filter.btnApply.click(function () {
                self.SendRequest(ui.filter.container.selector);
                return false;
            });
        }
        if (ui.filter.container.length) {
            ui.filter.container.submit(function () {
                self.SendRequest(ui.filter.container.selector);
                return false;
            });
        }
        

        if (ui.filter.btnClear.length) {
            ui.filter.btnClear.click(function () {
                $(":input", ui.filter.container).each(function () {
                    var $this = $(this);
                    $this.val(null);

                    if ($this.is(".multiselect")) {
                        $this.multiselect("refresh");
                    }
                    $this.removeClass("processed");

                    if ($this.is("[data-jq-watermark]")) {
                        $this.removeAttrs("data-jq-watermark");
                        $this.watermark();
                    }
                });

                if (ui.filter.btnClear.attr("disabled") != "disabled") {
                    self.SendRequest(ui.filter.container.selector, null, function () {
                    });
                }
                return false;
            });
        }
    }

    function init() {
        url = urlArg;

        initUi();
        initSorting();
        initPaging();
        initFilterHighlight();
    };

    init();
};