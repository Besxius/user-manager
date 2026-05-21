using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Features.Auth.Commands.Login;
using UserManager.Application.Features.Auth.Commands.RegisterUser;

namespace UserManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCommand command, 
            CancellationToken cancellationToken)
        {
            var userId = await _sender.Send(command, cancellationToken);

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var token = await _sender.Send(command, cancellationToken);
            return Ok(token);
        }
    }
}
