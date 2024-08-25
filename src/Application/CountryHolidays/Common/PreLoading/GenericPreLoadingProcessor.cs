using Application.Interfaces.Services.External;

using MediatR.Pipeline;

namespace Application.CountryHolidays.Common.PreLoading;

public class GenericPreLoadingProcessor<TRequest, TLoadData>(
    ILoadStep<TLoadData> _loader
    ) : IRequestPreProcessor<TRequest> where TRequest : IPreLoadable<TLoadData>
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var loadData = request.GeneratePreLoadData();

        if (await _loader.ShouldLoadAsync(loadData))
        {
            await _loader.LoadAsync(loadData);
        }
    }
}