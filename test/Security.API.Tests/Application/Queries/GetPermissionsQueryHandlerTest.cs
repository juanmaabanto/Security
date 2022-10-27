using System.Linq.Expressions;
using Moq;
using N5.Challenge.Services.Security.API.Application.Queries;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Repositories;
using static N5.Challenge.Services.Security.API.Application.Queries.GetPermissionsQuery;

namespace N5.Challenge.Services.Security.API.Tests.Application.Queries
{
    public class GetPermissionsQueryHandlerTest
    {
        private readonly Mock<IPermissionRepository> _repositoryMock;
        
        public GetPermissionsQueryHandlerTest()
        {
            _repositoryMock = new Mock<IPermissionRepository>();
        }

        [Fact]
        public async Task Handle_return_list()
        {
            //Arrange
            var query = new GetPermissionsQuery("test", "test");
            var expectedItem = new Permission(1, "test", "test");

            _repositoryMock.Setup(x => x.FindPermissionAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
                .ReturnsAsync(new List<Permission> { expectedItem });

            //Act
            var queryHandler = new GetPermissionsQueryHandler(_repositoryMock.Object);
            var result = await queryHandler.Handle(query, CancellationToken.None);

            //Assert
            foreach(var r in result)
            {
                Assert.Equal(expectedItem.PermissionTypeId, r.PermissionTypeId);
                Assert.Equal(expectedItem.EmployeeForename, r.EmployeeForename);
                Assert.Equal(expectedItem.EmployeeSurname, r.EmployeeSurname);
            }

        }
    }
}