using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GlodnyStudent.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GlodnyStudent.Models
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            this.CreateMap<Restaurant, RestaurantListViewModel>().ForMember(c=>c.Cuisine, o => o.MapFrom(m => m.CuisineType.Name));
            this.CreateMap<Restaurant, RestaurantDetailsViewModel>();
        }
    }
}
