using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.Infrastructure
{
    public class ScriptMessage
    {
        public enum ScriptMessageTypes
        {
            Info,
            Danger,
            Success,

            JsScript
        }

        private readonly List<ScriptMessageItem> _messages;
     
        public ScriptMessage()
        {
            _messages = new List<ScriptMessageItem>();
        }

        public void AddJavaScript(string script)
        {
            AddMessage(ScriptMessageTypes.JsScript, script);
        }
        public void AddError(string message)
        {
            AddMessage(ScriptMessageTypes.Danger, message);
        }
        public void AddInfo(string message)
        {
            AddMessage(ScriptMessageTypes.Info, message);
        }
        public void AddSuccess(string message)
        {
            AddMessage(ScriptMessageTypes.Success, message);
        }

        private void AddMessage(ScriptMessageTypes type, string message)
        {
            _messages.Add(new ScriptMessageItem(type, message));
        }

        public void SaveMessages(IResponseCookies cookies)
        {
            if (_messages.Count <= 0)
                return;

            var jsonMessages = JsonSerializer.Serialize(_messages);
            var base64JsonMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonMessages), Base64FormattingOptions.None);

            cookies.Append("SiteScriptMessage", base64JsonMessage, new CookieOptions {Path = "/"});
        }
    }

    public class ScriptMessageItem
    {
        public ScriptMessage.ScriptMessageTypes MessageType { get; private set; }
        public string Message { get; private set; }

        public string MessageTypeString => MessageType.ToString().ToLower();

        public string MessageDataType
        {
            get
            {
                switch (MessageType)
                {
                    case ScriptMessage.ScriptMessageTypes.Danger:
                    case ScriptMessage.ScriptMessageTypes.Info:
                    case ScriptMessage.ScriptMessageTypes.Success:
                        return "messsage";
                    case ScriptMessage.ScriptMessageTypes.JsScript:
                        return "script";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown messageType - "+ MessageType);
                }
            }
        }

        public ScriptMessageItem(ScriptMessage.ScriptMessageTypes messageType, string message)
        {
            MessageType = messageType;
            Message = message;
        }
    }
}