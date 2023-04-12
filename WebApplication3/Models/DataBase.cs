using AutoMapper;
using System.Collections.Generic;
using System.Linq;
namespace WebApplication3.Models
{
    public interface IDataBase
    {
        public List<FriendViewModel> GetAllFreinds();
    }
    public class DataBase : IDataBase
    {
        private List<FriendViewModel> _lists;
        private readonly IMapper mapper;
        public DataBase(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public List<FriendViewModel> GetAllFreinds()
        {
            if (_lists != null)
                return _lists;
            List<Friend> list =WebApplication3.Controllers.FriendsController.freinds.ToList();
            return list.Select(a => mapper.Map<Friend, FriendViewModel>(a)).ToList();
        }
    }
}
