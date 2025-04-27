using AzgaraCRM.WebUI.Validations.Statistics;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsModelView> GetAsync(CancellationToken cancellationToken = default);
}