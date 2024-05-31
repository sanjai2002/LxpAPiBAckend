using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Http;

namespace LXP.Common.Validator
{
    public class EnrollmentValidators : AbstractValidator<EnrollmentViewModel>

    {
        public EnrollmentValidators() 
        {
            RuleFor(enroll => enroll.CourseId)
                .NotEmpty().WithMessage("Course Name is required");

            RuleFor(enroll => enroll.LearnerId)
                .NotEmpty().WithMessage("Learner Name is required");

            RuleFor(enroll => enroll.EnrollmentDate)
                .NotEmpty().WithMessage("Enrollment Date is required");
        }

        private bool IsValidDate(DateTime date)
        {
            DateTime minDate = new DateTime(2024, 1, 1);
            DateTime maxDate = new DateTime(2099, 12, 31);

            return date >= minDate && date <= maxDate;
        }

    }
}
