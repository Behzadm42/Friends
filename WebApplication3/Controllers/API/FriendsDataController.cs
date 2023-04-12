using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsDataController : ControllerBase
    {
        private readonly FrindDbContext _context;

        
     
        IFriendsRepository friendRepository;
        private IWebHostEnvironment env;
        public FriendsDataController(IWebHostEnvironment webHostEnvironment, FrindDbContext context, IFriendsRepository friendsRepository)
        {
            env = webHostEnvironment;
            _context = context;
            this.friendRepository = friendsRepository;
        }
        /// <summary>
        /// for download image from url
        /// </summary>
        /// <param name="client"></param>
        /// <param name="image"></param>
        /// <param name="filepath"></param>
        void DownloadImage(WebClient client, string image, string filepath)
        {
            try
            {
                client.DownloadFile(new Uri(image), filepath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // GET: api/FriendsData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriend()
        {
          if (_context.Friend == null)
          {
              return NotFound();
          }
            return await _context.Friend.ToListAsync();
        }

        // GET: api/FriendsData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetFriend(int id)
        {
          if (_context.Friend == null)
          {
              return NotFound();
          }
            var friend = await _context.Friend.FindAsync(id);

            if (friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // PUT: api/FriendsData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriend(int id, Friend friend)
        {
            if (id != friend.Id)
            {
                return BadRequest();
            }

            _context.Entry(friend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FriendsData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Friend>> PostFriend(Friend friend)
        {
          if (_context.Friend == null)
          {
              return Problem("Entity set 'FrindDbContext.Friend'  is null.");
          }
            int Id =(await friendRepository.read()).LastOrDefault().Id + 1;
            var filepath = Path.Combine(env.WebRootPath, "Image", Id.ToString() + ".jpg");

            using (WebClient client = new WebClient())
            {
                if (!System.IO.File.Exists(filepath))
                {
                    DownloadImage(client, friend.Image, filepath);
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(filepath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        DownloadImage(client, friend.Image, filepath);
                    }
                }
            }
            friend.Image = $"/Image/{Id}.jpg";
            _context.Friend.Add(friend);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFriend", new { id = friend.Id }, friend);
        }

        // DELETE: api/FriendsData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriend(int id)
        {
            if (_context.Friend == null)
            {
                return NotFound();
            }
            var friend = await _context.Friend.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.Friend.Remove(friend);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FriendExists(int id)
        {
            return (_context.Friend?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
