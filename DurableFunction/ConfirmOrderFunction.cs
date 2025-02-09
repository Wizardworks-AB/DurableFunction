using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DurableFunction
{
    public class ConfirmOrderFunction
    {
        [Function("ConfirmOrder")]
        public async Task<HttpResponseData> ConfirmOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ConfirmOrder");

            string instanceId = req.Query["instanceId"];

            string confirmation = await new StreamReader(req.Body).ReadToEndAsync();

            await client.RaiseEventAsync(instanceId, "OrderConfirmed", confirmation);

            logger.LogInformation($"Bekräftelse mottagen för {instanceId}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Order confirmed");
            return response;
        }
    }

}
