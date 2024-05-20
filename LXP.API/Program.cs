using LXP.Common;
using LXP.Core.IServices;
using LXP.Core.Services;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

#region CORS setting for API
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
    policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowAnyMethod();
    }

    );
});

#endregion
// Add services to the container.
// Add services to the container.
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IForgetRepository, ForgetRepository>();
builder.Services.AddScoped<IForgetService, ForgetService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IService, Services>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUpdatePasswordService, UpdatePasswordService>();
builder.Services.AddScoped<IUpdatePasswordRepository, UpdatePasswordRepository>();
//Course 
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICourseLevelServices, CourseLevelServices>();
builder.Services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();

builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

//Learner
builder.Services.AddScoped<ILearnerServices, LearnerServices>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>();

builder.Services.AddSingleton<LXPDbContext>();





Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "CourseThumbnailImages")),
    RequestPath = "/wwwroot/CourseThumbnailImages"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "LearnerProfileImages")),
    RequestPath = "/wwwroot/LearnerProfileImages"
});

app.UseCors("_myAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
