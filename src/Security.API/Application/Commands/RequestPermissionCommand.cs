using MediatR;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Exceptions;
using N5.Challenge.Services.Security.Domain.Repositories;
using Nest;

namespace N5.Challenge.Services.Security.API.Application.Commands
{
    public record RequestPermissionCommand(int permissionTypeId, string employeeForename, string employeeSurname)
        : MediatR.IRequest<Permission>
    {
        public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, Permission>
        {
            private readonly IElasticClient _elasticClient;
            private readonly IPermissionRepository _repository;

            public RequestPermissionCommandHandler(IElasticClient? elasticClient, IPermissionRepository? repository)
            {
                _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Permission> Handle(RequestPermissionCommand command, CancellationToken cancellationToken)
            {
                var existent = await _repository.FindPermissionAsync(p =>
                    p.EmployeeSurname == command.employeeSurname &&
                    p.EmployeeForename == command.employeeForename &&
                    p.PermissionTypeId == command.permissionTypeId);

                if(existent.Count() > 0)
                {
                    throw new SecurityDomainException("Same record already exists");
                }

                var item = new Permission(command.permissionTypeId, command.employeeForename, command.employeeSurname);

                // Add to DB
                await _repository.AddAsync(item);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                // Add to ELS index
                await _elasticClient.IndexDocumentAsync(item);

                return item;
            }
        }
    }
}