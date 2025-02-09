using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DurableFunction
{
    public class PurchaseOrderOrchestrator
    {
        [Function("PurchaseOrderOrchestrator")]
        public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var order = context.GetInput<PurchaseOrder>();

            // Skicka ordern vidare
            await context.CallActivityAsync("SendOrder", order);

            // V�nta p� bekr�ftelse i upp till 24 timmar
            DateTime deadline = context.CurrentUtcDateTime.AddHours(24);
            Task timeoutTask = context.CreateTimer(deadline, CancellationToken.None);
            Task<string> confirmationTask = context.WaitForExternalEvent<string>("OrderConfirmed");

            Task completedTask = await Task.WhenAny(confirmationTask, timeoutTask);

            if (completedTask == confirmationTask)
            {
                // Bekr�ftelse mottagen, skicka vidare
                order.Status = "Confirmed";
                
                await context.CallActivityAsync("ForwardConfirmedOrder", confirmationTask.Result);
            }
            else
            {
                order.Status = "TimedOut";
                // Timeout intr�ffade, hantera felet
                await context.CallActivityAsync("HandleTimeout", order);
            }
        }
    }
}
