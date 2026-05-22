using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManager.Application.Features.Admin.Commands.UpdateAccountRole;
using UserManager.Application.Features.Admin.Commands.UpdateAccountStatus;
using UserManager.Application.Features.Admin.Queries.FilterUsers;
using UserManager.Application.Features.Admin.Queries.GetAllRoles;
using UserManager.Application.Features.Admin.Queries.GetAllUsers;
using UserManager.Application.Features.Admin.Queries.SearchUsers;
using UserManager.Domain.Constants;
using UserManager.Domain.Entities;

namespace UserManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public AdminController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var currentUserId = GetCurrentUserId();
            var query = new GetAllUsersQuery(currentUserId);
            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var query = new GetAllRolesQuery();
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("users/filter")]
        public async Task<IActionResult> FilterUsers(
        [FromQuery] string? roleId,
        [FromQuery] string? status,
        [FromQuery] string? gender)
        {
            var currentUserId = GetCurrentUserId();
            var query = new FilterUsersQuery(currentUserId, roleId, status, gender);
            var result = await _sender.Send(query, HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpGet("users/search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm)
        {
            var currentUserId = GetCurrentUserId();
            var query = new SearchUsersQuery(currentUserId, searchTerm);
            var result = await _sender.Send(query, HttpContext.RequestAborted);
            return Ok(result);
        }

        [HttpPut("users/{userId}/status")]
        public async Task<IActionResult> UpdateAccountStatus([FromBody] UpdateAccountStatusCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("users/{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateAccountRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? throw new UnauthorizedAccessException("User ID is missing from the token.");
        }
    }
}
