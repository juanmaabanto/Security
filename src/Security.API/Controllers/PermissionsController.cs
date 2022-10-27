using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Challenge.Services.Security.API.Application.Queries;
using N5.Challenge.Services.Security.Domain.Entities;
using N5.Challenge.Services.Security.API.Responses;
using N5.Challenge.Services.Security.API.Application.Commands;

namespace N5.Challenge.Services.Security.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator? mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get a list of permissions by filter.
        /// </summary>
        /// <param name="employeeForename">Filter by forename.</param>
        /// <param name="employeeSurname">Filter by surname.</param>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<Permission>>> GetList(string employeeForename, string employeeSurname, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPermissionsQuery(employeeForename, employeeSurname), cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns the permission by ID.
        /// </summary>
        /// <param name="permissionId">Id of the permission.</param>
        [HttpGet]
        [Route("{permissionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Permission>> Get(string permissionId, CancellationToken cancellationToken)
            => await _mediator.Send(new GetPermissionByIdQuery(permissionId), cancellationToken);

        /// <summary>
        /// Create a new permission.
        /// </summary>
        /// <param name="command">Object to be created.</param>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<Permission>> Post([FromBody]RequestPermissionCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Get), new { permissionId = result.Id}, result);
        }

        /// <summary>
        /// Modify a permission.
        /// </summary>
        /// <param name="command">Object to be modified.</param>
        [Route("")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult> Put([FromBody] ModifyPermissionCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

    }
}
