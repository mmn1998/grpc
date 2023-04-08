using Grpc.Core;
using Prime;
using System.Threading.Tasks;

namespace GrpcServiceTemplate.Services;

public class PrimeServiceImpl : PrimeNumberService.PrimeNumberServiceBase
{
    public override async Task PrimeNumberDecomposition
        (PrimeNumberDecompositionRequest request, IServerStreamWriter<PrimeNumberDecompositionResponse> responseStream, ServerCallContext context)
    {
        int number = request.Number;
        int divisior = 2;

        while (number > 1)
        {
            if (number % divisior == 0)
            {
                number /= divisior;
                await responseStream.WriteAsync(new PrimeNumberDecompositionResponse()
                {
                    PrimeFactor = divisior
                });
            }
            else
                divisior++;
        }
    }
}
