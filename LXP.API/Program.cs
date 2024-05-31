using FluentValidation.AspNetCore;
using LXP.Common.Entities;
using LXP.Common.Validators;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Core.Repositories;
using LXP.Core.Services;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using LXP.Services;
using LXP.Services.IServices;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using Serilog;
using System.Reflection;
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

builder.Services.AddScoped<ICourseTopicRepository, CourseTopicRepository>();
builder.Services.AddScoped<ICourseTopicServices, CourseTopicServices>();

builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IMaterialTypeRepository, MaterialTypeRepository>();
builder.Services.AddScoped<IMaterialTypeServices, MaterialTypeServices>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialServices, MaterialServices>();
builder.Services.AddScoped<IUserReportServices, UserReportServices>();
builder.Services.AddScoped<IUserReportRepository, UserReportRepository>();
//Learner
builder.Services.AddScoped<ILearnerServices, LearnerServices>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>();

builder.Services.AddScoped<LXPDbContext>();



//Quiz 
// Register the IQuizRepository and QuizRepository
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizQuestionService, QuizQuestionService>();
builder.Services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
builder.Services.AddScoped<IBulkQuestionRepository, BulkQuestionRepository>();
builder.Services.AddScoped<IBulkQuestionService, BulkQuestionService>();
builder.Services.AddScoped<IQuizFeedbackService, QuizFeedbackService>();
builder.Services.AddScoped<IQuizFeedbackRepository, QuizFeedbackRepository>();
builder.Services.AddScoped<ITopicFeedbackRepository, TopicFeedbackRepository>();
builder.Services.AddScoped<IQuizEngineRepository, QuizEngineRepository>();
builder.Services.AddScoped<IQuizEngineService, QuizEngineService>();
builder.Services.AddScoped<IQuizFeedbackService, QuizFeedbackService>();
builder.Services.AddScoped<ITopicFeedbackService, TopicFeedbackService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuizReportServices, QuizReportServices>();
builder.Services.AddScoped<IQuizReportRepository, QuizReportRepository>();
builder.Services.AddScoped<IFeedbackResponseRepository, FeedbackResponseRepository>();
builder.Services.AddScoped<IFeedbackResponseService,FeedbackResponseService>();

builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<LXPDbContext>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ILearnerService, LearnerService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
builder.Services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();

builder.Services.AddScoped<IMaterialServices, MaterialServices>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialTypeRepository, MaterialTypeRepository>();
builder.Services.AddScoped<IMaterialTypeServices, MaterialTypeServices>();




builder.Services.AddScoped<IProfilePasswordHistoryRepository, ProfilePasswordHistoryRepository>();
builder.Services.AddScoped<ICourseLevelServices, CourseLevelServices>();
builder.Services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();
builder.Services.AddScoped<ICourseTopicRepository, CourseTopicRepository>();
builder.Services.AddScoped<ICourseTopicServices, CourseTopicServices>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add<ApiExceptionInterceptor>();
//});

builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });



builder.Services.AddTransient<BulkQuizQuestionViewModelValidator>();
builder.Services.AddTransient<TopicFeedbackResponseViewModelValidator>();
builder.Services.AddTransient<QuizFeedbackResponseViewModelValidator>();


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
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "CourseMaterial")),
    RequestPath = "/wwwroot/CourseMaterial"
});
app.UseCors("_myAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
