﻿using FluentValidation;
using FluentValidation.Results;

namespace Users.Application.Exceptions
{
    public class UserValidationException : ValidationException
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public UserValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
        {
            Type = "user-validation-exception";
            Title = "User validation exception.";
        }
    }
}
