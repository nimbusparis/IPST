using System;
using System.Collections.Generic;
using Google.Apis.Gmail.v1.Data;
using IPST_Engine;
using NFluent;
using Xunit;


namespace Tests
{
    public class PortalSubmissionParserTest
    {
        const string submissionUrlEncodedText = "PGRpdiBkaXI9Imx0ciI-ICA8ZGl2PlRoYW5rIHlvdSBmb3Igc3VibWl0dGluZyB5b3VyIFBvcnRhbCBzdWdnZXN0aW9uIHRvIEluZ3Jlc3MuIFdlIHdpbGwgcmV2aWV3IHRoaXMgY2FuZGlkYXRlIHRvIHZlcmlmeSBYTSBjb25jZW50cmF0aW9uIGFuZCBlbnN1cmUgaXQgbWVldHMgb3VyIDxhIGhyZWY9Imh0dHBzOi8vc3VwcG9ydC5nb29nbGUuY29tL2luZ3Jlc3MvYW5zd2VyLzMwNjYxOTc_JnJlZl90b3BpYz0yNzk5MjcwIj5Qb3J0YWwgc3VibWlzc2lvbiBndWlkZWxpbmVzPC9hPi4gQWZ0ZXIgYWRkaXRpb25hbCBwcm9jZXNzaW5nLCB3ZSB3aWxsIHNlbmQgeW91IGEgZm9sbG93LXVwIG1lc3NhZ2UgYWJvdXQgeW91ciBzdWJtaXNzaW9uLiBUaGlzIG1heSB0YWtlIGEgZmV3IHdlZWtzLiAgPC9kaXY-ICA8ZGl2Pjxicj48L2Rpdj4gIDxkaXY-PGltZyBzcmM9Imh0dHA6Ly9saDMuZ2dwaHQuY29tL1hZT2FBRG02cHIxTWxYSWRNS3hET2ZPMDBvSUV3c2R1Y1ZfZ0Rtd2l2TVpndk9oeEpVRVR1N2FCUTZpclhTQmhrX0VJVURYaW92R3VpZVRxS09odyIgYWx0PSJJbmxpbmUgaW1hZ2UgMSI-PGJyPjwvZGl2PjwvZGl2Pg==";
        const string submissionHtmlBodyText = "<div dir=\"ltr\">  <div>Thank you for submitting your Portal suggestion to Ingress. We will review this candidate to verify XM concentration and ensure it meets our <a href=\"https://support.google.com/ingress/answer/3066197?&ref_topic=2799270\">Portal submission guidelines</a>. After additional processing, we will send you a follow-up message about your submission. This may take a few weeks.  </div>  <div><br></div>  <div><img src=\"http://lh3.ggpht.com/XYOaADm6pr1MlXIdMKxDOfO00oIEwsducV_gDmwivMZgvOhxJUETu7aBQ6irXSBhk_EIUDXiovGuieTqKOhw\" alt=\"Inline image 1\"><br></div></div>";
        const string submissionImageUri = "http://lh3.ggpht.com/XYOaADm6pr1MlXIdMKxDOfO00oIEwsducV_gDmwivMZgvOhxJUETu7aBQ6irXSBhk_EIUDXiovGuieTqKOhw";
        const string validationUrlEncodedText = "PGRpdiBkaXI9Imx0ciI-PHA-U3VjY2VzcyEgWW91ciBQb3J0YWwgY2FuZGlkYXRlIGhhcyBiZWVuIGFjY2VwdGVkIGFuZCB3aWxsIGJlIGVuYWJsZWQgaW4gdGhlIEluZ3Jlc3MgU2Nhbm5lciBpbiB0aGUgbmVhciBmdXR1cmUuICBZb3UgaGF2ZSBiZWVuIHJld2FyZGVkIHdpdGggMTAwMCBBUCBhbmQgYSBQb3J0YWwgS2V5IGZvciB0aGlzIG5ldyBQb3J0YWwuPC9wPjxwPlBvcnRhbDogQ2Fpcm4gRHUgUGljIE5lb3Vsb3VzIDwvcD48ZGl2PjxhIGhyZWY9Imh0dHBzOi8vd3d3LmluZ3Jlc3MuY29tL2ludGVsP2xsPTQyLjQ4MjE0MSwyLjk0NzAzNCZhbXA7ej0xOCI-Qm9pcyBOb2lyLCA2Njc0MCBMYXJvcXVlLWRlcy1BbGLDqHJlcywgRnJhbmNlPC9hPjwvZGl2PjxpbWcgc3JjPSJodHRwOi8vbGg0LmdncGh0LmNvbS9vUjNMZlEtUXBZVE5XNVZ3bmRmOW1NR1V6S2t0bDhORXhQUm9HZEpjTExZUmpIODVmYl9Ca05rTHBJUGpqV1Zvb0NBRzZBNmZIYUVPM2I0WWFqNzgiIGFsdD0iUG9ydGFsIC0gQ2Fpcm4gRHUgUGljIE5lb3Vsb3VzICIgc3R5bGU9ImRpc3BsYXk6IGJsb2NrO2JvcmRlcjogMDtoZWlnaHQ6IGF1dG87bGluZS1oZWlnaHQ6IDEwMCU7b3V0bGluZTogbm9uZTt0ZXh0LWRlY29yYXRpb246IG5vbmU7Ij48L2Rpdj4=";
        private const string validationHtmlBodyText = "<div dir=\"ltr\"><p>Success! Your Portal candidate has been accepted and will be enabled in the Ingress Scanner in the near future.  You have been rewarded with 1000 AP and a Portal Key for this new Portal.</p><p>Portal: Cairn Du Pic Neoulous </p><div><a href=\"https://www.ingress.com/intel?ll=42.482141,2.947034&amp;z=18\">Bois Noir, 66740 Laroque-des-Albères, France</a></div><img src=\"http://lh4.ggpht.com/oR3LfQ-QpYTNW5Vwndf9mMGUzKktl8NExPRoGdJcLLYRjH85fb_BkNkLpIPjjWVooCAG6A6fHaEO3b4Yaj78\" alt=\"Portal - Cairn Du Pic Neoulous \" style=\"display: block;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;\"></div>";
        const string validationImageUri = "http://lh4.ggpht.com/oR3LfQ-QpYTNW5Vwndf9mMGUzKktl8NExPRoGdJcLLYRjH85fb_BkNkLpIPjjWVooCAG6A6fHaEO3b4Yaj78";
        private const string portalUri = "https://www.ingress.com/intel?ll=42.482141,2.947034&amp;z=18";

        private const string validationNewFormatImageUri = "https://lh5.ggpht.com/XcoJB0YXd_Gu4XGdFLS7X43jCnzDiXEWMtd4zkMNRcHcK9MU_GhB1fMv3sGBXrnT1H7dpqK7SVXVpjL8wuIV";
        private const string validationNewFormatUrlEncodedBodyText =
            "PGRpdiBkaXI9Imx0ciI-PHA-R29vZCB3b3JrLCBBZ2VudDogd2UndmUgYWNjZXB0ZWQgeW91ciBzdWJtaXNzaW9uLCBhbmQgdGhpcyBQb3J0YWwgaXMgbm93IGF2YWlsYWJsZSBvbiB5b3VyIFNjYW5uZXIgYW5kIG9uIHRoZSBJbnRlbCBNYXAuIFlvdSBoYXZlIGJlZW4gYXdhcmRlZCAxMDAwIEFQIGFzIHdlbGwgYXMgdGhpcyBQb3J0YWwmIzM5O3MgS2V5IGZvciB5b3VyIGRpc2NvdmVyeS48L3A-PHA-PGk-LU5pYW50aWNPcHM8L2k-PC9wPkJvcm5lPGRpdj48YSBocmVmPSJodHRwczovL3d3dy5pbmdyZXNzLmNvbS9pbnRlbD9sbD0zOS43MjcwODMsMTQwLjcyMDQ5MyZhbXA7ej0xOCIgdGFyZ2V0PSJfYmxhbmsiPkTFjW5vbWFlLTk2LTMgVGF6YXdha28gT2JvbmFpLCBTZW5ib2t1LXNoaSwgQWtpdGEta2VuIDAxNC0xMjAxLCBKYXBhbjwvYT48L2Rpdj48aW1nIHNyYz0iaHR0cHM6Ly9saDUuZ2dwaHQuY29tL1hjb0pCMFlYZF9HdTRYR2RGTFM3WDQzakNuekRpWEVXTXRkNHprTU5SY0hjSzlNVV9HaEIxZk12M3NHQlhyblQxSDdkcHFLN1NWWFZwakw4d3VJViIgYWx0PSJQb3J0YWwgLSBCb3JuZSIgc3R5bGU9ImRpc3BsYXk6YmxvY2s7Ym9yZGVyOjA7bWluLWhlaWdodDphdXRvO2xpbmUtaGVpZ2h0OjEwMCU7b3V0bGluZTpub25lO3RleHQtZGVjb3JhdGlvbjpub25lIj48L2Rpdj4=";
        private const string validationNewFormatHtmlBodyText =
            "<div dir=\"ltr\"><p>Good work, Agent: we've accepted your submission, and this Portal is now available on your Scanner and on the Intel Map. You have been awarded 1000 AP as well as this Portal&#39;s Key for your discovery.</p><p><i>-NianticOps</i></p>Borne<div><a href=\"https://www.ingress.com/intel?ll=39.727083,140.720493&amp;z=18\" target=\"_blank\">Dōnomae-96-3 Tazawako Obonai, Senboku-shi, Akita-ken 014-1201, Japan</a></div><img src=\"https://lh5.ggpht.com/XcoJB0YXd_Gu4XGdFLS7X43jCnzDiXEWMtd4zkMNRcHcK9MU_GhB1fMv3sGBXrnT1H7dpqK7SVXVpjL8wuIV\" alt=\"Portal - Borne\" style=\"display:block;border:0;min-height:auto;line-height:100%;outline:none;text-decoration:none\"></div>";
        private const string rejectUrlEncodedText =
            "PGRpdiBkaXI9Imx0ciI-PHA-VGhhbmsgeW91IGZvciB5b3VyIFBvcnRhbCBzdWJtaXNzaW9uLiBIb3dldmVyLCB0aGlzIFBvcnRhbCBjYW5kaWRhdGUgZG9lcyBub3QgbWVldCB0aGUgY3JpdGVyaWEgcmVxdWlyZWQgZm9yIGFwcHJvdmFsLjwvcD48cD5QbGVhc2UgcmVmZXIgdG8gPGEgaHJlZj0iaHR0cHM6Ly9zdXBwb3J0Lmdvb2dsZS5jb20vaW5ncmVzcy9hbnN3ZXIvMzA2NjE5Nz9obD1lbiZyZWZfdG9waWM9Mjc5OTI3MCI-TmV3IFBvcnRhbCBTdWJtaXNzaW9uPC9hPiBjcml0ZXJpYSBhdCBvdXIgSGVscCBDZW50ZXIgZm9yIGZ1cnRoZXIgaW5mb3JtYXRpb24uPC9wPjxpbWcgc3JjPSJodHRwOi8vbGgzLmdncGh0LmNvbS9UNi1IM1VJdGxha2xUNml6V3BobDJPQVdxY04zWmk0LTVObnhLRWE4VXpJa2V6NGViRUxzRmFmbzRaLWxoUHJJUE5DLUVUcVZydEh0SWNhYlN5WW1adyIgYWx0PSJQb3J0YWwgLSBBbnRlbm5lIFJlbGFpcyBEdSBQaWMgTmVvdWxvdXMiIHN0eWxlPSJkaXNwbGF5OiBibG9jaztib3JkZXI6IDA7aGVpZ2h0OiBhdXRvO2xpbmUtaGVpZ2h0OiAxMDAlO291dGxpbmU6IG5vbmU7dGV4dC1kZWNvcmF0aW9uOiBub25lOyI-PC9kaXY-";
        private const string rejectHtmlBodyText = "<div dir=\"ltr\"><p>Thank you for your Portal submission. However, this Portal candidate does not meet the criteria required for approval.</p><p>Please refer to <a href=\"https://support.google.com/ingress/answer/3066197?hl=en&ref_topic=2799270\">New Portal Submission</a> criteria at our Help Center for further information.</p><img src=\"http://lh3.ggpht.com/T6-H3UItlaklT6izWphl2OAWqcN3Zi4-5NnxKEa8UzIkez4ebELsFafo4Z-lhPrIPNC-ETqVrtHtIcabSyYmZw\" alt=\"Portal - Antenne Relais Du Pic Neoulous\" style=\"display: block;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;\"></div>";
        private const string rejectImageUri = "http://lh3.ggpht.com/T6-H3UItlaklT6izWphl2OAWqcN3Zi4-5NnxKEa8UzIkez4ebELsFafo4Z-lhPrIPNC-ETqVrtHtIcabSyYmZw";
        private const string editedUrlEncodedText = "PGRpdiBkaXI9Imx0ciI-PHA-VGhhbmsgeW91IGZvciB5b3VyIEluZ3Jlc3MgUG9ydGFsIGRhdGEgZWRpdCBzdWJtaXNzaW9uLiBOSUEgb3BlcmF0aXZlcyBoYXZlIHJldmlld2VkIGFuZCBhY2NlcHRlZCB5b3VyIHByb3Bvc2VkIGNoYW5nZXMuIFRoZXNlIG1vZGlmaWNhdGlvbnMgc2hvdWxkIGJlIGxpdmUgc29vbi48L3A-PHA-MjAwIEFQIGhhcyBiZWVuIGF3YXJkZWQgdG8geW91ciBhY2NvdW50LjwvcD48cD5Qb3J0YWw6IExhZHk8L3A-PGRpdj48YSBocmVmPSJodHRwczovL3d3dy5pbmdyZXNzLmNvbS9pbnRlbD9sbD00OC44Njg5OTcsMi4zNTg4MjUmYW1wO3o9MTgiPjItNiBUYXlsb3IgU3RyZWV0LCA3NTAxMCBQYXJpcywgRnJhbmNlPC9hPjwvZGl2PjxpbWcgc3JjPSJodHRwOi8vbGgzLmdncGh0LmNvbS9lRnp5cWREVTROU2JsbUhra29lUGU5aEUwYWJaM2d6MGQ1Zm1LX2FsUm9XN184Q09KWTZrb2NXZlRiVGU0ZmpUZ2VPRkktOHl4UXNubVlpb2VWZyIgYWx0PSJQb3J0YWwgLSBMYWR5IiBzdHlsZT0iZGlzcGxheTogYmxvY2s7Ym9yZGVyOiAwO2hlaWdodDogYXV0bztsaW5lLWhlaWdodDogMTAwJTtvdXRsaW5lOiBub25lO3RleHQtZGVjb3JhdGlvbjogbm9uZTsiPjwvZGl2Pg==";
        private const string rejectHtmlNewFormatBodyText = 
            "<div dir=\"ltr\"><p>We've reviewed your Portal submission and given the information you&#39;ve provided in your submission, we have decided not to accept this candidate.</p><p>At this time, we’re not able to provide specific rejection reasons for each submission we review; however, the following are common reasons for rejection:</p><ul><li>The candidate is on our <b><a href=\"https://support.google.com/ingress/answer/3066197?%20utm_source=geostore&amp;utm_medium=auto_email&amp;utm_campaign=pds_reject#pds\" target=\"_blank\">PLEASE DON&#39;T SUBMIT</a></b> list</li><li>We couldn&#39;t find evidence that the candidate meets any of our <b><a href=\"https://support.google.com/ingress/answer/3066197?%20utm_source=geostore&amp;utm_medium=auto_email&amp;utm_campaign=ac_reject#ac\" target=\"_blank\">ACCEPTANCE CRITERIA</a></b></li><li>The candidate was submitted in an incorrect location, and we weren&#39;t able to find the right location</li></ul><p><i>-NianticOps</i></p>NHK Areal<div><a href=\"https://www.ingress.com/intel?ll=36.569424,136.661401&amp;z=18\" target=\"_blank\">14-1 Ōtemachi, Kanazawa-shi, Ishikawa-ken 920-0912, Japan</a></div><img src=\"https://lh3.ggpht.com/ruIHC0cJ_kDq51i5cRf6ERJPynCP11aBGK5QPLEriLpjIgv20WGPWwmsjD51ipCf-S8TMcCfh36RcowdmvV7\" alt=\"Portal - NHK Areal\" style=\"display:block;border:0;min-height:auto;line-height:100%;outline:none;text-decoration:none\"></div>";

        private const string rejectHtmlNewFormatEncodedText =
            "PGRpdiBkaXI9Imx0ciI-PHA-V2UndmUgcmV2aWV3ZWQgeW91ciBQb3J0YWwgc3VibWlzc2lvbiBhbmQgZ2l2ZW4gdGhlIGluZm9ybWF0aW9uIHlvdSYjMzk7dmUgcHJvdmlkZWQgaW4geW91ciBzdWJtaXNzaW9uLCB3ZSBoYXZlIGRlY2lkZWQgbm90IHRvIGFjY2VwdCB0aGlzIGNhbmRpZGF0ZS48L3A-PHA-QXQgdGhpcyB0aW1lLCB3ZeKAmXJlIG5vdCBhYmxlIHRvIHByb3ZpZGUgc3BlY2lmaWMgcmVqZWN0aW9uIHJlYXNvbnMgZm9yIGVhY2ggc3VibWlzc2lvbiB3ZSByZXZpZXc7IGhvd2V2ZXIsIHRoZSBmb2xsb3dpbmcgYXJlIGNvbW1vbiByZWFzb25zIGZvciByZWplY3Rpb246PC9wPjx1bD48bGk-VGhlIGNhbmRpZGF0ZSBpcyBvbiBvdXIgPGI-PGEgaHJlZj0iaHR0cHM6Ly9zdXBwb3J0Lmdvb2dsZS5jb20vaW5ncmVzcy9hbnN3ZXIvMzA2NjE5Nz8lMjB1dG1fc291cmNlPWdlb3N0b3JlJmFtcDt1dG1fbWVkaXVtPWF1dG9fZW1haWwmYW1wO3V0bV9jYW1wYWlnbj1wZHNfcmVqZWN0I3BkcyIgdGFyZ2V0PSJfYmxhbmsiPlBMRUFTRSBET04mIzM5O1QgU1VCTUlUPC9hPjwvYj4gbGlzdDwvbGk-PGxpPldlIGNvdWxkbiYjMzk7dCBmaW5kIGV2aWRlbmNlIHRoYXQgdGhlIGNhbmRpZGF0ZSBtZWV0cyBhbnkgb2Ygb3VyIDxiPjxhIGhyZWY9Imh0dHBzOi8vc3VwcG9ydC5nb29nbGUuY29tL2luZ3Jlc3MvYW5zd2VyLzMwNjYxOTc_JTIwdXRtX3NvdXJjZT1nZW9zdG9yZSZhbXA7dXRtX21lZGl1bT1hdXRvX2VtYWlsJmFtcDt1dG1fY2FtcGFpZ249YWNfcmVqZWN0I2FjIiB0YXJnZXQ9Il9ibGFuayI-QUNDRVBUQU5DRSBDUklURVJJQTwvYT48L2I-PC9saT48bGk-VGhlIGNhbmRpZGF0ZSB3YXMgc3VibWl0dGVkIGluIGFuIGluY29ycmVjdCBsb2NhdGlvbiwgYW5kIHdlIHdlcmVuJiMzOTt0IGFibGUgdG8gZmluZCB0aGUgcmlnaHQgbG9jYXRpb248L2xpPjwvdWw-PHA-PGk-LU5pYW50aWNPcHM8L2k-PC9wPk5ISyBBcmVhbDxkaXY-PGEgaHJlZj0iaHR0cHM6Ly93d3cuaW5ncmVzcy5jb20vaW50ZWw_bGw9MzYuNTY5NDI0LDEzNi42NjE0MDEmYW1wO3o9MTgiIHRhcmdldD0iX2JsYW5rIj4xNC0xIMWMdGVtYWNoaSwgS2FuYXphd2Etc2hpLCBJc2hpa2F3YS1rZW4gOTIwLTA5MTIsIEphcGFuPC9hPjwvZGl2PjxpbWcgc3JjPSJodHRwczovL2xoMy5nZ3BodC5jb20vcnVJSEMwY0pfa0RxNTFpNWNSZjZFUkpQeW5DUDExYUJHSzVRUExFcmlMcGpJZ3YyMFdHUFd3bXNqRDUxaXBDZi1TOFRNY0NmaDM2UmNvd2RtdlY3IiBhbHQ9IlBvcnRhbCAtIE5ISyBBcmVhbCIgc3R5bGU9ImRpc3BsYXk6YmxvY2s7Ym9yZGVyOjA7bWluLWhlaWdodDphdXRvO2xpbmUtaGVpZ2h0OjEwMCU7b3V0bGluZTpub25lO3RleHQtZGVjb3JhdGlvbjpub25lIj48L2Rpdj4=";
        [Fact]
        public void ParseNullMessage_Test()
        {
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(null);
            Check.That(result).IsNull();
            var message = new Message
            {
            };
            result = target.ParseMessage(message);
            Check.That(result).IsNull();
            message = new Message
            {
                Payload = new MessagePart()
            };
            result = target.ParseMessage(message);
            Check.That(result).IsNull();
        }
        [Fact]
        public void ParseSubmissionMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Ingress Portal Submitted: Immeuble À La Frise Florale En Faïence"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = submissionUrlEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Pending);
            Check.That(result.DateSubmission).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.Title).Equals("Immeuble À La Frise Florale En Faïence");
            Check.That(result.ImageUrl).Equals(submissionImageUri);
            Check.That(result.UpdateTime.Date).Equals(DateTime.Today);
        }

        [Fact]
        public void ParseValidationMessageNewFormat_Test()
        {
            
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Portal review complete: Borne"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = validationNewFormatUrlEncodedBodyText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Accepted);
            Check.That(result.DateAccept).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.Title).Equals("Borne");
            Check.That(result.PostalAddress).Equals("Dōnomae-96-3 Tazawako Obonai, Senboku-shi, Akita-ken 014-1201, Japan");
            Check.That(result.ImageUrl).Equals(validationNewFormatImageUri);
            Check.That(result.PortalUrl).Equals("https://www.ingress.com/intel?ll=39.727083,140.720493&amp;z=18");

        }
        [Fact]
        public void ParseValidationMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Ingress Portal Live: Immeuble À La Frise Florale En Faïence"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = validationUrlEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Accepted);
            Check.That(result.DateAccept).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.Title).Equals("Immeuble À La Frise Florale En Faïence");
            Check.That(result.PostalAddress).Equals("Bois Noir, 66740 Laroque-des-Albères, France");
            Check.That(result.ImageUrl).Equals(validationImageUri);
            Check.That(result.PortalUrl).Equals(portalUri);
        }
        [Fact]
        public void ParseRejectMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Ingress Portal Rejected: Antenne Relais Du Pic Neoulous"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = rejectUrlEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(result.DateReject).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.Title).Equals("Antenne Relais Du Pic Neoulous");
            Check.That(result.ImageUrl).Equals(rejectImageUri);
            Check.That(result.RejectionReason).Equals(RejectionReason.NotMeetCriteria);
        }
        [Fact]
        public void ParseRejectNewFormatMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Portal review complete: Antenne Relais Du Pic Neoulous"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = rejectHtmlNewFormatEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.DateReject).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(result.Title).Equals("Antenne Relais Du Pic Neoulous");
            Check.That(result.ImageUrl).Equals("https://lh3.ggpht.com/ruIHC0cJ_kDq51i5cRf6ERJPynCP11aBGK5QPLEriLpjIgv20WGPWwmsjD51ipCf-S8TMcCfh36RcowdmvV7");
            Check.That(result.PostalAddress).Equals("14-1 Ōtemachi, Kanazawa-shi, Ishikawa-ken 920-0912, Japan");
            Check.That(result.PortalUrl).Equals("https://www.ingress.com/intel?ll=36.569424,136.661401&amp;z=18");
            Check.That(result.RejectionReason).Equals(RejectionReason.NotMeetCriteria);
        }
        [Fact]
        public void ParseDuplicateMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Ingress Portal Duplicate: Fresque Naïve"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = rejectUrlEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result.SubmissionStatus).Equals(SubmissionStatus.Rejected);
            Check.That(result.DateReject).Equals(new DateTime(2014, 11, 16, 14, 14, 06, DateTimeKind.Local));
            Check.That(result.Title).Equals("Fresque Naïve");
            Check.That(result.ImageUrl).Equals(rejectImageUri);
            Check.That(result.RejectionReason).Equals(RejectionReason.Duplicate);
        }
        [Fact]
        public void ParseEditMessage_Test()
        {
            var message = new Message
            {
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new MessagePartHeader {Name = "Date", Value = "Sun, 16 Nov 2014 13:14:06 +0000"},
                        new MessagePartHeader {Name="Subject", Value = "Ingress Portal Data Edit NewAccepted: Fresque Naïve"},
                    },
                    Parts = new List<MessagePart>
                    {
                        new MessagePart
                        {
                            MimeType = "text/html",
                            Body = new MessagePartBody
                            {
                                Data = editedUrlEncodedText
                            }
                        }
                    }
                },
            };
            var target = new PortalSubmissionParser();
            var result = target.ParseMessage(message);
            Check.That(result).IsNull();
        }
        [Fact]
        public void DecodeBase64Url_test()
        {
            var result = PortalSubmissionParser.DecodeBase64Url(submissionUrlEncodedText);
            Check.That(result).Equals(submissionHtmlBodyText);
        }

        [Fact]
        public void EncoreBase64Url_Test()
        {
            var result = PortalSubmissionParser.EncodeBase64Url(submissionHtmlBodyText);
            Check.That(result).Equals(submissionUrlEncodedText);
            result = PortalSubmissionParser.EncodeBase64Url(rejectHtmlNewFormatBodyText);
        }
        [Fact]
        public void ExtractImageUrlTest()
        {
            var result = PortalSubmissionParser.ExtractImageUrl(submissionHtmlBodyText);
            Check.That(result).Equals(new Uri(submissionImageUri));
        }
        [Fact]
        public void ExtractImageUrlNoImageTest()
        {
            var result = PortalSubmissionParser.ExtractImageUrl("");
            Check.That(result).IsNull();
        }

        [Fact]
        public void ExtractPortalUrlTest()
        {
            var result = PortalSubmissionParser.ExtractPortalUrl(validationHtmlBodyText);
            Check.That(result).Equals(new Uri(portalUri));
        }
    }
}
