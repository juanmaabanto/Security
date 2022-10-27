

using MediatR;
using N5.Challenge.Services.Security.Domain.Entities;

namespace N5.Challenge.Services.Security.API.Application.Queries
{
    public record GetPermissionByIdQuery(string Id) : IRequest<Permission>
    {
        public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, Permission>
        {
            public Task<Permission> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}