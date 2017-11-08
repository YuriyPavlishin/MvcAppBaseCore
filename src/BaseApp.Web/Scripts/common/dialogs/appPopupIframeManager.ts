var PopupIframeResult = {
    OK: "OK",
    Cancel: "Cancel"
};


class PopupContext
{
    private readonly _callContext: any;
    private readonly _onOK: any;
    private readonly _onCancel: any;

    constructor(callContext: any, onOK: any, onCancel: any) {
        this._callContext = callContext;
        this._onOK = onOK;
        this._onCancel = onCancel;
    }

    Result = null as any;
    ReturnInfo = null as any;
    FireClose(sender: any) {
        var _eventArgs = { ReturnInfo: this.ReturnInfo, CallContext: this._callContext };
        if (this.Result === PopupIframeResult.OK && this._onOK != null)
            this._onOK(sender, _eventArgs);
        else if ((this.Result == null || this.Result === PopupIframeResult.Cancel) && this._onCancel != null)
            this._onCancel(sender, _eventArgs);
    };
}

var PopupIframeManager = {
    CurrentModalPopup: null as PopupContext,
    ModalOnLoad() {
        var dv = $('#divModalIframe');
        var dvLoading = dv.find('#divModalIframeLoading');
        var dvIfrHolder = dv.find("#divModalIframeHolder");

        dvLoading.hide();
        dvIfrHolder.show();
    },
    ShowModal(url: string, width: number, height: number, title: string, callContext: any, onOK: any, onCancel: any) {

        //use timeout for FIX: HTML Parsing Error: Unable to modify the parent container element before the child element is closed (KB927917)
        setTimeout(() => {

            url = url + (url.indexOf('?') > 0 ? "&" : "?") + "rnd=" + Math.random();

            var dv = $("<div id='divModalIframe' style='display:none; overflow:hidden;'><div id='divModalIframeLoading' style='display:none; color:Gray; padding:15px'>Loading...</div><div id='divModalIframeHolder' style='width:100%; height:100%;'></div></div>");

            //var dv = $('#divModalIframe');
            var dvLoading = dv.find('#divModalIframeLoading');
            var dvIfrHolder = dv.find("#divModalIframeHolder");

            var isIE = navigator.appName === 'Microsoft Internet Explorer';
            //fix chrome|safari bug with history back iframe initializing
            var rndID = Math.random();
            var tplIfr: string;
            if (isIE) {
                //FIX ERROR IN IE9: 'Object' is undefined (in jquery) - no src tag in html source
                tplIfr = '<iframe id="' + rndID + '" width="100%" height="100%" onload="PopupIframeManager.ModalOnLoad();" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto">Your browser does not support iframes</iframe>';
            } else {
                tplIfr = '<iframe id="' + rndID + '" width="100%" height="100%" onload="PopupIframeManager.ModalOnLoad();" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" src="' + url + '">Your browser does not support iframes</iframe>';
            }

            //set up iframe
            dvIfrHolder.hide();
            dvIfrHolder.html(tplIfr);

            if (isIE) {
                //FIX ERROR IN IE9: 'Object' is undefined (in jquery)
                (dvIfrHolder.find('iframe')[0] as any).src = url;
            }

            //show loading message
            dvLoading.show();

            PopupIframeManager.CurrentModalPopup = new PopupContext(callContext, onOK, onCancel);

            dv.dialog({
                modal: true,
                width: width,
                height: height,
                title: title,
                buttons: null, //{ Close: function () { $(this).dialog('close'); } },
                close() {
                    //remove iframe
                    dvIfrHolder.html("");
                    dv.remove();

                    if (PopupIframeManager.CurrentModalPopup != null) {
                        PopupIframeManager.CurrentModalPopup.FireClose(null); //no sender for now
                        PopupIframeManager.CurrentModalPopup = null;
                    }
                },
                resizable: false
            });

        }, 0);

    },
    CloseModal(returnInfo: any, isOK: boolean) {
        if (PopupIframeManager.CurrentModalPopup != null) {
            PopupIframeManager.CurrentModalPopup.Result = isOK ? PopupIframeResult.OK : PopupIframeResult.Cancel;
            PopupIframeManager.CurrentModalPopup.ReturnInfo = returnInfo;
        }
        $('#divModalIframe').dialog("close");
    }
};