using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using Test.Entities;
using Test.Entities.DTOs;
using DtoUserDetails = Test.Entities.DTOs.UserDetails;
using Test.Services;

namespace Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase //handles http req
    {
        private readonly UserService _service;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostEnvironment;
        public LoginController(UserService service, IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _service = service;
            _config = config;
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new MessageRes
                {
                    Success = false,
                    Message = "Model validation failed: " + errors
                });
            }

            var result = _service.Register(user);

            // You can still return 200 with your custom response object
            return Ok(result);
        }




        [HttpPost("LoginUser")]
        public MessageRes Login([FromBody] LoginDto login)
        {
            var result = _service.Login(login.EmailAddress, login.Password, _config);
            return result;
        }
        [Authorize(Roles = "admin,user")]
        [HttpGet("LoginUserDetailById/{id}")]
        public IActionResult LoginUserDetailById(int id)
        {
            var user = _service.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Status = "Error", Message = "User not found" });
            }

            return Ok(new { Status = "Success", Data = user });
        }
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(Entities.UserDetails model)
        {
            try
            {
                var updatedUser = await _service.UpdateUser(model, _hostEnvironment.WebRootPath);

                if (updatedUser == null)
                {
                    return NotFound(new { Status = "Error", Message = "User update failed or user not found" });
                }

                return Ok(new { Status = "Success", Data = updatedUser });
            }
            catch (Exception ex)
            {
                // Optionally log the exception: _logger.LogError(ex, "Error updating user");

                return StatusCode(500, new { Status = "Error", Message = "An error occurred while updating the user." });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost("add-book")]
        public IActionResult AddBook()
        {
            // Admins only
            return Ok("Book added.");
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("view-books")]
        public IActionResult ViewBooks()
        {
            // Students and Admins
            return Ok("List of Books");
        }

    
    }
}