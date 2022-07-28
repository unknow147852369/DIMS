using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using DIMSApis.Repositories;
using DIMSApis.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add more<
builder.Services.Configure<FireBaseSettings>(builder.Configuration.GetSection("FireBase"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
StripeConfiguration.SetApiKey(builder.Configuration.GetSection("Stripe")["SecretKey"]);

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger  Solution", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
        {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
        }
    },
    new string[] { }
    }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
    };
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<fptdimsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("concac")));


builder.Services.AddScoped<IAuth, AuthRepository>();
builder.Services.AddScoped<IUserManage, UserManageRepository>();
builder.Services.AddScoped<IBookingManage, BookingManageRepository>();
builder.Services.AddScoped<IQrManage, QrManageRepository>();
builder.Services.AddScoped<IAdminManage, AdminManageRepository>();
builder.Services.AddScoped<IHostManage, HostManageRepository>();
builder.Services.AddScoped<IHotelManage, HotelMangeRepository>();


builder.Services.AddScoped<ITokenService, DIMSApis.Services.TokenService>();
builder.Services.AddScoped<IOtherService, OtherService>();
builder.Services.AddScoped<IMail, MailService>();
builder.Services.AddScoped<IGenerateQr, GenerateQrImageStringService>();
builder.Services.AddScoped<IStripePayment, StripePaymentService>();
builder.Services.AddScoped<IMailQrService, MailQrService>();
builder.Services.AddScoped<IFireBaseService, FireBaseService>();
builder.Services.AddScoped<IMailBillService, MailBillService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IMailCheckOut, MailQrCheckOutBillService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
//>Add more

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
//Add more<
//PAHI NAM TRC HAN app.UseAuthorization();
app.UseAuthentication();
app.UseRouting();
app.UseCors(a => a
.SetIsOriginAllowed(origin => true)
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
);
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//>Add more
app.Run();
