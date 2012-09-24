﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class ContentFolder : Entity
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("ContentFolder").Id); }
        }

        public override string TemplatePath
        {
            get { return "/ContentFolder"; }
        }
    }
}
