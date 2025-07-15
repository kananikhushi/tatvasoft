using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Entities;
using Test.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Test.Repo
{
    public  class MissionThemeRepo(AppDbContext appDbContext )
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<bool>AddMissionTheme(MissionTheme missionTheme)
        {
            bool alreadyExist = await _appDbContext.MissionTheme.AnyAsync(m => m.ThemeName.ToLower() == missionTheme.ThemeName.ToLower() && !m.IsDeleted);
            if (alreadyExist) { 
            return false;
            }
            object value = await _appDbContext.MissionTheme.AddAsync(missionTheme);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        public Task<List<MissionThemeModel>> GetAllMissionTheme()
        {
            return _appDbContext.MissionTheme.Where(m => !m.IsDeleted).Select(m => new MissionThemeModel()
            {
                Id = m.Id,
                ThemeName = m.ThemeName,
                Status = m.Status,
            }).ToListAsync();
        }
        public Task<MissionThemeModel> GetMissionThemeById(int id)
        {
            return _appDbContext.MissionTheme
                .Where(m => m.Id == id && !m.IsDeleted)
                .Select(m => new MissionThemeModel()
                {
                    Id = m.Id,
                    ThemeName = m.ThemeName,
                    Status = m.Status,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMissionTheme(MissionTheme missionTheme)
        {
            var missionThemeExist = await _appDbContext.MissionTheme
                .FirstOrDefaultAsync(m => m.Id == missionTheme.Id && !m.IsDeleted);

            if (missionThemeExist == null)
            {
                return false;
            }

            missionThemeExist.ThemeName = missionTheme.ThemeName;
            missionThemeExist.Status = missionTheme.Status;
            missionThemeExist.ModifiedDate = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteMissionTheme(int id)
        {
            var missionTheme = await _appDbContext.MissionTheme
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (missionTheme == null)
                return false;

            missionTheme.IsDeleted = true;
            missionTheme.ModifiedDate = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            return true;
        }



    }
}
