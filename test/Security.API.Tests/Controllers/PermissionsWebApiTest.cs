using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using N5.Challenge.Services.Security.API.Application.Commands;
using N5.Challenge.Services.Security.API.Application.Queries;
using N5.Challenge.Services.Security.API.Controllers;
using N5.Challenge.Services.Security.Domain.Entities;

namespace N5.Challenge.Services.Security.API.Tests.Controllers
{
    public class PermissionsWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;

        public PermissionsWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Constructor_receivenull_mediator_throws()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new PermissionsController(null));
        }

        [Fact]
        public async Task GetList_return_result()
        {
            //Arrange
            var query = new GetPermissionsQuery("test", "test");
            IEnumerable<Permission> expectedResult = GetFakePermissionsList();

            _mediatorMock.Setup(x => x.Send(query, CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var permissionsController = new PermissionsController(_mediatorMock.Object);

            //Act
            var result = await permissionsController.GetList("test", "test");

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Permission>>>(result);
        }

        [Fact]
        public async Task Post_return_new_permission()
        {
            //Arrange
            var command = new RequestPermissionCommand(1, "test", "test");
            var expectedResult = new Permission(1, "test", "test");

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var permissionsController = new PermissionsController(_mediatorMock.Object);

            //Act
            var result = await permissionsController.Post(command, CancellationToken.None);

            //Assert
            var actionResult = Assert.IsType<ActionResult<Permission>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Permission>(createdAtActionResult.Value);

            Assert.Equal(expectedResult.EmployeeForename, returnValue.EmployeeForename);
        }

        [Fact]
        public async Task Put_return_no_content()
        {
            //Arrange
            var command = new ModifyPermissionCommand(1, 1, "test", "test");

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var permissionsController = new PermissionsController(_mediatorMock.Object);

            //Act
            var result = await permissionsController.Put(command, CancellationToken.None);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        private IEnumerable<Permission> GetFakePermissionsList()
        {
            var permissions = new List<Permission>();

            permissions.Add(new Permission(1, "test", "test"));
            permissions.Add(new Permission(2, "test", "test"));

            return permissions;
        }
    }
}