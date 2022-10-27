using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using N5.Challenge.Services.Security.API.Application.Behaviours;
using N5.Challenge.Services.Security.API.Application.Commands;
using N5.Challenge.Services.Security.API.Application.Validations;

namespace N5.Challenge.Services.Security.API.Infrastructure.AutofacModules
{
    [ExcludeFromCodeCoverage]
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(RequestPermissionCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            
            builder.RegisterAssemblyTypes(typeof(RequestPermissionValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t => { return componentContext.Resolve(t); };
            });

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}