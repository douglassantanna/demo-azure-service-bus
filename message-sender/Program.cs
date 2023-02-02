// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Azure.Messaging.ServiceBus;

public class Sender //Publicador
{
    static string connString = "Endpoint=sb://sb-teste-ivo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=GQjH8vFzPYC03olK2veqvPOEjNoB7iSsnOcyuGuZkwU=";
    static string topicName = "norgelabs";
    static async Task Main(string[] args)
    {
        List<TipoPagamento> pagamentos = new List<TipoPagamento>();
        pagamentos.Add(new TipoPagamento() { Id = 1, tipoPagamento = "reembolso" });
        pagamentos.Add(new TipoPagamento() { Id = 2, tipoPagamento = "dinheiro" });
        pagamentos.Add(new TipoPagamento() { Id = 3, tipoPagamento = "boleto" });
        pagamentos.Add(new TipoPagamento() { Id = 4, tipoPagamento = "reembolso" });
        pagamentos.Add(new TipoPagamento() { Id = 5, tipoPagamento = "pix" });

        var client = new ServiceBusClient(connString);
        var sender = client.CreateSender(topicName);
        Console.WriteLine("Enviando mensagens...");
        foreach (var pagamento in pagamentos)
        {
            var message = new ServiceBusMessage(JsonSerializer.Serialize(pagamento));
            message.ApplicationProperties.Add("tipoPagamento", pagamento.tipoPagamento.ToString());
            System.Console.WriteLine($"Enviando: {pagamento.tipoPagamento}");
            await sender.SendMessageAsync(message);
        }
        await sender.CloseAsync();
        System.Console.WriteLine("Mensagens enviadas");
    }
}
public class TipoPagamento
{
    public int Id { get; set; }
    public string tipoPagamento { get; set; } = string.Empty;

}