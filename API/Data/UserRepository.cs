using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<AppUser> GetUserByHash(int hash)
        {
            return await _context.Users.Where(u => u.ID == hash).FirstOrDefaultAsync();;
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