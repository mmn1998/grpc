// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using Greet;
using Greeting;
using Grpc.Core;
using Grpc.Net.Client;
using Max;
using Prime;
using Test;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7095");
#region uni
var client1 = new Greeter.GreeterClient(channel);
var reply = await client1.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);
var client2 = new SumService.SumServiceClient(channel);
var result = await client2.SumAsync(new SumRequest
{
    A = 75,
    B = 10
});
Console.WriteLine("Sum result: " + result.Result);
#endregion
#region Server streaming
var client3 = new PrimeNumberService.PrimeNumberServiceClient(channel);
var response = client3.PrimeNumberDecomposition(new PrimeNumberDecompositionRequest
{
    Number = 120
});
while (await response.ResponseStream.MoveNext())
{
    Console.WriteLine(response.ResponseStream.Current.PrimeFactor);
}
#endregion
#region BI streaming
var client4 = new FindMaxService.FindMaxServiceClient(channel);
var stream = client4.FindMax();
var responseReaderTask = Task.Run(async () =>
{
    while (await stream.ResponseStream.MoveNext())
        Console.WriteLine(stream.ResponseStream.Current.Max);
});
int[] requests = { 1, 0, 3, 8, 5, 7, 12, 13, 19, 15, 18, 21, 26, 56, 38, 40, 98, 100, 150, 69, 75, 220, 320 };
foreach (int number in requests)
{
    await stream.RequestStream.WriteAsync(new FindMaxRequest { Number = number });
}
await stream.RequestStream.CompleteAsync();
await responseReaderTask;
#endregion
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
