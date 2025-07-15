using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Test.Entities.DTOs;
using Test.Repo;
using Test.Services;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly AdminUserRepository _adminUserRepository;
        private readonly UserService _service;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminUserController(AdminUserRepository adminUserRepository, UserService service, IWebHostEnvironment hostEnvironment)
        {
            _adminUserRepository = adminUserRepository;
            _service = service;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("UserDetailList")]
        public ActionResult UserDetailsList()
        {
            try
            {
                var res = _adminUserRepository.UserDetailsList();
                return Ok(new { Data = res, Status = "Success", Message = "" });
            }
            catch
            {
                return BadRequest(new { Data = (object)null, Status = "Error", Message = "Failed to get User" });
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var res = _adminUserRepository.DeleteUser(id);
                return Ok(new { Data = res, Status = "Success", Message = "" });
            }
            catch
            {
                return BadRequest(new { Data = (object)null, Status = "Error", Message = "Failed to Delete User" });
            }
        }

        

    }
}
