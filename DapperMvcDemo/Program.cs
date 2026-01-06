using DapperMvcDemo.Data;
using DapperMvcDemo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// DI 등록하기
builder.Services.AddSingleton<DapperDbContext>();                    // DapperDbContext를 싱글톤으로 등록
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // IProductRepository 구현체 등록

builder.Services.AddControllersWithViews();

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
