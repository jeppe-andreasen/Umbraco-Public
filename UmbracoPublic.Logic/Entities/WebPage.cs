using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class WebPage : Page
    {
        public string Headline
        {
            get { return GetValue<string>("headline"); }
        }

        public Text Intro
        {
            get { return GetValue<Text>("intro"); }
        }

        public Html Body
        {
            get { return GetValue<Html>("body"); }
        }

        public Image Thumbnail
        {
            get { return GetValue<Image>("thumbnail"); }
        }

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("WebPage").Id); }
        }

        public override string TemplatePath
        {
            get { return "/WebPage"; }
        }
    }
}
