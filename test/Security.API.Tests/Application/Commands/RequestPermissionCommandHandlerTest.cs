using System.Linq.Expressions;
using Moq;
using N5.Challenge.Services.Security.API.Application.Commands;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Exceptions;
using N5.Challenge.Services.Security.Domain.Repositories;
using N5.Challenge.Services.Security.Domain.SeedWork;
using Nest;
using static N5.Challenge.Services.Security.API.Application.Commands.RequestPermissionCommand;

namespace N5.Challenge.Services.Security.API.Tests.Application.Commands
{
    public class RequestPermissionCommandHandlerTest
    {
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<IPermissionRepository> _repositoryMock;

        public RequestPermissionCommandHandlerTest()
        {
            _repositoryMock = new Mock<IPermissionRepository>();
            _elasticClientMock = new Mock<IElasticClient>();
            var ctxMock = new Mock<IUnitOfWork>();

            _repositoryMock.SetupGet(x => x.UnitOfWork).Returns(ctxMock.Object);
        }

        [Fact]
        public void Builder_receivenull_elastic_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new RequestPermissionCommandHandler(null, _repositoryMock.Object));
        }

        [Fact]
        public void Builder_receivenull_repository_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new RequestPermissionCommandHandler(_elasticClientMock.Object, null));
        }

        [Fact]
        public async Task Handle_return_item()
        {
            //Arrange
            var item = new Permission(1, "Juan", "Abanto");
            var command = new RequestPermissionCommand(1, "Juan", "Abanto");
            
            _repositoryMock.Setup(x => x.FindPermissionAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
                .ReturnsAsync(new List<Permission>());
            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Permission>()))
                .Returns(Task.CompletedTask);

            //Act
            var commandHandler = new RequestPermissionCommandHandler(_elasticClientMock.Object, _repositoryMock.Object);
            var result = await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Permission>()), Times.Once);
        
            Assert.Equal(item.EmployeeForename, result.EmployeeForename);
            Assert.Equal(item.EmployeeSurname, result.EmployeeSurname);
        }

        [Fact]
        public async Task Handle_exists_name_return_domain_exception()
        {
            //Arrange
            var command = new RequestPermissionCommand(1, "Juan", "Abanto");
            var lst = new List<Permission>() { new Permission(1, "Juan", "Abanto") };

            _repositoryMock.Setup(x => x.FindPermissionAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
                .ReturnsAsync(lst);

            //Act
            var commandHandler = new RequestPermissionCommandHandler(_elasticClientMock.Object, _repositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<SecurityDomainException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
    }
}