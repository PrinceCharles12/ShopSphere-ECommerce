using ShopSphereMVC.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddHttpClient<ProductService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<CartService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<OrderService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<PaymentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<WishlistService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
});

builder.Services.AddHttpClient<ReviewService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5066/api/");
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
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

