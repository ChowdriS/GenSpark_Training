
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Repository;
using shop_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen();
// opt =>
// {
//     opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopAPi", Version = "v1" });
//     opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please enter token",
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         BearerFormat = "JWT",
//         Scheme = "bearer"
//     });
//     opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type=ReferenceType.SecurityScheme,
//                     Id="Bearer"
//                 }
//             },
//             new string[]{}
//         }
//     });
// }
// );

builder.Services.AddTransient<ICategoryService,CategoryService>();
builder.Services.AddTransient<IProductService,ProductService>();
builder.Services.AddTransient<IColorService,ColorService>();
builder.Services.AddTransient<IContactService,ContactService>();
builder.Services.AddTransient<INewsService,NewsService>();
builder.Services.AddTransient<IOrderService,OrderService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

builder.Services.AddTransient<IRepository<int, Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<int, Color>, ColorRepository>();
builder.Services.AddTransient<IRepository<int, ContactU>, ContactURepository>();
builder.Services.AddTransient<IRepository<int, Model>, ModelRepository>();
builder.Services.AddTransient<IRepository<int, News>, NewsRepository>();
builder.Services.AddTransient<IRepository<int, Order>, OrderRepository>();
builder.Services.AddTransient<IRepository<(int, int), OrderDetail>, OrderDetailRepository>();
builder.Services.AddTransient<IRepository<int, Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<int, User>, UserRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });;
builder.Services.AddDistributedMemoryCache(); 

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddDbContext<ShopContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();  
app.UseCors("AllowSpecificOrigin");
// app.UseAuthentication();
// app.UseAuthorization();
// app.UseRateLimiter();
app.MapControllers();

app.Run();
