
using LXP.Api.Interceptors;

using LXP.Data.Repository;
using LXP.Data.IRepository;

using LXP.Core.Services;
using LXP.Core.IServices;

using Serilog;
using LXP.Data.DBContexts;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // Set up Serilog as the logging provider

//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add<ApiExceptionInterceptor>();
//});

builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseLevelServices, CourseLevelServices>();
builder.Services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();
builder.Services.AddScoped<ICourseTopicRepository,CourseTopicRepository>();
builder.Services.AddScoped<ICourseTopicServices,CourseTopicServices>();
builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<ICourseServices,CourseServices>();
//builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
//builder.Services.AddScoped<IMaterialServices, MaterialServices>();
builder.Services.AddScoped<IMaterialRepository,MaterialRepository>();
builder.Services.AddScoped<IMaterialServices,MaterialServices>();
builder.Services.AddScoped<IMaterialTypeRepository,MaterialTypeRepository>();
builder.Services.AddScoped<IMaterialTypeServices,MaterialTypeServices>();
builder.Services.AddScoped<LXPDbContext>();
builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificationOrigin",
        builder => builder.WithOrigins("http://localhost:3000/")
        .AllowAnyHeader()
         .AllowAnyOrigin()
       .AllowAnyMethod());
}
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

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
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificationOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
