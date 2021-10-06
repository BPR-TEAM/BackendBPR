using System.Collections.Generic;

namespace BackendBPR.Database
{
    public class Plant
    {
        public int Id { get; set; }
        public string CommonName { get; set; } 
        public string ScientificName { get; set; } 
        public byte[] Image { get; set;}
        public string Url { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserPlant> UserPlants { get; set; }       
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<MeasurementDefinition> MeasurementDefinitions {get;set;}        
    }
}