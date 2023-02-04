// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

public class Receiver //Consumidor
{
    static string connString = "Endpoint=sb://clinamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xTyRmgGwg8cvb8eEogQLaxKLXohu1dW7oHN5Ui4mgWc=";
    static string topicName = "topic01";
    static string fila = "geral";
    static async Task Main(string[] args)
    {
        var client = new ServiceBusClient(connString);
        var sender = client.CreateReceiver(topicName, fila);
        Console.WriteLine("Processando mensagens...");
        while (true)
        {
            var message = await sender.ReceiveMessageAsync();
            if (message is not null)
            {
                System.Console.WriteLine($"Processando: {message.Body}");
                await sender.CompleteMessageAsync(message);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Todas os pagamentos foram processados");
                break;
            }
        }
        await sender.CloseAsync();
    }
}