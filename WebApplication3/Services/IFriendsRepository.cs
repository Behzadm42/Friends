using WebApplication3.Models;

namespace WebApplication3.Services
{
    public interface IFriendsRepository
    {
        void create(Friend model);
        void delete(int id1);
        Task<List<Friend>> read();
        Task<Friend> read(int id);
        void update(Friend model);
        void update_v2(Friend model);
    }
}