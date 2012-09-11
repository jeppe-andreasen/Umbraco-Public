using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Logic.Entities
{
    public class MenuItem
    {
        private readonly List<MenuItem> _children = new List<MenuItem>();

        public string DisplayName { get; set; }

        public string Url { get; set; }

        internal void AddChild(MenuItem childItem)
        {
            _children.Add(childItem);
        }

        public IEnumerable<MenuItem> Children { get { return _children; } }

        public bool HasChildren { get { return _children.Count > 0; } }

        public bool Selected { get; set; }

        public bool Expanded { get; set; }

        public bool Collapsed { get; set; }
    }
}
