using AutoMapper;
using ElectricalCarStoreApi.Models;
using ElectricalCarStoreApi.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectricalCarStoreApi.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Car, CarListViewModel>();
        }
    }
}
