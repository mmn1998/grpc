using Greeting;
using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcServiceTemplate.Services;

public  class GreetingServiceImpl : GreetingService.GreetingServiceBase
{
    public override Task<GreetingResponse> TGreet(GreetingRequest request, ServerCallContext context)
    {        
        return Task.FromResult(new GreetingResponse
        {
            Result = request.Greeting.SirstName + " " + request.Greeting.LastName
        });
    }
    public override Task<GreetingResponse> Test(GreetingRequest request, ServerCallContext context)
    {
        return base.Test(request, context);
    }
}
