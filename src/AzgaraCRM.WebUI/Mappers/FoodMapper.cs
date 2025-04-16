using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Models.Foods;

namespace AzgaraCRM.WebUI.Mappers;

public class FoodMapper : Profile
{
    public FoodMapper()
    {
        CreateMap<CreateFoodModel, Food>();
        CreateMap<UpdateFoodModel, Food>();
        CreateMap<Food, FoodModelView>()
            .ConstructUsing((src, context) => new FoodModelView
            {
                CategoryName = src.Category?.Name,
            });
    }
}