// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Azure.Messaging.ServiceBus;

public class Sender //Publicador
{
    static string connString = "";
    static string topicName = "topic01";
    static async Task Main(string[] args)
    {
        List<TipoPagamento> pagamentos = new List<TipoPagamento>();
        pagamentos.Add(new TipoPagamento() { Id = 1, tipoPagamento = "boleto" });
        pagamentos.Add(new TipoPagamento() { Id = 2, tipoPagamento = "dinheiro" });
        pagamentos.Add(new TipoPagamento() { Id = 3, tipoPagamento = "boleto" });
        pagamentos.Add(new TipoPagamento() { Id = 4, tipoPagamento = "reembolso" });
        pagamentos.Add(new TipoPagamento() { Id = 5, tipoPagamento = "pix" });

        var client = new ServiceBusClient(connString);
        var sender = client.CreateSender(topicName);
        using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
        Console.WriteLine("Enviando mensagens...", ConsoleColor.Green);
        foreach (var pagamento in pagamentos) //enviando cada mensagem
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(pagamento))
            {
                ContentType = "application/json",
                Subject = pagamento.tipoPagamento
            };
            message.ApplicationProperties.Add("tipoPagamento", pagamento.tipoPagamento.ToString());
            System.Console.WriteLine($"Enviando: {pagamento.tipoPagamento}", ConsoleColor.Green);
            await sender.SendMessageAsync(message);
        }
        await sender.CloseAsync();
        System.Console.WriteLine("Mensagens enviadas", ConsoleColor.Green);
    }
}
public class TipoPagamento
{
    public int Id { get; set; }
    public string tipoPagamento { get; set; } = string.Empty;

}