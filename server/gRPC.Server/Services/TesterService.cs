using Grpc.Core;
using System.Threading.Tasks;
using Test;

namespace GrpcServiceTemplate.Services;

public  class TesterService : SumService.SumServiceBase
{
    public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SumResponse
        {
            Result = request.A + request.B,
        });
    }
}
