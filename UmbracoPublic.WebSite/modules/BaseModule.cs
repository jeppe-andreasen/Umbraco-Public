using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.modules
{
    public abstract class BaseModule<T> : System.Web.UI.UserControl, IGridModuleRendering where T:Entity, new()
    {
        public virtual int[] GetModuleColumnOptions()
        {
            return new []{3,4,6,9,12};
        }
        
        private T _module;

        protected void RegisterInitScript(string componentName)
        {
            ModuleScripts.Instance.RegisterInitializationScripts(this, "application." + componentName + ".init();");
        }

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
            var module = Module;
            if (module == null)
                return;

            var w = new HtmlWriter(writer);
            RenderModule(module, w);
            base.Render(writer);
        }
    }
}