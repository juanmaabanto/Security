using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using MediatR;
using N5.Challenge.Services.Security.API.Application.Adapters;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Repositories;

namespace N5.Challenge.Services.Security.API.Application.Queries
{
    public record GetPermissionsQuery(string employeeForename, string employeeSurname)
        : IRequest<IEnumerable<Permission>>
    {
        public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<Permission>>
        {
            private readonly IPermissionRepository _repository;

            public GetPermissionsQueryHandler(IPermissionRepository? repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }
            
            public async Task<IEnumerable<Permission>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
            {
                return await _repository.FindPermissionAsync(p => p.EmployeeForename == query.employeeForename && p.EmployeeSurname == query.employeeSurname);
            }
        }
    }
}