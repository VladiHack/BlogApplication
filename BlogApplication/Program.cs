using BlogApplication.Models;
using BlogApplication.Services.Categories;
using BlogApplication.Services.Comments;
using BlogApplication.Services.Posts;
using BlogApplication.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogDbContext>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPostsService, PostService>();
builder.Services.AddTransient<ICommentsService, CommentsService>();
builder.Services.AddTransient<ICategoriesService, CategoriesService>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
