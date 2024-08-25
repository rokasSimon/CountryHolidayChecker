using MediatR;
namespace Application.Interfaces.Services.External;

public interface ILoadStep<TRequest>
{
    Task<bool> ShouldLoadAsync(TRequest request);
    Task LoadAsync(TRequest request);
}