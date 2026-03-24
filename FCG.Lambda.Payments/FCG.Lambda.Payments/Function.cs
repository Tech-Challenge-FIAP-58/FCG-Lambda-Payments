using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FCG.Lambda.Payments.Core.Integration;
using FCG.Lambda.Payments.SRC.Domain.Entities;
using FCG.Lambda.Payments.SRC.Domain.Entities.Enums;
using FCG.Lambda.Payments.SRC.Facade;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FCG.Lambda.Payments;


/*
 * EXEMPLO JSON TESTE -> para ser utilizado localmente no Mock Lamba Test Tool
 * 
 * {\"ClientId\":11,\"OrderId\":\"7f3c2c5a-6d3b-4c1e-9a6f-1c2b3d4e5f60\",\"PaymentMethod\":1,\"Amount\":3.99,\"CardName\":\"Cardname Blah\",\"CardNumber\":\"123456789\",\"ExpirationDate\":\"02/2027\",\"Cvv\":\"123\"}
 * 
 */

/*
 * FCGPaymentsAPI 
 * https://unb1zq8jj5.execute-api.us-east-2.amazonaws.com/default/FCGPayments
 * JSON EXEMPLO -> para ser usado no Postman para chamar diretamente a função lambda na AWS armazenada no ambiente do Gilmar Pedretti
 {
  "ClientId": 11,
  "OrderId": "7f3c2c5a-6d3b-4c1e-9a6f-1c2b3d4e5f60",
  "PaymentMethod": 1,
  "Amount": 3.99,
  "CardName": "Cardname Blah",
  "CardNumber": "123456789",
  "ExpirationDate": "02/2027",C
  "Cvv": "123"
 }
 */

/*
 * KONG - API GATEWAY
 * http://localhost:8000/payments
 * pode usar o JSON exemplo acima
 *  
 */

public class Function
{
    public async Task<APIGatewayProxyResponse> FunctionHandler(
        APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        
        try
        {
            var order = JsonConvert.DeserializeObject<OrderPlacedEvent>(request.Body);

            var payment = new Payment(
            order.OrderId,
            (PaymentMethod)order.PaymentMethod,
            order.Amount,
            new CreditCard(
                order.CardName,
                order.CardNumber,
                order.ExpirationDate,
                order.Cvv)
        );

            // configuração fake só para funcionar no trabalho
            var config = Options.Create(new PaymentConfig
            {
                DefaultApiKey = "1234567890123456",        // 16 bytes → AES-128
                DefaultEncryptionKey = "ABCDEF1234567890"  // 16 bytes → IV válido
            });

            var facade = new CreditCardPaymentFacade(config);

            var transaction = await facade.ProcessPayment(payment);

            string json = JsonConvert.SerializeObject(transaction);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = json
            };
        }
        catch(Exception e)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = "Erro ao processar pagamento. Mensagem: " + e.Message + ". StackTrace: " + e.StackTrace
            };
        }
        

        

    }
}
