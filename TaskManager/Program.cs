using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Data;
using TaskManager.Services;
using TaskManager.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAccountManager, AccountManager>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(60);
    options.SlidingExpiration = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password policies
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// Add JWT authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,  // Make sure to validate token lifetime
            ValidIssuer = "Task_Api",
            ValidAudience = "Xamarin_Mobile_App",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3X4mp13_Str0ng_S3cr3t_K3y_!@#$%^&*")),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

// CORS configuration

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    // Security Headers Middleware
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");

    await next();
});

// Enable CORS
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=GetTasks}/{id?}");
app.MapRazorPages();

app.Run();
