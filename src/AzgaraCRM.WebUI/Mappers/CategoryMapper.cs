using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Models.Categories;
using AzgaraCRM.WebUI.Models.Foods;

namespace AzgaraCRM.WebUI.Mappers;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CreateCategoryModel, Category>();
        CreateMap<UpdateCategoryModel, Category>();
        CreateMap<Category, CategoryModelView>()
            .ConstructUsing((src, context) => new CategoryModelView
            {
                Foods = context.Mapper.Map<IEnumerable<FoodModelView>>(src.Foods)
            }); ;
    }
}