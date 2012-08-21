using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Providers
{
    public class ThemeProvider : NodeProvider
    {
        public ThemeProvider(string referenceId) : base(referenceId)
        {
            
        }

        public override Node GetNode(string value)
        {
            return CreateNode(value);
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            var themeDirectory = HttpContext.Current.Server.MapPath("~/themes");
            var directories = Directory.GetDirectories(themeDirectory).Select(Path.GetFileName);
            return directories.Select(CreateNode);
        }
    
        private static Node CreateNode(string themepath)
        {
            var result = new Node();
            result.Text = themepath;
            result.Id = themepath;
            return result;
        }
    }
}
