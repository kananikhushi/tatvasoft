using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Entities;
using Test.Entities.Model;
using Test.Repo;

namespace Test.Services
{
    public class MissionSkillService
    {
        private readonly MissionSkillRepo _missionSkillRepo;

        public MissionSkillService(MissionSkillRepo missionSkillRepo)
        {
            _missionSkillRepo = missionSkillRepo;
        }

        public Task<bool> AddMissionSkill(MissionSkillModel model)
        {
            MissionSkill missionSkill = new MissionSkill()
            {
                Id = model.Id,
                SkillName = model.SkillName,
                Status = model.Status, 
                
                IsDeleted = false
            };

            return _missionSkillRepo.AddMissionSkill(missionSkill);
        }

        public Task<List<MissionSkillModel>> GetAllMissionSkills()
        {
            return _missionSkillRepo.GetAllMissionSkill();
        }

        public MissionSkill? GetSkillById(int id)
        {
            return _missionSkillRepo.GetById(id);
        }

        public string DeleteSkill(int id)
        {
            return _missionSkillRepo.DeleteSkill(id);
        }

        public async Task<MissionSkill> UpdateSkill(MissionSkill model)
        {
            var isExist = _missionSkillRepo.GetByNameAndNotDeleted(model.SkillName, model.Id);

            if (isExist != null)
                throw new Exception("Skill already exists");

            var existingSkill = _missionSkillRepo.GetById(model.Id)
                ?? throw new Exception("Skill does not exist");

            existingSkill.ModifiedDate = DateTime.UtcNow;
            existingSkill.SkillName = model.SkillName;
            existingSkill.Status = model.Status;

            _missionSkillRepo.Update(existingSkill);
            await _missionSkillRepo.SaveChangesAsync();

            return existingSkill;
        }

    }
}
