using AutoMapper;
using IC_ebilet.pl.Models;
using IC_ebilet.pl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.App_Start
{
    public static class AutomapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Favourite, FavouriteViewModel>();
                cfg.CreateMap<FavouriteViewModel, Favourite>();
                cfg.CreateMap<TCategory, TCategoryViewModel>();
                cfg.CreateMap<TCategoryViewModel, TCategory>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>();
                cfg.CreateMap<Event, EventViewModel>();
                cfg.CreateMap<EventViewModel, Event>();
                //cfg.CreateMap<Favourite, FavouriteViewModel>().ForMember(dest => dest.FavCategory, opt => opt.());
                //cfg.CreateMap<FavouriteViewModel, Favourite>().ForMember(dest => dest.FavCategory, opt => opt.Ignore());
            });
        }
    }
}