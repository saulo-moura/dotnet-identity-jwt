using DotnetJwtAuth.Entities;
using DotnetJwtAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetJwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticate _auth;

        public AuthController(IAuthenticate auth)
        {
            _auth = auth;
        }

        public IActionResult Post([FromBody]ApplicationUser authParam)
        {
            var authorizedUser = _auth.Authenticate(authParam.UserName, authParam.PasswordHash);

            if (authorizedUser == null) 
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(authorizedUser);
        }
    }
}