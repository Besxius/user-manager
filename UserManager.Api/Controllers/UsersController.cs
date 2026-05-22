using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManager.Application.Features.Users.Commands.CreateProfile;
using UserManager.Application.Features.Users.Commands.UpdateProfile;
using UserManager.Application.Features.Users.Queries.GetMyProfile;
using UserManager.Domain.Constants;
using UserManager.Domain.Entities;

namespace UserManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.User)]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;

        public UsersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserById(CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            var query = new GetMyProfileQuery(currentUserId!);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();

            var command = new CreateProfileCommand(
                currentUserId,
                request.FullName,
                request.DateOfBirth,
                request.Gender,
                request.Address);

            var result = await _mediator.Send(command, cancellationToken);
            return Created($"api/users/profile", new { ProfileId = result });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();

            var command = new UpdateProfileCommand(
                currentUserId,
                request.FullName,
                request.DateOfBirth,
                request.Gender,
                request.Address);

            var result = await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("UserId is not found");
        }
    }
}
