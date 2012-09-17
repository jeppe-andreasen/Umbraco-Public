using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules
{
    public abstract class BaseModuleRendering<T> : System.Web.UI.Control, IGridModuleRendering where T:Entity, new()
    {
        private string _errorMessages;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this is IRequiresCookies && DataService.Instance.GetCookieState() != CookieState.Accepted)
                return;
            
            try
            {
                RegisterScripts();    
            }
            catch (ConfigurationErrorsException exc)
            {
                _errorMessages = exc.Message;
            }
        }

        protected virtual void RegisterScripts()
        {
        }

        public virtual int[] GetModuleColumnOptions()
        {
            return new []{3,4,6,9,12};
        }
        
        private T _module;

        public void InitializeModule(string id, int? columnSpan)
        {
            ModuleId = new Id(Convert.ToInt32(id));
            ColumnSpan = columnSpan;
        }

        protected int? ColumnSpan { get; private set; }

        protected Id ModuleId { get; private set; }

        protected T Module
        {
            get
            {
                return _module ?? (_module = CmsService.Instance.GetItem<T>(ModuleId));
            }
        }

        protected virtual void RenderModule(T item, HtmlWriter writer)
        {
            
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            var w = new HtmlWriter(writer);
            if (string.IsNullOrEmpty(_errorMessages))
            {
                try
                {
                    if (this is IRequiresCookies && DataService.Instance.GetCookieState() != CookieState.Accepted)
                        RenderCookieInfo(w);
                    else
                    {
                        var module = Module;
                        if (module == null)
                            return;

                        RenderModule(module, w);
                        base.Render(writer);
                    }
                }
                catch(Exception exc)
                {
                    _errorMessages = exc.Message;
                }
            }
            if (!string.IsNullOrEmpty(_errorMessages))
            {
                w = new HtmlWriter(writer);
                RenderErrorMessage(w, _errorMessages);
            }
            
        }

        protected virtual void RenderErrorMessage(HtmlWriter writer, string messages)
        {
            if (HttpContext.Current.IsDebuggingEnabled)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "alert alert-error");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.AddAttribute("data-dismiss", "alert");
                writer.RenderFullTag(HtmlTextWriterTag.Button, "×", "close");
                writer.RenderFullTag(HtmlTextWriterTag.Strong, "Error!");
                writer.Write(messages);
                writer.RenderEndTag(); // div.alert
            }
        }

        protected virtual void RenderCookieInfo(HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "alert no-cookies");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute("data-dismiss", "alert");
            writer.RenderFullTag(HtmlTextWriterTag.Button, "×", "close");
            writer.RenderFullTag(HtmlTextWriterTag.Strong, "Bemærk! ");
            writer.Write("Dette indhold kræver cookies for at blive vist korrekt.");
            try
            {
                
                var cookieInfoUrl = Urls.GetCookieAcceptancePageUrl();    
                if (!string.IsNullOrEmpty(cookieInfoUrl))
                {
                    writer.RenderLinkTag(cookieInfoUrl, "Læs mere om cookies");
                }
            }
            catch(ConfigurationErrorsException)
            {
            }
            writer.RenderEndTag(); // div.alert

        }

        protected TC GetRequiredConfig<TC>(string configurationName) where TC : Entity, new()
        {
            var result = CmsService.Instance.GetConfigurationItem<TC>(configurationName);
            if (result == null)
                throw new ConfigurationErrorsException("The " + configurationName + " configuration is missing");
            return result;
        }

        public abstract string ModuleDescription { get; }
    }
}