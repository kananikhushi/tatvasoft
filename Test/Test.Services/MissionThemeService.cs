using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Entities.Model;
using Test.Entities;
using Test.Services;
using Test.Repo;


namespace Test.Services
{
    public  class MissionThemeService(MissionThemeRepo missionThemeRepo)
    {
        private readonly MissionThemeRepo _missionThemeRepo = missionThemeRepo;
        public Task<bool> AddMissionTheme(MissionThemeModel model)
        {
            MissionTheme missionTheme = new MissionTheme()
            {
                Id = model.Id,
                Status = model.Status,
                ThemeName = model.ThemeName,
            };
            return  _missionThemeRepo.AddMissionTheme(missionTheme);
        }
        public Task<List<MissionThemeModel>> GetAllMissionThemes()
        {
            return _missionThemeRepo.GetAllMissionTheme();
        }

        public async Task<MissionThemeModel> GetMissionThemeById(int id)
        {
            return await _missionThemeRepo.GetMissionThemeById(id);
        }

        public async Task<bool> UpdateMissionTheme(MissionThemeModel model)
        {
            // Check if the record exists
            var existing = await _missionThemeRepo.GetMissionThemeById(model.Id);

            if (existing == null)
            {
                // Theme not found OR soft-deleted
                return false;
            }

            // Proceed to update
            MissionTheme missionTheme = new MissionTheme
            {
                Id = model.Id,
                ThemeName = model.ThemeName,
                Status = model.Status,
                ModifiedDate = DateTime.UtcNow
            };

            return await _missionThemeRepo.UpdateMissionTheme(missionTheme);
        }
        public Task<bool> DeleteMissionTheme(int id)
        {
            return _missionThemeRepo.DeleteMissionTheme(id);
        }


    }
}
