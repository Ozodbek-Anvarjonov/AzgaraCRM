using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services.Interfaces;
using AzgaraCRM.WebUI.Validations.Statistics;

namespace AzgaraCRM.WebUI.Services;

public class StatisticsService(IUnitOfWork unitOfWork) : IStatisticsService
{
    public async Task<StatisticsModelView> GetAsync(CancellationToken cancellationToken = default)
    {
        var categories = unitOfWork.Categories.SelectAsQueryable(entity => !entity.IsDeleted);
        var foods = unitOfWork.Foods.SelectAsQueryable(entity => !entity.IsDeleted);
        var admins = unitOfWork.Users.SelectAsQueryable(entity => entity.Role == UserRole.Admin && !entity.IsDeleted);

        var statistics = new StatisticsModelView()
        {
            CategoryCount = categories.Count(),
            FoodCount = foods.Count(),
            AdminCount = admins.Count(),
        };

        return await Task.FromResult(statistics);
    }
}