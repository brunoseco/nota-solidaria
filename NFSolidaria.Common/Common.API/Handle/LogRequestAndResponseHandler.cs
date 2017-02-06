using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.API
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            FactoryLog.GetInstace().Info(requestBody);

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var responseBody = task.Result.Content.ReadAsStringAsync().Result;
                    FactoryLog.GetInstace().Info(responseBody);

                    return task.Result;
                });
        }
    }
}
