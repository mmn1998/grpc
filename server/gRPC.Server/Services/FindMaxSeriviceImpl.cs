using Grpc.Core;
using Max;
using System.Threading.Tasks;

namespace GrpcServiceTemplate.Services;

public class FindMaxSeriviceImpl : FindMaxService.FindMaxServiceBase
{
    public override async Task FindMax(IAsyncStreamReader<FindMaxRequest> requestStream, IServerStreamWriter<FindMaxResponse> responseStream, ServerCallContext context)
    {
        int? max = null;

        while (await requestStream.MoveNext())
        {
            if (max is null || max < requestStream.Current.Number)
            {
                max = requestStream.Current.Number;
                await responseStream.WriteAsync(new FindMaxResponse
                {
                    Max = max.Value
                });
            }
        }
    }
}
