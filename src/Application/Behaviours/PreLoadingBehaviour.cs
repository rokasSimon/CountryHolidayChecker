using Application.Interfaces.Services.External;

using MediatR;

namespace Application.Behaviours;

public class PreLoadingBehaviour<TRequest, TLoadData, TResponse>(
    ILoadStep<TLoadData> _loader)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IPreLoadable<TLoadData>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var loadData = request.GeneratePreLoadData();

        if (await _loader.ShouldLoadAsync(loadData))
        {
            await _loader.LoadAsync(loadData);
        }

        return await next();
    }
}
