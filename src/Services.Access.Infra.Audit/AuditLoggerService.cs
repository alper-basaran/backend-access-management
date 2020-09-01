using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Access.Domain.Interfaces.Services;
using Services.Access.Domain.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Access.Infra.Audit
{
    public class AuditLoggerService : IAuditLoggerService
    {
        private readonly AuditServiceOptions _options;
        public AuditLoggerService(IOptions<AuditServiceOptions> options)
        {
            _options = options.Value;
        }
        public async Task LogEvent(AuditEvent auditEvent)
        {
            var apiKey = _options.ApiKey;
            var sinkUrl = $"http://{_options.Host}:{_options.Port}/{_options.Endpoint}";

            var json = JsonConvert.SerializeObject(auditEvent);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);
                
                var response = await httpClient.PostAsync(sinkUrl, data);
                string result = response.Content.ReadAsStringAsync().Result;
            }


        }
    }
}
