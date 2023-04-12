using WebApplication3.Data;
using WebApplication3.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Services
{
    public class FriendsRepository : IFriendsRepository
    {
        private FrindDbContext DbContext;

        public FriendsRepository(FrindDbContext context)
        {
            DbContext = context;
        }
        public async Task<List<Friend>> read()
        {
            var testfriend = await DbContext.Friend.FirstOrDefaultAsync();
            if (testfriend == null)
                foreach (var item in WebApplication3.Controllers.FriendsController.freinds)
                    create(new Friend() { Image = item.Image, Name = item.Name, Number = item.Number });
            var friendlist = await DbContext.Friend.ToListAsync();
            return friendlist;
        }
        public async Task<Friend> read(int id)
        {
            return await DbContext.Friend.FindAsync(id);
        }
        public async void create(Friend model)
        {
            DbContext.Friend.Add(model);
            saveAsync();
        }
        public async void update(Friend model)
        {
            DbContext.Friend.Update(model);
            saveAsync();
        }
        public  void update_v2(Friend model)
        {
            DbContext.Friend.Update(model);
            DbContext.SaveChanges();
        }
        public async void delete(int id1)
        {
            Friend friend = DbContext.Friend.Find(id1);
            DbContext.Friend.Remove(friend);
            saveAsync();
        }
        async void saveAsync()
        {
            await DbContext.SaveChangesAsync(); 
        }
    }
}
