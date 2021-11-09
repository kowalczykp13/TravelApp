using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TripInfoDto
    {
       public MemberDto Creator { get; set; }
       public List<MemberDto> Passenger { get; set; } = new List<MemberDto>();
       
       public decimal Price { get; set; }
       public DateTime StartTime { get; set; }
       
       public string StartFrom { get; set; }
       
       public string EndIn { get; set; }
    }
}