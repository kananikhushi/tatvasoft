using Microsoft.AspNetCore.Mvc;
using Test.Entities;
using Test.Entities.Model;
using Test.Services;

namespace Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionSkillController : ControllerBase
    {
        private readonly MissionSkillService _missionSkillService;

        public MissionSkillController(MissionSkillService missionSkillService)
        {
            _missionSkillService = missionSkillService;
        }

        [HttpPost("AddMissionSkill")]
        public async Task<IActionResult> AddMissionSkill(MissionSkillModel model)
        {
            try
            {
                var res = await _missionSkillService.AddMissionSkill(model);
                if (res)
                {
                    return Ok(new { Status = "Success", Data = "Add Mission Skill", Message = "" });
                }
                else
                {
                    return Ok(new { Status = "Error", Data = "", Message = "Data Already Exist" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Can't Add",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("GetMissionSkillList")]
        public async Task<IActionResult> GetAllMissionSkill()
        {
            try
            {
                var res = await _missionSkillService.GetAllMissionSkills();
                return Ok(new { Status = "Success", Data = res, Message = "" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to get Skill", Error = ex.Message });
            }
        }

        [HttpGet("GetMissionSkillById/{id}")]
        public IActionResult GetMissionSkillById(int id)
        {
            var skill = _missionSkillService.GetSkillById(id);
            if (skill == null)
            {
                return NotFound(new { Status = "Error", Message = "Skill not found" });
            }

            return Ok(new { Status = "Success", Data = skill });
        }

        [HttpDelete("DeleteMissionSkill/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var res = _missionSkillService.DeleteSkill(id);
                return Ok(new
                {
                    Data = res,
                    Status = "Success",
                    Message = "Skill Deleted"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Data = (object)null,
                    Status = "Error",
                    Message = "Failed to Delete Skill",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("UpdateMissionSkill")]
        public async Task<IActionResult> UpdateSkill(MissionSkill model)
        {
            try
            {
                var updatedSkill = await _missionSkillService.UpdateSkill(model);

                return Ok(new
                {
                    Status = "Success",
                    Data = updatedSkill,
                    Message = "Skill updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }
        }

    }
}
