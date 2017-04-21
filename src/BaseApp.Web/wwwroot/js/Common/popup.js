var PopupResult = {
    OK: "OK",
    Cancel: "Cancel"
};

function PopupContext(callContext, onOK, onCancel) {
    var _callContext = callContext;
    var _onOK = onOK;
    var _onCancel = onCancel;

    this.Result = null,
    this.ReturnInfo = null,
    this.FireClose = function(sender) {
        var _eventArgs = { ReturnInfo: this.ReturnInfo, CallContext: _callContext };
        if (this.Result == PopupResult.OK && _onOK != null)
            _onOK(sender, _eventArgs);
        else if ((this.Result == null || this.Result == PopupResult.Cancel) && _onCancel != null)
            _onCancel(sender, _eventArgs);
    };
}

var PopupManager = {
    CurrentModalPopup: null,

    ModalOnLoad: function () {
        var dv = $('#divModalIframe');
        var dvLoading = dv.find('#divModalIframeLoading');
        var dvIfrHolder = dv.find("#divModalIframeHolder");

        dvLoading.hide();
        dvIfrHolder.show();
    },

    ShowModal: function (url, width, height, title, callContext, onOK, onCancel) {

        //use timeout for FIX: HTML Parsing Error: Unable to modify the parent container element before the child element is closed (KB927917)
        setTimeout(function () {

            url = url + (url.indexOf('?') > 0 ? "&" : "?") + "rnd=" + Math.random(11);

            var dv = $("<div id='divModalIframe' style='display:none; overflow:hidden;'><div id='divModalIframeLoading' style='display:none; color:Gray; padding:15px'>Loading...</div><div id='divModalIframeHolder' style='width:100%; height:100%;'></div></div>");

            //var dv = $('#divModalIframe');
            var dvLoading = dv.find('#divModalIframeLoading');
            var dvIfrHolder = dv.find("#divModalIframeHolder");

            var isIE = navigator.appName == 'Microsoft Internet Explorer';
            //fix chrome|safari bug with history back iframe initializing
            var rndID = Math.random(11);
            var tplIfr;
            if (isIE == true) {
                //FIX ERROR IN IE9: 'Object' is undefined (in jquery) - no src tag in html source
                tplIfr = '<iframe id="' + rndID + '" width="100%" height="100%" onload="PopupManager.ModalOnLoad();" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto">Your browser does not support iframes</iframe>';
            } else {
                tplIfr = '<iframe id="' + rndID + '" width="100%" height="100%" onload="PopupManager.ModalOnLoad();" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" src="' + url + '">Your browser does not support iframes</iframe>';
            }

            //set up iframe
            dvIfrHolder.hide();
            dvIfrHolder.html(tplIfr);

            if (isIE == true) {
                //FIX ERROR IN IE9: 'Object' is undefined (in jquery)
                dvIfrHolder.find('iframe')[0].src = url;
            }

            //show loading message
            dvLoading.show();

            PopupManager.CurrentModalPopup = new PopupContext(callContext, onOK, onCancel);

            dv.dialog({
                modal: true,
                width: width,
                height: height,
                title: title,
                buttons: null, //{ Close: function () { $(this).dialog('close'); } },
                close: function (event, ui) {
                    
                    //remove iframe
                    dvIfrHolder.html("");
                    dv.remove();
                    
                    if (PopupManager.CurrentModalPopup != null) {
                        PopupManager.CurrentModalPopup.FireClose(null); //no sender for now
                        PopupManager.CurrentModalPopup = null;
                    }
                },
                resizable: false
            });

        }, 0);

    },

    CloseModal: function (returnInfo, isOK) {
        if (PopupManager.CurrentModalPopup != null) {
            PopupManager.CurrentModalPopup.Result = isOK ? PopupResult.OK : PopupResult.Cancel;
            PopupManager.CurrentModalPopup.ReturnInfo = returnInfo;
        }
        $('#divModalIframe').dialog("close");
    }
};

