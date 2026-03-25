# FCG Lambda Payments

Função AWS Lambda para processamento de pagamentos com cartão de crédito, usando .NET 8.

## Visão geral

Este projeto recebe uma requisição HTTP (via API Gateway/Kong), transforma o payload em um evento de pedido (`OrderPlacedEvent`), processa o pagamento via fachada (`CreditCardPaymentFacade`) e retorna os dados da transação.

## Estrutura da solução

- `FCG.Lambda.Payments`: projeto principal da Lambda.
- `FCG.Lambda.Payments.Core`: contratos de integração (eventos).
- `FCG.Lambda.Payments.FakePaymentProvider`: provedor fake para simular autorização/captura.

## Fluxo de processamento

1. Uma requisição chega na Lambda (`FunctionHandler`) com o JSON do pedido.
2. O corpo é desserializado para `OrderPlacedEvent`.
3. A função monta a entidade de domínio `Payment` com os dados do cartão (`CreditCard`).
4. A função instancia `PaymentConfig` com chaves padrão (mock) e cria a `CreditCardPaymentFacade`.
5. A fachada:
   - cria `FakePaymentService`;
   - gera `CardHash`;
   - cria `TransactionFake`;
   - chama `AuthorizeCardTransaction()`.
6. O resultado fake é mapeado para a entidade de domínio `Transaction`.
7. A Lambda retorna `200` com o JSON da transação.
8. Em caso de erro, retorna `500` com mensagem e stack trace.

## Exemplo de payload

```json
{
  "ClientId": 11,
  "OrderId": "7f3c2c5a-6d3b-4c1e-9a6f-1c2b3d4e5f60",
  "PaymentMethod": 1,
  "Amount": 3.99,
  "CardName": "Cardname Blah",
  "CardNumber": "123456789",
  "ExpirationDate": "02/2027",
  "Cvv": "123"
}
```

## Endpoint

- AWS API Gateway: `https://unb1zq8jj5.execute-api.us-east-2.amazonaws.com/default/FCGPayments`
- Kong local: `http://localhost:8000/payments`

## Observações

- O provedor de pagamento é **fake** e possui comportamento simulado.
- A autorização no fake provider pode variar (aprovado/recusado).
- O handler configurado para deploy está em `aws-lambda-tools-defaults.json`:
  - `FCG.Lambda.Payments::FCG.Lambda.Payments.Function::FunctionHandler`
