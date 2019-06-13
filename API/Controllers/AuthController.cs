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

        /// <summary>
        /// Encrypts and returns user token for logging in
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST api/Auth
        ///     {
        ///        "email": "test@gmail.com",
        ///        "password": "passw0rd"
        ///     }
        ///     
        /// </remarks>
        /// <response code="401">If credentials are not valid</response>
        /// <response code="500">If another exception happens</response>
        [HttpPost]
        public ActionResult<string> Post([FromForm] string email, [FromForm] string password)
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

        /// <summary>
        /// Decrypts and returns user token
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     GET api/Decode
        ///     {
        ///        "value": "hKEw7tMD6BirA8uWNQkqEzUCo5m7x+ru1Q/A8jBlaavIixeT5n3Zvsb7l4IxIrW1uGsiTYHVJMUBJ/c5OvT5mAu4pvCLbD7YfZqUnWiknZv3Hbb423V2PgDkd6mW2gHDYsE8sX6hqFOJxws5YtKi+T4Pe7cklz8uCvFATSubkmo="
        ///     }
        ///     
        /// </remarks>
        /// <response code="500">If exception happens</response>
        [HttpGet("Decode")]
        public ActionResult<LoggedUser> Decode(string value)
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