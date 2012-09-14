using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.GoogleMap
{
    public class GoogleMapModuleRendering : BaseModuleRendering<GoogleMapModule>
    {
        

        protected override void RegisterScripts()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), "googlemapsapi"))
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "googlemapsapi", "http://maps.googleapis.com/maps/api/js?sensor=false");

            ModuleScripts.RegisterInitScript("googlemaps", new JSONString(ClientID), new JSONNumber(Module.Latitude), new JSONNumber(Module.Longitude), new JSONNumber(Module.Zoom), new JSONBoolean(Module.ShowMarker));
        }

        protected override void RenderModule(GoogleMapModule item, LinqIt.Utils.Web.HtmlWriter writer)
        {
            base.RenderModule(item, writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddStyle(HtmlTextWriterStyle.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
        

        public override string ModuleDescription
        {
            get { return "A module displaying a google map"; }
        }
    }
}
