$(function () {
    Site.init();
});


SiteAjaxSetup.init();
SiteValidators.init();

var Site = new function() {

    this.init = function () {
        SiteScriptMessage.evaluateScriptMessages();
        SiteAutocomplete.init();
        autofocus();
    };

    this.htmlEncode = function(value) {
        //create a in-memory div, set it's inner text(which jQuery automatically encodes)
        //then grab the encoded contents back out.  The div never exists on the page.
        return $('<div/>').text(value).html();
    };

    this.htmlToText = function(value) {
        return $('<div/>').html(value).text();
    };

    this.readCookieRaw = function(name, options) {
        var raw = $.cookie.raw;
        var result = null;
        try {
            $.cookie.raw = true;
            result = $.cookie(name, undefined, options);
            $.cookie.raw = raw;
        } catch(e) {
            $.cookie.raw = raw;
            throw e;
        }
        return result;
    };
    
    function autofocus() {
        $(":not([data-setfocus])[data-focus='True']:first").each(function () {
            var $this = $(this);
            $this.attr("data-setfocus", true);
            $this.focus();
        });
    }
};