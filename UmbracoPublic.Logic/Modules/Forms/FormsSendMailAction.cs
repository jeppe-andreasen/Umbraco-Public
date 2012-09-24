using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using LinqIt.Utils.Web;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsSendMailAction : FormsAction
    {
        public override LinqIt.Cms.Data.Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsSendMailAction").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsAction/FormsSendMailAction"; }
        }

        internal override void Execute(List<FieldSpecification> specifications)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(GetValue<string>("senderAddress"), GetValue<string>("senderName"));
            foreach (var recipient in GetValue<string>("recipients").Split(';'))
                mail.To.Add(recipient.Trim());
            mail.Subject = GetValue<string>("mailSubject");
            mail.IsBodyHtml = true;
            mail.Body = HtmlWriter.Generate(w => GenerateBody(w, specifications));

            var uploads = specifications.Where(s => s.Get() is FileUpload).Select(s => (FileUpload) s.Get());
            var attachments = uploads.Where(u => u.HasFile).Select(u => new Attachment(u.FileContent, u.FileName));
            foreach (var attachment in attachments)
                mail.Attachments.Add(attachment);

            using (var client = new SmtpClient())
            {
                client.Send(mail);
            }
        }

        private static void GenerateBody(HtmlWriter w, IEnumerable<FieldSpecification> specifications)
        {
            w.RenderBeginTag(HtmlTextWriterTag.Table);
            foreach (var spec in specifications)
            {
                var obj = spec.Get();
                if (obj is FileUpload)
                    continue;

                w.RenderBeginTag(HtmlTextWriterTag.Tr);
                w.RenderFullTag(HtmlTextWriterTag.Td, spec.Label);
                w.RenderBeginTag(HtmlTextWriterTag.Td);
                if (obj != null)
                {
                    if (obj.GetType() == typeof(bool))
                        w.Write(((bool)obj)? "Ja" : "Nej");
                    else
                        w.Write(obj.ToString());
                }
                w.RenderEndTag();
                w.RenderEndTag();
            }
            w.RenderEndTag();
        }
    }
}
