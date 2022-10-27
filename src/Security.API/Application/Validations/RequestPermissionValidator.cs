using FluentValidation;
using N5.Challenge.Services.Security.API.Application.Commands;

namespace N5.Challenge.Services.Security.API.Application.Validations
{
    public class RequestPermissionValidator : AbstractValidator<RequestPermissionCommand>
    {
        public RequestPermissionValidator()
        {
            RuleFor(command => command.permissionTypeId).GreaterThan(0);
            RuleFor(command => command.employeeForename).Length(2, 20).NotEmpty();
            RuleFor(command => command.employeeSurname).Length(2, 20).NotEmpty();
        }
    }
}