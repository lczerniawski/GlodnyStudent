using AutoMapper;
using GlodnyStudent.Models.Domain;
using GlodnyStudent.ViewModels;

namespace GlodnyStudent.Models
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantListViewModel>()
                .ForMember(c=>c.Cuisine, o => o.MapFrom(m => m.Cuisine.Name))
                .ForMember(a=>a.Address,o=> o.MapFrom(m=>m.Address.Street + ' ' + m.Address.StreetNumber + '/' + m.Address.LocalNumber));

            CreateMap<Restaurant, RestaurantDetailsViewModel>();
            CreateMap<Review, ReviewViewModel>();
            CreateMap<MenuItemViewModel, MenuItem>();
            CreateMap<MenuItem, MenuViewModel>();
            CreateMap<RestaurantAddress, AddressViewModel>().ReverseMap();
        }
    }
}
