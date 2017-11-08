class appAjaxSetup {
    constructor() {
        $(document).ajaxComplete((event: JQueryEventObject, XMLHttpRequest: XMLHttpRequest) => {
            var isAjaxRedirect = XMLHttpRequest.getResponseHeader('AjaxRedirect') === "1";
            if (isAjaxRedirect) {
                this.ajaxRedirect(XMLHttpRequest);
                return;
            }

            site.init();
        });
        $(document).ajaxError((event: JQueryEventObject, jqXHR: JQueryXHR) => {
            $.notify({ message: `Unhandled Error: ${jqXHR.responseText}` }, { type: 'danger', delay: 7000 });
        });
    }
    

    private ajaxRedirect(jqXHR: XMLHttpRequest) {
        var redirectUrl = jqXHR.getResponseHeader('location');
        var pathAndQuery = redirectUrl.split('?');
        if (pathAndQuery.length === 2) {
            const queryArgs = pathAndQuery[1].split('&');
            let isUrlChanged = false;
            for (var i = 0; i < queryArgs.length; i++) {
                if (queryArgs[i].indexOf("ReturnUrl=") === 0) {
                    //do not redirect to ajax link - redirect to currect page
                    var rootPath = window.location.protocol + "//" + window.location.host + "/";
                    queryArgs[i] = "ReturnUrl=" + encodeURIComponent("/" + window.location.href.replace(rootPath, ""));
                    isUrlChanged = true;
                    break;
                }
            }
            if (isUrlChanged) {
                pathAndQuery[1] = queryArgs.join('&');
                redirectUrl = pathAndQuery.join('?');
            }
        }

        window.location.href = redirectUrl;
    }
}