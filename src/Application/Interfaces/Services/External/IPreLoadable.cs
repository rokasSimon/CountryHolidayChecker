namespace Application.Interfaces.Services.External;

public interface IPreLoadable<TLoadData>
{
    TLoadData GeneratePreLoadData();
}