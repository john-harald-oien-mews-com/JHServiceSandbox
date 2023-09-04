using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TripsafeScimService.Controllers
{
    [ApiController]
    [Route("scim/v2")]
    public class ScimController : ControllerBase
    {
        [HttpGet("Users/{userId}")]
        [Authorize]
        public IActionResult GetUser(string userId)
        {
            // TODO: Retrieve user data from database or external source
            // and return it as a SCIM User resource
            return Ok(new
            {
                id = userId,
                userName = "john.doe@example.com",
                active = true,
                name = new
                {
                    givenName = "John",
                    familyName = "Doe"
                },
                emails = new[]
                {
                    new { value = "john.doe@example.com", primary = true }
                }
            });
        }

        [HttpPost("Users")]
        [Authorize]
        public IActionResult CreateUser([FromBody] object user)
        {
            // TODO: Create new user in database or external source
            // and return the newly created user as a SCIM User resource
            return CreatedAtAction(nameof(GetUser), new { userId = "123" }, new
            {
                id = "123",
                userName = "jane.doe@example.com",
                active = true,
                name = new
                {
                    givenName = "Jane",
                    familyName = "Doe"
                },
                emails = new[]
                {
                    new { value = "jane.doe@example.com", primary = true }
                }
            });
        }

        [HttpPut("Users/{userId}")]
        [Authorize]
        public IActionResult UpdateUser(string userId, [FromBody] object user)
        {
            // TODO: Update existing user in database or external source
            // and return the updated user as a SCIM User resource
            return Ok(new
            {
                id = userId,
                userName = "jane.doe@example.com",
                active = true,
                name = new
                {
                    givenName = "Jane",
                    familyName = "Doe"
                },
                emails = new[]
                {
                    new { value = "jane.doe@example.com", primary = true }
                }
            });
        }

        [HttpDelete("Users/{userId}")]
        [Authorize]
        public IActionResult DeleteUser(string userId)
        {
            // TODO: Delete user from database or external source
            // and return 204 No Content status code
            return NoContent();
        }
    }
}
