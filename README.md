# BizTalk to Azure Durable Functions Migration  

Detta projekt är en **Azure Durable Functions**-baserad lösning för att ersätta **BizTalk Long Running Transactions** med **Azure Functions**.  
Exemplet visar en **inköpsorder-process** där en order skickas, väntar på bekräftelse och sedan skickas vidare.  

## 📦 Funktioner i projektet  

- **`PurchaseOrderOrchestrator`** – Huvudprocessen som hanterar orderflödet  
- **`SendOrder`** – Activity Function som skickar ordern  
- **`ForwardConfirmedOrder`** – Activity Function som skickar vidare en bekräftad order  
- **`HandleTimeout`** – Activity Function som hanterar timeout om ingen bekräftelse kommer  
- **`StartPurchaseOrderOrchestration`** – HTTP-trigger för att starta processen  
- **`ConfirmOrder`** – HTTP-trigger för att bekräfta ordern  

---

## Script för att testa

Kör nedan CURL-anrop för att starta processen och skicka in en Purchase Order
```
curl -X POST "http://localhost:7296/api/StartPurchaseOrderOrchestration" \
     -H "Content-Type: application/json" \
     -d '{ "id": "12345", "customer": "Wizardworks", "amount": 5000 }'
```
Response
```
{
  "Orchestration started. Instance ID: "abc123"
}
```

Kör nedan CURL-anrop för att confirmera ordern
```
curl -X POST "http://localhost:7296/api/ConfirmOrder?instanceId=abc123" \
     -H "Content-Type: application/json" \
     -d '"Order confirmed successfully"'
```
