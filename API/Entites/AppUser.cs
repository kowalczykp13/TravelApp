using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Entites
{
    public class AppUser
    {
        public int ID { get; set; }
        public int UserHash { get; set; }
        
        
        public string Email { get; set; }
        
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }


        
        
        
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
        
    }
}