using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Features.Users.Commands.CreateUser;
using UserManager.Application.Features.Users.Queries.GetAllUsers;

namespace UserManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;

        public UsersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserCommand command,
            CancellationToken cancellationToken) 
        {
            // MediatR sẽ truyền Token này vào Pipeline Behavior và Handler
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateUser), new { id = result }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var query = new GetAllUsersQuery();

            var users = await _mediator.Send(query, cancellationToken);

            return Ok(users);
        }
    }
}
