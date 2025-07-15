using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Entities;
using Test.Entities.Model;

namespace Test.Repo
{
    public class MissionSkillRepo(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<bool> AddMissionSkill(MissionSkill missionSkill)
        {
            bool alreadyExist = await _appDbContext.MissionTheme.AnyAsync(m => m.ThemeName.ToLower() == missionSkill.SkillName.ToLower() && !m.IsDeleted);
            if (alreadyExist)
            {
                return false;
            }
            object value = await _appDbContext.MissionSkill.AddAsync(missionSkill);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        public Task<List<MissionSkillModel>> GetAllMissionSkill()
        {
            return _appDbContext.MissionSkill.Where(m => !m.IsDeleted).Select(m => new MissionSkillModel()
            {
                Id = m.Id,
                SkillName = m.SkillName,
                Status = m.Status,
            }).ToListAsync();
        }
        public string DeleteSkill(int id)
        {
            var mission = _appDbContext.MissionSkill.Where(x => x.Id == id).FirstOrDefault();
            if (mission == null) throw new Exception("Skill not Exist");
            mission.IsDeleted = true;
            mission.ModifiedDate = DateTime.UtcNow;
            _appDbContext.MissionSkill.Update(mission);
            _appDbContext.SaveChanges();
            return "Skill Deleted";
        }
        public MissionSkill? GetByNameAndNotDeleted(string name, int excludeId)
        {
            return _appDbContext.MissionSkill
                .FirstOrDefault(x => x.SkillName == name && x.Id != excludeId && !x.IsDeleted);
        }

        public MissionSkill? GetById(int id)
        {
            return _appDbContext.MissionSkill
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        }


        public void Update(MissionSkill skill)
        {
            _appDbContext.MissionSkill.Update(skill);
        }
        public Task<int> SaveChangesAsync()
        {
            return _appDbContext.SaveChangesAsync();
        }
    }

}