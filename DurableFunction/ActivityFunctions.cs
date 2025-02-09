using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace DurableFunction
{
    public class ActivityFunctions
    {
        [Function("SendOrder")]
        public async Task SendOrder([ActivityTrigger] PurchaseOrder order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("SendOrder");
            order.Status = "SentForConfirmation";
            logger.LogInformation($"Skickar inköpsorder {order.Id}, status: {order.Status}");
            // Simulerat API-anrop till externt system
        }

        [Function("ForwardConfirmedOrder")]
        public async Task ForwardConfirmedOrder([ActivityTrigger] string confirmation, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ForwardConfirmedOrder");
            logger.LogInformation($"Order bekräftad: {confirmation}, skickar vidare.");
        }

        [Function("HandleTimeout")]
        public async Task HandleTimeout([ActivityTrigger] PurchaseOrder order, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("HandleTimeout");
            logger.LogWarning($"Timeout för order {order.Id}, ingen bekräftelse mottagen.");
        }
    }
}
