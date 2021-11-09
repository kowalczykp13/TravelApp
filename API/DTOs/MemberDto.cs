using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entites;

namespace API.DTOs
{
    public class MemberDto
    {
        public int ID { get; set; }
        public int UserHash { get; set; }
        
        
        public string Email { get; set; }
        
        public string Name { get; set; }

        public string Gender { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        
    }
}