using Microsoft.EntityFrameworkCore;
using Restaurant.BusinessLogic.Base;
using Restaurant.DataAccess;
using Restaurant.Web.Code;
using Restaurant.Web.Code.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(BaseService).Assembly);

builder.Services.AddDbContext<RestaurantContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddTicketAppCurrentUser();

builder.Services.AddPresentation();
builder.Services.AddTicketAppBusinessLogic();

builder.Services.AddAuthentication("RestaurantCookies")
       .AddCookie("RestaurantCookies", options =>
       {
           options.AccessDeniedPath = new PathString("/User/Login");
           options.LoginPath = new PathString("/User/Login");
       });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
