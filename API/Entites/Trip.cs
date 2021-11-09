using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entites
{
    public class Trip
    {
       public int Id { get; set; }
       [ForeignKey("Users"), Column(Order = 0)] 
       public int CreatorId { get; set; }
       
       public decimal Price { get; set; }
       public DateTime StartTime { get; set; }
       
       public string StartFrom { get; set; }
       
       public string EndIn { get; set; }

       
        
    }
}