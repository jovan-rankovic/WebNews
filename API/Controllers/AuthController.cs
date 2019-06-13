using API.Helpers;
using Application;
using Application.Commands.User;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginCommand _loginCommand;
        private readonly Encryption _enc;

        public AuthController(ILoginCommand loginCommand, Encryption enc)
        {
            _loginCommand = loginCommand;
            _enc = enc;
        }

        // POST: api/Auth
        [HttpPost]
        public IActionResult Post([FromForm] string email, [FromForm] string password)
        {
            try
            {
                var userDto = _loginCommand.Execute((email, password));
                var user = new LoggedUser
                {
                    Id = userDto.Id,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Role = userDto.Role
                };

                var stringObject = JsonConvert.SerializeObject(user);
                var encrypted = _enc.EncryptString(stringObject);
                return Ok(new { token = encrypted });
            }
            catch (LoginFailedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("decode")]
        public IActionResult Decode(string value)
        {
            try
            {
                var decodedString = _enc.DecryptString(value);
                decodedString = decodedString.Substring(0, decodedString.LastIndexOf("}") + 1);
                var user = JsonConvert.DeserializeObject<LoggedUser>(decodedString);
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}