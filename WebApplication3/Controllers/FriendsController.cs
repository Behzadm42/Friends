using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication3.Models;
using WebApplication3.Services;


namespace WebApplication3.Controllers
{
    public class FriendsController : Controller
    {
        readonly IDataBase db;
        readonly IMapper mapper;
        IFriendsRepository friendRepository;
        private IWebHostEnvironment env;
        public FriendsController(IWebHostEnvironment webHostEnvironment, IDataBase db, IMapper mapper, IFriendsRepository friendsRepository)
        {
            env = webHostEnvironment;
            this.db = db;
            this.mapper = mapper;
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
        // for  RecurringJob with hangfire
        public void toUpper(List<Friend> friends)
        {
            foreach (var item in friends)
            {
                if (item.Name != item.Name.ToUpper())
                {
                    string s = item.Name.ToUpper();
                    item.Name = s;
                    friendRepository.update_v2(item);
                    break;
                }
            }
        }
        //Frinds list static
        public static List<Models.Friend> freinds = new List<Models.Friend>(){
            new Models.Friend(1,"09368599352","ali ahmadi","/Image/1.jpg"),
            new Models.Friend(2,"09128599352","behzad movasegh","/Image/2.jpg"),
            new Models.Friend(3,"09305593352","mohammad mohammadi","/Image/3.jpg"),
            new Models.Friend(4,"09368009332","kasra babie","/Image/4.jpg"),
            new Models.Friend(5,"09128009332","saeed arabi","/Image/5.jpg"),
            new Models.Friend(6,"09038599352","pouyan razi","/Image/6.jpg"),
            new Models.Friend(7,"09366666652","amin rahimi","/Image/7.jpg"),
            new Models.Friend(8,"09368500052","ali alinezhad","/Image/8.jpg"),
            new Models.Friend(9,"09368599300","zahra ahmadi","/Image/9.jpg"),
            new Models.Friend(10,"09378549352","maryam moradi","/Image/10.jpg"),};
        [ResponseCache(VaryByHeader ="User-Agent",Duration =30)]
        [HttpGet]
        //Home
        public IActionResult Index()
        {
            return  RedirectToAction("List");
        }
        [HttpGet]
        //Show Frind List
        public async Task<IActionResult> List()
        {
            
            var FriendList =await friendRepository.read();
          RecurringJob.AddOrUpdate("myrecurringforUppercase", () => toUpper(FriendList), Cron.Minutely);
            var FriendViewModelList = FriendList.Select(a => mapper.Map<Friend, FriendViewModel>(a)).ToList();
            return View(FriendViewModelList);
        }
        [HttpGet]
        //Show a special friend
        public async Task<IActionResult> Detail(int id = 1)
        {
            
            var a = mapper.Map<Friend, FriendViewModel>(await friendRepository.read(id));
            return View(a);
        }
        [HttpGet]
        //Show a insert page
        public IActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        //Get the result of insert page
        public async Task<IActionResult> Insert(Models.FriendViewModel friend)
        {
            int Id = (await friendRepository.read()).LastOrDefault().Id + 1;
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
            if (this.ModelState.IsValid)
            {

                var a = mapper.Map<FriendViewModel, Friend>(friend);
                friendRepository.create(a);
                return Redirect("/Friends/List");
            }
            return View();
        }
    }
}
