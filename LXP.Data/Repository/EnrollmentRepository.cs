using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LXP.Data.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;

        public EnrollmentRepository(
            LXPDbContext lXPDbContext,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _lXPDbContext = lXPDbContext;
            _environment = webHostEnvironment;
            _contextAccessor = httpContextAccessor;
        }

        public async Task Addenroll(Enrollment enrollment)
        {
            await _lXPDbContext.Enrollments.AddAsync(enrollment);
            await _lXPDbContext.SaveChangesAsync();
        }

        public bool AnyEnrollmentByLearnerAndCourse(Guid LearnerId, Guid CourseId)
        {
            return _lXPDbContext.Enrollments.Any(enrollment =>
                enrollment.LearnerId == LearnerId && enrollment.CourseId == CourseId
            );
        }

        public object GetCourseandTopicsByLearnerId(Guid learnerId)
        {
            var result =
                from enrollment in _lXPDbContext.Enrollments
                where enrollment.LearnerId == learnerId
                select new
                {
                    enrollmentid = enrollment.EnrollmentId,
                    enrolledCourseId = enrollment.CourseId,
                    enrolledCoursename = enrollment.Course.Title,
                    enrolledcoursedescription = enrollment.Course.Description,
                    enrolledcoursecategory = enrollment.Course.Category.Category,
                    enrolledcourselevels = enrollment.Course.Level.Level,
                    Thumbnailimage = String.Format(
                        "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        enrollment.Course.Thumbnail
                    ),

                    Topics = (
                        from topic in _lXPDbContext.Topics
                        where topic.CourseId == enrollment.CourseId && topic.IsActive == true
                        select new
                        {
                            TopicName = topic.Name,
                            TopicDescription = topic.Description,
                            TopicId = topic.TopicId,
                            TopicIsActive = topic.IsActive,
                            Materials = (
                                from material in _lXPDbContext.Materials
                                join materialType in _lXPDbContext.MaterialTypes
                                    on material.MaterialTypeId equals materialType.MaterialTypeId

                                where material.TopicId == topic.TopicId
                                select new
                                {
                                    MaterialId = material.MaterialId,
                                    MaterialName = material.Name,
                                    MaterialType = materialType.Type,
                                    Material = String.Format(
                                        "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                        _contextAccessor.HttpContext.Request.Scheme,
                                        _contextAccessor.HttpContext.Request.Host,
                                        _contextAccessor.HttpContext.Request.PathBase,
                                        material.FilePath
                                    ),
                                    MaterialDuration = material.Duration
                                }
                            ).ToList(),
                            //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
                            //{
                            //    MaterialType=materialType.Type,
                            //    MaterialTypeId=materialType.MaterialTypeId,

                            //}).ToList(),
                        }
                    ).ToList()
                };
            return result;
        }

        public IEnumerable<EnrollmentReportViewModel> GetEnrollmentReport()
        {
            var course = _lXPDbContext
                .Enrollments.GroupBy(x => x.CourseId)
                .Select(x => new EnrollmentReportViewModel
                {
                    CourseId = x.First().CourseId,
                    CourseName = x.First().Course.Title,
                    EnrolledUsers = x.GroupBy(x => x.LearnerId).Count(),
                    InprogressUsers = x.Where(x => x.CompletedStatus == 0).Count(),
                    CompletedUsers = x.Where(x => x.CompletedStatus == 1).Count(),
                })
                .ToList();
            return course;
        }

        public IEnumerable<EnrolledUserViewModel> GetEnrolledUser(Guid courseId)
        {
            var users = _lXPDbContext
                .Enrollments.Where(x => x.CourseId == courseId)
                .Select(x => new EnrolledUserViewModel
                {
                    LearnerId = x.LearnerId,
                    Name =
                        x.Learner.LearnerProfiles.First().FirstName
                        + " "
                        + x.Learner.LearnerProfiles.First().LastName,
                    ProfilePhoto = String.Format(
                        "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        x.Learner.LearnerProfiles.First().ProfilePhoto
                    ),
                    Status = x.CompletedStatus,
                    EmailId = x.Learner.Email,
                });
            return users;
        }

        public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(
            Guid courseId
        )
        {
            var CompletedLearner = _lXPDbContext
                .Enrollments.Where(e => e.CourseId == courseId && e.CompletedStatus == 1)
                .GroupBy(e => e.LearnerId)
                .Select(e => new EnrollmentReportViewModel
                {
                    CourseId = e.First().CourseId,
                    LearnerId = e.Key,
                    LearnerName = e.First().Learner.LearnerProfiles.First().FirstName,
                    ProfilePhoto = String.Format(
                        "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                        _contextAccessor.HttpContext!.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        e.First().Learner.LearnerProfiles.First().ProfilePhoto
                    ),
                    EmailId = e.First().Learner.Email,
                    CourseCompletionPercentage = e.First().CourseCompletionPercentage,
                })
                .ToList();
            return CompletedLearner;
        }

        public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(
            Guid courseId
        )
        {
            var InprogressLearner = _lXPDbContext
                .Enrollments.Where(e => e.CourseId == courseId && e.CompletedStatus != 1)
                .GroupBy(e => e.LearnerId)
                .Select(e => new EnrollmentReportViewModel
                {
                    CourseId = e.First().CourseId,
                    LearnerId = e.Key,
                    LearnerName = e.First().Learner.LearnerProfiles.First().FirstName,
                    ProfilePhoto = String.Format(
                        "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                        _contextAccessor.HttpContext!.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        e.First().Learner.LearnerProfiles.First().ProfilePhoto
                    ),
                    EmailId = e.First().Learner.Email,
                    CourseCompletionPercentage = e.First().CourseCompletionPercentage,
                })
                .ToList();
            return InprogressLearner;
        }

        public Enrollment FindEnrollmentId(Guid enrollmentId)
        {
            return _lXPDbContext.Enrollments.Find(enrollmentId);
        }

        public async Task DeleteEnrollment(Enrollment enrollment)
        {
            _lXPDbContext.Enrollments.Remove(enrollment);
            await _lXPDbContext.SaveChangesAsync();
        }

        public object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId)
        {
            var result =
                from enrollment in _lXPDbContext.Enrollments
                where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
                select new
                {
                    enrollmentid = enrollment.EnrollmentId,
                    enrolledCourseId = enrollment.CourseId,
                    enrolledCoursename = enrollment.Course.Title,
                    enrolledcoursedescription = enrollment.Course.Description,
                    enrolledcoursecategory = enrollment.Course.Category.Category,
                    enrolledcourselevels = enrollment.Course.Level.Level,
                    Thumbnailimage = String.Format(
                        "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                        _contextAccessor.HttpContext.Request.Scheme,
                        _contextAccessor.HttpContext.Request.Host,
                        _contextAccessor.HttpContext.Request.PathBase,
                        enrollment.Course.Thumbnail
                    ),

                    Topics = (
                        from topic in _lXPDbContext.Topics
                        where topic.CourseId == enrollment.CourseId && topic.IsActive == true
                        select new
                        {
                            TopicName = topic.Name,
                            TopicDescription = topic.Description,
                            TopicId = topic.TopicId,
                            TopicIsActive = topic.IsActive,
                            IsQuiz = _lXPDbContext.Quizzes.Any(quizzes =>
                                quizzes.TopicId == topic.TopicId
                            )
                                ? (
                                    from q in _lXPDbContext.Quizzes
                                    join la in _lXPDbContext.LearnerAttempts
                                        on q.QuizId equals la.QuizId
                                    where la.LearnerId == learnerId && q.TopicId == topic.TopicId
                                    group la by new { la.QuizId, q.PassMark } into g
                                    where g.Max(x => x.Score) >= g.Key.PassMark
                                    select g.Key.PassMark
                                ).Count() == 0
                                : false,
                            IsFeedBack = _lXPDbContext.Topicfeedbackquestions.Any(
                                topicfeedbackquesion =>
                                    topicfeedbackquesion.TopicId == topic.TopicId
                            )
                                ? (
                                    from tfq in _lXPDbContext.Topicfeedbackquestions
                                    join fr in _lXPDbContext.Feedbackresponses
                                        on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                                    where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
                                    select tfq
                                ).Count() == 0
                                : false,
                            isAttemptOver = (
                                from quiz in _lXPDbContext.Quizzes
                                join attempt in _lXPDbContext.LearnerAttempts
                                    on quiz.QuizId equals attempt.QuizId
                                where
                                    attempt.LearnerId == learnerId && quiz.TopicId == topic.TopicId
                                group attempt by quiz.AttemptsAllowed into g
                                where g.Max(x => x.AttemptCount) == g.Key
                                select g.Key
                            ).Count() == 1,
                            isPassed = (
                                from q in _lXPDbContext.Quizzes
                                join la in _lXPDbContext.LearnerAttempts
                                    on q.QuizId equals la.QuizId
                                where la.LearnerId == learnerId && q.TopicId == topic.TopicId
                                group la by new { la.QuizId, q.PassMark } into g
                                where g.Max(x => x.Score) >= g.Key.PassMark
                                select g.Key.PassMark
                            ).Count() > 0,
                            Materials = (
                                from material in _lXPDbContext.Materials
                                join materialType in _lXPDbContext.MaterialTypes
                                    on material.MaterialTypeId equals materialType.MaterialTypeId

                                where material.TopicId == topic.TopicId && material.IsActive == true
                                select new
                                {
                                    MaterialId = material.MaterialId,
                                    MaterialName = material.Name,
                                    MaterialType = materialType.Type,
                                    Material = String.Format(
                                        "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                        _contextAccessor.HttpContext.Request.Scheme,
                                        _contextAccessor.HttpContext.Request.Host,
                                        _contextAccessor.HttpContext.Request.PathBase,
                                        material.FilePath
                                    ),
                                    MaterialDuration = material.Duration
                                }
                            ).ToList(),
                        }
                    ).ToList()
                };
            return result;
        }
    }
}



//public object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId) // 27062024
//{
//    var result =
//        from enrollment in _lXPDbContext.Enrollments
//        where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//        select new
//        {
//            enrollmentid = enrollment.EnrollmentId,
//            enrolledCourseId = enrollment.CourseId,
//            enrolledCoursename = enrollment.Course.Title,
//            enrolledcoursedescription = enrollment.Course.Description,
//            enrolledcoursecategory = enrollment.Course.Category.Category,
//            enrolledcourselevels = enrollment.Course.Level.Level,
//            Thumbnailimage = String.Format(
//                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                _contextAccessor.HttpContext.Request.Scheme,
//                _contextAccessor.HttpContext.Request.Host,
//                _contextAccessor.HttpContext.Request.PathBase,
//                enrollment.Course.Thumbnail
//            ),

//            Topics = (
//                from topic in _lXPDbContext.Topics
//                where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                orderby topic.CreatedAt ascending
//                select new
//                {
//                    TopicName = topic.Name,
//                    TopicDescription = topic.Description,
//                    TopicId = topic.TopicId,
//                    TopicIsActive = topic.IsActive,
//                    IsQuiz = _lXPDbContext.Quizzes.Any(quizzes =>
//                        quizzes.TopicId == topic.TopicId
//                    )
//                        ? (
//                            from q in _lXPDbContext.Quizzes
//                            join la in _lXPDbContext.LearnerAttempts
//                                on q.QuizId equals la.QuizId
//                            where la.LearnerId == learnerId && q.TopicId == topic.TopicId
//                            group la by new { la.QuizId, q.PassMark } into g
//                            where g.Max(x => x.Score) >= g.Key.PassMark
//                            select g.Key.PassMark
//                        ).Count() == 0
//                        : false,
//                    IsFeedBack = _lXPDbContext.Topicfeedbackquestions.Any(
//                        topicfeedbackquesion =>
//                            topicfeedbackquesion.TopicId == topic.TopicId
//                    )
//                        ? (
//                            from tfq in _lXPDbContext.Topicfeedbackquestions
//                            join fr in _lXPDbContext.Feedbackresponses
//                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
//                            where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
//                            select tfq
//                        ).Count() == 0
//                        : false,
//                    isAttemptOver = (
//                        from quiz in _lXPDbContext.Quizzes
//                        join attempt in _lXPDbContext.LearnerAttempts
//                            on quiz.QuizId equals attempt.QuizId
//                        where
//                            attempt.LearnerId == learnerId && quiz.TopicId == topic.TopicId
//                        group attempt by quiz.AttemptsAllowed into g
//                        where g.Max(x => x.AttemptCount) == g.Key
//                        select g.Key
//                    ).Count() == 1,
//                    Materials = (
//                        from material in _lXPDbContext.Materials
//                        join materialType in _lXPDbContext.MaterialTypes
//                            on material.MaterialTypeId equals materialType.MaterialTypeId

//                        where material.TopicId == topic.TopicId && material.IsActive == true
//                        select new
//                        {
//                            MaterialId = material.MaterialId,
//                            MaterialName = material.Name,
//                            MaterialType = materialType.Type,
//                            Material = String.Format(
//                                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                _contextAccessor.HttpContext.Request.Scheme,
//                                _contextAccessor.HttpContext.Request.Host,
//                                _contextAccessor.HttpContext.Request.PathBase,
//                                material.FilePath
//                            ),
//                            MaterialDuration = material.Duration
//                        }
//                    ).ToList(),
//                }
//            ).ToList()
//        };
//    return result;
//}


//public object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId)
//{
//    var result =
//        from enrollment in _lXPDbContext.Enrollments
//        where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//        select new
//        {
//            enrollmentid = enrollment.EnrollmentId,
//            enrolledCourseId = enrollment.CourseId,
//            enrolledCoursename = enrollment.Course.Title,
//            enrolledcoursedescription = enrollment.Course.Description,
//            enrolledcoursecategory = enrollment.Course.Category.Category,
//            enrolledcourselevels = enrollment.Course.Level.Level,
//            Thumbnailimage = String.Format(
//                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                _contextAccessor.HttpContext.Request.Scheme,
//                _contextAccessor.HttpContext.Request.Host,
//                _contextAccessor.HttpContext.Request.PathBase,
//                enrollment.Course.Thumbnail
//            ),

//            Topics = (
//                from topic in _lXPDbContext.Topics
//                where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                select new
//                {
//                    TopicName = topic.Name,
//                    TopicDescription = topic.Description,
//                    TopicId = topic.TopicId,
//                    TopicIsActive = topic.IsActive,
//                    Materials = (
//                        from material in _lXPDbContext.Materials
//                        join materialType in _lXPDbContext.MaterialTypes
//                            on material.MaterialTypeId equals materialType.MaterialTypeId

//                        where material.TopicId == topic.TopicId && material.IsActive == true
//                        select new
//                        {
//                            MaterialId = material.MaterialId,
//                            MaterialName = material.Name,
//                            MaterialType = materialType.Type,
//                            Material = String.Format(
//                                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                _contextAccessor.HttpContext.Request.Scheme,
//                                _contextAccessor.HttpContext.Request.Host,
//                                _contextAccessor.HttpContext.Request.PathBase,
//                                material.FilePath
//                            ),
//                            MaterialDuration = material.Duration
//                        }
//                    ).ToList(),
//                    //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
//                    //{
//                    //    MaterialType=materialType.Type,
//                    //    MaterialTypeId=materialType.MaterialTypeId,

//                    //}).ToList(),
//                }
//            ).ToList()
//        };
//    return result;
//}
// public object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId)
// {
//     var result =
//         from enrollment in _lXPDbContext.Enrollments
//         where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//         select new
//         {
//             enrollmentid = enrollment.EnrollmentId,
//             enrolledCourseId = enrollment.CourseId,
//             enrolledCoursename = enrollment.Course.Title,
//             enrolledcoursedescription = enrollment.Course.Description,
//             enrolledcoursecategory = enrollment.Course.Category.Category,
//             enrolledcourselevels = enrollment.Course.Level.Level,
//             Thumbnailimage = String.Format(
//                 "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                 _contextAccessor.HttpContext.Request.Scheme,
//                 _contextAccessor.HttpContext.Request.Host,
//                 _contextAccessor.HttpContext.Request.PathBase,
//                 enrollment.Course.Thumbnail
//             ),

//             Topics = (
//                 from topic in _lXPDbContext.Topics
//                 where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                 select new
//                 {
//                     TopicName = topic.Name,
//                     TopicDescription = topic.Description,
//                     TopicId = topic.TopicId,
//                     TopicIsActive = topic.IsActive,
//                     IsQuiz = _lXPDbContext.Quizzes.Any(quizzes =>
//                         quizzes.TopicId == topic.TopicId
//                     )
//                         ? (
//                             from q in _lXPDbContext.Quizzes
//                             join la in _lXPDbContext.LearnerAttempts
//                                 on q.QuizId equals la.QuizId
//                             where la.LearnerId == learnerId && q.TopicId == topic.TopicId
//                             group la by new { la.QuizId, q.PassMark } into g
//                             where g.Max(x => x.Score) >= g.Key.PassMark
//                             select g.Key.PassMark
//                         ).Count() != 0
//                         : true,
//                     IsFeedBack = _lXPDbContext.Topicfeedbackquestions.Any(
//                         topicfeedbackquesion =>
//                             topicfeedbackquesion.TopicId == topic.TopicId
//                     )
//                         ? (
//                             from tfq in _lXPDbContext.Topicfeedbackquestions
//                             join fr in _lXPDbContext.Feedbackresponses
//                                 on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
//                             where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
//                             select tfq
//                         ).Count() == 0
//                         : true,
//                     Materials = (
//                         from material in _lXPDbContext.Materials
//                         join materialType in _lXPDbContext.MaterialTypes
//                             on material.MaterialTypeId equals materialType.MaterialTypeId

//                         where material.TopicId == topic.TopicId && material.IsActive == true
//                         select new
//                         {
//                             MaterialId = material.MaterialId,
//                             MaterialName = material.Name,
//                             MaterialType = materialType.Type,
//                             Material = String.Format(
//                                 "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                 _contextAccessor.HttpContext.Request.Scheme,
//                                 _contextAccessor.HttpContext.Request.Host,
//                                 _contextAccessor.HttpContext.Request.PathBase,
//                                 material.FilePath
//                             ),
//                             MaterialDuration = material.Duration
//                         }
//                     ).ToList(),
//                     //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
//                     //{
//                     //    MaterialType=materialType.Type,
//                     //    MaterialTypeId=materialType.MaterialTypeId,

//                     //}).ToList(),
//                 }
//             ).ToList()
//         };
//     return result;
// }
