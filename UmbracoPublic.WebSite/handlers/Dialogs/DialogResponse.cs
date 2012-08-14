using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqIt.Ajax.Parsing;

namespace UmbracoPublic.WebSite.handlers.Dialogs
{
    public enum DialogCommand
    {
        RefreshNode,
        SelectNode,
        ShowDialog
    }

    public class DialogResponse
    {
        private readonly string _commandName;
        private readonly bool _success;
        private Dictionary<string, JSONValue> _values = new Dictionary<string, JSONValue>();

        private readonly List<KeyValuePair<DialogCommand, string>> _commands = new List<KeyValuePair<DialogCommand, string>>();

        public DialogResponse(string commandName, bool success)
        {
            _commandName = commandName;
            _success = success;
        }

        public void AddCommand(DialogCommand command, string args)
        {
            _commands.Add(new KeyValuePair<DialogCommand, string>(command, args));
        }

        public bool Success { get { return _success; } }

        public JSONObject ToJSON()
        {
            var response = new JSONObject();
            response.AddValue("commandName", _commandName);
            response.AddValue("success", _success);

            var commands = new JSONArray();
            foreach (var pair in _commands)
            {
                var command = new JSONObject();
                command.AddValue("name", pair.Key.ToString().ToLower());
                command.AddValue("args", pair.Value);
                commands.AddValue(command);
            }
            response.AddValue("commands", commands);

            var values = new JSONObject();
            foreach (string key in _values.Keys)
                values.AddValue(key, _values[key]);
            response.AddValue("values", values);

            return response;
        }

        public static DialogResponse Error(string commandName, string message)
        {
            DialogResponse result = new DialogResponse(commandName, false);
            result.Message = message;
            return result;
        }

        public string Message { get; set; }

        internal void AddValue(string key, string value)
        {
            this._values.Add(key, new JSONString(value));
        }

        internal void AddValue(string key, int value)
        {
            this._values.Add(key, new JSONNumber(value));
        }
    }
}