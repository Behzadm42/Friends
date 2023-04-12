using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication3.Services;



string FriendJson = JsonConvert.SerializeObject(WebApplication3.Controllers.FriendsController.freinds);
var builder = WebApplication.CreateBuilder(args);
// Add Hangfire
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseStorage(new MemoryStorage()));
// Add Swagger
builder.Services.AddSwaggerGen();
// Add Mapper
var autoMapperConfig = new MapperConfiguration(a =>
{
    a.AddProfile<WebApplication3.Infrustructure.AutoMapperProfile>();
});
var mapper = autoMapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
// Add services to the container.
builder.Services.AddControllersWithViews();
// Add DI for Database
Type FriendsService = typeof(WebApplication3.Models.IDataBase);
Type Implementation = typeof(WebApplication3.Models.DataBase);
builder.Services.AddSingleton(FriendsService, Implementation);
// Add DI for FrendServices
builder.Services.AddScoped(typeof(IFriendsRepository), typeof(FriendsRepository));

// Add ConnectionString
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<WebApplication3.Data.FrindDbContext>(Configs =>
{
    Configs.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});


var app = builder.Build();
// Use Hangfire
app.UseHangfireDashboard();
app.UseHangfireServer();
// Use Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Middleware to display the number of friends
app.Map("/total-friends", (context) =>
{
    context.Run(async httpcontext => { await httpcontext.Response.WriteAsync(WebApplication3.Controllers.FriendsController.freinds.Count.ToString()); });
});
//Middleware to display the number of friends
app.Map("/friends-json", (context) =>
{
    context.Run(async httpcontext => { await httpcontext.Response.WriteAsync(FriendJson); });
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
