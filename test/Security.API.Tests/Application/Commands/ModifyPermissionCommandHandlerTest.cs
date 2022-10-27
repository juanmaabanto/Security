using System.Linq.Expressions;
using MediatR;
using Moq;
using N5.Challenge.Services.Security.API.Application.Commands;
using N5.Challenge.Services.Security.API.Infrastructure.Exceptions;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.Domain.Exceptions;
using N5.Challenge.Services.Security.Domain.Repositories;
using N5.Challenge.Services.Security.Domain.SeedWork;
using Nest;
using static N5.Challenge.Services.Security.API.Application.Commands.ModifyPermissionCommand;

namespace N5.Challenge.Services.Security.API.Tests.Application.Commands
{
    public class ModifyPermissionCommandHandlerTest
    {
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<IPermissionRepository>  _repositoryMock;

        public ModifyPermissionCommandHandlerTest()
        {
            _elasticClientMock = new Mock<IElasticClient>();
            _repositoryMock = new Mock<IPermissionRepository>();
            var ctxMock = new Mock<IUnitOfWork>();

            _repositoryMock.SetupGet(x => x.UnitOfWork).Returns(ctxMock.Object);
        }

        [Fact]
        public void Builder_receivenull_elastic_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new ModifyPermissionCommandHandler(null, _repositoryMock.Object));
        }

        [Fact]
        public void Builder_receivenull_repository_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new ModifyPermissionCommandHandler(_elasticClientMock.Object, null));
        }

        [Fact]
        public async Task Handle_update_item_and_return_unit()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, 1, "test modify", "test modify");
            var item = new Permission(1, "test", "test");

            _repositoryMock.Setup(x => x.FindByIdAsync(command.permissionId))
                .ReturnsAsync(item);
            _repositoryMock.Setup(x => x.FindPermissionAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
                .ReturnsAsync(new List<Permission>());

            //Act
            var commandHandler = new ModifyPermissionCommandHandler(_elasticClientMock.Object, _repositoryMock.Object);
            var result = await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(command.employeeForename, item.EmployeeForename);
            Assert.Equal(command.employeeSurname, item.EmployeeSurname);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_throw_permission_not_found_exception()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, 1, "test modify", "test modify");

            _repositoryMock.Setup(x => x.FindByIdAsync(command.permissionId))
                .ReturnsAsync((Permission?) null);

            //Act
            var commandHandler = new ModifyPermissionCommandHandler(_elasticClientMock.Object, _repositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<PermissionNotFoundException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
        
        [Fact]
        public async Task Handle_if_exist_throw_domain_exception()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, 1, "test modify", "test modify");
            var item = new Permission(1, "test", "test");

            _repositoryMock.Setup(x => x.FindPermissionAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
            .ReturnsAsync(new List<Permission>() { item });

            //Act
            var commandHandler = new ModifyPermissionCommandHandler(_elasticClientMock.Object, _repositoryMock.Object);

            //Assert
            await Assert.ThrowsAsync<SecurityDomainException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
    }
}