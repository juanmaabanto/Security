using MediatR;
using N5.Challenge.Services.Security.API.Infrastructure.Exceptions;
using N5.Challenge.Services.Security.Domain.Exceptions;
using N5.Challenge.Services.Security.Domain.Repositories;
using Nest;

namespace N5.Challenge.Services.Security.API.Application.Commands
{
    public record ModifyPermissionCommand(int permissionId, int permissionTypeId, string employeeForename, string employeeSurname)
        : MediatR.IRequest
    {
        public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand>
        {
            private readonly IElasticClient _elasticClient;
            private readonly IPermissionRepository _repository;

            public ModifyPermissionCommandHandler(IElasticClient? elasticClient, IPermissionRepository? repository)
            {
                _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Unit> Handle(ModifyPermissionCommand command, CancellationToken cancellationToken)
            {
                var existent = await _repository.FindPermissionAsync(p =>
                    p.Id != command.permissionTypeId &&
                    p.EmployeeSurname == command.employeeSurname.Trim() &&
                    p.EmployeeForename == command.employeeForename.Trim() &&
                    p.PermissionTypeId == command.permissionTypeId);

                if(existent.Count() > 0)
                {
                    throw new SecurityDomainException("Same record already exists");
                }

                var permission = await _repository.FindByIdAsync(command.permissionId);

                if(permission is null)
                {
                    throw new PermissionNotFoundException(command.permissionId);
                }

                permission.Modify(command.permissionTypeId, command.employeeForename, command.employeeSurname);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

                // Update object in ELS index
                await _elasticClient.IndexDocumentAsync(permission);

                return Unit.Value;
            }
        }
    }
}