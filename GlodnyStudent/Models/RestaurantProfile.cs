using AutoMapper;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.ViewModels;

namespace GlodnyStudent.Models
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            this.CreateMap<Restaurant, RestaurantListViewModel>().ForMember(c=>c.Cuisine, o => o.MapFrom(m => m.Cuisine.Name));
            this.CreateMap<Restaurant, RestaurantDetailsViewModel>();
        }
    }
}
