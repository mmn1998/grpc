using gRPC.Server.Services;
using GrpcServiceTemplate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
string BaseSSlAddress = "..\\..\\/server/ssl/";

string serverCert = File.ReadAllText(BaseSSlAddress + "server.crt");
string serverKey = File.ReadAllText(BaseSSlAddress + "server.key");
string caCert = File.ReadAllText(BaseSSlAddress + "ca.crt");
//var cert = X509Certificate2.CreateFromPem(serverCert, serverKey);
var cert = new X509Certificate2(BaseSSlAddress + "ca.crt");


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7153, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps(cert);
    });
});
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<GreetingServiceImpl>();
app.MapGrpcService<TesterService>();
app.MapGrpcService<PrimeServiceImpl>();
app.MapGrpcService<FindMaxSeriviceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
