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
            .ForMember(dest => dest.Foods,
                opt => opt.MapFrom(src => src.Foods))
            .PreserveReferences();
    }
}