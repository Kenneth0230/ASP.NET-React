﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using FluentValidation;

namespace EUniversity.Core.Validation.University
{
    public class CreateClassroomDtoValidator : AbstractValidator<CreateClassromDto>
    {
        public CreateClassroomDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.PropertyRequired)
                .WithMessage("Classroom name is required");

            RuleFor(c => c.Name)
                .MaximumLength(Classroom.MaxNameLength)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .WithMessage($"Classroom name cannot exceed {Classroom.MaxNameLength} characters");
        }
    }
}
