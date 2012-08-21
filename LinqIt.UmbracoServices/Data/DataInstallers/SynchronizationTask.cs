using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public abstract class SynchronizationTask
    {
        public void Process(StringBuilder log)
        {
            AddMissingItems(log);
            UpdateItems(log);
            DeleteOldItems(log);
        }

        protected abstract void AddMissingItems(StringBuilder log);

        protected abstract void UpdateItems(StringBuilder log);

        protected abstract void DeleteOldItems(StringBuilder log);
    }
}
