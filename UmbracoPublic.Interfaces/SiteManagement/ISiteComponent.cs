using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Interfaces.Enumerations;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Interfaces.SiteManagement
{
    public interface ISiteComponent
    {
        string Name { get; }
        void Initialize(Document siteRoot);
        SiteComponentState State { get; }
        void InstantiateIn(ControlCollection controls);
    }
}
