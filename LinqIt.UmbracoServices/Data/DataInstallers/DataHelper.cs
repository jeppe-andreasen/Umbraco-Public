using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public static class DataHelper
    {
        public static string GetPath(DocumentType dt)
        {
            if (dt.MasterContentType != 0)
            {
                var parent = new DocumentType(dt.MasterContentType);
                return GetPath(parent) + "/" + dt.Alias;
            }
            return dt.Alias;
        }

        public static string GetPath(UmbracoDataContext context, cmsContentType ct)
        {
            if (ct.masterContentType != 0)
            {
                var parent = context.cmsContentTypes.Where(t => t.nodeId == ct.masterContentType).FirstOrDefault();
                if (parent != null)
                    return GetPath(context, parent) + "/" + ct.alias;
            }
            return ct.alias;
        }

        public static string GetPath(UmbracoDataContext context, cmsTab tab)
        {
            return GetPath(context, tab.cmsContentType) + "/" + tab.text;
        }
    }
}
