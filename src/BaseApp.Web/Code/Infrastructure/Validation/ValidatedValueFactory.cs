using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.Common.Extensions;
using FluentValidation.Results;

namespace BaseApp.Web.Code.Infrastructure.Validation;

public class ValidatedValueFactory
{
    public const string AbortWorkflowValidationKey = "AbortWorkflow";
    public const string AbortWorkflowValidationFileMessage = "Internal error during file storing.";
    
    public static ValidatedValue Create(ValidationResult validationResult, Action action)
    {
        if (validationResult.IsValid)
        {
            action();
            return new ValidatedValue(null);
        }
        return new ValidatedValue(GetValidationItems(validationResult));
    }
    
    public static ValidatedValue<T> Create<T>(ValidationResult validationResult, Func<T> action)
    {
        if (validationResult.IsValid)
        {
            return new ValidatedValue<T>(null, action());
        }
        return new ValidatedValue<T>(GetValidationItems(validationResult), default);
    }
    
    public static async Task<ValidatedValue> CreateAsync(ValidationResult validationResult, Func<Task> actionAsync)
    {
        if (validationResult.IsValid)
        {
            await actionAsync();
            return new ValidatedValue(null);
        }
        return new ValidatedValue(GetValidationItems(validationResult));
    }
    
    public static async Task<ValidatedValue<T>> CreateAsync<T>(ValidationResult validationResult, Func<Task<T>> actionAsync)
    {
        if (validationResult.IsValid)
        {
            return new ValidatedValue<T>(null, await actionAsync());
        }
        return new ValidatedValue<T>(GetValidationItems(validationResult), default);
    }

    private static ValidationItemModel[] GetValidationItems(ValidationResult validationResult)
    {
        return validationResult.Errors.GroupBy(x => x.PropertyName).Select(x => new ValidationItemModel
        {
            PropertyName = x.Key,
            ErrorMessage = "; ".UseForJoinNonEmpty(x.Select(e => e.ErrorMessage))
        }).ToArray();
    }

    public static IEnumerable<ValidationFailure> ConvertFromComponentValidation(IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationResults)
    {
        foreach (var validationResult in validationResults)
        {
            if (validationResult.MemberNames.IsNullOrEmpty())
            {
                yield return new ValidationFailure
                {
                    ErrorMessage = validationResult.ErrorMessage
                };
            }
            else
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    yield return new ValidationFailure(memberName, validationResult.ErrorMessage);
                }
            }
        }
    }
}