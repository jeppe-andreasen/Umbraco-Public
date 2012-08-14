using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;
namespace UmbracoPublic.Logic.Entities
{
    public class HeroModule : GridModule
	{
	    public string Headline
	    {
            get { return GetValue<string>("headline"); }
	    }

	    public Html Body
	    {
            get { return GetValue<Html>("body"); }
	    }
	}
}
