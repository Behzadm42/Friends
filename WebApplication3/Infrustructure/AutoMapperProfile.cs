using AutoMapper;
namespace WebApplication3.Infrustructure
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Friend,Models.FriendViewModel>();
            CreateMap<Models.FriendViewModel, Models.Friend>();
        }
    }
}
