function appUtils() {
    this.htmlEncode = function (value) {
        //create a in-memory div, set it's inner text(which jQuery automatically encodes)
        //then grab the encoded contents back out.  The div never exists on the page.
        return $('<div/>').text(value).html();
    };

    this.htmlToText = function (value) {
        return $('<div/>').html(value).text();
    };

    this.readCookieRaw = function (name, options) {
        var raw = $.cookie.raw;
        var result = null;
        try {
            $.cookie.raw = true;
            result = $.cookie(name, undefined, options);
            $.cookie.raw = raw;
        } catch (e) {
            $.cookie.raw = raw;
            throw e;
        }
        return result;
    };

    this.autofocus = function() {
        var $focus = $("[data-focus]").not("[data-focus-placed='true']");
        if ($focus.length > 0) {
            $focus.attr("data-focus-placed", "true");
            focusOnElement($focus);
        }
    }

    this.focusOnElement = focusOnElement;

    function focusOnElement($focus) {
        var elem = $focus[0];
        var elemLen = elem.value.length;
        // For IE Only
        if (document.selection) {
            // Set focus
            elem.focus();
            // Use IE Ranges
            var oSel = document.selection.createRange();
            // Reset position to 0 & then set at end
            oSel.moveStart('character', -elemLen);
            oSel.moveStart('character', elemLen);
            oSel.moveEnd('character', 0);
            oSel.select();
        }
        else if (elem.selectionStart || elem.selectionStart == '0') {
            // Firefox/Chrome
            elem.selectionStart = elemLen;
            elem.selectionEnd = elemLen;
            elem.focus();
        } // if
    }
}