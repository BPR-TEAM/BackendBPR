using System.Collections.Generic;

namespace BackendBPR.Database
{
    public class UserPlant
    {
        public int Id {get;set;}
        public int UserId { get; set; }
         public int PlantId { get; set; }
        public string Name { get; set; } 
        public byte[] Image { get; set;}

        public virtual User User { get; set; }       
        public virtual Plant Plant { get; set; }
        
        public virtual ICollection<Measurement> Measurements {get;set;}
    }
}