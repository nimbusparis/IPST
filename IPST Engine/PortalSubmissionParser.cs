using System;
using System.Linq;
using System.Text;
using Google.Apis.Gmail.v1.Data;
using NHibernate.Loader.Custom.Sql;

namespace IPST_Engine
{
    public class PortalSubmissionParser : IPortalSubmissionParser
    {
        public PortalSubmission ParseMessage(Message message)
        {
            PortalSubmission portalSubmission  = new PortalSubmission();
            if (message == null || message.Payload == null || message.Payload.Headers == null) return null;
            var subject = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject").Value;
            #region ImageUrl
            var htmlPart = message.Payload.Parts.FirstOrDefault(p => p.MimeType == "text/html");
            var decodedHtmlText = DecodeBase64Url(htmlPart.Body.Data);
            portalSubmission.ImageUrl =
                ExtractImageUrl(decodedHtmlText);
            portalSubmission.UpdateTime = DateTime.Now;
            #endregion

            var dateMail = Convert.ToDateTime((string) message.Payload.Headers.FirstOrDefault(h => h.Name == "Date").Value);
            if (subject.StartsWith("Ingress Portal Submitted:"))
            {
                #region Date
                portalSubmission.DateSubmission =
                    dateMail;
                #endregion
                #region Title
                portalSubmission.Title = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")
                    .Value.Substring(26);
                #endregion
                
            }
            else if (subject.StartsWith("Ingress Portal Live:"))
            {
                portalSubmission.SubmissionStatus = SubmissionStatus.Accepted;
                #region Date
                portalSubmission.DateAccept = dateMail;
                #endregion
                #region Title
                portalSubmission.Title = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")
                    .Value.Substring(21);
                #endregion
                #region Portal Url
                portalSubmission.PortalUrl = ExtractPortalUrl(decodedHtmlText);
                #endregion
                #region Postal Address
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(decodedHtmlText);
                var node = doc.DocumentNode.SelectSingleNode("//a");
                portalSubmission.PostalAddress = node.InnerText;
                #endregion
            }
            else if (subject.StartsWith("Ingress Portal Rejected:"))
            {
                portalSubmission.SubmissionStatus = SubmissionStatus.Rejected;
                portalSubmission.DateReject = dateMail;
                #region Title
                portalSubmission.Title = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")
                    .Value.Substring(25);
                #endregion
                portalSubmission.RejectionReason = RejectionReason.NotMeetCriteria;
            }
            else if (subject.StartsWith("Ingress Portal Duplicate:"))
            {
                portalSubmission.SubmissionStatus = SubmissionStatus.Rejected;
                portalSubmission.DateReject = dateMail;
                #region Title
                portalSubmission.Title = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")
                    .Value.Substring(26);
                #endregion
                portalSubmission.RejectionReason = RejectionReason.Duplicate;
            }
            else
            {
                return null;
            }
            return portalSubmission;
        }

        public static string EncodeBase64Url(string urlPlainText)
        {
            var encodedTest = Convert.ToBase64String(Encoding.UTF8.GetBytes(urlPlainText));
            var result = new StringBuilder(encodedTest);
            result.Replace('+', '-');
            result.Replace('/', '_');
            return result.ToString();
        }
        public static string DecodeBase64Url(string urlEncodedText)
        {
            int padChars = (urlEncodedText.Length % 4) == 0 ? 0 : (4 - (urlEncodedText.Length % 4));
            StringBuilder result = new StringBuilder(urlEncodedText, urlEncodedText.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Encoding.UTF8.GetString(Convert.FromBase64String(result.ToString()));
        }

        public static Uri ExtractImageUrl(string htmlBodyText)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlBodyText);
            var imageNode = doc.DocumentNode.SelectSingleNode("//img");
            if (imageNode != null)
            {
                string link = imageNode.Attributes["src"].Value;
                return new Uri(link);
            }
            return null;
        }

        public static Uri ExtractPortalUrl(string validationHtmlBodyText)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(validationHtmlBodyText);
            string link = doc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
            return new Uri(link);
        }
    }
}