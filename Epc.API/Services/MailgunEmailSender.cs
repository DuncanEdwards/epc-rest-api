using Epc.API.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Epc.API.Services
{
    public class MailgunEmailSender : IEmailSender
    {

        #region Private Fields

        private readonly EmailSettings _emailSettings;

        #endregion

        #region Public Constructor

        public MailgunEmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        #endregion

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_emailSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(_emailSettings.ApiKey)));
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("from", _emailSettings.From),
                    new KeyValuePair<string, string>("to", email),
                    new KeyValuePair<string, string>("subject", subject),
                    new KeyValuePair<string, string>("text", message)
                });

                var test = await client.PostAsync(_emailSettings.RequestUri, content);

                await client.PostAsync(_emailSettings.RequestUri, content).ConfigureAwait(false);
            }
        }
    }
}
