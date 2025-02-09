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
    public class StartOrchestrationFunction
    {
        [Function("StartPurchaseOrderOrchestration")]
        public async Task<HttpResponseData> Start(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("StartPurchaseOrderOrchestration");

            // Läs in ordern från HTTP-requestens body
            var order = await req.ReadFromJsonAsync<PurchaseOrder>();

            order.Status = "Pending";
            
            if (order == null)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid request payload");
                return badRequest;
            }

            // Starta orchestratorn
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync("PurchaseOrderOrchestrator", order);

            logger.LogInformation($"Orchestrator startad med Instance ID: {instanceId}, order status: {order.Status}");

            // Skapa en response med URL:er för att kolla status
            var response = req.CreateResponse(HttpStatusCode.Accepted);
            var statusQueryUri = $"{req.Url.GetLeftPart(UriPartial.Authority)}/runtime/webhooks/durabletask/instances/{instanceId}";
           
            response.Headers.Add("Location", statusQueryUri);

            await response.WriteStringAsync($"Orchestration started. Instance ID: {instanceId}");
            return response;
        }
    }
}
