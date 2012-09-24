using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmbracoPublic.Interfaces.Enumerations;

namespace UmbracoPublic.Logic.Exceptions
{
    public class SiteComponentException : Exception
    {
        public SiteComponentException(string message, SiteComponentState state) : base(message)
        {
            State = state;
        }

        public SiteComponentException(string message, SiteComponentState state, EventHandler buttonHandler, string buttonText = "Fix it", string commandArgs = "") : base(message)
        {
            State = state;
            ButtonHandler = buttonHandler;
            ButtonText = buttonText;
            CommandArgs = commandArgs;
        }

        public SiteComponentState State { get; private set; }

        public EventHandler ButtonHandler { get; private set; }

        public string CommandArgs { get; private set; }

        public string ButtonText { get; private set; }

        
    }
}
