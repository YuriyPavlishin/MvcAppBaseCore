var SiteAjaxSetup = new function () {

    var self = this;

    this.init = function() {
        $(document).ajaxComplete(onAjaxComplete);
        $(document).ajaxError(onAjaxError);
    };

    this.hideAjaxError = function (jqXHR) {
        jqXHR.ErrorHandled = true;
    };

    this.skipInitCall = function (jqXHR) {
        jqXHR.skipInit = true;
    };

    function onAjaxComplete(event, jqXHR, ajaxOptions) {
        var isAjaxRedirect = jqXHR.getResponseHeader('AjaxRedirect') === "1";
        if (isAjaxRedirect) {
            ajaxRedirect(jqXHR);
            return;
        }

        if (!jqXHR.skipInit) {
            Site.init();
        }
    }

    function onAjaxError(event, jqXHR, ajaxSettings, thrownError) {
        if (!jqXHR.ErrorHandled) {
            $.notify({ message: "Unhandled Error: " + jqXHR.responseText }, { type: 'danger', delay: 7000 });
        }
    }

    function ajaxRedirect(jqXHR) {
        var redirectUrl = jqXHR.getResponseHeader('location');
        var pathAndQuery = redirectUrl.split('?');
        if (pathAndQuery.length == 2) {
            var queryArgs = pathAndQuery[1].split('&');
            var isUrlChanged = false;
            for (var i = 0; i < queryArgs.length; i++) {
                if (queryArgs[i].indexOf("ReturnUrl=") == 0) {
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

        self.skipInitCall(jqXHR);
        window.location = redirectUrl;
    }
};