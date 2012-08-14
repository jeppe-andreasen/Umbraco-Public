using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.WebSite.usercontrols.Parts;

namespace UmbracoPublic.WebSite.Utilities
{
    public class Snippets
    {
        public static void RenderNewsResults(HtmlWriter writer, SearchResult result, Pager pager)
        {
            var subjects = DataService.Instance.GetSubjects();

            foreach (var record in result.Records.OrderByDescending(r => r.GetDate("date")).Skip(pager.Skip).Take(pager.Take))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.RenderLinkTag(record.GetString("url"), record.GetString("title"));
                writer.RenderEndTag();
                writer.RenderFullTag(HtmlTextWriterTag.H6, "Publiseret " + record.GetDate("date").Value.ToString("dd-MM-yyyy"));
                var subjectList = record.GetString("subjects");
                if (!string.IsNullOrEmpty(subjectList))
                {
                    foreach (var subjectId in subjectList.Split(',').Select(s => new Id(s.Trim())).Where(subjects.ContainsKey))
                    {
                        writer.RenderFullTag(HtmlTextWriterTag.Span, subjects[subjectId], "label");
                    }
                }

                string text = record.GetString("summary");
                if (text.Length > 150)
                    text = text.Substring(0, 150);
                writer.RenderFullTag(HtmlTextWriterTag.P, text);
            }
        }
    }
}