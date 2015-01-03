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

        private const string rejectUrlEncodedText =
            "PGRpdiBkaXI9Imx0ciI-PHA-VGhhbmsgeW91IGZvciB5b3VyIFBvcnRhbCBzdWJtaXNzaW9uLiBIb3dldmVyLCB0aGlzIFBvcnRhbCBjYW5kaWRhdGUgZG9lcyBub3QgbWVldCB0aGUgY3JpdGVyaWEgcmVxdWlyZWQgZm9yIGFwcHJvdmFsLjwvcD48cD5QbGVhc2UgcmVmZXIgdG8gPGEgaHJlZj0iaHR0cHM6Ly9zdXBwb3J0Lmdvb2dsZS5jb20vaW5ncmVzcy9hbnN3ZXIvMzA2NjE5Nz9obD1lbiZyZWZfdG9waWM9Mjc5OTI3MCI-TmV3IFBvcnRhbCBTdWJtaXNzaW9uPC9hPiBjcml0ZXJpYSBhdCBvdXIgSGVscCBDZW50ZXIgZm9yIGZ1cnRoZXIgaW5mb3JtYXRpb24uPC9wPjxpbWcgc3JjPSJodHRwOi8vbGgzLmdncGh0LmNvbS9UNi1IM1VJdGxha2xUNml6V3BobDJPQVdxY04zWmk0LTVObnhLRWE4VXpJa2V6NGViRUxzRmFmbzRaLWxoUHJJUE5DLUVUcVZydEh0SWNhYlN5WW1adyIgYWx0PSJQb3J0YWwgLSBBbnRlbm5lIFJlbGFpcyBEdSBQaWMgTmVvdWxvdXMiIHN0eWxlPSJkaXNwbGF5OiBibG9jaztib3JkZXI6IDA7aGVpZ2h0OiBhdXRvO2xpbmUtaGVpZ2h0OiAxMDAlO291dGxpbmU6IG5vbmU7dGV4dC1kZWNvcmF0aW9uOiBub25lOyI-PC9kaXY-";
        private const string rejectHtmlBodyText = "<div dir=\"ltr\"><p>Thank you for your Portal submission. However, this Portal candidate does not meet the criteria required for approval.</p><p>Please refer to <a href=\"https://support.google.com/ingress/answer/3066197?hl=en&ref_topic=2799270\">New Portal Submission</a> criteria at our Help Center for further information.</p><img src=\"http://lh3.ggpht.com/T6-H3UItlaklT6izWphl2OAWqcN3Zi4-5NnxKEa8UzIkez4ebELsFafo4Z-lhPrIPNC-ETqVrtHtIcabSyYmZw\" alt=\"Portal - Antenne Relais Du Pic Neoulous\" style=\"display: block;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;\"></div>";
        private const string rejectImageUri = "http://lh3.ggpht.com/T6-H3UItlaklT6izWphl2OAWqcN3Zi4-5NnxKEa8UzIkez4ebELsFafo4Z-lhPrIPNC-ETqVrtHtIcabSyYmZw";
        private const string editedUrlEncodedText = "PGRpdiBkaXI9Imx0ciI-PHA-VGhhbmsgeW91IGZvciB5b3VyIEluZ3Jlc3MgUG9ydGFsIGRhdGEgZWRpdCBzdWJtaXNzaW9uLiBOSUEgb3BlcmF0aXZlcyBoYXZlIHJldmlld2VkIGFuZCBhY2NlcHRlZCB5b3VyIHByb3Bvc2VkIGNoYW5nZXMuIFRoZXNlIG1vZGlmaWNhdGlvbnMgc2hvdWxkIGJlIGxpdmUgc29vbi48L3A-PHA-MjAwIEFQIGhhcyBiZWVuIGF3YXJkZWQgdG8geW91ciBhY2NvdW50LjwvcD48cD5Qb3J0YWw6IExhZHk8L3A-PGRpdj48YSBocmVmPSJodHRwczovL3d3dy5pbmdyZXNzLmNvbS9pbnRlbD9sbD00OC44Njg5OTcsMi4zNTg4MjUmYW1wO3o9MTgiPjItNiBUYXlsb3IgU3RyZWV0LCA3NTAxMCBQYXJpcywgRnJhbmNlPC9hPjwvZGl2PjxpbWcgc3JjPSJodHRwOi8vbGgzLmdncGh0LmNvbS9lRnp5cWREVTROU2JsbUhra29lUGU5aEUwYWJaM2d6MGQ1Zm1LX2FsUm9XN184Q09KWTZrb2NXZlRiVGU0ZmpUZ2VPRkktOHl4UXNubVlpb2VWZyIgYWx0PSJQb3J0YWwgLSBMYWR5IiBzdHlsZT0iZGlzcGxheTogYmxvY2s7Ym9yZGVyOiAwO2hlaWdodDogYXV0bztsaW5lLWhlaWdodDogMTAwJTtvdXRsaW5lOiBub25lO3RleHQtZGVjb3JhdGlvbjogbm9uZTsiPjwvZGl2Pg==";

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
        public void ConvertBase64Url_test()
        {
            var result = PortalSubmissionParser.DecodeBase64Url(submissionUrlEncodedText);
            Check.That(result).Equals(submissionHtmlBodyText);
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
