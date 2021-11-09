using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entites;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<AppUser>> GetUsersByDataAsync(string name);
        Task<AppUser> GetUserByHash(int hash);
       
    }
}