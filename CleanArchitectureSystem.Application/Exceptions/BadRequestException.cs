﻿using FluentValidation.Results;

namespace CleanArchitectureSystem.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        //public List<string> Errors { get; }

        //public BadRequestException(string message, List<ValidationFailure> validationFailures)
        //    : base(message)
        //{
        //    Errors = validationFailures.Select(vf => vf.ErrorMessage).ToList();
        //}       

        public IDictionary<string, string[]> ValidationErrors { get; }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationErrors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
