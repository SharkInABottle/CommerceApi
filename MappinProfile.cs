using CommerceApi.Models;

using AutoMapper;
using CommerceApi.Entities;

namespace CommerceApi
{
    public class MappinProfile : Profile
    {
        public MappinProfile()
        {
            CreateMap<NewSale, Sale>()
                .ForMember(dest => dest.Images, options =>
                {
                    options.MapFrom(src=>src.Images.Select(x=>
                    
                        new Images()
                        {
                            Id = x.Id,
                            ImagePath = x.ImagePath,
                            Url = x.Url
                            
                        }
                    ));
                });
            CreateMap<EditSale, Sale>();
        }
    }
}
