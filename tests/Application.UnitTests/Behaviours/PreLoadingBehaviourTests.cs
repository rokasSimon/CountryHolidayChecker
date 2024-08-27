using Application.Behaviours;
using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.Interfaces.Services.External;

using Moq;

namespace Application.UnitTests.Behaviours;

[TestFixture]
public class PreLoadingBehaviourTests
{
    private Mock<ILoadStep<CountryHolidayLoadData>> _loadStep;

    [SetUp]
    public void Setup()
    {
        _loadStep = new Mock<ILoadStep<CountryHolidayLoadData>>();
    }

    [Test]
    public async Task Handle_WhenGivenValidLoadData_ShouldCallLoadWithAppropriateData()
    {
        var request = new CountryHolidayLoadData("ltu", 2024);
        _loadStep
            .Setup(x => x.ShouldLoadAsync(request).Result)
            .Returns(true);
        var preLoadBehaviour = new PreLoadingBehaviour<GetDayStatusRequest, CountryHolidayLoadData, GetDayStatusResult>(_loadStep.Object);

        await preLoadBehaviour.Handle(
            new GetDayStatusRequest(2024, 3, 11, "ltu"),
            () => Task.FromResult(It.IsAny<GetDayStatusResult>()),
            new CancellationToken());

        _loadStep.Verify(x => x.ShouldLoadAsync(request), Times.Once);
    }

    [Test]
    public async Task Handle_WhenGivenNewRecord_ShouldCallLoad()
    {
        _loadStep
            .Setup(x => x.ShouldLoadAsync(It.IsAny<CountryHolidayLoadData>()).Result)
            .Returns(true);
        var preLoadBehaviour = new PreLoadingBehaviour<GetDayStatusRequest, CountryHolidayLoadData, GetDayStatusResult>(_loadStep.Object);

        await preLoadBehaviour.Handle(
            new GetDayStatusRequest(2024, 3, 11, "ltu"),
            () => Task.FromResult(It.IsAny<GetDayStatusResult>()),
            new CancellationToken());

        _loadStep.Verify(x => x.LoadAsync(It.IsAny<CountryHolidayLoadData>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenGivenOldRecord_ShouldNotCallLoad()
    {
        _loadStep
            .Setup(x => x.ShouldLoadAsync(It.IsAny<CountryHolidayLoadData>()).Result)
            .Returns(false);
        var preLoadBehaviour = new PreLoadingBehaviour<GetDayStatusRequest, CountryHolidayLoadData, GetDayStatusResult>(_loadStep.Object);

        await preLoadBehaviour.Handle(
            new GetDayStatusRequest(2024, 3, 11, "ltu"),
            () => Task.FromResult(It.IsAny<GetDayStatusResult>()),
            new CancellationToken());

        _loadStep.Verify(x => x.LoadAsync(It.IsAny<CountryHolidayLoadData>()), Times.Never);
    }
}
