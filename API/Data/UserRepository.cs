using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entites;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersByDataAsync(string data)
        {
            return await _context.Users
            .Where(u => u.Name.Contains(data) || data.Contains(u.Name) || data.Equals(u.UserHash)).ToListAsync();
        }
        public async Task<MemberDto> GetUserByHash(int hash)
        {
            var user = await _context.Users.Where(u => u.ID == hash).FirstOrDefaultAsync();
            MemberDto memberDto = new MemberDto();
            if(user != null)
            {   List<Opinion> opinions = new List<Opinion>();
                var opinionsList = _context.Opinions.Where(o => o.UserId == user.ID).ToList();
                foreach(var o in opinionsList)
                {
                    opinions.Add(o);
                }
                memberDto = new MemberDto()
                {
                    ID = user.ID,
                    UserHash = user.UserHash,
                    Opinions = opinions,
                    Email = user.Email,
                    Name = user.Name,
                    Gender = user.Gender,
                    Description = user.Description,
                    Avatar = user.Avatar
                };
                
            }
            return memberDto;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}