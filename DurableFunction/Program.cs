using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using DurableFunction;
using Microsoft.Extensions.DependencyInjection;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
builder.Services.AddSingleton<PurchaseOrderOrchestrator>();
builder.Services.AddSingleton<ActivityFunctions>();
builder.Services.AddSingleton<ConfirmOrderFunction>();
builder.Services.AddSingleton<StartOrchestrationFunction>();

builder.Build().Run();
