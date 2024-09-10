
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_sem3.Controllers;
using Project_sem3.Hubs;
using Project_sem3.InterFace;
using Project_sem3.MobileInterface;
using Project_sem3.MobileRepo;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.Repositories;
using Project_sem3.SendMail;
using Project_sem3.SqlTableDependencies;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<dataContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
    
    
});



var allow = builder.Configuration.GetSection("AlloweOrigins").Get<string[]>();

builder.Services.AddCors(op =>
{
    op.AddPolicy("myAppCors", policy =>
    {
        policy.WithOrigins(allow).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });

});
builder.Services.AddScoped<IProductFE, ProductFERepo>();
builder.Services.AddScoped<ICategoryFE, CategoryFERepo>();
builder.Services.AddScoped<IAuthFE, AuthFERepo>();
builder.Services.AddScoped<ICartFE, CartFERepo>();
builder.Services.AddScoped<IVoucherFE, VoucherFERepo>();
builder.Services.AddScoped<IOrderFE, OrderFERepo>();
builder.Services.AddScoped<IUserFE, UserFERepo>();
builder.Services.AddScoped<IStoreFE, StoreFERepo>();
builder.Services.AddScoped<IInteractFE, InteractFERepo>();





//Mobile
builder.Services.AddTransient<MbIProduct, MbProductRepo>();



builder.Services.AddSingleton<MiddleCheck>();
builder.Services.AddSignalR();
builder.Services.AddTransient<IPermission, PermissionRepo>();
builder.Services.AddTransient<IStore,StoreRepo>();
builder.Services.AddTransient<IAdmin, AdminRepo>();
builder.Services.AddTransient<IBrand, BrandRepo>();
builder.Services.AddTransient<ICategory, CategoryRepo>();
builder.Services.AddTransient<ISubcategory, SubcategoryRepo>();
builder.Services.AddTransient<ISegment, SegmentRepo>();
builder.Services.AddTransient<IShipping, ShippingRepo>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IFlashSale, FlashSaleRepo>();
builder.Services.AddTransient<IDiscount, DiscountRepo>();
builder.Services.AddTransient<IVoucher, VoucherRepo>();
builder.Services.AddTransient<IProduct, ProductRepo>();
builder.Services.AddTransient<IProperties, PropertiesRepo>();
builder.Services.AddTransient<IGoods, GoodRepo>();
builder.Services.AddTransient<IEvent, EventRepo>();
builder.Services.AddTransient<IRate, RateRepo>();
builder.Services.AddTransient<IQuestion, QuestionRepo>();
builder.Services.AddTransient<IOrder, OrderRepo>();
builder.Services.AddTransient<IChart, ChartRepo>();
builder.Services.AddTransient<IReport, ReportRepo>();
builder.Services.AddTransient<IImportFile, ImportFileRepo>();
builder.Services.AddSignalR().AddJsonProtocol(options => {
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<DemoHubs>();
builder.Services.AddSingleton<demoDependencies>();
builder.Services.AddSingleton<StoreHubRepo>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.SaveToken = true;
    op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddHostedService<NotificationController>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<DemoHubs>("/api/Demo-hub");
app.UseStaticFiles();
app.UseAuthorization();
app.UseCors("myAppCors");
app.MapControllers();
app.UseMiddleware<MiddleCheck>();

//app.Use(async (context, next) =>
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var service = scope.ServiceProvider.GetService<demoDependencies>();
//        if (service != null)
//        {
//            service.Listen();

//        }
//    }
//    await next(context);
//});

app.Run();
