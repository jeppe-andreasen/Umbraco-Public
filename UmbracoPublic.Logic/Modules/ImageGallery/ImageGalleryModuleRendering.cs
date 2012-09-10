using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.ImageGallery
{
    public class ImageGalleryModuleRendering : BaseModuleRendering<ImageGalleryModule>
    {
        protected override void RegisterScripts()
        {
            var options = new JSONObject();
            options.AddValue("interval", 5000);
            options.AddValue("pause", "hover");
            ModuleScripts.RegisterInitScript("carousel", options);
        }

        protected override void RenderModule(ImageGalleryModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "carousel slide");
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "carousel-inner");

            var data = module.Data;
            bool isFirst = true;
            foreach (var item in data.Items)
            {
                writer.AddClass("item");
                if (isFirst)
                {
                    writer.AddClass("active");
                    isFirst = false;
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if (!string.IsNullOrEmpty(item.ImageId))
                {
                    var image = CmsService.Instance.GetImage(new Id(item.ImageId));
                    if (image.Exists)
                    {
                        writer.RenderImageTag(image.Url, image.AlternateText, null);
                    }
                }
                if (!string.IsNullOrEmpty(item.Headline) || !string.IsNullOrEmpty(item.Content))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Div, "carousel-caption");
                    if (!string.IsNullOrEmpty(item.Headline))
                        writer.RenderFullTag(HtmlTextWriterTag.H4, "First Thumbnail label");
                    if (!string.IsNullOrEmpty(item.Content))
                        writer.RenderParagraph(item.Content);
                    writer.RenderEndTag(); // div.carousel-caption    
                }
                writer.RenderEndTag(); // div.item
            }
            writer.RenderEndTag(); // div.carousel-inner
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + this.ClientID);
            writer.AddAttribute("data-slide", "prev");
            writer.RenderFullTag(HtmlTextWriterTag.A, "&lsaquo;", "left carousel-control");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + this.ClientID);
            writer.AddAttribute("data-slide", "next");
            writer.RenderFullTag(HtmlTextWriterTag.A, "&rsaquo;", "right carousel-control");
            writer.RenderEndTag(); // div.carousel
        }
    }
}
