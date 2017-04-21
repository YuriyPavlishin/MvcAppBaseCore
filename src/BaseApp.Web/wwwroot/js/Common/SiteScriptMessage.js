var SiteScriptMessage = new function () {
    this.evaluateScriptMessages = function() {
        var scriptMessagesBase64 = $.cookie("SiteScriptMessage");
        if (scriptMessagesBase64) {
            //remove cookie
            $.cookie("SiteScriptMessage", null, { path: "/", expires: -1 }); 
        }

        if (scriptMessagesBase64 && scriptMessagesBase64 != "") {
            var scriptMessagesJson = Base64.decode(scriptMessagesBase64);
            showScriptMessages(scriptMessagesJson);
        }
    };

    function showScriptMessages(jsonString) {
        var json = $.parseJSON(jsonString);
        if (json) {
            var scriptItems = [];
            if (json.length) {
                scriptItems = json;
            }
            for (var i = 0; i < scriptItems.length; i++) {
                var message = scriptItems[i];
                evaluateScriptMessage(message.Message, message.MessageDataType, message.MessageTypeString);
            }
        }
    }

    function evaluateScriptMessage(message, dataType, dataMessageType) {
        if (dataType === 'messsage') {
            var delay = 2000;
            if (dataMessageType === 'danger')
                delay = 7000;

            $.notify({ message: message }, { type: dataMessageType, delay: delay });
        }
        else if (dataType === 'script') {
            eval(message);
        } else {
            throw new Error("Unknown message dataType - " + dataType);
        }
    }
};