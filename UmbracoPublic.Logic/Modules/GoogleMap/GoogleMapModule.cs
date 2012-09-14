using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LinqIt.Ajax.Parsing;

namespace UmbracoPublic.Logic.Modules.GoogleMap
{
    public class GoogleMapModule : BaseModule
    {
        private JSONObject _values;

        private void EnsureValuesLoaded()
        {
            if (_values == null)
                _values = JSONObject.Parse(GetValue<string>("view"));
        }

        public decimal Latitude
        {
            get
            {
                EnsureValuesLoaded();
                return Convert.ToDecimal((string) _values["latitude"], CultureInfo.InvariantCulture);
            }
        }

        public decimal Longitude
        {
            get
            {
                EnsureValuesLoaded();
                return Convert.ToDecimal((string)_values["longitude"], CultureInfo.InvariantCulture);
            }
        }

        public int Zoom
        {
            get
            {
                EnsureValuesLoaded();
                return Convert.ToInt32((string)_values["zoom"], CultureInfo.InvariantCulture);
            }
        }

        public bool ShowMarker
        {
            get { return GetValue<bool>("showMarker"); }
        }

    }
}
