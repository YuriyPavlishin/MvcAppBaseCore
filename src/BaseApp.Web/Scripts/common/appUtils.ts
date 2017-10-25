class appUtils {
    htmlEncode(value: string):string {
        //create a in-memory div, set it's inner text(which jQuery automatically encodes)
        //then grab the encoded contents back out.  The div never exists on the page.
        return $('<div/>').text(value).html();
    }

    htmlToText(value: string):string {
        return $('<div/>').html(value).text();
    }

    readCookieRaw(name: string): string {
        const raw = $.cookie.raw;
        try {
            $.cookie.raw = true;
            const result: string = $.cookie(name);
            $.cookie.raw = raw;
            return result;
        } catch (e) {
            $.cookie.raw = raw;
            throw e;
        }
    }

    autofocus() {
        var $focus = $("[data-focus]").not("[data-focus-placed='true']");
        if ($focus.length > 0) {
            $focus.attr("data-focus-placed", "true");
            this.focusOnElement($focus);
        }
    }

    focusOnElement($focus: JQuery) {
        const doc = document as any;
        var elem = $focus[0] as any;
        
        var elemLen = elem.value.length;
        // For IE Only
        if (doc.selection) {
            // Set focus
            elem.focus();
            // Use IE Ranges
            var oSel = doc.selection.createRange();
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