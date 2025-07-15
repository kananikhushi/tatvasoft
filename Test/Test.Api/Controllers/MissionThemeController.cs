using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Entities.Model;
using Test.Services;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionThemeController : ControllerBase
    {
        private readonly MissionThemeService _missionThemeService;

        public MissionThemeController(MissionThemeService missionThemeService)
        {
            _missionThemeService = missionThemeService;
        }

        [HttpPost]
        [Route("AddMissionTheme")]
        public async Task<IActionResult> AddMissionTheme(MissionThemeModel model)
        {
            try
            {
                var res = await _missionThemeService.AddMissionTheme(model);
                if (res == true)
                {
                    return Ok(new { Status = "Success", Data = "Add Mission Theme", Message = "" });
                }
                else
                {
                    return Ok(new { Status = "Error", Data = "", Message = "Data Already Exist" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Can't Add", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetMissionThemeList")]
        public async Task<IActionResult> GetAllMissionTheme()
        {
            try
            {
                var res = await _missionThemeService.GetAllMissionThemes();
                return Ok(new { Status = "Success", Data = res, Message = "" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to get Theme", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetMissionThemeById/{id}")]
        public async Task<IActionResult> GetMissionThemeById(int id)
        {
            var res = await _missionThemeService.GetMissionThemeById(id);

            if (res == null)
                return NotFound("Mission theme not found");

            return Ok(new { Status = "Success", Data = res });
        }

        [HttpPost]
        [Route("UpdateMissionTheme")]
        public async Task<IActionResult> UpdateMissionTheme(MissionThemeModel model)
        {
            var res = await _missionThemeService.UpdateMissionTheme(model);

            if (res)
            {
                return Ok(new
                {
                    Status = "Success",
                    Data = model,
                    Message = "Theme updated successfully"
                });
            }
            else
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Theme not found or already deleted"
                });
            }
        }

        [HttpDelete]
        [Route("DeleteMissionTheme/{id}")]
        public async Task<IActionResult> DeleteMissionTheme(int id)
        {
            var result = await _missionThemeService.DeleteMissionTheme(id);
            if (result)
                return Ok(new { Status = "Success", Message = "Theme deleted" });
            else
                return NotFound(new { Status = "Error", Message = "Theme not found or already deleted" });
        }
    }
}
